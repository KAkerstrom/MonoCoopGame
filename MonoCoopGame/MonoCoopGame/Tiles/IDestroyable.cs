﻿namespace monoCoopGame.Tiles
{
    public delegate void TileDestroyedDelegate(Tile tile, Player player);

    interface IDestroyable
    {
        event TileDestroyedDelegate TileDestroyed;

        int Health { get; }
        int InvulnFrames { get; set; }
        void Damage(int damage, GameState gameState, Player player = null);
        void Destroy(GameState gameState, Player player = null);
    }
}
