using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour 
{
	public Transform projectilePool;
	public GameObject bullet;
	public float timer;
	public float speed;
	[NonSerialized] public Vector2 velocity;

	SpriteRenderer sr;
	
	void Start () 
	{
		sr = GetComponent<SpriteRenderer>();
		StartCoroutine(Explode());
	}
	
	void Update () 
	{
		transform.position += new Vector3(velocity.x, velocity.y) * speed;
	}

	IEnumerator Explode()
	{
		yield return new WaitForSeconds(timer - 0.6f);
		for (int i = 0; i < 4; i++)
		{
			sr.enabled = !sr.enabled;
			yield return new WaitForSeconds(0.1f);
		}

		for (int i = 0; i < 8; i++)
		{
			GameObject instBullet = Instantiate(bullet, projectilePool);
			instBullet.transform.position = transform.position;
			instBullet.GetComponent<BulletScript>().velocity = new Vector2(Mathf.Cos(i * Mathf.PI / 4), Mathf.Sin(i * Mathf.PI / 4));
		}

		Destroy(gameObject);
	}
}
