using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    
    public void BuildHouse()
    {
        BuildingController.Instance.SetBuilding(BuildingType.house);
    }

    public void BuildBarn()
    {
        BuildingController.Instance.SetBuilding(BuildingType.barn);
    }

}
