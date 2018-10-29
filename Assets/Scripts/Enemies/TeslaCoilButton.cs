using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaCoilButton : MovingEntity 
{
	public Sprite pressedSprite;
	public TeslaCoil parentCoil;
	[HideInInspector] public bool pressed = false;
	BoxCollider2D col;
	SpriteRenderer sr;

	public AudioClip buttonPressSound;
	AudioSource audioS;

	void Start () 
	{
		col = GetComponent<BoxCollider2D>();
		sr = GetComponent<SpriteRenderer>();
		audioS = GetComponent<AudioSource>();
	}

	public override void divedOnto(Collision2D collision)
	{
		audioS.clip = buttonPressSound;
		audioS.Play();

		sr.sprite = pressedSprite;
		col.enabled = false;
		pressed = true;

		parentCoil.CheckActiveState();
	}
}
