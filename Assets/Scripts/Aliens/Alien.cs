using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Alien: MonoBehaviour
{
    //// FSM
    //public State currentState;

    public virtual void MoveToTarget() { }
    public virtual void MoveToPlayer() { }
    public virtual void MoveToBuilding() { }
    public virtual void Hover() { }
    public virtual void FireAtPlayer() { }
    public abstract void TakeDamage(Collision collision);
    public virtual void HitBuilding(Collision collision) { }
    public abstract void IsAlive();
    public virtual void SelfDestruct() { }
    public virtual void LookAtPlayer() { }
}

//public enum State
//{
//    Alive = 0,
//    Dead = 1,
//    Attack = 2,
//    Move = 3,
//    Idle = 4
//}
