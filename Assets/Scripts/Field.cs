using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Field : MonoBehaviour
{
    public FieldArea fieldArea;
    public CardData cardData;
    public List<EServentCondition> conditions;
    public EServentAttribute serventAttribute;
    
    public bool filled;
    public bool locked;
    public bool hasAbility;
    public bool canUseAbility;
    public bool isDragable;
    private bool attacked;
    public TMP_Text forceTMP;

    public GameObject conditionPanel;
    public GameObject conditionPanelButton;

    public GameObject floatingTextPrefab;

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
    bool suicide;

    bool voidWalker;

    bool damageBlock;
    int damageDecrease;
    int damageIncrease;

    int additionalDamage;

    private GameObject summonedServent;
    

    public void SetForce(int value)
    {
        if(voidWalker)
        return;

        currentForce = value;
    }
    public int GetForce(){return currentForce;}

    public int GetFieldNum(){return fieldNum;}

    public void GainForce(int value)
    {
        if(voidWalker)
        return;

        GameObject damageText = Instantiate(floatingTextPrefab, fieldArea.transform);
        damageText.GetComponent<FloatingDamageText>().SetFont(100);
        damageText.GetComponent<FloatingDamageText>().SetColor(Color.blue);
        damageText.GetComponent<FloatingDamageText>().SetDamageText(value);

        currentForce += value;
    }
    public EServentAttribute GetServentAttribute(){return serventAttribute;}
    public void LoseForce(int value)
    {
        if(voidWalker)
        return;

        if(!filled)
        return;

        if(damageBlock)
        return;

        currentForce -= value;
    }

    public void TakeDamage(int value)
    {
        if(!filled)
            return;

        if(damageBlock)
            return;


        // 피해 숫자 표시
        GameObject damageText = Instantiate(floatingTextPrefab, fieldArea.transform);
        damageText.GetComponent<FloatingDamageText>().SetDamageText(value);
        damageText.GetComponent<FloatingDamageText>().SetFont(150);

        currentForce -= value;
    }

    public void Kill()
    {
        if(voidWalker)
        {return;}

        forceTMP.gameObject.SetActive(false);
        filled = false;
        attacked = false;

        if(summonedServent.GetComponent<Servent>().GetServentType() == EServentType.Player)
        BattleManager.Inst.AddTrash(cardData);

        
        summonedServent.GetComponent<Servent>().Dead();
        currentForce = 0;
    }

    public void SetHealth(int value)
    {
        currentForce = value;
    }
    

    public void UpdateHealth()
    {

        if(!filled)
        {return;}
        forceTMP.text = currentForce.ToString();

        if(currentForce <= 0)
        {
            forceTMP.gameObject.SetActive(false);
            filled = false;
            attacked = false;

            if(summonedServent.GetComponent<Servent>().GetServentType() == EServentType.Player)
            BattleManager.Inst.AddTrash(cardData);

            
            summonedServent.GetComponent<Servent>().Dead();
            currentForce = 0;
        }
    }

    public void Summon(CardData cardData, GameObject gameObject)
    {

        filled = true;
        this.cardData = cardData;
        currentForce = cardData.GetForce();
        forceTMP.gameObject.SetActive(true);
        forceTMP.text = currentForce.ToString();
        
        attacked = false;
        penetrate = cardData.GetPenetrate();
        voidWalker = cardData.GetVoidWalker();
        serventAttribute = cardData.GetAttribute();
        summonedServent = gameObject;
        hasAbility = cardData.GetHasAbility();
        canUseAbility = cardData.GetCanUseAbility();

        gameObject.GetComponent<Servent>().SetServentType(cardData.GetServentType());


        // EffectManager.Inst.SpawnSummonEffect(cardData.serventAttribute, transform.position);
        locked = false;
    }

    public void UpdateCondition()
    {
        for(int i = 0; i < conditionPanel.transform.childCount; ++i)
        {Destroy(conditionPanel.transform.GetChild(i).gameObject);}

        foreach(EServentCondition condition in conditions)
        {
            GameObject gameObject = Instantiate(BattleManager.Inst.ReturnConditionMark(condition),
            conditionPanel.transform.position, Utils.QI);
            gameObject.transform.SetParent(conditionPanel.transform);
        }

        if(conditions.Count > 3)
        {conditionPanelButton.SetActive(true);}
        else
        {conditionPanelButton.SetActive(false);}
    }

    public void ActivateTurnEnd()
    {

        if(voidWalker)
        return;

        if(suicide)
        {currentForce = 0;}


    }

    public void ResetCondition()
    {

        if(voidWalker)
        return;


        conditions.Clear();
    }
    
    public void AddCondition(EServentCondition value)
    {
        if(voidWalker)
        return;

        conditions.Add(value);
    }

    public void RemoveCondition(EServentCondition value)
    {


        if(voidWalker)
        return;

        conditions.Remove(value);
    }

    public bool GetFilled()
    {return filled;}

    public bool GetAttacked()
    {return attacked;}

    public void SetAttacked(bool value)
    {this.attacked = value;}
    public CardData GetCardData()
    {return cardData;}

    public void SetSuicide(bool value)
    {
        if(voidWalker)
        return;

        suicide = value;
    }

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
    {BattleManager.Inst.SelectTarget(this.gameObject);}



}