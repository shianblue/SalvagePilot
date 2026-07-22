using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float thrustForceUp; //上方向のスロットル
    [SerializeField] private float thrustForceDown; //下方向のスロットル
    [SerializeField] private float thrustForceRevers; //逆噴射のパワー
    
    [SerializeField] private float thrustForceMove;
    [SerializeField] private float thrustForceReversMove;
    
    public Rigidbody rb;

    public float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        //Debug.Log(wsValue);
    }

    public void Move()
    {
        int wsValue = 0;
        int adValue = 0;
        int spcValue = 0;
        //==========================================
        //                 　W/S
        //==========================================
        float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
        float RLSpeed = Vector3.Dot(rb.linearVelocity, transform.right);
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
        else if (Mathf.Abs(forwardSpeed) > 0.1f)
        {
            rb.AddForce(-transform.forward *(thrustForceReversMove * Mathf.Sign(forwardSpeed)), ForceMode.Force);
        }
        else
        {
            rb.linearVelocity -= transform.forward * forwardSpeed;
            //rb.linearVelocity = new Vector3(0,0,0);
        }
        //==========================================
        //                 　A/D
        //==========================================
        if (Input.GetKey(KeyCode.A))
        {
            adValue = 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            adValue = -1;
        }

        if (adValue != 0)
        {
            rb.AddForce(-transform.right * (adValue * thrustForceMove), ForceMode.Force);
        }
        else if (Mathf.Abs(RLSpeed) > 0.1f)
        {
            rb.AddForce(transform.right * (thrustForceReversMove * -Mathf.Sign(RLSpeed)), ForceMode.Force);
        }
        else
        {
            //rb.linearVelocity = new Vector3(0,0,0);
            rb.linearVelocity -= transform.right * RLSpeed;
        }
    }
}
/*if (Input.GetKey(KeyCode.W))
        {
            time += Time.deltaTime;
            if (time > 0.2)
            {
                wsValue += 1f;
                time = 0;
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            time += Time.deltaTime;
            if (time > 0.2)
            {
                wsValue -= 1f;
                time = 0;
            }
        }*/