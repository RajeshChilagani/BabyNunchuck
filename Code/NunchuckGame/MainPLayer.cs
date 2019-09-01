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
    class MainPlayer : Sprite
    {
        public float BoostMeter = 0.6f;
        public bool Boosting = false;
        public double angle = 0f;

        int speed = 50;
        Animation playerAnimation = new Animation();

        public Texture2D arrowTexture { get; set; }
        public Texture2D baseTexture { get; set; }

        public MainPlayer( Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
        }
        public void LoadTexture(Texture2D playerTexture, Texture2D arrowTexture, Texture2D baseTexture)
        {
            Texture = playerTexture;
            this.arrowTexture = arrowTexture;
            this.baseTexture = baseTexture;
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
                BoostMeter -= (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;
                Boosting = true;
                speed = 5;
            }
            else
            {
                Boosting = false;
                speed = 50;
                magnitude = 250f;
                if(BoostMeter<0.6f && (Keyboard.GetState().IsKeyUp(Keys.Space) || Keyboard.GetState().IsKeyUp(Keys.Up)))
                    BoostMeter += (float)gameTime.ElapsedGameTime.TotalSeconds/4;
            }
            
            Velocity.X = (float)Math.Cos(angle);    
            Velocity.Y = (float)Math.Sin(angle);

            Velocity = Vector2.Multiply(Velocity, magnitude);
            
            Position += Vector2.Multiply(Velocity, (float)(gameTime.ElapsedGameTime.TotalSeconds));
            if(Position.X < 25f)
            {
                Position.X = 25f;
            }
            if((Position.X + Rectangle.Width) > ScreenSize.Width-25)
            {
                Position.X  = ScreenSize.Width - 25-Rectangle.Width;
            }
            if (Position.Y < 100f)
            {
                Position.Y = 100f;
            }
            if ((Position.Y + Rectangle.Height) > ScreenSize.Height - 180)
            {
                Position.Y = ScreenSize.Height - 180 - Rectangle.Height;
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
            spriteBatch.Draw(baseTexture, new Vector2((float)(Rectangle.Center.X - (playerAnimation.FrameWidth * Scale * 0.05)), (float)Rectangle.Bottom), null, Color.White, 0f, new Vector2(baseTexture.Width / 2, baseTexture.Height / 2), Scale * 1.5f, SpriteEffects.None, 0.51f);
            playerAnimation.Draw(spriteBatch);
            spriteBatch.Draw(arrowTexture, Position + new Vector2(playerAnimation.FrameWidth * Scale / 2, playerAnimation.FrameHeight * Scale / 2), null, Color.White, (float)angle + (float)(Math.PI / 2), new Vector2(10,40), 3f, SpriteEffects.None, 0.49f);
        }

        public void Initialize()
        {
            playerAnimation.Initialize(Texture, Position, 137, 78, 8, speed, Color.White, 1.5f, true);
            Scale = playerAnimation.scale;
        }
        public void Update(GameTime gameTime)
        {
            int srcY=0;
            if(angle>=Math.PI/4 && angle<=(Math.PI*3)/4)
            {
                srcY = 0;
            }
            else if(angle >= (Math.PI * 3) / 4 || angle <= -(Math.PI * 3) / 4)
            {
                srcY = playerAnimation.FrameHeight*2;
            }
            else if (angle >= -(Math.PI * 3) / 4 && angle <= -(Math.PI) / 4)
            {
                srcY = playerAnimation.FrameHeight*1;
            }
            else if (angle >= -(Math.PI) / 4 || angle <= Math.PI / 4)
            {
                srcY = playerAnimation.FrameHeight*3;
            }
            playerAnimation.Update(gameTime, Position, speed,srcY);
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
