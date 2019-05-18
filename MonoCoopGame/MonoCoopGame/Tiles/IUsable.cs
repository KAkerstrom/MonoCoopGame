using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monoCoopGame.Tiles
{
    interface IUsable
    {
        void Use(Player player, GameState gameState);
    }
}
