using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Activation
    {
        /// <summary>
        /// Создание ключа продукта
        /// </summary>
        /// <param name="productKey"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static bool GenerateProductKey(string dbSystemIdentifier, out string productKey, out string errors)
        {
            productKey = string.Empty;
            errors = string.Empty;

            try
            {
                byte[] cpuIdByte = BitConverter.GetBytes(ulong.Parse(dbSystemIdentifier));

                //Массив размером строго 8 байт
                byte[] cpuIdPad = new byte[8];

                //Если cpuId имеет длину меньше 8 байт, то заполнение до 8 байт числами 157                
                if (cpuIdByte.Length >= 8)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        cpuIdPad[i] = cpuIdByte[i];
                    }
                }
                else
                {
                    for (int i = 0; i < cpuIdByte.Length; i++)
                    {
                        cpuIdPad[i] = cpuIdByte[i];
                    }

                    for (int i = cpuIdByte.Length; i < 8; i++)
                    {
                        cpuIdPad[i] = 157;
                    }
                }


                //Дата первой установки программы (количество дней с  2000, 1, 1) 
                ushort createDate = (ushort)(DateTime.Now - new DateTime(2000, 1, 1)).Days;
                byte[] createDateByte = BitConverter.GetBytes(createDate);

                //Ключ продукта
                byte[] productKeyByte = new byte[10];
                for (int i = 0; i < cpuIdPad.Length; i++)
                {
                    productKeyByte[i] = cpuIdPad[i];
                }
                productKeyByte[8] = createDateByte[0]; //дата создания записывается в конец
                productKeyByte[9] = createDateByte[1];

                string productKeyHex = BitConverter.ToString(productKeyByte);

                productKey = productKeyHex;
                return true;
            }
            catch (Exception ex)
            {
                errors = "ошибка создания ключа продукта: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Проверка ключа продукта
        /// </summary>
        /// <param name="productKey"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static bool VerifyProductKey(string dbSystemIdentifier, string productKey, out string errors)
        {
            errors = string.Empty;

            try
            {
                byte[] cpuIdByte = BitConverter.GetBytes(ulong.Parse(dbSystemIdentifier));

                //Массив размером строго 8 байт
                byte[] cpuIdPad = new byte[8];

                //Если cpuId имеет длину меньше 8 байт, то заполнение до 8 байт числами 157                
                if (cpuIdByte.Length >= 8)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        cpuIdPad[i] = cpuIdByte[i];
                    }
                }
                else
                {
                    for (int i = 0; i < cpuIdByte.Length; i++)
                    {
                        cpuIdPad[i] = cpuIdByte[i];
                    }

                    for (int i = cpuIdByte.Length; i < 8; i++)
                    {
                        cpuIdPad[i] = 157;
                    }
                }

                //Проверка ключа продукта
                string prodKeyHex = productKey.Replace("-", "");
                if (prodKeyHex.Length != 20)
                {
                    errors = "неверно указан ключ продукта";
                    return false;
                }

                byte[] prodKeyByte;
                try
                {
                    prodKeyByte = StringToByteArray(prodKeyHex);
                }
                catch (Exception)
                {
                    errors = "неверно указан ключ продукта";
                    return false;
                }

                //Выделение из prodKey даты первого запуска программы
                byte[] createDateByte = new byte[2];
                createDateByte[0] = prodKeyByte[8];
                createDateByte[1] = prodKeyByte[9];
                ushort[] createDate = new ushort[1];
                Buffer.BlockCopy(createDateByte, 0, createDate, 0, 2);

                //Проверка соответствия ключа вычесленного и переданного ключа продукта
                for (int i = 0; i < 8; i++)
                {
                    if (cpuIdPad[i] != prodKeyByte[i])
                    {
                        errors = "недостоверный ключ продукта";
                        return false;
                    }
                }

                //Проверка соответствия времени первого запуска программы
                if (new DateTime(2000, 1, 1).AddDays((double)createDate[0]) > DateTime.Now) //временя первого запуска программы больше чем текущее (такого быть не может)
                {
                    errors = "недостоверное время первого запуска программы";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                errors = "ошибка проверки ключа продукта: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Проверка активации программы
        /// </summary>
        /// <param name="prodKey"></param>
        /// <param name="actKey"></param>
        /// <param name="isDemoMode"></param>
        /// <param name="isLimitedDate"></param>
        /// <param name="limitedDate"></param>
        /// <param name="isLimitedTagsCount"></param>
        /// <param name="limitedTagsCount"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static bool VerifyActivation(string prodKey, string actKey, out bool isDemoMode, out bool isLimitedDate, out DateTime limitedDate, out bool isLimitedTagsCount, out int limitedTagsCount, out string errors)
        {
            isDemoMode = false;
            isLimitedDate = false;
            limitedDate = new DateTime();
            isLimitedTagsCount = false;
            limitedTagsCount = 0;

            try
            {
                string prodKeyHex = prodKey.Replace("-", "");
                if (prodKeyHex.Length != 20)
                {
                    errors = "неверно указан ключ продукта";
                    return false;
                }

                byte[] prodKeyByte;
                try
                {
                    prodKeyByte = StringToByteArray(prodKeyHex);
                }
                catch (Exception)
                {
                    errors = "неверно указан ключ продукта";
                    return false;
                }

                byte[] createDateByte = new byte[2];
                createDateByte[0] = prodKeyByte[8];
                createDateByte[1] = prodKeyByte[9];
                ushort[] createDate = new ushort[1];
                Buffer.BlockCopy(createDateByte, 0, createDate, 0, 2);

                if (string.IsNullOrEmpty(actKey))
                {
                    isDemoMode = true;
                    isLimitedDate = true;
                    limitedDate = (new DateTime(2000, 1, 1)).AddDays((double)createDate[0]).AddDays(30);
                    errors = string.Empty;
                    return true;
                }

                string actKeyHex = actKey.Replace("-", "");
                if (actKeyHex.Length != 18)
                {
                    errors = "неверно указан ключ активации";
                    return false;
                }

                byte[] actKeyByte;
                try
                {
                    actKeyByte = StringToByteArray(actKeyHex);
                }
                catch (Exception)
                {
                    errors = "неверно указан ключ активации";
                    return false;
                }

                byte[] prodKeyHalf1 = new byte[5];
                byte[] prodKeyHalf2 = new byte[5];

                for (int i = 0; i < 5; i++)
                {
                    prodKeyHalf1[i] = prodKeyByte[i];
                    prodKeyHalf2[i] = prodKeyByte[i + 5];
                }

                byte[] xorProdKeyHalf = new byte[5];
                for (int i = 0; i < 5; i++)
                {
                    xorProdKeyHalf[i] = (byte)(prodKeyHalf1[i] ^ prodKeyHalf2[i]);
                }

                byte[] ProdKeyStep1 = new byte[9];

                ProdKeyStep1[0] = (byte)(actKeyByte[0] ^ 12);
                ProdKeyStep1[1] = (byte)(actKeyByte[1] ^ 217);
                ProdKeyStep1[2] = (byte)(actKeyByte[2] ^ 123);
                ProdKeyStep1[3] = (byte)(actKeyByte[3] ^ 184);
                ProdKeyStep1[4] = (byte)(actKeyByte[4] ^ 223);
                ProdKeyStep1[5] = (byte)(actKeyByte[5] ^ 168);
                ProdKeyStep1[6] = (byte)(actKeyByte[6] ^ 98);
                ProdKeyStep1[7] = (byte)(actKeyByte[7] ^ 72);
                ProdKeyStep1[8] = (byte)(actKeyByte[8] ^ 121);

                byte[] ProdKeyStep2 = new byte[9];
                byte[] swapKey = { 30, 50, 5, 60, 19, 24, 44, 65, 17, 31, 47, 27, 39, 26, 18, 28, 7, 11, 41, 56, 25, 63, 8, 9, 62, 45, 35, 6, 42, 10, 20, 2, 16, 21, 70, 55, 57, 14, 1, 3, 29, 15, 64, 12, 22, 36, 49, 71, 66, 23, 61, 40, 58, 34, 4, 51, 37, 48, 69, 68, 53, 52, 13, 59, 46, 32, 38, 0, 33, 67, 54, 43 };
                BitArray bits = new BitArray(ProdKeyStep1);
                BitArray bits2 = new BitArray(72);
                for (int i = 0; i < swapKey.Length; i++)
                {
                    bits2[swapKey[i]] = bits[i];
                }
                bits2.CopyTo(ProdKeyStep2, 0);

                for (int i = 0; i < 5; i++)
                {
                    if (xorProdKeyHalf[i] != ProdKeyStep2[i])
                    {
                        errors = "указан недостоверный ключ активации";
                        return false;
                    }
                }

                byte[] licensePerByte = new byte[2];
                licensePerByte[0] = ProdKeyStep2[5];
                licensePerByte[1] = ProdKeyStep2[6];
                ushort[] licensePer = new ushort[1];
                Buffer.BlockCopy(licensePerByte, 0, licensePer, 0, 2);

                if (licensePer[0] > 33000)
                {
                    errors = "указан недостоверный ключ активации";
                    return false;
                }

                if (licensePer[0] % 3 != 0)
                {
                    errors = "указан недостоверный ключ активации";
                    return false;
                }

                if (licensePer[0] < 33000)
                {
                    isLimitedDate = true;
                    limitedDate = (new DateTime(2020, 1, 1)).AddDays((double)licensePer[0] / 3);
                }

                byte[] licenseTagsByte = new byte[2];
                licenseTagsByte[0] = ProdKeyStep2[7];
                licenseTagsByte[1] = ProdKeyStep2[8];
                ushort[] licenseTags = new ushort[1];
                Buffer.BlockCopy(licenseTagsByte, 0, licenseTags, 0, 2);

                if (licenseTags[0] > 50000)
                {
                    errors = "указан недостоверный ключ активации";
                    return false;
                }

                if (licenseTags[0] < 50000)
                {
                    if (licenseTags[0] % 100 != 0)
                    {
                        errors = "указан недостоверный ключ активации";
                        return false;
                    }
                    else
                    {
                        isLimitedTagsCount = true;
                        limitedTagsCount = licenseTags[0];
                    }
                }

                errors = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                errors = $"ошибка активации: {ex.Message}";
                return false;
            }
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
