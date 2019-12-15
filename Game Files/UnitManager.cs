using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Linq;
using System.Collections.Generic;

namespace WalkAround
{
    public static class UnitManager
    {
        private static List<Unit> unit_list;

        public static Player player;

        // Return a list of all currently-loaded units
        public static List<Unit> GetUnitList()
        {
            return unit_list;
        }

        // Update the list of currently-loaded units
        public static void UpdateUnitList(List<Unit> list)
        {
            unit_list = list;
        }

        public static void CreatePlayer()
        {
            player = new Player(32, 32, 32, 64, "Sprites/Units/player", 4);
        }
    }

    // Units are objects that can participate in combat.
    // This includes the player, party members, and monsters
    public abstract class Unit : GameObject
    {
        public int MoveSpeed { get; set; }
        public Logic.Direction FacingDirection { get; set; }

        // Constructor
        protected Unit(int pos_x, int pos_y, int width, int height, string sprite, int speed) :
            base(pos_x, pos_y, width, height, sprite)
        {
            MoveSpeed = speed;
        }

        // Called once per frame, tells the entity how to move
        public abstract void Move();

        // Returns true if moving to the point (PosX + x_delta, PosY + y_delta)
        // would put the entity inside a non-traversable collision box
        public bool PredictCollision(int x_delta, int y_delta)
        {
            return Logic.FindOverlaps(this, TileManager.GetTileList(), x_delta, y_delta).Select(x => x as Tile).Any(x => !x.Traversable);
        }
    }

    public class Player : Unit
    {
        // Constructor
        public Player(int pos_x, int pos_y, int width, int height, string sprite, int speed) :
            base(pos_x, pos_y, width, height, sprite, speed) { }

        public override void Move()
        {
            if (Logic.IsButtonPressed(Logic.Actions.move_up) && !PredictCollision(0, -MoveSpeed))
            {
                PosY -= MoveSpeed;
                FacingDirection = Logic.Direction.up;
            }

            if (Logic.IsButtonPressed(Logic.Actions.move_down) && !PredictCollision(0, MoveSpeed))
            {
                PosY += MoveSpeed;
                FacingDirection = Logic.Direction.down;
            }

            if (Logic.IsButtonPressed(Logic.Actions.move_left) && !PredictCollision(-MoveSpeed, 0))
            {
                PosX -= MoveSpeed;
                FacingDirection = Logic.Direction.left;
            }

            if (Logic.IsButtonPressed(Logic.Actions.move_right) && !PredictCollision(MoveSpeed, 0))
            {
                PosX += MoveSpeed;
                FacingDirection = Logic.Direction.right;
            }
        }
    }
}
