using Microsoft.Xna.Framework.Graphics;
using monoCoopGame.Tiles;
using System.Collections.Generic;

namespace monoCoopGame
{
    public class GameState
    {
        public List<Player> Players = new List<Player>();
        public TileMap Map;

        public GameState(TileMap map, List<Player> players)
        {
            Map = map;
            Players = players;
        }

        public void Step()
        {
            foreach (Character c in Players)
                c.Step(this);
            Map.Step(this);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Map.DrawBegin(spriteBatch);
            foreach (Character character in Players)
                character.Draw(spriteBatch);
            Map.DrawEnd(spriteBatch);
        }

        public void DrawGUI(SpriteBatch spriteBatch)
        {
            foreach (Character character in Players)
                if (character is Player)
                    ((Player)character).DrawGUI(spriteBatch);
        }
    }
}
