using Microsoft.Xna.Framework;
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
        public Vector2 Position { get; set; }
        public bool WillBeDrawn { get; set; }

        public TetrisObject(TetrisFigure figure, Vector2 position)
        {
            Figure = figure;
            Position = position;
            WillBeDrawn = false;
        }
    }
}
