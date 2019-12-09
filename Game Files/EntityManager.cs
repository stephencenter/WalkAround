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
            player = new Player(0, 0, 32, 64, "player", content);
        }
    }

    public abstract class Entity : GameObject
    {
        // Constructor
        protected Entity(int pos_x, int pos_y, int width, int height, string sprite, ContentManager content) : base(pos_x, pos_y, width, height, sprite, content) { }
    }

    public class Player : Entity
    {
        // Constructor
        public Player(int pos_x, int pos_y, int width, int height, string sprite, ContentManager content) : base(pos_x, pos_y, width, height, sprite, content) { }
    }
}
