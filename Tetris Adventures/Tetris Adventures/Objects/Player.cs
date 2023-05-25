﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tetris_Adventures.Images;
using Tetris_Adventures.Managers;

namespace Tetris_Adventures.Objects
{
    public class Player : Entity
    {
        public Vector2 Velocity;
        public float PlayerSpeed = 2.4f;
        public float FallSpeed = 6;
        public float JumpSpeed = -12;
        public float StartY;
        public bool IsFalling = true;
        public bool IsJumping = false;
        public double JumpDelay = 0;
        public Rectangle PlayerFallRectangle;
        public Rectangle PlayerJumpRectangle;
        public Direction Direction = Direction.Right;
        public TilemapManager TilemapManager;
        public bool IsAlive = true;


        public Animation[] playerAnimation;
        public CurrentAnimation playerAnimationController;

        public Player(TilemapManager tilemapManager, Texture2D spawningSprite, Texture2D runSprite, Texture2D idleSprite, Texture2D fallingSprite, Texture2D jumpingSprite)
        {
            TilemapManager = tilemapManager;
            Position = TilemapManager.StartPosition;
            Velocity = TilemapManager.StartPosition;
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
            PlayerFallRectangle = new Rectangle((int)Position.X - 3, (int)Position.Y + 40, 35, 1);
            PlayerJumpRectangle = new Rectangle((int)Position.X - 3, (int)Position.Y, 35, 1);
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
            if (IsFalling)
            {
                Velocity.Y += FallSpeed;
                playerAnimationController = Direction == Direction.Right
                    ? CurrentAnimation.RightFalling
                    : CurrentAnimation.LeftFalling;
            }


            Move(keyboard);
            StartY = Position.Y;
            Jump(keyboard, gameTime, initPosition);

            Hitbox.X = Direction == Direction.Right
                ? (int)Position.X
                : (int)Position.X - 3;
            Hitbox.Y = (int)Position.Y - 3;
            PlayerFallRectangle.X = (int)Position.X;
            PlayerFallRectangle.Y = (int)Velocity.Y + 39;
            PlayerJumpRectangle.X = (int)Position.X;
            PlayerJumpRectangle.Y = (int)Velocity.Y;
            GetCollisions(initPosition);
            Position = Velocity;
        }

        public void GetCollisions(Vector2 initPosition)
        {
            foreach (var rectangle in TilemapManager.CollisionObjects)
            {
                if (rectangle.Intersects(PlayerFallRectangle))
                {
                    IsFalling = false;
                    break;
                }
                if (!IsJumping)
                {
                    IsFalling = true;
                }

            }
            foreach (var rectangle in TilemapManager.CollisionObjects)
            {
                if (rectangle.Intersects(Hitbox))
                {
                    Velocity.X = initPosition.X;
                    Position.X = initPosition.X;
                    break;
                }
            }
            foreach (var rectangle in TilemapManager.CollisionObjects)
            {
                if (rectangle.Intersects(PlayerJumpRectangle))
                {
                    IsJumping = false;
                }
            }

            if (TilemapManager.DeathRectangle.Intersects(PlayerFallRectangle))
            {
                IsAlive = false;
            }
        }

        private void Move(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.A))
            {
                Velocity.X -= PlayerSpeed;
                if (!IsJumping && !IsFalling)
                {
                    playerAnimationController = CurrentAnimation.RunLeft;
                    Direction = Direction.Left;
                }
                else
                {
                    playerAnimationController = IsJumping
                        ? CurrentAnimation.LeftJumping
                        : CurrentAnimation.LeftFalling;
                    Direction = Direction.Left;
                }
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                Velocity.X += PlayerSpeed;
                if (!IsJumping && !IsFalling)
                {
                    playerAnimationController = CurrentAnimation.RunRight;
                    Direction = Direction.Right;
                }
                else
                {
                    playerAnimationController = IsJumping
                        ? CurrentAnimation.RightJumping
                        : CurrentAnimation.RightFalling;
                    Direction = Direction.Right;
                }
            }

        }

        private void Jump(KeyboardState keyboard, GameTime gameTime, Vector2 initPosition)
        {
            if (IsJumping)
            {

                Velocity.Y += JumpSpeed;
                JumpSpeed += 1;

                Move(keyboard);
                playerAnimationController = Direction == Direction.Right
                    ? CurrentAnimation.RightJumping
                    : CurrentAnimation.LeftJumping;

                if (Velocity.Y >= StartY)
                {
                    IsJumping = false;
                }


            }
            else
            {
                if (keyboard.IsKeyDown(Keys.Space) && !IsFalling && gameTime.TotalGameTime.TotalMilliseconds - JumpDelay > 600)
                {
                    IsJumping = true;
                    IsFalling = false;
                    JumpSpeed = -12;
                    JumpDelay = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsAlive)
                switch (playerAnimationController)
                {
                    case CurrentAnimation.Spawning:
                        playerAnimation[0].Draw(spriteBatch, Position, gameTime, 1, 100);
                        break;
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