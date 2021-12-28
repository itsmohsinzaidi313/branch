namespace Branch.Classes
{
    public class Counter
    {
        internal int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string UUID { get; set; }
        internal bool Enabled { get; set; }
    }
}
