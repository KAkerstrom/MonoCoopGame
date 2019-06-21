using Microsoft.Xna.Framework.Graphics;

namespace monoCoopGame.InventoryItems
{
    public abstract class InventoryItem
    {
        public Texture2D Texture => Sprite.GetTexture(textureName);
        public string Name { get; }
        public int Quantity { get; set; }

        private string textureName;

        public InventoryItem(string name, string textureName, int quantity)
        {
            Name = name;
            this.textureName = textureName;
            Quantity = quantity;
        }

        public InventoryItem Copy()
        {
            return (InventoryItem)MemberwiseClone();
        }

        public abstract void Use(GameState gameState, Player player);
    }
}
