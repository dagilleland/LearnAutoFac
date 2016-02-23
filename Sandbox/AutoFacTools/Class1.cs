// Adapted from
// http://www.ramblingincode.com/building-a-command-pipeline-with-autofac/

using Autofac;
using Autofac.Core;
using Autofac.Features;
using Autofac.Builder;
using Autofac.Util;
using I_Teach.SchoolSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacTools
{
    //public class CommandModule<InterfaceResolver> : Module
    //public class CommandModule : Module
    //{
    //    private readonly Type _genericType;
    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="CommandModule"/> class.
    //    /// </summary>
    //    /// <param name="genericType"></param>
    //    public CommandModule(Type genericType)
    //    {
    //        //if(genericType.isg
    //        _genericType = genericType;
    //    }
    //    protected override void Load(ContainerBuilder builder)
    //    {
    //        // Load the assembly containing the command handlers
    //        var assembly = this.GetType().Assembly;//System.Reflection.Assembly.Load("CommandHandlers");

    //        // Scan the assembly and register keyed services
    //        builder.RegisterAssemblyTypes(assembly)
    //               //.As(o => o.GetInterfaces().Where(i => i.IsClosedTypeOf(typeof(IHandleCommand<>)))
    //               .As(o => o.GetInterfaces().Where(i => i.IsClosedTypeOf(_genericType))
    //                                         .Select(i => new KeyedService("Handler", i)));

    //        //// Decorate handlers with loggers
    //        //builder.RegisterGenericDecorator(typeof(Logger<>),
    //        //                                 typeof(IHandler<>),
    //        //                                 "Handler", "Logger");

    //        // Register the handler resolver
    //        builder.RegisterType<AutofacHandlerResolver>()
    //               .As<IHandlerResolver>();
    //        //builder.RegisterType<AutofacHandlerResolver>()
    //        //       .As<InterfaceResolver>();

    //        // Register the command dispatcher
    //        builder.RegisterType<CommandDispatcher>()
    //               .As<ICommandDispatcher>();
    //    }
    //}
    //public class AutofacCommandDispatcher : ICommandDispatcher
    //{
    //    private readonly IComponentContext _context;

    //    public AutofacCommandDispatcher(IComponentContext context)
    //    {
    //        _context = context;
    //    }

    //    public void Dispatch(ICommand command)
    //    {
    //        var handlerType = typeof(IHandleCommand<>).MakeGenericType(command.GetType());

    //        dynamic handler = _context.Resolve(handlerType);
    //        handler.Handle((dynamic)command);
    //    }
    //}
}
