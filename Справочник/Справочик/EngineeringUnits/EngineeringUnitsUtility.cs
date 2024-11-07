using System;
using System.Collections.Generic;
using Npgsql;
using ClosedXML.Excel;
using System.IO;
using Common.MsgLog;
using Configurator.Models.CommonModels.MsgLog;
using Common.CommonModels;
using Microsoft.Extensions.Logging;



namespace Configurator.Models.Main.EngineeringUnits
{
    public static class EngineeringUnitsUtility
    {
        private static string connectionString;

        //public static void SetConnectionString(string host, string username, string password, string database)
        //{
        //    connectionString = $"Host={host};Username={username};Password={password};Database={database}";
        //}

        //public static void SetConnectionString(string conn)
        //{
        //    connectionString = conn;
        //}

        /// <summary>
        /// Устанавливаем поле connectionString
        /// </summary>
        public static void SetConnectionString(DataBaseInfo info)
        {
            connectionString = $"Host={info.Host};Username={info.User};Password={info.Password};Database={info.DataBase}";
        }

        /// <summary>
        /// Парсим данные о базовых единицах из файла
        /// </summary>
        public static List<BasicUnit> ParseBasicUnits(string filePath)
        {
            List<BasicUnit> units = new List<BasicUnit>();

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(',');

                var unit = new BasicUnit
                {
                    // Id = counter++,
                    CollectionId = 4,
                    //AssetId = Guid.NewGuid(),
                    //AssetGroupId = Guid.NewGuid(),
                    UnitGroupId = int.Parse(parts[0]),
                    Name = parts[1],
                    National = parts[2],
                    International = parts[3],
                    NationalCode = parts[4],
                    InternationalCode = parts[5],
                    Code = parts[6]
                };
                units.Add(unit);
            }
            return units;
        }


        /// <summary>
        /// Парсим данные о производных единицах из файла
        /// </summary>
        public static List<DerivedsUnit> ParseDerivedsUnits(string filePath)
        {

            var units = new List<DerivedsUnit>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1); // указываем лист
                var rows = worksheet.RangeUsed().RowsUsed(); // Получаем все строки с данными

                foreach (var row in rows)
                {
                    if (row.RowNumber() == 1) continue; // Пропустить заголовок, если он есть

                    var unit = new DerivedsUnit
                    {
                        CollectionId = 5,
                        //AssetId = Guid.NewGuid(),
                        //AssetGroupId = Guid.NewGuid(),                
                        //UnitGroupId = UnitGroupDict[NameBasicUnit],
                        //UnitBasicId = BasicUnitDict[NameBasicUnit],
                        Name = row.Cell(3).GetValue<string>(),
                        National = row.Cell(4).GetValue<string>(),
                        International = row.Cell(5).GetValue<string>(),
                        NationalCode = row.Cell(6).GetValue<string>(),
                        InternationalCode = row.Cell(7).GetValue<string>(),
                        Code = row.Cell(2).GetValue<string>(),
                        ConvertK = row.Cell(8).GetValue<double>(),
                        ConvertB = row.Cell(9).GetValue<double>(),
                        UnitBasicName = row.Cell(10).GetValue<string>() // имя базовой единицы
                    };
                    units.Add(unit);
                }

            }

            //// Вывод для проверки всех атрибутов
            //foreach (var unit in units)
            //{
            //    Console.WriteLine($"CollectionId: {unit.CollectionId}, " +
            //                      $"AssetId: {unit.AssetId}, " +
            //                      $"AssetGroupId: {unit.AssetGroupId}, " +
            //                      $"UnitGroupId: {unit.UnitGroupId}, " +
            //                      $"UnitBasicId: {unit.UnitBasicId}, " +
            //                      $"Name: {unit.Name}, " +
            //                      $"National: {unit.National}, " +
            //                      $"International: {unit.International}, " +
            //                      $"NationalCode: {unit.NationalCode}, " +
            //                      $"InternationalCode: {unit.InternationalCode}, " +
            //                      $"Code: {unit.Code}, " +
            //                      $"ConvertK: {unit.ConvertK}, " +
            //                      $"ConvertB: {unit.ConvertB}");
            //}

            return units;
        }

        /// <summary>
        /// Парсим данные о группах единиц измерения из файла
        /// </summary>
        public static List<UnitGroup> ParseUnitGroups(string filePath)
        {
            var units = new List<UnitGroup>();

            int counter = 1;
            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(',');


                var unit = new UnitGroup
                {
                    Id = counter++,
                    CollectionId = 6,
                    GroupName = parts[4]
                };
                units.Add(unit);


            }

            return units;
        }

        /// <summary>
        /// Проверяем наличие записи о базовой единице в таблице UnitBasics
        /// </summary>
        public static bool RecordBasicExists(string name)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand($"SELECT COUNT(1) FROM \"AssetsInfoSmartIndustry\".\"UnitBasics\" WHERE \"Name\" = \'{name}\'", connection))
                {
                    var count = (long)cmd.ExecuteScalar();
                    return count > 0; // Если больше 0, значит запись существует
                }
            }
        }

        /// <summary>
        /// Проверяем наличие записи о производной единице в таблице UnitDeriveds
        /// </summary>
        public static bool RecordDerivedsExists(string name, string code)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand($"SELECT COUNT(1) FROM \"AssetsInfoSmartIndustry\".\"UnitDeriveds\" WHERE \"Name\" = \'{name}\' and \"Code\" = \'{code}\'", connection))
                {
                    var count = (long)cmd.ExecuteScalar();
                    return count > 0; // Если больше 0, значит запись существует
                }
            }
        }

        /// <summary>
        /// Производим вставку базовых единиц в БД
        /// </summary>
        public static void InsertBasicUnits(List<BasicUnit> units)
        {
            foreach (var unit in units)
            {
                if (!RecordBasicExists(unit.Name))
                {
                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        using (var cmd = new NpgsqlCommand("INSERT INTO \"AssetsInfoSmartIndustry\".\"UnitBasics\"" +
                            "(\"CollectionId\", \"AssetId\", \"AssetGroupId\", \"UnitGroupId\", \"Name\", \"National\", \"International\", \"NationalCode\", \"InternationalCode\", \"Code\")" +
                            "VALUES (@CollectionId, NULL, NULL, @UnitGroupId, @Name, @National, @International, @NationalCode, @InternationalCode, @Code)", connection))
                        {
                            //cmd.Parameters.AddWithValue("Id", unit.Id);
                            cmd.Parameters.AddWithValue("CollectionId", unit.CollectionId);
                            //cmd.Parameters.AddWithValue("AssetId", unit.AssetId);
                            //cmd.Parameters.AddWithValue("AssetGroupId",unit.AssetGroupId);
                            cmd.Parameters.AddWithValue("UnitGroupId", unit.UnitGroupId);
                            cmd.Parameters.AddWithValue("Name", unit.Name);
                            cmd.Parameters.AddWithValue("National", unit.National);
                            cmd.Parameters.AddWithValue("International", unit.International);
                            cmd.Parameters.AddWithValue("NationalCode", unit.NationalCode);
                            cmd.Parameters.AddWithValue("InternationalCode", unit.InternationalCode);
                            cmd.Parameters.AddWithValue("Code", unit.Code);

                            cmd.ExecuteNonQuery();
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Производим вставку производных единиц в БД
        /// </summary>
        public static void InsertDerivedsUnits(List<DerivedsUnit> units)
        {
            foreach (var unit in units)
            {
                if (!RecordDerivedsExists(unit.Name, unit.Code))
                {
                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        // берем UnitGroupId и UnitBasicId из таблицы базовых единиц
                        using (var cmd = new NpgsqlCommand($"SELECT \"UnitGroupId\", \"Id\" FROM \"AssetsInfoSmartIndustry\".\"UnitBasics\" WHERE \"Name\" = \'{unit.UnitBasicName}\'", connection))
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string UnitGroupId = reader["UnitGroupId"].ToString();
                                    string UnitBasicId = reader["Id"].ToString();
                                    unit.UnitGroupId = int.Parse(UnitGroupId);
                                    unit.UnitBasicId = int.Parse(UnitBasicId);
                                }
                            }
                        }

                        using (var cmd = new NpgsqlCommand("INSERT INTO \"AssetsInfoSmartIndustry\".\"UnitDeriveds\"" +
                                "(\"CollectionId\", \"AssetId\", \"AssetGroupId\", \"UnitGroupId\", \"UnitBasicId\", \"Name\", \"National\", \"International\", \"NationalCode\", \"InternationalCode\", \"Code\", \"ConvertK\", \"ConvertB\")" +
                                "VALUES (@CollectionId, NULL, NULL, @UnitGroupId, @UnitBasicId, @Name, @National, @International, @NationalCode, @InternationalCode, @Code, @ConvertK, @ConvertB)", connection))
                        {
                            //cmd.Parameters.AddWithValue("Id", unit.Id);
                            cmd.Parameters.AddWithValue("CollectionId", unit.CollectionId);
                            //cmd.Parameters.AddWithValue("AssetId", AssetId);
                            //cmd.Parameters.AddWithValue("AssetGroupId", unit.AssetGroupId);
                            cmd.Parameters.AddWithValue("UnitGroupId", unit.UnitGroupId);
                            cmd.Parameters.AddWithValue("UnitBasicId", unit.UnitBasicId);
                            cmd.Parameters.AddWithValue("Name", unit.Name);
                            cmd.Parameters.AddWithValue("National", unit.National);
                            cmd.Parameters.AddWithValue("International", unit.International);
                            cmd.Parameters.AddWithValue("NationalCode", unit.NationalCode);
                            cmd.Parameters.AddWithValue("InternationalCode", unit.InternationalCode);
                            cmd.Parameters.AddWithValue("Code", unit.Code);
                            cmd.Parameters.AddWithValue("ConvertK", unit.ConvertK);
                            cmd.Parameters.AddWithValue("ConvertB", unit.ConvertB);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Производим вставку групп инжнерных единиц в БД
        /// </summary>
        public static void InsertUnitGroups(List<UnitGroup> units)
        {
            foreach (var unit in units)
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("INSERT INTO \"AssetsInfoSmartIndustry\".\"UnitGroups\"(\"Id\", \"CollectionId\", \"AssetId\", \"AssetGroupId\", \"GroupName\")" +
                        "VALUES (@Id, @CollectionId, NULL, NULL, @GroupName);", connection))
                    {
                        cmd.Parameters.AddWithValue("Id", unit.Id);
                        cmd.Parameters.AddWithValue("CollectionId", unit.CollectionId);
                        cmd.Parameters.AddWithValue("GroupName", unit.GroupName);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Проверяем соединение с базой данных
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Соединение установлено успешно.");
                    return true;
                }
            }
            catch (NpgsqlException ex)
            {
                // Ошибка подключения к базе данных
                MsgLogShow.ShowMsg(new MsgLogClass() 
                { LogText =  $"Ошибка подключения к БД: {ex.Message}", TypeLog = LogLevel.Error });
                return false;
            }
            catch (Exception ex)
            {
                // Любая другая ошибка
                MsgLogShow.ShowMsg(new MsgLogClass()
                { LogText = $"Произошла ошибка: {ex.Message}", TypeLog = LogLevel.Error });
                return false;
            }

        }

        /// <summary>
        /// Проверяем наличие записей о группах единиц в таблице UnitGroups
        /// </summary>
        public static bool TestEmptyUnitGroups()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT COUNT(1) FROM  \"AssetsInfoSmartIndustry\".\"UnitGroups\"", connection))
                {
                    long res = (long)cmd.ExecuteScalar();
                    return res == 0;
                }
            }
        }

        /// <summary>
        /// Основной метод вставки инженерных единиц измерения
        /// </summary>
        public static bool InsertUnits() {

            SetConnectionString(StaticData.ProjDBInfo);

            string appDataPath = AppDomain.CurrentDomain.BaseDirectory; //  возвращает путь к папке, в которой находится исполняемый файл программы.

            if (TestConnection() == true)
            {
                // проверка на существование записей в UnitGroups
                if (TestEmptyUnitGroups() == true)
                {               
                    string groups = @"Assets\FilesEngineeringUnits\UnitGroups.csv";
                    string groups_path = Path.Combine(appDataPath, groups);

                    var UnitGroups = ParseUnitGroups(groups_path);
                    InsertUnitGroups(UnitGroups);

                    MsgLogShow.ShowMsg(new MsgLogClass()
                    { LogText = "Таблица UnitGroups пустая, выполняю заполнение этой таблицы. Повторите попытку заполнения", TypeLog = LogLevel.Warning });

                    return false;
                }
                else
                {
                    string basic_path = Path.Combine(appDataPath, @"Assets\FilesEngineeringUnits\UnitBasic.csv");
                    string deriveds_path = Path.Combine(appDataPath, @"Assets\FilesEngineeringUnits\UnitDerived.xlsx");

                    var Basicunits = ParseBasicUnits(basic_path);
                    InsertBasicUnits(Basicunits);

                    List<DerivedsUnit> DerivedsUnits = ParseDerivedsUnits(deriveds_path);
                    InsertDerivedsUnits(DerivedsUnits);
                }
                return true;
            }
            else {
                MsgLogShow.ShowMsg(new MsgLogClass()
                { LogText = "Нет соединения с базой", TypeLog = LogLevel.Error });
                return false; 
            }

        }

    }
}
