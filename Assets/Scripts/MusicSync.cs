using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSync : MonoBehaviour 
{
	public AudioSource aboveWaterSong;
	public AudioSource underWaterSong;

	bool underwater = false;
	bool lastUnderwater;

	Transform playerTransform;

	private void Start()
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update()
	{
		if (!playerTransform) return;

		lastUnderwater = underwater;
		underwater = (playerTransform.position.y < GameController.instance.waterline - 5);

		if (lastUnderwater != underwater)
		{
			StopAllCoroutines();
			StartCoroutine(SwitchTracks(underwater));
		}
	}

	IEnumerator SwitchTracks(bool under)
	{
		float t = under ? underWaterSong.volume : aboveWaterSong.volume;
		while(t < 1)
		{
			if (under)
			{
				underWaterSong.volume = t;
				aboveWaterSong.volume = (1 - t);
			}
			else 
			{
				aboveWaterSong.volume = t;
				underWaterSong.volume = (1 - t);
			}
			t += Time.deltaTime;
			yield return null;
		}
	}
}
