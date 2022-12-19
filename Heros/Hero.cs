using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FinalStat{
    public int damage;
    public float atkSpeed;
    public FinalStat(HeroStat baseStat){
        damage=baseStat.damage;
        atkSpeed=baseStat.atkSpeed;
    }
    public void StatInit(HeroStat baseStat){
        //영웅 등록 해제시, 적용중인 다른 영웅들의 패시브 효과를 없엔 순수 stat으로 적용
        damage=baseStat.damage;
        atkSpeed=baseStat.atkSpeed;
    }
    public void SetFinalStat(HeroStat baseStat){
        StatInit(baseStat);
        damage=(int)(damage*(1+SkillManager.p_TeamBuff.DamageIncRate)+SkillManager.p_TeamBuff.DamageInc);
        atkSpeed=atkSpeed*(1+SkillManager.p_TeamBuff.AtkSpeedIncRate)+SkillManager.p_TeamBuff.AtkSpeedInc;
        
    }
}
public class Hero : MonoBehaviour
{
    ///====stat
    [Header("Stat scriptable Object")]
    [SerializeField]HeroStat BaseStat;
    FinalStat FinalStat;

    [Header("Skill scriptable Object")]
    [SerializeField] Skill skill;
    public Skill GetSkill()=>skill;
    



    
    [Header("공격 오브젝트 directory Resources/")]
    public string atkObjPath="Characters/AtkObj/DefaultAtk";
    protected AttackObject[] attackObj;
    ///애니메이션
    protected Animator anim;
    /// 공격관련
    WaitForSeconds attackDelay;
    IEnumerator attcor;
    bool attacking;
    RaycastHit2D hit;
    
    LayerMask enemyLayer;

    public enum TargetAI{
        Closest,
        HighestHp,

    }
    TargetAI targetAI;
    Vector2 targetPosition;
    bool isTarget;

///공격 오브젝트

    public void HeroInit(){
        ComponentInit();
        //공격관련
        CreateAttackObject();
        attcor=CheckAction();
        SetAttackAnim();
        targetAI=TargetAI.Closest;
        ////
        enemyLayer =LayerMask.GetMask("Enemy");
        BaseStat.atkRange=Screen.currentResolution.width/2-transform.position.x;

        StatUpdate();
    }
    void ComponentInit(){
        anim=GetComponentInChildren<Animator>();
        if(anim==null) Debug.LogError("anim is null");

        FinalStat=new FinalStat(BaseStat);
    }
    void CreateAttackObject(){
        /// 공격 오브젝트 (화살)
        attackObj=new AttackObject[4];
        for(int i=0;i<4;i++){
            attackObj[i]=Instantiate(Resources.Load<GameObject>(atkObjPath),transform.position,Quaternion.identity).GetComponent<AttackObject>();
            attackObj[i].gameObject.transform.SetParent(transform);
            attackObj[i].gameObject.SetActive(false);
        }
        
    }
    
    public void StatUpdate(){
        FinalStat.SetFinalStat(BaseStat);
        //영웅 등록 해제시, 적용중인 다른 영웅들의 패시브 효과를 없엔 순수 stat으로 적용
        attackDelay=new WaitForSeconds(0.5f/FinalStat.atkSpeed);
        anim.SetFloat("AttackSpeed",FinalStat.atkSpeed);
    }
    void SetAttackAnim(){
        switch(BaseStat.weaponType){
            case HeroStat.WeaponType.Bow:
            anim.SetFloat("NormalState",0.5f);
            break;

            case HeroStat.WeaponType.Wand:
            anim.SetFloat("NormalState",1.0f);
            break;
        }
        //anim.SetFloat("AttackState",1f);
    }
    bool CheckAttackRange(){
        hit=Physics2D.Raycast(transform.position, Vector2.right, BaseStat.atkRange ,enemyLayer);
        if(hit) return true;
        else return false;
    }

    public void WaveStart(){
        //StopCoroutine(attcor);
        attcor=CheckAction();
        StartCoroutine(attcor);

    }
    public void WaveStop(){
        StopCoroutine(attcor);
    }

    //===========영웅 공격============

    IEnumerator CheckAction(){
        
        while(true){
            yield return attackDelay;
            GetTargetPosition();
            //살아있을때만 공격
            if(isTarget){
                
                PlayAnimation(5);
                yield return attackDelay;
                Attack_Melee();
                    //공격속도와 애니메이션 딜레이 고려하기
            }
            if(GameManager.Instance.gameState==EnumTypes.GameState.Idle){
                //게임중 아닐때 break;
                break;
            }
        }
    }

    void GetTargetPosition(){
        switch(targetAI){
            case TargetAI.Closest:
                /*hit=Physics2D.Raycast(transform.position, Vector2.right, stat.atkRange ,enemyLayer);
                if(hit){
                    targetPosition=hit.transform.position;
                    isTarget=true;
                    break;
                }
                */
                if(!MobManager.Instance.closestMob.isLive) {
                    if(MobManager.Instance.UpdateTargetAI()) {
                        targetPosition=MobManager.Instance.closestMob.transform.position;
                        isTarget=true;
                    }
                    else isTarget=false;
                    break;
                }
                targetPosition=MobManager.Instance.closestMob.transform.position;
                isTarget=true;
                //targetPosition=Vector2.zero;
                //isTarget=false;
                break;
            case TargetAI.HighestHp:
                if(!MobManager.Instance.highestHpMob.isLive) {
                    if(MobManager.Instance.UpdateTargetAI()) {
                        targetPosition=MobManager.Instance.highestHpMob.transform.position;
                        isTarget=true;
                    }
                    else isTarget=false;
                    break;
                }
                targetPosition=MobManager.Instance.highestHpMob.transform.position;
                isTarget=true;
                
            break;
        }
        
    }

    public void Attack_Melee(){
        //_playerState = PlayerState.attack;
        foreach(AttackObject obj in attackObj){
            if(!obj.gameObject.activeSelf){
                obj.Attack(FinalStat.damage,targetPosition);
                break;
            }
        }
        
    }

/*
    IEnumerator ProjectileMovement(){
        while(GameManager.Instance.gameState==EnumTypes.GameState.Wave){
            for(int i=0;i<attackObj.Length;i++) attackObj[i].ProjectileMotion(0.05f);
            yield return projectileFps;
        }
        for(int i=0;i<attackObj.Length;i++) attackObj[i].gameObject.SetActive(false);
    }
*/
    public void PlayAnimation (int num)
    {
        switch(num)
        {
            case 0: //Idle
            anim.SetFloat("RunState",0f);
            break;

            case 3: //Stun
            anim.SetFloat("RunState",1.0f);
            break;

            case 4: //Attack Sword
            anim.SetTrigger("Attack");
            anim.SetFloat("AttackState",0.0f);
            anim.SetFloat("NormalState",0.0f);
            break;

            case 5: //Attack Bow
            anim.SetTrigger("Attack");
            
            
            break;

            case 6: //Attack Magic
            anim.SetTrigger("Attack");
            anim.SetFloat("AttackState",0.0f);
            anim.SetFloat("NormalState",1.0f);
            break;

            case 7: //Skill Sword
            anim.SetTrigger("Attack");
            anim.SetFloat("AttackState",1.0f);
            anim.SetFloat("NormalState",0.0f);
            break;

            case 8: //Skill Bow
            anim.SetTrigger("Attack");
            anim.SetFloat("AttackState",1.0f);
            anim.SetFloat("NormalState",0.5f);
            break;

            case 9: //Skill Magic
            anim.SetTrigger("Attack");
            anim.SetFloat("AttackState",1.0f);
            anim.SetFloat("NormalState",1.0f);
            break;
        }
    }
    public bool LevelUp(int amount=1){
        if(!GameManager.Instance.PayMoney(BaseStat.lvUpCost)) return false;
        BaseStat.LevelUp(amount);
        FinalStat.SetFinalStat(BaseStat);
        return true;
    }

    public string GetInfo()=>BaseStat.name+"\nLv: "+BaseStat.lv+"  Damage: "+FinalStat.damage;
    public string GetName()=>BaseStat.name;
    public int GetDamage()=>FinalStat.damage;
    public int GetLevel()=>BaseStat.level;
    public int GetLvUpCost()=>BaseStat.lvUpCost;
    public float GetAtkSpeed()=>FinalStat.atkSpeed;

}