using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public static LineManager Inst {get; private set;}
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform field1;
    [SerializeField] Transform field2;
    [SerializeField] Transform field3;
    [SerializeField] Transform field4;
    [SerializeField] Transform field5;
    [SerializeField] Transform field6;
    [SerializeField] Transform player;
    [SerializeField] Transform opponent;
    Vector3 startPoint;
    Vector3 endPoint;
    Vector3 result;
    

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ready();
        }

        if (Input.GetMouseButton(0))
        {
            // 마우스 왼쪽 버튼을 누르고 있는 도중의 처리
            endPoint = DetectFieldArea();
            DrawLine(startPoint, endPoint);

        }

        if (Input.GetMouseButtonUp(0))
        {
            End();
        }
    }
    void Ready()
    {
        lineRenderer.enabled = true;
        startPoint = DetectFieldArea();
    }

    public void DrawLine(Vector3 from, Vector3 to)
    {
        
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
    }
    void End()
    {
        lineRenderer.enabled = false;
    }

    Vector3 DetectFieldArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);

        foreach(RaycastHit2D ray in hits)
        {
            switch(ray.collider.gameObject.layer)
            {
                case 7:
                result = field1.position;
                result.z = -10;
                return result;

                case 8:
                result = field2.position;
                result.z = -10;
                return result;

                case 9:
                result = field3.position;
                result.z = -10;
                return result;

                case 10:
                result = field4.position;
                result.z = -10;
                return result;

                case 11:
                result = field5.position;
                result.z = -10;
                return result;

                case 12:
                result = field6.position;
                result.z = -10;
                return result;

                case 14:
                result = player.position;
                result.z = -10;
                return result;

                case 15:
                result = opponent.position;
                result.z = -10;
                return result;

                default:
                return Utils.MousePos;
            }
        }
        return Utils.MousePos;
    }

}
