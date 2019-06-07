using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseController : MonoBehaviour
{

    public Text WarehouseInfo;
    public float Offset = 2;
    public int MetalCapacity = 100;
    public int WoodCapacity = 600;
    public string Name;

    [HideInInspector]
    public int Metal, Wood;


    private void Awake()
    {
        Metal = MetalCapacity;
        Wood = WoodCapacity;

        UpdateInfo();
    }


    public void UpdateInfo()
    {
        string info = Name + "\n";
        info += string.Format(" - м. {0}/{1};\n", Metal, MetalCapacity);
        info += string.Format(" - д. {0}/{1};\n", Wood, WoodCapacity);

        WarehouseInfo.text = info;
    }

    public void Interact(BuilderController builder, BuildingController lastBuilding)
    {

        //action with builder
        if (builder.ReturnMaterials)
        {
            Metal += builder.Metal;
            Wood += builder.Wood;

            builder.Metal = 0;
            builder.Wood = 0;

            builder.ReturnMaterials = false;
        }
        else
        {
            int metal = Metal - (builder.MetalCapacity - builder.Metal);

            if (metal < 0)
            {
                metal = Metal;
                Metal = 0;
            }
            else
            {
                Metal -= metal;
            }

            builder.Metal += metal;

            if (Metal > MetalCapacity)
            {
                builder.Metal += Metal - MetalCapacity;
                Metal = MetalCapacity;
            }


            int wood = Wood - (builder.WoodCapacity - builder.Wood);

            if (wood < 0)
            {
                wood = Wood;
                Wood = 0;
            }
            else
            {
                Wood -= wood;
            }

            builder.Wood += wood;

            if (Wood > WoodCapacity)
            {
                builder.Wood += Wood - WoodCapacity;
                Wood = WoodCapacity;
            }
        }


        //check is warehouse empty or not
        if (Metal <= 0 && Wood <= 0)
        {
            Metal = 0;
            Wood = 0;
            if (WarehousesList.ActiveWarehouses.Contains(this))
                WarehousesList.ActiveWarehouses.Remove(this);
        }
        else
        {
            if (!WarehousesList.ActiveWarehouses.Contains(this))
                WarehousesList.ActiveWarehouses.Add(this);
        }

        builder.UpdateInfo();
        UpdateInfo();

        builder.GoToLastBuilding(lastBuilding);
    }

}
