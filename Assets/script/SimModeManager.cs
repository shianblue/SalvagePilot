using UnityEngine;
using Evo.UI;
using TMPro;

public class SimModeManager : MonoBehaviour
{
    [SerializeField]private Slider reversePower;
    [SerializeField] private Slider reversePowerVer;
    [SerializeField] private Slider reversePowerRot;
    [SerializeField] PlayerController player;
    [SerializeField] private GameObject setting;

    void Start()
    {
        setting.SetActive(false);
        player.SetReversePower(reversePower.value, reversePowerVer.value, reversePowerRot.value);
        
        foreach (Slider slider in new[] {reversePower, reversePowerVer, reversePowerRot})
        {
            slider.value = 500f;
            slider.onValueChanged.AddListener(_ => ApplyReverPower());
        }
    }

    private void ApplyReverPower()
    {
        player.SetReversePower(reversePower.value, reversePowerVer.value, reversePowerRot.value);
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
