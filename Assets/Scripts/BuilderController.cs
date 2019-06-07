using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderController : MonoBehaviour
{
    public GameObject Selection;
    public float BuildingSpeed = 10;
    public string Name = "Builder##";

    private static BuilderController _selectedBuilder;


    private void OnMouseDown()
    {
        if (_selectedBuilder != this)
        {
            DeSelectLastBuilder();
            SelectBuilder();
        }
    }

    private void SelectBuilder()
    {
        _selectedBuilder = this;
        Selection.SetActive(true);
        Debug.Log(this.Name + " selected!");
    }

    public static void DeSelectLastBuilder()
    {
        if (_selectedBuilder == null)
            return;
        
        _selectedBuilder.Selection.SetActive(false);
        Debug.Log(_selectedBuilder.Name + " deselected!");
        _selectedBuilder = null;
    }
}
