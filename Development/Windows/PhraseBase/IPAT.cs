using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedSiaCore.Core;
using RedSiaCore.IPAT;
using RedSiaCore.IPT;

namespace PhraseBase
{
    public class TestPhraseAdditionalTranslator : AbstractPhraseAdditionalTranslator
    {

        public override string Execute(SiaExecutor executor, string phrase)
        {
            return phrase;
        }

        public TestPhraseAdditionalTranslator(IPhraseTranslator parent) : base(parent)
        {

        }
    }
}
