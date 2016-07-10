using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLANG_Windows
{
    class Program
    {
        //XLANG compiler
        static void Main(string[] args)
        {
            string testProg = @"
             int eger = 5+2;
             
";
            Parser parser = new Parser(testProg);
            
        }
    }
}
