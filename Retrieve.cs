using Microsoft.EntityFrameworkCore;
using RMSEnumerations;
using System.Collections.Generic;
using System.Linq;
using Branch.Classes;
using Branch.Classes.Menu;
using POSDatabaseModel.Models;
using Branch.Classes.Discounts;

namespace Branch
{
    public static class Retrieve
    {
        public static IEnumerable<Category> GetCategories(Database database, bool includeDisabled = false, bool includeTruncated = false)
        {
            var context = database.Context;
            var list = (from DbCategories x
                    in context.DbCategories.Cast<DbCategories>()
                        select x).ToList();
            if (includeDisabled)
                list = (from x in list where x.Enabled == includeDisabled select x).ToList();
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x).ToList();
            return (from x in list
                    select new Category
                    {
                        Name = x.Name,
                    }).ToList() ?? new List<Category>();
        }
        public static IEnumerable<Item> GetItems(Database database, bool includeDisabled = false, bool includeTruncated = false)
        {
            var context = database.Context;
            var list = (from x in context.DbMenus
                        join a in context.DbMenuDetails on x.Id equals a.MenuId
                        join b in context.DbCategories on x.CategoryId equals b.Id
                        where x.ItemType == ItemType.Item
                        select x).ToList();
            if (includeDisabled)
                list = (from x in list where x.Enabled == includeDisabled select x).ToList();
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x).ToList();

            return (from x in list
                    select new Item
                    {
                        Category = new Category
                        {
                            Name = x.Category.Name
                        },
                        Name = x.Name,
                        Addons = (from xx in x.MenuAddons
                                  select new Addon
                                  {
                                      Name = xx.Name,
                                      Price = xx.Price,
                                      Selected = false,
                                  }).ToList(),
                        Quantity = 1,
                        Discount = Helpers.GetItemDiscount(x.DiscountAmount, x.DiscountPercentage),
                        Tax = new Tax { Percentage = x.TaxPercentage },
                        UnitPrice = (from xx in x.MenuDetails where xx.Enabled == true select xx.Price).FirstOrDefault(),
                    }).ToList() ?? new List<Item>();
        }
        public static IEnumerable<MenuItem> GetMenus(Database database, bool includeDisabled = false, bool includeTruncated = false)
        {
            var list = new List<MenuItem>();
            list.AddRange(GetItems(database, includeDisabled, includeTruncated));
            list.AddRange(GetDeals(database, includeDisabled, includeTruncated));
            return list;
        }
        public static IEnumerable<Addon> GetAddons(Database database, bool includeDisabled = false, bool includeTruncated = false)
        {
            var context = database.Context;
            var list = (from x in context.DbMenuAddons
                        select x).ToList();
            if (includeDisabled)
                list = (from x in list where x.Enabled == includeDisabled select x).ToList();
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x).ToList();

            return (from x in list
                    select new Addon
                    {
                        Name = x.Name,
                        Price = x.Price,
                        Selected = false,
                    }).ToList() ?? new List<Addon>();
        }
        public static IEnumerable<Waiter> GetWaiters(Database database, bool includeDisabled = false, bool includeTruncated = false)
        {
            var context = database.Context;
            var list = (from x in context.DbWaiters
                        select x);
            if (includeDisabled)
                list = (from x in list where x.Enabled == true select x);
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x);
            return (from x in list.ToArray()
                    select new Waiter
                    {
                        Name = x.Name,
                        Commission = Helpers.GetCommissionObj(x.CommissionAmount, x.CommissionPercentage),
                    }).ToList() ?? new List<Waiter>();
        }
        public static IEnumerable<Rider> GetRiders(Database database, bool includeDisabled = false, bool includeTruncated = false)
        {
            var context = database.Context;
            var list = (from x in context.DbRiders
                        select x);
            if (includeDisabled)
                list = (from x in list where x.Enabled == includeDisabled select x);

            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x);

            return (from x in list
                    select new Rider
                    {
                        Name = x.Name,
                        Status = x.RiderStatus,
                        Commission = Helpers.GetCommissionObj(x.CommissionAmount, x.CommissionPercentage),
                    }).ToList();
        }
        public static IEnumerable<User> GetUsers(Database database, bool includeDisabled = false, bool includeTruncated = false)
        {
            var context = database.Context;
            var list = (from x in context.DbUsers
                        select x);
            if (includeDisabled)
                list = (from x in list where x.Enabled == includeDisabled select x);
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x);
            return (from x in list
                    select new User
                    {
                        Name = x.Name,
                        Username = x.Username,
                        Password = x.Password,
                    }).ToList();
        }
        public static IEnumerable<Counter> GetCounters(Database database, bool includeDisabled = true, bool includeTruncated = false)
        {
            var context = database.Context;
            var list = (from x in context.DbCounters
                        select x);
            if (includeDisabled)
                list = (from x in list where x.Enabled == includeDisabled select x);
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x);
            return (from x in list
                    select Helpers.GetCounterObj(x)).ToList();
        }
        public static IEnumerable<Commission> GetRiderCommissions(Database database, bool includeDisabled = false, bool includeTruncated = false)
        {
            var context = database.Context;
            var list = (from x in context.DbRiders
                        select x)
                        ;
            if (includeDisabled)
                list = (from x in list where x.Enabled == includeDisabled select x);
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x);
            return (from x in list select Helpers.GetCommissionObj(x.CommissionAmount, x.CommissionPercentage)).ToList();
        }
        public static IEnumerable<Commission> GetWaiterCommissions(Database database, bool includeDisabled = false, bool includeTruncated = false)
        {
            var context = database.Context;
            var list = (from x in context.DbWaiters
                        select x);
            if (includeDisabled)
                list = (from x in list where x.Enabled == includeDisabled select x);
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x);
            return (from x in list select Helpers.GetCommissionObj(x.CommissionAmount, x.CommissionPercentage)).ToList();
        }
        public static IEnumerable<Tax> GetTaxes(Database database, bool includeDisabled = false, bool includeTruncated = false)
        {
            var context = database.Context;
            var list = (from x in context.DbTaxes
                        select x);
            if (!includeDisabled)
                list = (from x in list where x.Enabled == true select x);
            else if (!includeTruncated)
                list = (from x in list where x.Truncated == true select x);
            return (from x in list
                    select new Tax
                    {
                        Name = x.Name,
                        Percentage = x.Percentage,
                    }).ToList();
        }
        public static IEnumerable<Shift> GetShifts(Database database, bool includeTruncated = false)
        {
            var context = database.Context;
            var list = (from x in context.DbShiftLogs
                        select x);
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x);
            return (from x in list
                    select new Shift
                    {
                        ShiftNumber = x.ShiftNumber,
                    }).ToList() ?? new List<Shift>();
        }
        public static IEnumerable<Deal> GetDeals(Database database, bool includeDisabled = false, bool includeTruncated = false)
        {
            var context = database.Context;
            var list = (from x in context.DbMenus
                        join a in context.DbMenuDetails on x.Id equals a.MenuId into details
                        join b in context.DbMenuDealItems on x.Id equals b.MenuId into dealItems
                        where x.ItemType == ItemType.Deal
                        select x).ToList();
            if (includeDisabled)
                list = (from x in list where x.Enabled == true select x).ToList();
            if (includeTruncated)
                list = (from x in list where x.Truncated == true select x).ToList();
            return (from x in list
                    select new Deal
                    {
                        Name = x.Name,
                        Quantity = 1,
                        Tax = Helpers.GetTax(),
                        Discount = Helpers.GetItemDiscount(x.DiscountAmount, x.DiscountPercentage),
                    }).ToList();
        }
        public static IEnumerable<Table> GetTables(Database database, bool includeDisabled = false, bool includeTruncated = false)
        {
            List<Table> tables = new();
            var context = database.Context;
            var list = (from x in context.DbTables
                        select x);
            if (includeDisabled)
                list = (from x in list
                        where x.Enabled == includeDisabled
                        select x);
            if (includeTruncated)
                list = (from x in list
                        where x.Truncated == includeTruncated
                        select x);
            return (from x in list
                    select new Table
                    {
                        Name = x.Name,
                        Status = x.TableStatus
                    }).ToList();
        }
        public static IEnumerable<Customer> GetCustomers(Database database, bool includeDisabled = false, bool includeTruncated = false)
        {
            var context = database.Context;
            var list = (from DbCustomers x in context.DbCustomers select x);

            if (includeDisabled)
                list = (from x in list where x.Enabled == includeDisabled select x);
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x);
            return (from x in list
                    select new Customer
                    {
                        Name = x.Name,
                        Contact = x.Contact,
                        Address = x.Address,
                    }).ToList();
        }
        public static IEnumerable<WorkDay> GetWorkDays(Database database, bool includeTruncated = false)
        {
            var context = database.Context;
            var list = (from DbDayLogs x in context.DbDayLogs select x);
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x);
            return (from x in list
                    select new WorkDay
                    {
                        DayNumber = x.DayNumber
                    }).ToList();
        }
        public static IEnumerable<GeneralDiscount> GetDiscounts(Database database, bool includeDisabled = false, bool includeTruncated = false)
        {
            var context = database.Context;
            var list = context.DbDiscountsDetails
                        .Include(x => x.Discounts)
                        .ToList();
            if (includeDisabled)
                list = (from x in list where x.Enabled == includeDisabled select x).ToList();
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x).ToList();
            return (from x in list
                    select Helpers.GetGeneralDiscountObj(x)).ToList();
        }
        public static IEnumerable<ItemDiscount> GetItemDiscounts(Database database, string itemName, bool includeDisabled = false, bool includeTruncated = false)
        {
            var context = database.Context;
            var list = context.DbMenus
                        .Include(x => x.MenuDetails)
                        .Where(x => x.Name.ToLower().Equals(itemName.ToLower()))
                        .ToList();
            if (includeDisabled)
                list = (from x in list where x.Enabled == includeDisabled select x).ToList();
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x).ToList();
            return (from x in list
                    select Helpers.GetItemDiscount(x.DiscountAmount, x.DiscountPercentage)).ToList();
        }
        public static IEnumerable<Order> GetOrders(Database database, bool includeTruncated = false)
        {
            var context = database.Context;
            var list = context.DbSalesMaster
                        .Include(x => x.SalesDetails)
                            .ThenInclude(x => x.SalesAddonItems)
                        .Include(x => x.SalesDetails)
                            .ThenInclude(x => x.MenuDetails.MenuId)
                        .Include(x => x.SalesDetails)
                            .ThenInclude(x => x.MenuDetails)
                        .Include(x => x.DiscountDetail)
                            .ThenInclude(x => x.Discounts)
                        .Include(x => x.Waiter)
                        .Include(x => x.Customer)
                        .Include(x => x.Table)
                        .Include(x => x.Rider)
                        .Include(x => x.Tax)
                        .ToList();
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x).ToList();
            return (from x in list
                    select new Order
                    {
                        OrderNo = x.OrderNo,
                        Counter = Helpers.GetCounterObj(x.Counters),
                        Customer = Helpers.GetCustomerObj(x.Customer),
                        Discount = Helpers.GetGeneralDiscountObj(x.DiscountDetail),
                        
                    }).ToList();
        }
        internal static Dictionary<string, bool> GetSettings(Database database, bool includeDisabled = false)
        {
            var context = database.Context;
            var dictionary = new Dictionary<string, bool>();
            if (includeDisabled)
                context.DbSettings
                    .Include(x => x.Settings)
                    .Where(x => x.DataType == ValueDataType.Boolean)
                    .ToList()
                    .ForEach(x => dictionary.Add(x.Name, bool.Parse(x.Settings[0].Value)));
            else
                context.DbSettings
                .Include(x => x.Settings)
                .Where(x => x.DataType == ValueDataType.Boolean && x.Enabled)
                .ToList()
                .ForEach(x => dictionary.Add(x.Name, bool.Parse(x.Settings[0].Value)));
            return dictionary;
        }
    }
}
