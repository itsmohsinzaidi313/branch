using Microsoft.EntityFrameworkCore;
using RMSEnumerations;
using System.Collections.Generic;
using System.Linq;
using Branch.Classes;
using Branch.Classes.Menu;
using POSDatabaseModel.Models;

namespace Branch
{
    public static class Data
    {
        public static IEnumerable<Category> GetCategories(bool includeDisabled = false, bool includeTruncated = false)
        {
            var context = new Database().Context;
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
                        Id = x.Id,
                        Name = x.Name,
                    }).ToList() ?? new List<Category>();
        }
        public static IEnumerable<Item> GetItems(bool includeDisabled = false, bool includeTruncated = false)
        {
            using var context = new Database().Context;
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
                            Id = x.Category.Id,
                            Name = x.Category.Name
                        },
                        ItemId = x.Id,
                        Name = x.Name,
                        Addons = (from xx in x.MenuAddons
                                  select new Addon
                                  {
                                      AddonId = xx.Id,
                                      Name = xx.Name,
                                      Price = xx.Price,
                                      Selected = false,
                                  }).ToList(),
                        Quantity = 1,
                        Discount = Helpers.GetItemDiscount(x.DiscountAmount, x.DiscountPercentage),
                        Tax = new Tax { Percentage = x.TaxPercentage },
                        Canceled = false,
                        UnitPrice = (from xx in x.MenuDetails where xx.Enabled == true select xx.Price).FirstOrDefault(),
                    }).ToList() ?? new List<Item>();
        }
        public static IEnumerable<MenuItem> GetMenus(bool includeDisabled = false, bool includeTruncated = false)
        {
            var list = new List<MenuItem>();
            list.AddRange(GetItems(includeDisabled, includeTruncated));
            list.AddRange(GetDeals(includeDisabled, includeTruncated));
            return list;
        }
        public static IEnumerable<Addon> GetAddons(bool includeDisabled = false, bool includeTruncated = false)
        {
            using var context = new Database().Context;
            var list = (from x in context.DbMenuAddons
                        select x).ToList();
            if (includeDisabled)
                list = (from x in list where x.Enabled == includeDisabled select x).ToList();
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x).ToList();

            return (from x in list
                    select new Addon
                    {
                        AddonId = x.Id,
                        ItemId = x.MenuId,
                        Name = x.Name,
                        Price = x.Price,
                        Selected = false,
                        Enabled = x.Enabled,
                    }).ToList() ?? new List<Addon>();
        }
        public static IEnumerable<Waiter> GetWaiters(bool includeDisabled = false, bool includeTruncated = false)
        {
            using var context = new Database().Context;
            var list = (from x in context.DbWaiters
                        select x);
            if (includeDisabled)
                list = (from x in list where x.Enabled == true select x);
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x);
            return (from x in list.ToArray()
                    select new Waiter
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Commission = Helpers.GetCommissionObj(x.Id, x.CommissionAmount, x.CommissionPercentage),
                        Enabled = x.Enabled,
                    }).ToList() ?? new List<Waiter>();
        }
        public static IEnumerable<Rider> GetRiders(bool includeDisabled = false, bool includeTruncated = false)
        {
            using var context = new Database().Context;
            var list = (from x in context.DbRiders
                        select x);
            if (includeDisabled)
                list = (from x in list where x.Enabled == includeDisabled select x);

            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x);

            return (from x in list
                    select new Rider
                    {
                        RiderId = x.Id,
                        Name = x.Name,
                        Status = x.RiderStatus,
                        Commission = Helpers.GetCommissionObj(x.Id, x.CommissionAmount, x.CommissionPercentage),
                        Enabled = x.Enabled,
                    }).ToList();
        }
        public static IEnumerable<User> GetUsers(bool includeDisabled = false, bool includeTruncated = false)
        {
            using var context = new Database().Context;
            var list = (from x in context.DbUsers
                        select x);
            if (includeDisabled)
                list = (from x in list where x.Enabled == includeDisabled select x);
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x);
            return (from x in list
                    select new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Username = x.Username,
                        Password = x.Password,
                        Enabled = x.Enabled,
                    }).ToList();
        }
        public static IEnumerable<Counter> GetCounters(bool includeDisabled = true, bool includeTruncated = false)
        {
            using var context = new Database().Context;
            var list = (from x in context.DbCounters
                        select x);
            if (includeDisabled)
                list = (from x in list where x.Enabled == includeDisabled select x);
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x);
            return (from x in list
                    select new Counter
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UUID = x.UUID,
                        Enabled = x.Enabled,
                    }).ToList();
        }
        public static IEnumerable<Commission> GetRiderCommissions(bool includeDisabled = false, bool includeTruncated = false)
        {
            using var context = new Database().Context;
            var list = (from x in context.DbRiders
                        select x)
                        ;
            if (includeDisabled)
                list = (from x in list where x.Enabled == includeDisabled select x);
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x);
            return (from x in list select Helpers.GetCommissionObj(x.Id, x.CommissionAmount, x.CommissionPercentage)).ToList();
        }
        public static IEnumerable<Commission> GetWaiterCommissions(bool includeDisabled = false, bool includeTruncated = false)
        {
            using var context = new Database().Context;
            var list = (from x in context.DbWaiters
                        select x);
            if (includeDisabled)
                list = (from x in list where x.Enabled == includeDisabled select x);
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x);
            return (from x in list select Helpers.GetCommissionObj(x.Id, x.CommissionAmount, x.CommissionPercentage)).ToList();
        }
        public static IEnumerable<Tax> GetTaxes(bool includeDisabled = false, bool includeTruncated = false)
        {
            using var context = new Database().Context;
            var list = (from x in context.DbTaxes
                        select x);
            if (!includeDisabled)
                list = (from x in list where x.Enabled == true select x);
            else if (!includeTruncated)
                list = (from x in list where x.Truncated == true select x);
            return (from x in list
                    select new Tax
                    {
                        TaxId = x.Id,
                        Name = x.Name,
                        Percentage = x.Percentage,
                    }).ToList();
        }
        public static IEnumerable<Shift> GetShifts(bool includeTruncated = false)
        {
            using var context = new Database().Context;
            var list = (from x in context.DbShiftLogs
                        select x);
            if (includeTruncated)
                list = (from x in list where x.Truncated == includeTruncated select x);
            return (from x in list select new Shift
            {
                ShiftId = x.Id,
                ShiftNumber = x.ShiftNumber,
            }).ToList() ?? new List<Shift>();
        }
        public static IEnumerable<Deal> GetDeals(bool includeDisabled = false, bool includeTruncated = false)
        {
            using var context = new Database().Context;
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
                        ItemId = x.Id,
                        Name = x.Name,
                        Quantity = 1,
                        Canceled = false,
                        Tax = Helpers.GetTax(),
                        Discount = Helpers.GetItemDiscount(x.DiscountAmount, x.DiscountPercentage),
                    }).ToList();
        }
        public static IEnumerable<Table> GetTables(bool includeDisabled = false, bool includeTruncated = false)
        {
            List<Table> tables = new();
            using var context = new Database().Context;
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
                        TableId = x.Id,
                        Name = x.Name,
                        Status = x.TableStatus
                    }).ToList();
        }
        internal static Dictionary<string, bool> GetSettings(bool includeDisabled = false)
        {
            using var context = new Database().Context;
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
