using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Cecil.Cil;
using Fody;
using System;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Diagnostics;

public partial class ModuleWeaver : BaseModuleWeaver
{

    public override bool ShouldCleanReference => true;

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "netstandard";
        yield return "mscorlib";
        yield return "Xamarin.Forms.Core";
    }

    public override void Execute()
    {
        Types = new WeaverTypes(this);
        var properties = CollectProperties().ToArray();

        foreach (var property in properties)
            property.Weave();
    }
}