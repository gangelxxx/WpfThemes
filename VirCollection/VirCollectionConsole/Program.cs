using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VirCollectionConsole
{
    using System;
    using System.Threading;

    //    public class ThreadSafe
    //    {
    //        private double totalValue = 0.0;
    //        public double Total { get { return totalValue; } }
    //        public double AddToTotal(double addend)
    //        {
    //            double initialValue, computedValue, res;
    //            do
    //            {
    //                initialValue = totalValue;
    //                computedValue = initialValue + addend;
    //
    //                res = Interlocked.CompareExchange(ref totalValue,
    //                    computedValue, initialValue);
    //            }
    //            while (initialValue != res);
    //        
    //            return computedValue;
    //        }
    //    }
    //
    //    public class Test
    //    {
    //        // Create an instance of the ThreadSafe class to test.
    //        private static ThreadSafe ts = new ThreadSafe();
    //        private static double control;
    //
    //        private static Random r = new Random();
    //        private static ManualResetEvent mre = new ManualResetEvent(false);
    //
    //        public static void Main()
    //        {
    //            // Create two threads, name them, and start them. The
    //            // thread will block on mre.
    //            Thread t1 = new Thread(TestThread);
    //            t1.Name = "Thread 1";
    //            t1.Start();
    ////            Thread t2 = new Thread(TestThread);
    ////            t2.Name = "Thread 2";
    ////            t2.Start();
    //
    //            // Now let the threads begin adding random numbers to 
    //            // the total.
    //            mre.Set();
    //
    //            // Wait until all the threads are done.
    //            t1.Join();
    ////            t2.Join();
    //
    //            Console.WriteLine("Thread safe: {0}  Ordinary Double: {1}",
    //                ts.Total, control);
    //
    //            Console.ReadLine();
    //        }
    //
    //        private static void TestThread()
    //        {
    //            // Wait until the signal.
    //            mre.WaitOne();
    //
    //            for (int i = 1; i <= 1000000; i++)
    //            {
    ////                double testValue = r.NextDouble();
    ////                control += testValue;
    //                ts.AddToTotal(i);
    //            }
    //        }
    //    }

    //    class Program
    //    {
    //        private static int _count = 0;
    //
    //
    //        public static async Task Foo(int num)
    //        {
    //            long count = -1;
    //
    //            Interlocked.Read(ref count);
    //            Interlocked.CompareExchange(ref _count, num, num);
    //
    //
    //
    //            // 4 5 5-4 = 1
    //            // 5 - 4 = 1
    //            //
    //            //            Console.WriteLine("Thread {0} - Start {1}", Thread.CurrentThread.ManagedThreadId, num);
    //
    //            //            await Task.Delay(1000);
    //
    //            //            Console.WriteLine("Thread {0} - End {1}", Thread.CurrentThread.ManagedThreadId, num);
    //        }
    //
    //        public static async Task Foo2(int num)
    //        {
    //            await Foo(num);
    //        }
    //
    //        public static List<Task> TaskList = new List<Task>();
    //
    //        public static void Main(string[] args)
    //        {
    //            for (int i = 1; i < 3; i++)
    //            {
    //                int idx = i;
    //                TaskList.Add(Foo2(idx));
    //            }
    //
    //            Task.WaitAll(TaskList.ToArray());
    //            Console.WriteLine("Press Enter to exit...");
    //            Console.ReadLine();
    //        }
    //    }

    //    class Program
    //    {
    //        static void Main(string[] args)
    //        {
    //            int pageSize = 10;
    //            int count = 100;
    //
    //            for (int i = 0; i < count; i++)
    //            {
    //                int pageIndex = i / pageSize;
    //                int pageOffset = i % pageSize;
    //
    //                Console.WriteLine("pageIndex:" + pageIndex + " pageOffset:" + pageOffset);
    //
    //            }
    //
    //            Console.Read();
    //        }
    //    }

    class Program
    {
        // little test console app
        static void Main(string[] args)
        {
            var list = new SearchStringList(true);
            int count = 20;
            int idx = 0;

//            for (int i = 0; i < count; i++)
//            {
//                for (int j = 0; j < count; j++)
//                {
//                    for (int z = 0; z < count; z++)
//                    {
//                        list.Add(i.ToString() + j.ToString() + z.ToString());
//                    }
//                }
//            }

            list.Add("12996");

            //            var list = new SearchStringList(true);
            //            list.Add("Now is the time");
            //            list.Add("for all good men");
            //            list.Add("Time now for something");
            //            list.Add("something completely different");

            string keyword = "12";

            foreach (var pos in list.FindAll(keyword))
            {
                Console.WriteLine(pos.ToString() + " =>" + list[pos.ListIndex] + " pos.StringIndex:" + pos.StringIndex);

                var str = list[pos.ListIndex];

                var left = str.Substring(0, pos.StringIndex); 
                var sub = str.Substring(pos.StringIndex, keyword.Length);
                var right = str.Substring(pos.StringIndex + keyword.Length);

                Console.WriteLine("left:" + left + " sub:" + sub + " right:" + right);
            }

            Console.ReadLine();

            //            while (true)
            //            {
            //                string keyword = Console.ReadLine();
            //                if (keyword.Length == 0) break;
            //                foreach (var pos in list.FindAll(keyword))
            //                {
            //                    Console.WriteLine(pos.ToString() + " =>" + list[pos.ListIndex]);
            //                }
            //            }
        }

        public class SearchStringList
        {
            private readonly List<string> _strings = new List<string>();
            private readonly List<StringPosition> _positions = new List<StringPosition>();
            private bool dirty = false;
            private readonly bool ignoreCase = true;

            public SearchStringList(bool ignoreCase)
            {
                this.ignoreCase = ignoreCase;
            }

            public void Add(string s)
            {
                if (s.Length > 255) throw new ArgumentOutOfRangeException("string too big.");
                this._strings.Add(s);
                this.dirty = true;
                for (byte i = 0; i < s.Length; i++)
                    this._positions.Add(new StringPosition(_strings.Count - 1, i));
            }

            public string this[int index] { get { return this._strings[index]; } }

            private void EnsureSorted()
            {
                if (dirty)
                {
                    this._positions.Sort(Compare);
                    this.dirty = false;
                }
            }

            public IEnumerable<StringPosition> FindAll(string keyword)
            {
                var idx = IndexOf(keyword);
                while ((idx >= 0) && (idx < this._positions.Count)
                    && (Compare(keyword, this._positions[idx]) == 0))
                {
                    yield return this._positions[idx];
                    idx++;
                }
            }

            private int IndexOf(string keyword)
            {
                EnsureSorted();

                int minP = 0;
                int maxP = this._positions.Count - 1;
                while (maxP > minP)
                {
                    int midP = minP + ((maxP - minP) / 2);
                    if (Compare(keyword, this._positions[midP]) > 0)
                    {
                        minP = midP + 1;
                    }
                    else
                    {
                        maxP = midP;
                    }
                }
                if ((maxP == minP) && (Compare(keyword, this._positions[minP]) == 0))
                {
                    return minP;
                }
                else
                {
                    return -1;
                }
            }

            private int Compare(StringPosition pos1, StringPosition pos2)
            {
                int len = Math.Max(this._strings[pos1.ListIndex].Length - pos1.StringIndex, this._strings[pos2.ListIndex].Length - pos2.StringIndex);
                return string.Compare(_strings[pos1.ListIndex], pos1.StringIndex, this._strings[pos2.ListIndex], pos2.StringIndex, len, ignoreCase);
            }

            private int Compare(string keyword, StringPosition pos2)
            {
                return string.Compare(keyword, 0, this._strings[pos2.ListIndex], pos2.StringIndex, keyword.Length, this.ignoreCase);
            }

            public struct StringPosition
            {
                public static StringPosition NotFound = new StringPosition(-1, 0);
                private readonly int _position;
                public StringPosition(int listIndex, byte stringIndex)
                {
                    this._position = (listIndex < 0) ? -1 : this._position = (listIndex << 8) | stringIndex;
                }
                public int ListIndex { get { return (this._position >= 0) ? (this._position >> 8) : -1; } }
                public byte StringIndex { get { return (byte)(this._position & 0xFF); } }
                public override string ToString()
                {
                    return ListIndex.ToString() + ":" + StringIndex;
                }
            }
        }

      
    }
    

    //        static void Main(string[] args)
    //        {
    //            List<List<int>> fullList = new List<List<int>>();
    //            int nd = 20;
    //
    //            for (int k = 0; k < nd; k++)
    //            {
    //                List<int> resList3 = new List<int>();
    //                for (int j = 0; j < nd; j++)
    //                {
    //                    resList3.Add(k);
    //                }
    //                fullList.Add(resList3);
    //
    //                for (int r = 0; r < nd; r++)
    //                {
    //                    for (int h = k + 1; h < nd; h++)
    //                    {
    //                        List<int> resList2 = new List<int>();
    //                        for (int j = 0; j < nd; j++)
    //                        {
    //                            resList2.Add(k);
    //                        }
    //
    //                        resList2[r] = h;
    //
    //                        fullList.Add(resList2);
    //                    }
    //                }
    //            }
    //
    //            Stopwatch sw = new Stopwatch();
    //            sw.Start();
    //
    //            for (int i = 0; i < fullList.Count; i++)
    //            {
    //                for (int j = i+1; j < fullList.Count; j++)
    //                {
    //                    var arr1 = fullList[i];
    //                    var arr2 = fullList[j];
    //
    //                    var p1 = GetHashCode(arr1);
    //                    var p2 = GetHashCode(arr2);
    //
    //                    if (p1 == p2)
    //                    {
    //                        Console.WriteLine("p1:" + p1 + " p2:" + p2 + " i:" + i + " j:" + j);
    //                    }
    //                }
    //
    ////                Console.WriteLine(i);
    //            }
    //
    //            sw.Stop();
    //            Console.WriteLine("ms:" + sw.ElapsedMilliseconds);
    //
    //            Console.Read();
    //        }
    //
    //        private static int GetHashCode(IReadOnlyList<int> list)
    //        {
    //            return list.Aggregate(list.Count, (current, item) => unchecked(current * 314159 + item));
    //        }
}
