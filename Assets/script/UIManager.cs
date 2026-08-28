using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    private Delivery.DeliveryStatus _deliveryStatus;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI goalsStatusText;

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
        statusText.text = $"Status: {_deliveryStatus}";
        switch (_goals)
        {
            case Goals.Phase1:
                goalsStatusText.text = "ドローンを追う";//プレイヤーの誘導をUI依存にするのかドローンを出してtailで追わせるのか決めてないのでどっちでも
                break;
            case Goals.Phase2:
                goalsStatusText.text = "デブリを探す";//これもどうなるかわからないの変更があれば変わる
                break;
            case Goals.Phase3:
                goalsStatusText.text = "デブリを回収する(Fボタン)";
                break;
            case Goals.Phase4:
                goalsStatusText.text = "回収地点へ向かう";
                break;
            case Goals.Phase5:
                goalsStatusText.text = "デブリを降ろす(Fボタン)";
                break;
        }
    }
}
