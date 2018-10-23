using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffect : MonoBehaviour 
{
	public float waterLine;
	public float animLine;

	public float diveUnderwaterMaxSpeed;
	public float diveSlowdown;

	[NonSerialized] public bool underwater;
	[NonSerialized] public bool animUnderwater;

	public ParticleSystem smallBubbles;

	PlayerBehaviour player;
	
	bool lastUnderwater;

	private void Start()
	{
		player = GetComponent<PlayerBehaviour>();
	}

	void Update () 
	{
		lastUnderwater = underwater;

		underwater = transform.position.y < waterLine;
		animUnderwater = transform.position.y < animLine;

		if (lastUnderwater != underwater)
		{
			if (player.state != PlayerBehaviour.playerState.Diving)player.state = PlayerBehaviour.playerState.Floating;	
			
			if (underwater) smallBubbles.Play();
			else smallBubbles.Stop();
		}
	}
}
