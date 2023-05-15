using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tetris_Adventures.TetrisManager;

namespace Tetris_Adventures
{
    public class TetrisObject
    {
        public TetrisFigure Figure { get; set; }
        public Rectangle TextureRectangle { get; set; }
        public Vector2 Position { get; set; }
        public double RotationCorner;
        public Vector2 Origin;
        public int Width;
        public int Height;

        public TetrisObject(TetrisFigure figure, Vector2 position, Rectangle textureRectangle)
        {
            Figure = figure;
            Position = position;
            RotationCorner = 0;
            TextureRectangle = textureRectangle;
            Width = textureRectangle.Width;
            Height = textureRectangle.Height;
            Origin = new Vector2(Width / 2, Height / 2);
        }

        public TetrisObject(TetrisObject tetrisObject, Vector2 position)
        {
            Figure = tetrisObject.Figure;
            TextureRectangle = tetrisObject.TextureRectangle;
            Width= tetrisObject.Width;
            Height= tetrisObject.Height;
            Position = position;
            RotationCorner = tetrisObject.RotationCorner;
            Origin = tetrisObject.Origin;
        }

        public void Rotate()
        {
            RotationCorner += Math.PI / 2;
            if (RotationCorner == 2*Math.PI)
                RotationCorner = 0;
            (Height, Width) = (Width, Height);
        }
    }
}
