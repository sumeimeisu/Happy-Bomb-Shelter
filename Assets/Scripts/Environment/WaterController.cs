using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour 
{
	Animator anim;

	public bool electrified = false;

	void Start () 
	{
		anim = GetComponent<Animator>();
	}
	
	void Update () 
	{
		if (electrified) anim.SetLayerWeight(1, 1);
		else anim.SetLayerWeight(1, 0);
	}
}
