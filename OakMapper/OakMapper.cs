using System;
using System.Collections.Generic;
using System.Linq;

namespace OakMapper
{
    public class OakMapper
    {
        private object _src;
        private object _temp;
        private object _dest;

        /// <summary>
        /// Initializes an instanse of OakMapper class.
        /// </summary>
        public OakMapper()
        {
            _src = null;
            _temp = null;
            _dest = null;
        }

        /// <summary>
        /// Simply maps source class properties values into destination class.
        /// </summary>
        /// <typeparam name="TSource">Source class type.</typeparam>
        /// <typeparam name="TDestination">Destination class type.</typeparam>
        /// <param name="source">Source class.</param>
        /// <returns>New instance of class with specified type.</returns>
        public TDestination QuickMap<TSource, TDestination>(TSource source)
            where TSource : new()
            where TDestination : new()
        {
            var destination = new TDestination();
            var sourceProps = source.GetType().GetProperties();
            var destinationProps = destination.GetType().GetProperties();
            foreach (var srcProp in sourceProps)
            {
                foreach (var destProp in destinationProps)
                {
                    if (srcProp.Name == destProp.Name && srcProp.PropertyType == destProp.PropertyType)
                    {
                        destProp.SetValue(destination, srcProp.GetValue(source), null);
                    }
                }
            }
            return destination;
        }

        /// <summary>
        /// Simply maps collection of source classes into collection of destination classes.
        /// </summary>
        /// <typeparam name="TSource">Source class type.</typeparam>
        /// <typeparam name="TDestination">Destination class type.</typeparam>
        /// <param name="source">Source class.</param>
        /// <returns>New instance of class with specified type.</returns>
        public IEnumerable<TDestination> QuickMap<TSource, TDestination>(IEnumerable<TSource> source)
            where TSource : new()
            where TDestination : new()
        {
            return source.Select(QuickMap<TSource, TDestination>);
        }

        /// <summary>
        /// Simply maps source class properties values into destination class.
        /// </summary>
        /// <typeparam name="TSource">Source class type.</typeparam>
        /// <typeparam name="TDestination">Destination class type.</typeparam>
        /// <param name="source">Source class.</param>
        /// <param name="destination">Destination class.</param>
        /// <returns>New instance of class with specified type.</returns>
        public TDestination QuickMap<TSource, TDestination>(TSource source, TDestination destination)
        {
            var sourceProps = source.GetType().GetProperties();
            var destinationProps = destination.GetType().GetProperties();
            foreach (var srcProp in sourceProps)
            {
                foreach (var destProp in destinationProps)
                {
                    destProp.SetValue(destination, srcProp.Name == destProp.Name ? srcProp.GetValue(source) : destProp.GetValue(destination), null);
                }
            }

            return destination;
        }


        /// <summary>
        /// Simply maps source class collection into collection of destination classed.
        /// </summary>
        /// <typeparam name="TSource">Source class type.</typeparam>
        /// <typeparam name="TDestination">Destination class type.</typeparam>
        /// <param name="source">Source classes collection.</param>
        /// <param name="destination">Destination classes collection.</param>
        /// <returns>New instance of class with specified type.</returns>
        public IEnumerable<TDestination> QuickMap<TSource, TDestination>(IEnumerable<TSource> source, IEnumerable<TDestination> destination)
            where TSource : new()
        {
            return source.Zip(destination, QuickMap);
        }

        /// <summary>
        /// Maps source class properties values into destination class.
        /// </summary>
        /// <typeparam name="TSource">Source class type.</typeparam>
        /// <typeparam name="TDestination">Destination class type.</typeparam>
        /// <param name="source">Source class.</param>
        /// <returns>New instance of class with specified type.</returns>
        public WoodenMapNode<TSource, TDestination> Map<TSource, TDestination>(TSource source)
           where TSource : new()
            where TDestination : new()
        {
            _src = source;
            _temp = QuickMap<TSource, TDestination>(source);
            return new WoodenMapNode<TSource, TDestination>((TSource)_src, (TDestination)_temp, (TDestination)_dest);
        }

        /// <summary>
        /// Maps collection of source classes into collection of destination classes.
        /// </summary>
        /// <typeparam name="TSource">Source class type.</typeparam>
        /// <typeparam name="TDestination">Destination class type.</typeparam>
        /// <param name="source">Source class.</param>
        /// <returns>New instance of class with specified type.</returns>
        public WoodenMapNode<TSource, TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
            where TSource : new()
            where TDestination : new()
        {
            _src = source;
            _temp = QuickMap<TSource, TDestination>(source);
            return new WoodenMapNode<TSource, TDestination>((TSource)_src, (TDestination)_temp, (TDestination)_dest);
        }

        /// <summary>
        /// Maps source class properties values into destination class.
        /// </summary>
        /// <typeparam name="TSource">Source class type.</typeparam>
        /// <typeparam name="TDestination">Destination class type.</typeparam>
        /// <param name="source">Source class.</param>
        /// <param name="destination">Destination class.</param>
        /// <returns>New instance of class with specified type.</returns>
        public WoodenMapNode<TSource, TDestination> Map<TSource, TDestination>(TSource source, TDestination destination)
            where TSource : new()
        {
            _src = source;
            _temp = QuickMap(source, destination);
            _dest = destination;
            return new WoodenMapNode<TSource, TDestination>((TSource)_src, (TDestination)_temp, (TDestination)_dest);
        }

        /// <summary>
        /// Maps source class collection into collection of destination classed.
        /// </summary>
        /// <typeparam name="TSource">Source class type.</typeparam>
        /// <typeparam name="TDestination">Destination class type.</typeparam>
        /// <param name="source">Source classes collection.</param>
        /// <param name="destination">Destination classes collection.</param>
        /// <returns>New instance of class with specified type.</returns>
        public WoodenMapNode<TSource, TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source, IEnumerable<TDestination> destination)
            where TSource : new()
        {
            _src = source;
            _temp = QuickMap(source, destination);
            _dest = destination;
            return new WoodenMapNode<TSource, TDestination>((TSource)_src, (TDestination)_temp, (TDestination)_dest);
        }
    }

    public class WoodenMapNode<TSource, TDestination>
    {
        private readonly TSource _src;
        private readonly TDestination _temp;
        private readonly TDestination _dest;
        public WoodenMapNode(TSource src, TDestination temp, TDestination dest)
        {
            _src = src;
            _temp = temp;
            _dest = dest;
        }

        public WoodenMapNode<TSource, TDestination> Set(Action<TDestination> action)
        {
            action(_temp);
            return this;
        }

        public WoodenMapNode<TSource, TDestination> PickSource(Action<TSource, TDestination> action)
        {
            action(_src, _temp);
            return this;
        }

        public WoodenMapNode<TSource, TDestination> PickDestination(Action<TDestination, TDestination> action)
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
