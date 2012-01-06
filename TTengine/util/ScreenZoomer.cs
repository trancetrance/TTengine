﻿using Microsoft.Xna.Framework.Input;
using TTengine.Core;

namespace TTengine.Util
{
    /// <summary>
    /// Attach this to a Screenlet to control the screen's zooming using page-up / page-down keys
    /// </summary>
    public class ScreenZoomer: Gamelet
    {
        Screenlet screen = null;

        protected override void OnNewParent()
        {
            base.OnNewParent();
            if (Parent is Screenlet)
            {
                screen = Parent as Screenlet;
            }
        }

        protected override void OnDraw(ref DrawParams p)
        {
            base.OnDraw(ref p);

            if (screen != null)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
                {
                    screen.Zoom += 0.003f;
                    Screen.DebugText(0.1f, 0.3f, "Zoom=" + screen.Zoom);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
                {
                    screen.Zoom -= 0.003f;
                    Screen.DebugText(0.1f, 0.3f, "Zoom=" + screen.Zoom);
                }
            }

        }

    }
}