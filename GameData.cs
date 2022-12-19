using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game Data")]

[System.Serializable]
public class GameData : ScriptableObject{
    public int waveLevel=1;
    public int money=0;
    public float baseMoneyRate=1f;
}