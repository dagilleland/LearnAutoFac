﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.Text;
//using System.Threading.Tasks;

//namespace Autofac.CqrsModules
//{
//    #region Autofac Modules
//    public class AutofacCommandModule : Module
//    {
//        private readonly Type _genericType;
//        private readonly System.Reflection.Assembly[] _assemblies;
//        public AutofacCommandModule(Type genericCommandHandlerInterface, params System.Reflection.Assembly[] assemblies)
//        {
//            _genericType = genericCommandHandlerInterface;
//            _assemblies = assemblies;
//        }

//        protected override void Load(ContainerBuilder builder)
//        {
//            // Register all the implementors of the generic command handler interface
//            builder.RegisterAssemblyTypes(_assemblies)
//                   .AsClosedTypesOf(_genericType);
//            // TODO: Ensure that the assemblies have only one implementation per closed type...

//            // Register the command dispatcher
//            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>();
//            // Register the resolver
//            builder.RegisterType<AutofacHandlerResolver>().As<IHandlerResolver>();
//        }
//    }
//    public class AutofacHandlerResolver : IHandlerResolver
//    {
//        private readonly IComponentContext _context;

//        public AutofacHandlerResolver(IComponentContext context)
//        {
//            _context = context;
//        }

//        public IHandleCommand<T> Resolve<T>() where T : class
//        {
//            T actual = _context.ResolveOptional<T>();
//            IHandleCommand<T> newVariable = actual as IHandleCommand<T>;
//            return newVariable;
//        }
//    }
//    public class CommandDispatcher : ICommandDispatcher
//    {
//        private readonly IHandlerResolver _resolver;

//        public CommandDispatcher(IHandlerResolver resolver)
//        {
//            _resolver = resolver;
//        }

//        public void SendCommand<TCommand>(TCommand command) where TCommand : class
//        {
//            var handler = _resolver.Resolve<TCommand>();
//            if (handler != null)
//                handler.Handle(command);
//            else
//                throw new CommandDispatchException<TCommand>(command);
//        }
//    }
//    [Serializable]
//    public class CommandDispatchException<TCommand> : Exception
//    {
//        private readonly TCommand _cmd;
//        public TCommand MessageCommand { get { return _cmd; } }
//        // constructors...
//        public CommandDispatchException(TCommand command)
//            : this("No handler registered for the command type '" + typeof(TCommand) + "'. See .MessageCommand for details.")
//        {
//            _cmd = command;
//        }

//        #region CommandDispatchException()
//        /// <summary>
//        /// Constructs a new CommandDispatchException.
//        /// </summary>
//        public CommandDispatchException() { }
//        #endregion
//        #region CommandDispatchException(string message)
//        /// <summary>
//        /// Constructs a new CommandDispatchException.
//        /// </summary>
//        /// <param name="message">The exception message</param>
//        public CommandDispatchException(string message) : base(message) { }
//        #endregion
//        #region CommandDispatchException(string message, Exception innerException)
//        /// <summary>
//        /// Constructs a new CommandDispatchException.
//        /// </summary>
//        /// <param name="message">The exception message</param>
//        /// <param name="innerException">The inner exception</param>
//        public CommandDispatchException(string message, Exception innerException) : base(message, innerException) { }
//        #endregion
//        #region CommandDispatchException(SerializationInfo info, StreamingContext context)
//        /// <summary>
//        /// Serialization constructor.
//        /// </summary>
//        protected CommandDispatchException(SerializationInfo info, StreamingContext context) : base(info, context) { }
//        #endregion
//    }
//    public interface IHandlerResolver<TGeneric<T>, T>
//    {
//        TGeneric<T> Resolve<T>() where T : class;
//    }
//    public interface ICommandDispatcher
//    {
//        void SendCommand<TCommand>(TCommand command) where TCommand : class;
//    }
//    #endregion
//}
