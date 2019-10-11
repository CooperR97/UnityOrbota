using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Abstracts/Ability", order = 1)]
public class OrbotaAbility : OrbotaAbstractObject
{
    [SerializeField]
    protected string Description;
    [SerializeField]
    protected Sprite Thumbnail;
}
