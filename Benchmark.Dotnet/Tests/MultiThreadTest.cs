using BenchmarkDotNet.Attributes;
using System.Threading.Tasks;

namespace Benchmark.Dotnet
{
    [MemoryDiagnoser]
    [Config(typeof(AllowNonOptimized))]
    public class MultiThreadTest
    {

        /// <summary>
        /// Better
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public Task<string> GetTaskAsync1()
        {
            return Task.Factory.StartNew(() =>
            {
                return "111";
            });
        }

        [Benchmark]
        public ValueTask<string> GetValueTaskAsync1()
        {
            return new ValueTask<string>(Task.Factory.StartNew(() =>
            {
                return "111";
            }));
        }


        [Benchmark]
        public async Task<string> Get1()
        {
            return "111";
        }

        /// <summary>
        /// Better
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public ValueTask<string> Get2()
        {
            return new ValueTask<string>("111");
        }

    }
}
