using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonBot : MovingEntity
{
	public float knockback;

	[Header("References")]
	#region References
	[SerializeField]
	private Transform DroneProjectilesPool;
	[SerializeField]
	private Transform DroneGunOutputTransforms;
	private Animator DroneAnimator;
	[SerializeField]
	private BoxCollider2D collider1;
	[SerializeField]
	private CircleCollider2D collider2;
	[SerializeField]
	private AudioClip balloonPop;

	private Transform PlayerTransform;
	private Rigidbody2D rb;
	private AudioSource audioS;
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
	[SerializeField]
	private float deathFallingRate;
	[SerializeField]
	private float hoverAboveWaterLevel;
	private bool facingLeft;
	#endregion

	[Header("Prefabs")]
	#region Prefabs
	[SerializeField]
	private GameObject GrenadePrefab;
	[SerializeField]
	private GameObject DeathExplosion;
	#endregion

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		DroneAnimator = GetComponent<Animator>();
		audioS = GetComponent<AudioSource>();

		//StartCoroutine(ShootLoop());
	}

	void Update()
	{
		if (!canMove()) return;

		if (!PlayerTransform) 
		{
			if (GameObject.FindWithTag("Player")) PlayerTransform = GameObject.FindWithTag("Player").transform;
			else return;
		}

		if (health > 0) MoveAbovePlayer();
		//else transform.position -= Vector3.up * deathFallingRate;

		facingLeft = PlayerTransform.position.x < transform.position.x;

		//Debug.Log("BBot: " + GameController.instance.waterline);

		if (transform.position.y < GameController.instance.waterline)
		{
			Instantiate(DeathExplosion, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}

	public void TakeDamage()
	{
		audioS.clip = balloonPop;
		audioS.Play();
		health--;
		if (health > 0)
		{
			DroneAnimator.SetLayerWeight(1, 1);
			StartCoroutine(WaitForColliderSwitch());
		}
		else Death();
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

		Vector3 Direction = new Vector3();

		if (PlayerTransform.position.y < GameController.instance.waterline + hoverAboveWaterLevel)
			TargetPosition = new Vector3(PlayerTransform.position.x, GameController.instance.waterline + hoverAboveWaterLevel);


		Direction = (TargetPosition - transform.position);
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

		// Don't move y if player is underwater
		transform.position += Velocity * Time.deltaTime * Speed;
	}

	void Death()
	{
		health = 0;
		StopAllCoroutines();
		DroneAnimator.SetTrigger("Death");
		collider2.enabled = collider1.enabled = GetComponent<BoxCollider2D>().enabled = false;
		rb.velocity = Vector2.zero;
		StartCoroutine(DeathFalling());
	}

	// extra precaution to avoid two hits on subsequent frames
	IEnumerator WaitForColliderSwitch()
	{
		yield return new WaitForSeconds(0.1f);
		collider1.enabled = false;
		collider2.enabled = true;
	}

	IEnumerator ShootLoop()
	{
		while (true)
		{
			yield return new WaitForSeconds(FireRate + Random.Range(-1.5f, 1.5f));
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

	IEnumerator DeathFalling()
	{
		// follow animation curve ? 
		float t = 0;
		while (true)
		{
			transform.position += Vector3.down * deathFallingRate;
			t += Time.deltaTime;
			yield return null;
		}
	}

	override public void divedOnto(Collision2D collision)
	{
		rb.AddForce(-collision.relativeVelocity * knockback, ForceMode2D.Impulse);
		TakeDamage();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Water"))	Death();
	}
}
