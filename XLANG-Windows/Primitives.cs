using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLANG_Windows
{
    public class XInt: XType
    {
        public XInt():base("int")
        {
            Functions["+"] = new XFunctionExternal();
            Functions["-"] = new XFunctionExternal();
            Functions["*"] = new XFunctionExternal();
            Functions["/"] = new XFunctionExternal();
            
        }
    }
}
