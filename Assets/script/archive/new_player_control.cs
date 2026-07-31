using System;
using Unity.VisualScripting;
using UnityEngine;

public class new_player_control : MonoBehaviour
{
    [SerializeField] private float thrustForceUp;//上方向のスロットル
    [SerializeField] private float thrustForcedown;//下方向のスロットル
    [SerializeField] private float thrustForcerevers;//逆噴射のパワー
    private int Jumptatus = 0;
    private float JumpTime = 0f;//逆噴射のための時間を入れる
    public Rigidbody rb;
    characterBox characterbox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterbox = GetComponent<characterBox>();
    }

    // Update is called once per frame
    void Update()
    {
        characterbox.move();
        characterbox.jump();
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("pressW");
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            Debug.Log("releaseW");
        }
    }   
    
}
