using Branch.Classes.Discounts;
using Branch.POSSettings;
using RMSEnumerations;
using System;
using System.Collections.Generic;

namespace Branch.Classes.Menu
{
    public abstract class MenuItem
    {
        public int ItemId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public Category Category { get; set; }
        public double UnitPrice { get; set; } = 0;
        public double Quantity { get; set; }
        public List<Addon> Addons { get; set; }
        public Tax Tax { get; set; } = new Tax { Percentage = 0 };
        public ItemDiscount Discount { get; set; } = new ItemDiscount { Amount = 0, AmountUnit = Units.Percentage};
        public double TotalDiscount
        {
            get
            {
                if (Settings.TaxBeforeDiscount)
                {
                    return Discount.ApplyExclusive(SubTotal + TaxAmount);
                }
                else
                {
                    return Discount.ApplyExclusive(SubTotal);
                }
            }
        }
        public double SubTotal
        {
            get
            {
                double addonsAmount = 0;
                Addons.ForEach(x => addonsAmount += x.Price);
                return (UnitPrice * Quantity) + addonsAmount;
            }
        }
        public double TaxAmount
        {
            get
            {
                return Tax.ApplyExclusive(SubTotal);
            }
        }
        public double NetAmount
        {
            get
            {
                return (SubTotal + TaxAmount) - TotalDiscount;
            }
        }
        public bool Canceled { get; set; }
        public override bool Equals(object obj)
        {
            return obj is MenuItem item &&
                   ItemId == item.ItemId &&
                   Name == item.Name &&
                   UnitPrice == item.UnitPrice &&
                   Quantity == item.Quantity &&
                   EqualityComparer<List<Addon>>.Default.Equals(Addons, item.Addons) &&
                   EqualityComparer<Tax>.Default.Equals(Tax, item.Tax) &&
                   EqualityComparer<ItemDiscount>.Default.Equals(Discount, item.Discount) &&
                   TotalDiscount == item.TotalDiscount &&
                   SubTotal == item.SubTotal &&
                   TaxAmount == item.TaxAmount &&
                   NetAmount == item.NetAmount;
        }
        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(ItemId);
            hash.Add(Name);
            hash.Add(UnitPrice);
            hash.Add(Quantity);
            hash.Add(Addons);
            hash.Add(Tax);
            hash.Add(Discount);
            hash.Add(TotalDiscount);
            hash.Add(SubTotal);
            hash.Add(TaxAmount);
            hash.Add(NetAmount);
            return hash.ToHashCode();
        }
    }
}
