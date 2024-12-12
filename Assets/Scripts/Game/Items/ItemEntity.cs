using Infrastructure.Data.Game.Shop;

namespace Game.Items
{
    public struct ItemEntity
    {
        public ItemType Type { get; }
        public ItemData.ShopVariable ShopVariable { get; }
        public int Price => ShopVariable.Price;
        public int Amount => ShopVariable.Amount;
        public bool IsDonat => ShopVariable.IsDonat;
        
        public ItemEntity(ItemType type, ItemData.ShopVariable shopVariable)
        {
            Type = type;
            ShopVariable = shopVariable;
        }
    }
}