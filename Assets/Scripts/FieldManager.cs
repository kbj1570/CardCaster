using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FieldManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static FieldManager Inst {get; private set;}

    [SerializeField] Hole hole;
    [SerializeField] Field player;
    [SerializeField] Field opponent;
    [SerializeField] Field field1;
    [SerializeField] Field field2;
    [SerializeField] Field field3;
    [SerializeField] Field field4;
    [SerializeField] Field field5;
    [SerializeField] Field field6;
    public Field mouseOnField;
    public Field startPointField;
    [SerializeField] List<Field> AllFields;
    [SerializeField] List<Field> myFields;
    [SerializeField] List<Field> opponentFields;

    List<CardData> selectedCards;
    List<Field> targetFields;
    EFieldSpell fieldSpell;
    public int heavyRainStack;
    public int plusHealth;
    public int minusHealth;
    public int attackLimit;
    public bool isOnHole;
    void Awake() => Inst = this;

    void Update()
    {
        DetectFieldArea();
        DetectIsOnHole();    
    }

    public List<Field> GetOpponentFields()
    {return opponentFields;}
    void DetectIsOnHole()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        foreach(RaycastHit2D ray in hits)
        {
            isOnHole = ray.collider.gameObject.layer == 13;
        }
    }
    
    void DetectFieldArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);

        foreach(RaycastHit2D ray in hits)
        {
            switch(ray.collider.gameObject.layer)
            {
                case 7:
                mouseOnField = field1;
                break;

                case 8:
                mouseOnField = field2;
                break;

                case 9:
                mouseOnField = field3;
                break;

                case 10:
                mouseOnField = field4;
                break;

                case 11:
                mouseOnField = field5;
                break;

                case 12:
                mouseOnField = field6;
                break;

                case 14:
                mouseOnField = player;
                break;

                case 15:
                mouseOnField = opponent;
                break;

                default:
                mouseOnField = null;
                break;
            }
        }
    }

    public void TurnStartEffect()
    {

    }

    public void TurnEndEffect()
    {
        switch(fieldSpell)
        {
            case EFieldSpell.HeavyRain:
            foreach(Field field in targetFields)
            {
                field.LoseForce(heavyRainStack);
            }
            heavyRainStack++;
            break;
        }
    }

    public void SummonMonster(CardData cardData)
    {
        // if(cardData.GetCardEffect() != null)
        // {
        //     ActivateCardEffect(cardData.GetCardEffect());
        // }
        // mouseOnField.Summon(cardData);
        
    }

    #region 카드 효과 시작
    public void ActivateSpell(CardData cardData)
    {
        // ActivateCardEffect(cardData.GetCardEffect());

        switch(cardData.GetCardNum())
        {
            case 0: // 엘리멘탈 부스트

            List<EServentAttribute> attrubutes = new();
            if(field1.GetFilled())
            {
                if(!attrubutes.Contains(field1.GetServentAttribute()))
                {attrubutes.Add(field1.GetServentAttribute());}
            }

            if(field2.GetFilled())
            {
                if(!attrubutes.Contains(field2.GetServentAttribute()))
                {attrubutes.Add(field2.GetServentAttribute());}
            }

            if(field3.GetFilled())
            {
                if(!attrubutes.Contains(field3.GetServentAttribute()))
                {attrubutes.Add(field3.GetServentAttribute());}
            }

            int value = attrubutes.Count;

            field1.GainForce(value);
            field2.GainForce(value);
            field3.GainForce(value);
            break;

            case 1: // 잔혹한 진실
            break;

            case 2: // 악을 멸하는 등불

            if(field1.filled)
            {
                if(field1.GetServentAttribute().Equals(EServentAttribute.Dark))
                {field1.Kill();}
            }

            if(field2.filled)
            {
                if(field2.GetServentAttribute().Equals(EServentAttribute.Dark))
                {field2.Kill();}
            }

            if(field3.filled)
            {
                if(field3.GetServentAttribute().Equals(EServentAttribute.Dark))
                {field3.Kill();}
            }

            if(field4.filled)
            {
                if(field4.GetServentAttribute().Equals(EServentAttribute.Dark))
                {field4.Kill();}
            }

            if(field5.filled)
            {
                if(field5.GetServentAttribute().Equals(EServentAttribute.Dark))
                {field5.Kill();}
            }

            if(field6.filled)
            {
                if(field6.GetServentAttribute().Equals(EServentAttribute.Dark))
                {field6.Kill();}
            }
            
            break;
        }
    }
    #endregion 카드 효과 끝

    // void ActivateCardEffect(List<ActionData> actionList)
    // {
        
    //     foreach(ActionData actionData in actionList)
    //     {
    //         ResetActionData();
            
    //         switch(actionData.GetTarget())
    //         {
    //             case ETarget.All:
    //             targetFields.Add(field1);
    //             targetFields.Add(field2);
    //             targetFields.Add(field3);
    //             targetFields.Add(field4);
    //             targetFields.Add(field5);
    //             targetFields.Add(field6);
    //             break;

    //             case ETarget.FacingField:
    //             targetFields.Add(ReturnFacingField(mouseOnField.GetFieldNum()));
    //             break;

    //             case ETarget.Random:
    //             targetFields.Add(ReturnRandomField());
    //             break;
                
    //             case ETarget.ThisField:
    //             targetFields.Add(mouseOnField);
    //             break;

    //             case ETarget.Hand:
    //             break;

    //             default:
    //             break;
    //         }

    //         switch(actionData.GetAction())
    //         {
    //             case EAction.Damage:
    //             foreach(Field field in targetFields)
    //             {
    //                 field.Damage(actionData.GetParameter());
    //             }
    //             break;

    //             case EAction.Heal:
    //             foreach(Field field in targetFields)
    //             {
    //                 field.Heal(actionData.GetParameter());
    //             }
    //             break;

    //             case EAction.Kill:
    //             foreach(Field field in targetFields)
    //             {
    //                 field.Kill();
    //             }
    //             break;

    //             case EAction.Draw:
    //             for(int i = 0; i < actionData.GetParameter(); ++i)
    //             {
    //                 TurnManager.OnAddCard?.Invoke(true);
    //             }
    //             break;

    //             case EAction.Trash:
    //             break;

    //             case EAction.ChangeField:
    //             this.fieldSpell = actionData.GetFieldSpell();
    //             break;

    //             case EAction.None:
    //             break;

    //             case EAction.SelectCard:

    //             int parameter = actionData.GetParameter();
                
    //             break;

    //             default:
    //             break;
    //         }

    //     }
    //}

    void ResetActionData()
    {
        targetFields = new List<Field>();
    }

    public void UpdateAllFieldStatus()
    {
        player.UpdateHealth();
        opponent.UpdateHealth();
        foreach(Field field in AllFields)
        {
            if(field.filled)
                field.UpdateHealth();
        }
    }

    public void ResetAttacked()
    {
        // player.attacked = false;
        // opponent.attacked = false;
        // foreach(Field field in AllFields)
        // {
        //     if(field.isFilled)
        //         field.attacked = false;
        // }
    }

    public Field ReturnField(int value)
    {
        switch(value)
        {
            case 0:
            return player;

            case 1:
            return field1;

            case 2:
            return field2;

            case 3:
            return field3;

            case 4:
            return field4;

            case 5:
            return field5;

            case 6:
            return field6;

            case 7:
            return opponent;

            default:
            return field1;
        }

    }
    Field ReturnRandomField()
    {
        int rand = Random.Range(0, 8);
        switch(rand)
        {
            case 0:
            return player;

            case 1:
            return field1;

            case 2:
            return field2;

            case 3:
            return field3;

            case 4:
            return field4;

            case 5:
            return field5;

            case 6:
            return field6;

            case 7:
            return opponent;

            default:
            return field1;
        }

    }

    public Field ReturnFacingField(int value)
    {
        switch(value)
        {
            case 1:
            return field6;
            case 2:
            return field5;
            case 3:
            return field4;
            case 4:
            return field3;
            case 5:
            return field2;
            case 6:
            return field1;
            default:
            return null;
        }
    }

    #region MyField
    public void FieldMouseOver(Field field)
    {mouseOnField = field;}

    public void FieldMouseExit(Field field)
    {mouseOnField = null;}

    public void FieldMouseUp(Field field)
    {
        if(startPointField != null && mouseOnField != null)
        {
            if(startPointField.filled && mouseOnField.filled);
                // BattleManager.Inst.Battle(startPointField, mouseOnField);
        }

        
        startPointField = null;
    }
    public void FieldMouseDown(Field field)
    {
        startPointField = field;
    }
    #endregion

    public void HoleMouseOver()
    {
    }
    public void HoleMouseDown()
    {
    }
    public void HoleMouseUp()
    {

    }
    public void HoleMouseExit()
    {
    }

    public void AddSelectedCard(CardData value)
    {
        selectedCards.Add(value);
    }


}
public enum EFieldSpell{None, HardFog, IceAge, Pressure, StrongWind, MadParty, WhimsOfFate, AcidRain, HeavyRain}
public enum ETarget
{
    None, ThisField, FacingField, All, Random, Hand, Trash
}
public enum EMoveFrom
{
    None, Hand, Trash
}
