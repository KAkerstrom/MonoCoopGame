using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monoCoopGame.UI
{
    public class TitleMenuItem : MenuItem
    {
        public TitleMenuItem(string name) : base
            (
            name,
            Utility.Fonts["blocks"],
            Sprite.GetTexture("titleMenuSelected"),
            Sprite.GetTexture("titleMenuUnselected"),
            Sprite.GetTexture("titleMenuDisabled")
            )
        {
        }

        protected override void ActivateDerived()
        {
        }
    }
}
