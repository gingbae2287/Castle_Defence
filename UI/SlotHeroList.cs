using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// 히어로 정보 보여주기

public class SlotHeroList : MonoBehaviour{
    GameObject heroObj;

    Hero hero;
    [SerializeField] GameObject heroImage;
    [SerializeField] Text heroInfo;
    [SerializeField] Text costText;
    public int heroIdx{get; private set;}

    bool isSet;

    //public bool IsEmpty{get{return _isEmpty;}set{_isEmpty=value;}}

    void Awake(){

    }
    void OnEnable(){
        if(!isSet) return;
        UpdateInfo();
    }
    //일단 프리펩 가져와서 보여주기 까지만 구현함
    public void SetSlot(GameObject Hero, int idx){
        heroObj=Hero;
        heroObj.transform.SetParent(heroImage.transform);
        heroObj.transform.localScale=new Vector3(64,64,1);
        heroObj.transform.localPosition=new Vector3(50,-20);

        heroIdx=idx;
        hero=HerosManager.Instance.HeroList[heroIdx].GetComponent<Hero>();
        heroInfo.text=hero.GetInfo();
        isSet=true;
    }

    void UpdateInfo(){
        heroInfo.text=hero.GetInfo();
        costText.text=hero.GetLvUpCost().ToString();
    }

    public void LvUpButton(){
        if(hero==null) Debug.LogError("there are no hero");
        if(!hero.LevelUp()) return;
        UpdateInfo();
    }

    public void ClickHeroList(){
        HerosManager.Instance.ShowHeroInfo(heroIdx);
          
    }
}