using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Fody;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using XF.BindableProperty.Fody.Extensions;

public class WeaverTypes
{
    public TypeReference BindableObject { get; private set; }
    public TypeReference BindableProperty { get; private set; }
    public TypeReference BindablePropertyKey { get; private set; }

    public MethodReference Create { get; private set; }
    public MethodReference CreateAttached { get; private set; }
    public MethodReference CreateReadonly { get; private set; }
    public MethodReference CreateAttachedReadonly { get; private set; }
    public MethodReference SetValue { get; private set; }
    public MethodReference SetReadonlyValue { get; private set; }
    public MethodReference GetValue { get; private set; }
    public MethodReference GetBindablePropertyFromKey { get; private set; }

    public TypeReference BindingMode { get; private set; }
    public TypeReference ValidateValueDelegate { get; private set; }
    public TypeReference BindingPropertyChangedDelegate { get; private set; }
    public TypeReference BindingPropertyChangingDelegate { get; private set; }
    public TypeReference CoerceValueDelegate { get; private set; }
    public TypeReference CreateDefaultValueDelegate { get; private set; }

    public TypeReference Type { get; private set; }
    public TypeReference Enum { get; private set; }

    public TypeReference CompilerGeneratedAttribute { get; private set; }
    public MethodReference CompilerGeneratedAttributeConstructor { get; private set; }

    public TypeReference RuntimeTypeHandle { get; private set; }
    public MethodReference GetTypeFromHandle { get; private set; }

    public WeaverTypes(ModuleWeaver weaver)
    {
        Type = weaver.Resolve(nameof(Type));
        Enum = weaver.Resolve(nameof(Enum));

        CompilerGeneratedAttribute = weaver.Resolve(nameof(CompilerGeneratedAttribute));
        CompilerGeneratedAttributeConstructor = weaver.ModuleDefinition.ImportReference(CompilerGeneratedAttribute.Resolve().GetConstructors().Single());

        RuntimeTypeHandle = weaver.Resolve(nameof(RuntimeTypeHandle));
        GetTypeFromHandle = weaver.ModuleDefinition.ImportReference(Type.Resolve().Methods.Single(m => m.Name == nameof(System.Type.GetTypeFromHandle)));

        BindingMode = weaver.Resolve(nameof(BindingMode));
        BindableObject = weaver.Resolve(nameof(BindableObject));
        BindableProperty = weaver.Resolve(nameof(BindableProperty));
        BindablePropertyKey = weaver.Resolve(nameof(BindablePropertyKey));

        GetBindablePropertyFromKey = weaver.ModuleDefinition.ImportReference(BindablePropertyKey.Resolve().Properties.Single(m => m.Name == "BindableProperty").GetMethod);

        SetValue = weaver.ModuleDefinition.ImportReference(BindableObject.Resolve().Methods.Single(m => m.Name == "SetValue" && m.IsPublic && m.Parameters.First().ParameterType.Name == "BindableProperty"));
        SetReadonlyValue = weaver.ModuleDefinition.ImportReference(BindableObject.Resolve().Methods.Single(m => m.Name == "SetValue" && m.IsPublic && m.Parameters.First().ParameterType.Name == "BindablePropertyKey"));
        GetValue = weaver.ModuleDefinition.ImportReference(BindableObject.Resolve().Methods.Single(m => m.Name == "GetValue" && m.IsPublic && m.Parameters.First().ParameterType.Name == "BindableProperty"));

        Create = weaver.ModuleDefinition.ImportReference(BindableProperty.Resolve().Methods.Single(m => m.Name == "Create" && m.IsPublic && !m.HasGenericParameters));
        CreateAttached = weaver.ModuleDefinition.ImportReference(BindableProperty.Resolve().Methods.Single(m => m.Name == "CreateAttached" && m.IsPublic && !m.HasGenericParameters));
        CreateReadonly = weaver.ModuleDefinition.ImportReference(BindableProperty.Resolve().Methods.Single(m => m.Name == "CreateReadOnly" && m.IsPublic && !m.HasGenericParameters));
        CreateAttachedReadonly = weaver.ModuleDefinition.ImportReference(BindableProperty.Resolve().Methods.Single(m => m.Name == "CreateAttachedReadOnly" && m.IsPublic && !m.HasGenericParameters));

        ValidateValueDelegate = weaver.ModuleDefinition.ImportReference(BindableProperty.Resolve().NestedTypes.Single(t => t.Name == "ValidateValueDelegate" && !t.HasGenericParameters));
        BindingPropertyChangedDelegate = weaver.ModuleDefinition.ImportReference(BindableProperty.Resolve().NestedTypes.Single(t => t.Name == "BindingPropertyChangedDelegate" && !t.HasGenericParameters));
        BindingPropertyChangingDelegate = weaver.ModuleDefinition.ImportReference(BindableProperty.Resolve().NestedTypes.Single(t => t.Name == "BindingPropertyChangingDelegate" && !t.HasGenericParameters));
        CoerceValueDelegate = weaver.ModuleDefinition.ImportReference(BindableProperty.Resolve().NestedTypes.Single(t => t.Name == "CoerceValueDelegate" && !t.HasGenericParameters));
        CreateDefaultValueDelegate = weaver.ModuleDefinition.ImportReference(BindableProperty.Resolve().NestedTypes.Single(t => t.Name == "CreateDefaultValueDelegate" && !t.HasGenericParameters));
    }
}
