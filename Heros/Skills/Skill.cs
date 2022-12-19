using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/create Skill", order = 0)]


[System.Serializable]
public abstract class Skill : ScriptableObject {
    [field: TextArea] public string Description;
    public abstract EnumTypes.SkillType SkillType();

    public abstract void Affect();
}