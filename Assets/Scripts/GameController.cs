using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public static GameController instance = null;

	[HideInInspector]
	public float waterline = 0.0f;
	public int score = 0;
	public int highScore = 0;

	public int stage = 1;

	public GameObject player;

	AsyncOperation asyncLoadLevel;

	void Awake()
	{
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		player = GameObject.FindGameObjectWithTag("Player");
	}

	public void LoadScene(int scene)
	{
		StartCoroutine(WaitForLevelLoad(scene));
	}

	public void RestartScene()
	{
		SceneManager.LoadScene(0);
	}

	IEnumerator WaitForLevelLoad(int scene)
	{
		asyncLoadLevel = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
		while (!asyncLoadLevel.isDone)
		{
			yield return null;
		}

		player = GameObject.FindGameObjectWithTag("Player");
		Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

		if (scene == 0)
		{
			player.transform.position = new Vector3(253, 65);
			playerRb.simulated = false;
		}
		
		waterline = player.GetComponent<WaterEffect>().waterLine;

		if (scene == 0)
		{
			yield return new WaitForSeconds(2f);
			playerRb.simulated = true;
			playerRb.AddForce(new Vector2(200, 200), ForceMode2D.Impulse);
		}
	}
}