using UnityEngine;
using TMPro;
using Evo.UI;
public class UIManager : MonoBehaviour
{
    private Delivery.DeliveryStatus _deliveryStatus;
    public TextMeshProUGUI goalsStatusText;
    [SerializeField] private Notification hintNotification;

    private enum Goals
    {
        Phase1,
        Phase2,
        Phase3,
        Phase4,
        Phase5
    }

    private Goals _goals;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (_goals)
        {
            case Goals.Phase1:
                goalsStatusText.text = "目標:ドローンを追う";//プレイヤーの誘導をUI依存にするのかドローンを出してtailで追わせるのか決めてないのでどっちでも
                break;
            case Goals.Phase2:
                goalsStatusText.text = "目標:デブリを探す";//これもどうなるかわからないの変更があれば変わる
                break;
            case Goals.Phase3:
                goalsStatusText.text = "目標:デブリを回収する(Fボタン)";
                break;
            case Goals.Phase4:
                goalsStatusText.text = "目標:回収地点へ向かう";
                break;
            case Goals.Phase5:
                goalsStatusText.text = "目標:デブリを降ろす(Fボタン)";
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            hintNotification.Open();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            hintNotification.Close();
        }
    }
}
