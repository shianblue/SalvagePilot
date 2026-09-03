using UnityEngine;

public class SimModeManager : MonoBehaviour
{
    [SerializeField] private GameObject setting;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
