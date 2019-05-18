using Microsoft.Xna.Framework.Graphics;
using monoCoopGame.Tiles;
using System.Collections.Generic;

namespace monoCoopGame
{
    public class GameState
    {
        public List<Character> Characters = new List<Character>();
        public TileMap Map;

        public GameState(TileMap map, List<Character> characters)
        {
            Map = map;
            Characters = characters;
        }

        public void Step()
        {
            foreach (Character c in Characters)
                c.Step(this);
            Map.Step(this);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Map.Draw(spriteBatch);
            foreach (Character character in Characters)
                character.Draw(spriteBatch);
        }

        public void DrawGUI(SpriteBatch spriteBatch)
        {
            foreach (Character character in Characters)
                if (character is Player)
                    ((Player)character).DrawGUI(spriteBatch);
        }
    }
}
