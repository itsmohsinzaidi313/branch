using System;
using System.Collections.Generic;

namespace Branch.Classes.Menu
{
    public class Addon
    {
        internal int AddonId { get; set; }
        internal int ItemId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool Selected { get; set; }
        internal bool Enabled { get; set; }
        internal bool Canceled { get; set; }
    }
}
