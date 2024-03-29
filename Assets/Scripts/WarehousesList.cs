﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehousesList : MonoBehaviour
{
    /// <summary>
    /// Содержит коллекцию всех складов, а также активных(непустых) складов; синглтон
    /// </summary>
    
    public WarehouseController[] WareHouses = new WarehouseController[0];

    public static List<WarehouseController> ActiveWarehouses = new List<WarehouseController>();

    public static WarehousesList Instance;


    private void Awake()
    {
        Instance = this;

        foreach (var wh in WareHouses)
            ActiveWarehouses.Add(wh);
    }

}
