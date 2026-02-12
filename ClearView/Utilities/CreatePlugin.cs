using log4net.Plugin;
using System.Reflection;
using System.Runtime.Loader;

namespace EPIC.ClearView.Utilities
{

    public static class CreatePlugin
    {
        public static IPlugin Compile(string sourceCode, string pluginName)
        {
            string dllPath = Path.Combine(AppContext.BaseDirectory, $"{pluginName}.dll");

            // 1. COMPILE (The "Cache" Generation)
            if (!File.Exists(dllPath))
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

                // Collect references (Crucial: the plugin needs to know about System and our Interface)
                var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(IPlugin).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location)
            };

                var compilation = CSharpCompilation.Create(pluginName)
                    .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                    .AddReferences(references)
                    .AddSyntaxTrees(syntaxTree);

                var result = compilation.Emit(dllPath);

                if (!result.Success)
                {
                    var failures = string.Join("\n", result.Diagnostics.Where(d => d.IsWarningAsError || d.Severity == DiagnosticSeverity.Error));
                    throw new Exception($"Compilation failed:\n{failures}");
                }
            }

            // 2. LOAD (The Plugin Injection)
            var loadContext = new AssemblyLoadContext(pluginName, isCollectible: true);
            var assembly = loadContext.LoadFromAssemblyPath(dllPath);

            var type = assembly.GetTypes().FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t));
            return (IPlugin)Activator.CreateInstance(type)!;
        }
    }
}
