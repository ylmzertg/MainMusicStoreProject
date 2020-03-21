using MainMusicStore.Models.DbModels;
using System.Collections.Generic;

namespace MainMusicStore.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ListCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
