using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monoCoopGame.Tiles;
using System.Collections.Generic;

namespace monoCoopGame
{
    public partial class Player : Character
    {
        public int PlayerIndex { get; set; }
        private PlayerGUI gui;
        public Reticle Reticle { get; }
        public Inventory Inventory;
        public int BombPower = 2;
        public Controller Controller { get; }

        public Player(int playerIndex, int x, int y, float moveSpeed) : base(x, y, moveSpeed)
        {
            PlayerIndex = playerIndex;
            Controller = new Controller(playerIndex);
            PopulateTextures(playerIndex);
            SetDefaultButtonMap(); // Action.cs
            sprite = sprites["walk"][Directions.South];
            Reticle = new Reticle(this);
            Inventory = new Inventory();
            gui = new PlayerGUI(this);
        }

        private void PopulateTextures(int playerIndex)
        {
            texturePrefix = "char" + (playerIndex);
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
        }

        /// <summary>
        /// Performs the logic for each update.
        /// </summary>
        /// <param name="gameState">The game state.</param>
        public override void Step(GameState gameState)
        {
            Controller.Update();
            sprite.Update();
            TileMap map = gameState.Map;

            if (Controller.ButtonPressed(Buttons.LeftShoulder))
                if (Controller.ButtonDown(Buttons.RightTrigger))
                {
                    if (map.IsTileAtGridPos(TileMap.Layers.Grass, Reticle.GridPos))
                        map.RemoveTile(TileMap.Layers.Grass, Reticle.GridPos);
                    else if (map.IsTileAtGridPos(TileMap.Layers.Dirt, Reticle.GridPos))
                        map.RemoveTile(TileMap.Layers.Dirt, Reticle.GridPos);
                }

            if (Controller.ButtonPressed(Buttons.RightShoulder))
                if (Controller.ButtonDown(Buttons.RightTrigger))
                {
                    if (!map.IsTileAtGridPos(TileMap.Layers.Dirt, Reticle.GridPos))
                        map.AddTile(TileMap.Layers.Dirt, new Dirt(Reticle.GridPos));
                    else if (!map.IsTileAtGridPos(TileMap.Layers.Grass, Reticle.GridPos))
                        map.AddTile(TileMap.Layers.Grass, new Grass(Reticle.GridPos));
                }

            foreach (Buttons button in buttonMap.Keys)
                if (Controller.ButtonPressed(button))
                    buttonMap[button].Perform(gameState);
            
            currentMoveSpeed = Controller.ButtonDown(Buttons.A) ? moveSpeed * 1.5f : moveSpeed;
            currentMoveSpeed = currentMoveSpeed - (currentMoveSpeed * Controller.State.Triggers.Left * 0.5f);
            Move(gameState, Controller.State.ThumbSticks.Left.X * currentMoveSpeed, -Controller.State.ThumbSticks.Left.Y * currentMoveSpeed);
            if (!gameState.Map.IsTileAtGridPos(GridPos) || gameState.Map.GetBlockAtGridPos(GridPos) is Slime)
            {
                action = "swim";
                sprite = sprites[action][Facing];
            }
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
