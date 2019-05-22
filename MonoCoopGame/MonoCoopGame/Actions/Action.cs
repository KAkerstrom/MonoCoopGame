using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace monoCoopGame
{
    public partial class Player : Character
    {
        private Dictionary<Buttons, Action> buttonMap = new Dictionary<Buttons, Action>
        {
            { Buttons.A, }
        };

        private abstract class Action
        {
            private Player parent;

            public Action(Player parent)
            {
                this.parent = parent;
            }

            public abstract void Perform(GameState gameState);
        }
    }
}
