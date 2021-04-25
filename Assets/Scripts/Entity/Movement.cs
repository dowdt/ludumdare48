using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Camera Cam;


    public Transform look;


    public Transform orientation;


    private CharacterController Controller;


    private float xRotation, yRotation;


    Vector3 velocity;

    [SerializeField]
    float Speed = 14f;
    [SerializeField]
    float SprintSpeedModifier = 1.4f;
    [SerializeField]
    float AirSpeedModifier = 0.5f;
    [SerializeField]
    float OnGroundTime = 0.5f;
    [SerializeField]
    float gravity = -9.8f*2f;
    [SerializeField]
    float jumpHeight = 1f;

    float normalFOV = 90;


    void Awake()
    {
        normalFOV = Cam.fieldOfView;
        Controller = GetComponent<CharacterController>();
    }


    float x, y;




    public void setPos(Vector3 pos)
    {
        Controller.enabled = false;
        Controller.transform.position = pos;
        look.position = orientation.position;
        Controller.enabled = true;

    }

    Vector3 move = Vector3.zero;

    bool canJump() {
        return (canJumpTimer > 0f);
    }

    public float GetVerticalMove() {
        return VerticalMove;

    }

    float VerticalMove = 0f;
    float HorizontalMove = 0f;
    private void Move()
    {



        VerticalMove = Input.GetAxisRaw("Vertical");
        HorizontalMove = Input.GetAxisRaw("Horizontal");

      

        float sp = Speed;

        bool sprint = (Input.GetKey(KeyCode.LeftShift) && VerticalMove > 0);
        if (sprint)
        {
            VerticalMove *= SprintSpeedModifier;

        }
        Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView,(!sprint ? normalFOV : normalFOV*1.05f),Time.deltaTime*10f);


        move = Vector3.Lerp(move, (transform.right * HorizontalMove + transform.forward * VerticalMove), Time.deltaTime * (onGround ? 15 : 15 * AirSpeedModifier));


        if (onGround)
        {

            velocity.y = -2f;
           
            Controller.slopeLimit = 50f;
            Controller.stepOffset = 0.3f;
        }
        else
        {
            Controller.slopeLimit = 0f;
            Controller.stepOffset = 0f;
        }
        if (onGround)
            canJumpTimer = OnGroundTime;

        if (canJump() && Input.GetKeyDown(KeyCode.Space))
        {
            canJumpTimer = 0;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        velocity.y += gravity * Time.deltaTime;

        Controller.Move(move * sp * Time.deltaTime + (velocity * Time.deltaTime));

        canJumpTimer -= Time.deltaTime;

        onGround = Controller.isGrounded;


    }

    bool onGround;
    float canJumpTimer = 0;

    public Quaternion getPlayerRot()
    {
        return Quaternion.Euler(xRotation, yRotation, 0f);
    }

    public void setRot(Quaternion rot)
    {
        xRotation = rot.eulerAngles.x;

    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * 5f;
        float mouseY = Input.GetAxis("Mouse Y") * 5f;

  


        yRotation += mouseX;


        xRotation -= mouseY;



        xRotation = Mathf.Clamp(xRotation, -89f, 89f);

        transform.eulerAngles = new Vector3(0, yRotation, 0);

    }




    private void Update()
    {

        Look();
        Move();

        look.rotation = Quaternion.Lerp(look.rotation, Quaternion.Euler(xRotation, yRotation, 0), Time.deltaTime * 30);
        look.position = Vector3.Lerp(look.position, orientation.position, Time.deltaTime * 30f);

    }

}
