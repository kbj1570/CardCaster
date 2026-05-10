using System.Collections.Generic;
using UnityEngine;
public enum EffectTiming
{
    OnAttackDeclare,
    OnAttack,
    OnDamaged,
    OnDeath,
    OnTurnStart,
    OnTurnEnd,
    OnSpellCast
}
public abstract class CardEffect
{
}

public abstract class TriggerEffect : CardEffect
{
    public EffectTiming timing;

}
public abstract class ContinuousEffect : CardEffect
{
    public abstract void Apply(EffectContext context);
}

public class EffectContext
{
    public EffectTiming timing;
    public Servant attacker;
    public Servant defender;
    public Player activePlayer;
    public Player opponentPlayer;
    public IEffectSource source;
    public int damage;
    public bool cancel;
    public bool skipDamageStep;
    public BattleManager battleManager;
    public Dictionary<string, object> customData = new();

    public void Reset()
    {
        timing = default;

        attacker = null;
        defender = null;
        source = null;

        damage = 0;

        cancel = false;
        skipDamageStep = false;

        customData.Clear();
    }
}

public abstract class EffectActionSO : ScriptableObject
{
}

public interface IEffectSource
{
    Player Owner { get; }
    List<CardEffect> Effects { get; }
}
