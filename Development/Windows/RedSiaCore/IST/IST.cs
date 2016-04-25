using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Speech.Synthesis;
using RedSiaCore.Core;
using RedSiaCore.IPT;
using RedSiaCore.ISV;
using RedSiaCore.Utils;
using RedSiaCore.XML;

namespace RedSiaCore.IST
{
    /// <summary>
    /// Обработчик скрипта
    /// </summary>
    public interface ISiaTranslator
    {
        /// <summary>
        /// Разрешение на запуск при условно-запрещённом выполнении
        /// </summary>
         bool RunningPermission { get; set; }

        /// <summary>
        /// Имя модуля (без названия интерфейса)
        /// </summary>
        /// <returns>Имя модуля (без названия интерфейса)</returns>
        string GetClassName();

        /// <summary>
        /// Выполняет скрипт
        /// </summary>
        /// <param name="executor">Текущий модульный обработчик</param>
        /// <param name="command">команда</param>
        /// <returns>Состояние после обработки</returns>
        SiaState Execute(SiaExecutor executor, string command);

        /// <summary>
        /// Родитель, создавший модуль в списке
        /// </summary>
        IPhraseTranslator Parent { get; set; }

        /// <summary>
        /// Назначение (SE - общий, ET - эмоциональная окраска)
        /// </summary>
        string Destination { get; set; }

    }

    /// <summary>
    /// Абстрактный класс для реализации ISiaTranslator
    /// </summary>
    public class AbstractSiaTranslator : ISiaTranslator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent">родитель</param>
        /// <param name="runungPermission">разрешение на условно-запрещённый запуск</param>
        public AbstractSiaTranslator(IPhraseTranslator parent, bool runungPermission)
        {
            Parent = parent;
            RunningPermission = runungPermission;
        }

        /// <summary>
        /// Разрешение на запуск при условно-запрещённом выполнении
        /// </summary>
        public bool RunningPermission { get; set; }

        /// <summary>
        /// Имя модуля (без названия интерфейса)
        /// </summary>
        /// <returns>Имя модуля (без названия интерфейса)</returns>
        public virtual string GetClassName()
        {
            var name = GetType().Name.Replace("SiaTranslator", string.Empty);
            return name;
        }

        /// <summary>
        /// Выполняет скрипт
        /// </summary>
        /// <param name="executor">Текущий модульный обработчик</param>
        /// <param name="command">команда</param>
        /// <returns>Состояние после обработки</returns>
        public virtual SiaState Execute(SiaExecutor executor, string command)
        {
            return null;
        }

        /// <summary>
        /// Родитель, создавший модуль в списке
        /// </summary>
        public IPhraseTranslator Parent { get; set; }

        /// <summary>
        /// Назначение (SE - общий, ET - эмоциональная окраска)
        /// </summary>
        public string Destination { get; set; }
    }


}