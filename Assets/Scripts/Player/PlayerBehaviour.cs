using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
	public enum playerState
	{
		Grounded,
		Flying,
		Diving,
		Dashing,
		Dying,
	}

	#region properties

	[Header("Movement")] [SerializeField] public Vector2 velocityMax;

	[SerializeField] private Vector2 velocityIncrement;

	[SerializeField] private float walkingSpeed;

	[SerializeField] private float walkingLDrag;

	[SerializeField] private float diveGravity;


	[Header("References")] [SerializeField]
	private BoxCollider2D bulletCollider;

	[NonSerialized] public Rigidbody2D rb;

	private Animator anim;

	private SpriteRenderer sprite;

	private bool facingLeft;

	private float lastFlap;

	private bool dead = false;

	private float defaultGravity;

	private float defaultLDrag;

	private float dyingTime;

	[SerializeField] DashAbility dash;

	[SerializeField] WaterEffect waterEffect;

	[NonSerialized] public bool dying;
	[NonSerialized] public bool diving;

	[SerializeField] public string inputHorizontal = "Horizontal";
	[SerializeField] public string inputVertical = "Vertical";
	[SerializeField] private string inputFlap = "Flap";
	[SerializeField] public string inputDive = "Dive";
	[SerializeField] public string inputDash = "Dash";

	public playerState state;

	#endregion

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		defaultGravity = rb.gravityScale;
		defaultLDrag = rb.drag;
		sprite = GetComponent<SpriteRenderer>();

		dash.Initialize(this);
	}

	bool IsGrounded()
	{
        return Physics2D.Raycast(transform.position + 10 * (Vector3)Vector2.down,
	        Vector2.down, 5f,
	        1 << LayerMask.NameToLayer("Ground")
		);
	}
	
	void HandleInput()
	{
		switch (state)
		{
			case playerState.Flying:
				if (Input.GetButtonDown(inputDive))
					state = playerState.Diving;
				else if (Input.GetButtonDown(inputDash))
					dash.tryDash();
				else if (IsGrounded())
					state = playerState.Grounded;
				break;
			case playerState.Diving:
				if (!Input.GetButton(inputDive))
				{
					if (IsGrounded())
						state = playerState.Grounded;
					else
						state = playerState.Flying;
				}	
				break;
			case playerState.Grounded:
				if (Input.GetButtonDown(inputDash))
					dash.tryDash();
				else if (!IsGrounded())
					state = playerState.Flying;
				break;
			case playerState.Dashing:
				if (dash.timeLeft == 0)
				{
					dash.stop();
					if (IsGrounded())
					{
						state = playerState.Grounded;
					}
					else
					{
						state = playerState.Flying;
					}
				}
				break;
			case playerState.Dying:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	void UpdateMovement()
	{
		switch (state)
		{
			case playerState.Grounded:
				if (Mathf.Abs(Input.GetAxisRaw(inputHorizontal)) > 0.2f)
					rb.velocity += Vector2.right * Input.GetAxisRaw(inputHorizontal) * walkingSpeed;
				else
					rb.velocity = new Vector2(0, rb.velocity.y);
				break;
			case playerState.Flying:
				if (Input.GetButtonDown(inputFlap))
				{
					lastFlap = Time.time;
					rb.velocity += new Vector2(velocityIncrement.x * Input.GetAxis(inputHorizontal),
												(CheckWaterLevel())? -velocityIncrement.y : velocityIncrement.y);
				}
                if (Mathf.Abs(Input.GetAxis(inputHorizontal)) > 0.1f)
					facingLeft = Input.GetAxis(inputHorizontal) > 0;
				break;
			case playerState.Diving:
				break;	
			case playerState.Dashing:
				rb.velocity = dash.direction * dash.velocity;
				break;
			case playerState.Dying:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
	
	float GetRigidbodyDrag()
	{
		return state == playerState.Grounded ? walkingLDrag : defaultLDrag;
	}

    float GetGravity()
    {
	    return state == playerState.Diving ? diveGravity : defaultGravity;
    }

    int GetHorizontalDirection()
    {
	    return facingLeft ? -1 : 1;
    }

    float GetAnimationSpeed()
    {
        var currentState = anim.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsName("Flying"))
        {
            if (Time.time - lastFlap > 0.2 && rb.velocity.y > 0)
                return 0;
            return 1;
        }

	    if (!currentState.IsName("Diving_Ball")) return 1;
	    
	    if (state == playerState.Grounded && Mathf.Abs(rb.velocity.x) < 0.1)
		    return 0;
	    return 1;
    }

	bool CheckWaterLevel()
	{
		if (!waterEffect) return false;

		if (waterEffect.underwater) anim.SetLayerWeight(2, 1);
		else anim.SetLayerWeight(2, 0);

		return waterEffect.underwater;
	}

// Update is called once per frame
	void Update () {
		HandleInput();

		UpdateMovement();

		diving = Input.GetButton(inputDive);

		rb.drag = GetRigidbodyDrag();
		rb.gravityScale = GetGravity();
		//anim.speed = GetAnimationSpeed();

		//TODO figure mirror animation
		sprite.flipX = facingLeft; // (state != playerState.Dashing && facingLeft) || rb.velocity.x < 0;

		anim.SetFloat("velocity_y", rb.velocity.y);
		anim.SetFloat("velocity_x_abs", Mathf.Abs(rb.velocity.x));
		anim.SetBool("FacingLeft", facingLeft); // rb.velocity.x < 0);
		anim.SetBool("Diving", state == playerState.Diving);
		anim.SetBool("Walking", state == playerState.Grounded);
		anim.SetBool("Dashing", state == playerState.Dashing);

		if (Input.GetKeyDown(KeyCode.H))
		{
			if (anim.GetLayerWeight(1) == 0) anim.SetLayerWeight(1, 1);
			else anim.SetLayerWeight(1, 0);
		}

		if (CheckWaterLevel()) rb.gravityScale = -rb.gravityScale;
	}

}
