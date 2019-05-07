using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monoCoopGame
{
    public abstract class Action
    {
        public int Cooldown { get; private set; }
        public int CurrentCharge { get; private set; }

        public void Perform()
        {

        }
    }
}
