# Sum benchmarks
This is an experiment exploring performance and optimization across different versions of .NET using a simple sum operation.
BenchmarkDotNet is used for timing measurements and visual plots.

<img width="2099" height="2099" alt="Sum SumInt-barplot" src="https://github.com/user-attachments/assets/e103196c-dab9-4a21-b3fd-9679b573c516" />

1. Array: Basic For loop over an array
2. LINQ: Sum LINQ's function on the array
3. SIMD: For loop over a Vector
4. Span: For loop over an array but as a span
5. Unrolled: For loop performing 4 additions per iteration
6. Vectorized: For loop slicing the data into 4 segments processed independently

# Commentary
Unexpectedly, the For loop over an array is the slowest way of summing an array of numbers.

Using a Span in place of the array help performance, and since .NET 9 it become as performant as manual unrolling.

Manual vectorization give another boost in performance but the best optimization possible without using unsafe code is the Vector class as it give us an easy access to the hardware intrinsics (SIMD).

Notably, since .NET 8, the LINQ's Sum function have performance comparable to the manual usage of SIMD.

# Raw Results
```
12th Gen Intel Core i3-12100F 3.30GHz, 1 CPU, 8 logical and 4 physical cores
```
| Method        | Runtime   | Mean       | Error    | StdDev   | Ratio |
|-------------- |---------- |-----------:|---------:|---------:|------:|
| LinqSum       | .NET 7.0  | 3,236.2 ns | 36.51 ns | 34.15 ns |  0.98 |
| ArraySum      | .NET 7.0  | 3,300.7 ns | 50.53 ns | 47.26 ns |  1.00 |
| SpanSum       | .NET 7.0  | 3,066.6 ns |  9.31 ns |  8.25 ns |  0.93 |
| UnrolledSum   | .NET 7.0  | 2,496.6 ns | 19.48 ns | 17.27 ns |  0.76 |
| VectorizedSum | .NET 7.0  | 1,213.4 ns |  3.06 ns |  2.56 ns |  0.37 |
| SimdSum       | .NET 7.0  |   473.9 ns |  2.31 ns |  2.04 ns |  0.14 |
|               |           |            |          |          |       |
| LinqSum       | .NET 8.0  |   697.1 ns | 12.87 ns | 12.04 ns |  0.21 |
| ArraySum      | .NET 8.0  | 3,268.6 ns | 19.14 ns | 14.94 ns |  1.00 |
| SpanSum       | .NET 8.0  | 3,075.3 ns | 22.43 ns | 19.89 ns |  0.94 |
| UnrolledSum   | .NET 8.0  | 2,461.6 ns | 12.50 ns | 11.08 ns |  0.75 |
| VectorizedSum | .NET 8.0  | 1,214.7 ns |  1.78 ns |  1.49 ns |  0.37 |
| SimdSum       | .NET 8.0  |   559.0 ns | 11.04 ns | 21.79 ns |  0.17 |
|               |           |            |          |          |       |
| LinqSum       | .NET 9.0  |   742.3 ns |  4.04 ns |  3.38 ns |  0.22 |
| ArraySum      | .NET 9.0  | 3,449.4 ns | 18.05 ns | 15.07 ns |  1.00 |
| SpanSum       | .NET 9.0  | 2,468.9 ns | 12.75 ns |  9.96 ns |  0.72 |
| UnrolledSum   | .NET 9.0  | 2,462.9 ns | 13.62 ns | 11.37 ns |  0.71 |
| VectorizedSum | .NET 9.0  | 1,224.2 ns |  9.40 ns |  8.33 ns |  0.35 |
| SimdSum       | .NET 9.0  |   517.6 ns |  4.10 ns |  3.64 ns |  0.15 |
|               |           |            |          |          |       |
| LinqSum       | .NET 10.0 |   733.6 ns |  3.98 ns |  3.53 ns |  0.21 |
| ArraySum      | .NET 10.0 | 3,484.3 ns | 51.31 ns | 48.00 ns |  1.00 |
| SpanSum       | .NET 10.0 | 2,463.7 ns |  2.86 ns |  2.23 ns |  0.71 |
| UnrolledSum   | .NET 10.0 | 2,451.2 ns |  7.82 ns |  6.11 ns |  0.70 |
| VectorizedSum | .NET 10.0 |   898.1 ns | 13.68 ns | 12.12 ns |  0.26 |
| SimdSum       | .NET 10.0 |   509.3 ns |  1.18 ns |  0.92 ns |  0.15 |
