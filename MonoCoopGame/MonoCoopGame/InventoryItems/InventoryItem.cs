using Microsoft.Xna.Framework.Graphics;

namespace monoCoopGame.InventoryItems
{
    public abstract class InventoryItem
    {
        public Texture2D Texture => Sprite.GetTexture(textureName);
        public string Name { get; }
        public int Quantity { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }

        private string textureName;

        public InventoryItem(string name, string textureName, int quantity, int value, string description)
        {
            Name = name;
            this.textureName = textureName;
            Quantity = quantity;
            Value = value;
            Description = description;
        }

        public InventoryItem Copy()
        {
            return (InventoryItem)MemberwiseClone();
        }

        public abstract void Use(GameState gameState, Player player);
    }
}
