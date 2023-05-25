using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.Linq;
using Tetris_Adventures.Objects;
using MonoGame.Extended.Timers;

namespace Tetris_Adventures.Managers
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

        public List<TetrisObject> DrawnFigures;
        public Texture2D TetrisSpriteSheet;
        public Dictionary<TetrisFigure, Rectangle> Shapes = new()
            {
                { TetrisFigure.None, new Rectangle(0, 0, 0, 0) },
                { TetrisFigure.IShape, new Rectangle(0, 0, 20, 80) },
                { TetrisFigure.JShape, new Rectangle(20, 0, 40, 60) },
                { TetrisFigure.LShape, new Rectangle(60, 0, 40, 60) },
                { TetrisFigure.OShape, new Rectangle(100, 0, 40, 40) },
                { TetrisFigure.ZShape, new Rectangle(140, 0, 60, 40) },
                { TetrisFigure.TShape, new Rectangle(200, 0, 60, 40) },
                { TetrisFigure.SShape, new Rectangle(260, 0, 60, 40) },
            };
        public TetrisObject CurrentTetrisObject;
        public Vector2 CurrentMousePosition;
        public TilemapManager MapManager;
        public Dictionary<Keys, TetrisFigure> KeyboardKeys = new()
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
        public double RotateTimeCheck = 0;
        public double SetObjectTimeCheck = 0;
        public static Vector2 DrawPos;
        public Player Player;
        public List<Rectangle> EnvironmentTetrisSquares;
        public Dictionary<TetrisFigure, (double, bool)> DelaysDictionary = new()
        {
            { TetrisFigure.None, (0, false) },
            { TetrisFigure.IShape, (0, true)},
            { TetrisFigure.JShape, (0, true)},
            { TetrisFigure.LShape, (0, true)},
            { TetrisFigure.OShape, (0, true)},
            { TetrisFigure.ZShape, (0, true)},
            { TetrisFigure.TShape, (0, true)},
            { TetrisFigure.SShape, (0, true)},
        };

        public TetrisManager(Texture2D tetrisSprite, TilemapManager tilemapManager, Player player)
        {
            TetrisSpriteSheet = tetrisSprite;
            MapManager = tilemapManager;
            CurrentTetrisObject = new TetrisObject(TetrisFigure.None, new Vector2(), new Rectangle());
            DrawnFigures = new List<TetrisObject>();
            EnvironmentTetrisSquares = new List<Rectangle>();
            Player = player;
        }

        public void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            var keyboard = Keyboard.GetState();
            CurrentMousePosition.X = mouse.X;
            CurrentMousePosition.Y = mouse.Y;
            DrawPos.X = CurrentTetrisObject.Width % 2 == 0
                ? CurrentMousePosition.X - CurrentMousePosition.X % 20
                : CurrentMousePosition.X - CurrentMousePosition.X % 20 - 10;
            DrawPos.Y = CurrentTetrisObject.Height % 2 == 0
                ? CurrentMousePosition.Y - CurrentMousePosition.Y % 20
                : CurrentMousePosition.Y - CurrentMousePosition.Y % 20 - 10;
            CurrentTetrisObject.CanBeSetted = CanCurrentObjectBeSetted(gameTime);
            GetHandleInput(keyboard, mouse, gameTime);
            CheckDelayStatus(gameTime);
        }

        public void GetHandleInput(KeyboardState keyboard, MouseState mouse, GameTime gameTime)
        {
            foreach (var keyboardKey in KeyboardKeys)
            {
                if (keyboard.IsKeyDown(keyboardKey.Key))
                {
                    ChangeCurrentTetrisFigure(keyboardKey.Value, gameTime);
                }
            }
            if (keyboard.IsKeyDown(Keys.C))
            {
                ClearMap();
            }
            if (mouse.LeftButton == ButtonState.Pressed
                && CurrentTetrisObject.Figure != TetrisFigure.None
                && gameTime.TotalGameTime.TotalMilliseconds - SetObjectTimeCheck > 200)
            {
                var possibleObject = GetCollisionTetrisObject(CurrentTetrisObject, DrawPos);
                if (CurrentTetrisObject.CanBeSetted)
                {
                    AddTetrisObject(possibleObject);
                    DelaysDictionary[CurrentTetrisObject.Figure] = (gameTime.TotalGameTime.TotalMilliseconds, false);
                    SetObjectTimeCheck = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            if (mouse.RightButton == ButtonState.Pressed
                && CurrentTetrisObject.Figure != TetrisFigure.None
                && gameTime.TotalGameTime.TotalMilliseconds - RotateTimeCheck > 200)
            {
                CurrentTetrisObject.Rotate();
                RotateTimeCheck = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        public List<Rectangle> GetCollisionTetrisObject(TetrisObject tetrisObject, Vector2 drawPos)
        {
            switch (tetrisObject.Figure)
            {
                case TetrisFigure.IShape:
                    return GetIShapeRectangles(tetrisObject, drawPos);
                case TetrisFigure.JShape:
                    return GetJShapeRectangles(tetrisObject, drawPos);
                case TetrisFigure.LShape:
                    return GetLShapeRectangles(tetrisObject, drawPos);
                case TetrisFigure.OShape:
                    return GetOShapeRectangles(tetrisObject, drawPos);
                case TetrisFigure.ZShape:
                    return GetZShapeRectangles(tetrisObject, drawPos);
                case TetrisFigure.TShape:
                    return GetTShapeRectangles(tetrisObject, drawPos);
                case TetrisFigure.SShape:
                    return GetSShapeRectangles(tetrisObject, drawPos);
            }
            return new List<Rectangle>();
        }

        public void ChangeCurrentTetrisFigure(TetrisFigure tetrisFigure, GameTime gameTime)
        {
            CurrentTetrisObject.Figure = tetrisFigure;
            CurrentTetrisObject.Width = (Shapes[CurrentTetrisObject.Figure].Width - Shapes[CurrentTetrisObject.Figure].Width % 20) / 20;
            CurrentTetrisObject.Height = (Shapes[CurrentTetrisObject.Figure].Height - Shapes[CurrentTetrisObject.Figure].Height % 20) / 20;
            CurrentTetrisObject.Origin.X = CurrentTetrisObject.Width * 10;
            CurrentTetrisObject.Origin.Y = CurrentTetrisObject.Height * 10;
            CurrentTetrisObject.RotationCorner = 0;
            CurrentTetrisObject.CanBeSetted = CanCurrentObjectBeSetted(gameTime);
        }
           
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tetrisObject in DrawnFigures)
            {
                spriteBatch.Draw(TetrisSpriteSheet, tetrisObject.Position, Shapes[tetrisObject.Figure], Color.White, (float)tetrisObject.RotationCorner, tetrisObject.Origin, 1f, SpriteEffects.None, 0f);
            } 
            if (CurrentTetrisObject.Figure != TetrisFigure.None)
            {
                spriteBatch.Draw(TetrisSpriteSheet,
                    DrawPos,
                    Shapes[CurrentTetrisObject.Figure],
                    CurrentTetrisObject.CanBeSetted ? Color.GhostWhite : Color.DarkSlateGray,
                    (float)CurrentTetrisObject.RotationCorner,
                    CurrentTetrisObject.Origin,
                    1f,
                    SpriteEffects.None,
                    0f);
            }
        }

        #region TetrisCanBeSetted

        public bool CanCurrentObjectBeSetted(GameTime gameTime) => !IsGroundOfOtherFiguresIntersected() && !IsPlayerHitboxIntersected() && IsTouchingOtherFigure() && IsDelayPassed(gameTime, CurrentTetrisObject.Figure);

        public bool IsPlayerHitboxIntersected()
        {
            var squares = GetCollisionTetrisObject(CurrentTetrisObject, DrawPos);
            foreach (var square in squares)
            {
                if (Player.Hitbox.Intersects(square))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsGroundOfOtherFiguresIntersected()
        {
            var squares = GetCollisionTetrisObject(CurrentTetrisObject, DrawPos);
            foreach (var square in squares)
            {
                foreach (var obj in MapManager.CollisionObjects)
                {
                    if (square.Intersects(obj)) return true;
                }
            }
            return false;
        }

        public bool IsTouchingOtherFigure()
        {
            var squares = GetCollisionTetrisObject(CurrentTetrisObject, DrawPos);
            return squares.Intersect(EnvironmentTetrisSquares).Count() > 0 || DrawnFigures.Count == 0;
        }

        public bool IsDelayPassed(GameTime gameTime, TetrisFigure tetrisFigure) 
        {
           if (gameTime.TotalGameTime.TotalMilliseconds - DelaysDictionary[tetrisFigure].Item1 > 5000)
            {
                DelaysDictionary[tetrisFigure] = (0, true);
            }
           return DelaysDictionary[tetrisFigure].Item2;
        }

        public void CheckDelayStatus(GameTime gameTime)
        {
            foreach(var obj in DelaysDictionary)
            {
                if ( IsDelayPassed(gameTime, obj.Key))
                {
                    DelaysDictionary[obj.Key] = (0, true);
                }
            }
        }
        #endregion

        public List<Rectangle> GetEnvironmentSquares(List<Rectangle> squaresList)
        {
            var environmentSquares = new List<Rectangle>();
            foreach (var square in squaresList)
            {
                for (var x = -20; x < 21; x += 20)
                {
                    for (var y = -20; y < 21; y += 20)
                    {
                        if (Math.Abs(x) == Math.Abs(y)) continue;
                        var newSquare = new Rectangle(square.X + x, square.Y + y, 20, 20);
                        if (!squaresList.Contains(newSquare))
                        {
                            environmentSquares.Add(newSquare);
                        }
                    }
                }
            }
            return environmentSquares;
        }

        public void ClearMap()
        {
            MapManager.CollisionObjects.RemoveRange(MapManager.CollisionObjects.Count - DrawnFigures.Count * 4, DrawnFigures.Count * 4);
            DrawnFigures.Clear();
            EnvironmentTetrisSquares.Clear();
            foreach (var obj in DelaysDictionary)
            {
                DelaysDictionary[obj.Key] = (0, true);
            }
        }

        public void AddTetrisObject(List<Rectangle> possibleObject)
        {
            MapManager.CollisionObjects.AddRange(possibleObject);
            DrawnFigures.Add(new TetrisObject(CurrentTetrisObject, DrawPos));
            EnvironmentTetrisSquares.AddRange(GetEnvironmentSquares(possibleObject));
        }

        #region Tetris Figures Collision
        public List<Rectangle> GetIShapeRectangles(TetrisObject tetrisObject, Vector2 drawPos)
        {
            return tetrisObject.RotationCorner == Math.PI / 2 || tetrisObject.RotationCorner == 3 * Math.PI / 2
                        ? new List<Rectangle>()
                        {
                            new Rectangle((int)drawPos.X - 40, (int)drawPos.Y - 10, 20, 20),
                            new Rectangle((int)drawPos.X - 20, (int)drawPos.Y - 10, 20, 20),
                            new Rectangle((int)drawPos.X, (int)drawPos.Y - 10, 20, 20),
                            new Rectangle((int)drawPos.X + 20, (int)drawPos.Y - 10, 20, 20),
                        }
                        : new List<Rectangle>()
                        {
                            new Rectangle((int)drawPos.X - 10, (int)drawPos.Y - 40, 20, 20),
                            new Rectangle((int)drawPos.X - 10, (int)drawPos.Y - 20, 20, 20),
                            new Rectangle((int) drawPos.X - 10, (int)drawPos.Y, 20, 20),
                            new Rectangle((int) drawPos.X - 10, (int)drawPos.Y + 20, 20, 20),
                        };
        }

        public List<Rectangle> GetJShapeRectangles(TetrisObject tetrisObject, Vector2 drawPos)
        {
            switch (tetrisObject.RotationCorner)
            {
                case 0:
                    return new List<Rectangle>()
                    {
                        new Rectangle((int)drawPos.X, (int)drawPos.Y - 30, 20, 20),
                        new Rectangle((int)drawPos.X, (int)drawPos.Y - 10, 20, 20),
                        new Rectangle((int)drawPos.X, (int)drawPos.Y + 10, 20, 20),
                        new Rectangle((int)drawPos.X - 20, (int)drawPos.Y + 10, 20, 20),
                     };
                case Math.PI / 2:
                    return new List<Rectangle>()
                    {
                        new Rectangle((int)drawPos.X - 30, (int)drawPos.Y - 20, 20, 20),
                        new Rectangle((int)drawPos.X - 30, (int)drawPos.Y, 20, 20),
                        new Rectangle((int)drawPos.X - 10, (int)drawPos.Y, 20, 20),
                        new Rectangle((int)drawPos.X + 10, (int)drawPos.Y, 20, 20),
                    };
                case Math.PI:
                    return new List<Rectangle>()
                    {
                        new Rectangle((int)drawPos.X, (int)drawPos.Y - 30, 20, 20),
                        new Rectangle((int)drawPos.X - 20, (int)drawPos.Y - 30, 20, 20),
                        new Rectangle((int)drawPos.X - 20, (int)drawPos.Y - 10, 20, 20),
                        new Rectangle((int)drawPos.X - 20, (int)drawPos.Y + 10, 20, 20),
                    };
                case 3 * Math.PI / 2:
                    return new List<Rectangle>()
                    {
                        new Rectangle((int)drawPos.X + 10, (int)drawPos.Y, 20, 20),
                        new Rectangle((int)drawPos.X - 30, (int)drawPos.Y - 20, 20, 20),
                        new Rectangle((int)drawPos.X - 10, (int)drawPos.Y - 20, 20, 20),
                        new Rectangle((int)drawPos.X + 10, (int)drawPos.Y - 20, 20, 20),
                    };
            }
            return new List<Rectangle>();
        }

        public List<Rectangle> GetLShapeRectangles(TetrisObject tetrisObject, Vector2 drawPos)
        {
            switch (tetrisObject.RotationCorner)
            {
                case 0:
                    return new List<Rectangle>()
                    {
                        new Rectangle((int)drawPos.X - 20, (int)drawPos.Y - 30, 20, 20),
                        new Rectangle((int)drawPos.X - 20, (int)drawPos.Y - 10, 20, 20),
                        new Rectangle((int)drawPos.X - 20, (int)drawPos.Y + 10, 20, 20),
                        new Rectangle((int)drawPos.X, (int)drawPos.Y + 10, 20, 20),
                     };
                case Math.PI / 2:
                    return new List<Rectangle>()
                    {
                        new Rectangle((int)drawPos.X - 30, (int)drawPos.Y, 20, 20),
                        new Rectangle((int)drawPos.X - 30, (int)drawPos.Y - 20, 20, 20),
                        new Rectangle((int)drawPos.X - 10, (int)drawPos.Y - 20, 20, 20),
                        new Rectangle((int)drawPos.X + 10, (int)drawPos.Y - 20, 20, 20),
                    };
                case Math.PI:
                    return new List<Rectangle>()
                    {
                        new Rectangle((int)drawPos.X - 20, (int)drawPos.Y - 30, 20, 20),
                        new Rectangle((int)drawPos.X, (int)drawPos.Y - 30, 20, 20),
                        new Rectangle((int)drawPos.X, (int)drawPos.Y - 10, 20, 20),
                        new Rectangle((int)drawPos.X, (int)drawPos.Y + 10, 20, 20),
                    };
                case 3 * Math.PI / 2:
                    return new List<Rectangle>()
                    {
                        new Rectangle((int)drawPos.X + 10, (int)drawPos.Y - 20, 20, 20),
                        new Rectangle((int)drawPos.X - 30, (int)drawPos.Y, 20, 20),
                        new Rectangle((int)drawPos.X - 10, (int)drawPos.Y, 20, 20),
                        new Rectangle((int)drawPos.X + 10, (int)drawPos.Y, 20, 20),
                    };
            }
            return new List<Rectangle>();
        }

        public List<Rectangle> GetOShapeRectangles(TetrisObject tetrisObject, Vector2 drawPos)
        {
            return new List<Rectangle>
            {
                new Rectangle((int)drawPos.X - 20, (int)drawPos.Y - 20, 20, 20),
                new Rectangle((int)drawPos.X - 20, (int)drawPos.Y, 20, 20),
                new Rectangle((int)drawPos.X, (int)drawPos.Y - 20, 20, 20),
                new Rectangle((int)drawPos.X, (int)drawPos.Y, 20, 20),
            };
        }

        public List<Rectangle> GetZShapeRectangles(TetrisObject tetrisObject, Vector2 drawPos)
        {
            return tetrisObject.RotationCorner == Math.PI / 2 || tetrisObject.RotationCorner == 3 * Math.PI / 2
                        ? new List<Rectangle>()
                        {
                            new Rectangle((int)drawPos.X, (int)drawPos.Y - 30, 20, 20),
                            new Rectangle((int)drawPos.X, (int)drawPos.Y - 10, 20, 20),
                            new Rectangle((int)drawPos.X - 20, (int)drawPos.Y - 10, 20, 20),
                            new Rectangle((int)drawPos.X - 20, (int)drawPos.Y + 10, 20, 20),
                        }
                        : new List<Rectangle>()
                        {
                            new Rectangle((int)drawPos.X - 30, (int)drawPos.Y - 20, 20, 20),
                            new Rectangle((int)drawPos.X - 10, (int)drawPos.Y - 20, 20, 20),
                            new Rectangle((int) drawPos.X - 10, (int)drawPos.Y, 20, 20),
                            new Rectangle((int) drawPos.X + 10, (int)drawPos.Y, 20, 20),
                        };
        }

        public List<Rectangle> GetTShapeRectangles(TetrisObject tetrisObject, Vector2 drawPos)
        {
            switch (tetrisObject.RotationCorner)
            {
                case 0:
                    return new List<Rectangle>()
                    {
                        new Rectangle((int)drawPos.X - 10, (int)drawPos.Y - 20, 20, 20),
                        new Rectangle((int)drawPos.X - 30, (int)drawPos.Y, 20, 20),
                        new Rectangle((int)drawPos.X - 10, (int)drawPos.Y, 20, 20),
                        new Rectangle((int)drawPos.X + 10, (int)drawPos.Y, 20, 20),
                     };
                case Math.PI / 2:
                    return new List<Rectangle>()
                    {
                        new Rectangle((int)drawPos.X - 20, (int)drawPos.Y - 30, 20, 20),
                        new Rectangle((int)drawPos.X - 20, (int)drawPos.Y - 10, 20, 20),
                        new Rectangle((int)drawPos.X - 20, (int)drawPos.Y + 10, 20, 20),
                        new Rectangle((int)drawPos.X, (int)drawPos.Y - 10, 20, 20),
                    };
                case Math.PI:
                    return new List<Rectangle>()
                    {
                        new Rectangle((int)drawPos.X - 10, (int)drawPos.Y, 20, 20),
                        new Rectangle((int)drawPos.X - 30, (int)drawPos.Y - 20, 20, 20),
                        new Rectangle((int)drawPos.X - 10, (int)drawPos.Y - 20, 20, 20),
                        new Rectangle((int)drawPos.X + 10, (int)drawPos.Y - 20, 20, 20),
                     };
                case 3 * Math.PI / 2:
                    return new List<Rectangle>()
                    {
                        new Rectangle((int)drawPos.X, (int)drawPos.Y - 30, 20, 20),
                        new Rectangle((int)drawPos.X, (int)drawPos.Y - 10, 20, 20),
                        new Rectangle((int)drawPos.X, (int)drawPos.Y + 10, 20, 20),
                        new Rectangle((int)drawPos.X - 20, (int)drawPos.Y - 10, 20, 20),
                    };
            }
            return new List<Rectangle>();
        }

        public List<Rectangle> GetSShapeRectangles(TetrisObject tetrisObject, Vector2 drawPos)
        {
            return tetrisObject.RotationCorner == Math.PI / 2 || tetrisObject.RotationCorner == 3 * Math.PI / 2
                        ? new List<Rectangle>()
                        {
                            new Rectangle((int)drawPos.X - 20, (int)drawPos.Y - 30, 20, 20),
                            new Rectangle((int)drawPos.X - 20, (int)drawPos.Y - 10, 20, 20),
                            new Rectangle((int)drawPos.X, (int)drawPos.Y - 10, 20, 20),
                            new Rectangle((int)drawPos.X, (int)drawPos.Y + 10, 20, 20),
                        }
                        : new List<Rectangle>()
                        {
                            new Rectangle((int)drawPos.X - 10, (int)drawPos.Y - 20, 20, 20),
                            new Rectangle((int)drawPos.X + 10, (int)drawPos.Y - 20, 20, 20),
                            new Rectangle((int)drawPos.X - 30, (int)drawPos.Y, 20, 20),
                            new Rectangle((int)drawPos.X - 10, (int)drawPos.Y, 20, 20),
                        };

        }



        #endregion
    }
}
