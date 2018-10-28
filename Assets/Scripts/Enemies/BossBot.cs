using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBot : MonoBehaviour 
{
	public BossArm leftArm;
	public BossArm rightArm;

	void Start () 
	{
		if (GameController.instance.stage == 2)
		{
			Debug.Log("onlyOne");
			leftArm.StartAttacking();
		}
		else if (GameController.instance.stage == 3)
		{
			Debug.Log("both");
			leftArm.StartAttacking();
			rightArm.StartAttacking();
		}
	}
}
