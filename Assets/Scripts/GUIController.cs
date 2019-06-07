using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    
    public void BuildHouse()
    {
        BasesController.Instance.SetBuilding(BuildingType.house);
    }

    public void BuildBarn()
    {
        BasesController.Instance.SetBuilding(BuildingType.barn);
    }

}
