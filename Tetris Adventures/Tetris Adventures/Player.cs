using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace Tetris_Adventures
{
    class Player : Entity
    {
        public Vector2 Velocity;
        public float playerSpeed = 1.5f;
        public Animation[] playerAnimation;

        public currentAnimation playerAnimationController;
        public Player(Texture2D runSprite, Texture2D idleSprite)
        {
            Velocity = new Vector2();
            playerAnimation = new Animation[] 
            { 
                new Animation(runSprite, 44, 40), new Animation(idleSprite, 35, 40) 
            };
        }

        public override void Update() 
        {

            KeyboardState keyboard = Keyboard.GetState();
            playerAnimationController = currentAnimation.Idle;

            if (keyboard.IsKeyDown(Keys.Left))
            {
                Velocity.X -= playerSpeed;
                playerAnimationController = currentAnimation.RunLeft;
            }

            if (keyboard.IsKeyDown(Keys.Right))
            {
                Velocity.X += playerSpeed;
                playerAnimationController = currentAnimation.RunRight;
            }

            position = Velocity;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            switch (playerAnimationController)
            {
                case currentAnimation.Idle:
                    playerAnimation[1].Draw(spriteBatch, position, gameTime, 1);
                    break;
                case currentAnimation.RunRight:
                    playerAnimation[0].Draw(spriteBatch, position, gameTime, 1);
                    break;
                case currentAnimation.RunLeft:
                    playerAnimation[0].Draw(spriteBatch, position, gameTime, 2);
                    break;
            }
            //spriteBatch.Draw(spritesheet, position, Color.White);
        }
    }
}
