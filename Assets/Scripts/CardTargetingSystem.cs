using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TargetType {
    None,           // 타겟팅이 필요없는 카드
    SingleAlly,     // 아군 하나
    SingleEnemy,    // 적 하나
    AllAllies,      // 아군 전체
    AllEnemies,     // 적 전체
    AnyField,       // 아무 필드
    EmptyField,     // 비어있는 필드
    FilledField     // 유닛이 있는 필드
}

public enum TargetCondition {
    None,
    HasAttribute,   // 특정 속성을 가진 대상
    HasCondition,   // 특정 상태를 가진 대상
    HasMinForce,    // 최소 포스 이상
    HasMaxForce,    // 최대 포스 이하
}

[System.Serializable]
public class CardTargetingRule {
    public int cardId;
    public TargetType targetType;
    public List<TargetCondition> conditions;
    public List<object> conditionValues; // 조건에 필요한 값들 (속성, 최소값 등)
}

// CardTargetingSystem.cs
public class CardTargetingSystem : MonoBehaviour {
    private Dictionary<int, CardTargetingRule> targetingRules = new Dictionary<int, CardTargetingRule>();
    public Color validTargetColor = Color.blue;
    public Color invalidTargetColor = Color.red;

    public void Initialize() {
        LoadTargetingRules();
    }

    private void LoadTargetingRules() {
        // JSON이나 ScriptableObject 등에서 규칙을 로드
        // 예시 코드:
        TextAsset rulesJson = Resources.Load<TextAsset>("CardTargetingRules");
        List<CardTargetingRule> rules = JsonUtility.FromJson<List<CardTargetingRule>>(rulesJson.text);
        foreach (var rule in rules) {
            targetingRules[rule.cardId] = rule;
        }
    }

    public bool CanTargetField(int cardId, Field targetField) {
        if (!targetingRules.TryGetValue(cardId, out CardTargetingRule rule)) {
            return false;
        }

        // 기본 타겟 타입 체크
        if (!IsValidTargetType(rule.targetType, targetField)) {
            return false;
        }

        // 추가 조건 체크
        for (int i = 0; i < rule.conditions.Count; i++) {
            if (!IsValidCondition(rule.conditions[i], rule.conditionValues[i], targetField)) {
                return false;
            }
        }

        return true;
    }

    private bool IsValidTargetType(TargetType targetType, Field field) {
        switch (targetType) {
            case TargetType.None:
                return false;
            case TargetType.SingleAlly:
                return field.GetFieldNum() == 0 && field.GetFilled();
            case TargetType.SingleEnemy:
                return field.GetFieldNum() != 0 && field.GetFilled();
            case TargetType.EmptyField:
                return !field.GetFilled();
            case TargetType.FilledField:
                return field.GetFilled();
            // 다른 케이스들 추가
            default:
                return false;
        }
    }

    private bool IsValidCondition(TargetCondition condition, object value, Field field) {
        switch (condition) {
            case TargetCondition.None:
                return true;
            case TargetCondition.HasAttribute:
                return field.GetServentAttribute() == (EServentAttribute)value;
            case TargetCondition.HasMinForce:
                return field.GetForce() >= (int)value;
            case TargetCondition.HasCondition:
                return field.HasCondition((EServentCondition)value);
            // 다른 조건들 추가
            default:
                return false;
        }
    }

    public void UpdateLineRendererColor(LineRenderer lineRenderer, int cardId, Field targetField) {
        bool isValidTarget = CanTargetField(cardId, targetField);
        lineRenderer.startColor = lineRenderer.endColor = isValidTarget ? validTargetColor : invalidTargetColor;
    }
}

