using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NunchuckGame
{
    class GameState
    {
        public int Score;
        public SpriteFont font;
        private List<Pickup> ActivePickups;
        private List<Pickup> InactivePickups;
        public bool IsGameOver;
        private Random random;
        double TimeToSpawn;
        float pikcups_Speed=300;

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

        public void Update(GameTime gameTime, Rectangle playArea, ref MainPlayer player)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 playerCenter = new Vector2(player.Position.X + player.Rectangle.Width / 2, player.Position.Y + player.Rectangle.Height / 2);

            TimeToSpawn -= deltaTime;
            if (TimeToSpawn <= 0)
            {
                float DifficultyModifier = 1f / (((float)Score * 0.1f) + 1f);
                TimeToSpawn = (float)random.Next(875, 4376) * DifficultyModifier / 1000;

                Pickup pickup;
                bool isColliding = false;

                do
                {
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
                    //Vector2 direction = new Vector2((playArea.Width / 2) - (x + Pickup.PickupWidth() / 2), (playArea.Height / 2) - (y + Pickup.PickupHeight() / 2));
                    Vector2 direction = playerCenter - position;
                    pickup = new FollowPickup(position, direction, pikcups_Speed);

                    foreach (Pickup other in InactivePickups)
                    {
                        isColliding = false;
                        if (pickup.IsColliding(other))
                        {
                            isColliding = true;
                            break;
                        }
                    }
                }
                while (isColliding);

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
                    //ActivePickups[count].Update(ref player);

                count++;
            }

            count = 0;
            while (count < InactivePickups.Count)
            {
                InactivePickups[count].Update(deltaTime, playerCenter);
                InactivePickups[count].Update(deltaTime);

                // Remove the object if it has left the screen
                if (InactivePickups[count].IsOutOfBounds(playArea))
                {
                    InactivePickups.RemoveAt(count);
                    continue;
                }

                for (int i = 0; i < InactivePickups.Count; i++)
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

                if (player.IsColliding(InactivePickups[count]))
                {
                    if (player.Boosting)
                    {
                        AddActivePickup(InactivePickups[count]);
                        InactivePickups.RemoveAt(count);
                        continue;
                    }
                    else
                    {
                        IsGameOver = true;
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
            foreach (Pickup pickup in InactivePickups)
            {
                pickup.Draw(spriteBatch);
            }
            string score = "Score: " + Score.ToString();
            spriteBatch.DrawString(font, score, new Vector2(10, 10), Color.Black, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);


            
        }
        public void RestartGame()
        {
            IsGameOver = false;
            ActivePickups.Clear();
            InactivePickups.Clear();
            Score = 0;
            

        }
    }
}
