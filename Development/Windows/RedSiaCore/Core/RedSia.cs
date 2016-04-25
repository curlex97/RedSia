using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using RedSiaCore.IPAT;
using RedSiaCore.IPT;
using RedSiaCore.IST;
using RedSiaCore.ISV;

namespace RedSiaCore.Core
{
    /// <summary>
    /// Управляющий класс (тут была Настя)
    /// </summary>
    public static class RedSiaExecutor
    {

        private static bool priority;
        private static SiaExecutor priorityExecutor = null;

        /// <summary>
        /// Список модульных обработчиков
        /// </summary>
        public static readonly List<SiaExecutor> Executors = new List<SiaExecutor>();

        /// <summary>
        /// Поток обработки нажатий с клавиатуры
        /// </summary>
        public static Thread KeyboardThread;

        /// <summary>
        /// Обработать фразу
        /// </summary>
        /// <param name="phrase">фраза</param>
        public static void Execute(string phrase)
        {
            // подписываемся к модулю эмоциональной окраски
            EmoTone.EmoTone.OnExecutingCompleted +=
                state =>
                {
                    // отписываемся
                    EmoTone.EmoTone.OnExecutingCompleted = null;
                    // если всё ок, то продолжаем обработку
                    // может быть или Ок, или Нет
                    if (state.State == ScriptState.Success) ToneExecute(phrase);
                };
            // обрабатываем 
            EmoTone.EmoTone.Execute(phrase);
        }

        /// <summary>
        /// Обработать фразу после модуля эмоциональной окраски
        /// </summary>
        /// <param name="phrase"></param>
        static void ToneExecute(string phrase)
        {
            // Создаём приоритетный обработчик
            // и проверяем наличие такого в списке          
            priority = Executors != null && Executors.Count > 0;
            if (priority)
                priority = Executors.Last() != null && 
                    Executors.Last().CurrentPhraseTranslator != null && 
                    Executors.Last().CurrentPhraseTranslator.PriorityPhrase;
            // если находим, то присваиваем
            if (priority) priorityExecutor = Executors.Last();

            // создаём новый модуль
            var executor = new SiaExecutor("SE");
            // подписываемся на завершение им обработки скрипта
            executor.OnStateChanged += (siaExecutor, state) =>
            {
                // если всё завершилось
                if (executor.CurrentState.State == ScriptState.Success && (executor.CurrentScript == null ||
                                                                           (executor.CurrentScript != null &&
                                                                            executor.CurrentScript.Commands.Count == 0)))
                {
                    // Проходимся по списку IPT модулей
                    for (var j = 0; j < SiaExecutor.PhraseTranslators.Count; j++)
                    {
                        // если находим дочерних
                        if (SiaExecutor.PhraseTranslators[j].Parent == siaExecutor.CurrentPhraseTranslator
                        && siaExecutor.CurrentPhraseTranslator != null)
                        {
                            // то удаляем их
                            SiaExecutor.PhraseTranslators.Remove(SiaExecutor.PhraseTranslators[j]);
                            j--;
                        }
                    }

                    for (var j = 0; j < SiaExecutor.AdditionalTranslators.Count; j++)
                    {
                        // если находим дочерних
                        if (SiaExecutor.AdditionalTranslators[j].Parent == siaExecutor.CurrentPhraseTranslator
                        && siaExecutor.CurrentPhraseTranslator != null)
                        {
                            // то удаляем их
                            SiaExecutor.AdditionalTranslators.Remove(SiaExecutor.AdditionalTranslators[j]);
                            j--;
                        }
                    }

                    for (var j = 0; j < SiaExecutor.SiaTranslators.Count; j++)
                    {
                        // если находим дочерних
                        if (SiaExecutor.SiaTranslators[j].Parent == siaExecutor.CurrentPhraseTranslator
                        && siaExecutor.CurrentPhraseTranslator != null)
                        {
                            // то удаляем их
                            SiaExecutor.SiaTranslators.Remove(SiaExecutor.SiaTranslators[j]);
                            j--;
                        }
                    }

                    // если текущий IPT был одноразовым, то удаляем его из списка
                    if (siaExecutor.CurrentPhraseTranslator != null && siaExecutor.CurrentPhraseTranslator.DynamicPhrase)
                        SiaExecutor.PhraseTranslators.Remove(siaExecutor.CurrentPhraseTranslator);


                    // удаяем обработчик из списка
                    Executors.Remove(siaExecutor);
                }
            };
            executor.OnExecutingChanged += (sExecutor, state) =>
            {
                // если фраза была приоритетной и при этом состояние не Super, то чистим весь скрипт, не давая ему выполниться
                //  if(priority && state.State != ScriptState.SuperReplacement) sExecutor.CurrentScript?.Commands.Clear();

                // если состояние окей
                if (state.State == ScriptState.Success)
                {
                    // то выполняем все остальные обработчики
                    foreach (var siaExecutor in Executors)
                    {
                        if (sExecutor != siaExecutor &&
                            siaExecutor.CurrentState.State == ScriptState.Voice) siaExecutor.Execute(phrase);
                    }
                }

                // если же состояние Replacement при отсутствии приоритета, либо SuperReplacement
                else if ((state.State == ScriptState.Replacement && !priority) ||
                         state.State == ScriptState.SuperReplacement)
                {
                    // явно удаляем приоритет
                    priority = false;
                    priorityExecutor = null;
                    // удаляем все обработчики и дочерние IPT от текущего
                    for (var i = 0; i < Executors.Count; i++)
                    {
                        if (Executors[i] != sExecutor)
                        {
                            for (var j = 0; j < SiaExecutor.PhraseTranslators.Count; j++)
                            {
                                // ищем детей
                                if (SiaExecutor.PhraseTranslators[j].Parent == Executors[i].CurrentPhraseTranslator
                        && sExecutor.CurrentPhraseTranslator != null)
                                {
                                    // и удаляем их
                                    SiaExecutor.PhraseTranslators.Remove(SiaExecutor.PhraseTranslators[j]);
                                    j--;
                                }
                            }

                            for (var j = 0; j < SiaExecutor.AdditionalTranslators.Count; j++)
                            {
                                // ищем детей
                                if (SiaExecutor.AdditionalTranslators[j].Parent == Executors[i].CurrentPhraseTranslator
                        && sExecutor.CurrentPhraseTranslator != null)
                                {
                                    // и удаляем их
                                    SiaExecutor.AdditionalTranslators.Remove(SiaExecutor.AdditionalTranslators[j]);
                                    j--;
                                }
                            }


                            for (var j = 0; j < SiaExecutor.SiaTranslators.Count; j++)
                            {
                                // ищем детей
                                if (SiaExecutor.SiaTranslators[j].Parent == Executors[i].CurrentPhraseTranslator
                        && sExecutor.CurrentPhraseTranslator != null)
                                {
                                    // и удаляем их
                                    SiaExecutor.SiaTranslators.Remove(SiaExecutor.SiaTranslators[j]);
                                    j--;
                                }
                            }


                            // удаляем 
                            Executors.Remove(Executors[i]);
                            i--;
                        }
                    }
                }
                // если же у нас есть приоритет, то выполняем его
                else if (priority) priorityExecutor?.Execute(phrase);


                // отписываемся
                executor.OnExecutingChanged = null;
            };

            
                // добавляем в список новый обработчик
                Executors.Add(executor);
                // и выполняем его
                Executors.Last().Execute(phrase);
            
        }

        /// <summary>
        /// Переводит текст с английской раскладки на русскую
        /// </summary>
        /// <param name="str">текст</param>
        /// <returns></returns>
        static string Translate(string str)
        {
            var ret = "";

            foreach (var t in str)
            {
                switch (t)
                {
                    case 'F':
                        ret += 'А';
                        break;
                    case ',':
                        ret += 'Б';
                        break;
                    case '<':
                        ret += 'Б';
                        break;
                    case 'D':
                        ret += 'В';
                        break;
                    case 'U':
                        ret += 'Г';
                        break;
                    case 'L':
                        ret += 'Д';
                        break;
                    case 'T':
                        ret += 'Е';
                        break;
                    case '~':
                        ret += 'Ё';
                        break;
                    case '`':
                        ret += 'Ё';
                        break;
                    case ':':
                        ret += 'Ж';
                        break;
                    case 'P':
                        ret += 'З';
                        break;
                    case 'B':
                        ret += 'И';
                        break;
                    case 'Q':
                        ret += 'Й';
                        break;
                    case 'R':
                        ret += 'К';
                        break;
                    case 'K':
                        ret += 'Л';
                        break;
                    case 'V':
                        ret += 'М';
                        break;
                    case 'Y':
                        ret += 'Н';
                        break;
                    case 'J':
                        ret += 'О';
                        break;
                    case 'G':
                        ret += 'П';
                        break;
                    case 'H':
                        ret += 'Р';
                        break;
                    case 'C':
                        ret += 'С';
                        break;
                    case 'N':
                        ret += 'Т';
                        break;
                    case 'E':
                        ret += 'У';
                        break;
                    case 'A':
                        ret += 'Ф';
                        break;
                    case '{':
                        ret += 'Х';
                        break;
                    case '[':
                        ret += 'Х';
                        break;
                    case 'W':
                        ret += 'Ц';
                        break;
                    case 'X':
                        ret += 'Ч';
                        break;
                    case 'I':
                        ret += 'Ш';
                        break;
                    case 'O':
                        ret += 'Щ';
                        break;
                    case ']':
                        ret += 'Ъ';
                        break;
                    case '}':
                        ret += 'Ъ';
                        break;
                    case 'S':
                        ret += 'Ф';
                        break;
                    case 'M':
                        ret += 'Ф';
                        break;
                    case '"':
                        ret += 'Э';
                        break;
                    case '\'':
                        ret += 'Э';
                        break;
                    case '>':
                        ret += 'Ю';
                        break;
                    case '.':
                        ret += 'Ю';
                        break;
                    case 'Z':
                        ret += 'Я';
                        break;
                }
            }
            return ret;
        }

        public static SiaScript CheckScript(SiaExecutor executor, SiaScript script, SiaState state)
        {
            if(script != null &&
                priority && 
                priorityExecutor != executor && 
                state.State != ScriptState.SuperReplacement) script.Commands.Clear();

            return script;
        }
    }

    /// <summary>
    /// Модульный обработчик фразы
    /// </summary>
    public class SiaExecutor
    { 
        /// <summary>
        /// делегат окончания обработки скрипта
        /// </summary>
        /// <param name="executor"></param>
        /// <param name="state"></param>
        public delegate void StateChanged(SiaExecutor executor, SiaState state);

        /// <summary>
        /// Список IST модулей (для всех обработчиков один)
        /// </summary>
        public static List<ISiaTranslator> SiaTranslators = new List<ISiaTranslator>();

        /// <summary>
        /// Список IЗT модулей (для всех обработчиков один)
        /// </summary>
        public static List<IPhraseTranslator> PhraseTranslators = new List<IPhraseTranslator>();

        /// <summary>
        /// Список IPAT модулей (для всех обработчиков один)
        /// </summary>
        public static List<IPhraseAdditionalTranslator> AdditionalTranslators = new List<IPhraseAdditionalTranslator>();

        /// <summary>
        /// Текущий IPAT модуль
        /// </summary>
        public IPhraseAdditionalTranslator CurrentPhraseAdditionalTranslator;

        /// <summary>
        /// Текущий IPT модуль
        /// </summary>
        public IPhraseTranslator CurrentPhraseTranslator;

        /// <summary>
        /// Текущий IST модуль
        /// </summary>
        public ISiaTranslator CurrentSiaTranslator;

        /// <summary>
        /// Текущий выполняемый скрипт
        /// </summary>
        public SiaScript CurrentScript = new SiaScript();

        /// <summary>
        /// Текущее состояние выполнения
        /// </summary>
        public SiaState CurrentState;

        /// <summary>
        /// Разрешено ли выполняться скрипту без RunningPermission
        /// </summary>
        public bool IsRunning = true;

        /// <summary>
        /// OSC - завершение обработки скрипта
        /// </summary>
        public StateChanged OnExecutingChanged;

        /// <summary>
        /// OEC - завершение обработки фразы
        /// </summary>
        public StateChanged OnStateChanged;

        /// <summary>
        /// Список переменных скрипта
        /// </summary>
        public List<SiaVariable> Variables = new List<SiaVariable>();

        public string Mode = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode">Назначение (SE - общий, ET - Эмоциональная окраска)</param>
        public SiaExecutor(string mode)
        {
            Mode = mode;
            // устанавливаем текущее состояние: у нас все хорошо
            CurrentState = new SiaState(ScriptState.Success);
            // если у библиотеки модулей не совпадает назначение
            if (SiaLibrary.SiaLibrary.Mode == null || SiaLibrary.SiaLibrary.Mode != mode)
                // переподгружаем модули для обработчика
                SiaLibrary.SiaLibrary.Initialize(mode);
         
           
        }

        /// <summary>
        /// Выполняет обработку фразы
        /// </summary>
        /// <param name="phrase">фраза</param>
        public async void Execute(string phrase)
        {
            // запускаем всё асинхронно, дабы не тормозить программу
            await Task.Run(() =>
            {
                // если всё у модуля окей
           
                if (CurrentState.State == ScriptState.Success)
                {
                    // обнуляем все текущие модули
                    CurrentPhraseAdditionalTranslator = null;
                    CurrentPhraseTranslator = null;
                    CurrentSiaTranslator = null;

                    // проходимся по всем IPT модулям
                    foreach (var phraseTranslator in PhraseTranslators.Where(translator => translator.Destination == Mode))
                    {
                        // пытаемся получить скрипт, если такого обработкчика нет в управляющем классе
                        if(RedSiaExecutor.Executors.All(executor => executor.CurrentPhraseTranslator != phraseTranslator))
                        CurrentScript = phraseTranslator.Execute(this, phrase);
                        // если получили
                        if (CurrentScript != null)
                        {
                            // Назначаем этот IPT текущим
                            CurrentPhraseTranslator = phraseTranslator;
                            // и выходим из цикла
                            break;
                        }
                    }
                    if(CurrentPhraseTranslator != null)
                    CurrentScript = RedSiaExecutor.CheckScript(this, CurrentScript, new SiaState(CurrentPhraseTranslator.State));
                    // выполняем скрипт с текущим транслятором
                    ExecuteScript(CurrentPhraseTranslator);

                    // если (дальше по условию)
                    // то переходим в режим ожидания повторного вызова
                    if (CurrentScript != null &&
                        (CurrentState.State != ScriptState.Success ||
                         CurrentScript.Commands.Count == 0))
                    {
                        OnStateChanged?.Invoke(this, CurrentState);
                        return;
                    }
                }

                // если же мы ждали дополнительную голосовую команду
                else if (CurrentState.State == ScriptState.Voice)
                {
                    // то устанавливаем ей значение текущей фразы
                    SetValue(CurrentState.Variable, new VoiceSiaValue(phrase));
                    // и дальше выполняем скрипт, уже имея значение
                    ExecuteScript(null);
                }

                try
                {
                    File.AppendAllText("log.txt",
                    phrase + @"; IPT: " + CurrentPhraseTranslator + @"; IPAT: " + CurrentPhraseAdditionalTranslator +
                    @"; IST: " + CurrentSiaTranslator + @"\n");
                }
                catch (Exception)
                {
                   
                }
                // в конце сообщаем, что мы закончили, и всё окей
             //   CurrentState = new SiaState(ScriptState.Success);
                OnStateChanged?.Invoke(this, CurrentState);
            });
        }

        /// <summary>
        /// Выполняет обработку сприпта
        /// </summary>
        /// <param name="phraseTranslator">отработваший IPT</param>
        private void ExecuteScript(IPhraseTranslator phraseTranslator)
        {
            // если скрипт получен
            if (CurrentScript != null)
            {
                // если отработавший IPT не равен null 
                if (phraseTranslator != null)
                {
                    // вызываем отработку OEC.
                    // Это сделано так, потому что скрипт
                    // может ожидать ввода дополнительных данных,
                    // но сам скрипт уже давно получен, а значит отрабатывать
                    // OEC было бы некорректно
                    OnExecutingChanged?.Invoke(this,
                        new SiaState(phraseTranslator.State));
                }

                    // одну за одной
                    while (CurrentScript.Commands.Count > 0)
                    {
                        // вытягиваем команды, предварительно получая значения
                        var command = SiaScript.TryGetValues(this, CurrentScript.Commands.Dequeue());

                        // проходимся по всем IST
                        foreach (var siaTranslator in SiaTranslators.Where(translator => translator.Destination == Mode))
                        {
                            // если можно обрабатывать, либо есть разрешение на обработку
                            if (IsRunning || (!IsRunning && siaTranslator.RunningPermission))
                            {
                                // получаем состояние после обработки
                                CurrentState = siaTranslator.Execute(this, command);
                                // и, если оно не удовлетворительно
                                if (CurrentState.State != ScriptState.Success)
                                {
                                    // вызываем OSC с этим состоянием
                                  //  OnStateChanged?.Invoke(this, CurrentState);
                                    return;
                                }
                            }
                        }

                        // если же израсходовали все команды
                        if (CurrentScript.Commands.Count == 0)
                        {
                            //  то тоже вызываем OSC
                         //   OnStateChanged?.Invoke(this, CurrentState);
                            return;
                        }
                    }
                

            }
            // если же скрит не получен
            else
            {
                // возвращаем, что всё окей
                OnExecutingChanged?.Invoke(this,
                    new SiaState(ScriptState.Success));
                // возвращаем, что всё окей
              //  OnStateChanged?.Invoke(this,
               //     new SiaState(ScriptState.Success));
               
              
            }
            return;

        }

        /// <summary>
        /// Устанавливает значение переменной
        /// </summary>
        /// <param name="variable">имя переменной</param>
        /// <param name="value">значение переменной</param>
        public void SetValue(string variable, SiaValue value)
        {
            foreach (var t in Variables)
            {
                if (t.Name == variable)
                {
                    t.Value = value;
                    return;
                }
            }
            Variables.Add(new SiaVariable(variable, value));
        }

        /// <summary>
        /// Возвращает значение переменной
        /// </summary>
        /// <param name="variable">имя переменной</param>
        /// <returns>значение переменной</returns>
        public SiaValue GetValue(string variable)
        {
            foreach (var t in Variables)
                if (t.Name == variable)
                    return t.Value;
            return new NumberSiaValue("");
        }

        public void AddIpt(IPhraseTranslator translator)
        {
            if (translator != null) PhraseTranslators.Add(translator);
        }

        public void AddIpat(IPhraseAdditionalTranslator translator)
        {
            if (translator != null) AdditionalTranslators.Add(translator);
        }

        public void AddIst(ISiaTranslator translator)
        {
            if (translator != null) SiaTranslators.Add(translator);
        }
    } 

    /// <summary>
    /// Модуль состояния обработки скрипта
    /// </summary>
    public class SiaState
    {
        /// <summary>
        /// Режим состояния
        /// </summary>
        public ScriptState State;

        /// <summary>
        /// Имя переменной
        /// </summary>
        public string Variable;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state">Режим состояния</param>
        /// <param name="variable">Имя переменной</param>
        public SiaState(ScriptState state, string variable = null)
        {
            State = state;
            Variable = variable;
        }
    }

    /// <summary>
    /// Режимы состояний модулей
    /// </summary>
    public enum ScriptState
    {
        Success = 1,
        Mouse = 2,
        Keyboard = 3,
        Voice = 4,
        Error = 0,
        Override = 5,
        Replacement = 6,
        SuperReplacement = 7
    }
}