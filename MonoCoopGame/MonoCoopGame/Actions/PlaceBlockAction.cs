using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monoCoopGame
{
    public partial class Player : Character
    {
        class PlaceBlockAction : Action
        {
            public PlaceBlockAction(Player parent) : base(parent)
            {
            }

            public override void Perform(GameState gameState)
            {

            }
        }
    }
}
