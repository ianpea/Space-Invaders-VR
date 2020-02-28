using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    private bool isSpawned = false;
    public bool IsSpawned { get { return isSpawned; } set { isSpawned = value; } }
    private List<Units> _unit = null;
    public List<Units> unit {
        get
        {
            if (_unit == null) return new List<Units>();
            else return _unit;
        }
        set
        {
            _unit = (value);
        }
    }
}
