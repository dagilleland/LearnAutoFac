using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autofac.CqrsModules.Specs.TestData
{
    interface IGenericCommandHandlerInterface<TCommand>
    {
        void Handle(TCommand command);
    }
    interface ICommandHandlerInterface
    {
        void Handle<TCommand>(TCommand command);
    }
}
