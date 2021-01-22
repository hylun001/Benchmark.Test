using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Validators;
using System.Linq;


namespace Benchmark.Dotnet
{
    /// <summary>
    /// 配置项
    /// </summary>
    public class AllowNonOptimized : ManualConfig
    {
        public AllowNonOptimized()
        {
            Add(JitOptimizationsValidator.DontFailOnError);

            Add(DefaultConfig.Instance.GetLoggers().ToArray());
            Add(DefaultConfig.Instance.GetExporters().ToArray());
            Add(DefaultConfig.Instance.GetColumnProviders().ToArray());
        }
    }
}
