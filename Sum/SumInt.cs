using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

using System.Numerics;

namespace Sum;

[RPlotExporter]
[SimpleJob(RuntimeMoniker.Net70, id: ".Net 07")]
[SimpleJob(RuntimeMoniker.Net80, id: ".Net 08")]
[SimpleJob(RuntimeMoniker.Net90, id: ".Net 09")]
[SimpleJob(RuntimeMoniker.Net10_0, id: ".Net 10")]
public class SumInt
{
    private const int N = 10000;
    private readonly byte[] source;
    private readonly int[] data;
    public SumInt()
    {
        source = new byte[N];
        new Random(1).NextBytes(source);
        data = new int[N];
        Array.Copy(source, data, N);
    }

    [Benchmark]
    public int LinqSum() => data.Sum();

    [Benchmark(Baseline = true)]
    public int ArraySum()
    {
        int result = 0;

        for (int i = 0; i < data.Length; i++) result += data[i];

        return result;
    }

    [Benchmark]
    public int SpanSum()
    {
        int result = 0;
        var span = data.AsSpan();

        for (int i = 0; i < span.Length; i++) result += span[i];

        return result;
    }

    [Benchmark]
    public int UnrolledSum()
    {
        //number of unrolling
        const int SIZE = 4;

        int result = 0;
        var span = data.AsSpan();
        int lastBlockIndex = data.Length - (data.Length % SIZE);

        for (int i = 0; i < lastBlockIndex; i += 4)
        {
            result += span[i + 0];
            result += span[i + 1];
            result += span[i + 2];
            result += span[i + 3];
        }

        for (int i = lastBlockIndex; i < span.Length; i++) result += span[i];

        return result;
    }

    [Benchmark]
    public int VectorizedSum()
    {
        //number of calculators per CPU core in haswell+ architecture
        const int SIZE = 4;

        int result = 0;
        int partial1 = 0;
        int partial2 = 0;
        int partial3 = 0;
        int partial4 = 0;

        int sliceLenght = data.Length / SIZE;

        var span = data.AsSpan();
        var slice1 = span[..sliceLenght];
        var slice2 = span.Slice(sliceLenght, sliceLenght);
        var slice3 = span.Slice(sliceLenght * 2, sliceLenght);
        var slice4 = span.Slice(sliceLenght * 3, sliceLenght);

        for (int i = 0; i < sliceLenght; i++)
        {
            partial1 += slice1[i];
            partial2 += slice2[i];
            partial3 += slice3[i];
            partial4 += slice4[i];
        }

        result += partial1;
        result += partial2;
        result += partial3;
        result += partial4;

        int lastBlockIndex = data.Length - (data.Length % SIZE);
        for (int i = lastBlockIndex; i < span.Length; i++) result += span[i];

        return result;
    }

    [Benchmark]
    public int SimdSum()
    {
        int result = 0;
        int length = data.Length;
        var span = data.AsSpan();
        int simdLength = Vector<int>.Count;

        int i = 0;
        Vector<int> vResult = Vector<int>.Zero;

        for (; i <= length - simdLength; i += simdLength) vResult += new Vector<int>(span.Slice(i, simdLength));

        for (int j = 0; j < simdLength; j++) result += vResult[j];

        for (; i < length; i++) result += data[i];

        return result;
    }
}
