using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour
{

    public Text BuildingInfo;
    public string Name;
    public int MetalCapacity;
    public int WoodCapacity;

    private int _metal, _wood, _percentage;


    private void Awake()
    {
        _metal = 0;
        _wood = 0;
        _percentage = 0;

        UpdateInfo();
    }


    private void UpdateInfo()
    {
        string info = Name + "\n";
        info += string.Format(" - м. {0}/{1};\n", _metal, MetalCapacity);
        info += string.Format(" - д. {0}/{1};\n", _wood, WoodCapacity);
        info += string.Format(" (завершён на {0}%)\n", _percentage);

        BuildingInfo.text = info;
    }


}
