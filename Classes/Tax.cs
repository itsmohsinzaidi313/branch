using System;

namespace Branch.Classes
{
    public class Tax
    {
        internal int TaxId { get; set; }
        public string Name { get; set; }
        public double Percentage { get; set; }
        public double ApplyExclusive(double amount) => amount + (Percentage / 100);
        public double ApplyInclusive(double amount) => amount + (amount + (Percentage / 100));
        public override bool Equals(object obj) => obj is Tax tax &&
                   Percentage == tax.Percentage;
        public override int GetHashCode() => HashCode.Combine(Percentage);
    }
}
