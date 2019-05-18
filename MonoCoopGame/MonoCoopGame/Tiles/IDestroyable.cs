namespace monoCoopGame.Tiles
{
    public delegate void TileDestroyedDelegate(Tile tile, Player player);

    interface IDestroyable
    {
        event TileDestroyedDelegate TileDestroyed;

        int Health { get; }
        void Damage(int damage, GameState gameState, Player player = null);
        void Destroy(Player player = null);
    }
}
