using UnityEngine;

public class CardHand : MonoBehaviour
{
    public GameObject[] cards; // 카드 오브젝트들 (1~5장)
    public float radius = 2f; // 부채꼴의 반지름 (패의 너비 조절)
    public float maxAngle = 30f; // 부채꼴의 최대 각도 (각도 조절 가능)

    void Start()
    {
        ArrangeCards();
    }

    void ArrangeCards()
    {
        int cardCount = cards.Length;
        if (cardCount == 0) return;

        float angleStep = (cardCount > 1) ? maxAngle / (cardCount - 1) : 0; // 카드 간격 계산
        float startAngle = -maxAngle / 2; // 첫 번째 카드의 시작 각도

        for (int i = 0; i < cardCount; i++)
        {
            float angle = startAngle + (angleStep * i); // 각 카드의 각도 계산
            float radian = angle * Mathf.Deg2Rad; // 삼각함수 계산을 위해 라디안 변환

            // 원호 상에서 위치 계산
            Vector3 cardPosition = new Vector3(Mathf.Sin(radian) * radius, Mathf.Cos(radian) * radius, 0);
            
            cards[i].transform.localPosition = cardPosition;
            cards[i].transform.localRotation = Quaternion.Euler(0, 0, angle); // 카드 기울이기
        }
    }
}