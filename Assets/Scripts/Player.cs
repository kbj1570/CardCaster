using UnityEngine;

public class Player : MonoBehaviour
{
	public EDirection direction;
	public Direction directionArrow;

	void Update()
	{
		direction = directionArrow.GetDirection();
	}

	public EDirection GetDirection()
	{
		return direction;
	}
	public void SetDirection(EDirection newDirection)
	{
		direction = newDirection;
		directionArrow.SetDirection(newDirection);
	}
}