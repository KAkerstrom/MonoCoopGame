using Microsoft.Xna.Framework;

namespace monoCoopGame.Tiles
{
    class Dirt : Blob
    {
        public Dirt(Point gridPos) : base("dirt", BlobGroups.Dirt, gridPos)
        { }
    }
}
