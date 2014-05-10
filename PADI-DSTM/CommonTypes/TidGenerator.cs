using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public class TidGenerator
    {
        static private int count = 0;

        public static TID GenerateTID()
        {
            return new TID(TidGenerator.count++);
        }
    }
}
