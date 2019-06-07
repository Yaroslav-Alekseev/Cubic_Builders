using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuilderController : MonoBehaviour
{
    public GameObject Selection;
    public GameObject MetalCube;
    public GameObject WoodenCube;
    public Text BuilderInfo;
    [Space]
    public int MetalCapacity = 50;
    public int WoodCapacity = 100;
    public int BuildingSpeed = 10;
    public float MovementSpeed = 0.01f;
    public string Name = "Строитель##";

    [HideInInspector]
    public int Metal, Wood;

    private BuildingController _target;
    private Vector3 _startPoint, _targetPoint;
    private float _path, _offset;
    private bool _isMoving;

    private static BuilderController _selectedBuilder;


    private void Awake()
    {
        UpdateInfo();
    }

    private void Update()
    {
        MoveBuilder();
    }

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

    public static BuilderController GetSelectedBuilder()
    {
        return _selectedBuilder;
    }

    private void MoveBuilder()
    {
        if (!_isMoving)
            return;

        _path += Vector3.Distance(_startPoint, _targetPoint) * MovementSpeed;
        Vector3 nextPoint = Vector3.Lerp(_startPoint, _targetPoint, _path);

        float distance = Vector3.Distance(nextPoint, _targetPoint);
        if (distance > _offset)
        {
            transform.position = nextPoint;
        }
        else
        {
            _isMoving = false;
            _startPoint = transform.position;
            _path = 0;

            _target.Interact(this);
        }
    }

    public void SetBuilding(BuildingController building)
    {
        _target = building;

        _startPoint = transform.position;
        _targetPoint = building.transform.position;
        _targetPoint.y = _startPoint.y;

        _offset = building.Offset;
        _path = 0;

        _isMoving = true;
    }

    public void UpdateInfo()
    {
        string info = Name + "\n";
        info += string.Format(" - м. {0}/{1};\n", Metal, MetalCapacity);
        info += string.Format(" - д. {0}/{1};\n", Wood, WoodCapacity);

        BuilderInfo.text = info;

        MetalCube.SetActive(Metal > 0);
        WoodenCube.SetActive(Wood > 0);
    }

    public void ReturnToWarehouse(BuildingController lastBuilding, bool startBuilding)
    {
        ///
    }

    public void StartBuilding(BuildingController building)
    {
        StartCoroutine(BuildingProcess(building));
    }

    private void StopBuilding()
    {
        StopCoroutine(BuildingProcess(null));
    }

    private IEnumerator BuildingProcess(BuildingController building)
    {
        while (building.GetPercetage() < 100)
        {
            building.ApplyBuildingProgress(BuildingSpeed);
            yield return new WaitForSecondsRealtime(1f);
        }
    }

}
