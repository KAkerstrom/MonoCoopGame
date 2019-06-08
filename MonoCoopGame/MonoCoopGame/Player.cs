using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monoCoopGame.Tiles;
using System.Collections.Generic;
using System.Diagnostics;

namespace monoCoopGame
{
    public partial class Player : Character
    {
        public int PlayerIndex { get; set; }
        public string Name { get; private set; }
        private PlayerGUI gui;
        public Reticle Reticle { get; }
        public Inventory Inventory;
        public int BombPower = 1;
        public int Speed = 2;
        public IController Controller { get; }

        public Player(string playerName, int playerIndex, int controllerIndex, int characterIndex, Inventory playerInventory, int x, int y) : base(x, y, characterIndex)
        {
            PlayerIndex = playerIndex;
            Name = playerName;
            Controller = ControllerFactory.GetController(controllerIndex);
            SetDefaultButtonMap(); // Action.cs
            sprite = sprites["walk"][Directions.South];
            Reticle = new Reticle(this);
            Inventory = playerInventory;
            gui = new PlayerGUI(this);
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

            DebugControls(gameState);
            foreach (Buttons button in buttonMap.Keys)
                if (Controller.ButtonPressed(button))
                    buttonMap[button].Perform(gameState);

            currentMoveSpeed = Controller.ButtonDown(Buttons.A) ? moveSpeed * 1.5f : moveSpeed;
            float xDelta = Controller.LeftStick.X * currentMoveSpeed;
            float yDelta = -Controller.LeftStick.Y * currentMoveSpeed;
            if (Controller.ButtonDown(Buttons.DPadUp)) yDelta = -currentMoveSpeed;
            if (Controller.ButtonDown(Buttons.DPadDown)) yDelta = currentMoveSpeed;
            if (Controller.ButtonDown(Buttons.DPadLeft)) xDelta = -currentMoveSpeed;
            if (Controller.ButtonDown(Buttons.DPadRight)) xDelta = currentMoveSpeed;
            if (!Controller.ButtonDown(Buttons.LeftTrigger))
                FaceTowardDelta(xDelta, yDelta);
            Strafe(gameState, xDelta, yDelta);
        }

        [Conditional("DEBUG")]
        private void DebugControls(GameState gameState)
        {
            TileMap map = gameState.Map;

            if (Controller.ButtonPressed(Buttons.RightShoulder))
                if (Controller.ButtonDown(Buttons.RightTrigger))
                {
                    if (!map.IsTileAtGridPos(TileMap.Layers.Dirt, Reticle.GridPos))
                        map.AddTile(TileMap.Layers.Dirt, new Dirt(Reticle.GridPos));
                    else if (!map.IsTileAtGridPos(TileMap.Layers.Grass, Reticle.GridPos))
                        map.AddTile(TileMap.Layers.Grass, new Grass(Reticle.GridPos));
                }
        }

        protected override void BeginDraw(SpriteBatch spriteBatch)
        {
            Reticle.Draw(spriteBatch, Controller.RightTrigger);
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
