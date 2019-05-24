using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace monoCoopGame
{
    public partial class Player : Character
    {
        private Dictionary<Buttons, Action> buttonMap;

        private void SetDefaultButtonMap()
        {
            buttonMap = new Dictionary<Buttons, Action>
            {
                {Buttons.RightShoulder, new PlaceBlockAction(this) },
                {Buttons.B, new AttackAction(this) },
                {Buttons.A, new UseAction(this) },
                {Buttons.Y, new IncrementInventoryIndexAction(this) },
                {Buttons.X, new DecrementInventoryIndexAction(this) },
            };
        }

        private abstract class Action
        {
            protected Player parent;
            protected Reticle reticle { get { return parent.Reticle; } }

            public Action(Player parent)
            {
                this.parent = parent;
            }

            public abstract void Perform(GameState gameState);
        }
    }
}
