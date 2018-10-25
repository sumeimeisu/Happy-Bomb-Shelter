using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBotNode : MonoBehaviour 
{
	[SerializeField] private LaserBot parent;

	public void MoveToTarget(Vector3 target)
	{
		if ((transform.position - target).magnitude < 1) return;

		Vector3 direction = target - transform.position;
		Debug.DrawRay(transform.position, direction, Color.cyan);
		transform.position += direction.normalized * parent.speed;
	}
}
