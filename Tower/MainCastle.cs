using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCastle : MonoBehaviour{
    ///  =====    UI   ==============
    [Header("HP UI")]
    [SerializeField]Image hpFillBar;
    [SerializeField] Text hpText;

    IEnumerator cor;
    WaitForSeconds gameOverDelay;
    private int level;
    private int maxHp;
    private int hp;
    private int maxMp;
    private int mp;
    private int defense;

    private bool isLive;

    public int Hp { get { return hp; } set { hp = value; } }
    public int MaxHp { get { return maxHp; } set { maxHp = value; } }
    public int Mp { get { return mp; } set { mp = value; } }
    public int MaxMp { get { return maxMp; } set { maxMp = value; } }
    public int Defense { get { return defense; } set { defense = value; } }
    private void Awake() {
        //castleCollider=GetComponent<EdgeCollider2D>();
        gameOverDelay=new WaitForSeconds(2f);
        
    }

    void Start(){
        level = 1;
        maxHp = 1000;
        hp = maxHp;
        maxMp = 30;
        mp = 30;
        defense = 5;
        isLive=true;
        HpBarUpdate();
    }

    private void OnEnable() {
        GameManager.Instance.WaveStartFuncs-=WaveStart;
        GameManager.Instance.WaveStartFuncs+=WaveStart;
        GameManager.Instance.WaveStopFuncs-=WaveStop;
        GameManager.Instance.WaveStopFuncs+=WaveStop;
    }


    void WaveStart(){
        isLive=true;
        hp=maxHp;
        mp=maxMp;
        HpBarUpdate();
        //castleCollider.enabled = true;
    }
    void WaveStop(){
        isLive=true;
        hp=maxHp;
        HpBarUpdate();
        mp=maxMp;
        //castleCollider.enabled = true;
    }

    public void Damaged(int damage){
        if(!isLive) return;
        int realdamage=damage-defense;
        if(hp<realdamage) {
            hp=0;
            Broken();
        }
        else {
            hp-=realdamage;
        }
        HpBarUpdate();
    }

    void Broken(){  //성 부서짐.
        isLive=false;
        cor=GameOver();
        StartCoroutine(cor);
    }
    void HpBarUpdate(){
        hpFillBar.fillAmount=(float)hp/MaxHp;
        hpText.text=hp+" / "+maxHp;
    }
    IEnumerator GameOver(){
        yield return gameOverDelay;
        GameManager.Instance.GameOver();
    }

    //영웅 등록 함수
    

}