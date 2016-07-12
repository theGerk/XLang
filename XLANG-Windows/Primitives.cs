using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLANG_Windows
{
    public static class CoreLibrary
    {
        
        static bool init = false;
        public static void Initialize()
        {
            if(init)
            {
                return;
            }
            init = true;
            

        }
    }
}
