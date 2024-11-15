using System.Collections;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine.UI;

// CardTargetingRule.cs
public enum EffectType {
    None,
    SingleTarget,    // 단일 대상 효과
    GlobalEffect,    // 전체 효과
    ConditionBased,  // 조건부 효과
    AttributeBased   // 속성 기반 효과
}

public enum EffectScope {
    None,
    Self,           // 자신
    AllAllies,      // 아군 전체
    AllEnemies,     // 적 전체
    Selected,       // 선택된 대상
    Conditional     // 조건부 대상
}

[System.Serializable]
public class CardEffect {
    public EffectType type;
    public EffectScope scope;
    public string effectFunction;  // 실행할 효과 함수 이름
    public List<object> parameters; // 효과에 필요한 매개변수들
}

[System.Serializable]
public class ExtendedCardRule {
    public int cardId;
    public TargetType targetType;
    public List<TargetCondition> conditions;
    public List<object> conditionValues;
    public List<CardEffect> effects;
}


// CardEffectSystem.cs
public class CardEffectSystem : MonoBehaviour {
    private Dictionary<int, ExtendedCardRule> cardRules = new Dictionary<int, ExtendedCardRule>();
    
    public void Initialize() {
        LoadCardRules();
    }

    private void LoadCardRules() {
        // JSON 파일에서 규칙 로드
        TextAsset rulesJson = Resources.Load<TextAsset>("CardRules");
        List<ExtendedCardRule> rules = JsonUtility.FromJson<List<ExtendedCardRule>>(rulesJson.text);
        foreach (var rule in rules) {
            cardRules[rule.cardId] = rule;
        }
    }

    public void ExecuteCardEffect(int cardId, BattleManagerAlt battleManager) {
        if (!cardRules.TryGetValue(cardId, out ExtendedCardRule rule)) {
            Debug.LogError($"Card rule not found for ID: {cardId}");
            return;
        }

        foreach (var effect in rule.effects) {
            ExecuteEffect(effect, battleManager);
        }
    }

    private void ExecuteEffect(CardEffect effect, BattleManagerAlt battleManager) {
        switch (effect.type) {
            case EffectType.AttributeBased:
                if (effect.effectFunction == "ElementalBoost") {
                    ExecuteElementalBoost(battleManager);
                }
                break;
            // 다른 효과 타입들에 대한 처리
        }
    }

    private void ExecuteElementalBoost(BattleManagerAlt battleManager) {
        // 속성 종류 수 계산
        HashSet<EServentAttribute> attributes = new HashSet<EServentAttribute>();
        
        // 필드 1, 2, 3의 속성 수집
        Field[] allyFields = new[] {
            battleManager.field_1.GetComponent<Field>(),
            battleManager.field_2.GetComponent<Field>(),
            battleManager.field_3.GetComponent<Field>()
        };

        foreach (var field in allyFields) {
            if (field.GetFilled()) {
                attributes.Add(field.GetServentAttribute());
            }
        }

        int boost = attributes.Count;

        // 모든 아군 소환수에게 포스 부여
        foreach (var field in allyFields) {
            if (field.GetFilled()) {
                field.GainForce(boost);
            }
        }
    }
}