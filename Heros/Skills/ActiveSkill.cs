using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/create Skill", order = 0)]


[System.Serializable]
public class ActiveSkill : ScriptableObject {
    float coolTime;
    [field: TextArea] public string Description;
}