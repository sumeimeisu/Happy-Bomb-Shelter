using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour 
{
	Animator anim;
	BoxCollider2D col;

	public bool electrified = true;
	bool lastElectrified = true;

	void Start () 
	{
		anim = GetComponent<Animator>();
		col = GetComponent<BoxCollider2D>();

		anim.SetLayerWeight(1, 1);
	}
	
	void Update () 
	{
		if (electrified != lastElectrified) DeStatic();
		lastElectrified = electrified;
	}

	void DeStatic()
	{
		anim.SetLayerWeight(1, 0);
		col.enabled = false;
	}
}
