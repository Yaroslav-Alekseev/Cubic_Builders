﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour
{

    public Text BuildingInfo;
    public BuildingType Type;
    public float Offset = 2;

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
        var builder = BuilderController.GetSelectedBuilder();

        if (builder == null)
            return;

        builder.SetTarget(this);

        Debug.Log(string.Format("Рабочий {0} строит {1}", builder.Name, _name.ToLower())); 
    }

    public void Interact(BuilderController builder/*int metal, int wood, float buildingSpeed*/)
    {
        if (_metal < _metalCapacity && builder.Metal > 0)
        {
            int metal = builder.Metal - (_metalCapacity - _metal);

            if (metal < 0)
                metal = builder.Metal;

            builder.Metal -= metal;
            _metal += metal;
        }

        if (_wood < _woodCapacity && builder.Wood > 0)
        {
            int wood = builder.Wood - (_woodCapacity - _wood);

            if (wood < 0)
                wood = builder.Wood;

            builder.Wood -= wood;
            _wood += wood;
        }

        builder.UpdateInfo();
        UpdateInfo();

        bool startBuilding = (_metal == _metalCapacity) && (_wood == _woodCapacity);

        if (startBuilding && builder.Metal == 0 && builder.Wood == 0)
            builder.StartBuilding(this);

        else
            builder.ReturnToWarehouse(this, startBuilding);
    }

    public void ApplyBuildingProgress(int deltaProgress)
    {
        _percentage += deltaProgress;

        if (_percentage > 100)
            _percentage = 100;

        Vector3 startPos = BasesController.Instance.DefaultShift;
        Vector3 finalPos = BasesController.Instance.FinalShift;

        float t = _percentage / 100f;
        transform.position = Vector3.Lerp(startPos, finalPos, t);

        UpdateInfo();
    }

    public int GetPercetage()
    {
        return _percentage;
    }

}


