using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunMovement : MonoBehaviour
{
    public float f_sway = 0.2f;
    public float f_maxSway = 0.4f;
    public float f_smoothAmount = 6.0f;


    private float f_player_verticalMovement;

    private Vector3 initPos;

    private playerMove player;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.localPosition;
        player = GameObject.Find("Capsule").GetComponent<playerMove>();
    }

    // Update is called once per frame
    void Update()
    {



        // GUN SWAYING
        f_player_verticalMovement = player.f_verticalMovement;

        float moveX = -Input.GetAxis("Mouse X") * f_sway;
        float moveY = -Input.GetAxis("Mouse Y") * f_sway;

        if (f_player_verticalMovement != 0)
        {
            moveY += -Mathf.Sign(f_player_verticalMovement) * f_sway;
        }


        moveX = Mathf.Clamp(moveX, -f_maxSway, f_maxSway);
        moveY = Mathf.Clamp(moveY, -f_maxSway, f_maxSway);

        Vector3 finalPosToMove = new Vector3(moveX, moveY, 0);
        Vector3 velocity = Vector3.zero;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosToMove + initPos, ref velocity, Time.deltaTime * f_smoothAmount);
    }


}
