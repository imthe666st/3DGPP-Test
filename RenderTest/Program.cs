using System;

namespace RenderTest
{
    /// <summary>
    /// The main class.
    /// This is a test comment to test version control.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}
