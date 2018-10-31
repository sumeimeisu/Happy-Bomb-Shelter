using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingEnvironment : MonoBehaviour 
{
	[Header("Properties")]
	public float translateSpeed;
	Vector3 origPos;

	[Header("Sprite")]
	public float width;

	void Start()
	{
		origPos = transform.position;
	}

	void Update () 
	{
		transform.position += Vector3.right * translateSpeed;
		if (transform.position.x > origPos.x + width)
		{
			transform.position = origPos;
		}
	}
}
