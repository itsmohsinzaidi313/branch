using System;

namespace Branch.Classes
{
    public class Customer
    {
        public string Name { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public override bool Equals(object obj)
        {
            return obj is Customer customer &&
                   Address == customer.Address &&
                   Contact == customer.Contact &&
                   Name == customer.Name;
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(Address);
            hashCode.Add(Name);
            hashCode.Add(Contact);
            return HashCode.Combine(hashCode);
        }
    }
}
