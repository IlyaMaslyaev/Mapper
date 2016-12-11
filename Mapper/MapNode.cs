using System;

namespace Mapper
{
    public class MapNode<TSource, TDestination>
    {
        private readonly TSource _src;
        private readonly TDestination _temp;
        private readonly TDestination _dest;

        public MapNode(TSource src, TDestination temp, TDestination dest)
        {
            _src = src;
            _temp = temp;
            _dest = dest;
        }

        public MapNode(TSource src, TDestination dest)
        {
            _src = src;
            _temp = dest;
            _dest = dest;
        }

        public MapNode<TSource, TDestination> Set(Action<TDestination> action)
        {
            action(_temp);
            return this;
        }

        public MapNode<TSource, TDestination> PickSource(Action<TSource, TDestination> action)
        {
            action(_src, _temp);
            return this;
        }

        public MapNode<TSource, TDestination> PickDestination(Action<TDestination, TDestination> action)
        {
            action(_dest, _temp);
            return this;
        }

        public TDestination Apply()
        {
            return _temp;
        }
    }
}
