using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///
/// PTB = Passive Team Buff

public delegate void SkillHandler();
public class SkillManager {
    
    public static event SkillHandler passiveTeamBuffHandler;
    //passive Team buff
    public static st.TeamBuff p_TeamBuff;
    //active Team buff
    private st.TeamBuff a_TeamBuff;
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
