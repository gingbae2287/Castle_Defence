using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy: MonoBehaviour
{
    public int hp{get; protected set;}
    public int level{get; protected set;}
    public abstract void Damaged(OnHit onHit);

    public virtual void Stun(){
    }

    public virtual void Knockback(){
    }
}