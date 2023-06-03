using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tetris_Adventures.Objects;
using TiledSharp;

namespace Tetris_Adventures.Managers
{
    public class TilemapManager : Tilemap
    {
        public List<TmxMap> Maps { get; set; }
        public int TilesetTilesWide { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int Level = 0;
        public List<Color> Colors = new()
        {
            Color.Aquamarine,
            Color.CadetBlue,
            Color.SteelBlue,
            Color.MidnightBlue,
            Color.DimGray,
        };
        public TilemapManager(List<TmxMap> maps, Texture2D tileset, Texture2D finish)
        {
            Maps = maps;
            Tileset = tileset;
            Finish = finish;
            InitializeTiles();
            GetCollisionsObjects();
        }

        public void InitializeTiles()
        {
            TileWidth = Maps[Level].Tilesets[0].TileWidth;
            TileHeight = Maps[Level].Tilesets[0].TileHeight;
            TilesetTilesWide = Tileset.Width / TileWidth;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < Maps[Level].TileLayers.Count; i++)
            {
                for (var j = 0; j < Maps[Level].TileLayers[i].Tiles.Count; j++)
                {
                    var gid = Maps[Level].TileLayers[i].Tiles[j].Gid;
                    if (gid == 0) continue;
                    var tileFrame = gid - 1;
                    var column = tileFrame % TilesetTilesWide;
                    var row = (int)Math.Floor(tileFrame / (double)TilesetTilesWide);
                    var x = j % Maps[Level].Width * Maps[Level].TileWidth;
                    var y = (float)Math.Floor(j / (double)Maps[Level].Width) * Maps[Level].TileHeight;
                    var tilesetRec = new Rectangle(TileWidth * column, TileHeight * row, TileWidth, TileHeight);
                    spriteBatch.Draw(Tileset, new Rectangle((int)x, (int)y, TileWidth, TileHeight), tilesetRec, Color.White);
                }
            }
            spriteBatch.Draw(Finish, new Vector2(FinishRectangle.X, FinishRectangle.Y), new Rectangle(0, 0, 50, 50), Color.White, 0f, new Vector2(25,25), 1.25f, SpriteEffects.None, 0f);
        }

        public void GetCollisionsObjects()
        {
            CollisionObjects = new List<Rectangle>();
            DeathRectangles = new List<Rectangle>();
            SurfaceRectangles = new List<Rectangle>();
            foreach (var obj in Maps[Level].ObjectGroups["collisions"].Objects)
            {
                var rectangle = new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height);
                switch (obj.Name)
                {
                    case "":
                        CollisionObjects.Add(rectangle);
                        break;
                    case "start":
                        StartPosition = new Vector2((int)obj.X, (int)obj.Y);
                        break;
                    case "finish":
                        FinishRectangle = rectangle;
                        break;
                    case "death":
                        DeathRectangles.Add(rectangle);
                        CollisionObjects.Add(rectangle);
                        break;
                    case "surface":
                        SurfaceRectangles.Add(rectangle);
                        break;
                }
            }
        }
    }
}
