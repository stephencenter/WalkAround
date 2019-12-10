using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalkAround
{
    public static class EntityManager
    {
        public static Player player;

        public static void CreatePlayer(ContentManager content)
        {
            player = new Player(0, 0, 32, 64, "Sprites/Entities/player", 4, content);
        }
    }

    public abstract class Entity : GameObject
    {
        public int MoveSpeed { get; set; }
        public Logic.Direction FacingDirection { get; set; }

        // Constructor
        protected Entity(int pos_x, int pos_y, int width, int height, string sprite, int speed, ContentManager content) : base(pos_x, pos_y, width, height, sprite, content)
        {
            MoveSpeed = speed;
        }

        public abstract void Move();

        public bool PredictCollision(int x_delta, int y_delta)
        {
            return Logic.FindOverlaps(this, TileManager.GetTileList(), x_delta, y_delta).Select(x => x as Tile).Any(x => !x.Traversable);
        }
    }

    public class Player : Entity
    {
        // Constructor
        public Player(int pos_x, int pos_y, int width, int height, string sprite, int speed, ContentManager content) : base(pos_x, pos_y, width, height, sprite, speed, content) { }

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
