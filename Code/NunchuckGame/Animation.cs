
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NunchuckGame
{
    class Animation
    {
        Texture2D spriteSheet;
        float scale;
        int elapsedTime;
        int frameTime;
        int frameCount;
        int currentFrame;
        Color color;
        Rectangle srcRect = new Rectangle();
        Rectangle desRect = new Rectangle();
        public int FrameWidth;
        public int FrameHeight;
        public bool Active;
        public bool Looping;
        public Vector2 Position;
        public void Initialize(Texture2D spriteSheetTexture, Vector2 positon, int frameWidth, int frameHeight, int frameCount, int frameTime, Color color, float scale, bool looping)
        {
            this.color = color;
            this.frameCount = frameCount;
            this.frameTime = frameTime;
            this.scale = scale;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;

            spriteSheet = spriteSheetTexture;
            Position = positon;
            Looping = looping;

            elapsedTime = 0;
            currentFrame = 0;

            Active = true;


        }
        public void Update(GameTime gameTime, Vector2 position, int frameTime,int srcY)
        {
            Position = position;
            this.frameTime = frameTime;
            if (Active == false) return;

            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime > frameTime)
            {
                currentFrame++;

                if (currentFrame == frameCount)
                {
                    currentFrame = 0;

                    if (Looping == false)
                        Active = false;
                }

                elapsedTime = 0;
            }

            srcRect = new Rectangle(currentFrame * FrameWidth, srcY, FrameWidth, FrameHeight);
            desRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(FrameWidth * scale), (int)(FrameHeight * scale));


        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteBatch.Draw(spriteSheet, desRect, srcRect, color);
            }
        }
    }
}