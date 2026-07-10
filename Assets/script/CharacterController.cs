using UnityEngine;
public class CharacterController : MonoBehaviour
{
    private float adValue;
    private float spcValue;
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
    void Update()
    {
        Move();
        //Debug.Log(wsValue);
    }

    public void Move()
    {
        int wsValue = 0;
        float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);

        if (Input.GetKey(KeyCode.W))
        {
            wsValue += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            wsValue -= 1;
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

        if (wsValue != 0f)
        {
            rb.AddForce(transform.forward * (wsValue * thrustForceMove), ForceMode.Force);
        }
        else if (Mathf.Abs(forwardSpeed) > 0.1f)
        {
            rb.AddForce(-transform.forward *(thrustForceReversMove * Mathf.Sign(forwardSpeed)), ForceMode.Force);
        }
        else
        {
            rb.linearVelocity = new Vector3(0,0,0);
        }
    }
}
