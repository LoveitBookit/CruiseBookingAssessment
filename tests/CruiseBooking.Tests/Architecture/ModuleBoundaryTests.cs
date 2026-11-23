using System.Reflection;

namespace CruiseBooking.Tests.Architecture;

public class ModuleBoundaryTests
{
    private const string ModulePrefix = "CruiseBooking.Modules.";
    private const string ContractsSuffix = ".Contracts";
    private const string BuildingBlocksAssemblyName = "CruiseBooking.BuildingBlocks";

    public static TheoryData<string> ModuleAssemblyNames() => ToTheoryData(FindModuleAssemblyNames());

    public static TheoryData<string> ContractsAssemblyNames() =>
        ToTheoryData(FindModuleAssemblyNames().Where(name => name.EndsWith(ContractsSuffix)));

    [Fact]
    public void AllModuleAssembliesAreDiscovered()
    {
        Assert.True(FindModuleAssemblyNames().Count >= 5, "Expected at least five module assemblies in the test output.");
    }

    [Theory]
    [MemberData(nameof(ModuleAssemblyNames))]
    public void Module_ReferencesOtherModulesOnlyThroughContracts(string assemblyName)
    {
        var module = ModuleOf(assemblyName);

        var violations = ReferencedModuleAssemblies(assemblyName)
            .Where(reference => ModuleOf(reference) != module && !reference.EndsWith(ContractsSuffix))
            .ToList();

        Assert.True(violations.Count == 0, $"{assemblyName} must not reference {string.Join(", ", violations)}. Use the module's Contracts instead.");
    }

    [Theory]
    [MemberData(nameof(ContractsAssemblyNames))]
    public void Contracts_DoNotReferenceModuleImplementations(string assemblyName)
    {
        var violations = ReferencedModuleAssemblies(assemblyName)
            .Where(reference => !reference.EndsWith(ContractsSuffix))
            .ToList();

        Assert.True(violations.Count == 0, $"{assemblyName} must not reference {string.Join(", ", violations)}.");
    }

    [Fact]
    public void BuildingBlocks_DoNotReferenceAnyModule()
    {
        var violations = ReferencedModuleAssemblies(BuildingBlocksAssemblyName);

        Assert.True(violations.Count == 0, $"{BuildingBlocksAssemblyName} must not reference {string.Join(", ", violations)}.");
    }

    private static List<string> FindModuleAssemblyNames() =>
        Directory.GetFiles(AppContext.BaseDirectory, $"{ModulePrefix}*.dll")
            .Select(Path.GetFileNameWithoutExtension)
            .OfType<string>()
            .Order()
            .ToList();

    private static List<string> ReferencedModuleAssemblies(string assemblyName) =>
        Assembly.Load(assemblyName)
            .GetReferencedAssemblies()
            .Select(reference => reference.Name)
            .OfType<string>()
            .Where(name => name.StartsWith(ModulePrefix))
            .ToList();

    private static string ModuleOf(string assemblyName) =>
        assemblyName[ModulePrefix.Length..].Split('.')[0];

    private static TheoryData<string> ToTheoryData(IEnumerable<string> names)
    {
        var data = new TheoryData<string>();
        foreach (var name in names)
        {
            data.Add(name);
        }

        return data;
    }
}
