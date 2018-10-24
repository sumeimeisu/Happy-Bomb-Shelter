using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonBot : MovingEntity
{

	[Header("References")]
	#region References
	[SerializeField]
	private Transform DroneProjectilesPool;
	[SerializeField]
	private Transform DroneGunOutputTransforms;
	private Animator DroneAnimator;
	[SerializeField]
	private CircleCollider2D[] balloons = new CircleCollider2D[3];

	private Transform PlayerTransform;
	#endregion

	[Header("Properties")]
	#region Properties
	private int health = 2;
	[SerializeField]
	private float Speed;
	[SerializeField]
	private float distanceFromPlayer;
	[SerializeField]
	[Tooltip("Idle time between shots")]
	private float FireRate;

	private bool facingLeft;
	#endregion

	[Header("Prefabs")]
	#region Prefabs
	[SerializeField]
	private GameObject GrenadePrefab;
	#endregion

	void Start()
	{
		PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		DroneAnimator = GetComponent<Animator>();

		StartCoroutine(ShootLoop());
	}

	void Update()
	{
		if (!canMove())
			return;

		if (!PlayerTransform) return;

		MoveAbovePlayer();

		facingLeft = PlayerTransform.position.x < transform.position.x;
	}

	public void TakeDamage()
	{
		health--;
		if (health > 0)
		{
			DroneAnimator.SetLayerWeight(1, 1);
			balloons[0].enabled = balloons[1].enabled = false;
			balloons[2].enabled = true;
		}
		else Destroy(gameObject);
	}

	// may have to change to rigidbody movement...
	void MoveAbovePlayer()
	{
		Vector3 TargetPosition;
		if (facingLeft)
		{
			transform.localScale = new Vector3(-1, 1, 1);
			TargetPosition = PlayerTransform.position + Vector3.right * distanceFromPlayer;
		}
		else
		{
			transform.localScale = new Vector3(1, 1, 1);
			TargetPosition = PlayerTransform.position - Vector3.right * distanceFromPlayer;
		}

		Vector3 Direction = (TargetPosition - transform.position);
		// Do not move if close enough to the player
		if (Direction.magnitude < 5.0f)
		{
			return;
		}
		if (Direction.magnitude > 200.0f)
		{
			return;
		}
		Vector3 Velocity = Direction.normalized;
		transform.position += Velocity * Time.deltaTime * Speed;
	}

	override public void divedOnto(Collision2D collision)
	{
		TakeDamage();
	}

	IEnumerator ShootLoop()
	{
		while (true)
		{
			yield return new WaitForSeconds(FireRate);
			DroneAnimator.SetTrigger("Shoot");
			Shoot();
		}
	}

	void Shoot()
	{
		GameObject Projectile = Instantiate(GrenadePrefab, DroneProjectilesPool);
		GrenadeScript gs = Projectile.GetComponent<GrenadeScript>();
		gs.projectilePool = DroneProjectilesPool;
		Projectile.transform.position = DroneGunOutputTransforms.position;
		gs.velocity = facingLeft ? -DroneGunOutputTransforms.right :  DroneGunOutputTransforms.right;
		Projectile.SetActive(true);
	}
}
