using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalkAround
{
    public static class TileManager
    {
        private static readonly List<Tile> tile_list = new List<Tile>();

        public static List<Tile> GetTileList()
        {
            return tile_list;
        }

        public static void CreateGameMap(ContentManager content)
        {
            int x = 0;
            int y = 0;

            List<string> text_map = File.ReadAllText("text_map.txt").Split('\n').ToList();

            foreach (string line in text_map)
            {
                foreach (char symbol in line.Trim())
                {
                    var t_info = TileBuilder.GetTileInfo(symbol);
                    tile_list.Add(new Tile(x, y, t_info.Width, t_info.Height, t_info.Sprite, t_info.Traversable, content));

                    x += WalkAround.tile_size;
                }

                x = 0;
                y += WalkAround.tile_size;
            }
        }
    }

    public class Tile : GameObject
    {
        public bool Traversable;
        public bool IsRecoveryPoint;

        public Tile(int pos_x, int pos_y, int width, int height, string sprite, bool traversable, ContentManager content) : base(pos_x, pos_y, width, height, sprite, content)
        {
            Traversable = traversable;
        }
    }
}
