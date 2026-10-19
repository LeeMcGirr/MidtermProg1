using UnityEngine;
using UnityEngine.InputSystem;

public class JumpScript : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 10f;
    public Rigidbody2D myRB;

    public float jumpForce = 100f;
    public bool isGrounded;

    float timer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        isGrounded = false;
    }


    private void Update()
    {
        timer += Time.deltaTime;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;


        direction = Vector3.zero;
        if(Keyboard.current.aKey.isPressed)
        {
            direction += Vector3.left;
        }
        if(Keyboard.current.dKey.isPressed)
        {
            direction += Vector3.right;
        }

        //one way to move objects is by manually moving the transform
        //this is OK but does NOT interact with the physics engine
        //maybe good for moving background sprites, clouds, vfx, etc?
        //transform.Translate(direction * speed);

        //for player controllers that work with physics, instead set the RB.linearVelocity!
        //first multiply direction by speed
        direction = direction * speed;
        //then we set the y based off whatever existing y is
        direction.y = myRB.linearVelocity.y;
        //we set the linearVelocity = to this new vector
        myRB.linearVelocity = direction; 
        
        //check to see if grounded AND spacekey pressed
        if(Keyboard.current.spaceKey.isPressed && isGrounded)
        {
            //we can also use .AddForce to apply an impulse force to a rigidbody!
            myRB.AddForce(Vector3.up * jumpForce);
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
