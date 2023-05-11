using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace Tetris_Adventures
{
    public class Tilemap 
    {
        public TmxMap Map { get; set; }
        public Texture2D Tileset { get; set; }
    }
}
