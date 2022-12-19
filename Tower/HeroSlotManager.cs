using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSlotManager : MonoBehaviour {
    HeroSlot[] slots;
    int[] heroIdxInSlot;
    int currentSlot;

    private void Awake() {
        slots=GetComponentsInChildren<HeroSlot>();
        heroIdxInSlot=new int[slots.Length];
        for(int i=0;i<slots.Length;i++){
            slots[i].SetIndex(i,this);
            heroIdxInSlot[i]=-1;
        }
        currentSlot=-1;
    }

    public void WaveStart(){
        for(int i=0;i<slots.Length;i++){
            slots[i].WaveStart();
        }
    }

    public void WaveStop(){
        for(int i=0;i<slots.Length;i++){
            slots[i].WaveStop();
        }
    }
    public void ClickSlot(int CurrentSlot){
        currentSlot=CurrentSlot;
        HerosManager.Instance.HeroListUIOn();
    }

    public void ChangeSlot(int heroIdx){
        //if(idx>=slots.Length) return;
        for(int i=0;i<heroIdxInSlot.Length;i++){
            if(heroIdxInSlot[i]==heroIdx){
                heroIdxInSlot[i]=-1;
                slots[i].ClearSlot();
                break;
            }
        }
        heroIdxInSlot[currentSlot]=heroIdx;
        slots[currentSlot].ChangeHero(heroIdx);

    }
}