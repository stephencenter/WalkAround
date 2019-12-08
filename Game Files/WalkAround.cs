using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WalkAround
{
    public class WalkAround : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch sprite_batch;
        private SpriteBatch font_batch;

        // Global Variables
        public static float scaling_factor = 1.0f;
        public const int tile_size = 32;
        public const int screen_width = 32;
        public const int screen_height = 18;

        public static int camera_x = 0;
        public static int camera_y = 0;

        // Constructor
        public WalkAround()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // Let the user resize their window if they want to, and also make their mouse cursor visable
            Window.AllowUserResizing = true;
            IsMouseVisible = true;

            // Set the window to its default resolution
            graphics.PreferredBackBufferWidth = (int)(tile_size * screen_width * scaling_factor);
            graphics.PreferredBackBufferHeight = (int)(tile_size * screen_height * scaling_factor);
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            sprite_batch = new SpriteBatch(GraphicsDevice);
            font_batch = new SpriteBatch(GraphicsDevice);
            TileManager.CreateGameMap(Content);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime game_time)
        {
            // Update the scaling_factor if the player resizes the window.
            // This makes it so that the same amount of space is visible regardless of 
            // resolution or resizing
            float sf1 = (float)(graphics.PreferredBackBufferWidth) / (tile_size * screen_width);
            float sf2 = (float)(graphics.PreferredBackBufferHeight) / (tile_size * screen_height);
            scaling_factor = Math.Min(sf1, sf2);

            if (Logic.IsButtonPressed(Logic.Actions.move_up))
            {
                camera_y += 4;
            }

            if (Logic.IsButtonPressed(Logic.Actions.move_down))
            {
                camera_y -= 4;
            }

            if (Logic.IsButtonPressed(Logic.Actions.move_left))
            {
                camera_x += 4;
            }

            if (Logic.IsButtonPressed(Logic.Actions.move_right))
            {
                camera_x -= 4;
            }

            base.Update(game_time);
        }

        protected override void Draw(GameTime game_time)
        {
            GraphicsDevice.Clear(Color.Black);
            sprite_batch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(scaling_factor));

            foreach (Tile tile in TileManager.GetTileList())
            {
                sprite_batch.Draw(tile.Sprite, new Vector2(tile.PosX + camera_x, tile.PosY + camera_y), Color.White);
            }

            sprite_batch.End();

            base.Draw(game_time);
        }
    }

    public class GameObject
    {
        // The position of the top-left corner in the game-world
        public int PosX;
        public int PosY;

        // The width and height of the objects collision box (independent of actual sprite size)
        public int Width;
        public int Height;

        // The GameObject's currently-loaded sprite
        public Texture2D Sprite;
    }

    public class Logic
    {
        // List of valid actions, these can have multiple keys assigned to them
        public enum Actions
        {
            move_up,
            move_down,
            move_left,
            move_right
        }

        // Dictionary that determines which keys correspond to which actions
        public static Dictionary<Actions, List<Keys>> control_map = new Dictionary<Actions, List<Keys>>()
        {
            { Actions.move_up, new List<Keys>() { Keys.W, Keys.Up } },
            { Actions.move_down, new List<Keys>() { Keys.S, Keys.Down } },
            { Actions.move_left, new List<Keys>() { Keys.A, Keys.Left } },
            { Actions.move_right, new List<Keys>() { Keys.D, Keys.Right } },
        };

        // List of valid directions entities can face or move in
        public enum Direction
        {
            up,
            down,
            left,
            right
        }

        public static bool IsButtonPressed(Actions action)
        {
            // Check to see if a specific button is pressed
            return control_map[action].Any(x => Keyboard.GetState().IsKeyDown(x));
        }

        public static List<GameObject> FindOverlapsFromList(GameObject object_1, IEnumerable<GameObject> the_list)
        {
            List<GameObject> current_objects = new List<GameObject>();
            foreach (GameObject object_2 in the_list)
            {
                if (DoObjectsOverlap(object_1, object_2))
                {
                    current_objects.Add(object_2);
                }
            }

            return current_objects;
        }

        public static bool DoObjectsOverlap(GameObject object_1, GameObject object_2)
        {
            Vector2 tl1 = new Vector2(object_1.PosX, object_1.PosY);
            Vector2 br1 = new Vector2(object_1.PosX + object_1.Width, object_1.PosY + object_1.Height);

            Vector2 tl2 = new Vector2(object_2.PosX, object_2.PosY);
            Vector2 br2 = new Vector2(object_2.PosX + object_2.Width, object_2.PosY + object_2.Height);

            return tl1.X < br2.X && tl2.X < br1.X && tl1.Y < br2.Y && tl2.Y < br1.Y;
        }
    }
}
