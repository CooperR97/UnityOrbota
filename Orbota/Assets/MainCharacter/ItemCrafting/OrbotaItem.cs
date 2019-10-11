using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Abstracts/Item", order = 0)]
public class OrbotaItem : OrbotaAbstractObject
{
    [SerializeField]
    protected string Description;
    [SerializeField]
    protected Sprite Thumbnail;
    [SerializeField]
    protected OrbotaItem[] CraftingComponents;
    [SerializeField]
    protected OrbotaWeapon WeaponApplied;

    public string getDescription() { return Description; }

    public Sprite getSprite() { return Thumbnail; }

    public int[] getItemCraftingValues()
    {
        int[] result = new int[CraftingComponents.Length];
        int count = 0;
        foreach(OrbotaItem item in CraftingComponents)
        {
            result[count] = item.getItemID();
            count++;
        }
        return result;
    }

    public OrbotaWeapon getItemAbilities()
    {
        return WeaponApplied;
    }
}
