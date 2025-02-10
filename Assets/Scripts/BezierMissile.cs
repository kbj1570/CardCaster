using UnityEngine;
using DG.Tweening;

public class BezierMissile : MonoBehaviour {

        Vector2[] point = new Vector2[4];
    bool hit = false;

    [SerializeField] [Range(0, 1)] private float t = 0;
    public float spd = 0.5f;
    public float posA = 100f;
    public float posB = 100f;
    public Vector3 master;
    public Vector3 enemy;

    void Start()
    {
        // 베지에 곡선용 포인트 설정
        point[0] = master;  // 시작점
        point[1] = PointSetting(master); // 곡선을 위한 제어점1
        point[2] = PointSetting(enemy);  // 곡선을 위한 제어점2
        point[3] = enemy;   // 도착점
    }

    void FixedUpdate() {
        if (hit) return;

        // 목표 지점 도착 체크
        if (Vector2.Distance(transform.position, enemy) < 0.5f)
        {
            hit = true;
            BattleManagerAlt.Inst.ActionDone();
            Destroy(gameObject, 0.1f);
        }

        // 베지에 곡선을 따라 이동
        t += Time.deltaTime * spd;
        if (t > 1) t = 1; // t 값이 1을 넘지 않도록 제한

        DrawTrajectory();
    }

    Vector2 PointSetting(Vector2 origin){
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        float x = posA * Mathf.Cos(angle) + origin.x;
        float y = posB * Mathf.Sin(angle) + origin.y;
        return new Vector2(x, y);
    }

    void DrawTrajectory() {
        Vector2 bezierPos = new Vector2(
            FourPointBezier(point[0].x, point[1].x, point[2].x, point[3].x),
            FourPointBezier(point[0].y, point[1].y, point[2].y, point[3].y)
        );

        transform.position = bezierPos;
    }

    private float FourPointBezier(float a, float b, float c, float d)
    {
        return Mathf.Pow((1 - t), 3) * a
            + Mathf.Pow((1 - t), 2) * 3 * t * b
            + Mathf.Pow(t, 2) * 3 * (1 - t) * c
            + Mathf.Pow(t, 3) * d;
    }
 
    // Vector2[] point = new Vector2[4];
    // bool hit = false;

    // [SerializeField] [Range(0, 1)] private float t = 0;
    // public float spd;
    // public float posA = 100f;
    // public float posB = 100f;
    // public Vector3 master;
    // public Vector3 enemy;

    // void Start()
    // {
    //     Debug.Log("미사일을 쐈어요");
    //     point[0] = master;// P0
    //     point[1] = PointSetting(master);// P1
    //     point[2] = PointSetting(enemy);// P2
    //     point[3] = enemy;// P3
    // }

    // void FixedUpdate() {
        
    //     if (hit) return;

    //     if(Vector2.Distance(transform.position, enemy) < 1f)
    //     {
    //         Destroy(gameObject, 0.1f);
    //     }

    //     t += Time.deltaTime * spd;
    //     DrawTrajectory();
    // }

    // public void Move()
    // {
    //     if (t > 1) return;
    //     t += Time.deltaTime * spd;
    //     DrawTrajectory();
    // }

    // Vector2 PointSetting(Vector2 origin){
    //     float x, y;

    //     x = posA * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.x;
    //     y = posB * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.y;
    //     return new Vector2(x, y);
    // }

    // void DrawTrajectory() {
    //     transform.DOMove(new Vector2(
    //         FourPointBezier(point[0].x, point[1].x, point[2].x, point[3].x),
    //         FourPointBezier(point[0].y, point[1].y, point[2].y, point[3].y)), 0);
    // }
    // private float FourPointBezier(float a, float b, float c, float d)
    // {
    //     return Mathf.Pow((1 - t), 3) * a
    //         + Mathf.Pow((1 - t), 2) * 3 * t * b
    //         + Mathf.Pow(t, 2) * 3 * (1 - t) * c
    //         + Mathf.Pow(t, 3) * d;
    // }
}