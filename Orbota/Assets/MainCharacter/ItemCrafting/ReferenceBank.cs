using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reference Bank", menuName = "Abstracts/Reference Bank", order = 4)]
public class ReferenceBank : OrbotaAbstractObject
{
    [SerializeField]
    protected OrbotaAbstractObject[] MyReferences;

    public OrbotaAbstractObject getReference(int index)
    {
        if(index > -1 && index < MyReferences.Length)
        {
            return MyReferences[index];
        }
        return null;
    }

    public OrbotaAbstractObject[] getReferences()
    {
        return MyReferences;
    }
}
