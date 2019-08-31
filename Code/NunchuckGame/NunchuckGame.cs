using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NunchuckGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class NunchuckGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        GameState gameState;
        MainPlayer mainChar;
        BoostMeter boostMeter;
        Environment env;
        Environment topBorder;
        Environment topFence;
        Environment botBorder;
        Environment botFence;



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
            
            
            gameState = new GameState();
            boostMeter = new BoostMeter();
            env = new Environment();
            topBorder = new Environment();
            topFence = new Environment();
            botBorder = new Environment();
            botFence = new Environment();


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
            Texture2D mainTexture = this.Content.Load<Texture2D>("All_Attacks");
           // float scale = 0.3f;

            arrowTexture = this.Content.Load<Texture2D>("arrow-pointer");

            baseTexture = this.Content.Load<Texture2D>("base");

            gameOverTexture = this.Content.Load<Texture2D>("gameover");

            // Load the player resources
            mainChar = new MainPlayer(new Vector2(GraphicsDevice.Viewport.Width / 2 - mainTexture.Width / 2, GraphicsDevice.Viewport.Height / 2-mainTexture.Height/2), new Vector2(50));

            //arrow = new MainPlayer(new Vector2(GraphicsDevice.Viewport.Width / 2 - mainTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 - mainTexture.Height / 2), new Vector2(50));

            Pickup.PickupTexture = Content.Load<Texture2D>("Enemy");
            Pickup.PickupScale = 2f;
            mainChar.LoadTexture(mainTexture, arrowTexture, baseTexture);
            mainChar.Initialize();

            gameState.SetObstaclesTexture(Content.Load<Texture2D>("player"));

            // Set the game font
            gameState.SetFont(Content.Load<SpriteFont>("font"));

            float boostScale = 3f;
            Texture2D boostContainerTexture = Content.Load<Texture2D>("BoostContainer");
            boostMeter.Initialize(boostContainerTexture, Content.Load<Texture2D>("BoostBarCropped"), boostScale, 
                new Vector2(GraphicsDevice.Viewport.Width - 20 - (boostContainerTexture.Width * boostScale), 20), ref mainChar);

            //Environment

            Texture2D TopWall = Content.Load<Texture2D>("TopWall");
            Texture2D Fence = Content.Load<Texture2D>("Fence");
            Texture2D BotWall = Content.Load<Texture2D>("BotWall");
            Texture2D SideWall = Content.Load<Texture2D>("SideWall");
            env.Intiliaze(Content.Load<Texture2D>("Background"),GraphicsDevice.Viewport);
            env.Blocks();
            //top
            topBorder.Intiliaze(TopWall, GraphicsDevice.Viewport);
            topBorder.walls(0,0);
            topFence.Intiliaze(Fence, GraphicsDevice.Viewport);
            topFence.fences(0, TopWall.Height*2- Fence.Height*2);
            //bottom
            botBorder.Intiliaze(BotWall, GraphicsDevice.Viewport);
            botBorder.walls(SideWall.Width*2,GraphicsDevice.Viewport.Height- BotWall.Height*2);
            botFence.Intiliaze(Fence, GraphicsDevice.Viewport);
            botFence.fences(SideWall.Width * 2, GraphicsDevice.Viewport.Height - BotWall.Height * 2-Fence.Height*2);
            //leftSide


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
            spriteBatch.Begin();

            // Draw the Player
            env.draw(spriteBatch);
            topBorder.draw(spriteBatch);
            topFence.draw(spriteBatch);
            botBorder.draw(spriteBatch);
            botFence.draw(spriteBatch);

            mainChar.Draw(spriteBatch);
            
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

            boostMeter.Draw(spriteBatch);

            // Stop drawing
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
