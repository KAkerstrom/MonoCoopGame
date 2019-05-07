using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace monoCoopGame
{
    public class Player : Character
    {
        public int PlayerIndex { get; set; }

        public Player(int x, int y, float moveSpeed) : base(x, y, moveSpeed)
        {
            Texture2D[] textures = new Texture2D[]
            {
                Sprite.GetTexture("char01_s_01"),
                Sprite.GetTexture("char01_s_02"),
                Sprite.GetTexture("char01_s_01"),
                Sprite.GetTexture("char01_s_03"),
            };
            sprite = new Sprite(textures, 20);
            PlayerIndex = 0;
        }

        /// <summary>
        /// Performs the logic for each frame.
        /// Wall collision could be made more efficient.
        /// Currently checks 4 corners per direction, rather than a theoretical max of 3.
        /// </summary>
        /// <param name="gameState">The game state.</param>
        public override void Step(GameState gameState)
        {
            sprite.Update();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex);
            Move(gameState, gamePadState.ThumbSticks.Left.X * moveSpeed, -gamePadState.ThumbSticks.Left.Y * moveSpeed);
        }
    }
}
