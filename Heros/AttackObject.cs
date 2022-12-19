using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject : MonoBehaviour {
//영웅 자식으로 들어갈 공격 오브젝트 - 화살

//component
    Transform tr;


    //공격타입
    //원거리,스킬은 자동타게팅 ai에 따라 알고리즘 다르게. (일단 default로 가까운적)
    public enum AttackType
    {
        Melee,
        Ranged,
        Skill,
    }
// val
    private const float vx=1000f;   //x방향속도
    private float vy;
    private const float g=600;   //중력 x속도에 맞춰 상대적으로 조절
    private float attackRange;
    public int attackDamage;       //최종 공격 데미지
    private bool isattacking;
    Vector2 targetPos;
    private void Awake() {
        ComponentInit();
    }
    void Start(){
        
        tr.localPosition=new Vector3(0, 0.2f, 0);    //부모기준 위치는 localposition
    }
    private void Update() {
        ProjectileMotion();
    }
    void ComponentInit(){
        //tr=GetComponent<Transform>();
        if(!TryGetComponent<Transform>(out tr)) Debug.LogError("tr is null");
    }
    public void Attack(int damage, Vector2 target){
        gameObject.SetActive(true);
        tr.localPosition=new Vector3(0, 0.2f, 0);
        
        targetPos=target;

        float w=targetPos.x-tr.position.x;
        float h=tr.position.y-targetPos.y;
        
        vy=g*w/(vx*2) - vx*h/w;
       
        float angle=Mathf.Atan2(vy,vx)*Mathf.Rad2Deg;
        tr.rotation=Quaternion.AngleAxis(angle,Vector3.forward);

        attackDamage=damage;
        isattacking=true;
    }

    public void ProjectileMotion(){
        if(!isattacking) return;
        float deltaTime=Time.deltaTime;
        //공격용 오브젝트 오른쪽으로 이동
        //캐릭터를 오른쪽을 보게 뒤집어 놔서 오른쪽이 - 방향이 됨. (부모기준이라)

        if((tr.position.x-targetPos.x) * (tr.position.x-targetPos.x)<50 || tr.position.y<targetPos.y-50){
            tr.localPosition=new Vector3(0, 0.2f, 0);
            isattacking=false;
            gameObject.SetActive(false);
            return;
        }

        vy-=g*deltaTime;
        float angle=Mathf.Atan2(vy,vx)*Mathf.Rad2Deg;
        tr.SetPositionAndRotation(tr.position+=new Vector3(vx*deltaTime,vy*deltaTime,0),Quaternion.AngleAxis(angle,Vector3.forward));
        //tr.rotation=Quaternion.AngleAxis(angle,Vector3.forward);
        //tr.position+=new Vector3(vx*Time.deltaTime,vy*Time.deltaTime,0);

        
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(isattacking){

            if(other.CompareTag("Enemy")){
                //StopCoroutine("FindTarget");
                tr.localPosition=new Vector3(0, 0.2f, 0); //오브젝트 히어로 앞으로 다시 이동
                //other.GetComponent<Enemy>().Damaged(attackDamage);  //getcomponent에 null이 발생할 경우 memory allocation이 발생하는데 이를 방지하기위해 try
                if(other.TryGetComponent(out Enemy enemy)) enemy.Damaged(attackDamage);
                isattacking=false;
                gameObject.SetActive(false);

            }
        }
    }
}