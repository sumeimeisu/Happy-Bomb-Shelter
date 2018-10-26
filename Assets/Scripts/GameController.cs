using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public static GameController instance = null;

	public float waterline = 0.0f;
	public int score = 0;
	public int highScore = 0;

	public bool paused = false;

	void Awake()
	{
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	private void Update()
	{
		/*
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			paused = !paused;
		}
		*/
	}

	public void RestartScene()
	{
		SceneManager.LoadScene(0);
	}
}