using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace NunchuckGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class NunchuckGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Audio
        List<SoundEffect> allSounds;
        Song gameMusic;

        GameState gameState;
        MainPlayer mainChar;
        BoostMeter boostMeter;
        //All Enivronment 
        Environment Wood;
        Environment topBorder;
        Environment topFence;
        Environment botBorder;
        Environment botFence;
        Environment leftBorder;
        Environment leftFence;
        Environment rightBorder;
        Environment rightFence;
        Environment DecorObj;
        Environment DecorObj1;
        Environment leftFenceAdj;
        Environment leftFenceAdj2;
        Environment rightFenceAdj;
        Environment rightFenceAdj2;


        Texture2D arrowTexture;
        Texture2D baseTexture;
        Texture2D gameOverTexture;

        
     

        public NunchuckGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            
            
            gameState = new GameState(GraphicsDevice.Viewport.Bounds);
            boostMeter = new BoostMeter();
            Wood = new Environment();
            topBorder = new Environment();
            topFence = new Environment();
            botBorder = new Environment();
            botFence = new Environment();
            leftBorder = new Environment();
            leftFence = new Environment();
            rightBorder = new Environment();
            rightFence = new Environment();
            DecorObj = new Environment();
            DecorObj1 = new Environment();
            leftFenceAdj = new Environment();
            leftFenceAdj2 = new Environment();
            rightFenceAdj = new Environment();
            rightFenceAdj2 = new Environment();


            //gameState.AddActivePickup(new SpeedPickup());

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Audio
            allSounds = new List<SoundEffect>();
            allSounds.Add(Content.Load<SoundEffect>("NunchuckSpin_Loopable"));
            allSounds.Add(Content.Load<SoundEffect>("Hit"));
            allSounds.Add(Content.Load<SoundEffect>("Hiyuh_louder"));
            this.gameMusic = Content.Load<Song>("GameSong");
            MediaPlayer.Play(gameMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
            Texture2D mainTexture = this.Content.Load<Texture2D>("All Attacks_Fixed");
            arrowTexture = this.Content.Load<Texture2D>("arrow-pointer");
            baseTexture = this.Content.Load<Texture2D>("base");
            Texture2D furyTexture = Content.Load<Texture2D>("Fury-Indicator3");
            Texture2D angryTexture = Content.Load<Texture2D>("Anger-Brackets");

            gameOverTexture = this.Content.Load<Texture2D>("gameover");

            // Load the player resources
            mainChar = new MainPlayer(new Vector2(GraphicsDevice.Viewport.Width / 2 - mainTexture.Width / 2, GraphicsDevice.Viewport.Height / 2-mainTexture.Height/2), new Vector2(50),allSounds);

            //arrow = new MainPlayer(new Vector2(GraphicsDevice.Viewport.Width / 2 - mainTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 - mainTexture.Height / 2), new Vector2(50));

            Pickup.PickupTexture = Content.Load<Texture2D>("NinjaStar");
            Pickup.DeathTexture = Content.Load<Texture2D>("EnemyDeath");
            Pickup.PickupScale = 3f;
            mainChar.LoadTexture(mainTexture, arrowTexture, baseTexture, furyTexture, angryTexture);
            mainChar.Initialize();

            gameState.InitSounds(allSounds);
            gameState.SetObstaclesTexture(Content.Load<Texture2D>("Block1"));
            gameState.SetObstaclesTexture(Content.Load<Texture2D>("Block1_transparent"));

            // Set the game font
            gameState.SetFont(Content.Load<SpriteFont>("font"));

            float boostScale = 2.5f;
            Texture2D boostContainerTexture = Content.Load<Texture2D>("FuryBar2");
            boostMeter.Initialize(boostContainerTexture, Content.Load<Texture2D>("FuryBar3_Filler"), Content.Load<Texture2D>("FuryBar_Full"), boostScale, ref mainChar);

            //Environment

            Texture2D TopWall = Content.Load<Texture2D>("TopWall");
            Texture2D Fence = Content.Load<Texture2D>("Fence");
            Texture2D BotWall = Content.Load<Texture2D>("BotWall");
            Texture2D SideWall = Content.Load<Texture2D>("SideWall");
            Texture2D SideFence = Content.Load<Texture2D>("FenceSide");
            Texture2D FenceEnd = Content.Load<Texture2D>("FenceSideN");
            Texture2D Decor = Content.Load<Texture2D>("Decor");
            Wood.Intiliaze(Content.Load<Texture2D>("Background"),GraphicsDevice.Viewport);
            Wood.Blocks();
            //top
            topBorder.Intiliaze(TopWall, GraphicsDevice.Viewport);
            topBorder.walls(0,0,3f,true,GraphicsDevice.Viewport.Width);
            topFence.Intiliaze(Fence, GraphicsDevice.Viewport);
            topFence.fences(SideWall.Width*2, TopWall.Height*3- Fence.Height*2+8,2f,true,GraphicsDevice.Viewport.Width-SideWall.Width*2);
            //bottom
            botBorder.Intiliaze(BotWall, GraphicsDevice.Viewport);
            botBorder.walls(SideWall.Width*2,GraphicsDevice.Viewport.Height- BotWall.Height,1f,true, GraphicsDevice.Viewport.Width - SideWall.Width * 2);
            botFence.Intiliaze(Fence, GraphicsDevice.Viewport);
            botFence.fences(SideWall.Width*2, GraphicsDevice.Viewport.Height - BotWall.Height-Fence.Height*2,2f,true, GraphicsDevice.Viewport.Width - SideWall.Width * 2);
            //leftSide
            leftBorder.Intiliaze(SideWall, GraphicsDevice.Viewport);
            leftBorder.walls(0, (int)(TopWall.Height*3f-Fence.Height/2), 2f, false,GraphicsDevice.Viewport.Height);
            leftFence.Intiliaze(SideFence, GraphicsDevice.Viewport);
            leftFence.fences(SideWall.Width*2-(SideFence.Width), (int)(TopWall.Height * 3f), 2f,false, GraphicsDevice.Viewport.Height - BotWall.Height - Fence.Height);
            //rightSide
            rightBorder.Intiliaze(SideWall, GraphicsDevice.Viewport);
            rightBorder.walls(GraphicsDevice.Viewport.Width-TopWall.Width*2, (int)(TopWall.Height * 3f-Fence.Height/2), 2f, false, GraphicsDevice.Viewport.Height);
            rightFence.Intiliaze(SideFence, GraphicsDevice.Viewport);
            rightFence.fences(GraphicsDevice.Viewport.Width - TopWall.Width * 2 - SideFence.Width, (int)(TopWall.Height * 3f), 2f, false, GraphicsDevice.Viewport.Height - BotWall.Height-Fence.Height);
            //left fence end
            DecorObj.Intiliaze(Decor,GraphicsDevice.Viewport);
            DecorObj1.Intiliaze(Decor, GraphicsDevice.Viewport);
            leftFenceAdj.Intiliaze(FenceEnd, GraphicsDevice.Viewport);
            leftFenceAdj2.Intiliaze(FenceEnd, GraphicsDevice.Viewport);
            rightFenceAdj.Intiliaze(FenceEnd, GraphicsDevice.Viewport);
            rightFenceAdj2.Intiliaze(FenceEnd, GraphicsDevice.Viewport);


            

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Handling input should come first. Player.Rotation and Player.SetDirection() should be used.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(Keyboard.GetState().IsKeyDown(Keys.R) && gameState.IsGameOver==true)
            {
                mainChar.BoostMeter = 0.6f;
                gameState.RestartGame();
                

            }
            // Game state is updated second and may override or ignore user input
            if(!gameState.IsGameOver)
            {
                gameState.Update(gameTime, GraphicsDevice.Viewport.TitleSafeArea, ref mainChar);
                mainChar.controller(gameTime, GraphicsDevice.Viewport, ref gameState);
                boostMeter.SetCurrentBoost(mainChar.BoostMeter);
                mainChar.Update(gameTime);
               
            }
          
            // The player gets moved
           
          
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.White);

            // Start drawing
            spriteBatch.Begin(SpriteSortMode.BackToFront);





            Wood.draw(spriteBatch, 1f);
            topBorder.draw(spriteBatch,0.99f);
            botBorder.draw(spriteBatch,0.49f);
            leftBorder.draw(spriteBatch, 0.45f,"hori");
            rightBorder.draw(spriteBatch, 0.45f);

            // Draw the Player
            mainChar.Draw(spriteBatch);

            topFence.draw(spriteBatch, 0.98f);
            botFence.draw(spriteBatch,0.48f);

            DecorObj.draw(spriteBatch,2.7f,(int)(GraphicsDevice.Viewport.Width/5),0,0.5f);
            DecorObj1.draw(spriteBatch, 2.7f, (int)(GraphicsDevice.Viewport.Width - GraphicsDevice.Viewport.Width / 5 - DecorObj.Background.Width * 2.7), 0,0.5f);
            leftFence.draw(spriteBatch, 0.42f);
            rightFence.draw(spriteBatch, 0.42f);

            leftFenceAdj.draw(spriteBatch, 2f, (int)(leftBorder.Background.Width * 2 + leftFence.Background.Width - leftFenceAdj.Background.Width * 2 -5), (int)(topBorder.Background.Height * 2 + leftFence.Background.Height - leftFenceAdj.Background.Height * 2 + 15),0.43f);
            leftFenceAdj.draw(spriteBatch, 2f, (int)(leftBorder.Background.Width * 2 + leftFence.Background.Width - leftFenceAdj.Background.Width * 2-5), (int)(GraphicsDevice.Viewport.Height-botBorder.Background.Height-botFence.Background.Height*2), 0.41f);
            leftFenceAdj.draw(spriteBatch, 2f, (int)(GraphicsDevice.Viewport.Width-rightBorder.Background.Width*2- rightFence.Background.Width-5), (int)(topBorder.Background.Height * 2 + leftFence.Background.Height - leftFenceAdj.Background.Height * 2 + 15), 0.43f);
            leftFenceAdj.draw(spriteBatch, 2f, (int)(GraphicsDevice.Viewport.Width - rightBorder.Background.Width * 2-rightFence.Background.Width-5), (int)(GraphicsDevice.Viewport.Height - botBorder.Background.Height - botFence.Background.Height * 2), 0.41f);
           
           
            if (!gameState.IsGameOver)
            {  
               gameState.Draw(spriteBatch,ref mainChar,gameTime);
            }
            else
            {
                //spriteBatch.DrawString(gameState.font, "Game Over", new Vector2(100, 400), Color.Red);
                float screenWidth = GraphicsDevice.Viewport.Width;
                float screenHeight = GraphicsDevice.Viewport.Height;
                //spriteBatch.DrawString(gameState.font, "Game Over", new Vector2(screenWidth, screenWidth), Color.Red, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
                spriteBatch.Draw(gameOverTexture, new Vector2(screenWidth/2, screenHeight/2), null, Color.White, 0f, new Vector2(gameOverTexture.Width/2, gameOverTexture.Height / 2), 4f, SpriteEffects.None, 0f);


                string score = "Score: " + gameState.Score.ToString();
                spriteBatch.DrawString(gameState.font, score, new Vector2(screenWidth/2 - 120, screenHeight/2 + 80), Color.Black, 0f, new Vector2(0,0), 2f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(gameState.font, "Press \"R\" to Restart", new Vector2(screenWidth / 2 - 280, screenHeight / 2 + 160), Color.Black, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
            }

          
          

            boostMeter.Draw(spriteBatch, mainChar.Position);
            // Stop drawing
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
