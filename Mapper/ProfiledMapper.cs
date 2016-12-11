using System;
using System.Collections.Generic;
using System.Linq;

namespace Mapper
{
    public class ProfiledMapper : Mapper
    {
        private Dictionary<string, Action<object, object>> _maps = new Dictionary<string, Action<object, object>>();

        public void AddMap<TSource, TDestination>(Action<TSource, TDestination> map)
        {
            Action<object, object> f = (x, y) => map((TSource)x, (TDestination)y);
            _maps.Add($"{typeof(TSource).Name}->{typeof(TDestination).Name}", f);
        }

        public void Initialize(IEnumerable<Action> maps)
        {
            foreach (var m in maps)
            {
                m.DynamicInvoke();
            }
        }

        public TDestination MapByProfile<TSource, TDestination>(TSource src, TDestination dest)
        {
            if (_maps.ContainsKey($"{typeof(TSource).Name}->{typeof(TDestination).Name}"))
            {
                _maps[$"{typeof(TSource).Name}->{typeof(TDestination).Name}"].DynamicInvoke(src, dest);
                return (TDestination)Temp;
            }
            return QuickMap(src, dest);
        }

        public IEnumerable<TDestination> MapByProfile<TSource, TDestination>(IEnumerable<TSource> src, IEnumerable<TDestination> dest)
        {
            var result = new List<TDestination>();
            var destinations = dest as IList<TDestination> ?? dest.ToList();
            var sources = src as IList<TSource> ?? src.ToList();

            if (sources.Count == destinations.Count)
            {
                result.AddRange(destinations.Select((t, i) => MapByProfile(sources.ElementAt(i), destinations.ElementAt(i))));
            }
            else
            {
                result = destinations.ToList();
            }
            return result;
        }
    }
}
