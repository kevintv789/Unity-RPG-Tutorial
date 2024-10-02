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
	public LayerMask interactableLayer;

	[SerializeField] private float m_CollisionRadius = 0.2f;

	private static readonly int s_MoveX = Animator.StringToHash("moveX");
	private static readonly int s_MoveY = Animator.StringToHash("moveY");
	private static readonly int s_IsMoving = Animator.StringToHash("isMoving");

	private void Awake()
	{
		animator = GetComponent<Animator>();
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	public void HandleUpdate()
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

		if (Input.GetKeyDown(KeyCode.E))
		{
			Interact();
		}
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
		bool isCollidingWithSolidObject = Physics2D.OverlapCircle(targetPos, m_CollisionRadius, solidObjectsLayer);
		bool isCollidingWithInteractable = Physics2D.OverlapCircle(targetPos, m_CollisionRadius, interactableLayer);

		return !isCollidingWithSolidObject && !isCollidingWithInteractable;
	}

	// Can only interact with NPCs if the player is facing them
	private void Interact()
	{
		// If the character is facing up, then this returns (0, 1, 0)
		var facingDirection = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
		var interactPosition = transform.position + facingDirection;

		// The drawline is only visible in the scene view
		// Debug.DrawLine(transform.position, interactPosition, Color.red, 2f);

		var collider = Physics2D.OverlapCircle(interactPosition, m_CollisionRadius, interactableLayer);
		if (collider)
		{
			collider.GetComponent<Interactable>()?.Interact();
		}
	}
}
