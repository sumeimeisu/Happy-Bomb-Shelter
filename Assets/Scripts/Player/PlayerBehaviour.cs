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

	[Header("Movement")] [SerializeField] private Vector2 velocityMax;

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

	[NonSerialized] public bool dying;
	[NonSerialized] public bool diving;

	[SerializeField] private string inputHorizontal = "Horizontal";
	[SerializeField] private string inputVertical = "Vertical";
	[SerializeField] private string inputFlap = "Flap";
	[SerializeField] private string inputDive = "Dive";
	[SerializeField] private string inputDash = "Dash";

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
				if (!IsGrounded())
					state = playerState.Flying;
				break;
			case playerState.Dashing:
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
												velocityIncrement.y);
				}
                if (Mathf.Abs(Input.GetAxis(inputHorizontal)) > 0.1f)
                    facingLeft = Input.GetAxis(inputHorizontal) < 0;
				break;
			case playerState.Diving:
			case playerState.Dashing:
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
            if (Time.time - last_flap > 0.2 && rb.velocity.y > 0)
                return 0;
            return 1;
        }

	    if (!currentState.IsName("Diving_Ball")) return 1;
	    
	    if (state == playerState.Grounded && Mathf.Abs(rb.velocity.x) < 0.1)
		    return 0;
	    return 1;
    }

// Update is called once per frame
	void Update () {
		HandleInput();

		UpdateMovement();

		diving = Input.GetButton(inputDive);

		rb.drag = GetRigidbodyDrag();
		rb.gravityScale = GetGravity();
		anim.speed = GetAnimationSpeed();

		//TODO figure mirror animation
		sprite.flipX = (state != playerState.Dashing && facingLeft) || rb.velocity.x < 0;

		anim.SetFloat("velocity_y", rb.velocity.y);
		anim.SetFloat("velocity_x_abs", Mathf.Abs(rb.velocity.x));
		anim.SetBool("GoingLeft", rb.velocity.x < 0);
		anim.SetBool("Diving", state == playerState.Diving);
		anim.SetBool("Walking", state == playerState.Grounded);
		anim.SetBool("Dashing", state == playerState.Dashing);	
	}
}
