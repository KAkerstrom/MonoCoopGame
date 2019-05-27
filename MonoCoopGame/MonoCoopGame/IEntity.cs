using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monoCoopGame
{
    public delegate void EntityDestroyedDelegate(IEntity entity);

    public interface IEntity
    {
        event EntityDestroyedDelegate EntityDestroyed;
        Point Pos { get; }
        Point GridPos { get; }
        void Step(GameState gameState);
        void Draw(SpriteBatch spriteBatch);
    }
}
