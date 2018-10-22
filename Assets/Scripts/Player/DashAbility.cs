using System;
using UnityEngine;
public class DashAbility : MonoBehaviour
{

	[SerializeField]
	float duration = 0.1f;

	[SerializeField]
	public float velocity = 700f;

	[SerializeField]
	public int basePoints = 3;

	[SerializeField]
	public float baseCooldown = 4f;

	[SerializeField]
	float momentumConservation = 0.2f;

	[SerializeField]
	float momentumConservationDiving = 0.8f;

	[HideInInspector]
	public int currentPoints;
	[HideInInspector]
	public float currentCooldown;
	[HideInInspector]
	public float timeLeft;

	[HideInInspector]
	public Vector2 direction;

	PlayerBehaviour player;

	public void Initialize(PlayerBehaviour player)
	{
		this.player = player;
		currentPoints = basePoints;
		//FindObjectOfType<DashPoints>().Init();
	}

	public bool tryDash()
	{
		if (currentPoints > 0)
		{
			direction = new Vector2(Input.GetAxisRaw(player.inputHorizontal),
										Input.GetAxisRaw(player.inputVertical));
			if (player.state == PlayerBehaviour.playerState.Grounded &&
				Vector2.Angle(Vector2.up, direction) > 70)
				return false;

			direction.Normalize();

			/*
			--currentPoints;
			if (currentPoints != 0)
				currentCooldown = baseCooldown;
			else
				currentCooldown = 3 * baseCooldown;
			*/
			timeLeft = duration;

			player.state = PlayerBehaviour.playerState.Dashing;

		}
		return player.state == PlayerBehaviour.playerState.Dashing;
	}
	public void Update()
	{
		if (currentCooldown > 0)
		{
			currentCooldown = Mathf.Max(0, currentCooldown - Time.deltaTime);
			if (currentCooldown <= 0)
			{
				if (currentPoints == 0)
				{
					currentPoints = basePoints;
				}
				else
				{
					currentPoints = Math.Min(currentPoints + 1, basePoints);
					if (currentPoints < basePoints)
						currentCooldown = baseCooldown;
				}
			}
		}

		if (timeLeft > 0)
		{
			timeLeft = Mathf.Max(0, timeLeft - Time.deltaTime);
		}
	}

	public void stop()
	{
		timeLeft = 0;
		float conservation = Input.GetButton(player.inputDive) ?
			momentumConservationDiving : momentumConservation;

		player.rb.velocity = direction * (player.velocityMax.magnitude * conservation);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{

		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && timeLeft > 0)
		{
			stop();
		}
	}

}
