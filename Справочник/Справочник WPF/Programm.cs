using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using ClosedXML.Excel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Spravochnik_Dima_WPF_2
{


    // Определим класс для хранения данных о единицах измерения
    public class BasicUnit
    {
        // public int Id { get; set; }
        public int CollectionId { get; set; }
        public Guid? AssetId { get; set; }
        public Guid? AssetGroupId { get; set; }
        public int UnitGroupId { get; set; }
        public string Name { get; set; }
        public string National { get; set; }
        public string? International { get; set; }
        public string? NationalCode { get; set; }
        public string? InternationalCode { get; set; }
        public string? Code { get; set; }
    }

    public class DerivedsUnit
    {
        public int CollectionId { get; set; }
        public Guid? AssetId { get; set; }
        public Guid? AssetGroupId { get; set; }
        public int UnitGroupId { get; set; }
        public int UnitBasicId { get; set; }
        public string Name { get; set; }
        public string National { get; set; }
        public string? International { get; set; }
        public string? NationalCode { get; set; }
        public string? InternationalCode { get; set; }
        public string? Code { get; set; }
        public double ConvertK { get; set; }
        public double ConvertB { get; set; }
        public string UnitBasicName { get; set; }

    }

    public class UnitGroups { 
        public int Id { get; set; }
        public int CollectionId { get; set; }
        public Guid? AssetId { get; set; }       
        public Guid? AssetGroupId { get; set;}
        public string GroupName { get; set; }
    }




    public class Program
    {
        private static string connectionString;

        public static void SetConnectionString(string host, string username, string password, string database)
        {
            connectionString = $"Host={host};Username={username};Password={password};Database={database}";
        }

        public static void SetConnectionString(string conn)
        {
            connectionString = conn;
        }

        public static List<BasicUnit> ParseBasicUnits(string filePath)
        {
            List<BasicUnit> units = new List<BasicUnit>();

            //int counter = 1;

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

        public static List<DerivedsUnit> ParseDerivedsUnits(string filePath)
        {

            var units = new List<DerivedsUnit>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1); // указываем лист
                var rows = worksheet.RangeUsed().RowsUsed(); // Получаем все строки с данными

                //int counter = 0;
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

                    //counter++;
                    //if (counter == 35)
                    //{
                    //    break;
                    //}
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

        public static List<UnitGroups> ParseUnitGroups(string filePath)
        {
            var units = new List<UnitGroups>();

            int counter = 1;
            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(',');

               
                var unit = new UnitGroups
                {
                    Id = counter++,
                    CollectionId = 6,
                    GroupName = parts[4]
                };
                units.Add(unit);

               
            }

            return units;
        }


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

        public static void InsertUnitGroups(List<UnitGroups> units)
        {
            foreach (var unit in units)
            {
                using(var connection  = new NpgsqlConnection(connectionString))
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

        public static bool TestConnection()
        {
            try
            {
                using(var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Соединение установлено успешно.");
                    return true;
                }
            }
            catch(NpgsqlException ex) {
                // Ошибка подключения к базе данных
                Console.WriteLine($"Ошибка подключения к БД: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Любая другая ошибка
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                return false;
            }

        }

        public static bool TestEmptyUnitGroups()
        {
            using(var connection = new NpgsqlConnection(connectionString)) {
                connection.Open();
                
                using(var cmd = new NpgsqlCommand("SELECT COUNT(1) FROM  \"AssetsInfoSmartIndustry\".\"UnitGroups\"", connection)) {
                    long res = (long)cmd.ExecuteScalar();
                    return !(res > 0);
                }
            }
        }




        //public static void Main(string[] args)
        //{
        //    string basic_path = @"C:\Users\Dmitriy.Simonov\Desktop\C#\Справочник_Шамиль\Базовые переменные\1.4.txt";
        //    string deriveds_path = @"C:\Users\Dmitriy.Simonov\Desktop\C#\Справочник_Шамиль\Производные переменные\okei(ver3).xlsx";


        //    //SetConnectionString("localhost", "postgres", "postgres", "PA_Project");



        //    var Basicunits = ParseBasicUnits(basic_path);
        //    InsertBasicUnits(Basicunits);

        //    List<DerivedsUnit> DerivedsUnits = ParseDerivedsUnits(deriveds_path);
        //    InsertDerivedsUnits(DerivedsUnits);
        //}

    }



}
