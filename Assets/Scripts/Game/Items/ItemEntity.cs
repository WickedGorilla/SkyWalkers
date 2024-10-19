using Infrastructure.Data.Game.Shop;

namespace Game.Items
{
    public struct ItemEntity
    {
        public ItemType Type { get; }
        public int Price { get; }
        public int Amount { get; }
        public bool IsDonat { get; }
        
        public ItemEntity(ItemType type, ItemData.ShopVariable shopVariable)
        {
            Type = type;
            Price = shopVariable.Price;
            Amount = shopVariable.Amount;
            IsDonat = shopVariable.IsDonat;
        }
    }
}