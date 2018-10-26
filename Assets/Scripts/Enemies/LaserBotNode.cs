using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBotNode : MovingEntity
{
	[SerializeField] private LaserBot parent;

	private Rigidbody2D rb;

	public ParticleSystem circleStatic;

	[SerializeField] private CircleCollider2D trigger;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();	
	}

	private void Update()
	{
		if (transform.position.y < GameController.instance.waterline)
		{
			Instantiate(parent.explosion, transform.position, Quaternion.identity);
			parent.StopAllCoroutines();
			Destroy(gameObject);
		}
	}

	public void MoveToTarget(Vector3 target)
	{
		if (!canMove()) return;

		if ((transform.position - target).magnitude < 1) return;

		Vector3 direction = target - transform.position;
		//Debug.DrawRay(transform.position, direction, Color.cyan);
		transform.position += direction.normalized * parent.speed;
	}

	public override void divedOnto(Collision2D collision)
	{
		rb.AddForce(-collision.relativeVelocity * parent.knockback, ForceMode2D.Impulse);
	}

	public void CycleAttack()
	{
		trigger.enabled = !trigger.enabled;
	}

	
}
