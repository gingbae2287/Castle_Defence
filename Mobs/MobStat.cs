using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "Monster/Create Monster", order = 0)]

[System.Serializable]
public class MobStat : ScriptableObject {
    
    [SerializeField] int baseHP;
    [SerializeField] int lvPerHP;
    [SerializeField] int baseDamage;
    [SerializeField] int lvPerDamage;
    public float atkRange;
    public float atkSpeed;
    public float speed;
    [SerializeField]int baseMoney;
    [SerializeField] int lvPerMoney;
    public int maxHp {get{
            return baseHP+(lvPerHP*lv);
        }
    }
    public int damage{
        get{
            return baseDamage+(lvPerDamage*lv);
        }
    }
    public int killMoney{
        get{
            return baseMoney+(lvPerMoney*lv);
        }
    }


    //====
    public int lv{get;private set;}

    public void SetLevel(int level){
        lv=level;
        //maxHp=baseHP+(lvPerHP*lv);
        //damage=baseDamage+(lvPerDamage*lv);
        //money=baseMoney+(lvPerMoney*lv);
    }
}