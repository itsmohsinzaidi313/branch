using System;
using RMSEnumerations;

namespace Branch.Classes
{
    public class Table
    {
        public string Name { get; set; } = string.Empty;
        public TableStatus Status { get; set; } = TableStatus.Undefined;

        public override bool Equals(object obj)
        {
            return obj is Table table &&
                   Name == table.Name &&
                   Status == table.Status;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Status);
        }
    }
}
