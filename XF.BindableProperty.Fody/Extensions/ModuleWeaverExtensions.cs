using Fody;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace XF.BindableProperty.Fody.Extensions
{
    public static class ModuleWeaverExtensions
    {
        public static TypeReference Resolve(this ModuleWeaver weaver, string typename)
        {
            return weaver.ModuleDefinition.ImportReference(weaver.FindTypeDefinition(typename)
                ?? throw new WeavingException($"Couldn't find {typename}!"));
        }
    }
}
