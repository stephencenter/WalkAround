using System;

namespace WalkAround
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new WalkAround())
                game.Run();
        }
    }
}
