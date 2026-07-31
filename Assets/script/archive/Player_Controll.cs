using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

/*public class Player_Controll : MonoBehaviour//inputsystemの残骸
{
    
    private float speed = 10f;
    private InputAction moveAction;
    private InputAction jumpAction;
    public Rigidbody rb;
    private int Jumptatus = 0;
    private float JumpTime = 0f;
    public float thrustForceUp;
    public float thrustForcedown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputAction
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {

        // 移動処理
        var moveValue = moveAction.ReadValue<Vector2>();
        var move = new Vector3(moveValue.x, 0f, moveValue.y * speed * Time.deltaTime);
        transform.Translate(move);
        if (Jumptatus == 1)
        {
            rb.AddForce(Vector3.up * thrustForceUp,ForceMode.Force);
            JumpTime += Time.deltaTime;
        }
        else if (Jumptatus == 2)
        {
            rb.AddForce(Vector3.down * thrustForcedown,ForceMode.Force);
            JumpTime -= Time.deltaTime;
        }
        else if (JumpTime >= 0)
        {
            Jumptatus = 3;
        }
    }

    private void OnJump(InputAction.CallbackContext context)//未完成、Debugを吐かないのでそもそも動作していない
    {
        if (context.started)
        {
            Jumptatus = 1;
            Debug.Log(Jumptatus);
        }

        if (context.canceled)
        {
            Jumptatus = 2;
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Jumptatus = 1;
            Debug.Log(Jumptatus);
        }

        if (context.canceled)
        {
            Jumptatus = 2;
        }
    }
}
*/
