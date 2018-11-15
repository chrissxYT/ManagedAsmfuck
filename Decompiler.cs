using System.IO;
using System.Collections.Generic;
using static ManagedAsmfuck.Consts;

namespace ManagedAsmfuck
{
    static class Decompiler
    {
        public static void Decompile(string input, string output)
        {
            Stream i = File.Open(input, FileMode.Open, FileAccess.Read);
            List<string> lines = new List<string>();
            int j;
            while((j = i.ReadByte()) != -1)
            {
                if(j == 0)
                    lines.Add(nop);
                else if(j == 1)
                    lines.Add(inc);
                else if(j == 2)
                    lines.Add(dec);
                else if(j == 3)
                    lines.Add(tsl);
                else if(j == 4)
                    lines.Add(tsr);
                else if(j == 5)
                    lines.Add(sjp);
                else if(j == 6)
                    lines.Add(jpb);
                else if(j == 7)
                    lines.Add(rac);
                else if(j == 8)
                    lines.Add(wac);
                else
                    lines.Add(nop);
            }
            File.WriteAllLines(output, lines);
        }
    }
}