using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MovingEntity 
{
	public float bulletSpeed;
	[NonSerialized] public Vector2 velocity;
	public float lifetime;

	private void Start()
	{
		StartCoroutine(DestroyAfterTime());
	}

	void Update () 
	{
		if (!canMove()) return;
		transform.position += new Vector3(velocity.x, velocity.y, 0) * bulletSpeed;
	}

	IEnumerator DestroyAfterTime()
	{
		yield return new WaitForSeconds(lifetime);
		Destroy(gameObject);
	}
}
