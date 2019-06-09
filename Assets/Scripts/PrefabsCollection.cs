using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsCollection : MonoBehaviour
{
    /// <summary>
    /// Коллекция префабов зданий; синглтон
    /// </summary>

    public GameObject HousePrefab;
    public GameObject BarnPrefab;

    public static PrefabsCollection Instance;


    private void Awake()
    {
        Instance = this;
    }

}
