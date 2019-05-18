﻿using Microsoft.Xna.Framework;

namespace monoCoopGame.Tiles
{
    public class Door : Tile, ISteppable, IUsable, IDestroyable
    {
        public event TileDestroyedDelegate TileDestroyed;
        public int Health { get; private set; }
        private bool isOpen = false;
        private bool isLiminal = false;

        public Door(Point gridPos) : base(new Sprite("doorWood"), gridPos)
        {
            HasTransparency = true;
        }

        public void Damage(int damage, GameState gameState, Player player = null)
        {
            Health -= damage;
            if (Health <= 0)
                Destroy(player);
        }

        public void Destroy(Player player = null)
        {
            TileDestroyed?.Invoke(this, player);
        }

        public void Step(GameState gameState)
        {
            if (isOpen)
            {
                if (!isLiminal)
                {
                    foreach (Character c in gameState.Characters)
                        if (c.GridPos == GridPos)
                            isLiminal = true;
                }
                else
                {
                    foreach (Character c in gameState.Characters)
                        if (c.GridPos != GridPos)
                        {
                            isLiminal = isOpen = false;
                            Sprite = new Sprite("doorWood_closed");
                        }
                }
            }
            SpeedModifier = isOpen ? 1 : 0;
        }

        public void Use(Player player, GameState gameState)
        {
            if (!isLiminal)
                isOpen = !isOpen;
            Sprite = new Sprite(isOpen? "doorWood_open" : "doorWood_closed");
        }
    }
}