using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris_Adventures.Images
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
            IdleRight,
            IdleLeft,
            RunRight,
            RunLeft,
            LeftJumping,
            RightJumping,
            LeftFalling,
            RightFalling,
            Spawning,
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
