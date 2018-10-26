using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}