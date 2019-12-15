using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WalkAround
{
    public static class MapBuilder
    {
        // A list of symbols usable when creating textmaps. These symbols each correspond
        // to a specific kind of tile.
        private static readonly Dictionary<char, Tile> TileCatalog = new Dictionary<char, Tile>()
        {
            { ' ', new Tile(0, 0, WalkAround.tile_size, WalkAround.tile_size, "Sprites/Tiles/void", true) },
            { 'W', new Tile(0, 0, WalkAround.tile_size, WalkAround.tile_size, "Sprites/Tiles/wall", false) },
            { 'G', new Tile(0, 0, WalkAround.tile_size, WalkAround.tile_size, "Sprites/Tiles/grass", true) },
            { 'C', new Tile(0, 0, WalkAround.tile_size, WalkAround.tile_size, "Sprites/Tiles/carpet", true) },
            { 'T', new Tile(0, 0, WalkAround.tile_size/2, WalkAround.tile_size/2, "Sprites/Tiles/test_2", false) }
        };

        private static readonly Dictionary<char, Entity> EntityCatalog = new Dictionary<char, Entity>()
        {
            { 'α', new NPC(0, 0, WalkAround.tile_size, WalkAround.tile_size, "Sprites/Entities/npc") },
            { 'β', new NPC(0, 0, WalkAround.tile_size, WalkAround.tile_size, "Sprites/Entities/npc") }
        };

        private static readonly Dictionary<char, Unit> UnitCatalog = new Dictionary<char, Unit>();

        public static Tile GetTileInfo(char symbol)
        {
            return TileCatalog[symbol];
        }

        public static Entity GetEntityInfo(char symbol)
        {
            return EntityCatalog[symbol];
        }

        public static Unit GetUnitInfo(char symbol)
        {
            return UnitCatalog[symbol];
        }

        // Get a list of textmaps for the area, convert all the textmaps into tiles,
        // then place the tiles into a list so they can be displayed on the screen
        public static void CreateGameMap()
        {
            // This is a list of file names, each file is a separate layer of the gamemap.
            // The layers are displayed alphabetically, so layer_2 is displayed above layer_1
            var files = Directory.GetFiles($"Maps/{WalkAround.current_map}", "*.txt", SearchOption.TopDirectoryOnly).ToList();
            files.Sort();

            List<List<string>> layers = new List<List<string>>();
            foreach (string file in files)
            {
                layers.Add(File.ReadAllText(file).Split('\n').ToList());
            }

            var tile_list = new List<Tile>();
            var entity_list = new List<Entity>();
            var unit_list = new List<Unit>();

            // Create all the tiles and place them in the proper location
            foreach (List<string> layer in layers)
            {
                int x = 0;
                int y = 0;

                foreach (string line in layer)
                {
                    foreach (char symbol in line.Trim())
                    {
                        if (TileCatalog.ContainsKey(symbol))
                        {
                            var tl = GetTileInfo(symbol);
                            tile_list.Add(new Tile(x, y, tl.Width, tl.Height, tl.SpriteLoc, tl.Traversable));
                        }

                        else if (EntityCatalog.ContainsKey(symbol))
                        {
                            var tl = GetEntityInfo(symbol);
                            entity_list.Add(new NPC(x, y, tl.Width, tl.Height, tl.SpriteLoc));
                        }

                        else if (UnitCatalog.ContainsKey(symbol))
                        {
                            var tl = GetUnitInfo(symbol);
                            unit_list.Add(new Player(x, y, tl.Width, tl.Height, tl.SpriteLoc, tl.MoveSpeed));
                        }

                        else
                        {
                            throw new KeyNotFoundException($"Tried to create GameObject with unmapped symbol '{symbol}'!");
                        }

                        x += WalkAround.tile_size;
                    }

                    x = 0;
                    y += WalkAround.tile_size;
                }
            }

            TileManager.UpdateTileList(tile_list);
            EntityManager.UpdateEntityList(entity_list);
            UnitManager.UpdateUnitList(unit_list);
        }
    }
}
