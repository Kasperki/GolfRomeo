using System;
using System.Collections.Generic;
using UnityEngine;


public interface IMappingData<TSource, TDestination>
{
    TSource MapToDTO(TDestination source);
    TDestination MapToGameObject(TSource source);
    TDestination MapToGameObject(TSource source, TDestination destination);
}