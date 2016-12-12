using System.Collections.Generic;
using System.Linq;

namespace Mapper
{
    public class Mapper
    {
        internal object Src;
        internal object Temp;
        internal object Dest;

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
        public MapNode<TSource, TDestination> Map<TSource, TDestination>(TSource source)
           where TSource : new()
            where TDestination : new()
        {
            Src = source;
            Temp = QuickMap<TSource, TDestination>(source);
            return new MapNode<TSource, TDestination>((TSource)Src, (TDestination)Temp, (TDestination)Dest);
        }

        /// <summary>
        /// Maps collection of source classes into collection of destination classes.
        /// </summary>
        /// <typeparam name="TSource">Source class type.</typeparam>
        /// <typeparam name="TDestination">Destination class type.</typeparam>
        /// <param name="source">Source class.</param>
        /// <returns>New instance of class with specified type.</returns>
        public MapNode<TSource, TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
            where TSource : new()
            where TDestination : new()
        {
            Src = source;
            Temp = QuickMap<TSource, TDestination>(source);
            return new MapNode<TSource, TDestination>((TSource)Src, (TDestination)Temp, (TDestination)Dest);
        }

        /// <summary>
        /// Maps source class properties values into destination class.
        /// </summary>
        /// <typeparam name="TSource">Source class type.</typeparam>
        /// <typeparam name="TDestination">Destination class type.</typeparam>
        /// <param name="source">Source class.</param>
        /// <param name="destination">Destination class.</param>
        /// <returns>New instance of class with specified type.</returns>
        public MapNode<TSource, TDestination> Map<TSource, TDestination>(TSource source, TDestination destination)
            where TSource : new()
        {
            Src = source;
            Temp = QuickMap(source, destination);
            Dest = destination;
            return new MapNode<TSource, TDestination>((TSource)Src, (TDestination)Temp, (TDestination)Dest);
        }

        /// <summary>
        /// Maps source class collection into collection of destination classed.
        /// </summary>
        /// <typeparam name="TSource">Source class type.</typeparam>
        /// <typeparam name="TDestination">Destination class type.</typeparam>
        /// <param name="source">Source classes collection.</param>
        /// <param name="destination">Destination classes collection.</param>
        /// <returns>New instance of class with specified type.</returns>
        public MapNode<TSource, TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source, IEnumerable<TDestination> destination)
            where TSource : new()
        {
            Src = source;
            Temp = QuickMap(source, destination);
            Dest = destination;
            return new MapNode<TSource, TDestination>((TSource)Src, (TDestination)Temp, (TDestination)Dest);
        }
    }
}

