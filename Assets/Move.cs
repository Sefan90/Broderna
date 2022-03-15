using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHieght = 10;
    public float mouseSensitivity = 7f;
    float xRotation = 0f;
    Camera MainCamera;
    CharacterController controller;
    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        MainCamera = Camera.main;
        controller = gameObject.GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        MainCamera.transform.localEulerAngles = Vector3.right * xRotation;
        transform.Rotate(Vector3.up * mouseX);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward *z;
        controller.Move(move * speed * Time.deltaTime);

        isGrounded = Physics.CheckSphere(transform.position,10f);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHieght * -2f * gravity);
        }

        velocity.y = gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
