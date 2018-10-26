﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingEntity : MonoBehaviour
{
	protected bool canMove()
	{
		return true; // !GameController.instance.paused;
	}

	public virtual void divedOnto(Collision2D collision)
	{
	
	}
}