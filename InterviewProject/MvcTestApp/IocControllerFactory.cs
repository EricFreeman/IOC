using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTestApp
{
    public class IocControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                throw new HttpException(404, "Page not found: " + requestContext.HttpContext.Request.Path);

            if (!typeof(IController).IsAssignableFrom(controllerType))
                throw new ArgumentException("Type does not subclass IController", "controllerType");

            object[] parameters = null;

            ConstructorInfo constructor = controllerType.GetConstructors().FirstOrDefault(c => c.GetParameters().Length > 0);
            if (constructor != null)
            {
                ParameterInfo[] parametersInfo = constructor.GetParameters();
                parameters = new object[parametersInfo.Length];

                for (int i = 0; i < parametersInfo.Length; i++)
                {
                    ParameterInfo p = parametersInfo[i];

                    parameters[i] = IocContainer.IocContainer.Resolve(p.ParameterType);
                }
            }

            try
            {
                return (IController)Activator.CreateInstance(controllerType, parameters);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(String.Format("Error creating controller: {0}", ex.ToString()));
            }
        }
    }
}