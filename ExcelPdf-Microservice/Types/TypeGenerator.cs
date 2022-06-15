using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace ExcelPdf_Microservice.Types
{
    public class TypeGenerator : TypeGeneratorGeneric<object>
    {
        public TypeGenerator(Dictionary<string, Type> properties) : base(properties)
        {
        }
    }

    public class TypeGeneratorGeneric<T> where T : class
    {
        private readonly Dictionary<string, MethodInfo> _setMethods;
        //public Dictionary<string, Type> Properties;

        public TypeGeneratorGeneric(Dictionary<string, Type> properties)
        {
            Properties = properties;
            _setMethods = new Dictionary<string, MethodInfo>();
            Initialize();
        }

        private void Initialize()
        {
            var newTypeName = Guid.NewGuid().ToString();
            AssemblyName assemblyName = new AssemblyName(newTypeName);
            AssemblyBuilder dynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var dynamicModule = dynamicAssembly.DefineDynamicModule("Main");
            var dynamicType = dynamicModule.DefineType(newTypeName, TypeAttributes.Public
                                                                    | TypeAttributes.Class
                                                                    | TypeAttributes.AutoClass
                                                                    | TypeAttributes.AnsiClass
                                                                    | TypeAttributes.BeforeFieldInit
                                                                    | TypeAttributes.AutoLayout,
                                                                    typeof(T));
            dynamicType.DefineDefaultConstructor(MethodAttributes.Public
                                                    | MethodAttributes.SpecialName
                                                    | MethodAttributes.RTSpecialName);

            foreach (var property in Properties)
                AddProperty(dynamicType, property.Key, property.Value);

            GeneratedType = dynamicType.CreateType();

            foreach (var property in Properties)
            {
                var propertyInfo = GeneratedType.GetProperty(property.Key);

                var setMethod = propertyInfo.GetSetMethod();

                if (setMethod == null) continue;
                _setMethods.Add(property.Key, setMethod);
            }
        }


        public Type GeneratedType { private set; get; }

        public Dictionary<string, Type> Properties { get; }


        private void AddProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            var fieldBuilder = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
            // add property to the type with given name and signature
            var propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

            var getMethod = typeBuilder.DefineMethod("get_" + propertyName,
                                                        MethodAttributes.Public |
                                                        MethodAttributes.SpecialName |
                                                        MethodAttributes.HideBySig,
                                                        propertyType,
                                                        Type.EmptyTypes);

            var getMethodIL = getMethod.GetILGenerator();
            getMethodIL.Emit(OpCodes.Ldarg_0);
            getMethodIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getMethodIL.Emit(OpCodes.Ret);

            var setMethod = typeBuilder.DefineMethod("set_" + propertyName,
                                                       MethodAttributes.Public |
                                                       MethodAttributes.SpecialName |
                                                       MethodAttributes.HideBySig,
                                                       null, new[] { propertyType });

            var setMethodIL = setMethod.GetILGenerator();
            Label modifyProperty = setMethodIL.DefineLabel();
            Label exitSet = setMethodIL.DefineLabel();

            setMethodIL.MarkLabel(modifyProperty);
            setMethodIL.Emit(OpCodes.Ldarg_0);
            setMethodIL.Emit(OpCodes.Ldarg_1);
            setMethodIL.Emit(OpCodes.Stfld, fieldBuilder);

            setMethodIL.Emit(OpCodes.Nop);
            setMethodIL.MarkLabel(exitSet);
            setMethodIL.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getMethod);
            propertyBuilder.SetSetMethod(setMethod);

        }

        public T CreateInstance(Dictionary<string, object> values = null)
        {
            var instance = (T)Activator.CreateInstance(GeneratedType);

            if (values != null)
                SetValues(instance, values);

            return instance;
        }

        public void SetValues(T instance, Dictionary<string, object> values)
        {
            foreach (var value in values)
                SetValue(instance, value.Key, value.Value);
        }

        public void SetValue(T instance, string propertyName, object propertyValue)
        {
            if (!_setMethods.TryGetValue(propertyName, out var setter))
                throw new ArgumentException($"Type does not contain settter for property {propertyName}", nameof(propertyName));
            setter.Invoke(instance, new[] { propertyValue });
        }

        public IList CreateList(T[] values = null)
        {
            var listGenericType = typeof(List<>);
            var list = listGenericType.MakeGenericType(GeneratedType);
            var constructor = list.GetConstructor(new Type[] { });
            var newList = (IList)constructor.Invoke(new object[] { });
            foreach (var value in values)
                newList.Add(value);
            return newList;
        }
    }
}
