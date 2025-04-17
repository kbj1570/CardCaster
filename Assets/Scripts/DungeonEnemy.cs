using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;


public class DungeonEnemy : MonoBehaviour
{
    bool moveLocked;
    Enemy enemy;
    protected int currentNodeNum;
    public EEnemyDirection enemyDirection;
    public EEnemyState enemyState;
    public Renderer renderer;
    public bool visible;

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

    public void SetVisible(bool value)
    {
        this.gameObject.SetActive(value);
        visible = value;
    }

    public bool GetVisible()
    {return visible;}

    public void SetMoveLock(bool value)
    {moveLocked = value;}

    public bool GetMoveLock()
    {return moveLocked;}

    public IEnumerator Kill()
    {

        float f = 1;
        while (f >= 0)
        {
            f -= 0.1f;
            Color ColorAlhpa = renderer.material.color;
            ColorAlhpa.a = f;
            renderer.material.color = ColorAlhpa;
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(this.gameObject);
    }

    public IEnumerator FadeIn()
    {
        float f = 1;
        while (f >= 0)
        {
            f -= 0.1f;
            Color ColorAlhpa = renderer.material.color;
            ColorAlhpa.a = f;
            renderer.material.color = ColorAlhpa;
            yield return new WaitForSeconds(0.02f);
        }
        this.gameObject.SetActive(false);
        visible = false;
    }

    public IEnumerator FadeOut()
    {
        this.gameObject.SetActive(true);
        float f = 0;
        while (f <= 1)
        {
            f += 0.1f;
            Color ColorAlhpa = renderer.material.color;
            ColorAlhpa.a = f;
            renderer.material.color = ColorAlhpa;
            yield return new WaitForSeconds(0.02f);
        }
        visible = true;
    }
}

public enum EEnemyDirection
{None, North, East, South, West}

public enum EEnemyState
{None, Idle, Chase}
