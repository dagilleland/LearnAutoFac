using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.SchoolSchedule
{
    public interface IPersist<T, TKey>
    {
        void Save(T obj, TKey key);
        T Find(TKey key);

        // TODO: Move to querying......
        IEnumerable<T> List(); // TODO: define with a func parameter to allow query specification?
    }

    public abstract class InMemoryPersistence<TAggregate, TKey> : IPersist<TAggregate, TKey>
    {
        private static Dictionary<TKey, TAggregate> AggregateStore = new Dictionary<TKey, TAggregate>();


        public void Save(TAggregate obj, TKey key)
        {
            if (AggregateStore.ContainsKey(key))
                AggregateStore[key] = obj;
            else
                AggregateStore.Add(key, obj);
        }

        public TAggregate Find(TKey key)
        {
            return AggregateStore[key];
        }

        public IEnumerable<TAggregate> List()
        {
            var result = from data in AggregateStore
                         select data.Value;
            return result;
        }
    }
}
