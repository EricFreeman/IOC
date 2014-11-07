using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IocContainer
{
    public static class Ioc
    {
        #region Properties

        private static Dictionary<Type, Component> objectDictionary = new Dictionary<Type, Component>();

        #endregion

        #region Register

        public static void Register<T, R>(LifeStyleType lifeStyle = LifeStyleType.Transient)
        {
            if (!objectDictionary.ContainsKey(typeof(T)))
            {
                Component c = new Component(typeof(R), lifeStyle);
                objectDictionary.Add(typeof(T), c);
            }
        }

        #endregion

        #region Resolve

        public static object Resolve(Type fromType)
        {
            if (objectDictionary.ContainsKey(fromType))
                return GetImplementationFromComponent(objectDictionary[fromType]);
            
            throw new Exception(string.Format("Cannot find implementation for {0}!", fromType.Name));
        }

        public static T Resolve<T>()
        {
            return (T)Resolve(typeof (T));
        }

        private static object GetImplementationFromComponent(Component component)
        {
            if (component.ImplementationLifestyle == LifeStyleType.Singleton)
            {
                return component.ImplementationInstance ?? (component.ImplementationInstance = CreateObject(component));
            }

            return CreateObject(component);
        }

        private static object CreateObject(Component component)
        {
            var paramsValues = new List<object>();
            foreach (var construct in component.ImplementationType.GetConstructors())
            {
                foreach (ParameterInfo param in construct.GetParameters())
                {
                    Type paramType = param.ParameterType;
                    if (paramType.IsInterface)
                    {
                        paramsValues.Add(Ioc.Resolve(paramType));
                    }
                }

                if (paramsValues.Count() == construct.GetParameters().Count())
                    break;

                paramsValues.Clear();
            }
            return Activator.CreateInstance(component.ImplementationType, paramsValues.ToArray());
        }

        #endregion

        #region Helper Methods

        public static void ClearRegistry()
        {
            objectDictionary.Clear();
        }

        #endregion
    }
}