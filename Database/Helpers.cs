using Branch.Classes;
using Branch.Classes.Discounts;
using Branch.Classes.Menu;
using POSDatabaseModel;
using POSDatabaseModel.Models;
using RMSEnumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Branch.Database
{
    internal static class Helpers
    {
        internal static int GetNewOrderNumber(bool uniquePerDay = false)
        {
            using POSContext context = new Database().Context;
            if (uniquePerDay)
            {
                return context.DbSalesMaster
                    .Count(x => x.SaleDate == DateTime.Now) + 1;
            }
            return context.DbSalesMaster.Max(x => x.OrderNo) + 1;
        }

        internal static DbDepartments GetDbDepartment(POSContext context, string name)
        {
            return (from x in context.DbCategories
                    join a in context.DbDepartments on x.DepartmentId equals a.Id
                    where x.Name.ToLower().Equals(name.ToLower()) && 
                        x.Enabled == true && a.Enabled == true
                    select a).FirstOrDefault();
        }

        internal static int GetNewRandomOrderNumber()
        {
            using (POSContext context = new Database().Context)
            {
                var random = new Random();
                int orderNo = 0;
                while (context.DbSalesMaster.Where(x => x.OrderNo == orderNo).Count() >= 1)
                {
                    orderNo = random.Next(100000, 999999);
                }
                return orderNo;
            }
        }
        internal static DbDayLogs GetDay()
        {
            using var context = new Database().Context;
            return (from x in context.DbDayLogs where x.Closed == false select x).FirstOrDefault();
        }
        internal static DbShiftLogs GetShift()
        {
            using var context = new Database().Context;
            return (from x in context.DbShiftLogs where x.Closed == false select x).FirstOrDefault();
        }
        internal static int GetNewTokenNumber()
        {
            using (POSContext context = new Database().Context)
            {
                return context.DbSalesMaster.Where(x => x.SaleDate == DateTime.Now).Max(x => x.TokenNo) + 1;
            }
        }
        internal static int GetNewRandomTokenNumber()
        {
            using (POSContext context = new Database().Context)
            {
                var random = new Random();
                int tokenNo = 0;
                while (context.DbSalesMaster.Where(x => x.SaleDate == DateTime.Now && x.TokenNo == tokenNo).Count() >= 1)
                {
                    tokenNo = random.Next(100000, 999999);
                }
                return tokenNo;
            }
        }
        internal static DbBranch GetBranch()
        {
            using POSContext context = new Database().Context;
            return (from DbBranch x in context.DbBranch select x).FirstOrDefault();
        }
        internal static DbTables GetTable(int tableId)
        {
            using POSContext context = new Database().Context;
            return (from DbTables x in context.DbTables where x.Id == tableId select x).FirstOrDefault();
        }
        internal static DbRiders GetRider(int riderId)
        {
            using POSContext context = new Database().Context;
            return (from DbRiders x in context.DbRiders where x.Id == riderId select x).FirstOrDefault();
        }
        internal static DbUsers GetUser(int userId)
        {
            using POSContext context = new Database().Context;
            return (from DbUsers x in context.DbUsers where x.Id == userId select x).FirstOrDefault();
        }
        internal static DbWaiters GetWaiter(int waiterId)
        {
            using POSContext context = new Database().Context;
            return (from DbWaiters x in context.DbWaiters where x.Id == waiterId select x).FirstOrDefault();
        }
        internal static Commission GetCommissionObj(double commissionAmount, double commissionPercentage)
        {
            var commission = new Commission();
            if (commissionAmount > 0)
            {
                commission.Amount = commissionAmount;
                commission.AmountUnit = Units.Amount;
            }
            else if (commissionPercentage > 0)
            {
                commission.Amount = commissionPercentage;
                commission.AmountUnit = Units.Percentage;
            }
            else
            {
                commission.Amount = 0;
                commission.AmountUnit = Units.Amount;
            }
            return commission;
        }
        internal static DbTaxes GetDbTax()
        {
            using POSContext context = new Database().Context;
            return (from DbTaxes x in context.DbTaxes
                    where x.Enabled == true
                    select x).FirstOrDefault();
        }
        internal static Tax GetTax()
        {
            using POSContext context = new Database().Context;
            return (from DbTaxes x in context.DbTaxes
                    where x.Enabled == true
                    select new Tax
                    {
                        Name = x.Name,

                    }).FirstOrDefault();
        }
        internal static DbCustomers GetCustomer(int customerId)
        {
            using POSContext context = new Database().Context;
            return (from DbCustomers x in context.DbCustomers where x.Id == customerId select x).FirstOrDefault();
        }
        internal static DbDiscounts GetDiscount(int discountId)
        {
            using POSContext context = new Database().Context;
            return (from DbDiscounts x in context.DbDiscounts where x.Id == discountId select x).FirstOrDefault();
        }
        internal static int GetMenuDetailsId(int itemId, POSContext context)
        {
            return (from DbMenu x in context.DbMenus
                    join a in context.DbMenuDetails on x.Id equals a.MenuId
                    where x.Enabled == true
                    select a.Id).FirstOrDefault();
        }
        internal static List<DbMenuAddons> GetMenuAddons(int itemId, POSContext context, bool includeDisabled = true)
        {
            var list = (from x in context.DbMenuAddons
                        where x.MenuId == itemId
                        select x).ToList();
            if (!includeDisabled)
                list = (from x in list
                        where x.Enabled == true
                        select x).ToList();
            return list;


        }
        internal static List<DealItem> GetOrderDealItems(int detailId, POSContext context)
        {
            var tax = GetDbTax();
            return (from x in context.DbSalesDetails
                    join b in context.DbMenuDetails on x.MenuDetailsId equals b.Id
                    join c in context.DbSalesDealItems on x.Id equals c.SalesDetailId
                    join d in context.DbMenus on c.MenuId equals d.Id
                    where x.Id == detailId && d.ItemType == ItemType.Deal
                    select new DealItem
                    {
                        Name = d.Name,
                        Discount = GetItemDiscount(b.DiscountAmount, b.DiscountPercentage),
                        Quantity = x.Quantity,
                        Choice = c.Choice,
                        UnitPrice = b.Price,
                        Tax = new Classes.Tax
                        {
                            Percentage = tax.Percentage,
                        }
                    }).ToList();
        }
        internal static List<Addon> GetOrderAddons(int detailId, POSContext context)
        {
            return (from x in context.DbSalesDetails
                    join b in context.DbSalesAddons on x.Id equals b.SalesDetailId
                    join c in context.DbMenuAddons on b.MenuAddonId equals c.Id
                    where x.Id == detailId
                    select new Addon
                    {
                        Name = c.Name,
                        Price = c.Price,
                        Selected = true,

                    }).ToList();
        }
        internal static ItemDiscount GetItemDiscount(double amount, double percentage)
        {
            var discount = new ItemDiscount();
            if (percentage > 0)
            {
                discount.Amount = percentage;
                discount.AmountUnit = Units.Percentage;
            }
            else if (amount > 0)
            {
                discount.Amount = amount;
                discount.AmountUnit = Units.Amount;
            }
            else
            {
                discount.Amount = 0;
                discount.AmountUnit = Units.Undefined;
            }
            return discount;
        }
        internal static List<DealItem> GetDealItems(int menuId, bool includeDisabled = true)
        {
            var tax = GetDbTax();
            using var context = new Database().Context;
            var list = (from x in context.DbMenus
                        join a in context.DbMenuDealItems on x.Id equals a.MenuId
                        join b in context.DbMenuDetails on a.MenuId equals b.MenuId
                        join c in context.DbMenuAddons on x.Id equals c.MenuId
                        where x.Id == menuId
                        select new DealItem
                        {
                            Name = x.Name,
                            UnitPrice = b.Price,
                            Quantity = 1,
                            Category = (from xx in context.DbCategories where xx.Id == x.CategoryId select new Category { Name = xx.Name, }).FirstOrDefault(),
                            Choice = a.Choice,
                            Discount = GetItemDiscount(x.DiscountAmount, x.DiscountPercentage),
                            Tax = new Tax { Percentage = tax.Percentage },
                        }).ToList();
            return list;
        }
        internal static Counter GetCounter(POSContext context)
        {
            return (from x in context.DbCounters
                    where x.Name == Environment.MachineName
                    select new Counter
                    {
                        Name = x.Name,
                        UUID = x.UUID,
                    }).FirstOrDefault();
        }
        internal static Customer GetCustomerObj(DbCustomers customers)
        {
            return new Customer
            {
                Name = customers.Name,
                Address = customers.Address,
                Contact = customers.Contact,
            };
        }
        internal static GeneralDiscount GetGeneralDiscountObj(DbDiscountsDetails discounts)
        {
            var discount = new GeneralDiscount
            {
                Name = discounts.Discounts.Name,
                From = discounts.Start,
                To = discounts.End,
                DiscountType = discounts.DiscountType,
            };
            if (discounts.Amount > 0)
            {
                discount.Amount = discounts.Amount;
                discount.AmountUnit = Units.Amount;
            }
            else if (discounts.Percentage > 0)
            {
                discount.Amount = discounts.Percentage;
                discount.AmountUnit = Units.Percentage;
            }
            else
            {
                discount.Amount = 0;
                discount.AmountUnit = Units.Undefined;
            }
            return discount;
        }
        internal static Counter GetCounterObj(DbCounters counters)
        {
            return new Counter
            {
                Name = counters.Name,
                UUID = counters.UUID,
            };
        }
        internal static Category GetCategoryObj(DbCategories categories)
        {
            return new Category{};
        }
    }
}
