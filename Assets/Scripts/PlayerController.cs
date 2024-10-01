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
	public LayerMask solidObjectsLayer;

	[SerializeField] private float m_CollisionRadius = 0.2f;

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
		Vector2 targetPos = rigidbody2D.position + input.normalized * moveSpeed * Time.fixedDeltaTime;

		if (IsWalkable(targetPos))
		{
			rigidbody2D.MovePosition(targetPos);
		}
	}

	private bool IsWalkable(Vector3 targetPos)
	{
		// If there is overlapping, then we don't want to move
		return !Physics2D.OverlapCircle(targetPos, m_CollisionRadius, solidObjectsLayer);
	}
}
