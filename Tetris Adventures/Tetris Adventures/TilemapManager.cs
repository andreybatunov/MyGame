using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TiledSharp;

namespace Tetris_Adventures
{
    public class TilemapManager
    {
        TmxMap map;
        Texture2D tileset;
        int tilesetTilesWide;
        int tileWidth;
        int tileHeight;

        public TilemapManager(TmxMap map, Texture2D tileset, int tilesetTilesWide, int tileWidth, int tileHeight) 
        {
            this.map = map;
            this.tileset = tileset;
            this.tilesetTilesWide = tilesetTilesWide;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < map.TileLayers.Count; i++) 
            {
                for (var j = 0; j < map.TileLayers[i].Tiles.Count; j++)
                {
                    var gid = map.TileLayers[i].Tiles[j].Gid;
                    if (gid == 0) continue;
                    else
                    {
                        var tileFrame = gid - 1;
                        var column = tileFrame % tilesetTilesWide;
                        var row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);
                        float x = (j % map.Width) * map.TileWidth;
                        float y = (float)Math.Floor(j / (double)map.Width) * map.TileHeight;
                        var tilesetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);
                        spriteBatch.Draw(tileset, new Rectangle((int)x, (int)y, tileWidth, tileHeight), tilesetRec, Color.White);
                    }
                }
            }
        }

    }
}
