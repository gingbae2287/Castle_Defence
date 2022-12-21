using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster: Enemy
{
    public MobStat stat;
    Transform tr;
    Rigidbody2D rg;
    protected IEnumerator cor;
    //protected WaitForSeconds attackDelay;
    protected WaitForSeconds delay;
    protected WaitForSeconds oneSecDelay;
    protected BoxCollider2D col;

    protected RaycastHit2D hit;

    protected LayerMask CastleLayer;
    ///=-=======
    
    
    public int curHP{get; protected set;}
    protected int damage;
    protected float hAttackSpeed;   
    //half 애니메이션 이벤트 설정이 번거로워 애니메이션 중간에 데미지를 주는 효과를 위해.
    
    

    //Action===
    protected bool isRun;
    public bool isLive{get; protected set;}
    protected float attackTime;
    protected bool isAttack;
    float stunTime;
    float knockbackDis;

    
    Animator anim;
    

    public void InitMonster(){
        ///component
        oneSecDelay=new WaitForSeconds(1f);
        if(!TryGetComponent<BoxCollider2D>(out col))
            Debug.LogError("col is null");
        if(!TryGetComponent<Transform>(out tr))
            Debug.LogError("tr is null");
        if(!TryGetComponent<Rigidbody2D>(out rg))
            Debug.LogError("rg is null");
        anim=GetComponentInChildren<Animator>();
        if(anim==null){Debug.LogError("anim  is null");}
        CastleLayer =LayerMask.GetMask("Castle");

        isRun=false;
        //attackDelay=new WaitForSeconds(0.5f/stat.atkSpeed);
        delay=MobManager.Instance.delay;
        hAttackSpeed=0.5f/stat.atkSpeed;
        anim.SetFloat("AttackSpeed",stat.atkSpeed);
    }
    private void OnDisable() {
        isLive=false;
        isRun=false;
        //StopCoroutine(cor);
    }
    public void WaveStart(){
        gameObject.SetActive(true);
        SetStat();
        PlayAnimation(3);

        isLive=true;
        col.enabled=true;

        cor=CheckAction();
        StartCoroutine(cor);
    }
    void SetStat(){
        //level = stat.lv;
        curHP = stat.maxHp;
        damage= stat.damage;

    }

    void Update(){
        Run();
        if(knockbackDis>0){
            float d=stat.speed*3*Time.deltaTime;
            tr.Translate(Vector3.right*d);
            knockbackDis-=d;
        }
    }

    //사거리 이내일시 공격
    //공격액션을 담당하는 코루틴
    IEnumerator CheckAction(){
        stunTime=0;
        attackTime=0;
        while(true){
            if(!isLive) break;
            if(CheckRange()){ 
                //적이 사거리 내에 잇으면
                StopRun();
                if(!isAttack && attackTime<=0){
                    isAttack=true;
                    Attack_Melee(); //애니메이션
                    attackTime=hAttackSpeed*2;
                }
                else if(isAttack && attackTime<=hAttackSpeed) {
                    MobManager.Instance.AttackCastle(damage);
                    
                    isAttack=false;
                }
            
                
            }
            //사거리 내에 없으면
            else {
                RunState();
                isAttack=false;
                attackTime=0;
            }
            while(stunTime>=0){
                StopRun();
                isAttack=false;
                attackTime=0;
                stunTime-=0.1f;
                PlayAnimation(5);
                yield return delay;
            }
            attackTime-=0.1f;   //delay=0.1f
            yield return delay;
            
            
        }
    }

    public void SetAttackSpeed(float AttSpeed){
        //stat.atkSpeed=AttSpeed;
        //attackDelay=new WaitForSeconds(0.5f/stat.atkSpeed);
        //anim.SetFloat("AttackSpeed",stat.atkSpeed);
    }
    bool CheckRange(){
        hit=Physics2D.Raycast(tr.position, Vector2.left, stat.atkRange ,CastleLayer);
        if(hit) return true;
        else return false;
    }
    void Run(){
        if(!isRun || !isLive) return;
        tr.Translate(new Vector3(stat.speed*(-1f)*Time.deltaTime,0,0));
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
        PlayAnimation(4);  
    }
    //대미지 입었을때 공격 오브젝트에서 호출

    public override void Damaged(OnHit onHit){
        if(!isLive) return;
        //int realdamage=damage-de;
        
        if(curHP<onHit.Damage) Die();
        else {
            curHP-=onHit.Damage;

            if(onHit.Stun) Stun();
            if(onHit.Knockback) Knockback();
            //Debug.Log(gameObject.GetInstanceID()+"남은 hp: "+curHP );
        }
    }

    public override void Stun()=>stunTime=1f; 

    public override void Knockback() => knockbackDis+=50f;

    IEnumerator StunCoroutine(){
        StopCoroutine(cor);
        while(stunTime>0){
            yield return delay;
            stunTime-=0.1f;
        }
        StartCoroutine(cor);
    }

    public void Die(){
        MobManager.Instance.RemoveLiveMob(this);
        StopCoroutine(cor);

        col.enabled=false;
        isLive=false;
        isRun=false;
        GameManager.Instance.GetMoney(stat.killMoney);
        
        StartCoroutine("DieTimer");
    }

    //게임 종료시 몹 초기화
    public void ResetMob(Vector3 position){
        tr.position=position;
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
    void PlayAnimation (int num)
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

            case 5: //Stun
            anim.SetTrigger("Stun");
            break;
        }
    }
}