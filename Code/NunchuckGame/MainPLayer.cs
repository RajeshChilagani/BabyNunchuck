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
            

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                angle += (150f * Math.PI) / 180f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {

                angle -= (150f * Math.PI) / 180f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && BoostMeter>0)
            {
                magnitude = 250f;
                BoostMeter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                Boosting = true;
                speed = 5;
            }
            else
            {
                Boosting = false;
                magnitude = 100f;
                speed = 30;
                if(BoostMeter<0.5f && Keyboard.GetState().IsKeyUp(Keys.Space))
                    BoostMeter += (float)gameTime.ElapsedGameTime.TotalSeconds/3;
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
            playerAnimation.Initialize(Texture, Position, 65, 37, 8, speed, Color.White, 1f, true);

        }
        public void Update(GameTime gameTime)
        {
            playerAnimation.Update(gameTime, Position, speed);
        }

        public override Rectangle Rectangle
        {
            get
            {
                Console.WriteLine(Position.ToString());
                return new Rectangle((int)Position.X, (int)Position.Y, (int)(playerAnimation.FrameWidth * Scale), (int)(playerAnimation.FrameHeight * Scale));
            }
        }
    }
}
