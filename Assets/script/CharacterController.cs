using UnityEngine;
public class CharacterController : MonoBehaviour
{
    public float wsValue;
    public float adValue;
    public float spcValue;
    [SerializeField] private float thrustForceUp; //上方向のスロットル
    [SerializeField] private float thrustForceDown; //下方向のスロットル
    [SerializeField] private float thrustForceRevers; //逆噴射のパワー
    
    [SerializeField] private float thrustForceMove;
    [SerializeField] private float thrustForceReversMove;

    Rigidbody rb;

    public float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            time += Time.deltaTime;
            if (time > 0.5)
            {
                wsValue += 1f;
                time = 0;
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            time += Time.deltaTime;
            if (time > 0.5)
            {
                wsValue -= 1f;
                time = 0;
            }
        }

        if (wsValue != 0f)
        {
            rb.AddForce(transform.forward * wsValue * adValue, ForceMode.Force);
            
        }
        else if (Mathf.Abs(rb.linearVelocity.z) > 0.1f)
        {
            rb.AddForce(transform.forward + rb.linearVelocity * thrustForceMove, ForceMode.Force);
        }
    }
}
