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
	public float breakthroughSpeed;

	[NonSerialized] public bool underwater;
	[NonSerialized] public bool animUnderwater;

	public ParticleSystem smallBubbles;
	public ParticleSystem splash;

	PlayerBehaviour player;
	
	bool lastUnderwater;

	private void Start()
	{
		player = GetComponent<PlayerBehaviour>();
		waterLine = GameController.instance.waterline;
		animLine = GameController.instance.animline;

		//	Debug.Log("WE : " + waterLine);

		underwater = lastUnderwater = transform.position.y <= waterLine;
	}

	void Update () 
	{
		lastUnderwater = underwater;

		underwater = transform.position.y <= waterLine;
		if (player.state == PlayerBehaviour.playerState.Floating) underwater = true;

		animUnderwater = transform.position.y < animLine;

		// not really a complete fix, this means that floating on the surface is considered underwater. 
		// there's no direct path from under/above water to floating
		// you have to go under->above->floating, which results in an extra call when surfacing
		// not a huge problem i guess...

		if (lastUnderwater != underwater)
		{
			if (player.state != PlayerBehaviour.playerState.Diving && player.rb.velocity.magnitude < breakthroughSpeed) player.state = PlayerBehaviour.playerState.Floating;
			if (player.state == PlayerBehaviour.playerState.Diving)
			{
				Instantiate(splash, new Vector2(transform.position.x, waterLine + splashOffset), Quaternion.identity);
				player.audioS.clip = player.splash;
				player.audioS.Play();
			}

			if (underwater) smallBubbles.Play();
			else smallBubbles.Stop();
		}
	}
}
