using System.Collections.Generic;

namespace WalkAround
{
    public static class TileBuilder
    {
        private static readonly Dictionary<char, TileInfo> TileCatalog = new Dictionary<char, TileInfo>()
        {
            { ' ', new TileInfo(WalkAround.tile_size, WalkAround.tile_size, "Sprites/Tiles/void", false) },
            { 'x', new TileInfo(WalkAround.tile_size, WalkAround.tile_size, "Sprites/Tiles/test_tile", true) },
            { 'Æ', new TileInfo(WalkAround.tile_size/2, WalkAround.tile_size/2, "Sprites/Tiles/test_2", false) }
        };

        public static TileInfo GetTileInfo(char symbol)
        {
            return TileCatalog[symbol];
        }
    }

    public class TileInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Sprite { get; set; }
        public bool Traversable { get; set; }

        public TileInfo(int width, int height, string sprite, bool traversable)
        {
            Width = width;
            Height = height;
            Sprite = sprite;
            Traversable = traversable;
        }
    }
}
