using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Timers;

namespace monoCoopGame
{
    public delegate void PlayerConnectedDelegate(IController controller);
    public delegate void PlayerDisconnectedDelegate(int controllerIndex);

    public class PlayerManager
    {
        public static List<IController> GetConnectedControllers()
        {
            List<IController> controllers = new List<IController> { new KeyboardController(-1) };
            for (int i = 0; i < GamePad.MaximumGamePadCount; i++)
                if (GamePad.GetState(i).IsConnected)
                    controllers.Add(new GamepadController(i));
            return controllers;
        }

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
            if (!players.Contains(-1))
            {
                players.Add(-1);
                PlayerConnected?.Invoke(new KeyboardController(-1));
            }

            for (int i = 0; i < GamePad.MaximumGamePadCount; i++)
                if (GamePad.GetState(i).IsConnected)
                {
                    if (!players.Contains(i))
                    {
                        players.Add(i);
                        PlayerConnected?.Invoke(new GamepadController(i));
                    }
                }
                else if (players.Contains(i))
                    PlayerDisconnected?.Invoke(i);
        }
    }
}
