using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Timers;

namespace monoCoopGame
{
    public delegate void PlayerConnectedDelegate(int playerIndex);
    public delegate void PlayerDisconnectedDelegate(int playerIndex);

    public class PlayerManager
    {
        public event PlayerConnectedDelegate PlayerConnected;
        public event PlayerDisconnectedDelegate PlayerDisconnected;

        private Timer controllerCheckTimer;
        private List<int> players = new List<int>();

        public PlayerManager()
        {
            controllerCheckTimer = new Timer(1000);
            controllerCheckTimer.Elapsed += ControllerCheckTimer_Elapsed;
            controllerCheckTimer.Start();
        }

        ~PlayerManager()
        {
            controllerCheckTimer.Dispose();
        }

        private void ControllerCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < GamePad.MaximumGamePadCount; i++)
                if (GamePad.GetState(i).IsConnected)
                {
                    if (!players.Contains(i))
                    {
                        players.Add(i);
                        PlayerConnected?.Invoke(i);
                    }
                }
                else if (players.Contains(i))
                    PlayerDisconnected?.Invoke(i);
        }
    }
}
