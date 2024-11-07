using Common.MsgLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common
{
    public static class Serializator
    {
        /// <summary>
        /// Бинарная сериализация
        /// </summary>
        /// <typeparam name="Object"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string Serialize<Object>(Object dictionary)
        {
            string ret = string.Empty;
            try // try to serialize the collection to a file
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, dictionary);

                    byte[] bb = stream.ToArray();
                    ret = Convert.ToBase64String(bb);
                }
                return ret;

            }
            catch (Exception ex)
            {
               // ShowMsg(new MsgLogClass() { LogText = ListMsg.SystemError, LogDetails = ex.ToString() });
                return ret;
            }
        }

        /// <summary>
        /// Бинарная десериализация
        /// </summary>
        /// <typeparam name="Object"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Object Deserialize<Object>(string text) where Object : new()
        {
            Object ret = CreateInstance<Object>();
            try
            {
                byte[] bb = Convert.FromBase64String(text);

                using (MemoryStream stream = new MemoryStream(bb))
                {
                    // create BinaryFormatter
                    BinaryFormatter bin = new BinaryFormatter();
                    // deserialize the collection (Employee) from file (stream)
                    ret = (Object)bin.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
               // MsgLog.ShowMsg($"Возникла ошибка десериализации: {ex.Message}", TypeMsg.Error);
            }
            return ret;
        }

        /// <summary>
        /// Бинарная десериализация 
        /// </summary>
        public static Object Deserialize<Object>(string text, out bool result)
        {
            Object ret = default;
            result = true;
            try
            {
                byte[] bb = Convert.FromBase64String(text);

                using (MemoryStream stream = new MemoryStream(bb))
                {
                    // create BinaryFormatter
                    BinaryFormatter bin = new BinaryFormatter();
                    // deserialize the collection (Employee) from file (stream)
                    ret = (Object)bin.Deserialize(stream);
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return ret;
        }

        /// <summary>
        /// XML десериализация
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toDeserialize"></param>
        /// <returns></returns>
        public static T DeserializeXml<T>(this string toDeserialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader textReader = new StringReader(toDeserialize))
            {
                return (T)xmlSerializer.Deserialize(textReader);
            }
        }

        /// <summary>
        /// XML сериализация
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toSerialize"></param>
        /// <returns></returns>
        public static string SerializeXml<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        // function to create instance of T
        public static Object CreateInstance<Object>() where Object : new()
        {
            return (Object)Activator.CreateInstance(typeof(Object));
        }

        public static string StringToHex(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        public static string HexToString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.Unicode.GetString(bytes); 
        }
    }
}

