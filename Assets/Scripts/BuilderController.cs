using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BuilderController : MonoBehaviour
{
    public GameObject Selection, MetalCube, WoodenCube;
    public Text BuilderInfo;
    [HideInInspector]
    public int Metal, Wood;
    public int MetalCapacity, WoodCapacity;
    public int BuildingSpeed = 10;
    public float Speed = 0.5f;
    public string Name = "Строитель##";

    public event Action<Building> OnTargetReached;

    public static BuilderController SelectedBuilder;

    private Building _target, _assignedBuilding;
    private Vector3 _startPoint, _targetPoint, _defaultPos;
    private float _distanceToTarget, _progress;
    private int _index;
    private bool _isMoving, _returnMaterials;


    private void Awake()
    {
        OnTargetReached += Stop;
        OnTargetReached += Interact;

        _defaultPos = transform.position;
        _index = UnityEngine.Random.Range(0, 1);

        UpdateInfo();
    }

    private void Update()
    {
        MoveBuilder();
    }

    private void OnDestroy()
    {
        OnTargetReached -= Stop;
        OnTargetReached -= Interact;

        if (_assignedBuilding != null && _assignedBuilding is BuildingController)
        {
            ((BuildingController)_assignedBuilding).OnBuildingFinished -= ReturnToDefaultPos;
            Debug.Log(Name + ": отписка от " + _assignedBuilding.Name);
        }
    }


    private void OnMouseDown()
    {
        Select();
    }

    private void Select()
    {
        if (SelectedBuilder == this)
            return;

        DeSelectLastBuilder();

        SelectedBuilder = this;
        Selection.SetActive(true);
        Debug.Log(Name + ": выбран");
    }

    public static void DeSelectLastBuilder()
    {
        if (SelectedBuilder == null)
            return;

        string name = SelectedBuilder.Name;
        SelectedBuilder.Selection.SetActive(false);
        SelectedBuilder = null;

        Debug.Log(name + ": выбор отменён");
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

    private void Stop(Building building)
    {
        _isMoving = false;
        _progress = 0;
    }

    public void AssignToBuilding(Building building)
    {
        _returnMaterials = false;

        if (building != _assignedBuilding)
        {
            StopBuilding();
            _assignedBuilding = building;
        }

        if (Metal == 0 &&  Wood == 0 && !building.IsReady)   //если у строителя нет материалов - идти на склад
        {
            var warehouse = GetNextWarehouse();
            GoToBuilding(warehouse);
        }
        else   //если у строителя есть материалы - идти строить
        {
            GoToBuilding(building);
        }

        Debug.Log(string.Format("{0} назначен на постройку {1}а", Name, building.Name.ToLower()));
    }

    private void GoToBuilding(Building building)
    {
        _target = building;

        _startPoint = transform.position;
        _targetPoint = building.transform.position;

        _distanceToTarget = Vector3.Distance(_startPoint, _targetPoint);
        _progress = 0;

        _isMoving = true;
    }

    private void TakeMaterials(Building building)
    {
        //take metal
        int metal = MetalCapacity - Metal;

        if (metal > building.Metal)
            metal = building.Metal;

        Metal += metal;
        building.Metal -= metal;

        //take wood
        int wood = WoodCapacity - Wood;

        if (wood > building.Wood)
            wood = building.Wood;

        Wood += wood;
        building.Wood -= wood;

        //update info
        UpdateInfo();

        Debug.Log(string.Format("{0} взял со склада {1} ед. металла и {2} ед. дерева", Name, metal, wood));
    }

    private void GiveMaterials(Building building)
    {
        //give metal
        int metal = building.MetalCapacity - building.Metal;

        if (metal > Metal)
            metal = Metal;

        Metal -= metal;
        building.Metal += metal;

        //give wood
        int wood = building.WoodCapacity - building.Wood;

        if (wood > Wood)
            wood = Wood;

        Wood -= wood;
        building.Wood += wood;

        //update info
        UpdateInfo();

        Debug.Log(string.Format("{0} вложил в {1} {2} ед. металла и {3} ед. дерева", Name, building.Name.ToLower(), metal, wood));
    }

    private void Interact(Building target)
    {
        switch(target.Type)
        {
            case BuildingType.warehouse: //склад

                var warehouse = target as WarehouseController;

                if (_returnMaterials)
                {
                    GiveMaterials(warehouse);
                    _returnMaterials = false;
                }
                else
                {
                    TakeMaterials(warehouse);
                }

                warehouse.CheckIsEmptyOrNot();
                warehouse.UpdateInfo();

                GoToBuilding(_assignedBuilding);

                break;


            case BuildingType.house: //дом или амбар
            case BuildingType.barn:

                var building = target as BuildingController;

                GiveMaterials(target);
                target.UpdateInfo();

                if (target.IsReady)
                {
                    if (Metal == 0 && Wood == 0)
                        StartBuilding(target as BuildingController);
                    else
                        ReturnMaterials();
                }
                else
                {
                    warehouse = GetNextWarehouse();
                    GoToBuilding(warehouse);
                }
                break;

            default: //исходная позиция
                Debug.Log(Name + " вернулся на исходную позицию.");
                break;
        }
    }

    private void MoveBuilder()
    {
        if (!_isMoving)
            return;

        _progress += Time.deltaTime * Speed;

        if (_progress > 1)
            _progress = 1;
           

        var pos = Vector3.Lerp(_startPoint, _targetPoint, _progress);
        pos.y = _startPoint.y;

        transform.position = pos;

        float distanceToTarget = Vector3.Distance(transform.position, _targetPoint);

        float offset = 0;
        if (_target != null)
            offset = _target.Offset;

        if (distanceToTarget <= offset || _progress == 1)
        {
            if (OnTargetReached != null && _target != null)
                OnTargetReached(_target);
        }
    }

    public void ReturnToDefaultPos()
    {
        _startPoint = transform.position;
        _targetPoint = _defaultPos;

        _target = null;

        _isMoving = true;
    }

    public void ReturnMaterials()
    {
        _returnMaterials = true;
        var warehouse = GetNextWarehouse();
        GoToBuilding(warehouse);
    }

    private WarehouseController GetNextWarehouse()
    {
        int maxIndex = WarehousesList.ActiveWarehouses.Count;
        WarehouseController warehouse = null;

        if (maxIndex == 0)
        {
            warehouse = WarehousesList.Instance.WareHouses[0];
            return warehouse;
        }

        if (_index >= maxIndex)
            _index = 0;

        warehouse = WarehousesList.ActiveWarehouses[_index];
        _index++;

        return warehouse;
    }


    public void StartBuilding(BuildingController building)
    {
        StartCoroutine(BuildingProcess(building));
        building.OnBuildingFinished += ReturnToDefaultPos;
    }

    private void StopBuilding()
    {
        StopCoroutine(BuildingProcess(null));
    }

    private IEnumerator BuildingProcess(BuildingController building)
    {
        while (building.Percentage < 100)
        {
            building.ApplyBuildingProgress(BuildingSpeed);
            yield return new WaitForSecondsRealtime(1f);
        }
    }

}
