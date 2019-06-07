﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour
{

    public Text BuildingInfo;
    public BuildingType Type;

    private int _metal, _wood, _percentage;
    private int _metalCapacity, _woodCapacity;
    private string _name;

    private void Awake()
    {
        _metal = 0;
        _wood = 0;
        _percentage = 0;

        var capacity = BuildingsConfig.GetCapacity(Type);
        _metalCapacity = capacity.Metal;
        _woodCapacity = capacity.Wood;

        _name = BuildingsConfig.GetName(Type);

        UpdateInfo();
    }


    private void OnMouseDown()
    {
        BindBuilder();
    }

    private void UpdateInfo()
    {
        string info = _name + "\n";
        info += string.Format(" - м. {0}/{1};\n", _metal, _metalCapacity);
        info += string.Format(" - д. {0}/{1};\n", _wood, _woodCapacity);
        info += string.Format(" (завершён на {0}%)\n", _percentage);

        BuildingInfo.text = info;
    }

    private void BindBuilder()
    {
        Debug.Log("Рабочий Х начал строить " + _name.ToLower()); 
    }

    public void Build(int metal, int wood, float buildingSpeed)
    {
        UpdateInfo();
    }

}
