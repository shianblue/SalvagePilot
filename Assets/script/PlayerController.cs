using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float thrustForceY; //Y軸のスロットル....UIでのスロットル操作を分けるためにMoveと分割。統一したほうが自然か
    [SerializeField] private float thrustForceRevers; //逆噴射のパワー
    
    [SerializeField] public float thrustForceMove;
    [SerializeField] private float thrustForceReversMove;

    [SerializeField] private float thrustForceTorque;
    [SerializeField] private float thrustForceReversTorque;
    
    public Rigidbody rb;
    
    [SerializeField] private Slider slider;
    

    public float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        thrustForceMove = slider.value;
        Move();
    }

    private void Move()//WASDの操作
    {
        //==========================================
        //                MOVE_W/S
        //==========================================
        int wsValue = 0;
        float forwardDot = Vector3.Dot(rb.linearVelocity, transform.forward);
        if (Input.GetKey(KeyCode.W))
        {
            wsValue += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            wsValue -= 1;
        }

        if (wsValue != 0)
        {
            rb.AddForce(transform.forward * (wsValue * thrustForceMove), ForceMode.Force);
        }
        else if (Mathf.Abs(forwardDot) > 0.1f)
        {
            rb.AddForce(-transform.forward *(thrustForceReversMove * Mathf.Sign(forwardDot)), ForceMode.Force);
        }
        else
        {
            rb.linearVelocity -= transform.forward * forwardDot;//FixedUpdateを用いても速度がちょうど0のときに静止が出来なかったため最終制動のみ直接操作
        }
        //==========================================
        //                MOVE_A/D
        //==========================================
        int adValue = 0;
        float rlDot = Vector3.Dot(rb.linearVelocity, transform.right);
        if (Input.GetKey(KeyCode.A))
        {
            adValue -= 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            adValue += 1;
        }

        if (adValue != 0)
        {
            rb.AddForce(transform.right * (adValue * thrustForceMove), ForceMode.Force);
        }
        else if (Mathf.Abs(rlDot) > 0.1f)
        {
            rb.AddForce(transform.right * (thrustForceReversMove * -Mathf.Sign(rlDot)), ForceMode.Force);
        }
        else
        {
            rb.linearVelocity -= transform.right * rlDot;
        }
        //==========================================
        //               ROTATE_Q/E
        //==========================================
        int qeValue = 0;
        float qeDot = Vector3.Dot(rb.angularVelocity, transform.up);
        if (Input.GetKey(KeyCode.Q))
        {
            qeValue -= 1;
        }

        if (Input.GetKey(KeyCode.E))
        {
            qeValue += 1;
        }

        if (qeValue != 0)
        {
            rb.AddTorque(transform.up * (qeValue * thrustForceTorque), ForceMode.Force);
        }
        else if (Mathf.Abs(qeDot) > 0.1f)
        {
            rb.AddTorque(transform.up * (thrustForceReversTorque * -Mathf.Sign(qeDot)), ForceMode.Force);
        }
        else
        {
            rb.angularVelocity -= transform.up * qeDot;
        }
        //X軸のROTATEを追加するかはステージ作成後決定
        //==========================================
        //              MOVE_SPACE/C
        //==========================================
        int spcValue = 0;
        if (Input.GetKey(KeyCode.Space))
        {
            spcValue += 1;
        }
        if (Input.GetKey(KeyCode.C))
        {
            spcValue -= 1;
        }

        if (spcValue != 0)
        {
            rb.AddForce(transform.up * (spcValue * thrustForceY), ForceMode.Force);
        }
        else if (Mathf.Abs(rb.linearVelocity.y) > 0.1f)
        {
            rb.AddForce(-transform.up * (thrustForceRevers * Mathf.Sign(rb.linearVelocity.y)), ForceMode.Force);
            //条件と実行内容にlinearVelocityを入れているが、X軸のRotateを追加するなら変更の必要あり
        }
        else
        {
            rb.linearVelocity -= transform.up * rb.linearVelocity.y;
        }
    }
}