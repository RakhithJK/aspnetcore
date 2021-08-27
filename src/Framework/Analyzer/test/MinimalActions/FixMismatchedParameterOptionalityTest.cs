// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.AspNetCore.Analyzer.Testing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;
using VerifyCS = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.CodeFixVerifier<
    Microsoft.AspNetCore.Analyzers.DelegateEndpoints.DelegateEndpointAnalyzer,
    Microsoft.AspNetCore.Analyzers.DelegateEndpoints.DelegateEndpointFixer>;

namespace Microsoft.AspNetCore.Analyzers.DelegateEndpoints;

public partial class FixMismatchedParameterOptionalityTest
{
    private TestDiagnosticAnalyzerRunner Runner { get; } = new(new DelegateEndpointAnalyzer());

    [Fact]
    public async Task MatchingRequiredOptionality_CanBeFixed()
    {
        // Console.WriteLine($"Waiting for debugger to attach for {System.Environment.ProcessId}");
        // while (!System.Diagnostics.Debugger.IsAttached)
        // {
        //     Thread.Sleep(100);
        // }
        // Console.WriteLine("Debugger attached");
        var source = @"
using Microsoft.AspNetCore.Builder;

class Program
{
    static void Main(string[] args)
    {
        var app = WebApplication.Create();
        app.MapGet(""/hello/{name?}"", (string name) => $""Hello {name}"");
    }
}
";
        var fixedSource = @"
using Microsoft.AspNetCore.Builder;

class Program
{
    static void Main(string[] args)
    {
        var app = WebApplication.Create();
        app.MapGet(""/hello/{name?}"", (string? name) => $""Hello {name}"");
    }
}
";
        var item = typeof(Microsoft.AspNetCore.Builder.WebApplication).Assembly.Location;
        var item2 = typeof(Microsoft.AspNetCore.Builder.DelegateEndpointRouteBuilderExtensions).Assembly.Location;
        var item3 = typeof(Microsoft.AspNetCore.Builder.IApplicationBuilder).Assembly.Location;
        var item4 = typeof(Microsoft.AspNetCore.Builder.IEndpointConventionBuilder).Assembly.Location;
        var item5 = typeof(Microsoft.Extensions.Hosting.IHost).Assembly.Location;
        var item6 = typeof(Microsoft.AspNetCore.Mvc.ModelBinding.IBinderTypeProviderMetadata).Assembly.Location;
        var item7 = typeof(Microsoft.AspNetCore.Mvc.BindAttribute).Assembly.Location;

        var csharpTest = new CSharpCodeFixTest<
            Microsoft.AspNetCore.Analyzers.DelegateEndpoints.DelegateEndpointAnalyzer,
            Microsoft.AspNetCore.Analyzers.DelegateEndpoints.DelegateEndpointFixer,
            XUnitVerifier>
        {
            TestState = {
                Sources = { source },
                ExpectedDiagnostics = {
                    new DiagnosticResult(DiagnosticDescriptors.DetectMismatchedParameterOptionality).WithArguments("name").WithSpan(9, 9, 9, 71)
                }
            },
            FixedState = {
                InheritanceMode = StateInheritanceMode.AutoInherit,
                Sources = { fixedSource },
                ExpectedDiagnostics = { }
            }
        };

        csharpTest.ReferenceAssemblies = ReferenceAssemblies.Net.Net60.AddAssemblies(ImmutableArray.Create(
            item.Replace(".dll", string.Empty),
            item2.Replace(".dll", string.Empty),
            item3.Replace(".dll", string.Empty),
            item4.Replace(".dll", string.Empty),
            item5.Replace(".dll", string.Empty),
            item6.Replace(".dll", string.Empty),
            item7.Replace(".dll", string.Empty)));

        await csharpTest.RunAsync();
    }

    [Fact]
    public async Task MatchingMultipleRequiredOptionality_CanBeFixed()
    {
        // Console.WriteLine($"Waiting for debugger to attach for {System.Environment.ProcessId}");
        // while (!System.Diagnostics.Debugger.IsAttached)
        // {
        //     Thread.Sleep(100);
        // }
        // Console.WriteLine("Debugger attached");
        var source = @"
using Microsoft.AspNetCore.Builder;

class Program
{
    static void Main(string[] args)
    {
        var app = WebApplication.Create();
        app.MapGet(""/hello/{name?}/{title?}"", (string name, string title) => $""Hello {name}, you are a {title}."");
    }
}
";
        var fixedSource = @"
using Microsoft.AspNetCore.Builder;

class Program
{
    static void Main(string[] args)
    {
        var app = WebApplication.Create();
        app.MapGet(""/hello/{name?}/{title?}"", (string? name, string? title) => $""Hello {name}, you are a {title}."");
    }
}
";
        var item = typeof(Microsoft.AspNetCore.Builder.WebApplication).Assembly.Location;
        var item2 = typeof(Microsoft.AspNetCore.Builder.DelegateEndpointRouteBuilderExtensions).Assembly.Location;
        var item3 = typeof(Microsoft.AspNetCore.Builder.IApplicationBuilder).Assembly.Location;
        var item4 = typeof(Microsoft.AspNetCore.Builder.IEndpointConventionBuilder).Assembly.Location;
        var item5 = typeof(Microsoft.Extensions.Hosting.IHost).Assembly.Location;
        var item6 = typeof(Microsoft.AspNetCore.Mvc.ModelBinding.IBinderTypeProviderMetadata).Assembly.Location;
        var item7 = typeof(Microsoft.AspNetCore.Mvc.BindAttribute).Assembly.Location;

        var csharpTest = new CSharpCodeFixTest<
            Microsoft.AspNetCore.Analyzers.DelegateEndpoints.DelegateEndpointAnalyzer,
            Microsoft.AspNetCore.Analyzers.DelegateEndpoints.DelegateEndpointFixer,
            XUnitVerifier>
        {
            TestState = {
                Sources = { source },
                ExpectedDiagnostics = {
                    new DiagnosticResult(DiagnosticDescriptors.DetectMismatchedParameterOptionality).WithArguments("name").WithSpan(9, 9, 9, 71)
                }
            },
            FixedState = {
                InheritanceMode = StateInheritanceMode.AutoInherit,
                Sources = { fixedSource },
                ExpectedDiagnostics = { }
            }
        };

        csharpTest.ReferenceAssemblies = ReferenceAssemblies.Net.Net60.AddAssemblies(ImmutableArray.Create(
            item.Replace(".dll", string.Empty),
            item2.Replace(".dll", string.Empty),
            item3.Replace(".dll", string.Empty),
            item4.Replace(".dll", string.Empty),
            item5.Replace(".dll", string.Empty),
            item6.Replace(".dll", string.Empty),
            item7.Replace(".dll", string.Empty)));

        await csharpTest.RunAsync();
    }
}
