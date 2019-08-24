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
        protected float Scale;
        public Color color = Color.White;
        public bool isTouching = false;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)(Texture.Width * Scale), (int)(Texture.Height * Scale));
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

            return this.Rectangle.Right + this.Velocity.X > sprite.Rectangle.Left &&
                    this.Rectangle.Left < sprite.Rectangle.Left &&
                    this.Rectangle.Bottom > sprite.Rectangle.Top &&
                    this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingRight(Sprite sprite)
        {

            return this.Rectangle.Left + this.Velocity.X < sprite.Rectangle.Right &&
                    this.Rectangle.Right > sprite.Rectangle.Right &&
                    this.Rectangle.Bottom > sprite.Rectangle.Top &&
                    this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingTop(Sprite sprite)
        {

            return this.Rectangle.Bottom + this.Velocity.Y > sprite.Rectangle.Top &&
                    this.Rectangle.Top < sprite.Rectangle.Top &&
                    this.Rectangle.Right > sprite.Rectangle.Left &&
                    this.Rectangle.Left < sprite.Rectangle.Right;
        }

        protected bool IsTouchingBottom(Sprite sprite)
        {

            return this.Rectangle.Top + this.Velocity.Y < sprite.Rectangle.Bottom &&
                    this.Rectangle.Bottom > sprite.Rectangle.Bottom &&
                    this.Rectangle.Right > sprite.Rectangle.Left &&
                    this.Rectangle.Left < sprite.Rectangle.Right;
        }

        public bool IsColliding(Sprite sprite)
        {
            return IsTouchingLeft(sprite) || IsTouchingRight(sprite) || IsTouchingTop(sprite) || IsTouchingBottom(sprite);
        }
        #endregion
    }
}
