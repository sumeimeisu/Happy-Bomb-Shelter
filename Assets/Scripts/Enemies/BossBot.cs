using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBot : MonoBehaviour 
{
	public BossArm leftArm;
	public BossArm rightArm;

	public GameObject explosion;
	public bool alive = true;

	void Start () 
	{
		if (!alive) StartCoroutine(Die());
	}

	IEnumerator Die()
	{
		for (int i = 0; i < 10; i++)
		{
			Instantiate(explosion, transform.position + new Vector3(Random.Range(-85f, 85f), Random.Range(-59.5f, 59.5f)), Quaternion.identity);
			yield return new WaitForSeconds(0.3f);
		}
	}
}
