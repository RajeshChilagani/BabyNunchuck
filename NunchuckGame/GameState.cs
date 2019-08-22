using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NunchuckGame
{
    class GameState
    {
        private int Score;
        private SpriteFont font;
        private List<Pickup> ActivePickups;
        private bool IsGameOver;

        public GameState()
        {
            Score = 0;
            ActivePickups = new List<Pickup>();
            IsGameOver = false;
        }

        public void AddActivePickup(Pickup pickup)
        {
            Score += pickup.GetScoreChange();
            if (pickup.GetDuration() > 0)
                ActivePickups.Add(pickup);
            else if (pickup.GetEndsGame())
                IsGameOver = true;
        }

        public void Update(GameTime gameTime, ref Player player)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;
            player.SetSpeed(1f);

            int count = 0;
            while (count < ActivePickups.Count)
            {
                // Remove pickups whose effects should no longer be active
                ActivePickups[count].UpdateDuration(gameTime.ElapsedGameTime.TotalSeconds);
                if (ActivePickups[count].GetRemainingDuration() <= 0)
                {
                    ActivePickups.RemoveAt(count);
                    continue;
                }

                // Update the player from the effects of the pickup
                if (ActivePickups[count].GetAffectsPlayer())
                    ActivePickups[count].Update(ref player);

                count++;
            }
        }

        public void SetFont(SpriteFont spriteFont)
        {
            font = spriteFont;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            string score = "Score: " + Score.ToString();
            spriteBatch.DrawString(font, score, new Vector2(10, 10), Color.Black);

            if (IsGameOver)
            {
                spriteBatch.DrawString(font, "Game Over", new Vector2(10, 40), Color.Red);
            }
        }
    }
}
