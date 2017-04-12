using System;
using System.Collections.Generic;
using UnityEngine;


public interface IMappingData<TSource, TDestination>
{
    TSource MapToDTO(TDestination source);
    TDestination MapToGameObject(TSource source, TDestination destination);
}

public class Mapper
{
    public Dictionary<TypePair, MapData> MapList;

    public Mapper ()
    {
        MapList = new Dictionary<TypePair, MapData>();
        Bind<RoadDTO, Road>
        ((src,destination) => 
            {
                destination.ID = src.ID;

                return destination;
            }
        );
    }

    public void Bind<TSource, TDestination>(MapData.MapCore<TSource, TDestination> mapping)
    {
        TypePair typePair = TypePair.Create<TSource, TDestination>();
        //MapList[typePair] = new MapData.CreateMapData(mapping);
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination = default(TDestination))
    {
        TypePair typePair = TypePair.Create<TSource, TDestination>();
        return (TDestination)MapList[typePair].Map(source, destination);
    }
}

public class TypePair
{
    public Type Source { get; private set; }
    public Type Destination { get; private set; }

    public TypePair(Type source, Type destination)
    {
        Source = source;
        Destination = destination;
    }

    public static TypePair Create(Type source, Type target)
    {
        return new TypePair(source, target);
    }

    public static TypePair Create<TSource, TTarget>()
    {
        return new TypePair(typeof(TSource), typeof(TTarget));
    }
}

public class MapData
{
    public MapData CreateMapData<T,TT>(MapCore<T,TT> map)
    {
        return new MapData();
    }

    public object Map(object source, object destination = null)
    {
        return null;
    }

    public delegate TT MapCore<T, TT>(T source, TT destination);
    private static MapCore<Type, Type> map;
}

