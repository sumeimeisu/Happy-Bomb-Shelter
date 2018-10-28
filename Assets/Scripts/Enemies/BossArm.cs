using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArm : MonoBehaviour 
{
	public bool uncovered;

	public GameObject bulletPrefab;
	public Transform bulletOutput;
	public Transform projectilePool;

	Transform playerTransform;
	Animator anim;
	SpriteRenderer sr;

	float aimingAngle;
	float spriteAngle;
	bool canFlip = true;
	bool facingRight;

	private void Start()
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

		sr = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();

		if (uncovered)
		{
			anim.SetTrigger("Uncover");
			StartCoroutine(Attack());
		}
	}

	private void Update()
	{
		if (canFlip) sr.flipX = facingRight = playerTransform.position.x > transform.position.x;
		aimingAngle = Vector3.Angle(playerTransform.position - (transform.position), Vector3.right);

		spriteAngle = aimingAngle;
		if (aimingAngle > 90) spriteAngle = aimingAngle - 2 * (aimingAngle - 90);
		anim.SetFloat("AimDirection", spriteAngle);
	}

	IEnumerator Attack()
	{
		while (true)
		{
			yield return new WaitForSeconds(3f);
			canFlip = false;
			anim.SetBool("Shooting", true);

			float shootingAngle = aimingAngle;
			anim.SetFloat("ShootDirection", spriteAngle);

			float roundAngle = RoundShootingAngle(shootingAngle);

			bulletOutput.position = new Vector3((facingRight ? 7 : -7) + Mathf.Cos(roundAngle * Mathf.Deg2Rad) * 26,
				Mathf.Sin(roundAngle * Mathf.Deg2Rad) * 26);

			bulletOutput.Rotate(Vector3.forward, roundAngle);

			for (int i = 0; i < 5; i++)
			{
				anim.SetTrigger("Fire");
				Shoot();
				yield return new WaitForSeconds(0.4f);
			}

			bulletOutput.rotation = Quaternion.identity;

			anim.SetBool("Shooting", false);
			canFlip = true;
		}
	}

	void Shoot()
	{
		GameObject projectile = Instantiate(bulletPrefab, projectilePool);
		BulletScript bs = projectile.GetComponent<BulletScript>();
		projectile.transform.position = transform.position + bulletOutput.position;
		bs.velocity = bulletOutput.right;
		projectile.SetActive(true);
	}

	float RoundShootingAngle(float angle)
	{
		if (angle < 22.5) return 0;
		else if (angle < 67.5) return 45;
		else if (angle < 112.5) return 90;
		else if (angle < 157.5) return 135;
		else return 180;
	}
}
