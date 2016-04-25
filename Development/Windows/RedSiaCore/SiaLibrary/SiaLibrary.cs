using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using RedSiaCore.Core;
using RedSiaCore.IPAT;
using RedSiaCore.IPT;
using RedSiaCore.IST;
using RedSiaCore.XML;

namespace RedSiaCore.SiaLibrary
{
    /// <summary>
    /// Библиотека модулей
    /// </summary>
    public static class SiaLibrary
    {
        /// <summary>
        /// Все IST обработчики
        /// </summary>
        public static List<ISiaTranslator> SiaTranslators = new List<ISiaTranslator>();

        /// <summary>
        /// Все IPT обработчики
        /// </summary>
        public static List<IPhraseTranslator> PhraseTranslators = new List<IPhraseTranslator>();

        /// <summary>
        /// Все IPAT обработчики
        /// </summary>
        public static List<IPhraseAdditionalTranslator> AdditionalTranslators = new List<IPhraseAdditionalTranslator>();

        /// <summary>
        /// Назначение (SE - общий, ET - Эмоциональная окраска)
        /// </summary>
        public static string Mode = null;

        /// <summary>
        /// Инициализация библиотеки модулей
        /// </summary>
        /// <param name="mode">Назначение (SE - общий, ET - Эмоциональная окраска)</param>
        public static void Initialize(string mode)
        {
            // переопределение текущего назначения с целью
            // избежания повторного заполнения
            Mode = mode;

            // если IST не загружены
            if (SiaTranslators.Count == 0)
            {
                foreach (FileInfo file in new DirectoryInfo(Environment.CurrentDirectory).GetFiles())
                {
                    // находим библиотеку
                    if (Path.GetExtension(file.FullName) == ".dll")
                    {
                        Assembly a = Assembly.Load(Path.GetFileNameWithoutExtension(file.FullName));


                        foreach (var module in a.GetModules())
                        {
                            foreach (Type type in module.GetTypes())
                            {
                                // если был найден
                                if (type.BaseType != null && type.BaseType.FullName == "RedSiaCore.IST.AbstractSiaTranslator")
                                {
                                    try
                                    {
                                        // инициализируем его
                                        ISiaTranslator ipt = Activator.CreateInstance(type, new object[] { null }) as ISiaTranslator;
                                        // и добавляем в список
                                        SiaTranslators.Add(ipt);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                        }

                        

                    }
                }
            }

            if (PhraseTranslators.Count == 0)
            {
                foreach (FileInfo file in new DirectoryInfo(Environment.CurrentDirectory).GetFiles())
                {
                    if (Path.GetExtension(file.FullName) == ".dll")
                    {
                        Assembly a = Assembly.Load(Path.GetFileNameWithoutExtension(file.FullName));

                        foreach (var module in a.GetModules())
                        {
                            foreach (Type type in module.GetTypes())
                            {
                                if (type.BaseType != null && type.BaseType.FullName == "RedSiaCore.IPT.AbstractPhraseTranslator")
                                {
                                    try
                                    {
                                        IPhraseTranslator ipt = Activator.CreateInstance(type, new object[] { null }) as IPhraseTranslator;
                                        PhraseTranslators.Add(ipt);
                                    }
                                    catch (Exception ex)
                                    {

                                    }

                                }
                            }
                        }

                        
                          
                    }
                }
            }

            if (AdditionalTranslators.Count == 0)
            {
                foreach (FileInfo file in new DirectoryInfo(Environment.CurrentDirectory).GetFiles())
                {
                    if (Path.GetExtension(file.FullName) == ".dll")
                    {
                        Assembly a = Assembly.Load(Path.GetFileNameWithoutExtension(file.FullName));

                        foreach (var module in a.GetModules())
                        {
                            foreach (Type type in module.GetTypes())
                            {

                                if (type.BaseType != null && type.BaseType.FullName == "RedSiaCore.IPAT.AbstractPhraseAdditionalTranslator")
                                {
                                    try
                                    {
                                        IPhraseAdditionalTranslator ipt = Activator.CreateInstance(type, new object[] { null }) 
                                            as IPhraseAdditionalTranslator;
                                        AdditionalTranslators.Add(ipt);
                                    }
                                    catch (Exception ex)
                                    {

                                    }

                                }
                            }
                        }



                       

                    }
                }
            }

            FillSiaExecutor(mode);




        }

        /// <summary>
        /// Заполняет списки SiaExecutor
        /// </summary>
        /// <param name="mode">Назначение (SE - общий, ET - Эмоциональная окраска)</param>
        static void FillSiaExecutor(string mode)
        {

            if (mode.ToLower() != "et")
            {
                
            }

            if (SiaExecutor.PhraseTranslators.Any(translator => translator.Destination.ToLower() == "se") &&
                SiaExecutor.SiaTranslators.Any(translator => translator.Destination.ToLower() == "se") &&
                SiaExecutor.AdditionalTranslators.Any(translator => translator.Destination.ToLower() == "se"))
            {
                if (mode.ToLower() != "et")
                {
                  

                    // проходимся по всем возможным загрузкам IST из XML
                    foreach (XmlLoad xmlLoad in SiaXml.GetLoads("IST", mode))
                    {
                        // получаем экземпляр класса по имени
                        ISiaTranslator ist = GetSiaTranslator(xmlLoad.Name, mode);
                        // и добавляем в список
                        if (ist != null && !SiaExecutor.SiaTranslators.Contains(ist)) SiaExecutor.SiaTranslators.Add(ist);
                    }
                    // проходимся по всем возможным загрузкам IPT из XML
                    foreach (XmlLoad xmlLoad in SiaXml.GetLoads("IPT", mode))
                    {
                        IPhraseTranslator ipt = GetPhraseTranslator(xmlLoad.Name, mode);
                        if (ipt != null && !SiaExecutor.PhraseTranslators.Contains(ipt)) SiaExecutor.PhraseTranslators.Add(ipt);
                    }
                    // проходимся по всем возможным загрузкам IPAT из XML
                    foreach (XmlLoad xmlLoad in SiaXml.GetLoads("IPAT", mode))
                    {
                        IPhraseAdditionalTranslator ipat = GetPhraseAdditionalTranslator(xmlLoad.Name, mode);
                        if (ipat != null && !SiaExecutor.AdditionalTranslators.Contains(ipat)) SiaExecutor.AdditionalTranslators.Add(ipat);
                    }
                }

                else
                {
                    // проходимся по всем возможным загрузкам IST из XML
                    foreach (XmlLoad xmlLoad in SiaXml.GetLoads("IST", mode))
                    {
                        // получаем экземпляр класса по имени
                        ISiaTranslator ist = GetSiaTranslator(xmlLoad.Name, mode);
                        // и добавляем в список
                        if (ist != null && !SiaExecutor.SiaTranslators.Contains(ist)) SiaExecutor.SiaTranslators.Add(ist);
                    }
                    // проходимся по всем возможным загрузкам IPT из XML
                    foreach (XmlLoad xmlLoad in SiaXml.GetLoads("IPT", mode))
                    {
                        IPhraseTranslator ipt = GetPhraseTranslator(xmlLoad.Name, mode);
                        if (ipt != null && !SiaExecutor.PhraseTranslators.Contains(ipt)) SiaExecutor.PhraseTranslators.Add(ipt);
                    }
                    // проходимся по всем возможным загрузкам IPAT из XML
                    foreach (XmlLoad xmlLoad in SiaXml.GetLoads("IPAT", mode))
                    {
                        IPhraseAdditionalTranslator ipat = GetPhraseAdditionalTranslator(xmlLoad.Name, mode);
                        if (ipat != null && !SiaExecutor.AdditionalTranslators.Contains(ipat)) SiaExecutor.AdditionalTranslators.Add(ipat);
                    }
                }
            }

            // очищаем все списки
            //    SiaExecutor.AdditionalTranslators.Clear();
            //    SiaExecutor.PhraseTranslators.Clear();
            //   SiaExecutor.SiaTranslators.Clear();

            else
            {
                // проходимся по всем возможным загрузкам IST из XML
                foreach (XmlLoad xmlLoad in SiaXml.GetLoads("IST", mode))
                {
                    // получаем экземпляр класса по имени
                    ISiaTranslator ist = GetSiaTranslator(xmlLoad.Name, mode);
                    // и добавляем в список
                    if (ist != null && !SiaExecutor.SiaTranslators.Contains(ist)) SiaExecutor.SiaTranslators.Add(ist);
                }
                // проходимся по всем возможным загрузкам IPT из XML
                foreach (XmlLoad xmlLoad in SiaXml.GetLoads("IPT", mode))
                {
                    IPhraseTranslator ipt = GetPhraseTranslator(xmlLoad.Name, mode);
                    if (ipt != null && !SiaExecutor.PhraseTranslators.Contains(ipt)) SiaExecutor.PhraseTranslators.Add(ipt);
                }
                // проходимся по всем возможным загрузкам IPAT из XML
                foreach (XmlLoad xmlLoad in SiaXml.GetLoads("IPAT", mode))
                {
                    IPhraseAdditionalTranslator ipat = GetPhraseAdditionalTranslator(xmlLoad.Name, mode);
                    if (ipat != null && !SiaExecutor.AdditionalTranslators.Contains(ipat)) SiaExecutor.AdditionalTranslators.Add(ipat);
                }
            }

          

        }

        /// <summary>
        /// Возвращает IPT модуль
        /// </summary>
        /// <param name="name">Имя модуля (без названия интерфейса)</param>
        /// <param name="mode">назначение</param>
        /// <returns></returns>
        public static IPhraseTranslator GetPhraseTranslator(string name, string mode)
        {
            IPhraseTranslator translator = null;
            foreach (var phraseTranslator in PhraseTranslators)
            {
                if (name == phraseTranslator.GetClassName()) translator = phraseTranslator;
            }
            if (translator != null) translator.Destination = mode;
            return translator;
        }

        /// <summary>
        /// Возвращает IPAT модуль
        /// </summary>
        /// <param name="name">Имя модуля (без названия интерфейса)</param>
        /// <param name="mode">назначение</param>
        /// <returns></returns>
        public static IPhraseAdditionalTranslator GetPhraseAdditionalTranslator(string name, string mode)
        {
            IPhraseAdditionalTranslator translator = null;
            foreach (var phraseTranslator in AdditionalTranslators)
            {
                if (name == phraseTranslator.GetClassName()) translator = phraseTranslator;
            }
            if (translator != null) translator.Destination = mode;
            return translator;
        }

        /// <summary>
        /// Возвращает IST модуль
        /// </summary>
        /// <param name="name">Имя модуля (без названия интерфейса)</param>
        /// <param name="mode">назначение</param>
        /// <returns></returns>
        public static ISiaTranslator GetSiaTranslator(string name, string mode)
        {
            ISiaTranslator translator = null;
            foreach (var phraseTranslator in SiaTranslators)
            {
                if (name == phraseTranslator.GetClassName()) translator = phraseTranslator;
            }
            if (translator != null) translator.Destination = mode;

            return translator;
        }
    }
}