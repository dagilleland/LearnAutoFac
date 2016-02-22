using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autofac.CqrsModules.Specs.TestData
{
    #region Command Objects
    public class DoThis
    {
    }
    public class DoThat
    {
    }
    public class DoTheOtherThing
    {
    }
    public class DoNothing
    {
    }
    public class DoBetter
    {
    }
    public class DoMoreWithLess
    {
    }
    #endregion

    #region CommandHandlers
    public class DoOneThingWell
        : IGenericCommandHandlerInterface<DoBetter>
    {
        public void Handle(DoBetter command)
        {
        }
    }

    public class DoLotsOfStuff
        : IGenericCommandHandlerInterface<DoThis>
        , IGenericCommandHandlerInterface<DoThat>
        , IGenericCommandHandlerInterface<DoTheOtherThing>
    {
        public void Handle(DoThis command)
        {
        }

        public void Handle(DoThat command)
        {
        }

        public void Handle(DoTheOtherThing command)
        {
        }
    }
    #endregion

}
