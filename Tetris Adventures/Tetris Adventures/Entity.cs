using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris_Adventures
{
    public abstract class Entity
    {
        public Texture2D spritesheet;
        public Vector2 position;

        public enum currentAnimation
        {
            Idle,
            RunRight,
            RunLeft,
        }

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
