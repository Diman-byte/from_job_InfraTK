using Cassandra.Data;
using Cassandra;
using System.Data.Common;
using System.Text;
using System.Xml.Linq;
using Common.MsgLog;
using static Cassandra.QueryTrace;
using System;
using Common;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using Cassandra.Mapping;
using static System.Collections.Specialized.BitVector32;

namespace HistoryDB
{
    public class CassandraHistory
    {
        public const string TimeFormat = "yyyy-MM-dd HH:mm:ss+0000";
        private Cluster _cluster;
        private ISession _session;
        private string _connectionString;

        /// <summary>
        /// Конструктор
        /// </summary>
        public CassandraHistory()
        {

        }

        /// <summary>
        /// Функция соединения с БД
        /// </summary>
        public bool TryConnect(HistoryDataBaseInfo serverConnectInfo, out MsgLogClass msgLog)
        {            
            try
            {
                Connect(serverConnectInfo);
            }
            catch (Exception exception)
            {                
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory),  LogText = $"Не удалось подключиться к Cassandra", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }

            msgLog = null;
            return true;
        }

        /// <summary>
        /// Функция разрыва соединения с БД
        /// </summary>
        /// <returns></returns>
        public bool TryDisсonnect()
        {           
            try
            {
                _session.ShutdownAsync();
            }
            catch (Exception exception)
            {
               return false;
                //msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Не удалось разорвать соединение с Cassandra", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };              
            }
            
            return true;
        }

        /// <summary>
        /// Выбрать первое значение
        /// </summary>
        /// <param name="database"></param>
        /// <param name="schema"></param>
        /// <param name="msgLog"></param>
        /// <returns></returns>
        public DateTime TrySelectFirstDateTime(string database, string schema, out MsgLogClass msgLog)
        {
            var result = new DateTime();
            try
            {
                int assetId = int.Parse(schema.Split('_')[1]);  //id актива

                var cql = $"SELECT MIN(\"DateTime\") from \"{database}\".\"NodeSlicesDT_{assetId}\";";

                var dateTime = (DateTimeOffset?)_session.Execute(cql).First().First();
                if (dateTime.HasValue)
                    result = dateTime.Value.DateTime;

            }
            catch (Exception exception)
            {                
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TrySelectFirstDateTime)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
            }

            msgLog = null;
            return result;
        }

        /// <summary>
        /// Выбрать последнее значение
        /// </summary>
        /// <param name="database"></param>
        /// <param name="schema"></param>
        /// <param name="msgLog"></param>
        /// <returns></returns>
        public DateTime TrySelectLastDateTime(string database, string schema, out MsgLogClass msgLog)
        {
            var result = new DateTime();
            try
            {
                int assetId = int.Parse(schema.Split('_')[1]);  //id актива

                var cql = $"SELECT MAX(\"DateTime\") from \"{database}\".\"NodeSlicesDT_{assetId}\";";

                var dateTime = (DateTimeOffset?)_session.Execute(cql).First().First();
                if (dateTime.HasValue)
                    result = dateTime.Value.DateTime;

            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TrySelectLastDateTime)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
            }

            msgLog = null;
            return result;
        }

        /// <summary>
        /// Очистка таблиц узла
        /// </summary>
        /// <param name="database"></param>
        /// <param name="schema"></param>
        /// <param name="tableName"></param>
        /// <param name="msgLog"></param>
        /// <returns></returns>
        public bool TryTruncateNodeTables(string database, int nodeId, out MsgLogClass msgLog)
        {            
            try
            {
                _session.Execute($"TRUNCATE \"{database}\".\"NodeSlicesDT_{nodeId}\"");
                _session.Execute($"TRUNCATE \"{database}\".\"NodeEvents_{nodeId}\"");
                _session.Execute($"TRUNCATE \"{database}\".\"NodeData_{nodeId}\"");

                /*string cql = string.Empty;

                switch (tableName)
                {
                    case "SlicesDT":
                        cql = $"TRUNCATE \"{database}\".\"NodeSlicesDT_{nodeId}\"";
                        break;

                    case "NodeEvents":
                        cql = $"TRUNCATE \"{database}\".\"NodeEvents_{nodeId}\"";
                        break;

                    case "NodeData":
                        cql = $"TRUNCATE \"{database}\".\"NodeData_{nodeId}\"";
                        break;
                }

                if (!string.IsNullOrEmpty(cql))
                    _session.Execute(cql);*/
            }
            catch (Exception exception)
            {                
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TryTruncateNodeTables)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }

            msgLog = null;
            return true;
        }

        /// <summary>
        /// Удаление таблиц узла
        /// </summary>
        /// <param name="database"></param>
        /// <param name="nodeId"></param>
        /// <param name="msgLog"></param>
        /// <returns></returns>
        public bool TryDropNodeTables(string database, int nodeId, out MsgLogClass msgLog)
        {            
            try
            {                

                var cql1 = $"DROP TABLE IF EXISTS \"{database}\".\"NodeData_{nodeId}\"";
                _session.Execute(cql1);

                var cql2 = $"DROP TABLE IF EXISTS \"{database}\".\"NodeSlicesDT_{nodeId}\"";
                _session.Execute(cql2);

                var cql3 = $"DROP TABLE IF EXISTS \"{database}\".\"NodeEvents_{nodeId}\"";
                _session.Execute(cql3);
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TryDropNodeTables)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }

            msgLog = null;
            return true;
        }

        /// <summary>
        /// Чтение срезов исторических данных
        /// </summary>
        /// <param name="database"></param>
        /// <param name="schema"></param>
        /// <param name="dateTimeFrom"></param>
        /// <param name="dateTimeTo"></param>
        /// <param name="idTags"></param>
        /// <param name="pointsAmount"></param>
        /// <param name="result"></param>
        /// <param name="algSlice"></param>
        /// <param name="msgLog"></param>
        /// <returns></returns>
        public bool TryReadSliceHistData(string database,
            int nodeId, DateTime dateTimeFrom, DateTime dateTimeTo, List<int> idTags, int pointsAmount, AlgSliceEnum algSlice,
            out Dictionary<int, List<DataVal>> result, out MsgLogClass msgLog)
        {
            result = new Dictionary<int, List<DataVal>>(0);            
            var connection = new CqlConnection(_connectionString);

            DbDataReader GetTagReaderMethod(int tagId, string sortOrder)
            {
                var cql = $"SELECT  \"DateTime\", \"Val\" FROM \"{database}\".\"NodeData_{nodeId}\" WHERE \"TagId\"={tagId} AND " +
                         $"\"DateTime\" >= \'{dateTimeFrom.ToString(TimeFormat)}\' AND \"DateTime\" < \'{dateTimeTo.ToString(TimeFormat)}\' ORDER BY \"DateTime\" {sortOrder}";
                var command = connection.CreateCommand();
                command.CommandText = cql;
                return command.ExecuteReader();
            }

            try
            {
                if (dateTimeTo <= dateTimeFrom)
                {
                    msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Интервал времени задан неверно", TypeLog = TypeMsg.Err };                    
                    return false;
                }
                connection.Open();
                var sliceReader = new DataBaseCommon(GetTagReaderMethod);
                result = sliceReader.ReadSliceTagData(nodeId, dateTimeFrom, dateTimeTo, idTags, pointsAmount, algSlice);
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TryReadSliceHistData)}. ", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }
            finally
            {
                connection.Close();
            }

            msgLog = null;
            return true;
        }

        /// <summary>
        /// Чтение исторических данных
        /// </summary>
        /// <param name="database"></param>
        /// <param name="schema"></param>
        /// <param name="beginDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <param name="idTags"></param>
        /// <param name="result"></param>
        /// <param name="msgLog"></param>
        /// <returns></returns>
        public bool TryReadHistData(string database,
            int nodeId, DateTime beginDateTime, DateTime endDateTime, List<int> idTags, out Dictionary<int, List<(DateTime dateTime, bool isGood, float value)>> result, out MsgLogClass msgLog)
        {
            result = idTags.ToDictionary(item => item, _ => new List<(DateTime dateTime, bool isGood, float value)>());

            try
            {
                var strBeginTimeStamp = beginDateTime.ToString(TimeFormat);
                var strEndTimeStamp = endDateTime.ToString(TimeFormat);

                foreach (var tagID in idTags)
                {
                    var cql =
                     $"SELECT  \"DateTime\", \"Val\" FROM \"{database}\".\"NodeData_{nodeId}\" WHERE \"TagId\"={tagID} AND " +
                     $"\"DateTime\" >= \'{strBeginTimeStamp}\' AND \"DateTime\" < \'{strEndTimeStamp}\'";
                    using (var rowData = _session.Execute(cql))
                    {
                        foreach (var entity in rowData)
                        {
                            var dt = entity.GetValue<DateTime>("DateTime");
                            var tagValue = entity.GetValue<float?>("Val");
                            var isGood = tagValue.HasValue;

                            var val = tagValue.GetValueOrDefault();
                            result[tagID].Add((dt, isGood, val));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TryReadHistData)}. ", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }

            msgLog = null;
            return true;
        }

        /// <summary>
        /// Добавление даных
        /// </summary>
        /// <param name="database"></param>
        /// <param name="nodeId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool TryInsertData(string database, int nodeId, Dictionary<int, List<DataVal>> data, out MsgLogClass msgLog)
        {
            try
            {
                foreach (var tagItem in data) //идем по всем тегам
                {
                    var strVal = new StringBuilder();
                    strVal.Append("BEGIN BATCH ");
                    foreach (var item in tagItem.Value)
                    {
                        strVal.Append($"INSERT INTO \"{database}\".\"NodeData_{nodeId}\"(\"TagId\",\"DateTime\",\"Val\")VALUES(");
                        //формат даты cassandra
                        strVal.Append(item.IsGood
                            ? $"{tagItem.Key},\'{item.DateTime.ToString(TimeFormat)}\',{item.Val.ToString(CultureInfo.InvariantCulture)}"
                            : $"{tagItem.Key},\'{item.DateTime.ToString(TimeFormat)}\',null");
                        strVal.Append(");");
                    }

                    strVal.Append("APPLY BATCH;");
                    _session.Execute(strVal.ToString());
                    //_session.ExecuteAsync(IStatement statement);
                }

                msgLog = null;
                return true;
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TryInsertData)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }
        }

        public bool TryInsertData2(string database, int nodeId, Dictionary<int, List<DataVal>> data, out MsgLogClass msgLog)
        {
            try
            {
                foreach (var tagItem in data) //идем по всем тегам
                {
                    /*PreparedStatement statement = _session.Prepare($"INSERT INTO \"{database}\".\"NodeData_{nodeId}\"(\"TagId\",\"DateTime\",\"Val\")VALUES (?, ?, ?)");
                    BoundStatement boundStatement = new BoundStatement(statement);
                    var batchStmt = new BatchStatement();
                    batchStmt.Add(boundStatement.Bind("User A", "10"));
                    batchStmt.Add(boundStatement.Bind("User B", "12"));
                    _session.Execute(batchStmt);*/



                   /* var strVal = new StringBuilder();
                    strVal.Append($"INSERT INTO \"{database}\".\"NodeData_{nodeId}\"(\"TagId\",\"DateTime\",\"Val\")VALUES");

                    foreach (var item in tagItem.Value)
                    {                      
                        strVal.Append(item.IsGood
                            ? $"({tagItem.Key},\'{item.DateTime.ToString(TimeFormat)}\',{item.Val.ToString(CultureInfo.InvariantCulture)}),"
                            : $"({tagItem.Key},\'{item.DateTime.ToString(TimeFormat)}\',null),");                        
                    }

                    strVal.Remove(strVal.Length-1, 1);                   
                    _session.Execute(strVal.ToString());     */               
                }

                msgLog = null;
                return true;
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TryInsertData)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }
        }

        /// <summary>
        /// Добавление даных
        /// </summary>
        /// <param name="database"></param>
        /// <param name="nodeId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool TryInsertDataAsync(string database, int nodeId, Dictionary<int, List<DataVal>> data, out MsgLogClass msgLog)
        {
            try
            {
                foreach (var tagItem in data) //идем по всем тегам
                {
                    var strVal = new StringBuilder();
                    strVal.Append("BEGIN BATCH ");
                    foreach (var item in tagItem.Value)
                    {
                        strVal.Append($"INSERT INTO \"{database}\".\"NodeData_{nodeId}\"(\"TagId\",\"DateTime\",\"Val\")VALUES(");
                        //формат даты cassandra
                        strVal.Append(item.IsGood
                            ? $"{tagItem.Key},\'{item.DateTime.ToString(TimeFormat)}\',{item.Val.ToString(CultureInfo.InvariantCulture)}"
                            : $"{tagItem.Key},\'{item.DateTime.ToString(TimeFormat)}\',null");
                        strVal.Append(");");
                    }

                    strVal.Append("APPLY BATCH;");

                    var statement = new SimpleStatement(strVal.ToString());
                    _session.ExecuteAsync(statement);
                }

                msgLog = null;
                return true;
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TryInsertData)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }
        }

        /// <summary>
        /// Добавление снимка данных
        /// </summary>
        /// <param name="database"></param>
        /// <param name="nodeId"></param>
        /// <param name="snapShot"></param>
        /// <param name="snapShotDT"></param>
        /// <param name="msgLog"></param>
        /// <returns></returns>
        /*public bool TryInsertSnapShot(string database, int nodeId, ConcurrentDictionary<int, DataVal> snapShot, DateTime snapShotDT, out MsgLogClass msgLog)
        {
            try
            {
                var data = new Dictionary<int, List<DataVal>>();
                foreach (var item in snapShot)
                {
                    data.Add(item.Key, new List<DataVal>());
                    data[item.Key].Add(new DataVal() { Val = item.Value.Val, IsGood = item.Value.IsGood, DateTime = snapShotDT });
                }

                if (TryInsertData(database, nodeId, data, out MsgLogClass msgLog2))
                {
                    var strVal = $"({nodeId},\'{snapShotDT.ToString(TimeFormat)}\')";
                    var cql = $"INSERT INTO \"{database}\".\"NodeSlicesDT_{nodeId}\" (\"NodeId\",\"DateTime\") VALUES {strVal}";
                    _session.Execute(cql);

                    msgLog = null;
                    return true;
                }
                else
                {
                    msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TryInsertSnapShot)}", TypeLog = TypeMsg.Err, LogDetails = msgLog2.LogDetails };
                    return false;
                }
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TryInsertSnapShot)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }
        }*/

        public int TryInsertEvent(string database, int nodeId, (DateTime dateTime, int detectorId, string category, string status, string message, string additional, string eventType, string info, string trends, string user, string comments, string history) notifyEvent, out MsgLogClass msgLog)
        {
            try
            {                
                var cqlGetNextId = $"SELECT MAX(\"Id\") FROM \"{database}\".\"NodeEvents_{nodeId}\"";
                var nextId = _session.Execute(cqlGetNextId).First().GetValue<int?>(0) ?? 0;
                nextId++;

                var cqlInsertEvent =
                    $"INSERT INTO \"{database}\".\"NodeEvents_{nodeId}\" (\"Id\",\"DateTime\", \"DetectorId\", \"Type\", \"Category\", \"Status\", " +
                    $"\"Message\", \"Additional\", \"Info\", \"Trends\", \"User\", \"Comments\", \"History\") " +
                    $"VALUES ({nextId},\'{notifyEvent.dateTime.ToString(TimeFormat)}\', " +
                    $"{notifyEvent.detectorId}, \'{notifyEvent.eventType}\', \'{notifyEvent.category}\', \'{notifyEvent.status}\', " +
                    $"\'{notifyEvent.message}\', \'{notifyEvent.additional}\', \'{notifyEvent.info}\', " +
                    $"\'{notifyEvent.trends}\', \'{notifyEvent.user}\', \'{notifyEvent.comments}\', \'{notifyEvent.history}\');";
                _session.Execute(cqlInsertEvent);

                msgLog = null;
                return nextId;
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TryInsertEvent)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };                
                return 0;
            }
        }

        public bool TryUpdateEvent(string database, int nodeId, int eventId, (DateTime dateTime, int detectorId, string category, string status, string message, string additional, string eventType, string info, string trends, string user, string comments, string history) notifyEvent, out MsgLogClass msgLog)
        {
            try
            {               
                var cql = $"UPDATE \"{database}\".\"NodeEvents_{nodeId}\" SET " +
                          $"\"Type\" = '{notifyEvent.eventType}', \"Category\" = '{notifyEvent.category}'," +
                          $" \"Status\" = \'{notifyEvent.status}\', \"Message\" = \'{notifyEvent.message}\'," +
                          $" \"Additional\" = \'{notifyEvent.additional}\', \"Info\" = \'{notifyEvent.info}\'," +
                          $" \"Trends\"= \'{notifyEvent.trends}\', \"User\"= \'{notifyEvent.user}\'," +
                          $" \"Comments\" = \'{notifyEvent.comments}\', \"History\" = \'{notifyEvent.history}\'" +
                          $"WHERE \"Id\" = {eventId} AND \"DetectorId\"={notifyEvent.detectorId} AND \"DateTime\"=\'{notifyEvent.dateTime.ToString(TimeFormat)}\';";
                _session.Execute(cql);

                msgLog = null;
                return true;
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TryUpdateEvent)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }
        }

        /// <summary>
        /// Чтение неподтвержденных событий
        /// </summary>
        /// <param name="database"></param>
        /// <param name="nodeId"></param>
        /// <param name="unAckEvents"></param>
        /// <param name="msgLog"></param>
        /// <returns></returns>
        public bool TryReadUnAckEvents(string database, int nodeId, out List<(int id, int detectorId, DateTime dateTime, string category, string status, string message, string additional, string eventType, string info, string trends, string user, string comments, string history)> unAckEvents, out MsgLogClass msgLog)
        {
            unAckEvents =
                new List<(int id, int detectorId, DateTime dateTime, string category, string status, string message,
                    string additional, string eventType, string info, string trends, string user, string comments,
                    string history)>();

            try
            {                
                var cqlReadEvents =
                    _session.Prepare(
                    "SELECT \"Id\", \"DetectorId\", \"DateTime\", \"Type\", \"Category\", \"Status\", \"Message\"," +
                    " \"Additional\", \"Info\", \"Trends\", \"User\", \"Comments\", \"History\" " +
                    $"FROM \"{database}\".\"NodeEvents_{nodeId}\" WHERE \"Type\"=? ALLOW FILTERING;");

                var eventTypes = new[]
                {
                    "newEvent", "repeatEvent", "acceptEvent", "lockEvent"
                };

                foreach (var eventType in eventTypes)
                {
                    var cql = cqlReadEvents.Bind(eventType);
                    var rowSet = _session.Execute(cql);

                    foreach (var row in rowSet)
                    {
                        unAckEvents.Add((int.Parse(row["Id"].ToString()), int.Parse(row["DetectorId"].ToString()),
                            DateTime.Parse(row["DateTime"].ToString()), row["Category"].ToString(),
                            row["Status"].ToString(), row["Message"].ToString(),
                            row["Additional"].ToString(), row["Type"].ToString(), row["Info"].ToString(),
                            row["Trends"].ToString(), row["User"].ToString(),
                            row["Comments"].ToString(), row["History"].ToString()));
                    }
                }

                msgLog = null;
                return true;
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TryReadUnAckEvents)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }
        }

        /// <summary>
        /// Чтение исторических событий
        /// </summary>
        /// <param name="database"></param>
        /// <param name="nodeId"></param>
        /// <param name="beginDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <param name="histEvents"></param>
        /// <param name="msgLog"></param>
        /// <returns></returns>
        public bool TryReadHistEvents(string database, int nodeId, DateTime beginDateTime, DateTime endDateTime, 
            out List<(int id, int detectorId, DateTime dateTime, string category, string status, string message, string additional, string eventType, string info, string trends, string user, string comments, string history)> histEvents, 
            out MsgLogClass msgLog)
        {
           histEvents =
                new List<(int id, int detectorId, DateTime dateTime, string category, string status, string message,
                    string additional, string eventType, string info, string trends, string user, string comments,
                    string history)>();            

            var strBeginTimeStamp = beginDateTime.ToString(TimeFormat);
            var strEndTimeStamp = endDateTime.ToString(TimeFormat);

            try
            {
                var cqlReadEvents =                    
                    "SELECT \"Id\", \"DetectorId\", \"DateTime\", \"Type\", \"Category\", \"Status\", \"Message\"," +
                    " \"Additional\", \"Info\", \"Trends\", \"User\", \"Comments\", \"History\" " +
                    $"FROM \"{database}\".\"NodeEvents_{nodeId}\" WHERE \"DateTime\" >= '{strBeginTimeStamp}' AND \"DateTime\" < '{strEndTimeStamp}' ALLOW FILTERING;";

                using var rowData = _session.Execute(cqlReadEvents);

                foreach (var rowEvent in rowData)
                {
                    var dt = rowEvent.GetValue<DateTime>("DateTime");
                    var eventType = rowEvent.GetValue<string>("Type");
                    var message = rowEvent.GetValue<string>("Message");
                    var status = rowEvent.GetValue<string>("Status");
                    var category = rowEvent.GetValue<string>("Category");
                    var additional = rowEvent.GetValue<string>("Additional");
                    var info = rowEvent.GetValue<string>("Info");
                    var trends = rowEvent.GetValue<string>("Trends");
                    var user = rowEvent.GetValue<string>("User");
                    var comments = rowEvent.GetValue<string>("Comments");
                    var history = rowEvent.GetValue<string>("History");
                    var id = rowEvent.GetValue<int>("Id");
                    var detectorId = rowEvent.GetValue<int>("DetectorId");

                    histEvents.Add((id, detectorId, dt, category, status, message, additional, eventType, info, trends, user, comments, history));
                }

                msgLog = null;
                return true;
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TryReadHistEvents)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }
        }

        /// <summary>
        /// Инициализация БД
        /// </summary>
        /// <param name="histDBName"></param>
        /// <param name="msgLog"></param>
        /// <returns></returns>
        public bool TryInitializeHistDB(string histDBName, out MsgLogClass msgLog)
        {
            //Создание пространства ключей для проекта
            if (!TryCreateKeyspace(histDBName, out MsgLogClass msg1))
            {
                msgLog = msg1;
                return false;
            }           

            msgLog = null;
            return true;
        }

        /// <summary>
        /// Инициализация колоночных семейств
        /// </summary>
        /// <param name="histDBName"></param>
        /// <param name="nodeIds"></param>
        /// <param name="msgLog"></param>
        /// <returns></returns>
        public bool TryInitializeHistDBColumns(string histDBName, List<int> nodeIds, out MsgLogClass msgLog)
        {
            //Создание колоночных семейств для каждого актива 
            foreach (var nodeId in nodeIds)
            {
                //Создание колоночного семейства данных
                if (!TryCreateColumnFamily(histDBName, "NodeData_" + nodeId, new[] { ("TagId", "int"), ("DateTime", "timestamp"), ("Val", "float") }, "DateTime", new[] { "TagId" }, out MsgLogClass msg2))
                {
                    msgLog = msg2;
                    return false;
                }

                //Создание колоночного семейства срезов данных
                if (!TryCreateColumnFamily(histDBName, "NodeSlicesDT_" + nodeId, new[] { ("NodeId", "int"), ("DateTime", "timestamp") }, "DateTime", new[] { "NodeId" }, out MsgLogClass msg3))
                {
                    msgLog = msg3;
                    return false;
                }

                //Создание колоночного семейства событий
                if (!TryCreateColumnFamily(histDBName, "NodeEvents_" + nodeId,
                    new[]
                    {
                        ("Id", "int"),("DateTime", "timestamp"), ("DetectorId", "int"), ("Type", "varchar"),
                    ("Category", "varchar"), ("Status", "varchar"), ("Message", "varchar"), ("Additional", "varchar"),
                    ("Info", "varchar"), ("Trends", "varchar"), ("User", "varchar"), ("Comments", "varchar"), ("History", "varchar")
                    },
                    "DateTime", new[] { "Id", "DetectorId" }, out MsgLogClass msg4))
                {
                    msgLog = msg4;
                    return false;
                }
            }

            msgLog = null;
            return true;
        }


        public void Dispose()
        {
            _cluster?.Dispose();
            _session?.Dispose();
        }

        /// <summary>
        ///  Удаляет схему(keyspace) в кассандре
        /// </summary>
        public bool TryDropKeySpace(string keyspace, out MsgLogClass msgLog)
        {            
            try
            {
                var cql = $"DROP KEYSPACE \"{keyspace}\";";
                _session.Execute(cql);
            }
            catch (Exception exception)
            {               
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TryDropKeySpace)}. ", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }

            msgLog = null;
            return true;
        }

        /// <summary>
        /// Создание пространства ключей
        /// </summary>
        /// <param name="keyspace"></param>
        /// <returns></returns>
        private bool TryCreateKeyspace(string keyspace, out MsgLogClass msgLog)
        {            
            try
            {
                var query =
                    $"CREATE KEYSPACE IF NOT EXISTS \"{keyspace}\" with replication={{'class':'SimpleStrategy','replication_factor': '1'}};";
                _session.Execute(query);
            }
            catch (Exception exception)
            {                
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TryCreateKeyspace)}. ", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }

            msgLog = null;
            return true;
        }

        /// <summary>
        /// Создание колоночного семейства данных
        /// </summary>
        private bool TryCreateColumnFamily(string keyspace, string tableName, (string columnName, string columnType)[] columns, string clusterKey, string[] partitionKeys, out MsgLogClass msgLog)
        {         
            try
            {
                var queryTable =
                    $"CREATE TABLE IF NOT EXISTS \"{keyspace}\".\"{tableName}\" ";
                var tableWithColumns = new StringBuilder();
                tableWithColumns.Append(queryTable);
                tableWithColumns.Append('(');
                foreach (var (columnName, columnType) in columns)
                {
                    tableWithColumns.Append($"\"{columnName}\" {columnType},");
                }

                var clusterSort = $"WITH CLUSTERING ORDER BY (\"{clusterKey}\" ASC)";

                var partitionKey = "(" + partitionKeys.Select(k => $"\"{k}\"").Aggregate((x, y) => $"{x},{y}") + ")";
                var commonKey = $"PRIMARY KEY ({partitionKey},\"{clusterKey}\")";
                tableWithColumns.Append(commonKey);
                tableWithColumns.Append("\n)");
                tableWithColumns.Append(clusterSort);
                tableWithColumns.Append(';');

                _session.Execute(tableWithColumns.ToString());
            }
            catch (Exception exception)
            {               
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TryCreateColumnFamily)}. ", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }

            msgLog = null;
            return true;
        }

        /// <summary>
        /// Функция соединения с бд
        /// </summary>
        private void Connect(HistoryDataBaseInfo serverConnectInfo)
        {
            var msQueryTimeout = (int)TimeSpan.FromSeconds(serverConnectInfo.CommandTimeout).TotalMilliseconds;
            _cluster = Cluster.Builder()
                .AddContactPoint(serverConnectInfo.Host)
                .WithAuthProvider(new PlainTextAuthProvider(serverConnectInfo.User, serverConnectInfo.Password))
                .WithPort(serverConnectInfo.Port)
                .WithQueryTimeout(msQueryTimeout)
                .Build();

            var connectionStringBuilder = new CassandraConnectionStringBuilder
            {
                Username = serverConnectInfo.User,
                Password = serverConnectInfo.Password,
                Port = serverConnectInfo.Port,
                ContactPoints = new[] { serverConnectInfo.Host }
            };
            _connectionString = connectionStringBuilder.ToString();
            _session = _cluster.Connect();
        }

    }
}