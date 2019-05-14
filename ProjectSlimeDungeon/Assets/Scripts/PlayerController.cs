using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 10;
    public float jumpForce;
    public int extraJumpsValue;
    private int extraJumps;
    private CharacterController controler;

    [Header("Physics")]
    public float gravityScale;
    private Vector3 moveDirection;

    void Start()
    {
        extraJumps = extraJumpsValue;
        controler = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        float yStore = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
        moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection.y = yStore;
        if (controler.isGrounded)
        {
            moveDirection.y = 0f;
        }
        if (Input.GetButtonDown("Jump") && extraJumps > 0)
        {
            moveDirection.y = jumpForce;
            extraJumps--;
        }
        else if (Input.GetButtonDown("Jump") && extraJumps == 0 && controler.isGrounded)
        {
            moveDirection.y = jumpForce;
        }
        if (controler.isGrounded)
        {
            extraJumps = extraJumpsValue;
        }
        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        controler.Move(moveDirection * Time.deltaTime);
    }
}
