using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float WalkSpeed = 6f;
    public float RunSpeed = 15f;
    public float spriteHeight;

    private Animator _Animator;

    private readonly Vector3 FACE_RIGHT = new Vector3(1,1,1);
    private readonly Vector3 FACE_LEFT = new Vector3(-1, 1, 1);

    private bool facingRight = true;
    private bool running = false;
	// Use this for initialization
	void Start () {
        _Animator = GetComponent<Animator>();
	}
	
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");

        running = Input.GetButton("Jump");

        float speed = running ? RunSpeed : WalkSpeed;

        transform.Translate(Vector3.right * xInput * speed * Time.deltaTime);

        _Animator.SetFloat("Speed", Mathf.Abs(xInput * speed * Time.deltaTime));
        _Animator.SetBool("Running", running);

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
        //use raycasts to keep the player grounded
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up*0.3f, -transform.up, out hit ,10f))
        {
            transform.up = hit.normal;
            transform.position = hit.point + transform.up*spriteHeight;
        }
    }
}
