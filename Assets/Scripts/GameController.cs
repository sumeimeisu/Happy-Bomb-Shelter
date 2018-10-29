using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public static GameController instance = null;
	public Transform respawnPoint;

	public float[] sceneWaterLines;
	public float[] sceneAnimLines;

	public float waterline;
	[HideInInspector]
	public float animline;
	public int score = 0;
	public int highScore = 0;

	public int stage = 1;
	public int lives = 3;

	public GameObject playerPrefab;
	[HideInInspector]
	public GameObject player;
	[HideInInspector]
	public Vector2 camOffset;

	AsyncOperation asyncLoadLevel;

	public object playerRb { get; private set; }

	void Awake()
	{
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	public void NextStage()
	{
		stage++;
		LoadScene(stage);
	}

	public void LoadScene(int scene)
	{
		SceneManager.LoadScene(scene + 1);

		waterline = sceneWaterLines[(scene == 0 ? 0 : 1)];
		animline = sceneAnimLines[(scene == 0 ? 0 : 1)];
	}

	IEnumerator Win()
	{
		yield return new WaitForSeconds(3f);
		Image winText = GameObject.FindGameObjectWithTag("Finish").GetComponent<Image>();
		winText.enabled = true;	
	}

	IEnumerator ThrowPlayer()
	{
		Debug.Log("throw");
		Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

		player.transform.position = new Vector3(253, 65);
		playerRb.simulated = false;

		yield return new WaitForSeconds(2f);

		Debug.Log("resim");

		playerRb.simulated = true;
		playerRb.AddForce(new Vector2(200, 200), ForceMode2D.Impulse);
	}

	public void RestartScene()
	{
		SceneManager.LoadScene(0);
	}

	void GameOver()
	{
		StopAllCoroutines();
		StartCoroutine(RestartGame());
	}

	public IEnumerator RespawnPlayer()
	{
		lives--;
		if (lives == 0) GameOver();
		yield return new WaitForSeconds(3f);
		player = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
		
		// Change this to some parent camera class
		Camera.main.GetComponent<FollowPlayer>().ReassignPlayer();
	}

	IEnumerator RestartGame()
	{
		Image gameOverText = GameObject.FindGameObjectWithTag("GameOverText").GetComponent<Image>();
		gameOverText.enabled = true;

		yield return new WaitForSeconds(3f);

		stage = 1;
		lives = 3;
		SceneManager.LoadScene(0);
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		if (scene.buildIndex != 0)
		{
			respawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
			player = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
			if (scene.buildIndex > 2) StartCoroutine(ThrowPlayer());
			if (scene.buildIndex == 5) StartCoroutine(Win());
		}
	}
}