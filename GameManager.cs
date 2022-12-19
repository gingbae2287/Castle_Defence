using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//게임 전체적인 시스템 관리 GameManager.cs
//싱글톤으로 작성

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance{
        get{
            if(instance==null) return null;
            return instance;
        }
    }
//===component
    [SerializeField]UIIngame uIIngame;
    [SerializeField] GameData gameData;

    float gameSpeed=1;
    public int waveLevel{get {return gameData.waveLevel;}}
//=== player var====
    public int money{get{return gameData.money;}}
    public float moneyRate{get; private set;}

//Component=====

    public MobManager mobManager;
    public MainCastle mainCastle;
    public HerosManager herosManager;

    public EnumTypes.GameState gameState{get; private set;}
    ///현재 게임상태 변수

    //=====wave 관련 delegate

    public delegate void WaveStartNotification();
    private event WaveStartNotification waveStartFuncs;
    public event WaveStartNotification WaveStartFuncs{
        add{
            waveStartFuncs-=value;
            waveStartFuncs+=value;
        }
        remove{
            waveStartFuncs-=value;
        }
    }
    public delegate void WaveStopNotification();
    private event WaveStopNotification waveStopFuncs;
    public event WaveStopNotification WaveStopFuncs{
        add{
            waveStopFuncs-=value;
            waveStopFuncs+=value;
        }
        remove{
            waveStopFuncs-=value;
        }
    }

    void Awake(){

        if(instance==null){
            instance=this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
        //해상도 설정. 현재 스크린 크기에 맞춤
        Screen.SetResolution( Screen.width, (Screen.width * 16) / 9 , true);

        //입력 없을시 어두워지며 잠기는시간(절전모드시간): 없음 
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Application.targetFrameRate=30;
    }
    
    
    void Start()
    {
        uIIngame.SetWaveText(gameData.waveLevel);
        uIIngame.SetMoneyText(gameData.money);
        gameState=EnumTypes.GameState.Idle;
    }
/*
    void InitPlayerInfo(){
        //money=gameData.money;
        moneyRate = gameData.baseMoneyRate;
    }
    */

///wave start, stop delegate 함수등록
    public void AddWaveStartFunc(WaveStartNotification newWaveStartFunc){
        /*
        foreach (WaveStartNotification n in WaveStartFuncs.GetInvocationList())
        {
            if (n == newWaveStartFunc)
            {
                return;
            }
        }*/
        WaveStartFuncs += newWaveStartFunc;
    }

    public void RemoveWaveStartFunc(WaveStartNotification newWaveStartFunc){
    }

    public void AddWaveStopFunc(WaveStopNotification newWaveStopFunc){
        
    }

    public void RemoveWaveStopFunc(WaveStopNotification newWaveStopFunc){
        
    }
    public void WaveStart(){
        if(gameState!=EnumTypes.GameState.Idle) return;
        gameState=EnumTypes.GameState.Wave;
        waveStartFuncs.Invoke();
        /*
        mobManager.WaveStart();
        mainCastle.WaveStart();
        herosManager.WaveStart();
        */
    }
    public void WaveStop(){
        if(gameState==EnumTypes.GameState.Idle) return;
        gameState=EnumTypes.GameState.Idle;
        waveStopFuncs.Invoke();
        /*
        mobManager.WaveStop();
        mainCastle.WaveStop();
        herosManager.WaveStop();
        */
    }

    public void GameOver(){
        Debug.Log("game over");
        WaveStop();
    }
    public void StageClear(){
        WaveStop();
        uIIngame.SetWaveText(++gameData.waveLevel);
    }
    public void GetMoney(int amount){
        gameData.money+=(int)(amount*gameData.baseMoneyRate);
        uIIngame.SetMoneyText(gameData.money);
    }
    public bool PayMoney(int amount){
        if(gameData.money-amount<0){
            //돈부족
            return false;
        }
        gameData.money-=amount;
        uIIngame.SetMoneyText(gameData.money);
        return true;
    }

}
