using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class characterBox : MonoBehaviour
{
    [SerializeField] private float thrustForceUp; //上方向のスロットル
    [SerializeField] private float thrustForcedown; //下方向のスロットル
    [SerializeField] private float thrustForcerevers; //逆噴射のパワー
    
    [SerializeField] private float thrustForcemove;
    [SerializeField] private float thrustForcerevers_wasd;

    private JumpStatus Jumpstatus = JumpStatus.JumpNormal;
    private MoveStatus Movestatus = MoveStatus.MoveNormal;

    private int WASD_status = 0;

    private float JumpTime = 0f; //逆噴射のための時間を入れる
    private float Wtime = 0f;
    private float Dtime = 0f;

    private float y_speed = 0f;
    private float x_speed = 0f;
    private float z_speed = 0f;

    public Rigidbody rb;

    private enum JumpStatus
    {
        JumpNormal, //0
        JumpAccelerate, //1
        JumpUpReverse, //2
        JumpStop, //3
        JumpDownAccelerate,
        JumpDownReverse,
        
    }

    private enum MoveStatus
    {
        MoveNormal, //0
        MoveForward, //1
        MoveForwardReverse, //2
        MoveStop, //3
        MoveLeft, //4
        MoveLeftReverse, //5
        MoveRight, //6
        MoveRightReverse, //7
        MoveBack, //8
        MoveBackReverse, //9
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void move()
    {
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            Movestatus = MoveStatus.MoveForward;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            Movestatus = MoveStatus.MoveForwardReverse;
        }
        
        if (Movestatus == MoveStatus.MoveForward)  // forward
        {
            rb.AddForce(transform.forward * thrustForcemove, ForceMode.Force);  // 試験的にtransform.forwardに変更
            Wtime += Time.deltaTime;
            Debug.Log("前進中");
        }

        if (Movestatus == MoveStatus.MoveForwardReverse)  // back
        {
            rb.AddForce(-transform.forward * thrustForcerevers_wasd, ForceMode.Force);
            Wtime -= Time.deltaTime;
            Debug.Log("後ろ方向に噴射中");
            if (rb.linearVelocity.z <= 0.0f)  // stop
            {
                Movestatus = MoveStatus.MoveStop;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, 0);  // このままだと前進中に一瞬sを押すとz軸の速度が0になるので調整予定(A,DキーSPACE,Cキーも同様)
            }
        }

        //===================================================
        //                    MOVE_S
        //===================================================

        if (Input.GetKeyDown(KeyCode.S))
        {
            Movestatus = MoveStatus.MoveBack;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            Movestatus = MoveStatus.MoveBackReverse;
        }

        if (Movestatus == MoveStatus.MoveBack)
        {
            rb.AddForce(-transform.forward * thrustForcemove, ForceMode.Force);
        }

        if (Movestatus == MoveStatus.MoveBackReverse)
        {
            rb.AddForce(transform.forward * thrustForcerevers, ForceMode.Force);
            if (rb.linearVelocity.z >= 0.0f)
            {
                Movestatus = MoveStatus.MoveStop;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, 0);
            }
        }
        
        //===================================================
        //                    MOVE_D
        //===================================================
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            Movestatus = MoveStatus.MoveRight;
        }
        if (Input.GetKeyUp(KeyCode.D))
        { 
            Movestatus = MoveStatus.MoveRightReverse;
        }
        
        if (Movestatus == MoveStatus.MoveRight)
        {
            rb.AddForce(transform.right * thrustForcemove, ForceMode.Force);  // Right
            Debug.Log("右に噴射中");
        }

        if (Movestatus == MoveStatus.MoveRightReverse)
        {
            rb.AddForce(-transform.right * thrustForcerevers_wasd, ForceMode.Force);  // Right_Reverse
            Debug.Log("左に噴射中");
            
            if (rb.linearVelocity.x <= 0.0f)
            {
                Movestatus = MoveStatus.MoveStop;
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, rb.linearVelocity.z);
            }
        }

        // ==================================================
        //                     MOVE_A    
        // ==================================================
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            Movestatus = MoveStatus.MoveLeft;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Movestatus = MoveStatus.MoveLeftReverse;
        }
        
        if (Movestatus == MoveStatus.MoveLeft)
        {
            rb.AddForce(-transform.right * thrustForcemove, ForceMode.Force);
            Debug.Log("左に噴射中");
        }

        if (Movestatus == MoveStatus.MoveLeftReverse)
        {
            rb.AddForce(transform.right * thrustForcerevers_wasd, ForceMode.Force);
            Debug.Log("右に噴射中");
            
            if (rb.linearVelocity.x >= 0.0f)
            {
                Movestatus = MoveStatus.MoveStop;
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, rb.linearVelocity.z);
            }
        }
    }

    public void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jumpstatus = JumpStatus.JumpAccelerate;
            //Debug.Log("downspace");
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Jumpstatus = JumpStatus.JumpUpReverse;
            Debug.Log("upspace");
        }
        //Debug.Log(rb.linearVelocity.y);
        Debug.Log(Jumpstatus);
        if (Jumpstatus == JumpStatus.JumpAccelerate)
        {
            rb.AddForce(Vector3.up * thrustForceUp, ForceMode.Force);
            Debug.Log("上方向に噴射中");
        }

        else if (Jumpstatus == JumpStatus.JumpUpReverse)
        {
            rb.AddForce(Vector3.down * thrustForcerevers, ForceMode.Force);
            Debug.Log("下方向に噴射中");
            if (rb.linearVelocity.y <= 0.0f)
            {
                Jumpstatus = JumpStatus.JumpStop;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            }
        }
        // ==================================================
        //                     MOVE_DOWN
        // ==================================================
        if (Input.GetKeyDown(KeyCode.C))
        {
            Jumpstatus = JumpStatus.JumpDownAccelerate;
        }

        if (Input.GetKeyUp(KeyCode.C) && Jumpstatus == JumpStatus.JumpDownAccelerate)
        {
            Jumpstatus = JumpStatus.JumpDownReverse;
        }

        if (Jumpstatus == JumpStatus.JumpDownAccelerate)
        {
            rb.AddForce(Vector3.down * thrustForcedown, ForceMode.Force);
        }

        else if (Jumpstatus == JumpStatus.JumpDownReverse)
        {
            rb.AddForce(Vector3.up * thrustForcerevers, ForceMode.Force);
            if (rb.linearVelocity.y >= 0.0f)
            {
                Jumpstatus = JumpStatus.JumpStop;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            }
        }
    }   
}
