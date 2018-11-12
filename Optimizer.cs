using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace ManagedAsmfuck
{
    static class Optimizer
    {
        static List<byte> DeNop(List<byte> i)
        {
            List<byte> o = new List<byte>();
            foreach(byte b in i)
            {
                if(b != 0)
                {
                    o.Add(b);
                }
            }
            return o;
        }

        static (int, int) UsedTapeRange(List<byte> bin)
        {
            List<int> pos = new List<int>();
            int tp = int.MaxValue / 2;
            foreach(byte b in bin)
            {
                if(b == 1 || b == 2 || b == 7 || b == 8)
                    pos.Add(tp);
                else if(b == 3)
                    tp--;
                else if(b == 4)
                    tp++;
            }
            return (pos.Min(), pos.Max());
        }

        static List<byte> MoveReduce(List<byte> i, (int, int) used)
        {
            List<byte> o = new List<byte>();
            int tp = int.MaxValue / 2;
            int mvs = 0;
            foreach(byte b in i)
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

        public static void Optimize(string input, string output)
        {
            List<byte> bin = new List<byte>(File.ReadAllBytes(input));
            bin = DeNop(bin);
            bin = MoveReduce(bin, UsedTapeRange(bin));
            File.WriteAllBytes(output, bin.ToArray());
        }
    }
}