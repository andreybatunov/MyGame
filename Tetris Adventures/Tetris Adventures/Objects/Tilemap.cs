using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TiledSharp;

namespace Tetris_Adventures.Objects
{
    public class Tilemap
    {
        public TmxMap Map { get; set; }
        public Texture2D Tileset { get; set; }
        public Texture2D Finish { get; set; }
        public List<Rectangle> CollisionObjects { get; set; }
        public Vector2 StartPosition { get; set; }
        public Rectangle FinishRectangle { get; set; }
        public List<Rectangle> DeathRectangles { get; set; }
        public List<Rectangle> SurfaceRectangles { get; set; }
    }
}
