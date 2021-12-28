using System;
using RMSEnumerations;

namespace Branch.Classes
{
    public class Table
    {
        public int TableId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public TableStatus Status { get; set; } = TableStatus.Undefined;

        public override bool Equals(object obj)
        {
            return obj is Table table &&
                   TableId == table.TableId &&
                   Name == table.Name &&
                   Status == table.Status;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TableId, Name, Status);
        }
    }
}
