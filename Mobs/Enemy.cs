using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy: MonoBehaviour
{
    public int hp;
    public abstract void Damaged(int Damage);
}