using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncIenumApp
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            await foreach (var dataPoint in Data())
            {
                Console.WriteLine(dataPoint);
            }

            Console.ReadLine();
        }

        static async IAsyncEnumerable<int> Data()
        {
            for (int i = 1; i <= 10; i++)
            {
                await Task.Delay(1000);//Simulate waiting for data to come through. 
                yield return i;
            }
        }
    }
}
