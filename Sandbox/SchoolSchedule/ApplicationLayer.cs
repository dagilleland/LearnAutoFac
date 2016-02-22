using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I_Teach.SchoolSchedule
{
    public class ApplicationHost
    {
        private ICommandDispatcher CommandDispatcher { get; set; }

        public ApplicationHost(ICommandDispatcher commandDispatcher)
        {
            CommandDispatcher = commandDispatcher;
        }

        public void Process<TCommand>(TCommand command) where TCommand : class
        {
            CommandDispatcher.SendCommand(command);
        }
    }

    #region Infrastructure
    #region Implementations
    public class ScheduleRepository : IScheduleRepository
    {
        public void AddSchedule(Schedule schedule)
        {
            throw new NotImplementedException();
        }

        public void UpdateSchedule(Schedule schedule)
        {
            throw new NotImplementedException();
        }


        public Schedule GetSchedule(Guid Id)
        {
            throw new NotImplementedException();
        }
    }

    public class AutoFacCommandBus : Module, ICommandDispatcher
    {


        public void SendCommand<TCommand>(TCommand command) where TCommand : class
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    #region Interfaces
    public interface ICommandDispatcher
    {
        void SendCommand<TCommand>(TCommand command) where TCommand : class;
    }
    public interface IEventPublisher
    {
        void Publish<TEvent>(TEvent eventObj);
    }
    #endregion
    #endregion
}
