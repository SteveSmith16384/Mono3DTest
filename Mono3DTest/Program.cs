using System;

namespace Mono3DTest
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Test3D.Test3DDemo())
                game.Run();
        }
    }
}
