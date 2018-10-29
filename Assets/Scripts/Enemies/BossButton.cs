using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossButton : MovingEntity
{
	public Sprite pressedSprite;
	public AudioClip pressedSound;

	public GameObject explosion;

	[HideInInspector] public bool pressed = false;
	BoxCollider2D col;
	SpriteRenderer sr;
	AudioSource audioS;

	void Start()
	{
		col = GetComponent<BoxCollider2D>();
		sr = GetComponent<SpriteRenderer>();
		audioS = GetComponent<AudioSource>();
	}

	public override void divedOnto(Collision2D collision)
	{
		audioS.clip = pressedSound;
		audioS.Play();
		sr.sprite = pressedSprite;
		col.enabled = false;
		pressed = true;
		StartCoroutine(WaitForSceneSwitch());
	}

	IEnumerator WaitForSceneSwitch()
	{
		for (int i = 0; i < 10; i++)
		{
			Instantiate(explosion, new Vector3(Random.Range(-128f, 128f), Random.Range(-120f, 120f)), Quaternion.identity);
			//Instantiate(explosion, new Vector3(Random.Range(-128f, 128f), Random.Range(-120f, 120f)), Quaternion.identity);
			yield return new WaitForSeconds(0.3f);
		}
		//yield return new WaitForSeconds(3f);
		GameController.instance.NextStage();
	}
}
