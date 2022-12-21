using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero Skill", menuName = "Skill/Passive/OnHit", order = 0)]

[System.Serializable]


public class PassiveOnHit : PassiveSkill {
    public override EnumTypes.SkillType SkillType(){
        return EnumTypes.SkillType.PassiveOnHit;
    }
    [SerializeField] OnHit onHit;
    public override void SetOwner(Hero hero)
    {
        owner=hero;
        owner.AffectOnHitSkill(onHit);
    }
    public override void Affect(){
    }
        
}
