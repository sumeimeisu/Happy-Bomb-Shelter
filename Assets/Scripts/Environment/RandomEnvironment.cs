using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnvironment : MonoBehaviour 
{
	Transform camTransform;

	[Header("Adjust")]
	public float scrollSpeed;

	// area to spawn in
	public int width;
	public int height;
	public int yoffset;

	public int density;

	[Header("References")]
	public GameObject[] objects;
	public int num;

	int index = 0;

	GameObject[] screens;

	float lastCamX;

	private void Start()
	{
		screens = new GameObject[num];

		camTransform = Camera.main.transform;
		lastCamX = camTransform.position.x;

		for (int i = 0; i < num; i++)
		{
			screens[i] = SpawnRandomInRect(width, height, density);
			screens[i].transform.localPosition += Vector3.right * width * i;
		}
	}

	private void Update()
	{
		float deltaX = camTransform.position.x - lastCamX;
		transform.position += Vector3.right * (deltaX * scrollSpeed);
		lastCamX = camTransform.position.x;

		if (camTransform.position.x > screens[index].transform.position.x + 3 * width / 2f)
		{
			Destroy(screens[index]);
			screens[index] = SpawnRandomInRect(width, height, density);
			screens[index].transform.position = Vector3.right * (screens[index - 1 < 0 ? screens.Length - 1 : index - 1].transform.position.x + width);
			index++;
			if (index > screens.Length - 1) index = 0;
		}
	}

	GameObject SpawnRandomInRect(int w, int h, int d)
	{
		GameObject spawnedParent = new GameObject();
		spawnedParent.transform.SetParent(transform);

		for (int i = 0; i < d; i++)
		{
			int x = (int) Random.Range(-w / 2f, w / 2f);
			int y = (int) Random.Range(-h / 2f, h / 2f);

			GameObject spawned = Instantiate(objects[Random.Range(0, objects.Length)], spawnedParent.transform) as GameObject;
			spawned.transform.localPosition = new Vector3(x, y + yoffset, 0);
		}

		return spawnedParent; 
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position + new Vector3(0, yoffset, 0), new Vector3(width, height, 0));
	}
}
