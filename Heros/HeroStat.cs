using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero Stat", menuName = "Hero/Create Hero Stat", order = 0)]


[System.Serializable]
public class HeroStat : ScriptableObject {
    public enum WeaponType
    {
    Bow,
    Wand
    }
    
    public WeaponType weaponType;
    const int baseLvUpCost=100;
    const float lvPerCostRate=1.2f;

    public string heroName;
    /////
    [Header("레벨1 기본 데미지")]
    public int baseDamage;
    [Header("레벨당 올라가는 Base Damage")]
    public int lvPerDamage;

    /////
    public float atkRange;
    [Header("공격속도 (1초당 공격 횟수)")]
    public float atkSpeed;

    ///
    public int damage{
        get{
            return baseDamage+(level-1)*lvPerDamage;
        }
    }
    public int lvUpCost{
        get{
            return (int)(Mathf.Pow(lvPerCostRate,level-1)*baseLvUpCost);
        }
    }
    public int level=1;
    public int lv{get{return level;}}
    public void LevelUp(int amount=1){
        level+=amount;
        //lvUpCost=(int)Mathf.Pow(lvPerCostRate,lv-1)*baseLvUpCost;
        //damage=baseDamage+(lv-1)*lvPerDamage;

    }

}