using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using RedSia.Utils;
using RedSiaCore.Utils;


namespace RedSiaCore.XML
{
    /// <summary>
    /// параметры команды speak IPT модуля в XML
    /// </summary>
    public class XmlSpeak
    {
        /// <summary>
        /// Группа слов на один вызов (различные фразы)
        /// </summary>
        public string Group;

        /// <summary>
        /// Отношение, применимое к пользователю
        /// </summary>
        public int Relation;

        /// <summary>
        /// Фраза
        /// </summary>
        public string Value;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="relation">Отношение, применимое к пользователю</param>
        /// <param name="group">Группа слов на один вызов (различные фразы)</param>
        /// <param name="value">Фраза</param>
        public XmlSpeak(int relation, string group, string value)
        {
            Relation = relation;
            Group = group;
            Value = value;
        }
    }

    /// <summary>
    /// Параметры вызова IPT моудя
    /// </summary>
    public class XmlCall
    {
        /// <summary>
        /// Фраза
        /// </summary>
        public string[] Values;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="value">Фраза</param>
        public XmlCall(string value)
        {
            Values = value.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    /// <summary>
    /// Параметры IPT модуля
    /// </summary>
    public class XmlTranslator
    {
        /// <summary>
        /// Список вызовов модуля
        /// </summary>
        public List<XmlCall> Calls;

        /// <summary>
        /// Библиотека-обработчик
        /// </summary>
        public string Interpreter;

        /// <summary>
        /// Имя модуля (без названия интерфейса)
        /// </summary>
        public string Name;

        /// <summary>
        /// Список команд speak
        /// </summary>
        public List<XmlSpeak> Speaks;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя модуля (без названия интерфейса)</param>
        /// <param name="interpreter">Библиотека-обработчик</param>
        public XmlTranslator(string name, string interpreter)
        {
            Name = name;
            Interpreter = interpreter;
            Calls = new List<XmlCall>();
            Speaks = new List<XmlSpeak>();
        }

        /// <summary>
        /// Возвращает параметры команды speak
        /// </summary>
        /// <param name="group">группа фразы</param>
        /// <returns></returns>
        public XmlSpeak GetSpeakByGroup(string group)
        {
            XmlSpeak ret;
            //  поиск запршиваемой группы фраз
            var speaks = Speaks.Where(xmlSpeak => xmlSpeak.Group == @group).ToList();
            // присваивание случайной фразы из группы
            if (speaks != null && speaks.Count == 0) return null;
            ret = speaks[SiaRandom.Rand.Next(0, speaks.Count)];
            return ret;
        }
    }

    /// <summary>
    /// Параметры первоначальной загрузки модулей
    /// </summary>
    public class XmlLoad
    {
        /// <summary>
        /// Тип моудля (IPT, IPAT, IST)
        /// </summary>
        public string Type;

        /// <summary>
        /// Имя модуля (без названия интерфейса)
        /// </summary>
        public string Name;

        /// <summary>
        /// Назначение (SE - общий, ET - Эмоциональная окраска, UN - универсальный)
        /// </summary>
        public string Destination;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type">Тип моудля (IPT, IPAT, IST)</param>
        /// <param name="name">Имя модуля (без названия интерфейса)</param>
        /// <param name="destination">Назначение (SE - общий, ET - Эмоциональная окраска)</param>
        public XmlLoad(string type, string name, string destination)
        {
            Type = type;
            Name = name;
            Destination = destination;
        }
    }

    /// <summary>
    /// Работает с XML конфигурацией
    /// </summary>
    public static class SiaXml
    {
        /// <summary>
        /// Возвращает параметры IPT модуля по имени
        /// </summary>
        /// <param name="name">Имя модуля (без названия интерфейса)</param>
        /// <returns></returns>
        public static XmlTranslator GetTranslator(string name)
        {
            XmlTranslator xmlTranslator = null;

            var xmldoc = new XmlDataDocument();
            XmlNodeList xmlnode;
            int i;
            var fs = new FileStream(Environment.CurrentDirectory + "/Configs/Translators.xml",
                FileMode.OpenOrCreate, FileAccess.Read);
            xmldoc.Load(fs);
            xmlnode = xmldoc.GetElementsByTagName("translator");
            for (i = 0; i <= xmlnode.Count - 1; i++)
            {
                try
                {
                    var xmlAttributeCollection = xmlnode[i].Attributes;
                    if (xmlAttributeCollection != null &&
                        xmlAttributeCollection.Count > 0 &&
                        xmlAttributeCollection["name"] != null &&
                        xmlAttributeCollection["interpreter"] != null &&
                        xmlAttributeCollection["name"].Value == name)
                    {
                        xmlTranslator = new XmlTranslator(xmlAttributeCollection["name"].Value,
                            xmlAttributeCollection["interpreter"].Value);

                        foreach (XmlNode childNode in xmlnode[i].ChildNodes)
                        {
                            if (childNode.Name == "call" && childNode.Attributes != null &&
                                childNode.Attributes["value"] != null)
                                xmlTranslator.Calls.Add(new XmlCall(childNode.Attributes["value"].Value.ToLower()));

                            if (childNode.Name == "speak" && childNode.Attributes != null &&
                                childNode.Attributes["value"] != null &&
                                childNode.Attributes["relation"] != null &&
                                childNode.Attributes["group"] != null &&
                                Convert.ToInt32(childNode.Attributes["relation"].Value) == EmoTone.EmoTone.Tone)
                                xmlTranslator.Speaks.Add(
                                    new XmlSpeak(Convert.ToInt32(childNode.Attributes["relation"].Value.ToLower()),
                                        childNode.Attributes["group"].Value.ToLower(),
                                        childNode.Attributes["value"].Value.ToLower()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                    
                }
               
            }
            return xmlTranslator;
        }

        /// <summary>
        /// Возвращает список параметров первоначальной загрузки модулей
        /// </summary>
        /// <param name="type">Тип моудля (IPT, IPAT, IST)</param>
        /// <param name="destination">Назначение (SE - общий, ET - Эмоциональная окраска, UN - универсальный)</param>
        /// <returns></returns>
        public static XmlLoad[] GetLoads(string type, string destination)
        {
            type = type.ToLower();
            destination = destination.ToLower();
            List<XmlLoad> loads = new List<XmlLoad>();

            var xmldoc = new XmlDataDocument();
            int i;
            var fs = new FileStream(Environment.CurrentDirectory + "/Configs/Translators.xml",
                FileMode.OpenOrCreate, FileAccess.Read);
            xmldoc.Load(fs);
            var xmlnode = xmldoc.GetElementsByTagName("load");
            for (i = 0; i <= xmlnode.Count - 1; i++)
            {
                var xmlAttributeCollection = xmlnode[i].Attributes;
                if (xmlAttributeCollection != null &&
                    xmlAttributeCollection.Count > 0 &&
                    xmlAttributeCollection["type"] != null &&
                    xmlAttributeCollection["name"] != null &&
                    xmlAttributeCollection["destination"] != null &&
                    xmlAttributeCollection["type"].Value.ToLower() == type &&
                    (xmlAttributeCollection["destination"].Value.ToLower() == destination || destination == "un"))
                {
                    loads.Add(new XmlLoad(xmlAttributeCollection["type"].Value, 
                        xmlAttributeCollection["name"].Value, 
                        xmlAttributeCollection["destination"].Value));
                }
            }

            return loads.ToArray();
        }

    }
}