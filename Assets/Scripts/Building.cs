using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Building : MonoBehaviour
{
    [HideInInspector]
    public int Metal, Wood, Percentage;
    public int MetalCapacity, WoodCapacity;
    [Space]
    public Text BuildingInfo;
    public float Offset = 2f;
    public BuildingType Type;
    public string Name;
    [HideInInspector]
    public bool IsReady;


    protected void Awake()
    {
        UpdateInfo();
    }

    public virtual void UpdateInfo()
    {
        string info = Name + "\n";
        info += string.Format(" - м. {0}/{1};\n", Metal, MetalCapacity);
        info += string.Format(" - д. {0}/{1};\n", Wood, WoodCapacity);

        if (Type != BuildingType.warehouse)
            info += string.Format(" (завершён на {0}%)\n", Percentage);

        BuildingInfo.text = info;

        if (Wood == WoodCapacity && Metal == MetalCapacity)
            IsReady = true;
    }

}
