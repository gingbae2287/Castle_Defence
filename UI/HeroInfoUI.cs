using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;
public class HeroInfoUI : MonoBehaviour{
    [SerializeField]Transform heroObjParent;
    [SerializeField] Text text_name;
    [SerializeField] Text text_level;
    [SerializeField] Text text_damage;
    [SerializeField] TMP_Text text_atkSpeed;

    GameObject heroObj;
    public int curHero;
    public void ShowHeroInfo(int heroIdx){
        if(heroIdx>=HerosManager.Instance.HeroList.Count) return;
        curHero=heroIdx;
        if(heroObj!=null) Destroy(heroObj);
        gameObject.SetActive(true);
        heroObj=Instantiate(HerosManager.Instance.HeroList[heroIdx],heroObjParent.transform.position,Quaternion.identity);
        heroObj.transform.SetParent(heroObjParent);
        heroObj.SetActive(true);
        heroObj.GetComponentInChildren<SortingGroup>().sortingOrder=6;
        heroObj.transform.localScale=new Vector3(128,128,1);

        text_name.text=HerosManager.Instance.heroInfo[heroIdx].GetName();
        text_level.text="Lv: "+HerosManager.Instance.heroInfo[heroIdx].GetLevel().ToString();
        text_damage.text="Damage: "+HerosManager.Instance.heroInfo[heroIdx].GetDamage().ToString();
        text_atkSpeed.text="Atk Speed: "+HerosManager.Instance.heroInfo[heroIdx].GetAtkSpeed().ToString("F3");
    }
    public void SelectHero(){
        HerosManager.Instance.SelectHero(curHero);  
        //gameObject.SetActive(false);
    }
}