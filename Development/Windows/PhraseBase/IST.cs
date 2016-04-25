using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using RedSiaCore.Core;
using RedSiaCore.EmoTone;
using RedSiaCore.IPT;
using RedSiaCore.IST;
using RedSiaCore.ISV;
using RedSiaCore.Utils;
using RedSiaCore.XML;

namespace PhraseBase
{
    public class SetVariableSiaTranslator : AbstractSiaTranslator
    {
        public override SiaState Execute(SiaExecutor executor, string command)
        {
            var tokens = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length >= 3 && tokens[0] == "set")
            {
                if (tokens[2] == "$voice") return new SiaState(ScriptState.Voice, tokens[1]);
                if (tokens[2] == "$keyboard") return new SiaState(ScriptState.Keyboard, tokens[1]);
                if (tokens[2] == "$mouse") return new SiaState(ScriptState.Mouse, tokens[1]);
                if (tokens[2] == "$phrase")
                {
                    executor.SetValue(tokens[1], new StringSiaValue(executor.CurrentPhraseTranslator.Phrase));
                    return new SiaState(ScriptState.Success);
                }
                if (tokens[2] == "$emotion")
                {
                    executor.SetValue(tokens[1], new NumberSiaValue(EmoTone.Tone.ToString()));
                    return new SiaState(ScriptState.Success);
                }

                if (tokens[2] == "$day" && tokens.Length > 3)
                {
                    DateTime dt;

                    try
                    {
                        dt = Convert.ToDateTime(tokens[3]);
                        executor.SetValue(tokens[1], new NumberSiaValue(dt.Day.ToString()));
                    }
                    catch (Exception)
                    {
                        executor.SetValue(tokens[1], new StringSiaValue(tokens[3]));
                    }
                    return new SiaState(ScriptState.Success);
                }

                if (tokens[2] == "$month" && tokens.Length > 3)
                {
                    DateTime dt;

                    try
                    {
                        dt = Convert.ToDateTime(tokens[3]);
                        executor.SetValue(tokens[1], new NumberSiaValue(dt.Month.ToString()));
                    }
                    catch (Exception)
                    {
                        executor.SetValue(tokens[1], new StringSiaValue(tokens[3]));
                    }
                    return new SiaState(ScriptState.Success);
                }

                if (tokens[2] == "$year" && tokens.Length > 3)
                {
                    DateTime dt;

                    try
                    {
                        dt = Convert.ToDateTime(tokens[3]);
                        executor.SetValue(tokens[1], new NumberSiaValue(dt.Year.ToString()));
                    }
                    catch (Exception)
                    {
                        executor.SetValue(tokens[1], new StringSiaValue(tokens[3]));
                    }
                    return new SiaState(ScriptState.Success);
                }

                if (tokens[2] == "$hour" && tokens.Length > 3)
                {
                    DateTime dt;

                    try
                    {
                        dt = Convert.ToDateTime(tokens[3]);
                        executor.SetValue(tokens[1], new NumberSiaValue(dt.Hour.ToString()));
                    }
                    catch (Exception)
                    {
                        executor.SetValue(tokens[1], new StringSiaValue(tokens[3]));
                    }
                    return new SiaState(ScriptState.Success);
                }

                if (tokens[2] == "$minute" && tokens.Length > 3)
                {
                    DateTime dt;

                    try
                    {
                        dt = Convert.ToDateTime(tokens[3]);
                        executor.SetValue(tokens[1], new NumberSiaValue(dt.Minute.ToString()));
                    }
                    catch (Exception)
                    {
                        executor.SetValue(tokens[1], new StringSiaValue(tokens[3]));
                    }
                    return new SiaState(ScriptState.Success);
                }

                if (tokens[2] == "$second" && tokens.Length > 3)
                {
                    DateTime dt;

                    try
                    {
                        dt = Convert.ToDateTime(tokens[3]);
                        executor.SetValue(tokens[1], new NumberSiaValue(dt.Second.ToString()));
                    }
                    catch (Exception)
                    {
                        executor.SetValue(tokens[1], new StringSiaValue(tokens[3]));
                    }
                    return new SiaState(ScriptState.Success);
                }


                string result = Eval(tokens[2]);

                try
                {
                    int r = Convert.ToInt32(result);
                    executor.SetValue(tokens[1], new NumberSiaValue(result));
                    return new SiaState(ScriptState.Success);
                }
                catch
                {
                    try
                    {
                        DateTime d = Convert.ToDateTime(result);
                        executor.SetValue(tokens[1], new DateSiaValue(result));
                        return new SiaState(ScriptState.Success);
                    }
                    catch
                    {
                        executor.SetValue(tokens[1], new StringSiaValue(result));
                        return new SiaState(ScriptState.Success);
                    }
                }

                
            }
            return new SiaState(ScriptState.Success);
        }

        private string Eval(string str)
        {
            string ret;
            try
            {
                ret = new DataTable().Compute(str, "").ToString();
            }
            catch
            {
                ret = str;
            }

            return ret;
        }

        public SetVariableSiaTranslator(IPhraseTranslator parent) : base(parent, false)
        {
        }
    }


    public class KeyboardPrintSiaTranslator : AbstractSiaTranslator
    {

        public override SiaState Execute(SiaExecutor executor, string command)
        {
            var tokens = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length > 1 && tokens[0] == "print")
            {
                try
                {
                    var str = "";
                    for (var i = 1; i < tokens.Length; i++)
                        str += tokens[i] + " ";
                    KeyboardImulation.Print(str.Substring(0, str.Length - 1));
                    return new SiaState(ScriptState.Success);
                }
                catch
                {
                    return new SiaState(ScriptState.Error);
                }
            }
            return new SiaState(ScriptState.Success);
        }


        public KeyboardPrintSiaTranslator(IPhraseTranslator parent) : base(parent, false)
        {
        }
    }


    public class ProcessingConditionsSiaTranslator : AbstractSiaTranslator
    {
        private bool _onceRun;

        public override SiaState Execute(SiaExecutor executor, string command)
        {
            var args = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);


            if (args.Length == 4 && args[0] == "if")
            {
                _onceRun = false;

                switch (args[2])
                {
                    case "==":
                        executor.IsRunning = args[1].ToLower() == args[3].ToLower();
                        break;

                    case "!=":
                        executor.IsRunning = args[1].ToLower() != args[3].ToLower();
                        break;

                    case ">=":
                        try
                        {
                            executor.IsRunning = Convert.ToInt32(args[1]) >= Convert.ToInt32(args[3]);
                        }
                        catch (Exception)
                        {
                            executor.IsRunning = false;
                        }

                        break;

                    case "<=":
                        try
                        {
                            executor.IsRunning = Convert.ToInt32(args[1]) <= Convert.ToInt32(args[3]);
                        }
                        catch (Exception)
                        {
                            executor.IsRunning = false;
                        }
                        break;

                    case ">":
                        try
                        {
                            executor.IsRunning = Convert.ToInt32(args[1]) > Convert.ToInt32(args[3]);
                        }
                        catch (Exception)
                        {
                            executor.IsRunning = false;
                        }
                        break;

                    case "<":
                        try
                        {
                            executor.IsRunning = Convert.ToInt32(args[1]) < Convert.ToInt32(args[3]);
                        }
                        catch (Exception)
                        {
                            executor.IsRunning = false;
                        }
                        break;
                    default:
                        executor.IsRunning = false;
                        break;
                }
                if (executor.IsRunning) _onceRun = true;
            }

            else if (args.Length == 4 && args[0] == "elseif")
            {
                if (!executor.IsRunning)
                {
                    switch (args[2])
                    {
                        case "==":
                            executor.IsRunning = args[1].ToLower() == args[3].ToLower();
                            break;

                        case "!=":
                            executor.IsRunning = args[1].ToLower() != args[3].ToLower();
                            break;

                        case ">=":
                            try
                            {
                                executor.IsRunning = Convert.ToInt32(args[1]) >= Convert.ToInt32(args[3]);
                            }
                            catch (Exception)
                            {
                                executor.IsRunning = false;
                            }

                            break;

                        case "<=":
                            try
                            {
                                executor.IsRunning = Convert.ToInt32(args[1]) <= Convert.ToInt32(args[3]);
                            }
                            catch (Exception)
                            {
                                executor.IsRunning = false;
                            }
                            break;

                        case ">":
                            try
                            {
                                executor.IsRunning = Convert.ToInt32(args[1]) > Convert.ToInt32(args[3]);
                            }
                            catch (Exception)
                            {
                                executor.IsRunning = false;
                            }
                            break;

                        case "<":
                            try
                            {
                                executor.IsRunning = Convert.ToInt32(args[1]) < Convert.ToInt32(args[3]);
                            }
                            catch (Exception)
                            {
                                executor.IsRunning = false;
                            }
                            break;
                        default:
                            executor.IsRunning = false;
                            break;
                    }
                }
                else executor.IsRunning = false;
                if (executor.IsRunning) _onceRun = true;
            }
            else if (args.Length == 1 && args[0] == "else") executor.IsRunning = !_onceRun;
            else if (args.Length == 1 && args[0] == "endif") executor.IsRunning = true;
            return new SiaState(ScriptState.Success);
        }


        public ProcessingConditionsSiaTranslator(IPhraseTranslator parent) : base(parent, true)
        {
        }
    }


    public class SpeakSiaTranslator : AbstractSiaTranslator
    {
        private readonly SpeechSynthesizer _speech = new SpeechSynthesizer();


        public override SiaState Execute(SiaExecutor executor, string command)
        {
            var tokens = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length > 1 && tokens[0] == "speak")
            {
                var xmlTranslator = SiaXml.GetTranslator(executor.CurrentPhraseTranslator.GetClassName());
                string text = "";
                string str = "";

                for (int i = 1; i < tokens.Length; i++)
                    str += tokens[i] + " ";
                str = str.Trim();
                foreach (string s in str.Split(new[] {"&"}, StringSplitOptions.RemoveEmptyEntries))
                {
                    string f = s.Trim();
                    var speak = xmlTranslator.GetSpeakByGroup(f);
                    if (speak != null)
                        text += speak.Value + " ";
                    else text += f;

                }
                _speech.Speak(text);
            }
            return new SiaState(ScriptState.Success);
        }

        public SpeakSiaTranslator(IPhraseTranslator parent) : base(parent, false)
        {
        }
    }


    public class StateReturnSiaTranslator : AbstractSiaTranslator
    {
        public override SiaState Execute(SiaExecutor executor, string command)
        {
            var tokens = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 2 && tokens[0] == "return")
            {
                executor.CurrentScript.Commands.Clear();

                switch (tokens[1].ToLower())
                {
                    case "success":
                        return new SiaState(ScriptState.Success);

                    case "mouse":
                        return new SiaState(ScriptState.Mouse);

                    case "keyboard":
                        return new SiaState(ScriptState.Keyboard);

                    case "error":
                        return new SiaState(ScriptState.Error);

                    case "override":
                        return new SiaState(ScriptState.Override);

                    case "replacement":
                        return new SiaState(ScriptState.Replacement);

                    case "sureplacement":
                        return new SiaState(ScriptState.SuperReplacement);

                    case "voice":
                        return new SiaState(ScriptState.Voice);
                        
                }
            }
            return new SiaState(ScriptState.Success);
        }

        public StateReturnSiaTranslator(IPhraseTranslator parent) : base(parent, false)
        {
        }
    }


    public class AddIptSiaTranslator : AbstractSiaTranslator
    {

        public override SiaState Execute(SiaExecutor executor, string command)
        {
            var tokens = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length > 2 && tokens[0] == "include")
            {
                if (tokens[1] == "ipt")
                {
                    var translator = RedSiaCore.SiaLibrary.SiaLibrary.GetPhraseTranslator(tokens[2],
                        executor.CurrentPhraseTranslator.Destination);
                    if (translator != null && !SiaExecutor.PhraseTranslators.Contains(translator))
                    {
                        translator.Parent = executor.CurrentPhraseTranslator;
                        executor.AddIpt(translator);
                    }
                }

                else if (tokens.Length > 1 && tokens[1] == "ipat")
                {
                    var translator = RedSiaCore.SiaLibrary.SiaLibrary.GetPhraseAdditionalTranslator(tokens[2],
                        executor.CurrentPhraseTranslator.Destination);
                    if (translator != null && !SiaExecutor.AdditionalTranslators.Contains(translator))
                    {
                        translator.Parent = executor.CurrentPhraseTranslator;
                       executor.AddIpat(translator);
                    }
                }

                else if (tokens.Length > 1 && tokens[1] == "ist")
                {
                    var translator = RedSiaCore.SiaLibrary.SiaLibrary.GetSiaTranslator(tokens[2],
                        executor.CurrentPhraseTranslator.Destination);
                    if (translator != null && !SiaExecutor.SiaTranslators.Contains(translator))
                    {
                        translator.Parent = executor.CurrentPhraseTranslator;
                        executor.AddIst(translator);
                    }
                }
            }

            if (tokens.Length > 2 && tokens[0] == "exclude")
            {
                if (tokens[1] == "ipt")
                {
                    var translator = RedSiaCore.SiaLibrary.SiaLibrary.GetPhraseTranslator(tokens[2],
                        executor.CurrentPhraseTranslator.Destination);
                    if (translator != null)
                        SiaExecutor.PhraseTranslators.Remove(translator);
                    
                }

                else if (tokens.Length > 1 && tokens[1] == "ipat")
                {
                    var translator = RedSiaCore.SiaLibrary.SiaLibrary.GetPhraseAdditionalTranslator(tokens[2],
                        executor.CurrentPhraseTranslator.Destination);
                    if (translator != null)
                        SiaExecutor.AdditionalTranslators.Remove(translator);
                }

                else if (tokens.Length > 1 && tokens[1] == "ist")
                {
                    var translator = RedSiaCore.SiaLibrary.SiaLibrary.GetSiaTranslator(tokens[2],
                        executor.CurrentPhraseTranslator.Destination);
                    if (translator != null)
                        SiaExecutor.SiaTranslators.Remove(translator);
                }
            }

            return new SiaState(ScriptState.Success);
        }

        public AddIptSiaTranslator(IPhraseTranslator parent) : base(parent, false)
        {
        }
    }


    public class StartTaskSiaTranslator : AbstractSiaTranslator
    {
        private readonly SpeechSynthesizer _speech = new SpeechSynthesizer();


        public override SiaState Execute(SiaExecutor executor, string command)
        {
            var tokens = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length > 1 && tokens[0] == "task")
            {
                try
                {
                    string str = "";
                    for (int i = 1; i < tokens.Length; i++)
                        str += tokens[i] + " ";
                    str = str.Trim();
                    Process.Start(str);
                }
                catch
                {
                    //
                }

                

            }
            return new SiaState(ScriptState.Success);
        }

        public StartTaskSiaTranslator(IPhraseTranslator parent) : base(parent, false)
        {
        }
    }

}


// рекурсия при удалении дочерних
// выбор лексики по наименьшей разнице