using System;

namespace Common
{
    public class DataVal
    {
        public uint sn;
        public DateTime DateTime;
        public double Val;
        public bool IsGood;
        public int Status;
    }

    public delegate void CommonEventHandler();
}