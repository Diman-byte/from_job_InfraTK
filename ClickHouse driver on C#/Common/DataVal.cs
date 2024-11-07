using System;

namespace Common
{
    public class DataVal
    {
        public uint Sn;
        public DateTime DateTime;
        public double Val;
        public bool IsGood;
        public int Status;
    }

    public delegate void CommonEventHandler();
}