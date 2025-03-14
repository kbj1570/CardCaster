using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using System.Collections;



public class DungeonEnemy : MonoBehaviour
{
    Enemy enemy;
    int currentNodeNum;
    EEnemyDirection enemyDirection;
    EEnemyState enemyState;

    public Enemy GetEnemy()
    {return enemy;}

    public int GetCurrentNodeNum()
    {return currentNodeNum;}

    public EEnemyDirection GetEnemyDirection()
    {return enemyDirection;}

    public EEnemyState GetEnemyState()
    {return enemyState;}

    public void SetEnemyDirection(EEnemyDirection value)
    {this.enemyDirection = value;}
}

public enum EEnemyDirection
{None, North, East, South, West}

public enum EEnemyState
{None, Idle, Chase}
