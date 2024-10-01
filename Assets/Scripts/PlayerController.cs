using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 2f;
	private bool isMoving;
	private Vector2 input;
	private Animator animator;
	private Rigidbody2D rigidbody2D;

	private static readonly int s_MoveX = Animator.StringToHash("moveX");
	private static readonly int s_MoveY = Animator.StringToHash("moveY");
	private static readonly int s_IsMoving = Animator.StringToHash("isMoving");

	private void Awake()
	{
		animator = GetComponent<Animator>();
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		// Get input from the player
		input.x = Input.GetAxisRaw("Horizontal");
		input.y = Input.GetAxisRaw("Vertical");

		// Prevent diagonal movement
		if (input.x != 0)
		{
			input.y = 0;
		}

		if (input != Vector2.zero)
		{
			animator.SetFloat(s_MoveX, input.x);
			animator.SetFloat(s_MoveY, input.y);
			isMoving = true;
		}
		else
		{
			isMoving = false;
		}

		animator.SetBool(s_IsMoving, isMoving);

	}

	private void FixedUpdate()
	{
		rigidbody2D.MovePosition(rigidbody2D.position + input * moveSpeed * Time.fixedDeltaTime);
	}
}
