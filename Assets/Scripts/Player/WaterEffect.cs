using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffect : MonoBehaviour 
{
	public float waterLine;
	float floatingRange = 20;

	public float diveUnderwaterMaxSpeed;
	public float floatingGravity;

	[NonSerialized] public bool underwater;

	public ParticleSystem smallBubbles;
	
	bool lastUnderwater;

	void Update () 
	{
		lastUnderwater = underwater;

		//underwater = (transform.position.y < waterLine) ? true : false;

		if (lastUnderwater != underwater)
		{
			if (underwater) smallBubbles.Play();
			else smallBubbles.Stop();
		}
	}

	public bool IsFloating()
	{
		return (transform.position.y < waterLine + floatingRange && transform.position.y > waterLine - floatingRange);
	}

	public bool IsUnderWater()
	{
		return (transform.position.y < waterLine) ;
	}
}
