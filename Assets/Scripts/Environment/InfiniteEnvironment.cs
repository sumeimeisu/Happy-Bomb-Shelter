using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteEnvironment : MonoBehaviour 
{
	Transform camTransform;

	[Header("References")]
	public float width; // based on sprite width
	public int viewportWidth = 240;
	public float num;

	private void Start()
	{
		camTransform = Camera.main.transform;
	}

	private void Update()
	{
		if (camTransform.position.x > transform.position.x + (num / 2) * width)
		{
			Vector3 newPos = new Vector3(transform.position.x + width, transform.position.y, transform.position.z);
			transform.position = newPos;
		}
	}
}
