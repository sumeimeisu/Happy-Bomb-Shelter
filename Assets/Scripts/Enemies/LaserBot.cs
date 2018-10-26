﻿using System.Collections;
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

	#region Properties
	[Header("Movement")] [SerializeField]
	public float speed;
	public float radius;
	public float spinRate;
	public float knockback;
	public float attackCycle;

	[Header("References")] [SerializeField]

	public LaserBotNode node1;
	public LaserBotNode node2;

	public ParticleSystem lineStatic;

	public ParticleSystem smallExplosion;

	public BoxCollider2D lineStaticTrigger;

	Transform playerTransform;
	#endregion

	private bool onlyOnce = true; 

	private void Start()
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		StartCoroutine(DualAttackPattern());
	}

	private void Update()
	{
		if (!playerTransform) return; 

		if (node1 && node2)
		{
			UpdateLineStaticPosition();

			Vector3[] positions = FindTargetPosition();
			node1.MoveToTarget(playerTransform.position + positions[0]);
			node2.MoveToTarget(playerTransform.position + positions[1]);
		}
		else if (node1)
		{
			node1.MoveToTarget(playerTransform.position);
			if (onlyOnce)
			{
				StartCoroutine(SingleAttackPattern(node1));
				Destroy(lineStatic.gameObject);
				onlyOnce = false;
			}
		}
		else if (node2)
		{
			node2.MoveToTarget(playerTransform.position);
			if (onlyOnce)
			{
				StartCoroutine(SingleAttackPattern(node2));
				Destroy(lineStatic.gameObject);
				onlyOnce = false;
			}
		}
		else 
		{
			Destroy(gameObject);
		}
	}

	Vector3[] FindTargetPosition()
	{
		Vector3[] targetPositions = new Vector3[2];

		Vector3 targetPos = new Vector3(Mathf.Sin(Time.time * spinRate), Mathf.Cos(Time.time * spinRate)) * radius;

		targetPositions[0] = targetPos;
		targetPositions[1] = -targetPos;
		
		return targetPositions;	
	}

	public IEnumerator DualAttackPattern()
	{
		while (true)
		{
			yield return new WaitForSeconds(attackCycle);
			node1.circleStatic.Play();
			node1.CycleAttack();
			node2.circleStatic.Play();
			node2.CycleAttack();

			lineStaticTrigger.enabled = true;
			lineStatic.Play();
			
			yield return new WaitForSeconds(attackCycle);
			node1.circleStatic.Stop();
			node1.CycleAttack();
			node2.circleStatic.Stop();
			node2.CycleAttack();

			lineStaticTrigger.enabled = false;
			lineStatic.Stop();
		}
	}
	
	public IEnumerator SingleAttackPattern(LaserBotNode node)
	{
		while(true)
		{
			yield return new WaitForSeconds(attackCycle);
			node.circleStatic.Play();
			node.CycleAttack();
			yield return new WaitForSeconds(attackCycle);
			node.circleStatic.Stop();
			node.CycleAttack();
		}
	}

	void UpdateLineStaticPosition()
	{
		var sh = lineStatic.shape;

		lineStatic.transform.position = (node2.transform.position - node1.transform.position) / 2 + node1.transform.position;
		Quaternion rotation = Quaternion.FromToRotation(Vector3.right, lineStatic.transform.position - node1.transform.position);
		lineStatic.transform.rotation = rotation;

		sh.scale = new Vector3((node1.transform.position - node2.transform.position).magnitude, 1, 1);

		lineStaticTrigger.size = new Vector2((node1.transform.position - node2.transform.position).magnitude, 10);
	}
}
