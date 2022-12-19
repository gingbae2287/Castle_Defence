using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///
/// PTB = Passive Team Buff
public struct TeamBuff{
    /// team buff skill values
    //Inc=Increase
    private float damageIncRate;
    public float DamageIncRate{get{return damageIncRate;} set{damageIncRate=value;}}
    private int damageInc;
    public int DamageInc{get{return damageInc;} set{damageInc=value;}}
    private float atkSpeedIncRate;
    public float AtkSpeedIncRate{get{return atkSpeedIncRate;} set{atkSpeedIncRate=value;}}
    private float atkSpeedInc;
    public float AtkSpeedInc{get{return atkSpeedInc;} set{atkSpeedInc=value;}}
    public void Init(){
        damageIncRate=0;
        damageInc=0;
        atkSpeedIncRate=0;
        atkSpeedInc=0;
        
    }

    
/*
    public TeamBuff operator+(TeamBuff tb1, TeamBuff tb2){
        TeamBuff tb= new TeamBuff();
        tb.damageIncRate=tb1.damageIncRate+tb2.damageIncRate;
        tb.damageInc=tb1.damageInc+tb2.damageInc;
        tb.atkSpeedIncRate=tb1.atkSpeedIncRate+tb2.atkSpeedIncRate;
        return tb;
    }*/
////////////////
}
public delegate void SkillHandler();
public class SkillManager {
    
    public static event SkillHandler passiveTeamBuffHandler;
    //passive Team buff
    public static TeamBuff p_TeamBuff;
    //active Team buff
    private TeamBuff a_TeamBuff;
    public SkillManager(){
        p_TeamBuff.Init();
        passiveTeamBuffHandler=null;
        if(null!=passiveTeamBuffHandler) Debug.LogError("핸들러 초기화 에러");
    }
    ~SkillManager(){
        p_TeamBuff.Init();
        passiveTeamBuffHandler=null;
        if(null!=passiveTeamBuffHandler) Debug.LogError("핸들러 초기화 에러-소멸자");
    }
    void AffectPTBSkills(){
        p_TeamBuff.Init();
        if(null!=passiveTeamBuffHandler) passiveTeamBuffHandler.Invoke();
    }

    public void SelectHero(int heroIdx){
        Skill skill=HerosManager.Instance.heroInfo[heroIdx].GetSkill();
        if(null==skill) return;
        switch(skill.SkillType()){
            case EnumTypes.SkillType.PassiveTeamBuff:
                passiveTeamBuffHandler-=skill.Affect;
                passiveTeamBuffHandler+=skill.Affect;
                break;
        }

        AffectPTBSkills();
    }

    public void UnSelectHero(int heroIdx){
        Skill skill=HerosManager.Instance.heroInfo[heroIdx].GetSkill();
        if(null==skill) return;
        switch(skill.SkillType()){
            case EnumTypes.SkillType.PassiveTeamBuff:
                passiveTeamBuffHandler-=skill.Affect;
                break;
        }

        AffectPTBSkills();
    }


}
