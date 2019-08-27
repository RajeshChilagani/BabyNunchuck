using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NunchuckGame
{
    class Sprite
    {
        protected Texture2D Texture;

        public Vector2 Position;
        public Vector2 Velocity;
        protected float Scale=2;
        public Color color = Color.White;
        public bool isTouching = false;

        public virtual Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X + Texture.Width / 4 * (int)Scale, (int)Position.Y + Texture.Height / 4 * (int)Scale, (int)(Texture.Width * Scale)/2, (int)(Texture.Height * Scale)/2);
            }
        }

        public Sprite()
        {
            
        }

        public void Initialize(Texture2D texture)
        {
            Texture = texture;
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites) { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, color);
            //spriteBatch.DrawString(), isTouching.ToString(), new Vector2(10,10), Color.Black);
        }

        #region Collision
        protected bool IsTouchingLeft(Sprite sprite)
        {

            return this.Rectangle.Right > sprite.Rectangle.Left &&
                    this.Rectangle.Left < sprite.Rectangle.Left &&
                    this.Rectangle.Bottom > sprite.Rectangle.Top &&
                    this.Rectangle.Top < sprite.Rectangle.Bottom &&
                    this.Velocity.X > 0;
        }

        protected bool IsTouchingRight(Sprite sprite)
        {

            return this.Rectangle.Left < sprite.Rectangle.Right &&
                    this.Rectangle.Right > sprite.Rectangle.Right &&
                    this.Rectangle.Bottom > sprite.Rectangle.Top &&
                    this.Rectangle.Top < sprite.Rectangle.Bottom &&
                    this.Velocity.X < 0;
        }

        protected bool IsTouchingTop(Sprite sprite)
        {

            return this.Rectangle.Bottom > sprite.Rectangle.Top &&
                    this.Rectangle.Top < sprite.Rectangle.Top &&
                    this.Rectangle.Right > sprite.Rectangle.Left &&
                    this.Rectangle.Left < sprite.Rectangle.Right &&
                    this.Velocity.Y > 0;
        }

        protected bool IsTouchingBottom(Sprite sprite)
        {

            return this.Rectangle.Top < sprite.Rectangle.Bottom &&
                    this.Rectangle.Bottom > sprite.Rectangle.Bottom &&
                    this.Rectangle.Right > sprite.Rectangle.Left &&
                    this.Rectangle.Left < sprite.Rectangle.Right &&
                    this.Velocity.Y < 0;
        }

        public bool IsColliding(Sprite sprite)
        {
            return IsTouchingLeft(sprite) || IsTouchingRight(sprite) || IsTouchingTop(sprite) || IsTouchingBottom(sprite);
        }
        #endregion
    }
}
