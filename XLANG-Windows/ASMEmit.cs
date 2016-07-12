using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace XLANG_Windows
{
    public class ASMEmit
    {
        public static void Emit(Parser parser, Stream output)
        {
            BinaryWriter mwriter = new BinaryWriter(output);
            mwriter.Write(0); //Version 0
            
        }
    }
}
