using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaCoilButton : MovingEntity 
{
	public Sprite pressedSprite;
	public TeslaCoil parentCoil;

	public ParticleSystem pressedParticles;
	public ParticleSystem staticShield1;
	public ParticleSystem staticShield2;

	[HideInInspector] public bool pressed = false;
	BoxCollider2D col;
	SpriteRenderer sr;

	public AudioClip buttonPressSound;
	AudioSource audioS;

	CircleCollider2D trigger;

	void Start () 
	{
		col = GetComponent<BoxCollider2D>();
		trigger = GetComponent<CircleCollider2D>();

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

	public IEnumerator CycleDefense()
	{
		while (!pressed)
		{
			trigger.enabled = true;
			staticShield1.Play();
			staticShield2.Play();

			yield return new WaitForSeconds(parentCoil.defenseCycle);

			trigger.enabled = false;
			staticShield1.Stop();
			staticShield2.Stop();

			yield return new WaitForSeconds(parentCoil.defenseCycle);
		}
	}
}
