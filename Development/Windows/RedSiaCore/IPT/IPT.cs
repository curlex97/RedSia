using System;
using System.Linq;
using RedSiaCore.Core;
using RedSiaCore.ISV;
using RedSiaCore.XML;

namespace RedSiaCore.IPT
{
    /// <summary>
    /// Основной фразовый обработчик
    /// </summary>
    public interface IPhraseTranslator
    {
        /// <summary>
        /// Состояние модуля
        /// </summary>
        ScriptState State { get; set; }

        /// <summary>
        /// Динамическая (одноразовая) фраза (удаляется после разовой отработки)
        /// </summary>
        bool DynamicPhrase { get; set; }

        /// <summary>
        /// Приоритетная фраза (не замещается, кроме SuReplacement)
        /// </summary>
        bool PriorityPhrase { get; set; }

        /// <summary>
        /// Родитель, создавший модуль в списке
        /// </summary>
        IPhraseTranslator Parent { get; set; }

        /// <summary>
        /// Текущая фраза
        /// </summary>
        string Phrase { get; set; }

        /// <summary>
        /// Выполняет скрипт
        /// </summary>
        /// <param name="executor">Текущий модульный обработчик</param>
        /// <param name="phrase">фраза</param>
        /// <returns>Состояние после обработки</returns>
        SiaScript Execute(SiaExecutor executor, string phrase);

        /// <summary>
        /// Имя модуля (без названия интерфейса)
        /// </summary>
        /// <returns>Имя модуля (без названия интерфейса)</returns>
        string GetClassName();

        /// <summary>
        /// Назначение (SE - общий, ET - эмоциональная окраска)
        /// </summary>
        string Destination { get; set; }
    }

    /// <summary>
    /// Абстрактный класс для реализации IPhraseTranslator
    /// </summary>
    public class AbstractPhraseTranslator : IPhraseTranslator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent">родитель</param>
        /// <param name="state">состояние</param>
        /// <param name="dynamicPhrase">одноразовая фраза</param>
        /// <param name="priorityPhrase">приоритетная фраза</param>
        public AbstractPhraseTranslator(IPhraseTranslator parent, ScriptState state, bool dynamicPhrase, bool priorityPhrase)
        {
            Parent = parent;
            State = state;
            DynamicPhrase = dynamicPhrase;
            PriorityPhrase = priorityPhrase;
            Phrase = null;
        }

        /// <summary>
        /// Состояние модуля
        /// </summary>
        public ScriptState State { get; set; }

        /// <summary>
        /// Динамическая (одноразовая) фраза (удаляется после разовой отработки)
        /// </summary>
        public bool DynamicPhrase { get; set; }

        /// <summary>
        /// Приоритетная фраза (не замещается, кроме SuReplacement)
        /// </summary>
        public bool PriorityPhrase { get; set; }

        /// <summary>
        /// Родитель, создавший модуль в списке
        /// </summary>
        public IPhraseTranslator Parent { get; set; }

        /// <summary>
        /// Текущая фраза
        /// </summary>
        public string Phrase { get; set; }

        /// <summary>
        /// Выполняет скрипт
        /// </summary>
        /// <param name="executor">Текущий модульный обработчик</param>
        /// <param name="phrase">фраза</param>
        /// <returns>Состояние после обработки</returns>
        public virtual SiaScript Execute(SiaExecutor executor, string phrase)
        {
            return null;
        }

        /// <summary>
        /// Имя модуля (без названия интерфейса)
        /// </summary>
        /// <returns>Имя модуля (без названия интерфейса)</returns>
        public virtual string GetClassName()
        {
            var name = GetType().Name.Replace("PhraseTranslator", string.Empty);
            return name;
        }

        protected bool CanExecute()
        {
            var xmlTranslator = SiaXml.GetTranslator(GetClassName());
            if (xmlTranslator == null) return false;
            foreach (XmlCall xmlCall in xmlTranslator.Calls)
            {
                bool ifl = true;
                foreach (string value in xmlCall.Values)
                {                  
                    if (!Phrase.ToLower().Contains(value.ToLower()))
                        ifl = false;
                }
                if (ifl) return true;
            }
            return false;
        }

        /// <summary>
        /// Назначение (SE - общий, ET - эмоциональная окраска)
        /// </summary>
        public string Destination { get; set; }
    }

}