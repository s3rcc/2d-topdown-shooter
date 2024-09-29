using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Scripts
{
    public enum ItemType
    {
        Exp
    }

    public class Item
    {
        public ItemType itemType;
        public int amount;

        public Item(ItemType itemType, int amount)
        {
            this.itemType = itemType;
            this.amount = amount;
        }
    }

}
