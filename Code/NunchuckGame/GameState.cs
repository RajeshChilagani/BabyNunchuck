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
        private List<Pickup> InactivePickups;
        private bool IsGameOver;
        private Random random;
        double TimeToSpawn;

        public GameState()
        {
            random = new Random();
            TimeToSpawn = 0f;
            Score = 0;
            ActivePickups = new List<Pickup>();
            InactivePickups = new List<Pickup>();
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

        public void Update(GameTime gameTime, Rectangle playArea, ref Player player)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;
            player.SetSpeed(1f);

            TimeToSpawn -= deltaTime;
            if (TimeToSpawn <= 0)
            {
                TimeToSpawn = (float)random.Next(500, 2501) / 1000f;

                // Find whether the object will be spawned from above, the right, below, or the left
                int start = random.Next(0, 2 * Pickup.PickupWidth() + 2 * Pickup.PickupHeight() + 1);
                int x, y;
                if (start < Pickup.PickupWidth())
                {
                    y = -(Pickup.PickupHeight());
                    x = random.Next(-(Pickup.PickupWidth()), playArea.Width);
                }
                else if (start < Pickup.PickupWidth() + Pickup.PickupHeight())
                {
                    x = playArea.Width;
                    y = random.Next(-(Pickup.PickupHeight()), playArea.Height);
                }
                else if (start < Pickup.PickupWidth() * 2 + Pickup.PickupHeight())
                {
                    y = playArea.Height;
                    x = random.Next(-(Pickup.PickupWidth()), playArea.Width);
                }
                else
                {
                    x = -(Pickup.PickupWidth());
                    y = random.Next(-(Pickup.PickupHeight()), playArea.Height);
                }

                Vector2 position = new Vector2(x, y);
                Vector2 direction = new Vector2((playArea.Width / 2) - (x + Pickup.PickupWidth() / 2), (playArea.Height / 2) - (y + Pickup.PickupHeight() / 2));
                TempPickup pickup = new TempPickup(position, direction, 100f);

                InactivePickups.Add(pickup);
            }

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

            count = 0;
            while (count < InactivePickups.Count)
            {
                InactivePickups[count].Update(deltaTime);

                // Remove the object if it has left the screen
                if (InactivePickups[count].IsOutOfBounds(playArea))
                {
                    InactivePickups.RemoveAt(count);
                    continue;
                }

                for (int i = count + 1; i < InactivePickups.Count; i++)
                {
                    if (InactivePickups[count].IsColliding(InactivePickups[i]))
                    {
                        Pickup temp1 = InactivePickups[count];
                        Pickup temp2 = InactivePickups[i];

                        Pickup.Collide(ref temp1, ref temp2);

                        InactivePickups[count] = temp1;
                        InactivePickups[i] = temp2;
                    }
                }

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

            foreach(Pickup pickup in InactivePickups)
            {
                pickup.Draw(spriteBatch);
            }

            if (IsGameOver)
            {
                spriteBatch.DrawString(font, "Game Over", new Vector2(10, 40), Color.Red);
            }
        }
    }
}
