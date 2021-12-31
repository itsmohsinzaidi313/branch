using RMSEnumerations;

namespace Branch.Classes
{
    public class Rider
    {
        public string Name { get; set; }
        public RiderStatus Status { get; set; }
        public Commission Commission { get; set; }
    }
}
