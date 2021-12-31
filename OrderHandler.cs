using Microsoft.EntityFrameworkCore;
using Branch.Classes;
using RMSEnumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using POSDatabaseModel;
using POSDatabaseModel.Models;

namespace Branch
{
    public static class OrderHandler
    {
        public static Order GetOrder(int orderNo)
        {
            using var context = new Database().Context;
            var master = context.Find<DbSalesMaster>(orderNo);
            return new Order();
        }
        public static void SaveOrder(Order order)
        {
            using POSContext context = new Database().Context;
            var branch = Helpers.GetBranch();
            var discount = order.Discount;
            var shift = Helpers.GetShift();
            var master = new DbSalesMaster
            {
                OrderNo = Helpers.GetNewOrderNumber(),
                TokenNo = Helpers.GetNewTokenNumber(),
                BillCreated = false,
                NetAmount = order.NetAmount,
                SubTotal = order.SubTotal,
                PaymentMode = order.PaymentMode,
                SaleDate = order.OrderDate,
                TaxId = Helpers.GetDbTax().Id,
                OrderType = order.OrderType,
                OrderStatus = OrderStatus.Pending,                
            };
            context.DbSalesMaster.Add(master);
            master.Id = context.SaveChanges();

            order.Items.ForEach(x => context.DbSalesDetails.Add(new DbSalesDetails
            {
                SalesMasterId = master.Id,
                Quantity = x.Quantity,
                Canceled = false,
            }));
            context.SaveChanges();
        }
        public static void PayOrder(string orderNo, PaymentMode paymentMode)
        {
            using POSContext context = new Database().Context;
            var master = context.Find<DbSalesMaster>(orderNo);
            master.PaymentMode = paymentMode;
            context.SaveChanges();
        }
        public static void ChangeCustomer(int orderNo, Customer customer)
        {
            using POSContext context = new Database().Context;
            var master = (from DbSalesMaster x in context.DbSalesMaster
                          where x.OrderNo == orderNo
                          select x).FirstOrDefault();
            context.SaveChanges();
        }
        public static void ChangeWaiter(int orderNo, Waiter waiter)
        {
            using POSContext context = new Database().Context;
            var master = (from DbSalesMaster x in context.DbSalesMaster
                          where x.OrderNo == orderNo
                          select x).FirstOrDefault();
            context.SaveChanges();
        }
        public static void ChangeTable(int orderNo, Table table)
        {
            using POSContext context = new Database().Context;
            var master = (from DbSalesMaster x in context.DbSalesMaster
                          where x.OrderNo == orderNo
                          select x).FirstOrDefault();
            context.SaveChanges();
        }
        public static void ChangeRider(int orderNo, Rider rider)
        {
            using POSContext context = new Database().Context;
            var master = (from DbSalesMaster x in context.DbSalesMaster
                          where x.OrderNo == orderNo
                          select x).FirstOrDefault();
            context.SaveChanges();
        }
        public static void ChangeCounter(int orderNo, int counterId)
        {
            using POSContext context = new Database().Context;
            var master = context.Find<DbSalesMaster>(orderNo);
            master.CounterId = counterId;
            context.SaveChanges();
        }
        public static void UpdateItems(POSContext context, Order order)
        {
                var dbMaster = (from x in context.DbSalesMaster
                                .Include(x => x.SalesDetails)
                                where x.OrderNo == order.OrderNo
                                select x)
                                .FirstOrDefault();

                var dbDetail = dbMaster.SalesDetails;

                dbDetail.ForEach(x =>
                {
                    if (order.Items.Exists(a => a.Name == x.MenuDetails.Menu.Name))
                    {
                        var item = (from a in order.Items where a.Name == x.MenuDetails.Menu.Name select x).FirstOrDefault();
                        double qtyDiff = item.Quantity - x.Quantity;
                        if (qtyDiff > 0)
                        {
                            x.Quantity += Math.Abs(qtyDiff);
                            return;
                        }
                        else if (qtyDiff < 0)
                        {
                            x.Quantity -= Math.Abs(qtyDiff);
                            return;
                        }

                        if (qtyDiff != 0)
                        {
                            dbMaster.SubTotal = order.SubTotal;
                            dbMaster.SaleDate = System.DateTime.Now;
                        }
                    }
                });

                dbDetail.ForEach(x =>
                {
                    if (!order.Items.Exists(a => a.Name == x.MenuDetails.Menu.Name))
                    {
                        x.Canceled = true;
                    }
                });
                context.SaveChanges();
        }
        public static void UpdateDeals(POSContext context, Order order)
        {
                var dbMaster = (from x in context.DbSalesMaster
                                where x.OrderNo == order.OrderNo
                                select x).FirstOrDefault();

                var dbDetail = (from x in context.DbSalesDetails
                                join a in context.DbMenus on x.MenuDetails.MenuId equals a.Id
                                where (x.SalesMasterId == dbMaster.Id) && (a.ItemType == ItemType.Deal)
                                select x)
                                .ToList();

                dbDetail.ForEach(x =>
                {
                    if (order.Items.Exists(a => a.Name == x.MenuDetails.Menu.Name))
                    {
                        var item = (from a in order.Items where a.Name == x.MenuDetails.Menu.Name select x).FirstOrDefault();
                        double qtyDiff = item.Quantity - x.Quantity;
                        if (qtyDiff > 0)
                        {
                            x.Quantity += Math.Abs(qtyDiff);
                            return;
                        }
                        else if (qtyDiff < 0)
                        {
                            x.Quantity -= Math.Abs(qtyDiff);
                            return;
                        }

                        if (qtyDiff != 0)
                        {
                            dbMaster.SubTotal = order.SubTotal;
                            dbMaster.SaleDate = System.DateTime.Now;
                        }
                    }
                });

                dbDetail.ForEach(x =>
                {
                    if (!order.Items.Exists(a => a.Name == x.MenuDetails.Menu.Name))
                    {
                        x.Canceled = true;
                    }
                });
        }
        public static void UpdateOrder(Order order)
        {
            using POSContext context = new Database().Context;
            UpdateItems(context, order);
            UpdateDeals(context, order);
        }
        public static void DisableOrder(int orderNo)
        {
            using POSContext context = new Database().Context;
            var master = context.Find<DbSalesMaster>(orderNo);
            master.OrderStatus = OrderStatus.Deleted;
        }
        public static void OrderBillPrinted(int orderNo)
        {
            using var context = new Database().Context;
            var master = context.Find<DbSalesMaster>(orderNo);
            master.BillCreated = true;
            context.SaveChanges();
        }
        public static void OrderKOTPrinted(int orderNo, List<int> itemIds)
        {
            using var context = new Database().Context;
            var master = context.Find<DbSalesMaster>(orderNo);
            var detail = (from x in context.DbSalesDetails
                          where x.SalesMasterId == master.Id
                          select x).ToList();
            itemIds.ForEach(x => 
            {
                detail.ForEach(xx => 
                {
                    if (x == xx.MenuDetails.MenuId)
                        xx.Printed = true;
                });
            });
            context.SaveChanges();
        }
        
    }
}
