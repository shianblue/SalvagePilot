using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class characterBox : MonoBehaviour
{
    [SerializeField] private float thrustForceUp;//上方向のスロットル
    [SerializeField] private float thrustForcedown;//下方向のスロットル
    [SerializeField] private float thrustForcerevers;//逆噴射のパワー
    [SerializeField] private float thrustForcemove;
    [SerializeField] private float thrustForcerevers_wasd;
    
    private int Jumpstatus = 0;
    private int WASD_status = 0;
    
    private float JumpTime = 0f;//逆噴射のための時間を入れる
    private float Wtime = 0f;
    
    private float y_speed = 0f;
    
    public GameObject stopper;
    
    private int stopper_count = 0;
    
    public Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void move()
    {
        if (WASD_status == 1)
        {
            rb.AddForce(Vector3.forward * thrustForcemove,ForceMode.Force);
            Wtime += Time.deltaTime;
            Debug.Log("前進中");
        }

        if (WASD_status == 2)
        {
            rb.AddForce(Vector3.back * thrustForcerevers_wasd,ForceMode.Force);
            Wtime -= Time.deltaTime;
            Debug.Log("後ろ方向に噴射中");
        }
        if (rb.linearVelocity.z <= 0.0f && !(rb.linearVelocity.z >= 0.0f))
        {
            WASD_status = 0;
            rb.linearVelocity = new Vector3(0, 0, 0);//このままだとすべての速度が止まるので、第一引数と第三引数に変数に格納した速度を入れる...WASDの逆噴射コード作成時に変更予定
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            WASD_status = 1;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            WASD_status = 2;
        }
    }
    
    public void jump()
    {
        //Debug.Log(rb.linearVelocity.y);
        Debug.Log(Jumpstatus);
        if (Jumpstatus == 1)
        {
            rb.AddForce(Vector3.up * thrustForceUp,ForceMode.Force);
            JumpTime += Time.deltaTime;
            Debug.Log("上方向に噴射中");
        }
        if (Jumpstatus == 2)
        {
            rb.AddForce(Vector3.down * thrustForcedown,ForceMode.Force);
            JumpTime -= Time.deltaTime;
            Debug.Log("下方向に噴射中");
        }
        
        if (JumpTime <= 0 && rb.linearVelocity.y <= 0.0f && Jumpstatus != 1)//どうしても0～-1の間になってしまう。低速になったらrb.linearVelocityを0にするか、y+方向に小さい力を加えるかで対策できる可能性.....
        {
            Jumpstatus = 3;
            //Debug.Log("噴射を停止");
            if (rb.linearVelocity.y <= 0.0f && !(rb.linearVelocity.y >= 0.0f))
            {
                rb.linearVelocity = new Vector3(0, 0, 0);//このままだとすべての速度が止まるので、第一引数と第三引数に変数に格納した速度を入れる...WASDの逆噴射コード作成時に変更予定
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (JumpTime <= 0)
            {
                Jumpstatus = 0;
            }
            Jumpstatus = 1;
            //Debug.Log("downspace");
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Jumpstatus = 2;
            //Debug.Log("upspace");
        }
    }
}
