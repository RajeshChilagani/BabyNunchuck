using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NunchuckGame
{
    class BoostMeter : Sprite
    {
        private Texture2D BarTexture;
        private Texture2D FullFuryTexture;
        private float CurrentBoost;
        private float MaxBoost;
        private float MaxSize;

        public BoostMeter()
        {

        }

        public void Initialize(Texture2D containerTexture, Texture2D barTexture, Texture2D fullFuryTexture, float scale, ref MainPlayer player)
        {
            MaxBoost = player.BoostMeter;
            CurrentBoost = MaxBoost;
            base.Initialize(containerTexture);
            BarTexture = barTexture;
            FullFuryTexture = fullFuryTexture;
            Scale = scale;

            MaxSize = Scale * barTexture.Height;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Position = new Vector2(position.X - 40, position.Y);

            // Draw the boost bar
            Vector2 BoostBarPos = Position;

            if (CurrentBoost < MaxBoost)
            {
                
                float ScaleY = Scale * CurrentBoost / MaxBoost;
                BoostBarPos.X += (int)(6f * Scale);
                BoostBarPos.Y += (int)(7f * Scale) + (MaxSize - MaxSize * ScaleY / Scale);
                spriteBatch.Draw(BarTexture, BoostBarPos, null, Color.White, 0f, Vector2.Zero, new Vector2(Scale, ScaleY), SpriteEffects.None, 0.01f);

                // Draw the boost container
                spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);

            }
            else
            {
                spriteBatch.Draw(FullFuryTexture, BoostBarPos, null, Color.White, 0f, Vector2.Zero, new Vector2(Scale, Scale), SpriteEffects.None, 0.01f);
            }
        }

        public void SetCurrentBoost(float boost)
        {
            CurrentBoost = boost;
        }
    }
}
