using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerV1 : MonoBehaviour
{

    Rigidbody2D rb;
    float xIn;
    float yIn;
    Vector2 moveForce;
    bool canJump = true;
    bool feetTouchingGround = false;
    float mSpeed;

    public float maxSpeed = 5f;
    public float moveSpeed = 20f;
    public float jumpForce = 500f;

    public float originalTime = 2.0f;
    public float timeChange = 1.0f;
    public float timeDuration = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        Time.timeScale = originalTime;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        MovementController();
    }

    private void MovementController()
    {
        if(mSpeed > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void GetInput()
    {
        mSpeed = rb.velocity.magnitude;

        xIn = Input.GetAxis("Horizontal");
        yIn = Input.GetAxis("Vertical");
        moveForce = new Vector2(xIn, 0f) * moveSpeed;

        if (Input.GetKeyDown("space") && canJump)
        {
            StartCoroutine(JumpRoutine());
            Jump();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            StartCoroutine(SetTimeScale());
        }
    }
    
    IEnumerator JumpRoutine()
    {
        float count = 0;

        while (Input.GetKey("space") && count < 10)
        {
            rb.AddForce(new Vector2(0.0f, jumpForce * 0.1f));
            //Debug.Log("adding force");
            yield return new WaitForSecondsRealtime(0.1f);
            ++count;
        }

        
    }

    IEnumerator SetTimeScale()
    {
        Time.timeScale = timeChange;
        yield return new WaitForSeconds(timeDuration);
        Time.timeScale = originalTime;
    }

    private void Move()
    {
        //transform.Translate(moveForce);
        if (!(rb.velocity.magnitude > maxSpeed))
            rb.AddForce(moveForce);
    }

    private void Jump()
    {
        rb.AddForce(new Vector2(0.0f, jumpForce));
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Stable"))
        {
            canJump = true;
            feetTouchingGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Stable"))
        {
            canJump = false;
            feetTouchingGround = false;
                
        }
    }
}