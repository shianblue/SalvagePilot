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

    public enum JumpStatus
    {
        JumpNormal, //0
        JumpAccelerate, //1
        JumpReverse, //2
        JumpStop, //3
    }

    public enum MoveStatus
    {
        MoveNormal, //0
        MoveForward, //1
        MoveForwardReverse, //2
        MoveStop, //3
        MoveLeft, //4
        MoveLeftReverse, //5
        MoveRight, //6
        MoveRightReverse, //7
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void move()
    {
        if (Movestatus == MoveStatus.MoveForward)  // forward
        {
            rb.AddForce(Vector3.forward * thrustForcemove, ForceMode.Force);
            Wtime += Time.deltaTime;
            Debug.Log("前進中");
        }

        if (Movestatus == MoveStatus.MoveForwardReverse)  // back
        {
            rb.AddForce(Vector3.back * thrustForcerevers_wasd, ForceMode.Force);
            Wtime -= Time.deltaTime;
            Debug.Log("後ろ方向に噴射中");
        }

        if (rb.linearVelocity.z <= 0.0f && !(rb.linearVelocity.z >= 0.0f))  // stop
        {
            Movestatus = MoveStatus.MoveStop;
            rb.linearVelocity = new Vector3(0, 0, 0);  // このままだとすべての速度が止まるので、第一引数と第三引数に変数に格納した速度を入れる...WASDの逆噴射コード作成時に変更予定
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Movestatus = MoveStatus.MoveForward;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            Movestatus = MoveStatus.MoveForwardReverse;
        }

        //===================================================
        //                    MOVE_D
        //===================================================
        if (Movestatus == MoveStatus.MoveRight)
        {
            rb.AddForce(Vector3.right * thrustForcemove, ForceMode.Force);  // Right
            Dtime += Time.deltaTime;
            Debug.Log("右に噴射中");
        }

        if (Movestatus == MoveStatus.MoveRightReverse)
        {
            rb.AddForce(Vector3.left * thrustForcerevers_wasd, ForceMode.Force);  // Right_Reverse
            Dtime -= Time.deltaTime;
            Debug.Log("左に噴射中");
        }

        if (rb.linearVelocity.x <= 0.0f && !(rb.linearVelocity.x >= 0.0f)) // stop
        {
            Movestatus = MoveStatus.MoveStop;
            rb.linearVelocity = new Vector3(0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Movestatus = MoveStatus.MoveRight;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            Movestatus = MoveStatus.MoveRightReverse;
        }

        // ==================================================
        //                     MOVE_A    AとDは設計を考え直す必要アリ
        // ==================================================
        if (Movestatus == MoveStatus.MoveLeft)
        {
            rb.AddForce(Vector3.left * thrustForcemove, ForceMode.Force);
            Debug.Log("左に噴射中");
        }

        if (Movestatus == MoveStatus.MoveLeftReverse)
        {
            rb.AddForce(Vector3.right * thrustForcerevers_wasd, ForceMode.Force);
            Debug.Log("右に噴射中");
        }

        if (rb.linearVelocity.x >= 0.0f && !(rb.linearVelocity.x <= 0.0f) && !(Dtime >= 0)) 
        {
            Movestatus = MoveStatus.MoveStop;
            rb.linearVelocity = new Vector3(0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Movestatus = MoveStatus.MoveLeft;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            Movestatus = MoveStatus.MoveLeftReverse;
        }
    }

    public void jump()
    {
        //Debug.Log(rb.linearVelocity.y);
        Debug.Log(Jumpstatus);
        if (Jumpstatus == JumpStatus.JumpAccelerate)
        {
            rb.AddForce(Vector3.up * thrustForceUp, ForceMode.Force);
            JumpTime += Time.deltaTime;
            Debug.Log("上方向に噴射中");
        }

        if (Jumpstatus == JumpStatus.JumpReverse)
        {
            rb.AddForce(Vector3.down * thrustForcedown, ForceMode.Force);
            JumpTime -= Time.deltaTime;
            Debug.Log("下方向に噴射中");
        }

        if (JumpTime <= 0 && rb.linearVelocity.y <= 0.0f &&
            Jumpstatus !=
            JumpStatus.JumpAccelerate) //どうしても0～-1の間になってしまう。低速になったらrb.linearVelocityを0にするか、y+方向に小さい力を加えるかで対策できる可能性.....
        {
            Jumpstatus = JumpStatus.JumpStop;
            //Debug.Log("噴射を停止");
            if (rb.linearVelocity.y <= 0.0f && !(rb.linearVelocity.y >= 0.0f))
            {
                rb.linearVelocity =
                    new Vector3(0, 0, 0); //このままだとすべての速度が止まるので、第一引数と第三引数に変数に格納した速度を入れる...WASDの逆噴射コード作成時に変更予定
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (JumpTime <= 0)
            {
                Jumpstatus = JumpStatus.JumpNormal;
            }

            Jumpstatus = JumpStatus.JumpAccelerate;
            //Debug.Log("downspace");
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Jumpstatus = JumpStatus.JumpReverse;
            //Debug.Log("upspace");
        }
    }
}