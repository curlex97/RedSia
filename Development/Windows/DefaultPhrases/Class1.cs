using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultPhrases
{
   public interface IPhraseTranslator
    {
        SiaScript Execute(string phrase);

        string GetClassName();
    }

}
