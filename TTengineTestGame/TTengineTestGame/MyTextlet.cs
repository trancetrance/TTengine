// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TTengine;
 using TTengine.Core;

namespace TTengineTestGame
{
    public class MyTextlet : Gamelet
    {
        protected string text;
        protected SpriteFont spriteFont;

        public MyTextlet( string text)
        {
            CreateDrawlet();
            this.text = text;
            DrawC.DrawColor = Color.White;
            spriteFont = TTGame.Instance.Content.Load<SpriteFont>("Font1");
        }

        protected override void OnDraw(ref DrawParams p)
        {
            Vector2 pos = Motion.PositionAbs;
            Vector2 posPixels = pos * TTGame.Instance.GraphicsDevice.DisplayMode.Height;
            DrawC.MySpriteBatch.DrawString(spriteFont, text, posPixels, DrawC.DrawColor);
        }
    }
}
