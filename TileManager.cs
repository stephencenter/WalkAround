using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WalkAround
{
    public static class TileManager
    {
        private static List<Tile> tile_list;

        // Return a list of all currently-loaded tiles
        public static List<Tile> GetTileList()
        {
            return tile_list;
        }

        // Update the list of currently-loaded tiles
        public static void UpdateTileList(List<Tile> list)
        {
            tile_list = list;
        }
    }

    public class Tile : GameObject
    {
        public bool Traversable { get; set; }

        public Tile(int pos_x, int pos_y, int width, int height, string sprite, bool traversable) :
            base(pos_x, pos_y, width, height, sprite)
        {
            Traversable = traversable;
        }
    }
}
