using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public static GameController instance = null;
	public Transform respawnPoint;

	public float[] sceneWaterLines;
	public float waterline;
	public int score = 0;
	public int highScore = 0;

	public int stage = 1;
	public int lives = 3;

	public GameObject player;

	AsyncOperation asyncLoadLevel;

	void Awake()
	{
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		//player = GameObject.FindGameObjectWithTag("Player
		player = Instantiate(player, respawnPoint.position, Quaternion.identity);
	}

	public void LoadScene(int scene)
	{
		StartCoroutine(WaitForLevelLoad(scene));
	}

	public void RestartScene()
	{
		SceneManager.LoadScene(0);
	}

	void RespawnPlayer()
	{
		Instantiate(player, transform.position, Quaternion.identity);
	}

	IEnumerator WaitForLevelLoad(int scene)
	{
		asyncLoadLevel = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
		while (!asyncLoadLevel.isDone)
		{
			yield return null;
		}

		waterline = sceneWaterLines[scene];
		player = GameObject.FindGameObjectWithTag("Player");

		Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

		if (scene == 0)
		{
			player.transform.position = new Vector3(253, 65);
			playerRb.simulated = false;
		}

		if (scene == 0)
		{
			yield return new WaitForSeconds(2f);
			playerRb.simulated = true;
			playerRb.AddForce(new Vector2(200, 200), ForceMode2D.Impulse);
		}
	}
}