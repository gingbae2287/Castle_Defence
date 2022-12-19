using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero Skill", menuName = "Skill/Passive/TeamBuff", order = 0)]

[System.Serializable]


public class PassiveTeamBuff : PassiveSkill {
    /*
    [SerializeField]float damageIncreaseRate=0;
    [SerializeField]float damageIncreaseValue=0;
    [SerializeField]float attackSpeedIncreaseRate=0;
    [SerializeField]float attackSpeedIncreaseValue=0;
    */
    [SerializeField] List<PTBSkillSet> skillSet;
    public override EnumTypes.SkillType SkillType(){
        return EnumTypes.SkillType.PassiveTeamBuff;
    }


    public override void Affect(){
        for(int i=0;i<skillSet.Count;i++){
            switch(skillSet[i].skillType){
                case EnumTypes.PTBSkillType.DamageIncreaseRate:
                    SkillManager.p_TeamBuff.DamageIncRate+=skillSet[i].value;
                    break;
                case EnumTypes.PTBSkillType.DamageIncreaseValue:
                    SkillManager.p_TeamBuff.DamageInc+=(int)skillSet[i].value;
                    break;
                case EnumTypes.PTBSkillType.AttackSpeedIncreaseRate:
                    SkillManager.p_TeamBuff.AtkSpeedIncRate+=skillSet[i].value;
                    break;
                case EnumTypes.PTBSkillType.AttackSpeedIncreaseValue:
                    SkillManager.p_TeamBuff.AtkSpeedInc+=skillSet[i].value;
                    break;
            }
        }
    }
    

}
[System.Serializable]
public struct PTBSkillSet{
        [Header("Increase Rate Ex) 0.2 => +20%")]
        public EnumTypes.PTBSkillType skillType;
        public float value;
}