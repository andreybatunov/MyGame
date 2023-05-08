using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris_Adventures
{
    public abstract class Entity
    {
        public Texture2D SpriteSheet;
        public Vector2 Position;
        public Rectangle Hitbox;

        public enum Direction
        {
            Left,
            Right,
        }

        public enum CurrentAnimation
        {
            IdleLeft,
            IdleRight,
            RunRight,
            RunLeft,
            LeftJumping,
            RightJumping,
            LeftFalling,
            RightFalling
        }

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
