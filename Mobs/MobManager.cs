using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobManager : MonoBehaviour{

    private static MobManager instance = null;
    public static MobManager Instance{
        get{
            if(instance==null) return null;
            return instance;
        }
    }

//===castle
     [SerializeField] MainCastle castle;
//====mob
    public GameObject[] prefabsMob;
    MobStat[] mobStats;
    string path="Characters/Mobs/";

// pos  ==================
    [SerializeField] Transform mobStartPoint;  //몹 시작 위치
    public Transform EndofMap;

//=========
    List<Mob> Mobs=new List<Mob>();
    

    //몬스터 타게팅 AI
    
    List<Mob> liveMobs=new List<Mob>();
    public Mob closestMob;
    public Mob highestHpMob;




    int mobCount;

    /////
    WaitForSeconds mobSpawnDelay;
    
    bool waveing;
    IEnumerator cor;

    private void Awake() {
        if(instance==null){
            instance=this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
        mobSpawnDelay=new WaitForSeconds(1f);

        mobStats=Resources.LoadAll<MobStat>(path);   
    }

    void Start(){
        CheckComponent();
        waveing=false;
        CreateMobs();   
        GameManager.Instance.WaveStartFuncs+=WaveStart;
        GameManager.Instance.WaveStopFuncs+=WaveStop;
    }

    void CheckComponent(){
        if(castle==null) Debug.LogError("castle is null");
        if(mobStartPoint==null) Debug.LogError("mobStartPoint is null");
    }
    //웨이브 시작시
    public void WaveStart(){
        
        if(waveing) return;
        foreach(MobStat stat in mobStats) stat.SetLevel(GameManager.Instance.waveLevel);

        mobCount=Mobs.Count;
        mobSpawnDelay=new WaitForSeconds(10f/mobCount);

        waveing=true;
        
        cor=ActiveMobs();
        StartCoroutine(cor);
        //ChangeTarget();
    } 

    public void WaveStop(){
        if(!waveing) return;
        waveing=false;
        StopCoroutine(cor);
        for(int i=0;i<Mobs.Count;i++){
            Vector3 tmpPosition=mobStartPoint.position;
            tmpPosition.y=Random.Range(-100f,0);
            Mobs[i].ResetMob(tmpPosition);
        }
        liveMobs.Clear();
    }

    void CreateMobs(){
        GameObject Mob;
        for(int i=0;i<prefabsMob.Length;i++){
            mobCount=(i+3)*(i+4); //몹당 생성되는 마리수 공식 (추후 변경)
            for(int k=0;k<mobCount;k++) {
                Vector3 tmpPosition=mobStartPoint.position;
                tmpPosition.y=Random.Range(-100f,0);
                Mob=Instantiate(prefabsMob[i], tmpPosition,Quaternion.identity);
                Mob.transform.SetParent(transform);
                Mob.transform.localScale=new Vector3(75f,75f,75f);
                Mobs.Add(Mob.GetComponent<Mob>());
                Mobs[Mobs.Count-1].ResetMob(tmpPosition);
            }
        }
        closestMob=Mobs[0];
        highestHpMob=Mobs[0];
    }

    public bool UpdateTargetAI(){
        float x=1000f;
        int hp=0;
        if(liveMobs.Count==0) return false;
        for(int i=liveMobs.Count-1;i>=0;--i){
            //remove시 c++ vector 처럼 당겨지기 때문에 뒤에서부터 탐색
            if(!liveMobs[i].isLive) liveMobs.RemoveAt(i);
            else if(liveMobs[i].transform.position.x<x){
                x=liveMobs[i].transform.position.x;
                closestMob=liveMobs[i];
            }
        }
        return true;
    }

    public void AddLiveMob(Mob mob)=>liveMobs.Add(mob);
    public void RemoveLiveMob(Mob mob)=>liveMobs.Remove(mob);


    IEnumerator ActiveMobs(){
        for(int i=0;i<Mobs.Count;i++){
            Mobs[i].gameObject.SetActive(true);
            AddLiveMob(Mobs[i]);
            yield return mobSpawnDelay; //1초마다 한마리 생성.
        }
    }
    public void AttackCastle(int Damage){
        castle.Damaged(Damage);
    }
    public void MobDie(){
        if(--mobCount<=0) GameManager.Instance.StageClear();
    }




}