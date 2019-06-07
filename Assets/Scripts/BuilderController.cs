using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderController : MonoBehaviour
{
    public GameObject Selection;
    public float BuildingSpeed = 10;
    public float MovementSpeed = 0.01f;
    public string Name = "Builder##";

    private BuildingController _target;
    private Vector3 _startPoint, _targetPoint;
    private float _path, _offset;
    private bool _isMoving;

    private static BuilderController _selectedBuilder;


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

    public void SetTarget(BuildingController target)
    {
        _target = target;

        _startPoint = transform.position;
        _targetPoint = target.transform.position;
        _targetPoint.y = _startPoint.y;

        _offset = target.Offset;
        _path = 0;

        _isMoving = true;
    }

}
