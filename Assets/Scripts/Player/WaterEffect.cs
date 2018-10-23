using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffect : MonoBehaviour 
{
	public float waterLine;

	[NonSerialized] public bool underwater;

	public ParticleSystem smallBubbles;
	
	bool lastUnderwater;

	void Update () 
	{
		lastUnderwater = underwater;
		underwater = (transform.position.y < waterLine) ? true : false;

		if (lastUnderwater != underwater)
		{
			if (underwater) smallBubbles.Play();
			else smallBubbles.Stop();
		}
	}
}
