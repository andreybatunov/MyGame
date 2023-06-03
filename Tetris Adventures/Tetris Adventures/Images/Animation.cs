using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Tetris_Adventures.Images
{
    public class Animation
    {
        private readonly Texture2D spritesheet;
        private readonly bool IsLooping;
        private readonly int width;
        private readonly int height;
        private readonly int frames;
        private int pointer;
        private float timeSinceLastFrame;

        public Animation(Texture2D spritesheet, int width, int height, bool isLooping)
        {
            this.spritesheet = spritesheet;
            frames = spritesheet.Width / width;
            this.width = width;
            this.height = height;
            IsLooping = isLooping;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime, int row, float millisecondsPerFrames)
        {
            var rectangle = new Rectangle(width * pointer, height * (row - 1), width, height);
            spriteBatch.Draw(spritesheet, position, rectangle, Color.White);
            UpdatePointer(gameTime, millisecondsPerFrames);
        }

        private void UpdatePointer(GameTime gameTime, float millisecondsPerFrames)
        {
            timeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeSinceLastFrame > millisecondsPerFrames)
            {
                timeSinceLastFrame -= millisecondsPerFrames;
                if (pointer != frames && IsLooping)
                {
                    pointer++;
                }
                if (pointer == frames)
                {
                    pointer = 0;
                }
            }
        }
    }
}
