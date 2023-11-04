using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public CharacterController characterController;

    private Vector3 moveInput;

    public Transform camTransform;

    public Animator anim;

    [Header("Gravity")]
    public float gravityModifier;

    [Header("Movement Controls")]
    public float moveSpeed;
    public float runSpeed;
    public float jumpPower;

    private bool canJump, canDoubleJump;
    public Transform grounCheckPoint;
    public LayerMask whatIsGround;

    [Header("Camera Controls")]
    public float mouseSensivity;
    public bool invertX;
    public bool invertY;

    private float pitch = 0f; // Agrega esto

    public GameObject bullet;
    public Transform firePoint;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        //moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        //guardar Y velocity
        float yStore = moveInput.y;

        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horitMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = horitMove + vertMove;
        moveInput.Normalize();


        if (Input.GetButton("Run"))
        {
            moveInput = moveInput * runSpeed;
        }
        else
        {
            moveInput = moveInput * moveSpeed;
        }


        moveInput.y = yStore;

        //Gravedad
        if (!characterController.isGrounded)
        {
            moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        canJump = characterController.isGrounded;

        if (canJump)
        {
            canDoubleJump = false;
        }


        //Salto del jugador
        if (Input.GetButtonDown("Jump") && canJump)
        {
            moveInput.y = jumpPower;

            canDoubleJump = true;
        }
        else if (canDoubleJump && (Input.GetButtonDown("Jump")))
        {
            moveInput.y = jumpPower;

            canDoubleJump = false;
        }


        characterController.Move(moveInput * Time.deltaTime);


        //Control Rotacion Camara
        Vector2 mauseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensivity;

        if (invertX)
        {
            mauseInput.x = -mauseInput.x;
        }

        // Actualiza 'pitch' basado en el input del mouse, y luego usa eso para establecer la rotación de la cámara
        pitch -= mauseInput.y;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        camTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mauseInput.x, transform.rotation.eulerAngles.z);

        //Shooting
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, 50f))
            {
                if (Vector3.Distance(camTransform.position, hit.point) > 2f)
                {
                    firePoint.LookAt(hit.point);
                }
            }
            else
            {
                firePoint.LookAt(camTransform.position + (camTransform.forward * 30f));
            }


            Instantiate(bullet, firePoint.position, firePoint.rotation);
        }

        anim.SetFloat("moveSpeed", moveInput.magnitude);
        anim.SetBool("onGround", canJump);
    }
}
