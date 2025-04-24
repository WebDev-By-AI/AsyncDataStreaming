using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelLoops
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = Enumerable.Range(0, 100).ToList();
            var result = GetPrimeNumbersParallel(numbers);
            foreach (var number in result)
            {
                Console.WriteLine($"Prime Number:{ string.Format("{0:0000}", number.Key)}, Managed Thread Id: { number.Value}");
            }
            Console.Read();

            //await foreach (var person in DB.GetData())
            //{
            //    Console.WriteLine(person.Id + " - "+ person.Name);
            //}

        }

        /// <summary>
        /// Simple execution of loop where code is running on 
        /// single thread of application
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        static ConcurrentDictionary<int,int> GetPrimeNumbersConcurrent(IList<int> numbers)
        {
            var primes = new ConcurrentDictionary<int, int>();
            foreach (var number in numbers)
            {
                if (IsPrime(number))
                {
                    primes.TryAdd(number,
                    Thread.CurrentThread.ManagedThreadId);
                }
            }
            return primes;
        }

        /// <summary>
        /// Parallel execution of code on diffrent cores of processor
        /// Diffrent threads are execuiting code
        /// Concurrent Dcitionary is responsible for thread safe dictionary operations
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        static ConcurrentDictionary<int, int> GetPrimeNumbersParallel(IList<int> numbers)
        {
            var primes = new ConcurrentDictionary<int, int>();
            Parallel.ForEach(numbers, number =>
            {
                if (IsPrime(number))
                {
                    primes.TryAdd(number,
                    Thread.CurrentThread.ManagedThreadId);
                }
            });
            return primes;
        }
        static bool IsPrime(int integer)
        {
            if (integer <= 1) return false;
            if (integer == 2) return true;
            var limit = Math.Ceiling(Math.Sqrt(integer));
            for (int i = 2; i <= limit; ++i)
                if (integer % i == 0)
                    return false;
            return true;
        }
    }


    #region Igonore it
    //public static class DB
    //{
    //    public static async IAsyncEnumerable<Person> GetData()
    //    {
    //        for (int i = 1; i <= 10; i++)
    //        {
    //            await Task.Delay(1000);//Simulate waiting for data to come through. 
    //            yield return new Person { Id=i, Name=i.ToString() };
    //        }
    //    }
    //}


    //public class Person 
    //{
    //   public int Id { get; set; }
    //    public string Name { get; set; }
    //}
    #endregion
}
