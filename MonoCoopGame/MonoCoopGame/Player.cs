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
        public int BombPower = 1;
        public Controller Controller { get; }

        public Player(int playerIndex, int x, int y, float moveSpeed) : base(x, y, moveSpeed)
        {
            PlayerIndex = playerIndex;
            Controller = new Controller(playerIndex);
            PopulateTextures(playerIndex);
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

            //some debug stuff
            currentMoveSpeed = Controller.ButtonDown(Buttons.A) ? moveSpeed * 1.5f : moveSpeed;
            currentMoveSpeed = currentMoveSpeed - (currentMoveSpeed * Controller.State.Triggers.Left * 0.5f);

            if (Controller.ButtonPressed(Buttons.LeftShoulder))
            {
                if (Controller.ButtonDown(Buttons.RightTrigger))
                {
                    if (map.IsTileAtGridPos(TileMap.Layers.Grass, Reticle.GridPos))
                        map.RemoveTile(TileMap.Layers.Grass, Reticle.GridPos);
                    else if (map.IsTileAtGridPos(TileMap.Layers.Dirt, Reticle.GridPos))
                        map.RemoveTile(TileMap.Layers.Dirt, Reticle.GridPos);
                }
                else
                    Inventory.DecrementIndex();
            }

            if (Controller.ButtonPressed(Buttons.RightShoulder))
            {
                if (Controller.ButtonDown(Buttons.RightTrigger))
                {
                    if (!map.IsTileAtGridPos(TileMap.Layers.Dirt, Reticle.GridPos))
                        map.AddTile(TileMap.Layers.Dirt, new Dirt(Reticle.GridPos));
                    else if (!map.IsTileAtGridPos(TileMap.Layers.Grass, Reticle.GridPos))
                        map.AddTile(TileMap.Layers.Grass, new Grass(Reticle.GridPos));
                }
                else
                    Inventory.IncrementIndex();
            }

            if (Controller.ButtonPressed(Buttons.X))
                if (map.IsTileAtGridPos(Reticle.GridPos)
                    && !gameState.Map.IsTileAtGridPos(TileMap.Layers.Blocks, Reticle.GridPos))
                    switch (Inventory.GetCurrentItem())
                    {
                        case "wallStone":
                            gameState.Map.AddTile(new WallStone(Reticle.GridPos));
                            break;
                        case "slime":
                            gameState.Map.AddTile(new Slime(Reticle.GridPos));
                            break;
                        case "door":
                            gameState.Map.AddTile(new Door(Reticle.GridPos));
                            break;
                        case "bush":
                            gameState.Map.AddTile(new Bush(Reticle.GridPos));
                            break;
                        case "bomb":
                            gameState.Map.AddTile(new Bomb(Reticle.GridPos, this));
                            break;
                    }

            if (Controller.ButtonPressed(Buttons.A))
                if (map.IsTileAtGridPos(Reticle.GridPos)
                        && map.GetBlockAtGridPos(Reticle.GridPos) is IUsable)
                    ((IUsable)map.GetBlockAtGridPos(Reticle.GridPos)).Use(this, gameState);

            if (Controller.ButtonPressed(Buttons.B))
            {
                if (gameState.Map.IsTileAtGridPos(Reticle.GridPos)
                    && map.GetBlockAtGridPos(Reticle.GridPos) is IDestroyable)
                    ((IDestroyable)gameState.Map.GetBlockAtGridPos(Reticle.GridPos)).Damage(1, gameState, this);
            }

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
