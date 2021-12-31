using System;
using System.Threading.Tasks;
using POSDatabaseModel;
using Branch.Classes;
using Branch.Classes.Discounts;
using Branch.Classes.Menu;
using System.Collections.Generic;

namespace Branch
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
                return Branch.Retrieve.GetCategories(this, includeDisabled, includeTruncated);
            else if (type == typeof(Counter))
                return Branch.Retrieve.GetCounters(this, includeDisabled, includeTruncated);
            else if (type == typeof(Customer))
                return Branch.Retrieve.GetCustomers(this, includeDisabled, includeTruncated);
            else if (type == typeof(Order))
                return Branch.Retrieve.GetOrders(this, includeTruncated);
            else if (type == typeof(Rider))
                return Branch.Retrieve.GetRiders(this, includeDisabled, includeTruncated);
            else if (type == typeof(Shift))
                return Branch.Retrieve.GetShifts(this, includeTruncated);
            else if (type == typeof(Table))
                return Branch.Retrieve.GetTables(this, includeDisabled, includeTruncated);
            else if (type == typeof(Tax))
                return Branch.Retrieve.GetTaxes(this, includeDisabled, includeTruncated);
            else if (type == typeof(User))
                return Branch.Retrieve.GetUsers(this, includeDisabled, includeTruncated);
            else if (type == typeof(Waiter))
                return Branch.Retrieve.GetWaiters(this, includeDisabled, includeTruncated);
            else if (type == typeof(WorkDay))
                return Branch.Retrieve.GetWorkDays(this, includeTruncated);
            else if (type == typeof(GeneralDiscount))
                return Branch.Retrieve.GetDiscounts(this, includeDisabled, includeTruncated);
            else if (type == typeof(ItemDiscount))
            {
                if (string.IsNullOrEmpty(itemName))
                    throw new Exception("Parameter itemName is either null or empty.");
                return Branch.Retrieve.GetItemDiscounts(this, itemName, includeDisabled, includeTruncated);
            }
            else if (type == typeof(Addon))
                return Branch.Retrieve.GetAddons(this, includeDisabled, includeTruncated);
            else if (type == typeof(Deal))
                return Branch.Retrieve.GetDeals(this, includeDisabled, includeTruncated);
            else if (type == typeof(Item))
                return Branch.Retrieve.GetItems(this, includeDisabled, includeTruncated);
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
