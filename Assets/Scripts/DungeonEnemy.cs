using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;


public class DungeonEnemy : MonoBehaviour
{
    Enemy enemy;
    int currentNodeNum;
    EEnemyDirection enemyDirection;
    EEnemyState enemyState;

    public Enemy GetEnemy()
    {return enemy;}
    
    public void SetEnemy(Enemy enemy)
    {this.enemy = enemy;}

    public int GetCurrentNodeNum()
    {return currentNodeNum;}

    public void SetCurrentNodeNum(int nodeNum)
    {currentNodeNum = nodeNum;}

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
