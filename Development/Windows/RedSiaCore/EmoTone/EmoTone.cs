using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedSiaCore.Core;

namespace RedSiaCore.EmoTone
{
    /// <summary>
    /// Модуль эмоциональной окраски
    /// </summary>
    public static class EmoTone
    {
        /// <summary>
        /// Модульный обработчик
        /// </summary>
        static SiaExecutor _emoTone;

        /// <summary>
        /// Отношение к пользователю
        /// </summary>
        public static int Tone = 100;

        /// <summary>
        /// делегат возврата окончания обработки
        /// </summary>
        /// <param name="state">Состояние выполнения</param>
        public delegate void ExecutingCompleted(SiaState state);

        /// <summary>
        /// Завершение выполнения модуля
        /// </summary>
        public static ExecutingCompleted OnExecutingCompleted;

        /// <summary>
        /// Выполняет команду
        /// </summary>
        /// <param name="phrase">фраза</param>
        public static void Execute(string phrase)
        {
            // содаём новый экземпляр с параметром ET
            _emoTone = new SiaExecutor("ET");
            // Подписываемся на окончание его работы своим делегатом
            _emoTone.OnStateChanged += (executor, state) =>
            {
                _emoTone.OnStateChanged = null;
                OnExecutingCompleted?.Invoke(state);
            };
            // выполняем
            _emoTone.Execute(phrase);
        }


    }
}
