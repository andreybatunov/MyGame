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
        public List<Rectangle> CollisionObjects { get; set; }
        public Vector2 StartPosition { get; set; }
        public Rectangle DeathRectangle { get; set; }
    }
}
