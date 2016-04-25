using System;
using System.Collections.Generic;
using System.Linq;
using RedSiaCore.Core;

namespace RedSiaCore.ISV
{
    /// <summary>
    /// Абстрактный тип переменной
    /// </summary>
    public abstract class SiaValue
    {
        /// <summary>
        /// Значение SiaScript переменной
        /// </summary>

        public virtual string ToString()
        {
            return "null";
        }
    
    }

    /// <summary>
    /// Строковый тип переменной
    /// </summary>
    public class StringSiaValue : SiaValue
    {
        public string Value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">Значение SiaScript переменной</param>
        public StringSiaValue(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }

    /// <summary>
    /// Числовой тип переменной
    /// </summary>
    public class NumberSiaValue : SiaValue
    {
        private int Value;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">Значение SiaScript переменной</param>
        public NumberSiaValue(string value)
        {
            try
            {
                Value = Convert.ToInt32(value);
            }
            catch
            {
                Value = 0;
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    /// <summary>
    /// Числовой тип переменной
    /// </summary>
    public class DateSiaValue : SiaValue
    {
        private DateTime Value;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">Значение SiaScript переменной</param>
        public DateSiaValue(string value)
        {
            try
            {
                Value = Convert.ToDateTime(value);
            }
            catch
            {
                Value = DateTime.MinValue;
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    /// <summary>
    /// Кливиатурный тип переменной
    /// </summary>
    public class KeyboardSiaValue : SiaValue
    {
        public string Value;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">Значение SiaScript переменной</param>
        public KeyboardSiaValue(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    /// <summary>
    /// Мышь- тип переменной
    /// </summary>
    public class MouseSiaValue : SiaValue
    {
        public string Value;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">Значение SiaScript переменной</param>
        public MouseSiaValue(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    /// <summary>
    /// Голосовой тип переменной
    /// </summary>
    public class VoiceSiaValue : SiaValue
    {
        public string Value;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">Значение SiaScript переменной</param>
        public VoiceSiaValue(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    /// <summary>
    /// SiaScript переменная
    /// </summary>
    public class SiaVariable
    {
        /// <summary>
        /// Имя переменной
        /// </summary>
        public string Name;

        /// <summary>
        /// Значение переменной
        /// </summary>
        public SiaValue Value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Имя переменной</param>
        /// <param name="value">Значение переменной</param>
        public SiaVariable(string name, SiaValue value)
        {
            Name = name;
            Value = value;
        }

    }

    /// <summary>
    /// Скрипт SiaScript
    /// </summary>
    public class SiaScript
    {
        /// <summary>
        /// Очередь команд скрипта
        /// </summary>
        public Queue<string> Commands = new Queue<string>();

        /// <summary>
        /// Преобразует текст скрипта в очередь команд
        /// </summary>
        /// <param name="executor">текущий обработчик</param>
        /// <param name="text">скрипт</param>
        /// <returns></returns>
        public static SiaScript ToScript(SiaExecutor executor, string text)
        {
            // создаём новый экземпляр
            SiaScript script = new SiaScript();
            // убираем лишние пробелы
            while (text.IndexOf("  ", StringComparison.Ordinal) != -1) text = text.Replace("  ", " ");
            // убираем эскейп-последовательности
            text = text.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace("\t", String.Empty);
            // режем по точке с запятой
            foreach (string s in text.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                // режем по пробелу
                string[] strs = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (SiaVariable siaVariable in executor.Variables)
                    for (int i = 0; i < strs.Length; i++)
                        // подствляем вместо имён переменных их значения
                        if (strs[i] == siaVariable.Name) strs[i] = siaVariable.Value.ToString();
                // добавляем в очередь готовую команду
                script.Commands.Enqueue(strs.Aggregate("", (current, t) => current + t + " "));
            }
            // возвращаем готовый скрипт
            return script;
        }

        /// <summary>
        /// Возвращает значение переменной
        /// </summary>
        /// <param name="executor">модульный обработчик</param>
        /// <param name="command">команда</param>
        /// <returns></returns>
        public static string TryGetValues(SiaExecutor executor, string command)
        {
            try
            {
                var strs = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var siaVariable in executor.Variables)
                    for (var i = 0; i < strs.Length; i++)
                        if (strs[i] == siaVariable.Name) strs[i] = siaVariable.Value.ToString();
                var str = strs.Aggregate("", (current, t) => current + t + " ");
                return str.Substring(0, str.Length - 1);
            }
            catch
            {
                return command;
            }
        }
    }

}
