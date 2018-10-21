using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteScroll : MonoBehaviour 
{
	public float speed; 

	private void LateUpdate()
	{
		transform.position += Vector3.right * speed;
	}
}
