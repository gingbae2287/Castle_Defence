using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Hero Stat", menuName = "Hero/Create Hero Stat", order = 0)]


[System.Serializable]
public abstract class PassiveSkill : Skill {
    public abstract override void Affect();

    public abstract override EnumTypes.SkillType SkillType();
    
}