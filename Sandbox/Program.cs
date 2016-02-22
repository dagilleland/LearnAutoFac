using Autofac;
using AutoFacTools;
using I_Teach.SchoolSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    class Program
    {
        private static IContainer Container { get; set; }
        static void Main(string[] args)
        {
            //GetTypes(typeof(IHandleCommand<>));

            var builder = new ContainerBuilder();
            // 1) REgister through the CommandModule
            //builder.RegisterAssemblyModules(typeof(CommandModule).Assembly);
            builder.RegisterModule(new CommandModule(typeof(IHandleCommand<>)));

            //    .. or
            //// 2) Register manually
            //// a) loop through assembly
            ////      tba
            //// b)
            //builder.RegisterType<AutofacHandlerResolver>()
            //           .As<IHandlerResolver>();
            //// c)
            //builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>();

            Container = builder.Build();

            foreach (var item in Container.ComponentRegistry.Registrations)
            {
                string message = item.Activator.ToString() + "\n\tServices:";
                foreach (var service in item.Services)
                    message += "\n\t\t" + service.Description;
                message += "\n\tMetaData";
                foreach (var md in item.Metadata)
                    message += "\n\t\t" + md.Key + ":" + md.Value;
                Console.WriteLine(message);
                Console.WriteLine("---------------------");
                //Console.WriteLine(item);
            }

            using (var scope = Container.BeginLifetimeScope())
            {
                var dispatcher = scope.Resolve<ICommandDispatcher>();
                Console.WriteLine(dispatcher);

                var command = new CreateSchedule();
                dispatcher.SendCommand(command);
            }
        }


/* Requirements
 * - One handler per specific command
 * - Generic type as the command handler interface: IHandleCommand<T>
 *      - Passed in to the constructor of the CommandModule that registers the command handlers
 * - Resolved class may handle many different commands
 */
        static void GetTypes(Type genericInterface)
        {
            var types = from asm in AppDomain.CurrentDomain.GetAssemblies()
                        from type in asm.GetTypes()
                        where type.IsClass
                           && type.GetInterfaces().Any(
                              intf => intf.IsGenericType
                                && intf.GetGenericTypeDefinition() == genericInterface)
                        select new
            {
                Handler = type.Name,
                GenericType = genericInterface.Name,
                Specifics = //type.GetInterface(
                type.GetInterfaces().Select(x=>x.GetGenericTypeDefinition() == genericInterface)
            };
            foreach (var item in types)
                Console.WriteLine(item);
        }
    }



}
