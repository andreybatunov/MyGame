using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.Linq;
using Tetris_Adventures.Objects;

namespace Tetris_Adventures.Managers
{
    public class TetrisManager
    {
        private const int delay = 200;
        private const int setObjectDelay = 5000;
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
        public TetrisObject CurrentTetrisObject { get; set; }
        public Texture2D TetrisSpriteSheet { get; set; }

        private List<TetrisObject> drawnFigures;
        private Vector2 currentMousePosition;
        private TilemapManager mapManager;
        private readonly Dictionary<Keys, TetrisFigure> keyboardKeys = new()
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
        private double rotateTimeCheck;
        private double setObjectTimeCheck;
        private Vector2 drawPos;
        private Player player;
        private List<Rectangle> environmentTetrisSquares;
        

        public TetrisManager(Texture2D tetrisSprite, TilemapManager tilemapManager, Player player)
        {
            TetrisSpriteSheet = tetrisSprite;
            mapManager = tilemapManager;
            CurrentTetrisObject = new TetrisObject(TetrisFigure.None, new Vector2(), new Rectangle());
            drawnFigures = new List<TetrisObject>();
            environmentTetrisSquares = new List<Rectangle>();
            this.player = player;
        }

        public void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            var keyboard = Keyboard.GetState();
            currentMousePosition.X = mouse.X;
            currentMousePosition.Y = mouse.Y;
            drawPos.X = CurrentTetrisObject.Width % 2 == 0
                ? currentMousePosition.X - currentMousePosition.X % 20
                : currentMousePosition.X - currentMousePosition.X % 20 - 10;
            drawPos.Y = CurrentTetrisObject.Height % 2 == 0
                ? currentMousePosition.Y - currentMousePosition.Y % 20
                : currentMousePosition.Y - currentMousePosition.Y % 20 - 10;
            CurrentTetrisObject.CanBeSetted = CanCurrentObjectBeSetted(gameTime);
            GetHandleInput(keyboard, mouse, gameTime);
            CheckDelayStatus(gameTime);
        }

        public void GetHandleInput(KeyboardState keyboard, MouseState mouse, GameTime gameTime)
        {
            foreach (var keyboardKey in keyboardKeys)
            {
                if (keyboard.IsKeyDown(keyboardKey.Key))
                    ChangeCurrentTetrisFigure(keyboardKey.Value, gameTime);
            }
            if (keyboard.IsKeyDown(Keys.C))
                ClearMap();
            if (mouse.LeftButton == ButtonState.Pressed
                && CurrentTetrisObject.Figure != TetrisFigure.None
                && gameTime.TotalGameTime.TotalMilliseconds - setObjectTimeCheck > delay)
            {
                var possibleObject = GetCollisionTetrisObject(CurrentTetrisObject, drawPos);
                if (CurrentTetrisObject.CanBeSetted)
                {
                    AddTetrisObject(possibleObject);
                    setObjectTimeCheck = gameTime.TotalGameTime.TotalMilliseconds;
                    DelaysDictionary[CurrentTetrisObject.Figure] = (setObjectTimeCheck, false);
                    
                }
            }
            if (mouse.RightButton == ButtonState.Pressed
                && CurrentTetrisObject.Figure != TetrisFigure.None
                && gameTime.TotalGameTime.TotalMilliseconds - rotateTimeCheck > delay)
            {
                CurrentTetrisObject.Rotate();
                rotateTimeCheck = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        public List<Rectangle> GetCollisionTetrisObject(TetrisObject tetrisObject, Vector2 drawPos)
        {
            return tetrisObject.Figure switch
            {
                TetrisFigure.IShape => GetIShapeRectangles(tetrisObject, drawPos),
                TetrisFigure.JShape => GetJShapeRectangles(tetrisObject, drawPos),
                TetrisFigure.LShape => GetLShapeRectangles(tetrisObject, drawPos),
                TetrisFigure.OShape => GetOShapeRectangles(tetrisObject, drawPos),
                TetrisFigure.ZShape => GetZShapeRectangles(tetrisObject, drawPos),
                TetrisFigure.TShape => GetTShapeRectangles(tetrisObject, drawPos),
                TetrisFigure.SShape => GetSShapeRectangles(tetrisObject, drawPos),
                _ => new List<Rectangle>(),
            };
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
            foreach (var tetrisObject in drawnFigures)
            {
                spriteBatch.Draw(TetrisSpriteSheet, tetrisObject.Position, Shapes[tetrisObject.Figure], Color.White, (float)tetrisObject.RotationCorner, tetrisObject.Origin, 1f, SpriteEffects.None, 0f);
            } 
            if (CurrentTetrisObject.Figure != TetrisFigure.None)
            {
                spriteBatch.Draw(TetrisSpriteSheet,
                    drawPos,
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

        public bool CanCurrentObjectBeSetted(GameTime gameTime) => !IsGroundOfOtherFiguresIntersected() 
            && !IsPlayerHitboxIntersected() 
            && IsTouchingOtherFigureOrSurface() 
            && IsDelayPassed(gameTime, CurrentTetrisObject.Figure);

        public bool IsPlayerHitboxIntersected()
        {
            var squares = GetCollisionTetrisObject(CurrentTetrisObject, drawPos);
            foreach (var square in squares)
            {
                if (player.Hitbox.Intersects(square)) 
                    return true;
            }
            return false;
        }

        public bool IsGroundOfOtherFiguresIntersected()
        {
            var squares = GetCollisionTetrisObject(CurrentTetrisObject, drawPos);
            foreach (var square in squares)
            {
                foreach (var obj in mapManager.CollisionObjects)
                {
                    if (square.Intersects(obj)) return true;
                }
            }
            return false;
        }

        public bool IsTouchingOtherFigureOrSurface()
        {
            var squares = GetCollisionTetrisObject(CurrentTetrisObject, drawPos);
            return squares.Intersect(environmentTetrisSquares).Count() > 0 || IsTouchingSurface();
        }

        public bool IsTouchingSurface()
        {
            var squares = GetCollisionTetrisObject(CurrentTetrisObject, drawPos);
            foreach (var square in squares)
            {
                foreach (var obj in mapManager.SurfaceRectangles)
                {
                    if (square.Intersects(obj)) return true;
                }
            }
            return false;
        }

        public bool IsDelayPassed(GameTime gameTime, TetrisFigure tetrisFigure) 
        {
           if (gameTime.TotalGameTime.TotalMilliseconds - DelaysDictionary[tetrisFigure].Item1 > setObjectDelay)
            {
                DelaysDictionary[tetrisFigure] = (0, true);
            }
           return DelaysDictionary[tetrisFigure].Item2;
        }

        public void CheckDelayStatus(GameTime gameTime)
        {
            foreach(var obj in DelaysDictionary)
            {
                if (IsDelayPassed(gameTime, obj.Key))
                {
                    DelaysDictionary[obj.Key] = (0, true);
                }
            }
        }

        public void ResetDelays()
        {
            foreach (var obj in DelaysDictionary)
            {
                    DelaysDictionary[obj.Key] = (0, true);
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
            mapManager.CollisionObjects.RemoveRange(mapManager.CollisionObjects.Count - drawnFigures.Count * 4, drawnFigures.Count * 4);
            mapManager.SurfaceRectangles.RemoveRange(mapManager.SurfaceRectangles.Count - drawnFigures.Count * 4, drawnFigures.Count * 4);
            drawnFigures.Clear();
            environmentTetrisSquares.Clear();
            ResetDelays();
        }

        public void AddTetrisObject(List<Rectangle> possibleObject)
        {
            mapManager.CollisionObjects.AddRange(possibleObject);
            mapManager.SurfaceRectangles.AddRange(possibleObject);
            drawnFigures.Add(new TetrisObject(CurrentTetrisObject, drawPos));
            environmentTetrisSquares.AddRange(GetEnvironmentSquares(possibleObject));
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
