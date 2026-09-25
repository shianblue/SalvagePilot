using UnityEngine;
using Evo.UI;
public class SimModeManager : MonoBehaviour
{
    [SerializeField]private Slider reversePower;
    [SerializeField] PlayerController player;
    [SerializeField] private GameObject setting;

    void Start()
    {
        setting.SetActive(false);
        reversePower.value = 500f;
        reversePower.onValueChanged.AddListener(player.SetReversePower);
        player.SetReversePower(reversePower.value);
    }

    public void SettingOpen()
    {
        setting.SetActive(true);
    }

    public void SettingClose()
    {
        setting.SetActive(false);
    }
}
