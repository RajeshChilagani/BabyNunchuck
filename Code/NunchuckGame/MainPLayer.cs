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
        public float BoostMeter = 0.5f;
        public bool Boosting = false;
        public double angle = 0f;

        int speed = 30;
        Animation playerAnimation = new Animation();

        public Texture2D arrowTexture { get; set; }

       public MainPlayer( Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
        }
        public void LoadTexture(Texture2D playerTexture, Texture2D arrowTexture)
        {
            Texture = playerTexture;
            this.arrowTexture = arrowTexture;
        }
        public void controller(GameTime gameTime, Viewport ScreenSize)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            angle = Math.Atan2(Velocity.Y, Velocity.X);
            float magnitude ;
            

           
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                angle += (150 * Math.PI) / 180f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {

                angle -= (150 * Math.PI) / 180f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && BoostMeter>0)
            {
                magnitude = 450f;
                BoostMeter -= (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;
                Boosting = true;
                speed = 10;
            }
            else
            {
                Boosting = false;
                magnitude = 200f;
                speed = 60;
                if(BoostMeter<0.5f && Keyboard.GetState().IsKeyUp(Keys.Space))
                    BoostMeter += (float)gameTime.ElapsedGameTime.TotalSeconds/6;
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
            if (Position.Y < 25f)
            {
                Position.Y = 25f;
            }
            if ((Position.Y + Rectangle.Height) > ScreenSize.Height - 25)
            {
                Position.Y = ScreenSize.Height - 25 - Rectangle.Height;
            }
            Console.WriteLine();

            
        }

     
        public override void Draw(SpriteBatch spriteBatch)
        {
            playerAnimation.Draw(spriteBatch);
            spriteBatch.Draw(arrowTexture, Position + new Vector2(Rectangle.Width/2,Rectangle.Height/2), null, Color.White, (float)(angle - 45f), Vector2.Zero, 0.3f, SpriteEffects.None, 0f);
        }

        public void Initialize()
        {
            playerAnimation.Initialize(Texture, Position, 65, 37, 8, speed, Color.White, 2f, true);

        }
        public void Update(GameTime gameTime)
        {
            int srcY=0;
            Console.WriteLine(angle);
            if(angle>=Math.PI/4 && angle<=(Math.PI*3)/4)
            {
                srcY = 0;
            }
            else if(angle >= (Math.PI * 3) / 4 || angle <= -(Math.PI * 3) / 4)
            {
                srcY = 74;
            }
            else if (angle >= -(Math.PI * 3) / 4 && angle <= -(Math.PI) / 4)
            {
                srcY = 37;
            }
            else if (angle >= -(Math.PI) / 4 || angle <= Math.PI / 4)
            {
                srcY = 111;
            }
            playerAnimation.Update(gameTime, Position, speed,srcY);
        }

        public override Rectangle Rectangle
        {
            get
            {
                Console.WriteLine(Position.ToString());
                return new Rectangle((int)Position.X + (int)(playerAnimation.FrameWidth * Scale) / 4, (int)Position.Y + (int)(playerAnimation.FrameHeight * Scale) / 4, (int)(playerAnimation.FrameWidth * Scale)/2, (int)(playerAnimation.FrameHeight * Scale)/2);
            }
        }
    }
}
