using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ch19_P12_PLINQDataProcessingWithCancellation
{
    class Program
    {
        static CancellationTokenSource cancelToken = new CancellationTokenSource();
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Start any key to start processing");
                Console.ReadKey();
                Console.WriteLine("Processing");
                Task.Factory.StartNew(() => ProcessIntData());
                Console.Write("Enter Q to quit: ");
                string answer = Console.ReadLine();
                // Does user want to quit?
                if (answer.Equals("Q", StringComparison.OrdinalIgnoreCase))
                {
                    cancelToken.Cancel();
                    break;
                }
            } while (true);
            Console.ReadLine();

            //Console.WriteLine("Start any key to start processing");
            //Console.ReadKey();
            //Console.WriteLine("Processing");
            ////Task.Factory.StartNew(() => ProcessIntData());
            //ProcessIntData();
            //Console.ReadLine();
        }

        static void ProcessIntData()
        {
            Stopwatch stopwatch = new Stopwatch();
            // Get a very large array of integers.
            int[] source = Enumerable.Range(1, 10_000_000).ToArray();

            stopwatch.Start();

            ////Find the numbers where num % 3 == 0 is true, returned
            //// in descending order.
            //int[] modThreeIsZero = (from num in source
            //                        where num % 3 == 0
            //                        orderby num descending
            //                        select num).ToArray();

            int[] modThreeIsZero = (from num in source.AsParallel().WithCancellation(cancelToken.Token)
                                    where num % 3 == 0
                                    orderby num descending
                                    select num).ToArray();
            stopwatch.Stop();
            Console.WriteLine($"Found { modThreeIsZero.Count()} numbers that match query!");
            Console.WriteLine($"Time in miliseconds : {stopwatch.ElapsedMilliseconds}");
        }
    }
}
