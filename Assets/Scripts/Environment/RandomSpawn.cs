using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour 
{
	// area to spawn in
	public int width;
	public int height;
	public int yoffset;

	public int density;

	[Header("References")]
	public GameObject[] objects;

	void Start () 
	{
		SpawnRandomInRect(width, height, density);
	}

	void SpawnRandomInRect(int w, int h, int d)
	{
		for (int i = 0; i < d; i++)
		{
			int x = (int)Random.Range(-w / 2f, w / 2f);
			int y = (int)Random.Range(-h / 2f, h / 2f);

			GameObject spawned = Instantiate(objects[Random.Range(0, objects.Length)], transform);
			spawned.transform.localPosition = new Vector3(x, y + yoffset, 0);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position + new Vector3(0, yoffset, 0), new Vector3(width, height, 0));
	}
}
