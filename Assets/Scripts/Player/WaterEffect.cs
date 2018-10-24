using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffect : MonoBehaviour 
{
	public float waterLine;
	public float animLine;
	public float splashOffset;

	public float diveUnderwaterMaxSpeed;
	public float diveSlowdown;

	public bool underwater;
	[NonSerialized] public bool animUnderwater;

	public ParticleSystem smallBubbles;
	public ParticleSystem splash;

	PlayerBehaviour player;
	
	bool lastUnderwater;

	private void Start()
	{
		player = GetComponent<PlayerBehaviour>();
	}

	void Update () 
	{
		lastUnderwater = underwater;

		underwater = transform.position.y <= waterLine;
		if (player.state == PlayerBehaviour.playerState.Floating) underwater = true;

		animUnderwater = transform.position.y < animLine;

		if (lastUnderwater != underwater)
		{
			if (player.state != PlayerBehaviour.playerState.Diving) player.state = PlayerBehaviour.playerState.Floating;
			if (player.state == PlayerBehaviour.playerState.Diving)	Instantiate(splash, new Vector2(transform.position.x, waterLine + splashOffset), Quaternion.identity);

			if (underwater) smallBubbles.Play();
			else smallBubbles.Stop();
		}
	}
}
