using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace NunchuckGame
{
    class MainPlayer : Sprite
    {
        public float BoostMeter = 1.2f;
        public bool Boosting = false;
        public double angle = 0f;
        public List<SoundEffect> allsounds;
        int speed = 50;
        int FuryAnimDuration = 50;
        int AngryAnimDuration = 50;
        Animation playerAnimation = new Animation();
        Animation furyAnimation = new Animation();
        Animation angryAnimation = new Animation();

        public Texture2D arrowTexture { get; set; }
        public Texture2D baseTexture { get; set; }
        public Texture2D furyTexture { get; set; }
        public Texture2D angryTexture { get; set; }

        public MainPlayer( Vector2 position, Vector2 velocity,List<SoundEffect> allsounds)
        {
            Position = position;
            Velocity = velocity;
            this.allsounds = allsounds;
        }
        public void LoadTexture(Texture2D playerTexture, Texture2D arrowTexture, Texture2D baseTexture, Texture2D furyTexture, Texture2D angryTexture)
        {
            Texture = playerTexture;
            this.arrowTexture = arrowTexture;
            this.baseTexture = baseTexture;
            this.furyTexture = furyTexture;
            this.angryTexture = angryTexture;
        }
        public void controller(GameTime gameTime, Viewport ScreenSize, ref GameState gameState)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            angle = Math.Atan2(Velocity.Y, Velocity.X);
            float magnitude ;
            

           
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                angle += (350 * Math.PI) / 180f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                angle -= (350 * Math.PI) / 180f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Up)) && BoostMeter>0)
            {
                magnitude = 500f;
                BoostMeter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                Boosting = true;
                speed = 25;
                allsounds[0].Play();
            }
            else
            {
                Boosting = false;
                speed = 50;
                magnitude = 250f;
                if(BoostMeter<1.2f && (Keyboard.GetState().IsKeyUp(Keys.Space) && Keyboard.GetState().IsKeyUp(Keys.Up)))
                    BoostMeter += (float)gameTime.ElapsedGameTime.TotalSeconds/4;
            }
            
            Velocity.X = (float)Math.Cos(angle);    
            Velocity.Y = (float)Math.Sin(angle);

            Velocity = Vector2.Multiply(Velocity, magnitude);
            
            Position += Vector2.Multiply(Velocity, (float)(gameTime.ElapsedGameTime.TotalSeconds));
            if(Position.X < 110f)
            {
                Position.X = 110f;
            }
            if((Position.X + Rectangle.Width) > ScreenSize.Width-150)
            {
                Position.X  = ScreenSize.Width - 150-Rectangle.Width;
            }
            if (Position.Y < 100f)
            {
                Position.Y = 100f;
            }
            if ((Position.Y + Rectangle.Height) > ScreenSize.Height - 120)
            {
                Position.Y = ScreenSize.Height - 120 - Rectangle.Height;
            }

            Rectangle playerRect = this.Rectangle;

            int proximity = 999999999;
            foreach (Sprite obstacle in gameState.Obstacles)
            {
                Rectangle obstacleRect = obstacle.Rectangle;
                int minHorizontal = Math.Min(Math.Abs(obstacleRect.Left - playerRect.Right), Math.Abs(playerRect.Left - obstacleRect.Right));
                int minVertical = Math.Min(Math.Abs(obstacleRect.Top - playerRect.Bottom), Math.Abs(playerRect.Top - obstacleRect.Bottom));
                if (minHorizontal < minVertical)
                {
                    if (this.IsTouchingLeft(obstacle))
                    {
                        Position.X = obstacleRect.X - playerRect.Width - (playerRect.X - Position.X);
                    }
                    else if (this.IsTouchingRight(obstacle))
                    {
                        Position.X = obstacle.Rectangle.X + obstacle.Rectangle.Width - (Rectangle.X - Position.X);
                    }
                }
                else
                {
                    if (this.IsTouchingTop(obstacle))
                    {
                        Position.Y = obstacle.Rectangle.Y - Rectangle.Height - (Rectangle.Y - Position.Y);
                    }
                    else if (this.IsTouchingBottom(obstacle))
                    {
                        Position.Y = obstacle.Rectangle.Y + obstacle.Rectangle.Height - (Rectangle.Y - Position.Y);
                    }
                }

                if (minVertical + minHorizontal < proximity)
                {
                    proximity = minVertical + minHorizontal;
                    if (playerRect.Bottom > obstacleRect.Bottom)
                    {
                        Obstacle.Depth = 0.59f;
                    }
                    else
                    {
                        Obstacle.Depth = 0.45f;
                    }
                }
            }
        }

     
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(baseTexture, new Vector2((float)(Rectangle.Center.X), (float)Rectangle.Bottom), null, Color.White, 0f, new Vector2(baseTexture.Width / 2, baseTexture.Height / 2), Scale * 1.5f, SpriteEffects.None, 0.51f);
            playerAnimation.Draw(spriteBatch);

            if (Boosting)
            {
                furyAnimation.Draw(spriteBatch, 0.499f);
                angryAnimation.Draw(spriteBatch, 0.498f);
            }

            spriteBatch.Draw(arrowTexture, Position + new Vector2(playerAnimation.FrameWidth * Scale / 2, playerAnimation.FrameHeight * Scale / 2), null, Color.White, (float)angle + (float)(Math.PI / 2), new Vector2(10,40), 3f, SpriteEffects.None, 0.49f);
        }

        public void Initialize()
        {
            playerAnimation.Initialize(Texture, Position, 98, 73, 8, speed, Color.White, 1.5f, true);
            Scale = playerAnimation.scale;
            furyAnimation.Initialize(furyTexture, Position + new Vector2(playerAnimation.FrameWidth * Scale / 2 - 20 * Scale * 2f, playerAnimation.FrameHeight * Scale * 6f / 7f - 16 * Scale * 2f), 
                40, 32, 3, FuryAnimDuration, Color.White, Scale * 2f, true);
            angryAnimation.Initialize(angryTexture, Position + new Vector2(playerAnimation.FrameWidth * Scale * 3f / 5f - 15 * Scale, playerAnimation.FrameHeight * Scale / 4 - 10 * Scale),
                20, 20, 3, FuryAnimDuration, Color.White, Scale, true);
        }
        public void Update(GameTime gameTime)
        {
            int srcY=0;
            if(angle>=Math.PI/4 && angle<=(Math.PI*3)/4)
            {
                srcY = playerAnimation.FrameHeight;
            }
            else if(angle >= (Math.PI * 3) / 4 || angle <= -(Math.PI * 3) / 4)
            {
                srcY = playerAnimation.FrameHeight * 3;
            }
            else if (angle >= -(Math.PI * 3) / 4 && angle <= -(Math.PI) / 4)
            {
                srcY = 0;
            }
            else if (angle >= -(Math.PI) / 4 || angle <= Math.PI / 4)
            {
                srcY = playerAnimation.FrameHeight * 2;
            }

            if (!Boosting)
            {
                srcY += playerAnimation.FrameHeight * 4;
            }

            playerAnimation.Update(gameTime, Position, speed, srcY);
            furyAnimation.Update(gameTime, Position + new Vector2(playerAnimation.FrameWidth * Scale / 2 - 20 * Scale * 2f, playerAnimation.FrameHeight * Scale * 6f / 7f - 16 * Scale * 2f), 
                FuryAnimDuration, 0);
            angryAnimation.Update(gameTime, Position + new Vector2(playerAnimation.FrameWidth * Scale * 3f / 5f - 15 * Scale, playerAnimation.FrameHeight * Scale / 4 - 10 * Scale), 
                AngryAnimDuration, 0);
        }

        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)(Position.X + (float)playerAnimation.FrameWidth * Scale / 6f), (int)(Position.Y + (float)playerAnimation.FrameHeight * Scale / 6f), (int)((float)playerAnimation.FrameWidth * Scale/1.5), (int)((float)playerAnimation.FrameHeight * Scale/1.5));
            }
        }
    }
}
