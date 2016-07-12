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
            //Data type (named int)
            //Struct size: 4 bytes
            //Alignment: 4 bytes
            string testProg = @"
             struct int 4.4 {
                 int +(int other);
                 int -(int other);
                 int *(int other);
                 int /(int other);
             };
";
            Parser parser = new Parser(testProg);
            
        }
    }
}
