using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace monoCoopGame
{
    class KeyboardController : IController
    {
        private static List<Dictionary<Buttons, Keys>> allKeyMaps;
        static KeyboardController()
        {
            allKeyMaps = new List<Dictionary<Buttons, Keys>>();
            allKeyMaps.Add(new Dictionary<Buttons, Keys>
            {
                { Buttons.DPadDown, Keys.Down },
                { Buttons.DPadUp, Keys.Up },
                { Buttons.DPadLeft, Keys.Left },
                { Buttons.DPadRight, Keys.Right },
                { Buttons.A, Keys.Z },
                { Buttons.X, Keys.X },
                { Buttons.B, Keys.C },
                { Buttons.Y, Keys.V },
                { Buttons.LeftShoulder, Keys.A },
                { Buttons.RightShoulder, Keys.S },
                { Buttons.Back, Keys.Back },
                { Buttons.Start, Keys.Enter },
            });
        }
        public int ControllerIndex { get; }
        public float LeftTrigger { get { return 0; } }
        public float RightTrigger { get { return 0; } }
        public Vector2 LeftStick { get { return new Vector2(0, 0); } }
        public Vector2 RightStick { get { return new Vector2(0, 0); } }
        public Vector2 PreviousLeftStick { get { return new Vector2(0, 0); } }
        public Vector2 PreviousRightStick { get { return new Vector2(0, 0); } }

        private KeyboardState state;
        private KeyboardState previousState;
        private Dictionary<Buttons, Keys> keyMap;

        public KeyboardController(int controllerIndex)
        {
            ControllerIndex = controllerIndex;
            if (controllerIndex >= 0)
                throw new Exception("Keyboard controllers must have negative indices.");
            keyMap = allKeyMaps[(controllerIndex + 1) * -1];
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

        public void Update()
        {
            previousState = state;
            state = Keyboard.GetState();
        }

        public bool ButtonPressed(Buttons button)
        {
            return (state.IsKeyDown(keyMap[button]) && previousState.IsKeyUp(keyMap[button]));
        }

        public bool ButtonDown(Buttons button)
        {
            return state.IsKeyDown(keyMap[button]);
        }

        public bool ButtonUp(Buttons button)
        {
            return state.IsKeyUp(keyMap[button]);
        }

        public void Vibrate(float intensity, int milliseconds) { }
    }
}
