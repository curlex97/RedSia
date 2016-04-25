using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using RedSiaCore.Core;
using RedSiaCore.IPT;
using RedSiaCore.ISV;
using RedSiaCore.XML;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using HtmlAgilityPack;

namespace PhraseBase
{
    public class CancelPhraseTranslator : AbstractPhraseTranslator
    {
        public CancelPhraseTranslator(IPhraseTranslator parent) :
            base(parent, ScriptState.SuperReplacement, false, false)
        {

        }

        public override SiaScript Execute(SiaExecutor executor, string phrase)
        {
            Phrase = phrase;
            if (CanExecute())
            {
                return SiaScript.ToScript(executor, @" 


speak 0;


");
            }

            return null;
        }

    }


    public class TestEmoTonePhraseTranslator : AbstractPhraseTranslator
    {

        public TestEmoTonePhraseTranslator(IPhraseTranslator parent) :
            base(parent, ScriptState.Replacement, false, false)
        {
        }

        public override SiaScript Execute(SiaExecutor executor, string phrase)
        {
            Phrase = phrase;

            if (CanExecute())
            {
                return SiaScript.ToScript(executor, @" 


set phrase $phrase;

if phrase == старт;
    return success;
else;
    return error;
endif;


");
            }

            return null;
        }


    }


    public class CalcPhraseTranslator : AbstractPhraseTranslator
    {
        public CalcPhraseTranslator(IPhraseTranslator parent) :
            base(parent, ScriptState.Replacement, false, false)
        {
           
        }

        public override SiaScript Execute(SiaExecutor executor, string phrase)
        {
            Phrase = phrase;
            if (CanExecute())
            {

                string ret = "";
                string[] strs = phrase.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < strs.Length; i++)
                {

                    try
                    {
                        if (strs[i].ToLower() == "один") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "1"; }
                        else if(strs[i].ToLower() == "одна") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "1"; }
                        else if (strs[i].ToLower() == "два") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "2"; }
                        else if (strs[i].ToLower() == "две") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "2"; }
                        else if (strs[i].ToLower() == "три") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "3"; }
                        else if (strs[i].ToLower() == "четыре") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "4"; }
                        else if (strs[i].ToLower() == "пять") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "5"; }
                        else if (strs[i].ToLower() == "шесть") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "6"; }
                        else if (strs[i].ToLower() == "семь") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "7"; }
                        else if (strs[i].ToLower() == "восемь") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "8"; }
                        else if (strs[i].ToLower() == "девять") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "9"; }

                        else if (strs[i].ToLower() == "десять") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "10"; }
                        else if (strs[i].ToLower() == "двадцать") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "20"; }
                        else if (strs[i].ToLower() == "тридцать") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "30"; }
                        else if (strs[i].ToLower() == "сорок") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "40"; }
                        else if (strs[i].ToLower() == "пятьдесят") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "50"; }
                        else if (strs[i].ToLower() == "шестьдесят") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "60"; }
                        else if (strs[i].ToLower() == "семьдесят") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "70"; }
                        else if (strs[i].ToLower() == "восемьдесят") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "80"; }
                        else if (strs[i].ToLower() == "девяносто") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "90"; }


                        else if (strs[i].ToLower() == "сто") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "10"; }
                        else if (strs[i].ToLower() == "двести") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "20"; }
                        else if (strs[i].ToLower() == "триста") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "30"; }
                        else if (strs[i].ToLower() == "четыреста") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "40"; }
                        else if (strs[i].ToLower() == "пятьсот") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "50"; }
                        else if (strs[i].ToLower() == "шестьсот") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "60"; }
                        else if (strs[i].ToLower() == "семьсот") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "70"; }
                        else if (strs[i].ToLower() == "восемьсот") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "80"; }
                        else if (strs[i].ToLower() == "девятьсот") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "90"; }

                        else if (strs[i].ToLower().Contains("тысяч")) ret += "000";
                        else if (strs[i].ToLower().Contains("миллион")) ret += "000000";

                        else if (strs[i].ToLower() == "дважды") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "2*"; }
                        else if (strs[i].ToLower() == "трижды") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "3*"; }
                        else if (strs[i].ToLower() == "четырежды") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "4*"; }
                        else if (strs[i].ToLower() == "пятью") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "5*"; }
                        else if (strs[i].ToLower() == "шестью") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "6*"; }
                        else if (strs[i].ToLower() == "семью") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "7*"; }
                        else if (strs[i].ToLower() == "восемью") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "8*"; }
                        else if (strs[i].ToLower() == "девятью") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "9*"; }
                        else if (strs[i].ToLower() == "десятью") { if (ret.Length > 0 && ret[ret.Length - 1] == '0') ret = ret.Substring(0, ret.Length - 1); ret += "10*"; }

                        else if (strs[i].ToLower() == "ноль") ret += "0";
                        else if (strs[i].ToLower() == "нуль") ret += "0";
                        else if (strs[i].ToLower().Contains("плюс")) ret += "+";
                        else if (strs[i].ToLower().Contains("минус")) ret += "-";
                        else if (strs[i].ToLower() == "умножить") ret += "*";
                        else if (strs[i].ToLower() == "разделить") ret += "/";
                        else if (strs[i].ToLower().Contains("откр") && strs[i+1].ToLower().Contains("скобк")) ret += "(";
                        else if (strs[i].ToLower().Contains("закр") && strs[i + 1].ToLower().Contains("скобк")) ret += ")";
                        else if ((strs[i].ToLower() == "в" || strs[i].ToLower() == "во") && i + 2 < strs.Length && strs[i + 2].ToLower() == "степени")
                        {
                            int n = 1;
                            if (strs[i + 1].ToLower() == "второй") n = 2;
                            else if (strs[i + 1].ToLower() == "третьей") n = 3;
                            else if (strs[i + 1].ToLower() == "четвертой") n = 4;
                            else if (strs[i + 1].ToLower() == "пятой") n = 5;
                            else if (strs[i + 1].ToLower() == "шестой") n = 6;
                            else if (strs[i + 1].ToLower() == "седьмой") n = 7;
                            else if (strs[i + 1].ToLower() == "восьмой") n = 8;
                            else if (strs[i + 1].ToLower() == "девятой") n = 9;
                            else if (strs[i + 1].ToLower() == "десятой") n = 10;
                            else
                            {
                                try { n = Convert.ToInt32(strs[i + 1].ToLower()); }
                                catch { return SiaScript.ToScript(executor, @"speak 3;"); }
                            }
                            string dstr = "(" + ret + ")";
                            ret = "";
                            for (int j = 1; j < n; j++) ret += dstr + "*";
                            ret += dstr;
                        }
                        else if ("-+*/".Contains(strs[i])) ret += strs[i];
                        else
                        {
                            try { ret += Convert.ToInt32(strs[i].ToLower()).ToString(); }
                            catch { }
                        }
                    }
                    catch
                    {
                        //
                    }

                    



                }

                string sret = Eval(ret);
                if (sret == ret)
                    return SiaScript.ToScript(executor, @"speak 2;");
                return SiaScript.ToScript(executor, @"speak " + sret + ";");

            }

            return null;
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
    }


    public class WikiSearchPhraseTranslator : AbstractPhraseTranslator
    {


        public static List<string[]> SearchResults = new List<string[]>();
        public static int SelectedResult = 0;
        public static string Query = "";

        public WikiSearchPhraseTranslator(IPhraseTranslator parent) :
            base(parent, ScriptState.Replacement, false, false)
        {
        }

        public override SiaScript Execute(SiaExecutor executor, string phrase)
        {


            Phrase = phrase;
            if(CanExecute())
            {
                SelectedResult = 0;
                Query = "";
                SearchResults = new List<string[]>();
                string[] strs = phrase.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                string title = "";
                for(int i=0; i<strs.Length; i++)
                {
                    if (strs[i].ToLower().Contains("тако"))
                    {
                        for(int j=i+1; j<strs.Length; j++)
                        title += strs[j] + " ";
                        break;
                    }
                }
                if (title.Length > 0)
                {
                    title = title.Trim();

                    if (Query != title)
                        SearchResults = GetWikiSearch("https://ru.wikipedia.org/w/api.php?action=opensearch&prop=info&format=xml&inprop=url&search=" +
                            System.Net.WebUtility.UrlEncode(title));

                    Query = title;

                    if (SearchResults.Count > 0)
                    {
                        

                        return SiaScript.ToScript(executor, @"

include ipt DetailWikiSearch;
speak 1& " + SearchResults.Count +@" определений;
speak 31&32& " + SearchResults[SelectedResult][2] + @";
set va $voice;
");
                    }

                }
                else
                {
                    return SiaScript.ToScript(executor, @"speak 2;");
                }
            }

            return null;
        }

        public List<string[]> GetWikiSearch(string title)
        {
            List<string[]> retList = new List<string[]>();
            try
            {
                XmlDocument myXmlDocument = new XmlDocument();
                myXmlDocument.Load(title);
                XmlNodeList elemList = myXmlDocument.GetElementsByTagName("Item");

                for (int i = 0; i < elemList.Count; i++)
                {
                    string[] strs = new string[3];
                    for (int j = 0; j < 3; j++) strs[j] = "";

                    for (int j = 0; j < elemList[i].ChildNodes.Count; j++)
                    {

                        if (elemList[i].ChildNodes[j].Name == "Url") strs[0] = elemList[i].ChildNodes[j].InnerText;
                        if (elemList[i].ChildNodes[j].Name == "Description") strs[1] = elemList[i].ChildNodes[j].InnerText;
                        if (elemList[i].ChildNodes[j].Name == "Text") strs[2] = elemList[i].ChildNodes[j].InnerText;

                    }
                    retList.Add(strs);
                }

            }
            catch
            {
                //
            }

            return retList;
        }

        
    }


    public class DetailWikiSearchPhraseTranslator : AbstractPhraseTranslator
    {
        public static string PageText = ""; 

        public DetailWikiSearchPhraseTranslator(IPhraseTranslator parent) :
            base(parent, ScriptState.Override, false, false)
        {

        }

        public override SiaScript Execute(SiaExecutor executor, string phrase)
        {
           
            Phrase = phrase;
            if (CanExecute())
            {
                PageText = "";
                if (phrase.ToLower().Contains("да") ||
                    phrase.ToLower().Contains("точно") ||
                    phrase.ToLower().Contains("ага") ||
                    phrase.ToLower().Contains("именно"))
                {
                    GetText();
                    string text = GetInfo();

                    if(text != null)
                    return SiaScript.ToScript(executor, @"
include ipt ManipWikiSearch;
speak " + text + @";
set va $voice;
");
                    return SiaScript.ToScript(executor, @"speak50&51&52&53&54;");
                }
                
                
                    WikiSearchPhraseTranslator.SelectedResult ++;
                    if (WikiSearchPhraseTranslator.SelectedResult < WikiSearchPhraseTranslator.SearchResults.Count)
                    {
                   
                        return SiaScript.ToScript(executor, @"
speak 30&31&32& " + WikiSearchPhraseTranslator.SearchResults[WikiSearchPhraseTranslator.SelectedResult][2] + @"
");
                    }
                    return SiaScript.ToScript(executor, @"speak 40&41;");               
            }

            return null;
        }

        public string GetInfo()
        {
            string text = WikiSearchPhraseTranslator.SearchResults[WikiSearchPhraseTranslator.SelectedResult][1];

            try
            {
                text = Regex.Replace(text, @"(.*?)", string.Empty);
                text = Regex.Replace(text, @"(.*?)", string.Empty);
                text = Regex.Replace(text, @"(.*?)", string.Empty).Substring(text.IndexOf(") —", StringComparison.Ordinal) + 1);
            }
            catch (Exception)
            {
                try
                {
                    text = Regex.Replace(text, @"(.*?)", string.Empty);
                    text = Regex.Replace(text, @"(.*?)", string.Empty);
                    text = Regex.Replace(text, @"(.*?)", string.Empty).Substring(text.IndexOf("—", StringComparison.Ordinal) + 1);
                }
                catch (Exception)
                {

                    try
                    {
                        text = Regex.Replace(text, @"(.*?)", string.Empty);
                        text = Regex.Replace(text, @"(.*?)", string.Empty);
                        text =
                            Regex.Replace(text, @"(.*?)", string.Empty)
                                .Substring(text.IndexOf("-", StringComparison.Ordinal) + 1);
                        if (
                            text.Trim() ==
                                WikiSearchPhraseTranslator.SearchResults[WikiSearchPhraseTranslator.SelectedResult][2].Trim())
                            return null;

                    }
                    catch
                    {
                        //

                    }
                }
                //
            }

            text = WikiSearchPhraseTranslator.SearchResults[WikiSearchPhraseTranslator.SelectedResult][2] + " - это " + text;

            return text;
        }

        public void GetText()
        {
            try
            {
                if (WikiSearchPhraseTranslator.SelectedResult < WikiSearchPhraseTranslator.SearchResults.Count)
                {
                    HtmlWeb web = new HtmlWeb();
                    HtmlAgilityPack.HtmlDocument doc =
                        web.Load("https://ru.wikipedia.org/w/index.php?title=" +
                                 System.Net.WebUtility.UrlEncode(
                                     WikiSearchPhraseTranslator.SearchResults[WikiSearchPhraseTranslator.SelectedResult]
                                         [2]) + "&printable=yes");

                    var root = doc.DocumentNode;
                    var sb = new StringBuilder();
                    foreach (var node in root.DescendantNodesAndSelf())
                    {
                        if (!node.HasChildNodes)
                        {
                            string text = node.InnerText;
                            if (!string.IsNullOrEmpty(text))
                                sb.AppendLine(text.Trim());
                        }
                    }
                    string stext = sb.ToString();

                    while (stext.IndexOf("\r\n\r\n") != -1) stext = stext.Replace("\r\n\r\n", "\r\n");
                    if (stext.IndexOf("&#160;—") != -1)
                        stext =
                            stext.Substring(stext.IndexOf("&#160;—") + "&#160;—".Length);

                    while (stext.IndexOf("[") != -1 && stext.IndexOf("]") != -1)
                        stext = stext.Remove(stext.IndexOf("["), stext.IndexOf("]") - stext.IndexOf("[") + 1);

                    stext = stext.Substring(0, stext.IndexOf("Содержание"));

                    PageText = stext;

                    File.WriteAllText(@"E:\1\33.txt", PageText);
                }
            }
            catch
            {
                //
            }
           
        }

    }


    public class ManipWikiSearchPhraseTranslator : AbstractPhraseTranslator
    {
        public static string PageText = "";

        public ManipWikiSearchPhraseTranslator(IPhraseTranslator parent) :
            base(parent, ScriptState.Override, false, false)
        {

        }

        public override SiaScript Execute(SiaExecutor executor, string phrase)
        {

            Phrase = phrase;
            if (CanExecute())
            {
                if (Phrase.ToLower().Contains("открой") ||
                    Phrase.ToLower().Contains("покажи") ||
                    Phrase.ToLower().Contains("на экран"))
                {
                    if (Phrase.ToLower().Contains("печат"))
                    
                        return SiaScript.ToScript(executor, @"
task " + "https://ru.wikipedia.org/w/index.php?title=" +
                                                            System.Net.WebUtility.UrlEncode(
                                                                WikiSearchPhraseTranslator.SearchResults[
                                                                    WikiSearchPhraseTranslator.SelectedResult]
                                                                    [2]) + "&printable=yes; speak 0;");                  
                    
                    
                        return SiaScript.ToScript(executor, @"
task " + "https://ru.wikipedia.org/w/index.php?title=" +
                                                            System.Net.WebUtility.UrlEncode(
                                                                WikiSearchPhraseTranslator.SearchResults[
                                                                    WikiSearchPhraseTranslator.SelectedResult]
                                                                    [2]) + "; speak 0;");
                }
                
                return SiaScript.ToScript(executor, @"
speak " + DetailWikiSearchPhraseTranslator.PageText + ";");

            }

            return null;
        }

 

    }

}
