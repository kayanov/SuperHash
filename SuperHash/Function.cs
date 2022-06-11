using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperHash
{
    internal class Function
    {
        public string name;
        public Func<byte[], uint> func;
        public long totalTime,
            totalCollisions,
            totalUniformity;

        public Function(Func<byte[], uint> func, string name)
        {
            this.func = func;
            this.name = name;
        }
    }
}
