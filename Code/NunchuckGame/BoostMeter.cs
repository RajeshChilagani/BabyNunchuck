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
        private float CurrentBoost;
        private float MaxBoost;
        private float MaxSize;

        public BoostMeter()
        {

        }

        public void Initialize(Texture2D containerTexture, Texture2D barTexture, float scale, Vector2 pos, ref MainPlayer player)
        {
            MaxBoost = player.BoostMeter;
            CurrentBoost = MaxBoost;
            base.Initialize(containerTexture);
            BarTexture = barTexture;
            Scale = scale;
            Position = pos;

            MaxSize = Scale * barTexture.Height;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the boost bar
            Vector2 BoostBarPos = Position;
            float ScaleY = Scale * CurrentBoost / MaxBoost;
            BoostBarPos.X += (int)(3f * Scale);
            BoostBarPos.Y += (int)(12f * Scale) + (MaxSize - MaxSize * ScaleY / Scale);
            spriteBatch.Draw(BarTexture, BoostBarPos, null, Color.White, 0f, Vector2.Zero, new Vector2(Scale, ScaleY), SpriteEffects.None, 0f);

            // Draw the boost container
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }

        public void SetCurrentBoost(float boost)
        {
            CurrentBoost = boost;
        }
    }
}
