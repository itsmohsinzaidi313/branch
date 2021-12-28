using RMSEnumerations;
using System;

namespace Branch.Classes
{
    public abstract class OrderContext
    {
        public int OrderNo { get; set; }
        public int TokenNo { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public Customer Customer { get; set; } = new Customer();
        public Waiter Waiter { get; set; } = new Waiter();
        public Rider Rider { get; set; } = new Rider();
        public Table Table { get; set; } = new Table();
        public OrderType OrderType { get; set; } = OrderType.Undefined;
        public PaymentMode PaymentMode { get; set; } = PaymentMode.Undefined;
        public Counter Counter { get; set; } = new Counter();
        public User User { get; set; } = new User();
        public int Persons { get; set; }
        public Shift Shift { get; set; } = new Shift();
        public double DeliveryCharges { get; set; }
        public double ExtraCharges { get; set; }
    }
}
