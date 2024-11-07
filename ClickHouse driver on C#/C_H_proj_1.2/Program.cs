using Common;
using Common.MsgLog;
using HistoryDB;

var connectionInfo = new HistoryDB.HistoryDataBaseInfo()
{
    Host = "172.30.201.25",
    Port = 8123,
    DataBase = "PA_History",
    User = "clickhouse_admin",
    Password = "bebra_hz_1",
    CommandTimeout = 40
};


var connection = new ClickHouseHistory();
bool status_conn = connection.TryConnect(connectionInfo, out var msgLog);

if(status_conn == true)
{
    Console.WriteLine($"Подключение к БД под хостом: {connectionInfo.Host} выполнено успешно");
}
else
{
    Console.WriteLine("Не подлючено");
}

string version = connection.GetDatabaseVersion();
Console.WriteLine($"ClickHouse version: {version}");

//connection.TryInitializeHistDB("db_test_4", out msgLog);


//string tableName = "new_table_2";
//List<Tuple<uint, string>> data = connection.GetDataFromTable(tableName);
//foreach (var tuple in data)
//{
//    Console.WriteLine($"ID: {tuple.Item1}, Name: {tuple.Item2}");
//}

//bool statussss = connection.TryInitializeHistDBColumns(connectionInfo.DataBase, new List<Guid> { Guid.NewGuid() }, out MsgLogClass msg5);
//if (statussss)
//{
//    Console.WriteLine("Таблицы и столбцы инициализированы");
//}
//else
//{
//    Console.WriteLine("Ошибка инициализвции таблиц и столцов");
//}


// доабавление данных
Dictionary<Guid, List<DataVal>> data_1 = new Dictionary<Guid, List<DataVal>>();

// ручной метод
data_1.Add(Guid.NewGuid(), new List<DataVal>() { new DataVal { DateTime = DateTime.Now, Val = 10.5, IsGood = true } });
//data_1.Add(2, new List<DataVal>() { new DataVal { DateTime = DateTime.Now, Val = 11, IsGood = true } });

// рандом
//int limit_tag = 1000;   // количество тегов для генерации
//int interval = 1000;  // диапазон рандомных чисел от 0 до interval
//int kolvo_val = 1000;   // количество значений в каждом теге
//var random = new Random();
//for (int tag_id = 1; tag_id <= limit_tag; tag_id++)
//{
//    var new_list = new List<DataVal>();
//    for (int i = 0; i < kolvo_val; i++)
//    {
//        new_list.Add(new DataVal
//        {
//            DateTime = DateTime.Now.AddSeconds(-i),
//            Val = (double)(random.NextDouble() * interval), // генерация случайного значения
//            IsGood = random.NextDouble() > 0.1 // 10% шанс на false
//        });
//    }
//    data_1[tag_id] = new_list;
//}

//bool status_3 = connection.TryInsertData_3(connectionInfo.DataBase, "d9970dca_a848_45fe_af20_61792737e15a", data_1, out var msgLog6);
//if (status_3) { Console.WriteLine("Данные успешно вставлены"); }
//else { Console.WriteLine("Провал вставки данных"); }

// асинхронно
//MsgLogClass msgLog66 = null;
//bool status_33 = await connection.TryInsertData_2_async(connectionInfo.DataBase, 4, data_1, msgLog66);
//if (status_33) { Console.WriteLine("Данные асинхронно успешно вставлены"); }
//else { Console.WriteLine("Провал асинхронно вставки данных"); }


// вывод
DateTime beginDateTime = new DateTime(2024, 07, 29, 16, 20, 45);
DateTime endDateTime = DateTime.Now;
List<string> idTags = new List<string>() { "eba52b87-3c78-43c5-af1b-d4b0c3ad1d4c" };

string guid_node = "45955a91_4baa_47bc_bc72_a949200a6160";
string guid_node_formatted = guid_node.Replace('_', '-');
Guid my_node_guid = Guid.Parse(guid_node_formatted);

if (connection.TryReadHistData(connectionInfo.DataBase, my_node_guid, beginDateTime, endDateTime, idTags, out var result, out var msgLog8))
{
    foreach (string i in idTags)
    {
        Console.WriteLine($"Tag ID: {i}");
        foreach (var data in result[Guid.Parse(i.Replace('_', '-'))])
        {
            Console.WriteLine($"\tDateTime: {data.dateTime}, IsGood: {data.isGood}, Value: {data.value}");
        }
    }
}


//if(connection.TryTruncateNodeTables(connectionInfo.DataBase, 4, out var msgLog9))
//{
//    Console.WriteLine("Таблицы очищены успешно");
//}
//else { Console.WriteLine("Ошибка очистки таблиц"); }


//if (connection.TryDropNodeTables(connectionInfo.DataBase, 4, out var msgLog10))
//{
//    Console.WriteLine("Таблицы удалены успешно");
//}
//else { Console.WriteLine("Таблицы НЕ удалены"); }


//if(connection.TryDropDatabase("db_test_4", out var msgLog11)) {
//    Console.WriteLine("База данных удалена успешно");
//}
//else { Console.WriteLine("Ошибка удаления базы данных"); }



//int pointsAmount = 3; // это параметр, который определяет количество точек данных (или измерений), которые нужно извлечь из базы данных в заданном временном интервале 
//AlgSliceEnum algSlice = AlgSliceEnum.FirstPoint;
//Dictionary<int, List<DataVal>> result_5;
//if (connection.TryReadSliceHistData(connectionInfo.DataBase, 4, beginDateTime, endDateTime, idTags, pointsAmount, algSlice, out result_5, out var msgLog12))
//{
//    Console.WriteLine("Успешна работа TryReadSliceHistData");
//    foreach (var tagData in result_5)
//    {
//        Console.WriteLine($"TagId: {tagData.Key}");
//        foreach (var dataVal in tagData.Value)
//        {
//            Console.WriteLine($"DateTime: {dataVal.DateTime}, Value: {dataVal.Val}");
//        }
//    }
//}
//else { Console.WriteLine("Не успешна работа TryReadSliceHistData"); }





//var notifyEvent = (
//                dateTime: DateTime.Now,
//                detectorId: 123,
//                category: "Error",
//                status: "Active",
//                message: "An error occurred",
//                additional: "Additional info",
//                eventType: "newEvent",
//                info: "Error info",
//                trends: "No trends",
//                user: "Admin",
//                comments: "No comments",
//                history: "No history"
//            );
//int eventId = connection.TryInsertEvent(connectionInfo.DataBase, 4, notifyEvent, out var msgLog13);
//// Обработка результата
//if (eventId > 0)
//{
//    Console.WriteLine($"Event inserted successfully with ID: {eventId}");
//}
//else
//{
//    Console.WriteLine($"Error inserting event: {msgLog13.LogText}");
//    Console.WriteLine($"Details: {msgLog13.LogDetails}");
//}



//List<(int id, int detectorId, DateTime dateTime, string category, string status, string message, string additional, string eventType, string info, string trends, string user, string comments, string history)> unAckEvents;
//bool success_33 = connection.TryReadUnAckEvents(connectionInfo.DataBase, 4, out unAckEvents, out var msgLog14);
//// Обработка результата
//if (success_33)
//{
//    Console.WriteLine("Unacknowledged events read successfully:");
//    foreach (var eventData in unAckEvents)
//    {
//        Console.WriteLine($"ID: {eventData.id}, Detector ID: {eventData.detectorId}, DateTime: {eventData.dateTime}, " +
//                          $"Category: {eventData.category}, Status: {eventData.status}, Message: {eventData.message}, " +
//                          $"Additional: {eventData.additional}, EventType: {eventData.eventType}, Info: {eventData.info}, " +
//                          $"Trends: {eventData.trends}, User: {eventData.user}, Comments: {eventData.comments}, History: {eventData.history}");
//    }
//}
//else
//{
//    Console.WriteLine($"Error reading unacknowledged events: {msgLog.LogText}");
//    Console.WriteLine($"Details: {msgLog.LogDetails}");
//}





// Переменные для результата и логов
//List<(int id, int detectorId, DateTime dateTime, string category, string status, string message, string additional, string eventType, string info, string trends, string user, string comments, string history)> histEvents;
//bool success_44 = connection.TryReadHistEvents(connectionInfo.DataBase, 4, beginDateTime, endDateTime, out histEvents, out var msgLog15);
//// Обработка результата
//if (success_44)
//{
//    Console.WriteLine("Historical events read successfully:");
//    foreach (var eventData in histEvents)
//    {
//        Console.WriteLine($"ID: {eventData.id}, Detector ID: {eventData.detectorId}, DateTime: {eventData.dateTime}, " +
//                          $"Category: {eventData.category}, Status: {eventData.status}, Message: {eventData.message}, " +
//                          $"Additional: {eventData.additional}, EventType: {eventData.eventType}, Info: {eventData.info}, " +
//                          $"Trends: {eventData.trends}, User: {eventData.user}, Comments: {eventData.comments}, History: {eventData.history}");
//    }
//}
//else
//{
//    Console.WriteLine($"Error reading historical events: {msgLog.LogText}");
//    Console.WriteLine($"Details: {msgLog.LogDetails}");
//}