using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseController : MonoBehaviour
{

    public Text WarehouseInfo;
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
}
