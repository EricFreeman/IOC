using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public static T CreateModel<T>()
        {
            return (T)CreateObject(typeof (T));
        }

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
                return component.ImplementationInstance ?? (component.ImplementationInstance = CreateObject(component.ImplementationType));
            }

            return CreateObject(component.ImplementationType);
        }

        private static object CreateObject(Type implementationType)
        {
            var paramsValues = new List<object>();
            foreach (var construct in implementationType.GetConstructors())
            {
                foreach (ParameterInfo param in construct.GetParameters())
                {
                    var paramType = param.ParameterType;
                    if (paramType.IsInterface) paramsValues.Add(Resolve(paramType));
                }

                if (paramsValues.Count() == construct.GetParameters().Count())
                    break;

                paramsValues.Clear();
            }
            return Activator.CreateInstance(implementationType, paramsValues.ToArray());
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