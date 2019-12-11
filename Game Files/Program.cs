using System;

namespace WalkAround
{
#if WINDOWS || LINUX
    // The main class.
    public static class Program
    {
        // The main entry point for the application.
        [STAThread]
        private static void Main()
        {
            using (var game = new WalkAround())
            {
                game.Run();
            }
        }
    }
#endif
}
