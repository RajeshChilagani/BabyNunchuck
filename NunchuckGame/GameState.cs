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

                // Find whether the object will be spawned from above (0), the right (1), below (2), or the left (3)
                int start = random.Next(0, 4);
                int x, y;
                switch(start)
                {
                    case 0:
                        y = -(Pickup.PickupHeight());
                        x = random.Next(-(Pickup.PickupWidth()), playArea.Width);
                        break;
                    case 1:
                        x = playArea.Width;
                        y = random.Next(-(Pickup.PickupHeight()), playArea.Height);
                        break;
                    case 2:
                        y = playArea.Height;
                        x = random.Next(-(Pickup.PickupWidth()), playArea.Width);
                        break;
                    case 3:
                        x = -(Pickup.PickupWidth());
                        y = random.Next(-(Pickup.PickupHeight()), playArea.Height);
                        break;
                    default:
                        x = y = 0;
                        break;
                }

                Vector2 position = new Vector2(x, y);
                Vector2 direction = new Vector2((playArea.Width / 2) - (x + Pickup.PickupWidth() / 2), (playArea.Height / 2) - (y + Pickup.PickupHeight() / 2));
                SmallPointsPickup pickup = new SmallPointsPickup(position, direction, 100f);

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
