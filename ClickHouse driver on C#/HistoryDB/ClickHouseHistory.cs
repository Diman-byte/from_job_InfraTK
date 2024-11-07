using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClickHouse.Client.ADO;
using ClickHouse.Client.ADO.Parameters;
using ClickHouse.Client.Utility;
using ClickHouse.Client.ADO.Readers;
using Common;
using Common.MsgLog;
using Microsoft.Extensions.Logging;

namespace HistoryDB
{
    

    public class ClickHouseHistory
    {
        private ClickHouseConnection _session;
        public const string TimeFormat = "yyyy-MM-dd HH:mm:ss";

        public ClickHouseHistory() {

        }

        public bool TryConnect(HistoryDataBaseInfo serverConnectInfo, out MsgLogClass msgLog)
        {
            try
            {
                _session = new ClickHouseConnection($"Host={serverConnectInfo.Host};Port={serverConnectInfo.Port};Username={serverConnectInfo.User};password={serverConnectInfo.Password};Database={serverConnectInfo.DataBase}");
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(ClickHouseHistory), LogText = $"Не удалось подключиться к Cassandra", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }

            msgLog = null;
            return true;
        }

        public bool TryDisсonnect()
        {
            try
            {
                _session.Close();
                _session.Dispose();
            }
            catch (Exception exception)
            {
                return false;
                //msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Не удалось разорвать соединение с Cassandra", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };              
            }

            return true;
        }

        // создание ключевого пространства (базы данных)
        // потом можно будет добавить выбор движка
        private bool TryCreateKeyspace(string database, out MsgLogClass msgLog)
        {
            try
            {
                using var connection = _session;
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = $"CREATE DATABASE IF NOT EXISTS \"{database}\"ENGINE = Memory;";
                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(ClickHouseHistory), LogText = $"Возникло исключение в функции {nameof(TryCreateKeyspace)}. ", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }

            msgLog = null;
            return true;
        }

        // Инициализация БД
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

        // с выбором движка потом можно поработать
        private bool TryCreateColumnFamily(string database, string tableName, (string columnName, string columnType)[] columns, string sortingKey, string[] primaryKey, out MsgLogClass msgLog)
        {
            try
            {
                var queryTable =
                    $"CREATE TABLE IF NOT EXISTS \"{database}\".\"{tableName}\" ";
                var tableWithColumns = new StringBuilder();
                tableWithColumns.Append(queryTable);
                tableWithColumns.Append('(');
                foreach (var (columnName, columnType) in columns)
                {
                    tableWithColumns.Append($"\"{columnName}\" {columnType},");
                }
                tableWithColumns.Length--;


                var engine_table = ") ENGINE = MergeTree()\n";
                var partitionKey = primaryKey.Select(k => $"\"{k}\"").Aggregate((x, y) => $"{x},{y}");
                var commonKey = $"PRIMARY KEY ({partitionKey},\"{sortingKey}\")";


                var clusterSort = $"ORDER BY ({partitionKey},\"{sortingKey}\")";

                tableWithColumns.Append(engine_table);
                tableWithColumns.Append(commonKey);
                tableWithColumns.Append("\n");
                tableWithColumns.Append(clusterSort);
                tableWithColumns.Append(';');

                // Console.WriteLine(tableWithColumns.ToString());

                using var connection = _session;
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = tableWithColumns.ToString();
                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(ClickHouseHistory), LogText = $"Возникло исключение в функции {nameof(TryCreateColumnFamily)}. ", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                Console.WriteLine(msgLog.TypeLog + msgLog.LogDetails);
                return false;
            }

            msgLog = null;
            return true;
        }


        /// Инициализация колоночных семейств
        public bool TryInitializeHistDBColumns(string histDBName, List<Guid> nodeIds, out MsgLogClass msgLog)
        {
            //Создание колоночных семейств для каждого актива 
            foreach (var nodeId in nodeIds)
            {
                //Создание колоночного семейства данных
                if (!TryCreateColumnFamily(histDBName, "NodeData_" + nodeId.ToString().Replace('-', '_'), new[] { ("TagId", "UUID"), ("DateTime", "timestamp"), ("Val", "float") }, "DateTime", new[] { "TagId" }, out MsgLogClass msg2))
                {
                    msgLog = msg2;
                    return false;
                }

                //Создание колоночного семейства срезов данных
                if (!TryCreateColumnFamily(histDBName, "NodeSlicesDT_" + nodeId.ToString().Replace('-', '_'), new[] { ("NodeId", "UUID"), ("DateTime", "timestamp") }, "DateTime", new[] { "NodeId" }, out MsgLogClass msg3))
                {
                    msgLog = msg3;
                    return false;
                }

                //Создание колоночного семейства событий
                if (!TryCreateColumnFamily(histDBName, "NodeEvents_" + nodeId.ToString().Replace('-', '_'),
                    new[]
                    {
                        ("Id", "UUID"),("DateTime", "timestamp"), ("DetectorId", "UUID"), ("Type", "varchar"),
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

        // я думаю, можно было бы в один запрос все теги добавлять, но сделал также, как было в ClickHouseHistory
        public bool TryInsertData_1(string database, int nodeId, Dictionary<int, List<DataVal>> data, out MsgLogClass msgLog)
        {
            try
            {
                foreach (var tagItem in data) // идем по всем тегам
                {
                    var strVal = new StringBuilder();
                    strVal.Append($"INSERT INTO \"{database}\".\"NodeData_{nodeId}\" (\"TagId\", \"DateTime\", \"Val\") VALUES ");

                    var values = new List<string>();
                    foreach (var item in tagItem.Value)
                    {
                        var value = item.IsGood
                            ? $"({tagItem.Key}, \'{item.DateTime.ToString(TimeFormat)}\', {item.Val.ToString(CultureInfo.InvariantCulture)})"
                            : $"({tagItem.Key}, \'{item.DateTime.ToString(TimeFormat)}\', NULL)";
                        values.Add(value);
                    }

                    strVal.Append(string.Join(", ", values));
                    strVal.Append(';');

                    // Console.WriteLine(strVal.ToString());

                    using var connection = _session;
                    connection.Open();
                    using var command = connection.CreateCommand();
                    command.CommandText = strVal.ToString();
                    command.ExecuteNonQuery();
                }

                msgLog = null;
                return true;
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(ClickHouseHistory), LogText = $"Возникло исключение в функции {nameof(TryInsertData_1)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                Console.WriteLine(msgLog.TypeLog + msgLog.LogDetails);
                return false;
            }
        }

        public bool TryInsertData_2(string database, int nodeId, Dictionary<int, List<DataVal>> data, out MsgLogClass msgLog)
        {
            try
            {
                var strVal = new StringBuilder();
                strVal.Append($"INSERT INTO \"{database}\".\"NodeData_{nodeId}\" (\"TagId\", \"DateTime\", \"Val\") VALUES ");

                foreach (var tagItem in data) // идем по всем тегам
                {

                    var values = new List<string>();
                    foreach (var item in tagItem.Value)
                    {
                        var value = item.IsGood
                            ? $"({tagItem.Key}, \'{item.DateTime.ToString(TimeFormat)}\', {item.Val.ToString(CultureInfo.InvariantCulture)})"
                            : $"({tagItem.Key}, \'{item.DateTime.ToString(TimeFormat)}\', NULL)";
                        values.Add(value);
                    }

                    strVal.Append(string.Join(", ", values));
                    strVal.Append(", ");
                    
                }
                strVal.Length = strVal.Length - 2;
                strVal.Append(';');

                using var connection = _session;
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = strVal.ToString();
                command.ExecuteNonQuery();

                // Console.WriteLine(strVal.ToString());

                msgLog = null;
                return true;
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(ClickHouseHistory), LogText = $"Возникло исключение в функции {nameof(TryInsertData_2)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                Console.WriteLine(msgLog.TypeLog + msgLog.LogDetails);
                return false;
            }
        }


        public bool TryInsertData_3(string database, string nodeId, Dictionary<Guid, List<DataVal>> data, out MsgLogClass msgLog)
        {
            try
            {
                var strVal = new StringBuilder();
                strVal.Append($"INSERT INTO \"{database}\".\"NodeData_{nodeId.Replace('-', '_')}\" (\"TagId\", \"DateTime\", \"Val\") VALUES ");

                foreach (var tagItem in data) // идем по всем тегам
                {

                    var values = new List<string>();
                    foreach (var item in tagItem.Value)
                    {
                        var value = item.IsGood
                            ? $"(\'{tagItem.Key}\', \'{item.DateTime.ToString(TimeFormat)}\', {item.Val.ToString(CultureInfo.InvariantCulture)})"
                            : $"(\'{tagItem.Key}\', \'{item.DateTime.ToString(TimeFormat)}\', NULL)";
                        values.Add(value);
                    }

                    strVal.Append(string.Join(", ", values));
                    strVal.Append(", ");

                }
                strVal.Length = strVal.Length - 2;
                strVal.Append(';');

                Console.WriteLine("sosi\n" + strVal.ToString() + "\n\n");

                using var connection = _session;
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = strVal.ToString();
                command.ExecuteNonQuery();

                

                msgLog = null;
                return true;
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(ClickHouseHistory), LogText = $"Возникло исключение в функции {nameof(TryInsertData_3)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                Console.WriteLine(msgLog.TypeLog + msgLog.LogDetails);
                return false;
            }
        }


        public async Task<bool> TryInsertData_2_async(string database, int nodeId, Dictionary<int, List<DataVal>> data, MsgLogClass msgLog)
        {
            try
            {
                var strVal = new StringBuilder();
                strVal.Append($"INSERT INTO \"{database}\".\"NodeData_{nodeId}\" (\"TagId\", \"DateTime\", \"Val\") VALUES ");

                foreach (var tagItem in data) // идем по всем тегам
                {

                    var values = new List<string>();
                    foreach (var item in tagItem.Value)
                    {
                        var value = item.IsGood
                            ? $"({tagItem.Key}, \'{item.DateTime.ToString(TimeFormat)}\', {item.Val.ToString(CultureInfo.InvariantCulture)})"
                            : $"({tagItem.Key}, \'{item.DateTime.ToString(TimeFormat)}\', NULL)";
                        values.Add(value);
                    }

                    strVal.Append(string.Join(", ", values));
                    strVal.Append(", ");

                }
                strVal.Length = strVal.Length - 2;
                strVal.Append(';');

                using var connection = _session;
                await connection.OpenAsync();
                using var command = connection.CreateCommand();
                command.CommandText = strVal.ToString();
                await command.ExecuteNonQueryAsync();

                // Console.WriteLine(strVal.ToString());

                msgLog = null;
                return true;
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(ClickHouseHistory), LogText = $"Возникло исключение в функции {nameof(TryInsertData_2)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                Console.WriteLine(msgLog.TypeLog + msgLog.LogDetails);
                return false;
            }
        }


        public bool TryReadHistData(string database,
            Guid nodeId, DateTime beginDateTime, DateTime endDateTime, List<string> idTags,
            out Dictionary<Guid, List<(DateTime dateTime, bool isGood, float value)>> result, out MsgLogClass msgLog)
        {
            result = idTags.ToDictionary(item => Guid.Parse(item),
                _ => new List<(DateTime dateTime, bool isGood, float value)>());

            try
            {
                var strBeginTimeStamp = beginDateTime.ToString(TimeFormat);
                var strEndTimeStamp = endDateTime.ToString(TimeFormat);

                using var connection = _session;
                connection.Open();
                using var command = connection.CreateCommand();


                foreach (var tagID in idTags)
                {
                    var sql =
                     $"SELECT  \"DateTime\", \"Val\" FROM \"{database}\".\"NodeData_{nodeId.ToString().Replace('-', '_')}\" WHERE \"TagId\"=\'{tagID}\' AND " +
                     $"\"DateTime\" >= \'{strBeginTimeStamp}\' AND \"DateTime\" < \'{strEndTimeStamp}\'";
                    //Console.WriteLine(sql.ToString());
                    command.CommandText = sql;
                    using (var rowData = command.ExecuteReader())
                    {
                        while (rowData.Read())
                        {
                            var dt = Convert.ToDateTime(rowData["DateTime"]);
                            float? tagValue = (float)rowData["Val"];
                            bool isGood;
                            if (tagValue != 0)
                            {
                                isGood = true;
                            }
                            else
                            {
                                isGood = false;
                            }

                            var val = tagValue.GetValueOrDefault();
                            result[Guid.Parse(tagID)].Add((dt, isGood, val));
                        }
                        
                    }
                }
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(ClickHouseHistory), LogText = $"Возникло исключение в функции {nameof(TryReadHistData)}. ", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }

            msgLog = null;
            msgLog = null;
            return true;
        }

        public bool TryTruncateNodeTables(string database, int nodeId, out MsgLogClass msgLog)
        {
            try
            {
                using var connection = _session;
                connection.Open();
                using var command = connection.CreateCommand();

                var truncateQueries = new[] {
                    $"TRUNCATE TABLE IF EXISTS \"{database}\".\"NodeSlicesDT_{nodeId}\"",
                    $"TRUNCATE TABLE IF EXISTS \"{database}\".\"NodeEvents_{nodeId}\"",
                    $"TRUNCATE TABLE IF EXISTS \"{database}\".\"NodeData_{nodeId}\""
                };

                foreach (var query in truncateQueries)
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(ClickHouseHistory), LogText = $"Возникло исключение в функции {nameof(TryTruncateNodeTables)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                Console.WriteLine(msgLog.TypeLog + msgLog.LogDetails);
                return false;
            }

            msgLog = null;
            return true;
        }

        public bool TryDropNodeTables(string database, int nodeId, out MsgLogClass msgLog)
        {
            try
            {
                using var connection = _session;
                connection.Open();
                using var command = connection.CreateCommand();

                var truncateQueries = new[] {
                $"DROP TABLE IF EXISTS \"{database}\".\"NodeData_{nodeId}\"",
                $"DROP TABLE IF EXISTS \"{database}\".\"NodeSlicesDT_{nodeId}\"",
                $"DROP TABLE IF EXISTS \"{database}\".\"NodeEvents_{nodeId}\""
                };

                foreach (var query in truncateQueries)
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(ClickHouseHistory), LogText = $"Возникло исключение в функции {nameof(TryDropNodeTables)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }

            msgLog = null;
            return true;
        }

        public bool TryDropDatabase(string database, out MsgLogClass msgLog)
        {
            try
            {
                using var connection = _session;
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = $"DROP DATABASE IF EXISTS \"{database}\";";
                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(ClickHouseHistory), LogText = $"Возникло исключение в функции {nameof(TryDropDatabase)}. ", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }

            msgLog = null;
            return true;
        }


        public bool TryReadSliceHistData(string database,
            int nodeId, DateTime dateTimeFrom, DateTime dateTimeTo, List<int> idTags, int pointsAmount, AlgSliceEnum algSlice,
            out Dictionary<int, List<DataVal>> result, out MsgLogClass msgLog)
        {
            result = new Dictionary<int, List<DataVal>>(0);
            using var connection = _session;
        

            DbDataReader GetTagReaderMethod(int tagId, string sortOrder)
            {
                var cql = $"SELECT  \"DateTime\", \"Val\" FROM \"{database}\".\"NodeData_{nodeId}\" WHERE \"TagId\"={tagId} AND " +
                         $"\"DateTime\" >= \'{dateTimeFrom.ToString(TimeFormat)}\' AND \"DateTime\" < \'{dateTimeTo.ToString(TimeFormat)}\' ORDER BY \"DateTime\" {sortOrder}";
                using var command = connection.CreateCommand();
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


        public int TryInsertEvent(string database, int nodeId, (DateTime dateTime, int detectorId, string category, string status, string message, string additional, string eventType, string info, string trends, string user, string comments, string history) notifyEvent, out MsgLogClass msgLog)
        {
            try
            {
                using var connection = _session;
                connection.Open();
                using var command_next_id = connection.CreateCommand();

                var sqlGetNextId = $"SELECT MAX(\"Id\") FROM \"{database}\".\"NodeEvents_{nodeId}\"";
                command_next_id.CommandText = sqlGetNextId;


                var nextId = (int?)command_next_id.ExecuteScalar() ?? 0;
                nextId++;

                var cqlInsertEvent =
                    $"INSERT INTO \"{database}\".\"NodeEvents_{nodeId}\" (\"Id\",\"DateTime\", \"DetectorId\", \"Type\", \"Category\", \"Status\", " +
                    $"\"Message\", \"Additional\", \"Info\", \"Trends\", \"User\", \"Comments\", \"History\") " +
                    $"VALUES ({nextId},\'{notifyEvent.dateTime.ToString(TimeFormat)}\', " +
                    $"{notifyEvent.detectorId}, \'{notifyEvent.eventType}\', \'{notifyEvent.category}\', \'{notifyEvent.status}\', " +
                    $"\'{notifyEvent.message}\', \'{notifyEvent.additional}\', \'{notifyEvent.info}\', " +
                    $"\'{notifyEvent.trends}\', \'{notifyEvent.user}\', \'{notifyEvent.comments}\', \'{notifyEvent.history}\');";

                using var command_insertEvent = connection.CreateCommand();
                command_insertEvent.CommandText = cqlInsertEvent;
                command_insertEvent.ExecuteNonQuery();

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
                var sql = $"UPDATE \"{database}\".\"NodeEvents_{nodeId}\" SET " +
                          $"\"Type\" = '{notifyEvent.eventType}', \"Category\" = '{notifyEvent.category}'," +
                          $" \"Status\" = \'{notifyEvent.status}\', \"Message\" = \'{notifyEvent.message}\'," +
                          $" \"Additional\" = \'{notifyEvent.additional}\', \"Info\" = \'{notifyEvent.info}\'," +
                          $" \"Trends\"= \'{notifyEvent.trends}\', \"User\"= \'{notifyEvent.user}\'," +
                          $" \"Comments\" = \'{notifyEvent.comments}\', \"History\" = \'{notifyEvent.history}\'" +
                          $"WHERE \"Id\" = {eventId} AND \"DetectorId\"={notifyEvent.detectorId} AND \"DateTime\"=\'{notifyEvent.dateTime.ToString(TimeFormat)}\';";

                using var connection = _session;
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();

                msgLog = null;
                return true;
            }
            catch (Exception exception)
            {
                msgLog = new MsgLogClass() { SourceLog = nameof(CassandraHistory), LogText = $"Возникло исключение в функции {nameof(TryUpdateEvent)}", TypeLog = TypeMsg.Err, LogDetails = exception.ToString() };
                return false;
            }
        }


        public bool TryReadUnAckEvents(string database, int nodeId, out List<(int id, int detectorId, DateTime dateTime, string category, string status, string message, string additional, string eventType, string info, string trends, string user, string comments, string history)> unAckEvents, out MsgLogClass msgLog)
        {
            unAckEvents = new List<(int id, int detectorId, DateTime dateTime, string category, string status, string message, string additional, string eventType, string info, string trends, string user, string comments, string history)>();
            try
            {
                using var connection = _session;
                connection.Open();

                var eventTypes = new[]
                {
                "newEvent", "repeatEvent", "acceptEvent", "lockEvent"
            };

                foreach (var eventType in eventTypes)
                {
                    var sqlReadEvents =
                        $"SELECT Id, DetectorId, DateTime, Type, Category, Status, Message, Additional, Info, Trends, User, Comments, History " +
                        $"FROM {database}.NodeEvents_{nodeId} WHERE Type = @eventType";

                    using var command = connection.CreateCommand();
                    command.CommandText = sqlReadEvents;

                    var parameter = new ClickHouseDbParameter
                    {
                        ParameterName = "eventType",
                        DbType = System.Data.DbType.String,
                        Value = eventType
                    };
                    command.Parameters.Add(parameter);

                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        unAckEvents.Add((
                            reader.GetInt32(0), // Id
                            reader.GetInt32(1), // DetectorId
                            reader.GetDateTime(2), // DateTime
                            reader.GetString(4), // Category
                            reader.GetString(5), // Status
                            reader.GetString(6), // Message
                            reader.GetString(7), // Additional
                            reader.GetString(3), // Type (eventType)
                            reader.GetString(8), // Info
                            reader.GetString(9), // Trends
                            reader.GetString(10), // User
                            reader.GetString(11), // Comments
                            reader.GetString(12)  // History
                        ));
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


        public bool TryReadHistEvents(string database, int nodeId, DateTime beginDateTime, DateTime endDateTime,
        out List<(int id, int detectorId, DateTime dateTime, string category, string status, string message, string additional, string eventType, string info, string trends, string user, string comments, string history)> histEvents,
        out MsgLogClass msgLog)
        {
            histEvents = new List<(int id, int detectorId, DateTime dateTime, string category, string status, string message, string additional, string eventType, string info, string trends, string user, string comments, string history)>();

            var strBeginTimeStamp = beginDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            var strEndTimeStamp = endDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                using var connection = _session;
                connection.Open();

                var sqlReadEvents =
                    $"SELECT Id, DetectorId, DateTime, Type, Category, Status, Message, Additional, Info, Trends, User, Comments, History " +
                    $"FROM {database}.NodeEvents_{nodeId} WHERE DateTime >= '{strBeginTimeStamp}' AND DateTime < '{strEndTimeStamp}'";

                using var command = connection.CreateCommand();
                command.CommandText = sqlReadEvents;

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    histEvents.Add((
                        reader.GetInt32(0), // Id
                        reader.GetInt32(1), // DetectorId
                        reader.GetDateTime(2), // DateTime
                        reader.GetString(4), // Category
                        reader.GetString(5), // Status
                        reader.GetString(6), // Message
                        reader.GetString(7), // Additional
                        reader.GetString(3), // Type (eventType)
                        reader.GetString(8), // Info
                        reader.GetString(9), // Trends
                        reader.GetString(10), // User
                        reader.GetString(11), // Comments
                        reader.GetString(12)  // History
                    ));
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





        public async Task<string> GetDatabaseVersionAsync()
        {
            await using var connection = _session;
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT version()";
            var result = await command.ExecuteScalarAsync();
            return result.ToString();
        }

        public string GetDatabaseVersion()
        {
            using var connection = _session;
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT version()";
            var result = command.ExecuteScalar();
            return result.ToString();
        }

        public void Create_table(string name_table)
        {
            using var connection = _session;
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = $"CREATE TABLE IF NOT EXISTS {name_table} (id UInt32, name String) ENGINE = MergeTree() ORDER BY id";
            command.ExecuteNonQuery();
        }



        public List<Tuple<uint, string>> GetDataFromTable(string tableName)
        {
            List<Tuple<uint, string>> result = new List<Tuple<uint, string>>();

            using (var connection = _session)
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT id, name FROM {tableName}";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            uint id = Convert.ToUInt32(reader["id"]);
                            string name = Convert.ToString(reader["name"]);
                            result.Add(new Tuple<uint, string>(id, name));
                        }
                    }
                }
            }

            return result;
        }




    }


   
}
