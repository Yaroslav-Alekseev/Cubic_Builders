﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseController : Building
{
    /// <summary>
    /// Управляет зданиями складов
    /// </summary>

    protected new void Awake()
    {
        Metal = MetalCapacity;
        Wood = WoodCapacity;

        CheckIsEmptyOrNot();
        base.Awake();
    }

    public void CheckIsEmptyOrNot()
    //проверяет, есть ли на складе ресурсы
    {
        var activeWarehouses = WarehousesList.ActiveWarehouses;

        bool isEmpty = (Metal == 0) && (Wood == 0);

        if (isEmpty)
        {
            if (activeWarehouses.Contains(this))
                activeWarehouses.Remove(this);
        }
        else
        {
            if (!activeWarehouses.Contains(this))
                activeWarehouses.Add(this);
        }

    }

}
