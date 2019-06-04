using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace monoCoopGame
{
    public interface IController
    {
        int ControllerIndex { get; }
        float RightTrigger { get; }
        float LeftTrigger { get; }
        Vector2 LeftStick { get; }
        Vector2 RightStick { get; }
        Vector2 PreviousLeftStick { get; }
        Vector2 PreviousRightStick { get; }

        void Update();
        void Vibrate(float intensity, int milliseconds);
        bool ButtonPressed(Buttons button);
        bool ButtonDown(Buttons button);
        bool ButtonUp(Buttons button);
        bool LeftStickMoved(Directions direction, float threshold = 0.5f);
        bool RightStickMoved(Directions direction, float threshold = 0.5f);
    }
}
