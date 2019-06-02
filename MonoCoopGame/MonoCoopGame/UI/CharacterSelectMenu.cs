using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monoCoopGame.UI
{
    class CharacterSelectMenu
    {
        public enum MenuStates { PressStart, Selection, Ready }

        public Player player { get; }
        public MenuStates State = MenuStates.PressStart;

        public CharacterSelectMenu(int playerIndex)
        {
            player = new Player(playerIndex, 0, 0);
        }

        public void Step()
        {
            player.Controller.Update();
            switch (State)
            {
                case MenuStates.PressStart:
                    if (player.Controller.ButtonPressed(Buttons.Start))
                        State = MenuStates.Selection;
                    break;
                case MenuStates.Selection:
                    break;
                case MenuStates.Ready:
                    if (player.Controller.ButtonPressed(Buttons.B))
                        State = MenuStates.Selection;
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle drawBounds)
        {
            switch (State)
            {
                case MenuStates.PressStart:
                    break;
                case MenuStates.Selection:
                    break;
                case MenuStates.Ready:
                    break;
            }
        }
    }
}
