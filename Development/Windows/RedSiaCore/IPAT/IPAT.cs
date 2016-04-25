using RedSiaCore.Core;
using RedSiaCore.IPT;

namespace RedSiaCore.IPAT
{
    /// <summary>
    /// Дополнительный фразовый обработчик
    /// </summary>
    public interface IPhraseAdditionalTranslator
    {
        /// <summary>
        /// Имя модуля (без названия интерфейса)
        /// </summary>
        /// <returns>Имя модуля (без названия интерфейса)</returns>
        string GetClassName();

        /// <summary>
        /// Выполняет скрипт
        /// </summary>
        /// <param name="executor">Текущий модульный обработчик</param>
        /// <param name="phrase">фраза</param>
        /// <returns>Состояние после обработки</returns>
        string Execute(SiaExecutor executor, string phrase);

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
    /// Абстрактный класс для реализации IPhraseAdditionalTranslator
    /// </summary>
    public class AbstractPhraseAdditionalTranslator : IPhraseAdditionalTranslator
    {


        public AbstractPhraseAdditionalTranslator(IPhraseTranslator parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Имя модуля (без названия интерфейса)
        /// </summary>
        /// <returns>Имя модуля (без названия интерфейса)</returns>
        public virtual string GetClassName()
        {
            var name = GetType().Name.Replace("PhraseAdditionalTranslator", string.Empty);
            return name;
        }

        /// <summary>
        /// Выполняет скрипт
        /// </summary>
        /// <param name="executor">Текущий модульный обработчик</param>
        /// <param name="phrase">фраза</param>
        /// <returns>Состояние после обработки</returns>
        public virtual string Execute(SiaExecutor executor, string phrase)
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