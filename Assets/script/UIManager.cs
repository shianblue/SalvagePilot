using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    private Delivery.DeliveryStatus _deliveryStatus;
    public TextMeshProUGUI statusText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        statusText.text = $"Status: {_deliveryStatus}";
    }
}
