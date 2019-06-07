using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    house,
    barn
}

public class BasesController : MonoBehaviour
{
    public Transform[] BasesList;
    [Space]
    public Vector3 DefaultShift = new Vector3(0, -0.9f, 0);
    public Vector3 FinalShift = new Vector3(0, 1.3f, 0);

    public static BasesController Instance;

    private List<Transform> _vacantBases = new List<Transform>();


    private void Awake()
    {
        Instance = this;
    
        foreach (var bl in BasesList)
            _vacantBases.Add(bl);
    }


    public GameObject SetBuilding(BuildingType buildingType)
    {
        GameObject prefab;

        switch(buildingType)
        {
            case BuildingType.house:
                prefab = PrefabsCollection.Instance.HousePrefab;
                break;

            case BuildingType.barn:
                prefab = PrefabsCollection.Instance.BarnPrefab;
                break;

            default:
                return null;
        }


        Transform parent; 

        if (_vacantBases.Count > 0)
        {
            parent = _vacantBases[0];
            _vacantBases.Remove(parent);
        }
        else
        {
            return null;
        }

        GameObject building = Instantiate(prefab, parent.position + DefaultShift, Quaternion.identity);
        building.transform.SetParent(parent, true);

        Debug.Log(buildingType + " is building!");
        return building;
    }

}
