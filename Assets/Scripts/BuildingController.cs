using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingController : Building
{

    public event Action OnBuildingFinished;


    private new void Awake()
    {
        var capacity = BuildingsConfig.GetCapacity(Type);

        if (capacity != null)
        {
            MetalCapacity = capacity.Metal;
            WoodCapacity = capacity.Wood;
        }

        Name = BuildingsConfig.GetName(Type);
        base.Awake();
    }


    private void OnMouseDown()
    {
        var builder = BuilderController.SelectedBuilder;

        if (builder != null)
            builder.AssignToBuilding(this);
    }


    public void ApplyBuildingProgress(int deltaProgress)
    {
        Percentage += deltaProgress;

        if (Percentage >= 100)
        {
            Percentage = 100;

            if (OnBuildingFinished != null)
                OnBuildingFinished();

            Debug.Log(string.Format(Name + " построен!"));
        }

        Vector3 startPos = BasesController.Instance.DefaultShift;
        Vector3 finalPos = BasesController.Instance.FinalShift;

        float t = Percentage / 100f;
        transform.localPosition = Vector3.Lerp(startPos, finalPos, t);

        UpdateInfo();
    }

}
