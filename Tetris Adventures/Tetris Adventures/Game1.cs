using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledSharp;
using SharpDX.Direct3D9;
using System.Collections.Generic;

namespace Tetris_Adventures
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        #region TetrisManager
        private TetrisManager tetris;
        private Texture2D tetrisSprite;
        #endregion

        #region Tilemaps
        private TmxMap map;
        private TilemapManager tilemapManager;
        private Texture2D tileset;
        private List<Rectangle> collisionRectangles;
        private Vector2 startPosition;
        private Rectangle deathRectangle;
        #endregion

        #region Player
        private Player player;
        private Texture2D playerSprite;
        private Texture2D runningPlayerSprite;
        private Texture2D fallingPlayerSprite;
        private Texture2D jumpingPlayerSprite;
        private Texture2D spawningPlayerSprite;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            IsMouseVisible = false;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1440;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

          

            #region Tilemap
            map = new TmxMap("Content\\map.tmx");
            tileset = Content.Load<Texture2D>("tileset");
            var tileWidth = map.Tilesets[0].TileWidth;
            var tileHeight = map.Tilesets[0].TileHeight;
            var tilesetTileWidth = tileset.Width / tileWidth;
            tilemapManager = new TilemapManager(map, tileset, tilesetTileWidth, tileWidth, tileHeight);
            #endregion

            collisionRectangles = new List<Rectangle>();
            foreach (var obj in map.ObjectGroups["collisions"].Objects)
            {
                if (obj.Name == "")
                {
                    collisionRectangles.Add(new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height));
                }
                if (obj.Name == "start")
                {
                    startPosition = new Vector2((int)obj.X, (int)obj.Y);
                }
                if (obj.Name == "death")
                {
                    deathRectangle = new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height);
                }
            }

            #region Player

            playerSprite = Content.Load<Texture2D>("idleSprite");
            spawningPlayerSprite = Content.Load<Texture2D>("spawningSprite");
            runningPlayerSprite = Content.Load<Texture2D>("runningSpritePlayer");
            fallingPlayerSprite = Content.Load<Texture2D>("fallingSprite");
            jumpingPlayerSprite = Content.Load<Texture2D>("jumpingSprite");
            player = new Player(startPosition, spawningPlayerSprite, runningPlayerSprite, playerSprite, fallingPlayerSprite, jumpingPlayerSprite);
            #endregion

            #region TetrisManager
            tetrisSprite = Content.Load<Texture2D>("tetrisFigures");
            tetris = new TetrisManager(tetrisSprite);
            #endregion


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            var initPosition = player.Position;
            player.Update();
            #region Player Collisions
            foreach (var rectangle in collisionRectangles)
            {
                if (!player.IsJumping)
                {
                    player.IsFalling = true;
                }
                if (rectangle.Intersects(player.PlayerFallRectangle))
                {
                    player.IsFalling = false;
                    break;
                }
            }
            foreach (var rectangle in collisionRectangles)
            {
                if (rectangle.Intersects(player.Hitbox))
                {
                    player.Velocity.X = initPosition.X;
                    player.Position.X = initPosition.X;
                    break;
                }
            }
            if (deathRectangle.Intersects(player.PlayerFallRectangle))
            {
                player = new Player(startPosition, spawningPlayerSprite, runningPlayerSprite, playerSprite, fallingPlayerSprite, jumpingPlayerSprite);
            }
            #endregion

            tetris.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightCoral);
            spriteBatch.Begin();
            tilemapManager.Draw(spriteBatch);
            player.Draw(spriteBatch, gameTime);
            tetris.Draw(spriteBatch, gameTime);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}