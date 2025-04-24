using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ExecutionTests
    {
        [Fact]
        public void ConcurentExecution() 
        {
            var numbers = new List<int>{1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20 };
            Stopwatch sw = new Stopwatch();

            sw.Start();
            var result = TestClass.GetPrimeNumbersConcurrent(numbers);
            sw.Stop();

            Debug.WriteLine("Single Thread: Elapsed={0}", sw.Elapsed.TotalSeconds);
           
        }

        [Fact]
        public void ConcurentExecutionParallel()
        {
            // var numbers = Enumerable.Range(0, 1000).ToList();
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            Stopwatch sw = new Stopwatch();

            sw.Start();
            var result = TestClass.GetPrimeNumbersParallel(numbers);
            sw.Stop();

            Debug.WriteLine("Multiple Thread :Elapsed={0}", sw.Elapsed.TotalSeconds);

        }
    }

    public static class TestClass
    {
        /// <summary>
        /// Simple execution of loop where code is running on 
        /// single thread of application
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static ConcurrentDictionary<int, int> GetPrimeNumbersConcurrent(IList<int> numbers)
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
        public static ConcurrentDictionary<int, int> GetPrimeNumbersParallel(IList<int> numbers)
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
}
