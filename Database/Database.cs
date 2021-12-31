using System;
using System.Threading.Tasks;
using POSDatabaseModel;
using Branch.Classes;
using Branch.Classes.Discounts;
using Branch.Classes.Menu;
using System.Collections.Generic;

namespace Branch.Database
{
    public class Database : IAsyncDisposable, IDisposable
    {
        internal POSContext Context => _context;
        private POSContext _context { get; set; }
        public Database(string server = "", string database = "", string username = "", string password = "")
        {
            if (string.IsNullOrEmpty(string.Empty) && string.IsNullOrEmpty(database) && string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
                throw new Exception("Invalid connection string.");
            else if (!string.IsNullOrEmpty(string.Empty) && !string.IsNullOrEmpty(database) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                _context = new POSContext(server, database, username, password);
            else
                _context = new POSContext();
        }
        public void SetEvents(SavingChanges savingChanges, SavedChanges savedChanges, SaveChangesFailed saveChangesFailed)
        {
            _context.SavedChanges += (sender, e) => _context_SavedChanges(sender, e, savedChanges);
            _context.SavingChanges += (sender, e) => _context_SavingChanges(sender, e, savingChanges);
            _context.SaveChangesFailed += (sender, e) => _context_SaveChangesFailed(sender, e, saveChangesFailed);
        }
        public void Create(object value)
        {

        }
        public IEnumerable<object> Retrieve(Type type, string itemName = "", bool includeDisabled = false, bool includeTruncated = false)
        {
            if (type == typeof(Category))
                return Branch.Database.Retrieve.GetCategories(this, includeDisabled, includeTruncated);
            else if (type == typeof(Counter))
                return Branch.Database.Retrieve.GetCounters(this, includeDisabled, includeTruncated);
            else if (type == typeof(Customer))
                return Branch.Database.Retrieve.GetCustomers(this, includeDisabled, includeTruncated);
            else if (type == typeof(Order))
                return Branch.Database.Retrieve.GetOrders(this, includeTruncated);
            else if (type == typeof(Rider))
                return Branch.Database.Retrieve.GetRiders(this, includeDisabled, includeTruncated);
            else if (type == typeof(Shift))
                return Branch.Database.Retrieve.GetShifts(this, includeTruncated);
            else if (type == typeof(Table))
                return Branch.Database.Retrieve.GetTables(this, includeDisabled, includeTruncated);
            else if (type == typeof(Tax))
                return Branch.Database.Retrieve.GetTaxes(this, includeDisabled, includeTruncated);
            else if (type == typeof(User))
                return Branch.Database.Retrieve.GetUsers(this, includeDisabled, includeTruncated);
            else if (type == typeof(Waiter))
                return Branch.Database.Retrieve.GetWaiters(this, includeDisabled, includeTruncated);
            else if (type == typeof(WorkDay))
                return Branch.Database.Retrieve.GetWorkDays(this, includeTruncated);
            else if (type == typeof(GeneralDiscount))
                return Branch.Database.Retrieve.GetDiscounts(this, includeDisabled, includeTruncated);
            else if (type == typeof(ItemDiscount))
            {
                if (string.IsNullOrEmpty(itemName))
                    throw new Exception("Parameter itemName is either null or empty.");
                return Branch.Database.Retrieve.GetItemDiscounts(this, itemName, includeDisabled, includeTruncated);
            }
            else if (type == typeof(Addon))
                return Branch.Database.Retrieve.GetAddons(this, includeDisabled, includeTruncated);
            else if (type == typeof(Deal))
                return Branch.Database.Retrieve.GetDeals(this, includeDisabled, includeTruncated);
            else if (type == typeof(Item))
                return Branch.Database.Retrieve.GetItems(this, includeDisabled, includeTruncated);
            else
                throw new Exception("Type not supported.");
        }
        public delegate void SavingChanges();
        public delegate void SavedChanges();
        public delegate void SaveChangesFailed();

        private void _context_SaveChangesFailed(object sender, Microsoft.EntityFrameworkCore.SaveChangesFailedEventArgs e, SaveChangesFailed f)
        {
            f();
        }
        private void _context_SavedChanges(object sender, Microsoft.EntityFrameworkCore.SavedChangesEventArgs e, SavedChanges f)
        {
            f();
        }
        private void _context_SavingChanges(object sender, Microsoft.EntityFrameworkCore.SavingChangesEventArgs e, SavingChanges f)
        {
            f();
        }
        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
