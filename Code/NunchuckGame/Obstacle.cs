using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NunchuckGame
{
    class Obstacle : Sprite
    {
        public static float Depth = 0.6f;
        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)((float)Texture.Width * Scale), (int)((float)Texture.Height * Scale * 0.66f));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, color, 0f, Vector2.Zero, Scale, SpriteEffects.None, Depth);
        }
    }
}
