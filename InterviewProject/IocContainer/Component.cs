using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocContainer
{
    public class Component
    {
        #region Properties

        public Type ImplementationType { get; set; }

        public object ImplementationInstance { get; set; }

        public LifeStyleType ImplementationLifestyle { get; set; }

        #endregion

        #region Constructors

        public Component(Type type, LifeStyleType lifeStyle)
        {
            ImplementationType = type;
            ImplementationLifestyle = lifeStyle;
        }

        #endregion
    }
}