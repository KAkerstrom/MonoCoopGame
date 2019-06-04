using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Timers;

namespace monoCoopGame
{
    class GamepadController : IController
    {
        public int ControllerIndex { get; }
        public float LeftTrigger { get { return state.Triggers.Left; } }
        public float RightTrigger { get { return state.Triggers.Right; } }
        public Vector2 LeftStick { get { return state.ThumbSticks.Left; } }
        public Vector2 RightStick { get { return state.ThumbSticks.Right; } }
        public Vector2 PreviousLeftStick { get { return previousState.ThumbSticks.Left; } }
        public Vector2 PreviousRightStick { get { return previousState.ThumbSticks.Right; } }

        private Timer vibrationTimer;
        private GamePadState state;
        private GamePadState previousState;

        public GamepadController(int controllerIndex)
        {
            ControllerIndex = controllerIndex;
            previousState = state = GamePad.GetState(controllerIndex);
            vibrationTimer = new Timer();
            vibrationTimer.Elapsed += VibrationTimer_Elapsed;
        }

        public bool LeftStickMoved(Directions direction, float threshold = 0.5f)
        {
            switch (direction)
            {
                case Directions.North: return LeftStick.Y > 0.5 && PreviousLeftStick.Y <= 0.5;
                case Directions.East: return LeftStick.X > 0.5 && PreviousLeftStick.X <= 0.5;
                case Directions.West: return LeftStick.X < -0.5 && PreviousLeftStick.X >= -0.5;
                case Directions.South: return LeftStick.Y < -0.5 && PreviousLeftStick.Y >= -0.5;
                default: return false;
            }
        }

        public bool RightStickMoved(Directions direction, float threshold = 0.5f)
        {
            switch (direction)
            {
                case Directions.North: return RightStick.Y > 0.5 && PreviousRightStick.Y <= 0.5;
                case Directions.East: return RightStick.X > 0.5 && PreviousRightStick.X <= 0.5;
                case Directions.West: return RightStick.X < -0.5 && PreviousRightStick.X >= -0.5;
                case Directions.South: return RightStick.Y < -0.5 && PreviousRightStick.Y >= -0.5;
                default: return false;
            }
        }

        private void VibrationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            vibrationTimer.Stop();
            GamePad.SetVibration(ControllerIndex, 0, 0);
        }

        public  void Vibrate(float intensity, int milliseconds)
        {
            vibrationTimer.Interval = milliseconds;
            vibrationTimer.Start();
            GamePad.SetVibration(ControllerIndex, intensity, intensity);
        }

        public void Update()
        {
            previousState = state;
            state = GamePad.GetState(ControllerIndex);
        }

        public  bool ButtonPressed(Buttons button)
        {
            return (state.IsButtonDown(button) && previousState.IsButtonUp(button));
        }

        public  bool ButtonDown(Buttons button)
        {
            return state.IsButtonDown(button);
        }

        public  bool ButtonUp(Buttons button)
        {
            return state.IsButtonUp(button);
        }
    }
}
