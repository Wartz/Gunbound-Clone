using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float WalkSpeed = 6f;
    public float RunSpeed = 15f;
    public float SpriteHeightOffset = 1.6f;

    private Animator _Animator;

    private readonly Vector3 FACE_RIGHT = new Vector3(1,1,1);
    private readonly Vector3 FACE_LEFT = new Vector3(-1, 1, 1);

    private bool facingRight = true;
    private bool running = false;

    private float xInput;
    private float MoveSpeed;

	void Start () {
        _Animator = GetComponent<Animator>();
	}
	
    void Update()
    {
        GetInput();
        MoveCharacter();
        CheckBounds();
        UpdateAnimator();
        CheckFacingDirection();
        AlignToGround();
    }

    private void GetInput()
    {
        xInput = Input.GetAxis("Horizontal");
        running = Input.GetButton("Jump");
        MoveSpeed = running ? RunSpeed : WalkSpeed;
    }

    private void MoveCharacter()
    {
        transform.Translate(Vector3.right * xInput * MoveSpeed * Time.deltaTime);
    }

    private void CheckBounds()
    {
        //TODO: doesn't account for aspect ratio
        if (transform.position.x < 7f) transform.position = new Vector3(7f, transform.position.y, transform.position.z);
        if (transform.position.x > 93f) transform.position = new Vector3(93f, transform.position.y, transform.position.z);
    }

    private void UpdateAnimator()
    {
        _Animator.SetFloat("Speed", Mathf.Abs(xInput * MoveSpeed * Time.deltaTime));
        _Animator.SetBool("Running", running);
    }

    private void AlignToGround()
    {
        /* NOTE: The raycast is sent one unit behind the sprite (transform.forward) in order for it to hit the mesh collider of the 2D terrain.
         *       When we set the position of the sprite, we adjust the hit.point value by this offset once again in order to keep the sprite
         *       at the same z level.
         */

        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.3f + transform.forward, -transform.up, out hit, 100f))
        {
            transform.up = hit.normal;
            transform.position = hit.point + transform.up * SpriteHeightOffset - transform.forward;
        }
    }

    private void CheckFacingDirection()
    {
        if (facingRight && xInput < 0)
        {
            transform.localScale = FACE_LEFT;
            facingRight = false;
        }
        else if (!facingRight && xInput > 0)
        {
            transform.localScale = FACE_RIGHT;
            facingRight = true;
        }
    }
}
