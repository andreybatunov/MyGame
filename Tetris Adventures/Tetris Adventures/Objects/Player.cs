using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Security.Policy;
using Tetris_Adventures.Images;
using Tetris_Adventures.Managers;

namespace Tetris_Adventures.Objects
{
    public class Player : Entity
    {
        public Vector2 Velocity;
        public Direction Direction = Direction.Right;
        public bool IsAlive = true;
        public bool IsReachedFinish = false;

        public float StartY;
        private const float playerSpeed = 2.4f;
        private const float fallSpeed = 6f;
        private float jumpSpeed = -12f;
        
        private bool isFalling = true;
        private bool isJumping = false;
        private double jumpDelayCheck;
        private Rectangle playerFallRectangle;
        private Rectangle playerJumpRectangle;
        
        private TilemapManager tilemapManager;
        private Animation[] playerAnimation;
        private CurrentAnimation playerAnimationController;

        public Player(TilemapManager tilemapManager, Texture2D spawningSprite, Texture2D runSprite, Texture2D idleSprite, Texture2D fallingSprite, Texture2D jumpingSprite)
        {
            this.tilemapManager = tilemapManager;
            Position = this.tilemapManager.StartPosition;
            Velocity = this.tilemapManager.StartPosition;
            StartY = Position.Y;
            playerAnimation = new Animation[]
            {
                new Animation(spawningSprite, 51, 48, true),
                new Animation(runSprite, 44, 41, true),
                new Animation(idleSprite, 35, 40, true),
                new Animation(fallingSprite, 30, 48, false),
                new Animation(jumpingSprite, 29, 53, false)
            };
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, 40, 35);
            playerFallRectangle = new Rectangle((int)Position.X - 3, (int)Position.Y + 40, 35, 1);
            playerJumpRectangle = new Rectangle((int)Position.X - 3, (int)Position.Y, 35, 1);
        }

        public override void Update(GameTime gameTime)
        {
            var initPosition = Position;
            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Escape))
            {

            }
            playerAnimationController = Direction == Direction.Right
                ? CurrentAnimation.IdleRight
                : CurrentAnimation.IdleLeft;
            if (isFalling)
            {
                Velocity.Y += fallSpeed;
                playerAnimationController = Direction == Direction.Right
                    ? CurrentAnimation.RightFalling
                    : CurrentAnimation.LeftFalling;
            }


            Move(keyboard);
            StartY = Position.Y;
            Jump(keyboard, gameTime);

            UpdateHitboxes();
            GetCollisions(initPosition);
            Position = Velocity;
        }

        public void GetCollisions(Vector2 initPosition)
        {
            foreach (var rectangle in tilemapManager.SurfaceRectangles)
            {
                if (rectangle.Intersects(playerFallRectangle))
                {
                    isFalling = false;
                    break;
                }
                if (!isJumping)
                {
                    isFalling = true;
                }

            }
            foreach (var rectangle in tilemapManager.SurfaceRectangles)
            {
                if (rectangle.Intersects(Hitbox))
                {
                    Velocity.X = initPosition.X;
                    Position.X = initPosition.X;
                    break;
                }
            }
            foreach (var rectangle in tilemapManager.SurfaceRectangles)
            {
                if (rectangle.Intersects(playerJumpRectangle))
                {
                    isJumping = false;
                }
            }
            foreach (var rectangle in tilemapManager.DeathRectangles)
            {
                if (rectangle.Intersects(playerFallRectangle))
                {
                    IsAlive = false;
                }
            }
            if (tilemapManager.FinishRectangle.Intersects(Hitbox))
            {
                IsReachedFinish = true;
            }
        }

        private void Move(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.A))
            {
                Velocity.X -= playerSpeed;
                if (!isJumping && !isFalling)
                {
                    playerAnimationController = CurrentAnimation.RunLeft;
                    Direction = Direction.Left;
                }
                else
                {
                    playerAnimationController = isJumping
                        ? CurrentAnimation.LeftJumping
                        : CurrentAnimation.LeftFalling;
                    Direction = Direction.Left;
                }
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                Velocity.X += playerSpeed;
                if (!isJumping && !isFalling)
                {
                    playerAnimationController = CurrentAnimation.RunRight;
                    Direction = Direction.Right;
                }
                else
                {
                    playerAnimationController = isJumping
                        ? CurrentAnimation.RightJumping
                        : CurrentAnimation.RightFalling;
                    Direction = Direction.Right;
                }
            }

        }

        private void Jump(KeyboardState keyboard, GameTime gameTime)
        {
            if (isJumping)
            {

                Velocity.Y += jumpSpeed;
                jumpSpeed += 1;
                Move(keyboard);
                playerAnimationController = Direction == Direction.Right
                    ? CurrentAnimation.RightJumping
                    : CurrentAnimation.LeftJumping;
                if (Velocity.Y >= StartY)
                {
                    isJumping = false;
                }
            }
            else
            {
                if (keyboard.IsKeyDown(Keys.Space) && !isFalling && gameTime.TotalGameTime.TotalMilliseconds - jumpDelayCheck > 600)
                {
                    isJumping = true;
                    isFalling = false;
                    jumpSpeed = -12;
                    jumpDelayCheck = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
        }

        public void UpdateHitboxes()
        {
            Hitbox.X = Direction == Direction.Right
                ? (int)Position.X
                : (int)Position.X - 3;
            Hitbox.Y = (int)Position.Y - 3;
            playerFallRectangle.X = (int)Position.X;
            playerFallRectangle.Y = (int)Velocity.Y + 39;
            playerJumpRectangle.X = (int)Position.X;
            playerJumpRectangle.Y = (int)Velocity.Y;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsAlive)
                switch (playerAnimationController)
                {
                    case CurrentAnimation.IdleRight:
                        playerAnimation[2].Draw(spriteBatch, Position, gameTime, 1, 150);
                        break;
                    case CurrentAnimation.IdleLeft:
                        playerAnimation[2].Draw(spriteBatch, Position - new Vector2(5, 0), gameTime, 2, 150);
                        break;
                    case CurrentAnimation.RunRight:
                        playerAnimation[1].Draw(spriteBatch, Position, gameTime, 1, 75);
                        break;
                    case CurrentAnimation.RunLeft:
                        playerAnimation[1].Draw(spriteBatch, Position - new Vector2(5, 0), gameTime, 2, 75);
                        break;
                    case CurrentAnimation.RightJumping:
                        playerAnimation[4].Draw(spriteBatch, Position, gameTime, 1, 100);
                        break;
                    case CurrentAnimation.LeftJumping:
                        playerAnimation[4].Draw(spriteBatch, Position, gameTime, 2, 100);
                        break;
                    case CurrentAnimation.RightFalling:
                        playerAnimation[3].Draw(spriteBatch, Position, gameTime, 1, 75);
                        break;
                    case CurrentAnimation.LeftFalling:
                        playerAnimation[3].Draw(spriteBatch, Position, gameTime, 2, 75);
                        break;
                }
        }
    }
}
