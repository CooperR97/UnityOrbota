using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbotaAbstractObject : ScriptableObject
{
    [SerializeField]
    protected int ID;
    [SerializeField]
    protected string Name;
    
    public string getName() { return Name; }
    public int getItemID() { return ID; }
}
