//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace monoCoopGame
//{

//    public abstract class Block
//    {
//        public bool Visible = true;

//        public Block(Sprite sprite, Point gridPos, Player owner = null)
//        {
//            GridPos = gridPos;
//            Sprite = sprite;
//            Owner = owner;
//        }

//        public virtual void Damage(Player player, GameState gameState, int damage)
//        {
//            Health -= damage;
//            if (Health <= 0)
//                Destroy(player);
//        }

//        public virtual void Destroy(Player player)
//        {
//            BlockDestroyed?.Invoke(this, player);
//        }

//        public virtual void Draw(SpriteBatch spriteBatch)
//        {
//            Sprite.Draw(spriteBatch, Pos.X, Pos.Y);
//        }
//    }
//}
