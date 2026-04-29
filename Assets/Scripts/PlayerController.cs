using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsMoving { get; private set; }
    public bool CanRun;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Joystick joystick;

    private Animator animator;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        Vector3 moveDirection = GetMoveDirection();

        if (moveDirection.magnitude > 0.1f)
        {
            if (CanRun)
            {
                IsMoving = true;
                animator.SetBool("Move", true);
                MoveCharacter(moveDirection);
                RotateCharacter(moveDirection);
            }
        }
        else
        {
            IsMoving = false;
            animator.SetBool("Move", false);
        }
    }

    private Vector3 GetMoveDirection()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;
        return new Vector3(horizontal, 0, vertical).normalized;
    }

    private void MoveCharacter(Vector3 moveDirection)
    {
        Vector3 movement = moveDirection * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    public void RotateCharacter(Vector3 moveDirection)
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = newRotation;
        }
    }

    public void PlayAnimation(string animationName)
    {
        animator.SetBool(animationName, true);
    }

    public void StopAnimation(string animationName)
    {
        animator.SetBool(animationName, false);
    }
}