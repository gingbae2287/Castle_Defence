using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIngame : MonoBehaviour{
    [SerializeField] Text moneyText;
    [SerializeField] Text waveText;

    private void Start() {
        SetMoneyText(GameManager.Instance.money);
        SetWaveText(GameManager.Instance.waveLevel);
    }
    public void SetMoneyText(int money){

        moneyText.text=money.ToString();
    }

    public void SetWaveText(int wave){
        waveText.text="Wave: "+wave;
    }
}