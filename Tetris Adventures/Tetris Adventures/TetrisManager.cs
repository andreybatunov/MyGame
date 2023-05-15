using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.Threading;

namespace Tetris_Adventures
{
    public class TetrisManager 
    { 
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
        public TetrisObject CurrentTetrisObject;
        public Vector2 CurrentMousePosition;
        public TilemapManager TilemapManager;
        public Dictionary<Keys, TetrisFigure> KeyboardKeys;
        public double RotateTimeCheck = 0;
        public double SetObjectTimeCheck = 0;
        public Vector2 DrawPos;
        
        public TetrisManager(Texture2D tetrisSprite, TilemapManager tilemapManager)
        {
            TetrisSpriteSheet = tetrisSprite;
            Shapes = new Dictionary<TetrisFigure, Rectangle>()
            {
                { TetrisFigure.None, new Rectangle(0, 0, 0, 0) },
                { TetrisFigure.IShape, new Rectangle(0, 0, 21, 80) },
                { TetrisFigure.JShape, new Rectangle(23, 0, 41, 60) },
                { TetrisFigure.LShape, new Rectangle(64, 0, 43, 60) },
                { TetrisFigure.OShape, new Rectangle(107, 0, 45, 40) },
                { TetrisFigure.ZShape, new Rectangle(150, 0, 65, 40) },
                { TetrisFigure.TShape, new Rectangle(212, 0, 66, 40) },
                { TetrisFigure.SShape, new Rectangle(277, 0, 60, 40) },
            };
            KeyboardKeys = new Dictionary<Keys, TetrisFigure>()
            {
                {Keys.Q, TetrisFigure.None },
                {Keys.D1, TetrisFigure.IShape },
                {Keys.D2, TetrisFigure.JShape },
                {Keys.D3, TetrisFigure.LShape },
                {Keys.D4, TetrisFigure.OShape },
                {Keys.D5, TetrisFigure.ZShape },
                {Keys.D6, TetrisFigure.TShape },
                {Keys.D7, TetrisFigure.SShape }
            };
            TilemapManager = tilemapManager;
            CurrentTetrisObject = new TetrisObject(TetrisFigure.None, new Vector2(), new Rectangle());
            SettedFigures = new List<TetrisObject>();
        }

        public void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            var keyboard = Keyboard.GetState();
            GetHandleInput(keyboard, mouse, gameTime);
            CurrentMousePosition.X = mouse.X;
            CurrentMousePosition.Y = mouse.Y;
            DrawPos.X = CurrentTetrisObject.Width % 2 == 0
                ? CurrentMousePosition.X - (CurrentMousePosition.X % 20)
                : CurrentMousePosition.X - (CurrentMousePosition.X % 20) - 10;
            DrawPos.Y = CurrentTetrisObject.Height % 2 == 0
                ? CurrentMousePosition.Y - (CurrentMousePosition.Y % 20)
                : CurrentMousePosition.Y - (CurrentMousePosition.Y % 20) - 10;


        }

        public void GetHandleInput(KeyboardState keyboard, MouseState mouse, GameTime gameTime)
        {

            foreach (var keyboardKey in KeyboardKeys)
            {
                if (keyboard.IsKeyDown(keyboardKey.Key))
                {
                    CurrentTetrisObject.Figure = keyboardKey.Value;
                    CurrentTetrisObject.Width = (Shapes[CurrentTetrisObject.Figure].Width - Shapes[CurrentTetrisObject.Figure].Width % 20) / 20;
                    CurrentTetrisObject.Height = (Shapes[CurrentTetrisObject.Figure].Height - Shapes[CurrentTetrisObject.Figure].Height % 20) / 20;
                    CurrentTetrisObject.Origin.X = Shapes[CurrentTetrisObject.Figure].Width / 2;
                    CurrentTetrisObject.Origin.Y = Shapes[CurrentTetrisObject.Figure].Height / 2;
                    CurrentTetrisObject.RotationCorner = 0;
                }
            }
            if (keyboard.IsKeyDown(Keys.C))
            {
                SettedFigures.Clear();
            }
            if (mouse.LeftButton == ButtonState.Pressed 
                && CurrentTetrisObject.Figure != TetrisFigure.None 
                && gameTime.TotalGameTime.TotalMilliseconds - SetObjectTimeCheck > 1000)
            {
                SettedFigures.Add(new TetrisObject(CurrentTetrisObject, DrawPos));
                SetObjectTimeCheck = gameTime.TotalGameTime.TotalMilliseconds;
                //todo
            }
            if (mouse.RightButton == ButtonState.Pressed 
                && CurrentTetrisObject.Figure != TetrisFigure.None 
                && gameTime.TotalGameTime.TotalMilliseconds - RotateTimeCheck > 200)
            { 
                CurrentTetrisObject.Rotate();
                RotateTimeCheck = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tetrisObject in SettedFigures)
            {
                spriteBatch.Draw(TetrisSpriteSheet, tetrisObject.Position, Shapes[tetrisObject.Figure], Color.White, (float)tetrisObject.RotationCorner, tetrisObject.Origin, 1f, SpriteEffects.None, 0f);
            }
            if (CurrentTetrisObject.Figure != TetrisFigure.None)
            { 
                spriteBatch.Draw(TetrisSpriteSheet,
                    DrawPos,
                    Shapes[CurrentTetrisObject.Figure],
                    Color.White,
                    (float)CurrentTetrisObject.RotationCorner,
                    CurrentTetrisObject.Origin,
                    1f,
                    SpriteEffects.None,
                    0f);
            }
        }
    }
}
