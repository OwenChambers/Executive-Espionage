using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float MOVESPEED;
    float CurrentMoveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float RotateSpeed = 100;
    [SerializeField] float GraviyMultiplyer = 2;

    [Header("Player Items")]
    [SerializeField] List<GameObject> guns;
    [SerializeField] List<GameObject> gadgets;



    Rigidbody rb;
    Transform groundCheck;

    Vector3 moveDirection;
    public Vector3 TargetDirection { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = transform.Find("GroundCheck");
        CurrentMoveSpeed = MOVESPEED;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        CameraRelMove();
        rb.velocity = TargetDirection;
    }

    private void CameraRelMove()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;
        Vector3 forwardRelativeInput = moveDirection.y * forward;
        Vector3 rightRelativeInput = moveDirection.x * right;
        Vector3 cameraRelative = forwardRelativeInput + rightRelativeInput;
        TargetDirection = new Vector3(cameraRelative.x * CurrentMoveSpeed, rb.velocity.y - GraviyMultiplyer, cameraRelative.z * CurrentMoveSpeed);
        //if (rb.velocity.y < 0)
        //{
        //    TargetDirection.y -= GraviyMultiplyer;
        //}
        //if (IsGrounded())
        //{
        //    TargetDirection.y = -5;
        //}
        Rotate(forward);
    }

    private void Rotate(Vector3 forward)
    {
        Quaternion toRotate = Quaternion.LookRotation(forward, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, RotateSpeed).normalized;
    }

    #region Actions
    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        moveDirection.Normalize();
        moveDirection *= MOVESPEED;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpSpeed);
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            guns[0].GetComponent<GunController>().Shoot();
        }
    }

    public void Gadget(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            gadgets[0].GetComponent<GadgetController>().Activate();
        }
    }

    #endregion

    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.1f, LayerMask.GetMask("Ground"));
    }
}
