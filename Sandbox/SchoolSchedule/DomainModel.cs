using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
namespace I_Teach.SchoolSchedule
{
    /*
     * Class Schedule Domain
     *  Instructors and students have class schedules. Each class on the schedule has a specific name, location, and time slot in the week. There can never be overlaps in the schedule. Also, there may be basic rules/structures to the time slots (for example, classes might start at 15 minutes past the hour and end at 10 minutes past the hour).
     *  EXTRA: For each course in a class schedule, there may be significant due dates.
     *
     *  Key Commands
     *      - CreateSchedule(Person, TermStart, WeeksDuration)
     *      - AddCourse
     *      - ScheduleRoom
     *      Future...
     *      - ScheduleClass(CourseNumber, Section, RoomBooking) - to do all the rooms for a course in one swoop
     *      - ChangeRoom(CourseNumber, Section, RoomBooking)
     *      - ExcludeDates(Date...)
     *      - IncludeReadingWeek(StartDate)
     */
    #region Aggregates -- INTERNAL
    public class Schedule
    {
        public Schedule(Person person, SchoolTerm term)
        {
            throw new NotImplementedException();
        }

        public Guid ScheduleId { get; set; }
        public void AddCourse(Course course)
        {
            Contract.Requires(course != null, "course is null.");
        }
        public void ScheduleRoom(Guid courseId, ClassRoom classRoom)
        {
            Contract.Requires(classRoom != null, "classRoom is null.");
        }
    }
    #endregion

    #region Business Rules -- INTERNAL
    #endregion

    #region Commands and Handlers -- PUBLIC
    public abstract class AbstractScheduleHandler
    {
        private readonly IScheduleRepository _DomainRepository;
        public IScheduleRepository DomainRepository
        { get { return _DomainRepository; } }

        public AbstractScheduleHandler(IScheduleRepository domainRepository)
        {
            _DomainRepository = domainRepository;
        }
    }

    public class CreateSchedule
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime TermStart { get; set; }
        public int WeeksDuration { get; set; }
    }
    public class CreateScheduleHandler
        : AbstractScheduleHandler
        , IHandleCommand<CreateSchedule>
    {

        public CreateScheduleHandler(IScheduleRepository repository)
            : base(repository) { }

        public IEnumerable Handle(CreateSchedule command)
        {
            // TODO: Do we allow multiple schedules for
            var agRoot = ScheduleFactory.CreateSchedule(command);
            DomainRepository.AddSchedule(agRoot);

            return new object[] { ScheduleFactory.EventFactory.ScheduleCreated(agRoot) };
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // NOTE: Don't use yield return, as it can cause silent failures in execution
            //yield return ScheduleFactory.EventFactory.ScheduleCreated(agRoot);
        }
    }

    public class AddCourse
    {
        public Guid ScheduleId { get; set; }
        public string CourseNumber { get; set; }
        public string Section { get; set; }
        public int HoursPerWeek { get; set; }
    }
    public class AddCourseHandler
        : AbstractScheduleHandler
        , IHandleCommand<AddCourse>
    {
        public AddCourseHandler(IScheduleRepository domainRepository)
            : base(domainRepository) { }

        public IEnumerable Handle(AddCourse command)
        {
            Course course = ScheduleFactory.CreateCourse(command);
            var agRoot = DomainRepository.GetSchedule(command.ScheduleId);
            // TODO: Any rules about duplicate courses scheduled?
            agRoot.AddCourse(course);

            DomainRepository.UpdateSchedule(agRoot);

            yield return ScheduleFactory.EventFactory.CourseAddedToSchedule();
        }
    }

    public class ScheduleRoom
    {
        public Guid ScheduleId { get; set; }
        public Guid CourseId { get; set; }
        public string RoomNumber { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
    public class ScheduleRoomHandler
        : AbstractScheduleHandler
        , IHandleCommand<ScheduleRoom>
    {
        public ScheduleRoomHandler(IScheduleRepository domainRepository)
            : base(domainRepository) { }

        public IEnumerable Handle(ScheduleRoom command)
        {
            ClassRoom classRoom = ScheduleFactory.CreateClassRoom(command.RoomNumber, command.Day, command.StartTime, command.EndTime);
            var agRoot = DomainRepository.GetSchedule(command.ScheduleId);
            // TODO: Rules about start and end times
            //          - All courses start at hh:15 and end at hh+:10
            //          - In blocks of 1, 2, 3, or 4 hours only
            //          - Fits in the standard work schedule (7AM to 11PM Mon-Fri, 7AM to 5PM Sat)
            // TODO: Rules about over-laps
            //          - Time overlaps in schedule
            // TODO: Rules about course
            //          - Too many hours slotted
            //          - Course happens only once per day
            agRoot.ScheduleRoom(command.CourseId, classRoom);

            DomainRepository.UpdateSchedule(agRoot);

            yield return ScheduleFactory.EventFactory.RoomScheduledForCourse();
        }
    }
    #endregion

    #region Domain Objects -- INTERNAL
    // TODO: Rename to "ScheduledClass", "ClassRoom" or leave as just "Class"?
    public class ClassRoom
    {
        public string RoomNumber { get; private set; }
        public DayOfWeek Day { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        public ClassRoom(string roomNumber, DayOfWeek day, TimeSpan startTime, TimeSpan endTime)
        {
            Contract.Requires(!string.IsNullOrEmpty(roomNumber), "roomNumber is null or empty.");
            Contract.Requires(endTime > startTime, "endTime must be later than startTime");

            RoomNumber = roomNumber;
            Day = day;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
    public class Course
    {
        public string CourseNumber { get; private set; }
        public string Section { get; private set; }
        public int HoursPerWeek { get; private set; }
        public IList<ClassRoom> Rooms { get; private set;}

        public Course(string courseNumber, string section, int hoursPerWeek)
        {
            CourseNumber = courseNumber;
            Section = section;
            HoursPerWeek = hoursPerWeek;

            Rooms = new List<ClassRoom>();
        }

        public void AssignRoom(ClassRoom room)
        {
            Contract.Requires(room != null, "room is null.");
            Rooms.Add(room);
        }
    }
    public class Person
    {
        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
        }
        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
        }

        public Person(string firstName, string lastName)
        {
            _FirstName = firstName;
            _LastName = lastName;
        }
    }
    public class SchoolTerm
    {
        public DateTime TermStart { get; private set; }
        public int Duration { get; private set; }
        public DateTime TermEnd { get; private set; }

        public SchoolTerm(DateTime termStart, int duration, DateTime termEnd)
        {
            TermStart = termStart;
            Duration = duration;
            TermEnd = termEnd;
        }
    }
    #endregion

    #region Events -- PUBLIC
    public class ScheduleCreated
    {
        public Guid ScheduleId { get; private  set; }

        public ScheduleCreated(Guid scheduleId)
        {
            ScheduleId = scheduleId;
        }
    }
    public class CourseAddedToSchedule
    {
    }
    public class RoomScheduledForCourse
    {
    }
    #endregion

    #region Factories -- INTERNAL
    public static class ScheduleFactory
    {
        internal static Schedule CreateSchedule(CreateSchedule info)
        {
            return new Schedule(CreatePerson(info.FirstName, info.LastName), CreateTerm(info.TermStart, info.WeeksDuration));
        }
        internal static Person CreatePerson(string firstName, string lastName)
        {
            return new Person(firstName, lastName);
        }
        internal static SchoolTerm CreateTerm(DateTime start, int duration)
        {
            DateTime end = start.AddDays(7 * duration);
            end = end.AddDays(DayOfWeek.Saturday - end.DayOfWeek);
            return new SchoolTerm(start, duration, end);
        }
        public static Course CreateCourse(AddCourse info)
        {
            return new Course(info.CourseNumber, info.Section, info.HoursPerWeek);
        }
        public static ClassRoom CreateClassRoom(string roomNumber, DayOfWeek day, TimeSpan startTime, TimeSpan endTime)
        {
            return new ClassRoom(roomNumber, day, startTime, endTime);
        }
        public static class EventFactory
        {
            public static ScheduleCreated ScheduleCreated(Schedule schedule)
            {
                return new ScheduleCreated(schedule.ScheduleId);
            }
            public static CourseAddedToSchedule CourseAddedToSchedule()
            {
                return new CourseAddedToSchedule();
            }
            public static RoomScheduledForCourse RoomScheduledForCourse()
            {
                return new RoomScheduledForCourse();
            }
        }
    }
    #endregion

    #region General Services -- PUBLIC
    #endregion

    #region H
    #endregion

    #region Interfaces -- PUBLIC
    // TODO: Move this interface to a common library, as it is not really domain-specific (rather, the domain uses it for command routing only)
    public interface IHandleCommand<TCommand> where TCommand : class
    {
        IEnumerable Handle(TCommand command);
    }
    public interface IScheduleRepository
    {
        void AddSchedule(Schedule schedule);
        void UpdateSchedule(Schedule schedule);
        Schedule GetSchedule(Guid Id);
    }
    #endregion
}
