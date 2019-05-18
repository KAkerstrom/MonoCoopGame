using Microsoft.Xna.Framework;

namespace monoCoopGame.Tiles
{
    public class Grass : Blob
    { 
        public Grass(Point gridPos) : base("grass", BlobGroups.Grass, gridPos)
        {
        }
    }
}
