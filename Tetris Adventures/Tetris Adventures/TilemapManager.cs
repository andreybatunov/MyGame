using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TiledSharp;

namespace Tetris_Adventures
{
    public class TilemapManager : Tilemap
    {
        public int TilesetTilesWide { get; set; }   
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }

        public TilemapManager(TmxMap map, Texture2D tileset) 
        {
            Map = map;
            Tileset = tileset;
            TileWidth = map.Tilesets[0].TileWidth;
            TileHeight = map.Tilesets[0].TileHeight;
            TilesetTilesWide = tileset.Width / TileWidth;
            GetCollisionsObjects();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < Map.TileLayers.Count; i++) 
            {
                for (var j = 0; j < Map.TileLayers[i].Tiles.Count; j++)
                {
                    var gid = Map.TileLayers[i].Tiles[j].Gid;
                    if (gid == 0) continue;
                    else
                    {
                        var tileFrame = gid - 1;
                        var column = tileFrame % TilesetTilesWide;
                        var row = (int)Math.Floor(tileFrame / (double)TilesetTilesWide);
                        float x = j % Map.Width * Map.TileWidth;
                        float y = (float)Math.Floor(j / (double)Map.Width) * Map.TileHeight;
                        var tilesetRec = new Rectangle(TileWidth * column, TileHeight * row, TileWidth, TileHeight);
                        spriteBatch.Draw(Tileset, new Rectangle((int)x, (int)y, TileWidth, TileHeight), tilesetRec, Color.White);
                    }
                }
            }
        }

        public void GetCollisionsObjects()
        {
            CollisionObjects = new List<Rectangle>();
            foreach (var obj in Map.ObjectGroups["collisions"].Objects)
            {
                if (obj.Name == "")
                {
                    CollisionObjects.Add(new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height));
                }
                if (obj.Name == "start")
                {
                    StartPosition = new Vector2((int)obj.X, (int)obj.Y);
                }
                if (obj.Name == "death")
                {
                    DeathRectangle = new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height);
                }
            }
        }

    }
}
