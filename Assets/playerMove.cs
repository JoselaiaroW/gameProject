using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    private string str_state;

    public float f_SPEED = 8f;
    public float f_ROTATIONSPEED = 2f;
    private float xDir = 0f;
    private float zDir = 0f;

    public float JUMPHEIGHT = 0.2f;
    public float MAXJUMPHEIGHT = 0.2f;
    public float GRAVITY = 0.3f;
    public float MAXVERTICALSPEED = 3f;

    public bool canJump = true;
    public bool canDash = true;

    public Vector3 v3_horizontalMovement;
    public float f_verticalMovement;
    public Vector3 v3_Movement;

    private float f_DASH_MULTIPLIER = 4f;
    private float f_DASH_TIME = 0.20f;
    private float f_DASH_COOLDOWN = 3.0f;
    private float f_xDashDir;
    private float f_zDashDir;
    private bool jumpKey;
    private bool dashKey;
    private float fDashMultiplier = 1f;
    private float fDashTime = 0f;
    private float fDashCooldown = 0f;

    public float F_GROUNDRAYCAST = 1.08f;

    private CharacterController playerController;

    GameObject Camera;


    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<CharacterController>();
        Camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        jumpKey = Input.GetKey(KeyCode.Space);
        dashKey = Input.GetKeyDown(KeyCode.LeftShift);

        //////////////////////////// MOVER PERSONAJE

        xDir = 0;
        zDir = 0;

        xDir = Input.GetAxis("Horizontal");
        zDir = Input.GetAxis("Vertical");

        ManageDash();
        

        float xVelocity = xDir * f_SPEED * fDashMultiplier;
        float zVelocity = zDir * f_SPEED * fDashMultiplier;


        float cameraRotX = Input.GetAxis("Mouse X");
        float cameraRotY = Input.GetAxis("Mouse Y");

        v3_horizontalMovement = new Vector3(xVelocity, v3_horizontalMovement.y, zVelocity);
        Vector3.Normalize(v3_horizontalMovement);

        //////////////////////////// ROTAR CÁMARA
        Vector3 rotY = transform.rotation.eulerAngles + new Vector3(0, cameraRotX * f_ROTATIONSPEED, 0f); // ROTA EL PERSONAJE DE FORMA HORIZONTAL
        Vector3 rotX = Camera.transform.rotation.eulerAngles + new Vector3(-cameraRotY * f_ROTATIONSPEED, 0f, 0f); // ROTA LA CÁMARA DE FORMA VERTICAL

        rotX.x = ClampAngle(rotX.x, -60f, 60f);

        Camera.transform.eulerAngles = rotX;
        transform.eulerAngles = rotY;

        //////////////////////////// SALTAR

        if (isOnGround()) //SI NO ESTÁ EN EL SUELO = GRAVEDAD;
        {
            if (f_verticalMovement <= 0)
            {
            f_verticalMovement = 0;
                if (canJump)
                {
                    if (jumpKey)
                    {
                        canJump = false;
                        f_verticalMovement = JUMPHEIGHT;
                    }
                }
                else
                {
                    canJump = true;
                }
            }
        }
        else
        {  
            f_verticalMovement -= GRAVITY * Time.deltaTime;
        }

        f_verticalMovement = Mathf.Clamp(f_verticalMovement, -MAXVERTICALSPEED, MAXJUMPHEIGHT);

        ManageWallJump();

        v3_horizontalMovement = transform.TransformVector(v3_horizontalMovement) * Time.deltaTime; // CONVIERTE EL VECTOR A GLOBAL SINCRONIZA EL VECTOR DE MOVIMIENTO HORIZONTAL CON EL TIEMPO
        v3_Movement = new Vector3(v3_horizontalMovement.x, f_verticalMovement, v3_horizontalMovement.z); // CREA EL VECTOR FINAL DE MOVIMIENTO CON LOS PARÁMETROS DE v3_horizontalMovement y f_verticalMovement
        playerController.Move(v3_Movement); // CC MUEVE EL JUGADOR
    }


    float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }

    float valueSign(float value)
    {
        if (value != 0)
            return Mathf.Sign(value);
        else
            return Mathf.Sign(value);
    }

    private bool isOnGround()
    {
        LayerMask ground = LayerMask.GetMask("Ground");
        Debug.DrawRay(transform.position, Vector3.down * (playerController.height/2 + 0.1f), Color.yellow);

        if (Physics.Raycast(transform.position, Vector3.down, (playerController.height/2) + 0.1f, ground))
        {
            return true;
        }
        return false;
    }


    private bool isOnWall()
    {
        LayerMask ground = LayerMask.GetMask("Ground");
        Debug.DrawRay(transform.position, Vector3.down * (playerController.height / 2 + 0.1f), Color.yellow);

        Vector3 v3_playerDir = new Vector3(valueSign(xDir), 0f, 0f);

        bool wallDetection = Physics.Raycast(transform.position, v3_playerDir, (playerController.height / 3) + 0.1f, ground);
            Debug.DrawRay(transform.position, v3_playerDir * (playerController.height / 3 + 0.1f), Color.yellow);
        
        return wallDetection;
    }


    private void ManageDash()
    {
        if (dashKey)
        {
            if (canDash)
            {
                f_xDashDir = valueSign(xDir);
                f_zDashDir = valueSign(zDir);

                canDash = false;
                str_state = "DASH";
                fDashMultiplier = f_DASH_MULTIPLIER;
                fDashTime = f_DASH_TIME;
            }
        }

        if (fDashTime > 0)
        {
            fDashTime -= Time.deltaTime;
            xDir = f_xDashDir;
            zDir = f_zDashDir;
        }
        else if (fDashTime < 0)
        {
            fDashTime = 0;
            fDashMultiplier = 1f;
            canDash = true;
            str_state = "MOVE";
        }
    }

    private void ManageWallJump()
    {
        if (!isOnGround())

            if (isOnWall())
            {
                if (jumpKey)
                {
                    //v3_horizontalMovement = new Vector3(70f,0f,0f);
                }
            }
    }

}


