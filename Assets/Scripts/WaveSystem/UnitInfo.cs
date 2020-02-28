using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units
{
    private GameObject _unit = null;
    private Vector3 _position;
    private Quaternion _rotation;
    public GameObject unitGet
    {
        get
        {
            if (_unit == null) return new GameObject();
            else return _unit;
        }
        set
        {
            _unit = value;
        }
    }
    public Vector3 position
    {
        get
        {
            if (_position != null) return _position;
            else return Vector3.zero;
        }
        set
        {
            _position = value;
        }
    }
    public Quaternion rotation
    {
        get
        {
            if (_rotation != null) return _rotation;
            else return Quaternion.identity;
        }
        set
        {
            _rotation = value;
        }
    }
}
