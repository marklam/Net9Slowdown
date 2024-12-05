open System
open System.Numerics
open System.Runtime.CompilerServices
open System.Runtime.InteropServices
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

module Simd =
    let inline mapInPlaceInline (data:Span<'T>, [<InlineIfLambda>] mapV : Vector<'T> -> Vector<'T>, [<InlineIfLambda>] mapS : 'T -> 'T) =
        let vectored =
            if Vector.IsHardwareAccelerated then
                let vecData = MemoryMarshal.Cast<'T, Vector<'T>>(data)
                for i = 0 to vecData.Length - 1 do
                    vecData.[i] <- mapV vecData.[i]

                vecData.Length * Vector<'T>.Count
            else
                0

        for i = vectored to data.Length - 1 do
            data[i] <- mapS data[i]

    let mapInPlace (data:Span<'T>, mapV : Vector<'T> -> Vector<'T>, mapS : 'T -> 'T) =
        let vectored =
            if Vector.IsHardwareAccelerated then
                let vecData = MemoryMarshal.Cast<'T, Vector<'T>>(data)
                for i = 0 to vecData.Length - 1 do
                    vecData.[i] <- mapV vecData.[i]

                vecData.Length * Vector<'T>.Count
            else
                0

        for i = vectored to data.Length - 1 do
            data[i] <- mapS data[i]


type IVectorFuncs<'T> =
    interface
        abstract InvokeV : Vector<'T> -> Vector<'T>
        abstract InvokeS : 'T -> 'T
    end

type IFunc<'T, 'TRes when 'T : struct and 'TRes : struct> =
    interface
        abstract Invoke : 'T -> 'TRes
    end

type [<Struct>] IntFuncS =
    interface IFunc<int,int> with
        member _.Invoke (x : int) = x - 1

type [<Struct>] IntFuncV =
    interface IFunc<Vector<int>,Vector<int>> with
        member _.Invoke (x : Vector<int>) = x - Vector 1

type [<Struct>] TransformFuncsImpl (dummy:int) =
    interface IVectorFuncs<int> with
        [<MethodImpl(MethodImplOptions.AggressiveInlining)>] member _.InvokeS (x : int) = x - 1
        [<MethodImpl(MethodImplOptions.AggressiveInlining)>] member _.InvokeV (x : Vector<int>) = x - Vector 1

[<DisassemblyDiagnoser(printSource = true, maxDepth = 3,exportCombinedDisassemblyReport=true, exportDiff=true)>]
type InvocationBenchmarks() =
    let mutable values = [| |]

    [<Params(10, 100, 10_000); DefaultValue>]
    val mutable N : int

    [<GlobalSetup>]
    member this.GlobalSetup () =
        let r = System.Random(1234)
        values <- Array.init this.N (fun _ -> r.Next())

    [<Benchmark(Baseline=true)>]
    member _.InlineScalar() =
        let values = Span(values)
        let lambdaS = fun x -> x - 1
        for i = 0 to values.Length - 1 do
            values.[i] <- lambdaS values.[i]

    [<Benchmark>]
        member _.SimdFuncsInterface() =
            let values = Span(values)
            let lambdas = TransformFuncsImpl(3) :> IVectorFuncs<int>
            Simd.mapInPlace (values, lambdas.InvokeV, lambdas.InvokeS)

    [<Benchmark>]
        member _.SimdFuncsInterfaceViaInline() =
            let values = Span(values)
            let lambdas = TransformFuncsImpl(3) :> IVectorFuncs<int>
            Simd.mapInPlaceInline (values, lambdas.InvokeV, lambdas.InvokeS)

// dotnet run -c Release -f net8.0 --filter "*" --runtimes net8.0 net9.0
module Program =
    [<EntryPoint>]
    let main args =
        BenchmarkSwitcher.FromAssembly(typeof<InvocationBenchmarks>.Assembly).Run(args) |> ignore
        0