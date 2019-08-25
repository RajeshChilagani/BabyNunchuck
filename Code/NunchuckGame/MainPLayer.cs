﻿using System;
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
       public MainPlayer( Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
        }
        public void LoadTexture(Texture2D texture)
        {
            Initialize(texture);
        }
        public void controller(GameTime gameTime, Viewport ScreenSize)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            double angle = Math.Atan2(Velocity.Y, Velocity.X);
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
            }
            else
            {
                Boosting = false;
                magnitude = 100f;
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
    }
}