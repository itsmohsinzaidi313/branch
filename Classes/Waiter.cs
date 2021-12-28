using RMSEnumerations;

namespace Branch.Classes
{
    public class Waiter
    {
        internal int Id { get; set; }
        public string Name { get; set; }
        public Commission Commission { get; set; }
        internal bool Enabled { get; set; }
    }
}
