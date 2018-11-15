using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedAsmfuck
{
    static class Consts
    {
        public const string nop = "nop";
        public const string inc = "inc";
        public const string dec = "dec";
        public const string tsl = "tsl";
        public const string tsr = "tsr";
        public const string sjp = "sjp";
        public const string jpb = "jpb";
        public const string rac = "rac";
        public const string wac = "wac";
        public const string nullstr = "";
        public const int tapelen = 50 * 1024 * 1024; // 50 MiB of tape length should be enough
        public const int startpos = tapelen / 2;
    }
}
