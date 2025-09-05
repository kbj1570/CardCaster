using UnityEngine;
using DG.Tweening;

public class BezierMissile : MonoBehaviour {

	Vector2[] point = new Vector2[4];
	bool hit = false;

	private float t = 0;
	float spd = 3f;
	float posA = 3f;
	float posB = 3f;
	public Vector3 masterPos;
	public Vector3 enemyPos;

	void Start()
	{
		point[0] = masterPos;
		point[1] = PointSetting(masterPos);
		point[2] = PointSetting(enemyPos);
		point[3] = enemyPos;
	}

	void FixedUpdate() {
		if (hit) return;

		if (Vector2.Distance(transform.position, enemyPos) < 0.5f)
		{
			hit = true;
			Destroy(gameObject, 0.1f);
		}

		t += Time.deltaTime * spd;
		if (t > 1) t = 1;

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
}