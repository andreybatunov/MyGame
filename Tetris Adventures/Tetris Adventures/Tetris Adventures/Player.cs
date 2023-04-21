using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_Adventures
{
    class Player : AnimatedSprite  
    {
        float mySpeed = 120;
        public Player(Vector2 position) : base(position)
        {
            FramesPerSecond = 11;   
        }

        public void LoadContent(ContentManager content)
        {
            sTexture = content.Load<Texture2D>("Player");
            AddAnimation(12);
        }

        public override void Update(GameTime gameTime) 
        {
            sDirection = Vector2.Zero;
            HandleInput(Keyboard.GetState());
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            sDirection *= mySpeed;
            sPosition += (sDirection * deltaTime);
            base.Update(gameTime);
        }

        private void HandleInput(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.Right))
            {
                sDirection += new Vector2(1, 0);
            }
            if (keyState.IsKeyDown(Keys.Left))
            {
                sDirection += new Vector2(-1, 0);
            }
            
        }
    }
}
