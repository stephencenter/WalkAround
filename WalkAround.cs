using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WalkAround
{
    public class WalkAround : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch sprite_batch;
        RenderTarget2D OffScreenRenderTarget;
        Point OldWindowSize;

        // The size of a tile in pixels (e.g. tile_size of 16 gives 16x16 pixel tiles)
        public const int tile_size = 32;

        // The width and height of the screen in tiles
        public const int screen_width = 32;
        public const int screen_height = 18;
        public static float aspect_ratio = 16/9f;

        // The currently loaded map
        public static string current_map = "area1";

        // Constructor
        public WalkAround()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Logic.UpdateContent(Content);
            
            // Let the user resize their window if they want to, and also make their mouse cursor visable
            Window.AllowUserResizing = true;
            IsMouseVisible = true;
            Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
        }
        
        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            // This code is from https://stackoverflow.com/a/8396832
            // It is licensed under CC BY-SA 3.0: https://creativecommons.org/licenses/by-sa/3.0/
            Window.ClientSizeChanged -= new EventHandler<EventArgs>(Window_ClientSizeChanged);

            if (Window.ClientBounds.Width != OldWindowSize.X)
            {
                graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                graphics.PreferredBackBufferHeight = (int)(Window.ClientBounds.Width / aspect_ratio);

                graphics.ApplyChanges();
            }
            
            if (Window.ClientBounds.Height != OldWindowSize.Y)
            {
                graphics.PreferredBackBufferWidth = (int)(Window.ClientBounds.Height * aspect_ratio);
                graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;

                graphics.ApplyChanges();
            }

            OldWindowSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);
            Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
        }

        protected override void Initialize()
        {
            // Set the window to its default resolution
            graphics.PreferredBackBufferWidth = (int)(tile_size * screen_width);
            graphics.PreferredBackBufferHeight = (int)(tile_size * screen_height);
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            sprite_batch = new SpriteBatch(GraphicsDevice);
            OldWindowSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);
            OffScreenRenderTarget = new RenderTarget2D(GraphicsDevice, Window.ClientBounds.Width, Window.ClientBounds.Height);
            UnitManager.CreatePlayer();
            MapBuilder.CreateGameMap();
        }

        protected override void UnloadContent()
        {
            if (OffScreenRenderTarget != null)
                OffScreenRenderTarget.Dispose();

            if (sprite_batch != null)
                sprite_batch.Dispose();

            base.UnloadContent();
        }

        protected override void Update(GameTime game_time)
        {
            // Update the value of Logic.Content so we can use it elsewhere
            Logic.UpdateContent(Content);

            UnitManager.player.GetAction();
            Camera.UpdateCamera(UnitManager.player, graphics);
            base.Update(game_time);
        }

        protected override bool BeginDraw()
        {
            GraphicsDevice.SetRenderTarget(OffScreenRenderTarget);
            return base.BeginDraw();
        }

        protected override void Draw(GameTime game_time)
        {
            GraphicsDevice.Clear(Color.Black);
            sprite_batch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);

            // Draw all the tiles to the screen
            foreach (Tile tile in TileManager.GetTileList())
            {
                // We draw the tiles with an offset so that the camera is always centered on the player
                // regardless of where they are in space
                sprite_batch.Draw(tile.Sprite, new Vector2(tile.PosX + Camera.x_offset, tile.PosY + Camera.y_offset), Color.White);
            }

            // Then, draw the entities to the screen
            foreach (Entity entity in EntityManager.GetEntityList())
            {
                // We draw the tiles with an offset so that the camera is always centered on the player
                // regardless of where they are in space
                sprite_batch.Draw(entity.Sprite, new Vector2(entity.PosX + Camera.x_offset, entity.PosY + Camera.y_offset), Color.White);
            }

            // Finally, draw the units to the screen
            foreach (Unit unit in UnitManager.GetUnitList())
            {
                // We draw the tiles with an offset so that the camera is always centered on the player
                // regardless of where they are in space
                sprite_batch.Draw(unit.Sprite, new Vector2(unit.PosX + Camera.x_offset, unit.PosY + Camera.y_offset), Color.White);
            }

            // Draw the player to the screen, we draw it with the camera offset for the same reason as the tiles
            Player player = UnitManager.player;
            sprite_batch.Draw(player.Sprite, new Vector2(player.PosX + Camera.x_offset, player.PosY + Camera.y_offset), Color.White);

            sprite_batch.End();
            base.Draw(game_time);
        }        
        
        protected override void EndDraw()
        {
            GraphicsDevice.SetRenderTarget(null);
            sprite_batch.Begin();
            sprite_batch.Draw(OffScreenRenderTarget, GraphicsDevice.Viewport.Bounds, Color.White);
            sprite_batch.End();
            base.EndDraw();
        }
    }

    public abstract class GameObject
    {
        // The position of the top-left corner in the game-world
        public int PosX { get; set; }
        public int PosY { get; set; }

        // The width and height of the objects collision box (independent of actual sprite size)
        public int Width { get; set; }
        public int Height { get; set; }

        // The GameObject's currently-loaded sprite
        public string SpriteLoc { get; set; }
        public Texture2D Sprite { get; set; }

        // Constructor
        protected GameObject(int pos_x, int pos_y, int width, int height, string sprite)
        {
            PosX = pos_x;
            PosY = pos_y;
            Width = width;
            Height = height;
            SpriteLoc = sprite;
            Sprite = Logic.Content.Load<Texture2D>(sprite);
        }
    }

    public static class Logic
    {
        public static ContentManager Content;

        // List of valid actions, these can have multiple keys assigned to them
        public enum Actions
        {
            move_up,
            move_down,
            move_left,
            move_right,
            interact,
            attack
        }

        // Dictionary that determines which keys correspond to which actions
        public static Dictionary<Actions, List<Keys>> control_map = new Dictionary<Actions, List<Keys>>()
        {
            { Actions.move_up, new List<Keys>() { Keys.W, Keys.Up } },
            { Actions.move_down, new List<Keys>() { Keys.S, Keys.Down } },
            { Actions.move_left, new List<Keys>() { Keys.A, Keys.Left } },
            { Actions.move_right, new List<Keys>() { Keys.D, Keys.Right } },
            { Actions.interact, new List<Keys>() { Keys.K, Keys.Z } },
            { Actions.attack, new List<Keys>() { Keys.J, Keys.X } }
        };

        // List of valid directions entities can face or move in
        public enum Direction
        {
            up,
            down,
            left,
            right
        }

        // Check to see if a specific button is pressed
        public static bool IsButtonPressed(Actions action)
        {
            return control_map[action].Any(x => Keyboard.GetState().IsKeyDown(x));
        }

        // Returns a list of objects from a list that overlap with a specific object
        public static List<T> FindOverlaps<T>(GameObject object_1, IEnumerable<T> the_list, int x = 0, int y = 0)
        {
            List<T> current_objects = new List<T>();
            foreach (T object_2 in the_list)
            {
                if (DoObjectsOverlap(object_1, object_2 as GameObject, x, y))
                {
                    current_objects.Add(object_2);
                }
            }

            return current_objects;
        }

        // Determines whether 2 objects overlap. 
        // x is how much object_1 will be moved to the right
        // y is how much object_1 will be moved down
        // x and y can be used for predictive collision detection
        public static bool DoObjectsOverlap(GameObject object_1, GameObject object_2, int x = 0, int y = 0)
        {
            Vector2 tl1 = new Vector2(object_1.PosX + x, object_1.PosY + y);
            Vector2 br1 = new Vector2(object_1.PosX + object_1.Width + x, object_1.PosY + object_1.Height + y);

            Vector2 tl2 = new Vector2(object_2.PosX, object_2.PosY);
            Vector2 br2 = new Vector2(object_2.PosX + object_2.Width, object_2.PosY + object_2.Height);

            return tl1.X < br2.X && tl2.X < br1.X && tl1.Y < br2.Y && tl2.Y < br1.Y;
        }

        public static void UpdateContent(ContentManager content)
        {
            Content = content;
        }
    }
}
