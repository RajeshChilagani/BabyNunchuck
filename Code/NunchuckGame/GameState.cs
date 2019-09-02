using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace NunchuckGame
{
    class GameState
    {
        public int Score;
        public SpriteFont font;
        private List<Pickup> Enemies;
        public bool IsGameOver;
        private Random random;
        double TimeToSpawn;
        float Difficulty = 0.1f; // Higher means more difficult
        int EnemiesKilled = 0;

        float MaxComboTime = 0.5f;
        float ComboTimer = 0f;
        int ComboCount = 0;
        float pikcups_Speed=300;

        public List<Sprite> Obstacles;

        //sounds
        List<SoundEffect> allSounds;

        public void InitSounds(List<SoundEffect> allSounds)
        {
            this.allSounds = allSounds;
        }
        public GameState(Rectangle screenBounds)
        {
            Obstacles = new List<Sprite>();
            float obstaclesScale = 2f;
            Sprite obstacle1 = new Obstacle();
            Sprite obstacle2 = new Obstacle();
            Sprite obstacle3 = new Obstacle();
            Sprite obstacle4 = new Obstacle();
            Sprite obstacle5 = new Obstacle();
            Sprite obstacle6 = new Obstacle();
            Sprite obstacle7 = new Obstacle();
            obstacle1.Position = new Vector2(screenBounds.Width / 7, screenBounds.Height / 5);
            obstacle2.Position = new Vector2(screenBounds.Width / 7, screenBounds.Height / 5 + obstaclesScale * 32);
            obstacle3.Position = new Vector2(screenBounds.Width / 7 + obstaclesScale * 64, screenBounds.Height / 5);
            obstacle4.Position = new Vector2(screenBounds.Width * 6 / 7 - obstaclesScale * 64, screenBounds.Height * 3 / 4 - obstaclesScale * 96);
            obstacle5.Position = new Vector2(screenBounds.Width * 6 / 7 - obstaclesScale * 128, screenBounds.Height * 3 / 4 - obstaclesScale * 64);
            obstacle6.Position = new Vector2(screenBounds.Width * 6 / 7 - obstaclesScale * 64, screenBounds.Height * 3 / 4 - obstaclesScale * 64);
            obstacle7.Position = new Vector2(screenBounds.Width / 2 - obstaclesScale * 32, (obstacle1.Position.Y + obstacle6.Position.Y) / 2);

            Obstacles.Add(obstacle1);
            Obstacles.Add(obstacle2);
            Obstacles.Add(obstacle3);
            Obstacles.Add(obstacle4);
            Obstacles.Add(obstacle5);
            Obstacles.Add(obstacle6);
            Obstacles.Add(obstacle7);

            foreach (Sprite obstacle in Obstacles)
            {
                obstacle.Scale = obstaclesScale;
            }

            random = new Random();
            TimeToSpawn = 10f;
            Score = 0;
            Enemies = new List<Pickup>();
            IsGameOver = false;
        }

        public void SetObstaclesTexture(Texture2D texture)
        {
            foreach (Sprite obstacle in Obstacles)
            {
                obstacle.Initialize(texture);
            }
        }

        public void HandleEnemyDestroyed(Pickup pickup)
        {
            ComboTimer = MaxComboTime;
            ComboCount++;
            switch(ComboCount)
            {
                case 1:
                    break;
                case 2:
                   
                    allSounds[2].Play(volume:0.4f,pitch:0.2f,pan:0f);
                    break;
                case 3:

                    allSounds[2].Play(volume: 0.6f, pitch: 0.2f, pan: 0f);
                    break;
                case 4:

                    allSounds[2].Play(volume: 0.8f, pitch: 0.2f, pan: 0f);
                    break;
                default:
                    allSounds[2].Play(volume: 1f, pitch: 0.2f, pan: 0f);
                    break;
            }
            Score += 1 << (ComboCount - 1);
        }

        public void Update(GameTime gameTime, Rectangle playArea, ref MainPlayer player)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            ComboTimer -= (float)deltaTime;
            if (ComboTimer <= 0)
            {
                ComboCount = 0;
            }

            Vector2 playerCenter = new Vector2(player.Position.X + player.Rectangle.Width / 2, player.Position.Y + player.Rectangle.Height / 2);

            TimeToSpawn -= deltaTime;
            if (TimeToSpawn <= 0 && Enemies.Count < 7)
            {
                float DifficultyModifier = 1f / (((float)EnemiesKilled * Difficulty) + 1f);
                TimeToSpawn = (float)random.Next(875, 4376) * DifficultyModifier / 1000;

                Pickup pickup;
                bool isColliding = false;

                int spawnAttempts = 0;
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

                    spawnAttempts++;
                    if (spawnAttempts >= 10)
                        break;

                    foreach (Pickup other in Enemies)
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

                Enemies.Add(pickup);
            }

            // Check enemy collisions. This is done the naive O(n^2) way. If given more time, something like
            // an axis-aligned bounding box tree could be implemented.
            int count = 0;
            while (count < Enemies.Count)
            {
                // Delete enemies who are dead and have had their animation finish
                if (Enemies[count].NeedsDeletion())
                {
                    Enemies.RemoveAt(count);
                    continue;
                }

                // Update enemies
                Enemies[count].Update(deltaTime, playerCenter);
                Enemies[count].Update(gameTime);

                // Remove the object if it has left the screen
                if (Enemies[count].IsOutOfBounds(playArea))
                {
                    Enemies.RemoveAt(count);
                    continue;
                }

                // Make enemies somewhat bounce off one another
                for (int i = 0; i < Enemies.Count; i++)
                {
                    if (Enemies[count].IsColliding(Enemies[i]))
                    {
                        Pickup temp1 = Enemies[count];
                        Pickup temp2 = Enemies[i];

                        Pickup.Collide(ref temp1, ref temp2);

                        Enemies[count] = temp1;
                        Enemies[i] = temp2;
                    }
                }

                // Kill the enemy or the player if the two collide
                if (player.IsColliding(Enemies[count]))
                {
                    if (player.Boosting)
                    {
                        HandleEnemyDestroyed(Enemies[count]);
                        Enemies[count].Kill();
                        EnemiesKilled++;

                        player.BoostMeter += 0.2f;
                        if (player.BoostMeter >= 1.2f)
                            player.BoostMeter = 1.2f;

                        player.allsounds[1].Play(volume:0.3f,pitch:0f,pan:0f);
                    }
                    else
                    {
                        IsGameOver = true;
                    }
                }

                // Kill the enemies if they hit any obstacles
                //for (int i = 0; i < Obstacles.Count; i++)
                //{
                //    if (Enemies[count].IsColliding(Obstacles[i]))
                //    {
                //        Enemies[count].Kill();
                //        break;
                //    }
                //}

                count++;
            }
        }

        public void SetFont(SpriteFont spriteFont)
        {
            font = spriteFont;
        }

        public void Draw(SpriteBatch spriteBatch, ref MainPlayer player, GameTime gameTime)
        {
            foreach (Sprite obstacle in Obstacles)
            {
                obstacle.Draw(spriteBatch);
                Obstacle.Depth -= 0.00001f;
            }

            foreach (Pickup pickup in Enemies)
            {
                pickup.Draw(spriteBatch);
            }

            string score = "Score: " + Score.ToString();
            spriteBatch.DrawString(font, score, new Vector2(10, 10), Color.Black, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);

            if (ComboCount > 1) {
                
                float shakeCount = ComboTimer; 
                string comboUI = "x" + ComboCount.ToString() + " COMBO!";
                Vector2 comboPos = new Vector2(20, -60f);
                float shake = (float)((int)gameTime.TotalGameTime.TotalMilliseconds % 20);
                
                if (shake < 10)
                {
                    shake = 10 - shake % 10;
                }

                comboPos.Y += shake;

                spriteBatch.DrawString(font, comboUI, player.Position + comboPos, Color.Black, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            }


            
        }
        public void RestartGame()
        {
            IsGameOver = false;
            Enemies.Clear();
            Score = 0;
            ComboTimer = 0f;
            ComboCount = 0;
            EnemiesKilled = 0;
        }
    }
}
