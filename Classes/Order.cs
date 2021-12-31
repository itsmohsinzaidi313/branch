using Branch.Classes.Discounts;
using Branch.Classes.Menu;
using Branch.Exceptions;
using Branch.Exceptions.OrderExceptions;
using Branch.POSSettings;
using RMSEnumerations;
using System;
using System.Collections.Generic;

namespace Branch.Classes
{
    public class Order : OrderContext
    {
        public Order() { }
        public Order(int orderNo)
        {
            var order = OrderHandler.GetOrder(orderNo);
            OrderNo = order.OrderNo;
            TokenNo = order.TokenNo;
            OrderDate = order.OrderDate;
            Waiter = order.Waiter;
            Customer = order.Customer;
            Waiter = order.Waiter;
            Rider = order.Rider;
            Table = order.Table;
            OrderType = order.OrderType;
            PaymentMode = order.PaymentMode;
            Counter = order.Counter;
            User = order.User;
            Persons = order.Persons;
            Shift = order.Shift;
            DeliveryCharges = order.DeliveryCharges;
            ExtraCharges = order.ExtraCharges;
            Items = order.Items;
            Discount = order.Discount;
        }
        internal List<MenuItem> Items { get; set; } = new List<MenuItem>();
        public GeneralDiscount Discount { get; set; } = new GeneralDiscount { DiscountType = DiscountType.Limited, Amount = 0.0, AmountUnit = Units.Amount, From = DateTime.Now, To = DateTime.Now };
        public double SubTotal
        {
            get
            {
                double amount = 0;
                foreach (MenuItem item in Items)
                    amount += item.SubTotal;
                return amount;
            }
        }
        public double TotalTax
        {
            get
            {
                double amount = 0.00;
                foreach (MenuItem item in Items)
                    amount += item.TaxAmount;
                return amount;
            }
        }
        public double NetAmount
        {
            get
            {
                double amount = 0.00;
                foreach (MenuItem item in Items)
                    amount += item.NetAmount;
                return amount;
            }
        }
        public double TotalDiscount
        {
            get
            {
                double amount = 0.00;
                amount += ItemDiscounts;
                amount += OrderDiscount;
                return amount;
            }
        }
        private double ItemDiscounts
        {
            get
            {
                double amount = 0.00;
                if (!Settings.IncludeItemDiscountInOrderDiscount)
                    Items.ForEach(x => amount += x.TotalDiscount);
                return amount;
            }
        }
        private double OrderDiscount
        {
            get
            {
                if (Settings.TaxBeforeDiscount)
                {
                    return Discount.ApplyExclusive(SubTotal + TotalTax);
                }
                else
                {
                    return Discount.ApplyExclusive(SubTotal);
                }
            }
        }
        public override bool Equals(object obj)
        {
            return obj is Order order &&
                   OrderNo == order.OrderNo &&
                   TokenNo == order.TokenNo &&
                   OrderDate == order.OrderDate &&
                   EqualityComparer<Customer>.Default.Equals(Customer, order.Customer) &&
                   EqualityComparer<Waiter>.Default.Equals(Waiter, order.Waiter) &&
                   EqualityComparer<Rider>.Default.Equals(Rider, order.Rider) &&
                   EqualityComparer<Table>.Default.Equals(Table, order.Table) &&
                   OrderType == order.OrderType &&
                   PaymentMode == order.PaymentMode &&
                   EqualityComparer<Counter>.Default.Equals(Counter, order.Counter) &&
                   EqualityComparer<User>.Default.Equals(User, order.User) &&
                   Persons == order.Persons &&
                   DeliveryCharges == order.DeliveryCharges &&
                   ExtraCharges == order.ExtraCharges &&
                   EqualityComparer<List<MenuItem>>.Default.Equals(Items, order.Items) &&
                   EqualityComparer<Discount>.Default.Equals(Discount, order.Discount) &&
                   SubTotal == order.SubTotal &&
                   TotalTax == order.TotalTax &&
                   NetAmount == order.NetAmount &&
                   TotalDiscount == order.TotalDiscount &&
                   ItemDiscounts == order.ItemDiscounts &&
                   OrderDiscount == order.OrderDiscount;
        }
        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(OrderNo);
            hash.Add(TokenNo);
            hash.Add(OrderDate);
            hash.Add(Customer);
            hash.Add(Waiter);
            hash.Add(Rider);
            hash.Add(Table);
            hash.Add(OrderType);
            hash.Add(PaymentMode);
            hash.Add(Counter);
            hash.Add(User);
            hash.Add(Persons);
            hash.Add(DeliveryCharges);
            hash.Add(ExtraCharges);
            hash.Add(Items);
            hash.Add(Discount);
            hash.Add(SubTotal);
            hash.Add(TotalTax);
            hash.Add(NetAmount);
            hash.Add(TotalDiscount);
            hash.Add(ItemDiscounts);
            hash.Add(OrderDiscount);
            return hash.ToHashCode();
        }
        public void AddItem(MenuItem menuItem)
        {
            ValidateItem(menuItem);
            if (Items.Exists(x => x.Equals(menuItem)))
                Items.Find(x => x.Name == menuItem.Name).Quantity++;
            else
                Items.Add(menuItem);
        }
        public void ReduceItem(MenuItem menuItem)
        {
            ValidateItem(menuItem);
            if (Items.Exists(x => x.Equals(menuItem)))
            {
                var item = Items.Find(x => x.Name == menuItem.Name);
                if (item.Quantity > 0)
                    item.Quantity--;
            }
        }
        public void RemoveItem(MenuItem menuItem)
        {
            ValidateItem(menuItem);
            if (Items.Exists(x => x.Name == menuItem.Name))
                Items.Remove(Items.Find(x => x.Name == menuItem.Name));
        }
        public void SetQuantity(string name, double quantity)
        {
            if (string.IsNullOrEmpty(name) || quantity <= 0)
                throw new Exception(ExceptionMessages.ErrorSetItemQuantity);
            if (Items.Exists(x => x.Name == name))
                Items.Find(x => x.Name == name).Quantity = quantity;
        }
        public void Save()
        {
            ValidateSaveOrder();
            OrderHandler.SaveOrder(this);
        }
        private void ValidateItem(MenuItem menuItem)
        {
            if (menuItem is null || string.IsNullOrEmpty(menuItem.Name) || menuItem.Quantity <= 0 || menuItem.UnitPrice <= 0 || string.IsNullOrEmpty(menuItem.Category.Name))
                throw new ItemException(ExceptionMessages.InvalidItem);
        }
        private void ValidateSaveOrder()
        {
            if (Items is null || Items.Count <= 0)
                throw new OrderSaveException(ExceptionMessages.MissingItems);
            if (OrderDate.CompareTo(DateTime.Now) < 0)
                throw new OrderSaveException(ExceptionMessages.InvalidOrderDate);
            if (OrderNo == 0)
                throw new OrderSaveException(ExceptionMessages.MissingOrderNo);
            if (TokenNo == 0)
                throw new OrderSaveException(ExceptionMessages.MissingTokenNo);
            if (Counter is null || string.IsNullOrEmpty(Counter.Name))
                throw new OrderSaveException(ExceptionMessages.MissingCounter);
            if (User is null || string.IsNullOrEmpty(User.Name))
                throw new OrderSaveException(ExceptionMessages.MissingUser);
            if (Shift is null || string.IsNullOrEmpty(Shift.ShiftNumber))
                throw new OrderSaveException(ExceptionMessages.MissingShift);

            switch (OrderType)
            {
                case OrderType.Undefined:
                    throw new OrderSaveException(ExceptionMessages.MissingOrderType);
                case OrderType.DineInIndoor:
                    if (Waiter is null || string.IsNullOrEmpty(Waiter.Name) || string.IsNullOrEmpty(Table.Name))
                        throw new OrderSaveException(ExceptionMessages.MissingWaiter);
                    break;
                case OrderType.DineInOutdoor:
                    break;
                case OrderType.TakeAway:
                    if (string.IsNullOrEmpty(Customer.Name) || string.IsNullOrEmpty(Customer.Contact))
                        throw new OrderSaveException(ExceptionMessages.MissingCustomer);
                    break;
                case OrderType.Delivery:
                    if (string.IsNullOrEmpty(Customer.Name) || string.IsNullOrEmpty(Customer.Contact) || string.IsNullOrEmpty(Customer.Address))
                        throw new OrderSaveException(ExceptionMessages.MissingCustomer);
                    break;
                default:
                    break;
            }
        }
    }
}
