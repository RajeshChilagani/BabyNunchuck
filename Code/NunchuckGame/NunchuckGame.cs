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

        Texture2D arrowTexture;


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
            Texture2D mainTexture = this.Content.Load<Texture2D>( "AllAttacks");
            float scale = 0.3f;

            arrowTexture = this.Content.Load<Texture2D>("arrow");

            // Load the player resources
            mainChar = new MainPlayer(new Vector2(GraphicsDevice.Viewport.Width / 2 - mainTexture.Width / 2, GraphicsDevice.Viewport.Height / 2-mainTexture.Height/2), new Vector2(50));

            //arrow = new MainPlayer(new Vector2(GraphicsDevice.Viewport.Width / 2 - mainTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 - mainTexture.Height / 2), new Vector2(50));

            Pickup.PickupTexture = Content.Load<Texture2D>("Enemy");
            Pickup.PickupScale = 2f;
            mainChar.LoadTexture(mainTexture, arrowTexture);
            mainChar.Initialize();

            // Set the game font
            gameState.SetFont(Content.Load<SpriteFont>("font"));

            float boostScale = 3f;
            Texture2D boostContainerTexture = Content.Load<Texture2D>("BoostContainer");
            boostMeter.Initialize(boostContainerTexture, Content.Load<Texture2D>("BoostBarCropped"), boostScale, 
                new Vector2(GraphicsDevice.Viewport.Width - 20 - (boostContainerTexture.Width * boostScale), 20), ref mainChar);
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
                gameState.RestartGame();

            }
            // Game state is updated second and may override or ignore user input
            if(!gameState.IsGameOver)
            {
                gameState.Update(gameTime, GraphicsDevice.Viewport.TitleSafeArea, ref mainChar);
                mainChar.controller(gameTime, GraphicsDevice.Viewport);
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Start drawing
            spriteBatch.Begin();

            // Draw the Player
            mainChar.Draw(spriteBatch);
            
            if (!gameState.IsGameOver)
            {  
               gameState.Draw(spriteBatch);
            }
            else
            {
                spriteBatch.DrawString(gameState.font, "Game Over", new Vector2(10, 40), Color.Red);
            }

            boostMeter.Draw(spriteBatch);

            // Stop drawing
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
