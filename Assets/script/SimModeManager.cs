using UnityEngine;

public class SimModeManager : MonoBehaviour
{
    [SerializeField] private GameObject setting;
    void Start()
    {
        setting.SetActive(false);
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
