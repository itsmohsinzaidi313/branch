using System.Collections.Generic;

namespace Branch.Classes.Menu
{
    public class Item : MenuItem
    {
        public List<Addon> Addons { get; set; }
        public override double SubTotal
        {
            get
            {
                double addonsAmount = 0;
                Addons.ForEach(x => addonsAmount += x.Price);
                return (UnitPrice * Quantity) + addonsAmount;
            }
        }
    }
}
