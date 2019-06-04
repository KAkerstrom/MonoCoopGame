using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace monoCoopGame
{
    public abstract class State
    {
        public delegate void FullScreenChangedDelegate(bool fullscreen);
        public delegate void GameQuitDelegate();
        public delegate void StateChangedDelegate(State newState);
        public static event FullScreenChangedDelegate FullScreenChanged;
        public static event GameQuitDelegate GameQuit;
        public static event StateChangedDelegate StateChanged;

        private static bool isFullScreen;
        private static State previousState;
        private static State currentState;

        public static State CurrentState
        {
            get => currentState;
            set
            {
                value.graphics.Viewport = new Viewport(value.graphics.PresentationParameters.Bounds);
                previousState = currentState;
                currentState = value;
                StateChanged?.Invoke(value);
            }
        }

        public static bool IsFullScreen
        {
            get => isFullScreen;
            set
            {
                if (value != isFullScreen)
                {
                    isFullScreen = value;
                    FullScreenChanged?.Invoke(value);
                }
            }
        }

        protected SpriteBatch spriteBatch;
        protected GraphicsDevice graphics;

        public State(GraphicsDevice graphics)
        {
            this.graphics = graphics;
            spriteBatch = new SpriteBatch(graphics);
        }

        public abstract void Step();
        public abstract void Draw();

        protected void QuitGame()
        {
            GameQuit?.Invoke();
        }
    }
}
