using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class CamaraController : MonoBehaviour
{
    public float moveSpeed;
    public float minXRot;
    public float maxXRot;
    private float curXRot;
    public float minZoom;
    public float maxZoom;
    public float zoomSpeed;
    public float rotateSpeed;
    private float curZoom;
    // reference to the camera object
    private Camera cam;

    private float mouseX;

    private bool rotating;

    private Vector2 moveDirection;

    private void Start()
    {
        cam=Camera.main;
        curZoom = cam.transform.localPosition.y;
        curXRot = -50;
    }
    public void OnZoom(InputAction.CallbackContext context)
    {
        curZoom += context.ReadValue<Vector2>().y * zoomSpeed;

        curZoom=+Mathf.Clamp(curZoom, minZoom, maxZoom);
        
    }

    public void OnRotateTogle(InputAction.CallbackContext context)
    {
        rotating=context.ReadValueAsButton();
        if (!rotating)
            mouseX = 0;
    }
    public void OnRoTate(InputAction.CallbackContext context)
    {
        if (rotating)
        {
            mouseX=context.ReadValue<Vector2>().x;
            float mouseY=context.ReadValue<Vector2>().y;
            curXRot += -mouseY * rotateSpeed;
            curXRot=Mathf.Clamp(curXRot,minXRot,maxXRot);
            transform.eulerAngles = new Vector3(curXRot, transform.eulerAngles.y + (mouseX * rotateSpeed), 0.0f);
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
         moveDirection = context.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        cam.transform.localPosition = Vector3.up * curZoom;

        transform.eulerAngles=new Vector3(curXRot,transform.eulerAngles.y+(mouseX*rotateSpeed), 0.0f);  

        Vector3 forward= cam.transform.forward;
        forward.y = 0.0f;
       

        Vector3 right= cam.transform.right;
        Vector3 dir=forward*moveDirection.y+right*moveDirection.x;
       
        dir*=moveSpeed*Time.deltaTime;
        transform.position += dir;
      
    }





}
