using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 히어로 선택 목록

public class HerosManager : MonoBehaviour{
    private static HerosManager instance = null;
    public static HerosManager Instance{
        get{
            if(instance==null) return null;
            return instance;
        }
    }
    //==Component=====
    public MainCastle caslte;
    SkillManager skillManager=new SkillManager();
    [SerializeField]HeroSlotManager slotManager;
    //=====List UI==========
    [SerializeField] GameObject HeroListUI;
    [SerializeField] GameObject heroInfoCanvas;
    [SerializeField] GameObject HeroSlot;
    [SerializeField] HeroInfoUI heroInfoUI;


    //=====heros
    public List<GameObject> HeroList;   //해당 리스트에 영웅정보가 모두 담김
    public List<Hero> heroInfo;
    [SerializeField] Transform heroListParent;
    GameObject heroListPrefab;

    string HeroPath="Characters/Heros";
    string listPrefabPath="UI/SlotHeroList";


    void Awake(){
        if(instance==null){
            instance=this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }
    private void Start() {
        CreateHeroList();
        GameManager.Instance.WaveStartFuncs+=WaveStart;
        GameManager.Instance.WaveStopFuncs+=WaveStop;

    }
    void CreateHeroList(){
        GameObject hero;
        GameObject listSlot;
        heroListPrefab=Resources.Load<GameObject>(listPrefabPath);
        GameObject[] TmpList=Resources.LoadAll<GameObject>(HeroPath);

        for(int i=0;i<TmpList.Length;i++){
            SpriteRenderer[] renderers=TmpList[i].GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer renderer in renderers)
                renderer.maskInteraction=SpriteMaskInteraction.VisibleOutsideMask;

            hero=Instantiate(TmpList[i], transform.position, Quaternion.identity);
            TmpList[i]=Instantiate(TmpList[i], transform.position, Quaternion.identity);
            HeroList.Add(hero);
            heroInfo.Add(hero.GetComponent<Hero>());
            heroInfo[i].HeroInit();

            HeroList[i].transform.SetParent(transform);

            listSlot=Instantiate(heroListPrefab,Vector3.zero,Quaternion.identity);
            listSlot.transform.SetParent(heroListParent);
            listSlot.GetComponent<SlotHeroList>().SetSlot(TmpList[i],i);

            
            HeroList[i].SetActive(false);
        }
    }
    ///Game
    void WaveStart(){
        slotManager.WaveStart();
    }
    void WaveStop(){
        slotManager.WaveStop();
    }
///==== skill buff value
/*
    void InitTeamBuffValue(){
        damageIncRate=1f;
        damageInc=0;
        atkSpeedIncRate=1f;
        atkSpeedInc=0;
    }
*/
    //UI
    public void ShowHeroInfo(int heroIdx){
        heroInfoCanvas.SetActive(true);
        heroInfoUI.ShowHeroInfo(heroIdx);
    }
    public void SelectHero(int heroIdx){
        slotManager.ChangeSlot(heroIdx);
        heroInfoCanvas.SetActive(false);
        skillManager.SelectHero(heroIdx);
        UpdateHeroInfo();
        CloseHeroListUI();
    }
    public void UnSelectHero(int heroIdx){
        HeroList[heroIdx].transform.SetParent(transform);
        skillManager.UnSelectHero(heroIdx);
        UpdateHeroInfo();
        HeroList[heroIdx].SetActive(false);
        
    }

    public void UpdateHeroInfo(){
        for(int i=0;i<heroInfo.Count;i++)
            heroInfo[i].StatUpdate();
    }
    public void HeroListUIOn(){
        //
        HeroListUI.SetActive(true);
    }

    public void CloseHeroListUI(){
        heroInfoCanvas.SetActive(false);
        HeroListUI.SetActive(false);
    }
}