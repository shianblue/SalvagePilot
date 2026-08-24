using System;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    public enum DeliveryStatus
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


    private enum ZoneType
    {
        None,
        Pickup,
        Delivery
    }

    private DeliveryStatus _status;

    private PortStatus _portStatus;

    private ZoneType _currentZone = ZoneType.None; // 今どちらのゾーンにいるか

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_portStatus == PortStatus.Ready && Input.GetKeyDown(KeyCode.F))
        {
            if (_currentZone == ZoneType.Pickup && _status != DeliveryStatus.Carrying)//積み込み
            {
                _status = DeliveryStatus.Carrying;
            }
            else if (_currentZone == ZoneType.Delivery && _status == DeliveryStatus.Carrying)//荷下ろし
            {
                _status = DeliveryStatus.Delivered;
            }

            Debug.Log(_status);
        }
    }

    private void OnTriggerEnter(Collider other)
    {//PickupZone / DeliveryZoneは仮のタグ、後で本番タグに置き換える
        if (other.CompareTag("PickupZone"))
        {
            _portStatus = PortStatus.Ready;
            _currentZone = ZoneType.Pickup;
        }
        else if (other.CompareTag("DeliveryZone"))
        {
            _portStatus = PortStatus.Ready;
            _currentZone = ZoneType.Delivery;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickupZone") || other.CompareTag("DeliveryZone"))
        {
            _portStatus = PortStatus.Unready;
            _currentZone = ZoneType.None;
        }
    }
}
    
