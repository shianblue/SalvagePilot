using UnityEngine;
using Evo.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float thrustForceY; //Y軸のスロットル....UIでのスロットル操作を分けるためにMoveと分割。
    [SerializeField] private float reversedThrustForce; //Y軸逆噴射のパワー
    [SerializeField] private float thrustForceMove;
    [SerializeField] private float reversedThrustForceMove;
    [SerializeField] private float thrustForceTorque;
    [SerializeField] private float reversedThrustForceTorque;

    [SerializeField] private ParticleSystem mainEngine;
    [SerializeField] private ParticleSystem subEngineRight;
    [SerializeField] private ParticleSystem subEngineLeft;
    [SerializeField] private ParticleSystem frontRight;
    [SerializeField] private ParticleSystem frontLeft;
    [SerializeField] private ParticleSystem rearRight;
    [SerializeField] private ParticleSystem rearLeft;

    [SerializeField] private float maxSpeed = 50f;
    
    [SerializeField] private Slider lateralSpeedSlider;
    [SerializeField] private Slider forwardSpeedSlider;
    [SerializeField] private Slider verticalSpeedSlider;
    [SerializeField] private RadialSlider slider;
    
    private Rigidbody rb;
    
    private float lateralSpeed => Vector3.Dot(rb.linearVelocity, transform.right);
    private float forwardSpeed => Vector3.Dot(rb.linearVelocity, transform.forward);
    private float verticalSpeed => rb.linearVelocity.y;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        thrustForceMove = slider.Value;
        thrustForceY = slider.Value;
        Move();
    }

    private void Move()//全体の操作
    {
        float linearBrakeStep = (reversedThrustForceMove / rb.mass) * Time.fixedDeltaTime;
        float verticalBrakeStep = (reversedThrustForce / rb.mass) * Time.fixedDeltaTime;
        float angularBrakeStep = (reversedThrustForceTorque / rb.inertiaTensor.y) * Time.fixedDeltaTime;
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
        else if (Mathf.Abs(forwardDot) > linearBrakeStep)
        {
            rb.AddForce(-transform.forward *(reversedThrustForceMove * Mathf.Sign(forwardDot)), ForceMode.Force);
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
        else if (Mathf.Abs(rlDot) > linearBrakeStep)
        {
            rb.AddForce(transform.right * (reversedThrustForceMove * -Mathf.Sign(rlDot)), ForceMode.Force);
        }
        else
        {
            rb.linearVelocity -= transform.right * rlDot;
        }
        //==========================================
        //               ROTATE_Q/E
        //==========================================
        int qeValue = 0;
        bool isBraking = false;
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
        else if (Mathf.Abs(qeDot) > angularBrakeStep)
        {
            rb.AddTorque(transform.up * (reversedThrustForceTorque * -Mathf.Sign(qeDot)), ForceMode.Force);
            isBraking = true;
        }
        else
        {
            rb.angularVelocity -= transform.up * qeDot;
        }
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
        else if (Mathf.Abs(rb.linearVelocity.y) > verticalBrakeStep)
        {
            rb.AddForce(-transform.up * (reversedThrustForce * Mathf.Sign(rb.linearVelocity.y)), ForceMode.Force);
            //条件と実行内容にlinearVelocityを入れているが、X軸のRotateを追加するなら変更の必要あり
        }
        else
        {
            rb.linearVelocity -= transform.up * rb.linearVelocity.y;
        }
        //==========================================
        //                 other
        //==========================================
        bool isThrusting = wsValue == 1 && thrustForceMove >= 1;
        bool mainActive = isThrusting;
        bool subLeftActive = isThrusting;
        bool subRightActive = isThrusting;
        bool isTurning = qeValue == 1 || qeValue == -1 && thrustForceTorque >= 1;//これを使って旋回時のエフェクトを起動させる方法は下のEnginEffectを参照
        //エフェクトの条件
        
        EngineEffect(mainEngine, mainActive);
        EngineEffect(subEngineRight, subRightActive);
        EngineEffect(subEngineLeft, subLeftActive);
        EngineEffect(frontRight, (isBraking && qeDot > 0) || (isTurning && qeValue < 0));
        EngineEffect(frontLeft, (isBraking && qeDot < 0) || (isTurning && qeValue > 0));
        EngineEffect(rearRight, (isBraking && qeDot < 0) || (isTurning && qeValue > 0));
        EngineEffect(rearLeft, (isBraking && qeDot > 0) || (isTurning && qeValue < 0));
        //エンジンのパーティクルの起動
        
        float clampedForward = Mathf.Clamp(forwardSpeed, -maxSpeed, maxSpeed);
        float clampedLateral = Mathf.Clamp(lateralSpeed, -maxSpeed, maxSpeed);
        float clampedVertical = Mathf.Clamp(verticalSpeed, -maxSpeed, maxSpeed);
        
        rb.linearVelocity = transform.forward * clampedForward + transform.right * clampedLateral + Vector3.up * clampedVertical;
        
        lateralSpeedSlider.value = clampedLateral;
        forwardSpeedSlider.value = clampedForward;
        verticalSpeedSlider.value = clampedVertical;
    }
    void EngineEffect(ParticleSystem ps,bool isActive)//エンジンのパーティクルシステムを起動する
    {
        if (isActive && !ps.isEmitting)
        {
            ps.Play(true);
        }
        else if (!isActive && ps.isEmitting)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    public void  SetReversePower(float value,float valueVer,float valueRot)//SimModeで推力を調整するため
    {
        reversedThrustForceMove = value;
        reversedThrustForce = valueVer;
        reversedThrustForceTorque = valueRot;
        Debug.Log(reversedThrustForceMove);
    }
}