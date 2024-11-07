using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    public CardData cardData;
    public List<EServentCondition> conditions;
    
    public bool filled;
    public bool isDragable;
    private bool attacked;
    public TMP_Text forceTMP;

    public GameObject conditionPanel;
    public GameObject conditionPanelButton;

    public GameObject monsterPrefab;
    public GameObject summonEffectPrefab;
    public GameObject summonEffectObject;
    public GameObject monsterEntity;

    public Color forceColorFire;
    public Color forceColorWater;
    public Color forceColorEarth;
    public Color forceColorWind;
    public Color forceColorDarkness;
    public Color forceColorLight;


    
    public int currentForce;
    
    public int fieldNum;

    void Start()
    {}

    public void SetForce(int value){ currentForce = value;}
    public int GetForce(){return currentForce;}

    public int GetFieldNum(){return fieldNum;}

    public void GainForce(int value){currentForce += value;}
    public EServentAttribute GetServentAttribute(){return cardData.GetAttribute();}
    public void LoseForce(int value)
    {
        if(!filled)
            return;
        currentForce -= value;
    }
    public void Kill(){currentForce = 0;}

    public void UpdateHealth()
    {
        // if(fieldNum == 0 && currentForce <= 0){BattleManager.Inst.Notification("패배");}
        // else if(fieldNum == 7 && currentForce <= 0){BattleManager.Inst.Notification("승리");}
        // else if(currentForce <= 0)
        // {
        //     Destroy(monsterEntity.gameObject);
        //     healthTMP.text = "";
        //     currentForce = 0;
        //     filled = false;
        //     attacked = false;
        //     return;
        // }
        // healthTMP.text = currentForce.ToString();
    }

    public void Summon(CardData cardData)
    {
        monsterEntity = Instantiate(monsterPrefab, this.transform.position, Utils.QI);
        this.cardData = cardData;
        currentForce = cardData.GetForce();
        forceTMP.text = currentForce.ToString();
        filled = true;
        attacked = false;
    }

    public void UpdateCondition()
    {
        for(int i = 0; i < conditionPanel.transform.childCount; ++i)
        {Destroy(conditionPanel.transform.GetChild(i).gameObject);}

        foreach(EServentCondition condition in conditions)
        {
            GameObject gameObject = Instantiate(BattleManagerAlt.Inst.ReturnConditionMark(condition),
            conditionPanel.transform.position, Utils.QI);
            gameObject.transform.SetParent(conditionPanel.transform);
        }

        if(conditions.Count > 3)
        {conditionPanelButton.SetActive(true);}
        else
        {conditionPanelButton.SetActive(false);}

    }

    public void ResetCondition()
    {conditions.Clear();}
    
    public void AddCondition(EServentCondition value)
    {conditions.Add(value);}

    public void RemoveCondition(EServentCondition value)
    {conditions.Remove(value);}

    public bool GetFilled()
    {return filled;}

    public bool GetAttacked()
    {return attacked;}

    public void SetAttacked(bool value)
    {this.attacked = value;}
    public CardData GetCardData()
    {return cardData;}

    void OnMouseDown()
    {BattleManagerAlt.Inst.SelectTarget(this.gameObject);}

    void OnMouseEnter()
    {BattleManagerAlt.Inst.SetMouseOnField(this);}

    void OnMouseExit()
    {BattleManagerAlt.Inst.ResetMouseOnField();}
}