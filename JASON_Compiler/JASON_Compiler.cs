using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JASON_Compiler
{
    public static class JASON_Compiler
    {
        public static ScannerPhase Jason_Scanner = new ScannerPhase();
        public static List<Token> TokenStream = new List<Token>();
        public static Node treeroot;

        public static void Start_Compiling(string[] SourceCode) //character by character
        {
            //Scanner
            Jason_Scanner.scanning(SourceCode,ref TokenStream);
            //Parser
            
        } 
    }
}
