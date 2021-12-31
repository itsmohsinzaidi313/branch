using POSDatabaseModel.Models;
using Branch.Classes;
using Branch.Classes.Discounts;
using Branch.Classes.Menu;
using RMSEnumerations;
using System.Collections.Generic;
using System;

namespace Branch.Database
{
    public static class Converters
    {
        public static Category GetCategory(DbCategories categories)
        {
            if (categories == null)
                throw new NullReferenceException("DbCategories cannot be null");
            if (string.IsNullOrEmpty(categories.Name))
                throw new ObjectPropertyNullOrEmpty("DbCategories", "Name");
            return new Category { Name = categories.Name };
        }
        public static Customer GetCustomer(DbCustomers customers)
        {
            if (customers == null)
                throw new NullReferenceException("DbCustomer cannot be null.");
            if (string.IsNullOrEmpty(customers.Name) || string.IsNullOrEmpty(customers.Contact))
                throw new ObjectPropertyNullOrEmpty("DbCustomer", "Name, Contact");
            return new Customer
            {
                Name = customers.Name,
                Address = customers.Address,
                Contact = customers.Contact,
            };
        }
        public static Counter GetCounter(DbCounters counters)
        {
            if (counters == null)
                throw new NullReferenceException("DbCounters cannot be null");
            if (string.IsNullOrEmpty(counters.Name) || string.IsNullOrEmpty(counters.UUID))
                throw new ObjectPropertyNullOrEmpty("DbCounters", "Name, UUID");
            return new Counter
            {
                Name = counters.Name,
                UUID = counters.UUID,
            };
        }
        public static Rider GetRider(DbRiders riders)
        {
            if (riders == null)
                throw new NullReferenceException("DbRiders cannot be null.");
            if (string.IsNullOrEmpty(riders.Name))
                throw new ObjectPropertyNullOrEmpty("DbRiders", "Name, CommissionAmount, CommissionPercentage");
            return new Rider
            {
                Name = riders.Name,
                Commission = Helpers.GetCommissionObj(riders.CommissionAmount, riders.CommissionPercentage),
                Status = riders.RiderStatus
            };
        }
        public static Table GetTable(DbTables tables)
        {
            if (tables == null)
                throw new NullReferenceException("DbTables cannot be null.");
            if (string.IsNullOrEmpty(tables.Name))
                throw new ObjectPropertyNullOrEmpty("DbTables", "Name");
            return new Table
            {
                Name = tables.Name,
                Status = tables.TableStatus,
            };
        }
        public static Tax GetTax(DbTaxes taxes)
        {
            if (taxes == null)
                throw new NullReferenceException("DbTaxes cannot be null.");
            if (string.IsNullOrEmpty(taxes.Name))
                throw new ObjectPropertyNullOrEmpty("DbTaxes", "Name");
            return new Tax
            {
                Name = taxes.Name,
                Percentage = taxes.Percentage,
            };
        }
        public static User GetUser(DbUsers users)
        {
            if (users == null)
                throw new NullReferenceException("DbUsers cannot be null.");
            if (string.IsNullOrEmpty(users.Name) || string.IsNullOrEmpty(users.Password) || string.IsNullOrEmpty(users.Username))
                throw new ObjectPropertyNullOrEmpty("DbUsers", "Name, Password, Username");
            return new User
            {
                Name = users.Name,
                Password = users.Password,
                Username = users.Username,
            };
        }
        public static Waiter GetWaiter(DbWaiters waiters)
        {
            if (waiters == null)
                throw new NullReferenceException("DbWaiters cannot be null.");
            if (string.IsNullOrEmpty(waiters.Name))
                throw new ObjectPropertyNullOrEmpty("DbWaiters", "Name");
            return new Waiter
            {
                Name = waiters.Name,
                Commission = Helpers.GetCommissionObj(waiters.CommissionAmount, waiters.CommissionPercentage),
            };
        }
        public static WorkDay GetWorkDay(DbDayLogs dbDayLogs)
        {
            if (dbDayLogs == null)
                throw new NullReferenceException("DbDayLogs");
            if (string.IsNullOrEmpty(dbDayLogs.DayNumber))
                throw new ObjectPropertyNullOrEmpty("DbDayLogs", "DayNumber");
            return new WorkDay
            {
                DayNumber = dbDayLogs.DayNumber
            };
        }
        public static Shift GetShift(DbShiftLogs shiftLogs)
        {
            if (shiftLogs == null)
                throw new NullReferenceException("DbShiftLogs cannot be null.");
            if (string.IsNullOrEmpty(shiftLogs.ShiftNumber))
                throw new ObjectPropertyNullOrEmpty("DbShiftLogs", "ShiftNumber");
            return new Shift
            {
                ShiftNumber = shiftLogs.ShiftNumber
            };
        }
        public static GeneralDiscount GetGeneralDiscount(DbDiscountsDetails discountsDetails)
        {
            if (discountsDetails == null)
                throw new NullReferenceException("DbDiscountsDetails cannot be null.");
            if (discountsDetails.Start == null || discountsDetails.End == null || discountsDetails.Discounts == null || string.IsNullOrEmpty(discountsDetails.Discounts.Name))
                throw new ObjectPropertyNullOrEmpty("DbDiscountDetails", "Start, End, Discounts, Discounts.Name");

            GeneralDiscount discount = new GeneralDiscount
            {
                Name = discountsDetails.Discounts.Name,
                DiscountType = discountsDetails.DiscountType,
                From = discountsDetails.Start,
                To = discountsDetails.End,
            };
            if (discountsDetails.Amount > 0)
            {
                discount.Amount = discountsDetails.Amount;
                discount.AmountUnit = Units.Amount;
            }
            else if (discountsDetails.Percentage > 0)
            {
                discount.Amount = discountsDetails.Percentage;
                discount.AmountUnit = Units.Percentage;
            }
            else
            {
                discount.Amount = 0;
                discount.AmountUnit = Units.Undefined;
            }
            return discount;
        }
        public static Order GetOrder(DbSalesMaster salesMaster)
        {
            if (salesMaster == null)
                throw new NullReferenceException("DbSalesMaster cannot be null.");
            if (salesMaster.Counters == null || salesMaster.DiscountDetail == null || salesMaster.ShiftLog == null || salesMaster.Users == null || salesMaster.SalesDetails == null)
                throw new ObjectPropertyNullOrEmpty("DbSalesMaster", "DbCounters, DbWaiters, DbRiders, DbTables, DbCustomers, DbDiscountsDetails, DbShiftLogs, DbUsers, List<DbSalesDetails>");

            if (salesMaster.OrderType == OrderType.DineInIndoor || salesMaster.OrderType == OrderType.DineInOutdoor)
            {
                if (salesMaster.Waiter == null || salesMaster.Table == null)
                    throw new ObjectPropertyNullOrEmpty("DbSalesMaster", "DbWaiter, DbTables");
            }
            if (salesMaster.OrderType == OrderType.TakeAway)
            {
                if (salesMaster.Customer == null || string.IsNullOrEmpty(salesMaster.Customer.Name))
                    throw new ObjectPropertyNullOrEmpty("DbSalesMaster", "DbRiders, DbCustomers, DbCustomers.Name");
            }
            if (salesMaster.OrderType == OrderType.Delivery)
            {
                if (salesMaster.Rider == null || salesMaster.Customer == null || string.IsNullOrEmpty(salesMaster.Customer.Name) || string.IsNullOrEmpty(salesMaster.Customer.Contact) || string.IsNullOrEmpty(salesMaster.Customer.Address))
                    throw new ObjectPropertyNullOrEmpty("DbSalesMaster", "DbCustomers, DbCustomers.Contact, DbCustomers.Name, DbCustomers.Address");
            }

            Order order = new Order
            {
                Counter = GetCounter(salesMaster.Counters),
                Waiter = GetWaiter(salesMaster.Waiter),
                Rider = GetRider(salesMaster.Rider),
                Table = GetTable(salesMaster.Table),
                Customer = GetCustomer(salesMaster.Customer),
                Discount = GetGeneralDiscount(salesMaster.DiscountDetail),
                Shift = GetShift(salesMaster.ShiftLog),
                User = GetUser(salesMaster.Users),
                Items = GetItems(salesMaster.SalesDetails),
                OrderNo = salesMaster.OrderNo,
                TokenNo = salesMaster.TokenNo,
                OrderDate = salesMaster.SaleDate,
                OrderType = salesMaster.OrderType,
                PaymentMode = salesMaster.PaymentMode,
                Persons = salesMaster.Persons,
                DeliveryCharges = salesMaster.DeliveryCharges,
                ExtraCharges = salesMaster.ExtraCharges,
            };
            return order;
        }
        public static List<DealItem> GetDealItems(List<DbSalesDealItems> salesDealItems)
        {
            List<DealItem> dealItems = new();
            salesDealItems.ForEach(x =>
            {
                DealItem dealItem = new DealItem
                {
                    Name = x.Menu.Name,
                    Category = GetCategory(x.Menu.Category),
                    Choice = x.Choice,
                    Discount = Helpers.GetItemDiscount(x.Menu.DiscountAmount, x.Menu.DiscountPercentage),
                    Quantity = x.Quantity,
                    Tax = new Tax { Name = x.Menu.Name, Percentage = x.Menu.TaxPercentage },
                    UnitPrice = x.MenuDetail.Price,
                };

            });
            return dealItems;
        }
        public static List<DealItem> GetDealItems(List<DbMenuDealItems> menuDealItems)
        {
            List<DealItem> dealItems = new();
            menuDealItems.ForEach(x => dealItems.Add(GetDealItem(x)));
            return dealItems;
        }
        public static DealItem GetDealItem(DbMenuDealItems menuDealItem)
        {
            return new DealItem
            {
                Name = menuDealItem.Menu.Name,
                Category = GetCategory(menuDealItem.Menu.Category),
                Quantity = 1,
                Discount = Helpers.GetItemDiscount(menuDealItem.MenuDetail.DiscountAmount, menuDealItem.MenuDetail.DiscountPercentage),
                Tax = new Tax { Name = menuDealItem.Menu.Name, Percentage = menuDealItem.MenuDetail.TaxPercentage },
                Choice = menuDealItem.Choice,
                UnitPrice = menuDealItem.MenuDetail.Price,
            };
        }
        public static List<Addon> GetAddons(List<DbSalesAddons> dbAddons)
        {
            List<Addon> addons = new();
            dbAddons.ForEach(x =>
            {
                Addon addon = new Addon
                {
                    Name = x.MenuAddon.Name,
                    Price = x.MenuAddon.Price,
                    Selected = true,
                };
                addons.Add(addon);
            });
            return addons;
        }
        public static List<Addon> GetAddons(List<DbMenuAddons> dbAddons)
        {
            List<Addon> addons = new();
            dbAddons.ForEach(x =>
            {
                Addon addon = new Addon
                {
                    Name = x.Name,
                    Price = x.Price,
                    Selected = false,
                };
            });
            return addons;
        }
        public static List<MenuItem> GetItems(List<DbMenuDetails> details)
        {
            List<MenuItem> items = new();
            details.ForEach(x =>
            {
                items.Add(GetItem(x));
            });
            return items;
        }
        public static List<MenuItem> GetItems(List<DbSalesDetails> details)
        {
            List<MenuItem> items = new();
            details.ForEach(x =>
                {
                    if (x.MenuDetails.Menu.ItemType == ItemType.Item)
                    {
                        Item item = new Item
                        {
                            Name = x.MenuDetails.Menu.Name,
                            Category = GetCategory(x.MenuDetails.Menu.Category),
                            Discount = Helpers.GetItemDiscount(x.MenuDetails.DiscountAmount, x.MenuDetails.DiscountPercentage),
                            Tax = new Tax { Name = x.MenuDetails.Menu.Name, Percentage = x.MenuDetails.TaxPercentage },
                            Quantity = x.Quantity,
                            UnitPrice = x.MenuDetails.Price,
                            Addons = GetAddons(x.SalesAddonItems),
                        };
                        items.Add(item);
                    }
                    else if (x.MenuDetails.Menu.ItemType == ItemType.Deal)
                    {
                        Deal deal = new Deal
                        {
                            Name = x.MenuDetails.Menu.Name,
                            Category = GetCategory(x.MenuDetails.Menu.Category),
                            Discount = Helpers.GetItemDiscount(x.MenuDetails.DiscountAmount, x.MenuDetails.DiscountPercentage),
                            Tax = new Tax { Name = x.MenuDetails.Menu.Name, Percentage = x.MenuDetails.TaxPercentage },
                            Quantity = x.Quantity,
                            UnitPrice = x.MenuDetails.Price,
                            Addons = GetAddons(x.SalesAddonItems),
                            DealItems = GetDealItems(x.SalesDealItems),
                        };
                        items.Add(deal);
                    }
                });
            return items;
        }
        public static MenuItem GetItem(DbMenuDetails detail)
        {
            if (detail.Menu.ItemType == ItemType.Item)
            {
                return new Item
                {
                    Name = detail.Menu.Name,
                    Category = GetCategory(detail.Menu.Category),
                    Quantity = 1,
                    Discount = Helpers.GetItemDiscount(detail.DiscountAmount, detail.DiscountPercentage),
                    UnitPrice = detail.Price,
                    Tax = new Tax { Name = detail.Menu.Name, Percentage = detail.TaxPercentage },
                    Addons = GetAddons(detail.Menu.MenuAddons),
                };
            }
            else if (detail.Menu.ItemType == ItemType.Deal)
            {
                return new Deal
                {
                    Name = detail.Menu.Name,
                    Category = GetCategory(detail.Menu.Category),
                    Quantity = 1,
                    Discount = Helpers.GetItemDiscount(detail.DiscountAmount, detail.DiscountPercentage),
                    UnitPrice = detail.Price,
                    Tax = new Tax { Name = detail.Menu.Name, Percentage = detail.TaxPercentage },
                    DealItems = GetDealItems(detail.Menu.MenuDeals),
                    Addons = GetAddons(detail.Menu.MenuAddons),
                };
            }
            else
            {
                throw new Exception("ItemType not supported.");
            }
        }
        public static Menu GetMenu(List<DbMenuDetails> details)
        {
            Menu menu = new();
            details.ForEach(x =>
            {
                menu.Categories.Add(GetCategory(x.Menu.Category));
                menu.Items.Add(GetItem(x));
            });
            return menu;
        }
    }
}