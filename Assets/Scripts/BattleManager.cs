// using UnityEngine;
// using System;
// using System.Collections;
// using Random = UnityEngine.Random;
// using System.Collections.Generic;

// public class  BattleManager : MonoBehaviour
// {
//     public static BattleManager Inst{get; private set;}
//     void Awake() => Inst = this;
//     public NotificationPanel notificationPanel;
//     WaitForSeconds delay07 = new WaitForSeconds(0.7f);

//     private int spellCost;
//     private int monsterCost;
//     private List<CardData> deck;
//     private List<CardData> trash;
//     void Start()
//     {StartGame();}

//     void Update()
//     {
//         #if UNITY_EDITOR
//             InputCheatKey();
//         #endif
//     }

//     public void ResetMonsterCost(){monsterCost = 0;}
//     public void ResetSpellCost(){spellCost = 0;}

//     public void UpdateAllStatus()
//     {
//         FieldManager.Inst.UpdateAllFieldStatus();
//         CardManager.Inst.UpdateCardStatus(monsterCost, spellCost);
//     }

//     public void Battle(Field start, Field target)
//     {

//         if(!start.filled || !target.filled){return;}
//         int startHealth = start.GetForce();
//         int targetHealth = target.GetForce();

//         if((target.GetFieldNum() == 7) ||
//         (target.GetFieldNum() == 0))
//         {
//             target.SetForce(targetHealth - startHealth);
//             start.SetAttacked(true);
//             UpdateAllStatus();
//             return;
//         }
//         start.SetAttacked(true);
//         start.SetForce(startHealth - targetHealth);
//         target.SetForce(targetHealth - startHealth);
//         UpdateAllStatus();
//     }

//     public IEnumerator EnemyAutoAttack()
//     {
//         foreach(Field field in FieldManager.Inst.GetOpponentFields())
//         {
//             if(field.filled)
//             {
//                 Field facingField = FieldManager.Inst.ReturnFacingField(field.GetFieldNum());

//                 if(!facingField.filled){facingField = FieldManager.Inst.ReturnField(0);}
                
//                 Battle(field, facingField);
//                  yield return delay07;
//             }
//         }
//     }

//     public bool Summon(CardData cardData)
//     {

//         if(FieldManager.Inst.mouseOnField == null)
//         {
//             UpdateAllStatus();
//             return false;
//         }
            
//         if(!FieldManager.Inst.mouseOnField.filled &&
//         CardManager.Inst.selectCard != null)
//         {
//             if(cardData.GetCardCost() > 0){ResetMonsterCost();}

//             FieldManager.Inst.SummonMonster(cardData);
//             UpdateAllStatus();
//             return true;
//         }
//         UpdateAllStatus();
//         return false;
//     }

//     public bool ActivateSpell(CardData cardData)
//     {
//         if(FieldManager.Inst.mouseOnField != null && CardManager.Inst.selectCard != null)
//         {
//             if(cardData.GetCardCost() > 0){ResetSpellCost();}
//             FieldManager.Inst.ActivateSpell(cardData);
//             UpdateAllStatus();
//             return true;
//         }
//         UpdateAllStatus();
//         return false;
//     }

//     public bool Sacrifice(CardData cardData)
//     {
//         if(FieldManager.Inst.isOnHole && CardManager.Inst.selectCard != null)
//         {
//             if(cardData.GetCardType() == ECardType.Servent)
//             {
//                 monsterCost++;
//             }
//             else if(cardData.GetCardType() == ECardType.Spell)
//             {
//                 spellCost++;
//             }
//             Destroy(CardManager.Inst.selectCard.gameObject);
//             UpdateAllStatus();
//             return true;
//         }
//         UpdateAllStatus();
//         return false;
//     }

//     void InputCheatKey()
//     {
//         if(Input.GetKeyDown(KeyCode.A))
//         {TurnManager.OnAddCard?.Invoke(true);}
//         if(Input.GetKeyDown(KeyCode.S))
//         {TurnManager.OnAddCard?.Invoke(false);}
//         if(Input.GetKeyDown(KeyCode.D))
//         {TurnManager.Inst.EndTurn();}
            
//     }

//     public void StartGame()
//     {StartCoroutine(TurnManager.Inst.StartGameCo());}

//     public void Notification(string message)
//     {notificationPanel.Show(message);}

// }