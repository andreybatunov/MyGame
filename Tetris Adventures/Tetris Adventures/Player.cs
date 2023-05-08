using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace Tetris_Adventures
{
    class Player : Entity
    {
        public Vector2 Velocity;
        public float PlayerSpeed = 1.55f;
        public float FallSpeed = 5f;
        public float JumpSpeed = -13;
        public float StartY;
        public bool IsFalling = true;
        public bool IsJumping = false;
        public Rectangle PlayerFallRectangle;
        public Direction Direction = Direction.Right;


        public Animation[] playerAnimation;
        public CurrentAnimation playerAnimationController;

        public Player(Texture2D runSprite, Texture2D idleSprite, Texture2D fallingSprite, Texture2D jumpingSprite)
        {
            Position = new Vector2();
            Velocity = new Vector2(100,500);
            StartY = Position.Y;
            playerAnimation = new Animation[] 
            { 
                new Animation(runSprite, 44, 41),
                new Animation(idleSprite, 35, 41),
                new Animation(fallingSprite, 30, 48),
                new Animation(jumpingSprite, 31, 53)
            };
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, 40, 35);
            PlayerFallRectangle = new Rectangle((int)Position.X - 3, (int)Position.Y + 40, 35, 1);
        }

        public override void Update() 
        {
            var keyboard = Keyboard.GetState();
            playerAnimationController = Direction == Direction.Right
                ? CurrentAnimation.IdleRight
                : CurrentAnimation.IdleLeft;
            Position = Velocity;
            if (IsFalling)
            {
                Velocity.Y += FallSpeed;
                playerAnimationController = Direction == Direction.Right
                    ? CurrentAnimation.RightFalling
                    : CurrentAnimation.LeftFalling;
            }
            
            StartY = Position.Y;
            Move(keyboard);
            Jump(keyboard);

            Hitbox.X = (int)Position.X;
            Hitbox.Y = (int)Position.Y - 3;
            PlayerFallRectangle.X = (int)Position.X;
            PlayerFallRectangle.Y = (int)Velocity.Y + 40;
            
           
        }

        private void Move(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Left))
            {
                Velocity.X -= PlayerSpeed;
                playerAnimationController = CurrentAnimation.RunLeft;
                Direction = Direction.Left;
            }

            if (keyboard.IsKeyDown(Keys.Right))
            {
                Velocity.X += PlayerSpeed;
                playerAnimationController = CurrentAnimation.RunRight;
                Direction = Direction.Right;
            }
        }

        private void Jump(KeyboardState keyboard)
        {
            if (IsJumping)
            {
                Velocity.Y += JumpSpeed;
                JumpSpeed += 1;
                playerAnimationController = Direction == Direction.Right
                    ? CurrentAnimation.RightJumping
                    : CurrentAnimation.LeftJumping;
                Move(keyboard);
                if (Velocity.Y >= StartY) 
                {
                    Velocity.Y = StartY;
                    IsJumping = false;
                }
            }
            else
            {
                if (keyboard.IsKeyDown(Keys.Up) && !IsFalling)
                {
                    IsJumping = true;
                    IsFalling = false;
                    JumpSpeed = -13;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            switch (playerAnimationController)
            {
                case CurrentAnimation.IdleRight:
                    playerAnimation[1].Draw(spriteBatch, Position, gameTime, 1, 150);
                    break;
                case CurrentAnimation.IdleLeft:
                    playerAnimation[1].Draw(spriteBatch, Position, gameTime, 2, 150);
                    break;
                case CurrentAnimation.RunRight:
                    playerAnimation[0].Draw(spriteBatch, Position, gameTime, 1, 75);
                    break;
                case CurrentAnimation.RunLeft:
                    playerAnimation[0].Draw(spriteBatch, Position, gameTime, 2, 75);
                    break;
                case CurrentAnimation.RightJumping:
                    playerAnimation[3].Draw(spriteBatch, Position, gameTime, 1, 100);
                    break;
                case CurrentAnimation.LeftJumping:
                    playerAnimation[3].Draw(spriteBatch, Position, gameTime, 2, 100);
                    break;
                case CurrentAnimation.RightFalling:
                    playerAnimation[2].Draw(spriteBatch, Position, gameTime, 1, 75);
                    break;
                case CurrentAnimation.LeftFalling:
                    playerAnimation[2].Draw(spriteBatch, Position, gameTime, 2, 75);
                    break;
            }
        }
    }
}
