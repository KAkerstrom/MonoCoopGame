using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monoCoopGame.Blocks;
using System.Collections.Generic;

namespace monoCoopGame
{
    public class Player : Character
    {
        public int PlayerIndex { get; set; }
        private PlayerGUI gui;
        public Reticle Reticle { get; }

        private GamePadState previousGamePadState;

        public Player(int playerIndex, int x, int y, float moveSpeed) : base(x, y, moveSpeed)
        {
            PlayerIndex = playerIndex;
            texturePrefix = "char" + (playerIndex + 1);
            string[] actions = { "walk", "swim" };
            foreach (string act in actions)
            {
                if (!sprites.ContainsKey(act))
                    sprites.Add(act, new Dictionary<Directions, Sprite>());

                for (int dir = 0; dir < 4; dir++)
                {
                    List<Texture2D> newSprite = new List<Texture2D>();
                    string textureName = $"{texturePrefix}_{act}_" + "news"[dir] + "_";
                    int index = 0;
                    while (Sprite.TextureExists(textureName + index))
                        newSprite.Add(Sprite.GetTexture(textureName + index++));
                    sprites[act].Add((Directions)dir, new Sprite(newSprite.ToArray(), 15));
                }
            }
            sprite = sprites["walk"][Directions.South];
            Reticle = new Reticle(this);
            gui = new PlayerGUI(new Rectangle(16, 0, 250, 100), this);
        }

        /// <summary>
        /// Performs the logic for each update.
        /// </summary>
        /// <param name="gameState">The game state.</param>
        public override void Step(GameState gameState)
        {
            sprite.Update();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex);

            //some debug stuff
            currentMoveSpeed = gamePadState.IsButtonDown(Buttons.A) ? moveSpeed * 2 : moveSpeed;

            Tile.TileType oldType = gameState.Map.TileMap[Reticle.GridPos.X, Reticle.GridPos.Y].Type;
            if (gameState.Map.GridPointIsInMap(Reticle.GridPos))
            {
                if (gamePadState.IsButtonDown(Buttons.LeftShoulder) && previousGamePadState.IsButtonUp(Buttons.LeftShoulder))
                {
                    if ((int)oldType - 1 >= 0)
                        gameState.Map.ChangeTile(Reticle.GridPos.X, Reticle.GridPos.Y, new Tile((Tile.TileType)((int)oldType - 1)));
                }

                if (gamePadState.IsButtonDown(Buttons.RightShoulder) && previousGamePadState.IsButtonUp(Buttons.RightShoulder))
                {
                    if ((int)oldType + 1 <= 3)
                        gameState.Map.ChangeTile(Reticle.GridPos.X, Reticle.GridPos.Y, new Tile((Tile.TileType)((int)oldType + 1)));
                }

                if (gamePadState.IsButtonDown(Buttons.X) && previousGamePadState.IsButtonUp(Buttons.X))
                    if (gameState.Map.IsBlockAtGridPos(Reticle.GridPos))
                        gameState.Map.GetBlockAtGridPos(Reticle.GridPos).Use(this, gameState);

                if (gamePadState.IsButtonDown(Buttons.B) && previousGamePadState.IsButtonUp(Buttons.B))
                {
                    if (gameState.Map.IsBlockAtGridPos(Reticle.GridPos))
                        gameState.Map.GetBlockAtGridPos(Reticle.GridPos).Damage(this, gameState, 1);
                }

                if (gamePadState.DPad.Down == ButtonState.Pressed)
                    if (gameState.Map.GetTileAtGridPos(Reticle.GridPos).Type != Tile.TileType.Water
                        && !gameState.Map.IsBlockAtGridPos(Reticle.GridPos))
                        gameState.Map.AddBlock(new Bush(Reticle.GridPos));

                if (gamePadState.DPad.Left == ButtonState.Pressed)
                    if (gameState.Map.GetTileAtGridPos(Reticle.GridPos).Type != Tile.TileType.Water
                        && !gameState.Map.IsBlockAtGridPos(Reticle.GridPos))
                        gameState.Map.AddBlock(new WallStone(Reticle.GridPos));

                if (gamePadState.DPad.Up == ButtonState.Pressed)
                    if (gameState.Map.GetTileAtGridPos(Reticle.GridPos).Type != Tile.TileType.Water
                        && !gameState.Map.IsBlockAtGridPos(Reticle.GridPos))
                        gameState.Map.AddBlock(new Door(Reticle.GridPos));

            }

            Move(gameState, gamePadState.ThumbSticks.Left.X * currentMoveSpeed, -gamePadState.ThumbSticks.Left.Y * currentMoveSpeed);
            previousGamePadState = gamePadState;
        }

        protected override void BeginDraw(SpriteBatch spriteBatch)
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex);
            Reticle.Draw(spriteBatch, gamePadState.Triggers.Left);
        }

        protected override void EndDraw(SpriteBatch spriteBatch)
        {
        }

        public void DrawGUI(SpriteBatch spriteBatch)
        {
            gui.Draw(spriteBatch);
        }
    }
}
