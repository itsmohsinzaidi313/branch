using RMSEnumerations;

namespace Branch.Classes
{
    public class Commission
    {
        internal int CommissionId { get; set; }
        public double Amount { get; set; }
        public Units AmountUnit { get; set; }
        internal bool Enabled { get; set; }
    }
}
