using RMSEnumerations;

namespace Branch.Classes
{
    public class Rider
    {
        internal int RiderId { get; set; }
        public string Name { get; set; }
        public RiderStatus Status { get; set; }
        public Commission Commission { get; set; }
        internal bool Enabled { get; set; }
    }
}
