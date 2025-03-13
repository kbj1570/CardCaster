using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DungeonCamera : MonoBehaviour
{
    public Transform player; // 플레이어
    public float followSpeed = 0.3f; // 카메라 이동 속도

    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.DOMove(targetPosition, followSpeed).SetEase(Ease.OutSine);
    }
}