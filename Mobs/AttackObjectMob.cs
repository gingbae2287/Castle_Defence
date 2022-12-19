using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjectMob : MonoBehaviour {
    //형태없는 공격용 오브젝트. (collider만 있음) hero의 자식

    //공격타입
    //원거리,스킬은 자동타게팅 ai에 따라 알고리즘 다르게. (일단 default로 가까운적)
    public enum AttackType
    {
        Melee,
        Ranged,
        Skill,
    }
    public GameObject _parentMob;
    public float attackObjSpeed;
    private int _attackDamage;       //최종 공격 데미지
    private float _attackRange;
    private bool _isattacking;
    int castleLayer;
    private void Awake() {
        castleLayer=1<<LayerMask.NameToLayer("Castle");
    }

    void Start(){
        transform.localPosition=new Vector3(0, 0.2f, 0);    //부모기준 위치는 localposition
        //attackObjSpeed=15f;
    }

    void Update(){
        if(_isattacking) //공격범위 지정
        {
            //StopCoroutine("FindTarget"); 
            //transform.localPosition=new Vector3(0, 0.2f, 0);
            FindTarget();
        }
    }
    public void Attack(int damage, float attackRange){
        if(_isattacking) return;
        _attackDamage=damage;
        _attackRange=attackRange;
        _isattacking=true;
        
        //HeorActions.cs에서 어택 명령시 실행
        //StartCoroutine("FindTarget");
    }

    void FindTarget(){
        
        //공격용 오브젝트 오른쪽으로 이동
        //프레임에따라 공격타게팅을 찾는게 맞는거 같아 deltatime안씀
        //캐릭터를 오른쪽을 보게 뒤집어 놔서 오른쪽이 - 방향이 됨. (부모기준이라)
        /*for(int i=0;i<200;i++){
            //Debug.Log("for문 실행"+i);
            transform.Translate(new Vector3(attackObjSpeed*(-1),0,0)); 
            yield return null;
        }*/
    
        if(Mathf.Abs(transform.localPosition.x)>_attackRange) {    //x가 -로 향해가므로 절댓값 씌워줘야함
            transform.localPosition=new Vector3(0,0.4f,0);
            _isattacking=false;
            return;
        }
        transform.Translate(new Vector3(attackObjSpeed*(-1)*Time.deltaTime,0,0));
        //transform.localPosition=new Vector3(0, 0.2f, 0);
    }
    

}