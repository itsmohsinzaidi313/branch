using RMSEnumerations;

namespace Branch.Classes
{
    public class Commission
    {
        public double Amount { get; set; }
        public Units AmountUnit { get; set; }
        internal bool Enabled { get; set; }
    }
}
