// using System.Collections.Generic;
// public enum EffectTiming
// {
//     OnAttackDeclare,
//     OnAttack,
//     OnDamaged,
//     OnDeath,

//     OnTurnStart,
//     OnTurnEnd,
//     OnSpellCast
// }
// public abstract class CardEffect
// {
// }

// // public abstract class TriggerEffect : CardEffect
// // {
// //     public EffectTiming timing;

// //     public abstract IEnumerator Execute(EffectContext context);
// // }
// public abstract class ContinuousEffect : CardEffect
// {
//     public abstract void Apply(EffectContext context);
// }

// public class EffectContext
// {
//     // 이벤트 정보
//     public EffectTiming timing;

//     // 전투 정보
//     public Servant attacker;
//     public Servant defender;

//     public Player activePlayer;
//     public Player opponentPlayer;

//     // 효과 주체
//     public IEffectSource source;

//     // 상태
//     public int damage;

//     // 흐름 제어
//     public bool cancel;
//     public bool skipDamageStep;

//     // 시스템 접근
//     public BattleManager battleManager;

//     // 확장 데이터
//     public Dictionary<string, object> customData = new();

//     public void Reset()
//     {
//         timing = default;

//         attacker = null;
//         defender = null;
//         source = null;

//         damage = 0;

//         cancel = false;
//         skipDamageStep = false;

//         customData.Clear();
//     }
// }
// /*
//     효과 체크
//     소환시
//     모든 객체한테 물어봄

//     조건체크
// */

// public abstract class EffectActionSO : ScriptableObject
// {
//     public abstract IEnumerator Execute(EffectContext context);
// }

// public interface IEffectSource
// {
//     Player Owner { get; }
//     List<CardEffect> Effects { get; }
// }
