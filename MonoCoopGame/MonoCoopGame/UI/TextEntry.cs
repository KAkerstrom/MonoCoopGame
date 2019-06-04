using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace monoCoopGame.UI
{
    class TextEntry
    {
        private static bool KeyboardInputEnabled = true;

        public string Text { get { return new string(text).Trim(); } }
        public int MaxLength { get { return text.Length; } }
        public bool Active { get { return controller != null; } }

        private int repeatKeyTimer = 0;
        private const int repeatKeyTrigger = 10;
        private char[] text;
        private IController controller;
        private Rectangle bounds;
        private int index = 0;
        private KeyboardState keyState;
        private KeyboardState previousKeyState;

        public TextEntry(int maxLength, Rectangle drawBounds)
        {
            bounds = drawBounds;
            text = new char[maxLength];
            for (int i = 0; i < maxLength; i++)
                text[i] = ' ';
        }

        public void Activate(IController controller)
        {
            if (controller is KeyboardController)
                KeyboardInputEnabled = false;
            this.controller = controller;
            keyState = Keyboard.GetState();
            controller.Update();
        }

        public void Deactivate()
        {
            if (controller is KeyboardController)
                KeyboardInputEnabled = true;
            controller = null;
        }

        public void Clear()
        {
            for (int i = 0; i < MaxLength; i++)
                text[i] = ' ';
            index = 0;
        }

        public void SetText(string text)
        {
            if (text.Length > MaxLength)
                throw new Exception("Text exceeds max length.");
            else
            {
                Clear();
                for (int i = 0; i < text.Length; i++)
                    this.text[i] = text[i];
                index = text.Length - 1;
            }
        }

        public void Step()
        {
            if (Active)
            {
                //if (KeyboardInputEnabled || controller is KeyboardController)
                //{
                //    //Keyboard Input
                //    Keys key = GetKeyboardInput();
                //    switch (key)
                //    {
                //        case Keys.Enter:
                //            Deactivate();
                //            return;
                //        case Keys.Back:
                //            text[index] = ' ';
                //            if (index == 0)
                //            {
                //                Deactivate();
                //                return;
                //            }
                //            else
                //            {
                //                if (index > 0)
                //                    index--;
                //            }
                //            break;
                //        case 0: // Do nothing
                //            break;
                //        default: // Characters + space
                //            text[index] = (char)key;
                //            if (++index == MaxLength)
                //            {
                //                index--;
                //                Deactivate();
                //                return;
                //            }
                //            break;
                //    }
                //}

                // Controller Input
                if (controller.ButtonPressed(Buttons.A)
                    || controller.ButtonPressed(Buttons.B))
                {
                    Deactivate();
                    return;
                }

                if (controller.ButtonUp(Buttons.DPadDown)
                    && controller.ButtonUp(Buttons.DPadUp)
                    && controller.LeftStick.Y <= 0.5
                    && controller.LeftStick.Y >= -0.5)
                    repeatKeyTimer = 0;
                else
                    repeatKeyTimer++;

                if (repeatKeyTimer == 1 || repeatKeyTimer == repeatKeyTrigger)
                {
                    if (repeatKeyTimer == repeatKeyTrigger)
                        repeatKeyTimer = 0;

                    if (controller.LeftStick.Y < -0.5
                        || controller.ButtonDown(Buttons.DPadDown))
                    {
                        if (text[index] > (char)Keys.A)
                            text[index]--;
                        else if (text[index] == (char)Keys.A)
                            text[index] = ' ';
                    }
                    else if (controller.LeftStick.Y > 0.5
                        || controller.ButtonDown(Buttons.DPadUp))
                    {
                        if (text[index] < (char)Keys.Z)
                        {
                            if (text[index] == ' ')
                                text[index] = 'A';
                            else
                                text[index]++;
                        }
                    }
                }

                if ((controller.LeftStick.X > 0.5
                    && controller.PreviousLeftStick.X <= 0.5)
                    || controller.ButtonPressed(Buttons.DPadRight))
                {
                    if (index < MaxLength - 1)
                        index++;
                }
                else if ((controller.LeftStick.X < -0.5
                    && controller.PreviousLeftStick.X >= -0.5)
                    || controller.ButtonPressed(Buttons.DPadLeft))
                {
                    if (index > 0)
                        index--;
                }
            }
        }

        private Keys GetKeyboardInput()
        {
            previousKeyState = keyState;
            keyState = Keyboard.GetState();

            Keys key = 0;
            List<Keys> pressed = keyState.GetPressedKeys().ToList();
            foreach (Keys k in pressed)
                if (!previousKeyState.IsKeyDown(k)
                    && k != Keys.LeftShift
                    && k != Keys.RightShift)
                {
                    key = k;
                    break;
                }
            if ((key < Keys.A || key > Keys.Z)
                && key != Keys.Space
                && key != Keys.Enter
                && key != Keys.Back)
                return 0;
            return key;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite.GetTexture(Active ? "titleMenuSelected" : "titleMenuUnselected"), bounds, Color.White);
            SpriteFont font = Utility.Fonts["blocks"];
            for (int i = 0; i < MaxLength; i++)
            {
                Vector2 charPosition = new Vector2(bounds.X + 24 + font.MeasureString("W").X * i, bounds.Y);
                charPosition.X += (font.MeasureString("W").X - font.MeasureString(text[i].ToString()).X) / 2;
                spriteBatch.DrawString(font, text[i].ToString(), charPosition, Active && i == index ? Color.LightYellow : Color.Black);
            }
        }
    }
}
