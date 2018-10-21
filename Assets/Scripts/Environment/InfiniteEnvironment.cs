using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteEnvironment : MonoBehaviour 
{
	[Header("Adjust")]
	public float scrollSpeed;

	Transform camTransform;

	[Header("References")]
	public float width; // based on sprite width
	public int viewportWidth = 240;
	public float num;

	float lastCamX;

	private void Start()
	{
		camTransform = Camera.main.transform;
		lastCamX = camTransform.position.x;
	}

	private void Update()
	{
		float deltaX = camTransform.position.x - lastCamX;
		transform.position += Vector3.right * (deltaX * scrollSpeed);
		lastCamX = camTransform.position.x;

		if (camTransform.position.x > transform.position.x + (num / 2) * width)
		{
			Vector3 newPos = new Vector3(transform.position.x + width, transform.position.y, transform.position.z);
			transform.position = newPos;
		}
	}
}
