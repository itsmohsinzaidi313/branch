using System.Collections.Generic;
using System.Linq;

namespace Branch.Classes.Menu
{
    public class Deal : Item
    {
        public IList<DealItem> DealItems { get; set; } = new List<DealItem>();

        public override bool Equals(object obj)
        {
            if (obj is not Deal)
            {
                return false;
            }

            if (this == null || obj == null)
            {
                return false;
            }

            if ((obj as Deal).DealItems.Count != DealItems.Count)
            {
                return false;
            }

            string thisItemNames = string.Empty;
            string objItemNames = string.Empty;
            DealItems.ToList().ForEach(x => thisItemNames += x.Name);
            (obj as Deal).DealItems.ToList().ForEach(x => objItemNames += x.Name);

            return base.Equals(thisItemNames.ToUpper().Equals(objItemNames.ToUpper()));
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            DealItems.ToList().ForEach(x => hashCode += x.GetHashCode());
            return hashCode;
        }
    }

    public class DealItem : MenuItem
    {
        public int Choice { get; set; } = 0;
    }
}
