using System;

namespace FractalViewer
{
    class Program
    {
        static int factorial(int i)
        {
            int product = 1;
            for (int j = 2; j <= i; j++)
            {
                product *= j;
            }
            return product;
        }
        static float pow(float a, int b)
        {
            float product = 1;
            for (int i = 0; i < b; i++)
            {
                product *= a;
            }
            return product;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            float a = 1.25f;
            float b = -2f;

            double newA = 0;
            double newB = 0;

            int power = 2;
            for (int k = 0; k <= power; k++)
            {
                double aSum = 0;
                double bSum = 0;
                int multiplier = 1;
                for (int i = 0; i < k; i++)
                {
                    if(i % 2 != 0)
                        multiplier *= -1;
                }

                if (k % 2 == 0)
                    aSum = factorial(power) / (factorial(k) * factorial(power - k)) * pow(a, power - k) * pow(b, k);
                else
                    bSum = factorial(power) / (factorial(k) * factorial(power - k)) * pow(a, power - k) * pow(b, k);
                newA += aSum * multiplier;
                newB += bSum * multiplier;
                Console.WriteLine(aSum * multiplier);
                Console.WriteLine(bSum * multiplier);
            }

            Console.WriteLine();
            Console.WriteLine(newA);
            Console.WriteLine(newB);

            using (Window window = new Window())
            {
                Input.Instantiate(window);
                window.Run();
            }
        }
    }
}
