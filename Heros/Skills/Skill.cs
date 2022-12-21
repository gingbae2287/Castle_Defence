using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Skill : ScriptableObject {
    [field: TextArea] public string Description;
    public abstract EnumTypes.SkillType SkillType();
    public abstract void Affect();
    protected Hero owner;
    public virtual void SetOwner(Hero hero)=>owner=hero;
}