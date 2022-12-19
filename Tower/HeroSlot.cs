using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSlot : MonoBehaviour {
    HeroSlotManager slotManager;
    private bool isEmpty=true;
    public bool IsEmpty{get{return isEmpty;}}
    GameObject hero; 
    Hero heroScript;
    public int heroIdx{get; private set;}
    public int slotIdx{get; private set;}

    
    void Start(){
         heroIdx=-1;
    }

    public void SetIndex(int idx, HeroSlotManager manager){
        slotIdx=idx;
        slotManager=manager;
    }

    //영웅 죽이거나 살릴때 heroaction 에서 이함수 호출
    public void WaveStart(){
        if(isEmpty) return;
        heroScript.WaveStart();

    }

    public void WaveStop(){
        if(isEmpty) return;
        heroScript.WaveStop();

    }

    public void Bt_ClickCastleSlot(){
        ///영웅 슬롯 클릭시
        if(GameManager.Instance.gameState!=EnumTypes.GameState.Idle) return;
        slotManager.ClickSlot(slotIdx);

    }

    //슬롯의 영웅을 변경
    public void ChangeHero(int HeroIdx){
        //영웅이 이미 있으면 원래자리로 돌려놈

        if(!isEmpty) HerosManager.Instance.UnSelectHero(heroIdx);
        heroIdx=HeroIdx;
        
        hero=HerosManager.Instance.HeroList[heroIdx];
        heroScript=hero.GetComponent<Hero>();
        hero.SetActive(true);
        //hero.transform.SetParent(transform);
        hero.transform.localScale=new Vector3(-80,80,80);
        hero.transform.position=transform.position;
        //hero.transform.localPosition=new Vector3(0,-0.5f,0);
        isEmpty=false;
    }
    public void ClearSlot(){
        HerosManager.Instance.UnSelectHero(heroIdx);
        heroScript=null;
        isEmpty=true;
        heroIdx=-1;
        hero=null;
    }
}