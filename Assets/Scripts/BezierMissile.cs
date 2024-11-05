using UnityEngine;
using DG.Tweening;

public class BezierMissile : MonoBehaviour {
 
    Vector2[] point = new Vector2[4];
    bool hit = false;

    [SerializeField] [Range(0, 1)] private float t = 0;
    public float spd;
    public float posA = 100f;
    public float posB = 100f;
    public GameObject master;
    public GameObject enemy;

    void Start()
    {

        point[0] = master.transform.position;// P0
        point[1] = PointSetting(master.transform.position);// P1
        point[2] = PointSetting(enemy.transform.position);// P2
        point[3] = enemy.transform.position;// P3
    }

    void FixedUpdate() {
        if (t > 1) return;
        if (hit) return;

        if(Vector2.Distance(transform.position, enemy.transform.position) < 1f)
        {Destroy(gameObject, 0.1f);}

        t += Time.deltaTime * spd;
        DrawTrajectory();
    }

    Vector2 PointSetting(Vector2 origin){
        float x, y;

        x = posA * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.x;
        y = posB * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.y;
        return new Vector2(x, y);
    }

    void DrawTrajectory() {
        transform.DOMove(new Vector2(
            FourPointBezier(point[0].x, point[1].x, point[2].x, point[3].x),
            FourPointBezier(point[0].y, point[1].y, point[2].y, point[3].y)), 0);
    }
    private float FourPointBezier(float a, float b, float c, float d)
    {
        return Mathf.Pow((1 - t), 3) * a
            + Mathf.Pow((1 - t), 2) * 3 * t * b
            + Mathf.Pow(t, 2) * 3 * (1 - t) * c
            + Mathf.Pow(t, 3) * d;
    }
}