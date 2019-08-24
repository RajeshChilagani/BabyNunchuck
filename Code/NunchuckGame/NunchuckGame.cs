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
        Player player;
        GameState gameState;

        public NunchuckGame()
        {
            graphics = new GraphicsDeviceManager(this);
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
            player = new Player();
            gameState = new GameState();
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

            Texture2D texture = Content.Load<Texture2D>("player");
            float scale = 0.3f;

            // Load the player resources
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + 
                GraphicsDevice.Viewport.TitleSafeArea.Width / 2 - texture.Width * scale / 2, 
                GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2 -
                texture.Height * scale / 2);
            player.Initialize(texture, playerPosition, scale);

            Pickup.PickupTexture = Content.Load<Texture2D>("player");
            Pickup.PickupScale = 0.03f;

            // Set the game font
            gameState.SetFont(Content.Load<SpriteFont>("font"));
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


            // Game state is updated second and may override or ignore user input
            gameState.Update(gameTime, GraphicsDevice.Viewport.TitleSafeArea, ref player);

            // The player gets moved
            player.Update(gameTime);

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
            player.Draw(spriteBatch);

            gameState.Draw(spriteBatch);

            // Stop drawing
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
