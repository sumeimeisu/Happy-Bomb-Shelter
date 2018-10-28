using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour 
{
	public Transform player;
	public Vector2 center;
	public Vector2 dimensions;

	Vector3 offset;
	Camera cam;

	private void Start()
	{
		offset = transform.position - player.transform.position;
		cam = Camera.main;
	}

	private void Update()
	{
		if (!player) return;

		Vector2 cameraPosition = new Vector2();

		if (player.transform.position.x + offset.x < center.x - (dimensions.x / 2f) + (cam.orthographicSize * cam.aspect))
			cameraPosition.x = center.x - (dimensions.x / 2f) + (cam.orthographicSize * cam.aspect);
		else if (player.transform.position.x + offset.x > center.x + (dimensions.x / 2f) - (cam.orthographicSize * cam.aspect))
			cameraPosition.x = center.x + (dimensions.x / 2f) - (cam.orthographicSize * cam.aspect);
		else cameraPosition.x = player.transform.position.x + offset.x;

		if (player.transform.position.y + offset.y < center.y - (dimensions.y / 2f) + (cam.orthographicSize))
			cameraPosition.y = center.y - (dimensions.y / 2f) + (cam.orthographicSize);
		else if (player.transform.position.y + offset.y > center.y + (dimensions.y / 2f) - (cam.orthographicSize))
			cameraPosition.y = center.y + (dimensions.y / 2f) - (cam.orthographicSize);
		else cameraPosition.y = player.transform.position.y + offset.y;

		transform.position = (Vector3)cameraPosition + Vector3.forward * transform.position.z;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(center, dimensions);
	}
}
