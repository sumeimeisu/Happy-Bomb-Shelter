using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossButton : MovingEntity
{
	public Sprite pressedSprite;

	[HideInInspector] public bool pressed = false;
	BoxCollider2D col;
	SpriteRenderer sr;

	void Start()
	{
		col = GetComponent<BoxCollider2D>();
		sr = GetComponent<SpriteRenderer>();
	}

	public override void divedOnto(Collision2D collision)
	{
		sr.sprite = pressedSprite;
		col.enabled = false;
		pressed = true;
		StartCoroutine(WaitForSceneSwitch());
	}

	IEnumerator WaitForSceneSwitch()
	{
		yield return new WaitForSeconds(3f);
		GameController.instance.LoadScene(0);
		GameController.instance.stage++;
	}
}
