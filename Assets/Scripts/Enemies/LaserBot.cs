using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBot : MonoBehaviour 
{
	/* LaserBot Todo:
	 *   > two nodes 
	 *   > each node needs to move to a point 90 degrees on a circle from the closest point
	 *   > attack regularly and activate particle system
	 *   > when one dies, change behavior to rushing
	*/

	public enum LaserState
	{

	}

	Transform playerTransform;

	public LaserBotNode node1;
	public LaserBotNode node2;

	public float speed;
	public float radius;
	public float spinRate;

	private void Start()
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void Update()
	{
		if (node1 && node2)
		{
			Vector3[] positions = FindTargetPosition();
			node1.MoveToTarget(playerTransform.position + positions[0]);
			node2.MoveToTarget(playerTransform.position + positions[1]);
		}
	}

	Vector3[] FindTargetPosition()
	{
		Vector3[] targetPositions = new Vector3[2];

		Vector3 targetPos = new Vector3(Mathf.Sin(Time.time * spinRate), Mathf.Cos(Time.time * spinRate)) * radius;

		Debug.DrawRay(playerTransform.position - targetPos, Vector3.up * radius, Color.green);
		Debug.DrawRay(playerTransform.position + targetPos, Vector3.up * radius, Color.red);


		targetPositions[0] = targetPos;
		targetPositions[1] = -targetPos;
		
		return targetPositions;	
	}

	IEnumerator AttackPattern()
	{
		yield return null;
	}

	void FindTargetPositionSingle()
	{

	}
}
