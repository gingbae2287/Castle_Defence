using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob: Enemy
{
    public AttackObjectMob attackObj;

    //Transform _target;

    //타겟 안쓰고 레이로 layermask 체크
    
    Animator anim;
    

    IEnumerator cor;
    WaitForSeconds attackDelay;
    WaitForSeconds delay;
    WaitForSeconds oneSecDelay;
    BoxCollider2D col;

    RaycastHit2D hit;

    LayerMask CastleLayer;
    ///=-=======
    public MobStat stat;
    public int level{get; private set;}
    public int curHP{get; private set;}
    int damage;
    bool attacking;
    bool isRun;
    public bool isLive{get; private set;}
    private void Awake() {
        
        
        delay=new WaitForSeconds(0.1f);
        oneSecDelay=new WaitForSeconds(1f);
        col=GetComponent<BoxCollider2D>();
        anim=GetComponentInChildren<Animator>();
        if(anim==null){Debug.LogError("anim  is null");}
        
        CastleLayer =LayerMask.GetMask("Castle");
        
    }

    void Start(){
        Init();
        
    }

    void Init(){
        isLive=true;
        isRun=false;
        attackDelay=new WaitForSeconds(0.5f/stat.atkSpeed);
        anim.SetFloat("AttackSpeed",stat.atkSpeed);
        RunState();
    }

    private void OnEnable() {
        SetStat();
        PlayAnimation(3);
        cor=CheckAction();
        StartCoroutine(cor);
    }
    private void OnDisable() {
        isLive=false;
        StopCoroutine(cor);
    }

    void SetStat(){
        //level = stat.lv;
        curHP = stat.maxHp;
        damage= stat.damage;

    }

    void Update(){
        Run();
    }

    //사거리 이내일시 공격
    //공격액션을 담당하는 코루틴
    IEnumerator CheckAction(){
        isLive=true;
        col.enabled=true;
        
        while(true){
            if(!isLive) break;
            //적이 사거리 내에 잇으면
            if(CheckRange()){ 
                StopRun();
                if(!attacking){
                    attacking=true;
                    Attack_Melee();
                    yield return attackDelay;
                    MobManager.Instance.AttackCastle(damage);
                }
                
                     //공격속도와 애니메이션 딜레이 고려하기
                attacking=false;
            }
            //사거리 내에 없으면
            else {
                RunState();
            }
            yield return attackDelay;
        }
    }

    public void SetAttackSpeed(float AttSpeed){
        //stat.atkSpeed=AttSpeed;
        attackDelay=new WaitForSeconds(0.5f/stat.atkSpeed);
        anim.SetFloat("AttackSpeed",stat.atkSpeed);
    }
    bool CheckRange(){
        hit=Physics2D.Raycast(transform.position, Vector2.left, stat.atkRange ,CastleLayer);
        if(hit) return true;
        else return false;
    }
    void Run(){
        if(!isRun || !isLive) return;
        transform.Translate(new Vector3(stat.speed*(-1f)*Time.deltaTime,0,0));
    }
    //이동상태로 바꿔줌
    private void RunState(){
        if(!isRun){
            isRun=true;
            PlayAnimation(1);
        }
    }
    void StopRun(){
        if(isRun){
            isRun=false;
            PlayAnimation(0);
        }
    }
    
    //공격 오브젝트를 날려? 공격을 하는 함수
    private void Attack_Melee(){
        

        //hit.collider.gameObject.GetComponent<MainCastle>().Damaged(stat.damage);
        PlayAnimation(4);
        
    }
    //대미지 입었을때 공격 오브젝트에서 호출

    public override void Damaged(int Damage){
        if(!isLive) return;
        //int realdamage=damage-de;
        
        if(curHP<Damage) Die();
        else {
            curHP-=Damage;
            //Debug.Log(gameObject.GetInstanceID()+"남은 hp: "+curHP );
        }
    }

    public void Die(){
        MobManager.Instance.RemoveLiveMob(this);
        col.enabled=false;
        isLive=false;
        isRun=false;
        GameManager.Instance.GetMoney(stat.killMoney);
        
        StartCoroutine("DieTimer");
    }

    //게임 종료시 몹 초기화
    public void ResetMob(Vector3 position){
        gameObject.transform.position=position;
        attackObj.transform.localPosition=new Vector3(0,0.4f,0);
        isRun=false;
        col.enabled=true;
        gameObject.SetActive(false);
        
    }

    IEnumerator DieTimer(){
        PlayAnimation(2);
        yield return oneSecDelay;
        MobManager.Instance.MobDie();
        gameObject.SetActive(false);
        //
    }



    public void PlayAnimation (int num)
    {
        switch(num)
        {
            case 0: //Idle
            anim.SetBool("Run",false);
            break;

            case 1: //Run
            anim.SetBool("Run",true);
            break;

            case 2: //Death
            anim.SetBool("IsLive",false);
            //anim.SetBool("Run",false);
            //anim.SetTrigger("Die");
            break;

            case 3: //Live
            anim.SetBool("IsLive",true);
            break;

            case 4: //Attack Sword
            anim.SetBool("Run",false);
            anim.SetTrigger("Attack");
            
            //anim.SetFloat("AttackState",0.0f);
            //anim.SetFloat("NormalState",0.0f);
            
            break;

            case 5: //Attack Bow
            anim.SetTrigger("Attack");
            anim.SetFloat("AttackState",0.0f);
            anim.SetFloat("NormalState",0.5f);
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
}
