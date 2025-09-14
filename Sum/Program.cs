using BenchmarkDotNet.Running;

namespace Sum;

class Program
{
    static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

    //IConfig config = DefaultConfig.Instance.WithSummaryStyle(SummaryStyle.Default.WithTimeUnit(TimeUnit.Microsecond))
    //    .AddExporter(RPlotExporter.Default)
    //    .AddJob(Job.Default.WithRuntime(CoreRuntime.Core70).WithId(".Net 07"))
    //    .AddJob(Job.Default.WithRuntime(CoreRuntime.Core80).WithId(".Net 08"))
    //    .AddJob(Job.Default.WithRuntime(CoreRuntime.Core90).WithId(".Net 09"))
    //    .AddJob(Job.Default.WithRuntime(CoreRuntime.Core10_0).WithId(".Net 10"));
    //BenchmarkRunner.Run<SumInt>(config);
}