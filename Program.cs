using System;
using System.Threading;

namespace FractalViewer
{
    class Program
    {
        private static Window window = new Window();
        public static Window GetWindow() => window;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Thread cmdThread = new Thread(new ThreadStart(CommandThread));
            //cmdThread.Start();

            using (window)
            {
                Input.Instantiate(window);
                window.Run();
            }
        }

        static void CommandThread()
        {
            while (true) {
                string input = Console.ReadLine();
                string[] splitInput = input.Split(' ');
                switch (input[0])
                {
                    case 'N':
                        if(float.TryParse(splitInput[1], out float result))
                        {
                            window.juliaWindow.NPower = result;
                            window.fractal.NPower = result;
                        }

                        break;
                }
            }
        }
    }
}
