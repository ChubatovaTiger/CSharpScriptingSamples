using System;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.IO;
using Microsoft.CodeAnalysis.Scripting;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Sample03
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var scriptSourceCode = File.ReadAllText("script.csx");
            var scriptOptions = ScriptOptions.Default
                .WithImports("System");

            // without diagnostics 
            // var scriptResult = await CSharpScript.RunAsync(scriptSourceCode, scriptOptions);
            
            // or with it
            var script = CSharpScript.Create(scriptSourceCode, scriptOptions);
            var diagnostics = script.GetCompilation().GetDiagnostics();
            var scriptResult = await script.RunAsync();

            Console.WriteLine(scriptResult.ReturnValue);
        }
    }
}