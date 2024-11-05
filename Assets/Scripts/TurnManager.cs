// using UnityEngine;
// using System;
// using System.Collections;
// using Random = UnityEngine.Random;

// public class TurnManager : MonoBehaviour
// {
//     public static TurnManager Inst{get; private set;}
//     void Awake() => Inst = this;

//     [Header("Develop")]
//     [SerializeField] [Tooltip("시작 턴 모드를 정합니다")] ETurnMode eTurnMode;
//     [SerializeField] [Tooltip("시작 카드 갯수를 정합니다")] int startCardCount;
//     [SerializeField] [Tooltip("카드 배분이 매우 빨라집니다")] bool fastMode;
//     [Header("Properties")]
//     public bool myTurn;
//     public bool isLoading;

//     enum ETurnMode{Random, My, Other}
//     WaitForSeconds delay05 = new WaitForSeconds(0.5f);
//     WaitForSeconds delay07 = new WaitForSeconds(0.7f);
//     public static Action<bool> OnAddCard;


//     void GameSetup()
//     {
//         if(fastMode)
//             delay05 = new WaitForSeconds(0.05f);
//         switch(eTurnMode)
//         {
//             case ETurnMode.Random:
//                 myTurn = Random.Range(0, 2) == 0;
//                 break;
//             case ETurnMode.My:
//                 myTurn = true;
//                 break;
//             case ETurnMode.Other:
//                 myTurn = false;
//                 break;
//         }
//     }

//     public IEnumerator StartGameCo()
//     {
//         GameSetup();
//         isLoading = true;

//         for(int i = 0; i < startCardCount; ++i)
//         {
//             yield return delay05;
//             OnAddCard?.Invoke(false);
//             yield return delay05;
//             OnAddCard?.Invoke(true);
//         }
//         StartCoroutine(StartTurnCo());
//     }

//     IEnumerator StartTurnCo()
//     {
//         isLoading = true;
//         BattleManager.Inst.ResetMonsterCost();
//         BattleManager.Inst.ResetSpellCost();
//         BattleManager.Inst.UpdateAllStatus();
//         FieldManager.Inst.ResetAttacked();

//         if(myTurn)
//             BattleManager.Inst.Notification("나의 턴");
//         else if(!myTurn)
//             BattleManager.Inst.Notification("상대 턴");
//         yield return delay07;
        

//         if(CardManager.Inst.GetHandsCount() < 5)
//         {
//             int p = 5 - CardManager.Inst.GetHandsCount();
//             for(int i = 0; i < p; ++i)
//             {OnAddCard?.Invoke(myTurn);}
//         }
//         else
//         {OnAddCard?.Invoke(myTurn);}
//         yield return delay07;

//         if(!myTurn){StartCoroutine(BattleManager.Inst.EnemyAutoAttack());}

//         isLoading = false;
//     }

//     public void EndTurn()
//     {
//         myTurn = !myTurn;
//         StartCoroutine(StartTurnCo());
//     }

//     public void DrawCard()
//     {
//         OnAddCard?.Invoke(myTurn);
//     }
// }