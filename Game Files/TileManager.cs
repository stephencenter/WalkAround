using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

            List<string> files = Directory.GetFiles($"Maps/{WalkAround.current_map}", "*.txt", SearchOption.TopDirectoryOnly).ToList();
            List<List<string>> layers = new List<List<string>>();
            layers.Sort();

            foreach (string file in files)
            {
                layers.Add(File.ReadAllText(file).Split('\n').ToList());
            }

            foreach (List<string> layer in layers)
            {
                int x = 0;
                int y = 0;

                foreach (string line in layer)
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
