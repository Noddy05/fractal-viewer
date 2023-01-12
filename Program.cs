using System;

namespace Fractal_Viewer
{
    class Program
    {
        private static Window window;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (Window window = new Window())
            {
                window.Run();
            }
        }
    }
}
