using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.ProBuilder;

public class Field : MonoBehaviour
{
    public FieldArea fieldArea;
    public CardData cardData;
    public List<EServentCondition> conditions;
    public EServentAttribute serventAttribute;
    
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

    public Transform lowLinePoint;
    public Transform middleLinePoint;
    public Transform highLinePoint;

    public Color forceColorFire;
    public Color forceColorWater;
    public Color forceColorEarth;
    public Color forceColorWind;
    public Color forceColorDarkness;
    public Color forceColorLightness;
    
    public int currentForce;
    public int fieldNum;

    bool penetrate;

    bool damageBlock;
    int damageDecrease;
    int damageIncrease;

    int additionalDamage;

    private GameObject summonedServent;
    

    public void SetForce(int value){ currentForce = value;}
    public int GetForce(){return currentForce;}

    public int GetFieldNum(){return fieldNum;}

    public void GainForce(int value){currentForce += value;}
    public EServentAttribute GetServentAttribute(){return serventAttribute;}
    public void LoseForce(int value)
    {
        if(!filled)
            return;

        if(damageBlock)
            return;

        currentForce -= value;
    }
    public void Kill(){currentForce = 0;}

    public void UpdateHealth()
    {
        forceTMP.text = currentForce.ToString();

        if(!filled)
        {return;}

        if(currentForce <= 0)
        {
            forceTMP.gameObject.SetActive(false);
            filled = false;
            attacked = false;

            BattleManagerAlt.Inst.AddTrash(cardData);
            Destroy(summonedServent);
        }
    }

    public void Summon(CardData cardData, GameObject gameObject)
    {
        this.cardData = cardData;
        currentForce = cardData.GetForce();
        forceTMP.gameObject.SetActive(true);
        forceTMP.text = currentForce.ToString();
        filled = true;
        attacked = false;
        penetrate = cardData.GetPenetrate();
        serventAttribute = cardData.serventAttribute;
        summonedServent = gameObject;
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

    public bool GetPenetrate()
    {return penetrate;}

    public int GetAdditionalDamage()
    {return additionalDamage;}


    public Transform GetLinePoint()
    {
        if(!filled)
        return lowLinePoint;

        switch(cardData.GetSize())
        {
            case EServentSize.Small:
            return lowLinePoint;

            case EServentSize.Middle:
            return middleLinePoint;

            case EServentSize.Big:
            return highLinePoint;

            default:
            return null;
        }
    }
    void OnMouseDown()
    {BattleManagerAlt.Inst.SelectTarget(this.gameObject);}



}