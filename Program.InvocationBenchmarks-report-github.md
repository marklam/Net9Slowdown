```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4541/23H2/2023Update/SunValley3)
13th Gen Intel Core i9-13900KS, 1 CPU, 32 logical and 24 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2 DEBUG
  Job-AQCHLG : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  Job-JFKRXR : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX2


```
| Method                      | Runtime  | N     | Mean         | Error      | StdDev     | Ratio | RatioSD | Code Size |
|---------------------------- |--------- |------ |-------------:|-----------:|-----------:|------:|--------:|----------:|
| **InlineScalar**                | **.NET 8.0** | **10**    |     **2.436 ns** |  **0.0383 ns** |  **0.0339 ns** |  **1.00** |    **0.02** |      **52 B** |
| SimdFuncsInterface          | .NET 8.0 | 10    |    14.809 ns |  0.2845 ns |  0.2922 ns |  6.08 |    0.14 |     606 B |
| SimdFuncsInterfaceViaInline | .NET 8.0 | 10    |     2.363 ns |  0.0479 ns |  0.0400 ns |  0.97 |    0.02 |        NA |
| InlineScalar                | .NET 9.0 | 10    |     2.178 ns |  0.0304 ns |  0.0254 ns |  0.89 |    0.02 |      52 B |
| SimdFuncsInterface          | .NET 9.0 | 10    |    16.603 ns |  0.3424 ns |  0.3663 ns |  6.82 |    0.17 |     174 B |
| SimdFuncsInterfaceViaInline | .NET 9.0 | 10    |     4.905 ns |  0.0370 ns |  0.0328 ns |  2.01 |    0.03 |        NA |
|                             |          |       |              |            |            |       |         |           |
| **InlineScalar**                | **.NET 8.0** | **100**   |    **26.552 ns** |  **0.2567 ns** |  **0.2143 ns** |  **1.00** |    **0.01** |      **52 B** |
| SimdFuncsInterface          | .NET 8.0 | 100   |    51.692 ns |  0.5077 ns |  0.4239 ns |  1.95 |    0.02 |     606 B |
| SimdFuncsInterfaceViaInline | .NET 8.0 | 100   |     6.847 ns |  0.1604 ns |  0.2196 ns |  0.26 |    0.01 |        NA |
| InlineScalar                | .NET 9.0 | 100   |    28.064 ns |  0.3575 ns |  0.3169 ns |  1.06 |    0.01 |      52 B |
| SimdFuncsInterface          | .NET 9.0 | 100   |    71.868 ns |  1.0512 ns |  0.8778 ns |  2.71 |    0.04 |     174 B |
| SimdFuncsInterfaceViaInline | .NET 9.0 | 100   |    54.275 ns |  0.8074 ns |  0.7157 ns |  2.04 |    0.03 |        NA |
|                             |          |       |              |            |            |       |         |           |
| **InlineScalar**                | **.NET 8.0** | **10000** | **2,296.555 ns** | **33.1912 ns** | **29.4231 ns** |  **1.00** |    **0.02** |      **52 B** |
| SimdFuncsInterface          | .NET 8.0 | 10000 | 3,167.623 ns | 14.8353 ns | 12.3882 ns |  1.38 |    0.02 |     354 B |
| SimdFuncsInterfaceViaInline | .NET 8.0 | 10000 |   385.700 ns |  3.9845 ns |  3.5321 ns |  0.17 |    0.00 |        NA |
| InlineScalar                | .NET 9.0 | 10000 | 2,310.529 ns | 25.7017 ns | 22.7839 ns |  1.01 |    0.02 |      52 B |
| SimdFuncsInterface          | .NET 9.0 | 10000 | 6,537.173 ns | 55.0947 ns | 48.8400 ns |  2.85 |    0.04 |     174 B |
| SimdFuncsInterfaceViaInline | .NET 9.0 | 10000 | 6,180.175 ns | 34.4980 ns | 30.5816 ns |  2.69 |    0.04 |        NA |
