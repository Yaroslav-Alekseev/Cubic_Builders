using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BuildingType
{
    house,
    barn,
    warehouse
}

public class Capacity
{
    public int Metal;
    public int Wood;
}


public class BuildingsConfig
{

    private static Dictionary<BuildingType, string> _names = new Dictionary<BuildingType, string>()
    {
        {BuildingType.house, "Дом"},
        {BuildingType.barn, "Амбар"},
        {BuildingType.warehouse, "Склад"}
    };


    private static Dictionary<BuildingType, Capacity> _capacities = new Dictionary<BuildingType, Capacity>()
    {
        {BuildingType.house, new Capacity {Metal = 200, Wood = 800 } },
        {BuildingType.barn, new Capacity {Metal = 300, Wood = 1200 } },
    };


    public static Capacity GetCapacity(BuildingType buildingType)
    {
        Capacity capacity = null;

        if (_capacities.ContainsKey(buildingType))
        {
            capacity = _capacities[buildingType];
        }

        return capacity;
    }


    public static string GetName(BuildingType buildingType)
    {
        string name = null;

        if (_names.ContainsKey(buildingType))
        {
            name = _names[buildingType];
        }

        return name;
    }

}
