using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BuilderController : MonoBehaviour
{
    /// <summary>
    /// Управляет строителем: его перемещением и действиями
    /// </summary>

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
            ((BuildingController)_assignedBuilding).OnBuildingFinished -= ReturnToDefaultPos; //отписывается от событий построенного здания
            Debug.Log(Name + ": отписка от " + _assignedBuilding.Name);
        }
    }


    private void OnMouseDown()
    {
        Select();
    }

    private void Select()
    //выбрать и подсветить рабочего по щелчку мыши
    {
        if (SelectedBuilder == this)
            return;

        DeSelectLastBuilder();

        SelectedBuilder = this;
        Selection.SetActive(true);
        Debug.Log(Name + ": выбран");
    }

    public static void DeSelectLastBuilder()
    //снять выделение с последнего выбранного рабочего
    {
        if (SelectedBuilder == null)
            return;

        string name = SelectedBuilder.Name;
        SelectedBuilder.Selection.SetActive(false);
        SelectedBuilder = null;

        Debug.Log(name + ": выбор отменён");
    }


    public void UpdateInfo()
    //обновить надписи и индкаторы над "головой" рабочего
    {
        string info = Name + "\n";
        info += string.Format(" - м. {0}/{1};\n", Metal, MetalCapacity);
        info += string.Format(" - д. {0}/{1};\n", Wood, WoodCapacity);

        BuilderInfo.text = info;

        MetalCube.SetActive(Metal > 0);
        WoodenCube.SetActive(Wood > 0);
    }

    private void Stop(Building building)
    //прервать движение рабочего
    {
        _isMoving = false;
        _progress = 0;
    }

    public void AssignToBuilding(Building building)
    //назначить строителя на постройку (дома/амбара)
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
    //отправить  строителя к зданию (дому/амбару/складу)
    {
        _target = building;

        _startPoint = transform.position;
        _targetPoint = building.transform.position;

        _distanceToTarget = Vector3.Distance(_startPoint, _targetPoint);
        _progress = 0;

        _isMoving = true;
    }

    private void TakeMaterials(Building building)
    //строитель берёт материалы (со склада)
    {
        //берёт металл
        int metal = MetalCapacity - Metal;

        if (metal > building.Metal)
            metal = building.Metal;

        Metal += metal;
        building.Metal -= metal;

        //берёт дерево
        int wood = WoodCapacity - Wood;

        if (wood > building.Wood)
            wood = building.Wood;

        Wood += wood;
        building.Wood -= wood;

        //обновить индикаторы строителя и лог
        UpdateInfo();

        Debug.Log(string.Format("{0} взял со склада {1} ед. металла и {2} ед. дерева", Name, metal, wood));
    }

    private void GiveMaterials(Building building)
    //строитель вкладывает материалы в здание (дом/амбар/склад)
    {
        //вкладывает металл
        int metal = building.MetalCapacity - building.Metal;

        if (metal > Metal)
            metal = Metal;

        Metal -= metal;
        building.Metal += metal;

        //вкладывает дерево 
        int wood = building.WoodCapacity - building.Wood;

        if (wood > Wood)
            wood = Wood;

        Wood -= wood;
        building.Wood += wood;

        //обновить индикаторы строителя и лог
        UpdateInfo();

        Debug.Log(string.Format("{0} вложил в {1} {2} ед. металла и {3} ед. дерева", Name, building.Name.ToLower(), metal, wood));
    }

    private void Interact(Building target)
    //строитель взаимодействует со зданиями
    {
        switch(target.Type)
        {
            case BuildingType.warehouse: //взаимодействует со складом

                var warehouse = target as WarehouseController;

                if (_returnMaterials) //возвращает материалы на склад
                {
                    GiveMaterials(warehouse);
                    _returnMaterials = false;
                }
                else //берёт материалы со склада
                {
                    TakeMaterials(warehouse);
                }

                warehouse.CheckIsEmptyOrNot();
                warehouse.UpdateInfo();

                GoToBuilding(_assignedBuilding);

                break;


            case BuildingType.house: //взаимодействует с домом
            case BuildingType.barn: // или амбаром

                var building = target as BuildingController;

                GiveMaterials(target); //вкладывает матералы в стройку (дома/амбара)
                target.UpdateInfo();

                if (target.IsReady) //начинает стройку, если она накопила материалы
                {
                    if (Metal == 0 && Wood == 0) //начало стройки
                        StartBuilding(target as BuildingController);
                    else //если остались лишние материалы, то возвращает их на склад
                        ReturnMaterials(); 
                }
                else //идёт на склад за недостающими материалами
                {
                    warehouse = GetNextWarehouse();
                    GoToBuilding(warehouse);
                }
                break;

            default: //строитель вернулся в исходную позицию
                Debug.Log(Name + " вернулся на исходную позицию.");
                break;
        }
    }

    private void MoveBuilder()
    //перемещает строителя из стартовой позиции к цели (с учётом отступа)
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
    //после завершения стройки строитель возвращается на исходну позицию
    {
        _startPoint = transform.position;
        _targetPoint = _defaultPos;

        _target = null;

        _isMoving = true;
    }

    public void ReturnMaterials()
    //строитель возвращает на склад излишки материалов
    {
        _returnMaterials = true;
        var warehouse = GetNextWarehouse();
        GoToBuilding(warehouse);
    }

    private WarehouseController GetNextWarehouse()
    //строитель выбирает склад, к которому он пойдёт
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
    //строитель начинает строит здание (дом/амбар)
    {
        StartCoroutine(BuildingProcess(building));
        building.OnBuildingFinished += ReturnToDefaultPos;
    }

    private void StopBuilding()
    //строитель перестаёт строить здание
    {
        StopCoroutine(BuildingProcess(null));
    }

    private IEnumerator BuildingProcess(BuildingController building)
    //процесс постройки здания
    {
        while (building.Percentage < 100)
        {
            building.ApplyBuildingProgress(BuildingSpeed);
            yield return new WaitForSecondsRealtime(1f);
        }
    }

}
