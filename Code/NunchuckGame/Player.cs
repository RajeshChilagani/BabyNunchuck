using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NunchuckGame
{
    class Player
    {
        // Animation representing the player
        private Texture2D PlayerTexture;

        // Position of the Player relative to the upper left side of the screen
        private Vector2 Position;

        // Default velocity
        private Vector2 DefaultVelocity;

        // Velocity of the Player (pixels / second)
        private Vector2 Velocity;

        // The scale of the sprite
        private float Scale;

        public float Rotation;

        public Player()
        {
            DefaultVelocity = Vector2.Multiply(Vector2.Zero, 15f);
        }

        public void Initialize(Texture2D texture, Vector2 position, float scale)
        {
            PlayerTexture = texture;

            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;
            Rotation = 0f;
            Velocity = DefaultVelocity;
            Scale = scale;
        }

        public void Update(GameTime gameTime)
        {
            Rotation = (Rotation + (float)gameTime.ElapsedGameTime.TotalSeconds * 0.05f) % 360;
            Position += Vector2.Multiply(Velocity, (float)(gameTime.ElapsedGameTime.TotalSeconds));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlayerTexture, Position, null, Color.White, Rotation, Vector2.Zero, Scale, 
                SpriteEffects.None, 0f);
        }

        // Get the width of the player
        public int Width
        {
            get { return PlayerTexture.Width; }
        }

        // Get the height of the player ship
        public int Height
        {
            get { return PlayerTexture.Height; }
        }

        public void SetSpeed(float speed)
        {
            Velocity = Vector2.Multiply(DefaultVelocity, speed);
        }

        public void SetDirection(Vector2 direction)
        {
            direction.Normalize();
            Velocity = Velocity.Length() * direction;
        }

        public Vector2 GetVelocity()
        {
            return Velocity;
        }
    }
}
