using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;


namespace Tetris_Adventures
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Player player;
        private Texture2D playerSprite;
        private Texture2D runningPlayerSprite;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        { 
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerSprite = Content.Load<Texture2D>("playerSprite");
            runningPlayerSprite = Content.Load<Texture2D>("runningSpritePlayer");



            player = new Player(runningPlayerSprite, playerSprite);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            player.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightCoral);
            spriteBatch.Begin();
            player.Draw(spriteBatch, gameTime);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}