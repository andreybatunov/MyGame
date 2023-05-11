using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static Tetris_Adventures.TetrisObject;


namespace Tetris_Adventures
{

    public class TetrisManager { 

    
        public enum TetrisFigure
        {
            None,
            IShape,
            JShape,
            LShape,
            OShape,
            ZShape,
            TShape,
            SShape,
        }

        public List<TetrisObject> SettedFigures;
        public Texture2D TetrisSpriteSheet;
        public Dictionary<TetrisFigure, Rectangle> Shapes;
        public TetrisObject CurrentTetrisFigure;
        public Vector2 CurrentMousePosition;
        
        public TetrisManager(Texture2D tetrisSprite)
        {
            TetrisSpriteSheet = tetrisSprite;
            Shapes = new Dictionary<TetrisFigure, Rectangle>()
            {
                { TetrisFigure.IShape, new Rectangle(0, 0, 22, 80) },
                { TetrisFigure.JShape, new Rectangle(23, 0, 41, 80) },
                { TetrisFigure.LShape, new Rectangle(64, 0, 43, 80) },
                { TetrisFigure.OShape, new Rectangle(107, 0, 45, 80) },
                { TetrisFigure.ZShape, new Rectangle(150, 0, 65, 80) },
                { TetrisFigure.TShape, new Rectangle(212, 0, 66, 80) },
                { TetrisFigure.SShape, new Rectangle(277, 0, 60, 80) },
            };
            CurrentTetrisFigure = new TetrisObject(TetrisFigure.None, new Vector2());
            SettedFigures = new List<TetrisObject>();
        }

        public void Update()
        {
            var mouse = Mouse.GetState();
            var keyboard = Keyboard.GetState();
            CurrentMousePosition.X = mouse.X;
            CurrentMousePosition.Y = mouse.Y;
            GetHandleInput(keyboard, mouse);
            

        }

        public void GetHandleInput(KeyboardState keyboard, MouseState mouse)
        {
            if (keyboard.IsKeyDown(Keys.D1))
                CurrentTetrisFigure.Figure = TetrisFigure.IShape;
            if (keyboard.IsKeyDown(Keys.D2))
                CurrentTetrisFigure.Figure = TetrisFigure.JShape;
            if (keyboard.IsKeyDown(Keys.D3))
                CurrentTetrisFigure.Figure = TetrisFigure.LShape;
            if (keyboard.IsKeyDown(Keys.D4))
                CurrentTetrisFigure.Figure = TetrisFigure.OShape;
            if (keyboard.IsKeyDown(Keys.D5))
                CurrentTetrisFigure.Figure = TetrisFigure.ZShape;
            if (keyboard.IsKeyDown(Keys.D6))
                CurrentTetrisFigure.Figure = TetrisFigure.TShape;
            if (keyboard.IsKeyDown(Keys.D7))
                CurrentTetrisFigure.Figure = TetrisFigure.SShape;
            if (keyboard.IsKeyDown(Keys.Q))
                CurrentTetrisFigure.Figure = TetrisFigure.None;
            if (mouse.LeftButton == ButtonState.Pressed && CurrentTetrisFigure.Figure != TetrisFigure.None)
            {
                SettedFigures.Add(new TetrisObject(CurrentTetrisFigure.Figure, new Vector2(mouse.X, mouse.Y)));

            }
        }


        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var figure in SettedFigures)
            {
                spriteBatch.Draw(TetrisSpriteSheet, figure.Position, Shapes[figure.Figure], Color.White);
            }
            if (CurrentTetrisFigure.Figure != TetrisFigure.None)
            {
                spriteBatch.Draw(TetrisSpriteSheet, CurrentMousePosition, Shapes[CurrentTetrisFigure.Figure], Color.White);
            }
        }
    }
}
