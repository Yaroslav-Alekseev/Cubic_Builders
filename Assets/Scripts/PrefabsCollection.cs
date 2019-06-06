using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsCollection : MonoBehaviour
{
    public GameObject HousePrefab;
    public GameObject BarnPrefab;

    public static PrefabsCollection Instance;


    private void Awake()
    {
        Instance = this;
    }

}
