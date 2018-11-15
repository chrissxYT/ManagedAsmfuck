using System.Linq;
using System.Collections.Generic;
using System.IO;
using static ManagedAsmfuck.Consts;

namespace ManagedAsmfuck
{
    static class Optimizer
    {
        static List<byte> DeNop(List<byte> i)
        {
            i.RemoveAll((b) => b == 0);
            return i;
        }

        static List<byte> MoveReduce(List<byte> i, (int, int) used)
        {
            List<byte> o = new List<byte>();
            int tp = startpos;
            int mvs = 0;
            foreach (byte b in i)
            {
                if(b == 3 && tp - 1 < used.Item1)
                    mvs++;
                else if(b == 3 && mvs < 0)
                    mvs++;
                else if(b == 4 && tp + 1 > used.Item2)
                    mvs--;
                else if(b == 4 && mvs > 0)
                    mvs--;
                else
                    o.Add(b);
            }
            while(o.Last() == 3 || o.Last() == 4)
                o.RemoveAt(o.Count - 1);
            return o;
        }

        static List<byte> SimpleWriteGC(List<byte> i, (int, int) used)
        {
            List<byte> o = new List<byte>();
            int tp = startpos;
            foreach (byte b in i)
                if ((b != 1 && b != 2 && b != 7) || (tp >= used.Item1 && tp <= used.Item2))
                    o.Add(b);
            return o;
        }

        public static void Optimize(string input, string output)
        {
            List<byte> bin = new List<byte>(File.ReadAllBytes(input));
            bin = DeNop(bin);
            bin = MoveReduce(bin, ASMVM.MovedTapeRange(bin));
            bin = SimpleWriteGC(bin, ASMVM.ReadTapeRange(bin));
            File.WriteAllBytes(output, bin.ToArray());
        }
    }
}