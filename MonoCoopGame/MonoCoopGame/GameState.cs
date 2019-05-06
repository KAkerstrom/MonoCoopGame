using System.Collections.Generic;

namespace monoCoopGame
{
    public class GameState
    {
        public List<Character> Characters = new List<Character>();
        public TileMap Map;

        public GameState (TileMap map, List<Character> characters)
        {
            Map = map;
            Characters = characters;
        }
    }
}
