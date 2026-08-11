using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Delivery : MonoBehaviour
{
    private enum DeliveryStatus
    {
        WaitingForPickup,
        Carrying,
        Delivered
    }

    private enum PortStatus
    {
        Ready,
        Unready
    }


    private DeliveryStatus _status;

    private PortStatus _portStatus;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_portStatus == PortStatus.Ready && Input.GetKeyDown(KeyCode.F))//荷物配達時
        {
            _status = DeliveryStatus.Delivered;//StatusをUIで出す
            Debug.Log("配達完了");
        }
    }

    private void OnTriggerEnter(Collider other)
    {//portをまだ作ってないのでTagは空欄
        if (other.CompareTag(""))
        {
            _portStatus = PortStatus.Ready;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(""))
        {
            _portStatus = PortStatus.Unready;
        }
    }
}
    
