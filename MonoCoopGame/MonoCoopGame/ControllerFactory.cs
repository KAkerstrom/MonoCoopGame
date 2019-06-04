namespace monoCoopGame
{
    public static class ControllerFactory
    {
        public static IController GetController(int controllerIndex)
        {
            if (controllerIndex < 0)
                return new KeyboardController(controllerIndex);
            else
                return new GamepadController(controllerIndex);
        }
    }
}
