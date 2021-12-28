using System;
using System.Threading.Tasks;
using POSDatabaseModel;

namespace Branch
{
    internal class Database : IAsyncDisposable, IDisposable
    {
        internal POSContext Context => _context;
        private POSContext _context { get; set; }
        internal Database()
        {
            _context = new POSContext();
            _context.SavingChanges += _context_SavingChanges;
            _context.SavedChanges += _context_SavedChanges;
            _context.SaveChangesFailed += _context_SaveChangesFailed;
        }

        private void _context_SaveChangesFailed(object sender, Microsoft.EntityFrameworkCore.SaveChangesFailedEventArgs e)
        {

        }


        private void _context_SavedChanges(object sender, Microsoft.EntityFrameworkCore.SavedChangesEventArgs e)
        {

        }

        private void _context_SavingChanges(object sender, Microsoft.EntityFrameworkCore.SavingChangesEventArgs e)
        {

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
