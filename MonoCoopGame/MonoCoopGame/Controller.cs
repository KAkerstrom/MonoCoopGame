using Microsoft.Xna.Framework.Input;
using System.Timers;

namespace monoCoopGame
{
    public delegate void ButtonPressedDelegate(Buttons button);

    public class Controller
    {
        public int PlayerIndex { get; }
        public GamePadState State;
        public GamePadState PreviousState;

        private Timer vibrationTimer;

        public Controller(int playerIndex)
        {
            PlayerIndex = playerIndex;
            PreviousState = State = GamePad.GetState(playerIndex);
            vibrationTimer = new Timer();
            vibrationTimer.Elapsed += VibrationTimer_Elapsed;
        }

        private void VibrationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            vibrationTimer.Stop();
            GamePad.SetVibration(PlayerIndex, 0, 0);
        }

        public void Update()
        {
            PreviousState = State;
            State = GamePad.GetState(PlayerIndex);
        }

        public void Vibrate(float intensity, int milliseconds)
        {
            vibrationTimer.Interval = milliseconds;
            vibrationTimer.Start();
            GamePad.SetVibration(PlayerIndex, intensity, intensity);
        }

        public bool ButtonPressed(Buttons button)
        {
            return (State.IsButtonDown(button) && PreviousState.IsButtonUp(button));
        }

        public bool ButtonDown(Buttons button)
        {
            return State.IsButtonDown(button);
        }

        public bool ButtonUp(Buttons button)
        {
            return State.IsButtonUp(button);
        }
    }
}
