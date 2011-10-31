// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;

namespace TTengine.Core
{
    /**
     * parameters collection used for Draw() method of GameItems.
     */
    public class DrawParams
    {
        /// GameTime as passed by the XNA Game class
        public GameTime gameTime = null;

        /// create all params with null values
        public DrawParams()
        {
        }

        /// create params set with times according to a given GameTime
        public DrawParams(GameTime gameTime)
        {
            this.gameTime = gameTime;
        }

    }
}
