using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chm_lab_1
{
    class RandomWIthBlackJackAndHoe
    {
        private static long _seed = Environment.TickCount;
        private const long _magicNumberWithBlackJackAndHoe = 1284865837;




        public static void setSeed(int seed)
        {
            _seed = (seed - 1);
        }

        public static int Next(int absBorder)
        {
            long varWithBlackJackAndHoe = _magicNumberWithBlackJackAndHoe*_seed;
            varWithBlackJackAndHoe += 148127659533*_seed + varWithBlackJackAndHoe;
            _seed = varWithBlackJackAndHoe + 1;
            int randomVarWithBlackJackAndHoe = (int) (_seed >> 31) >> 1;
            return Math.Sign(varWithBlackJackAndHoe)*(randomVarWithBlackJackAndHoe % (absBorder + 1));
        }
    }
}
