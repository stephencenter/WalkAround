using System.Collections.Generic;

namespace WalkAround
{
    public static class EntityManager
    {
        private static List<Entity> entity_list;

        // Get a list of currently-loaded entities
        public static List<Entity> GetEntityList()
        {
            return entity_list;
        }

        // Update the list of currently-loaded entities
        public static void UpdateEntityList(List<Entity> list)
        {
            entity_list = list;
        }
    }

    // Entities are objects in the game world you can interact with.
    // This includes NPCs, levers, pushable blocks, etc.
    public abstract class Entity : GameObject
    {
        public abstract void DoInteraction();

        protected Entity(int pos_x, int pos_y, int width, int height, string sprite) :
            base(pos_x, pos_y, width, height, sprite)
        {

        }
    }

    public class NPC : Entity
    {
        public override void DoInteraction()
        {
            System.Console.WriteLine("Talk talk talk!");
        }

        // Constructor
        public NPC(int pos_x, int pos_y, int width, int height, string sprite) :
            base(pos_x, pos_y, width, height, sprite)
        {

        }
    }
}
