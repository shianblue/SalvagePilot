using UnityEngine;
using TMPro;
using Evo.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goalsStatusText;
    [SerializeField] private OffScreenIndicator indicatorPoint1;
    [SerializeField] private OffScreenIndicator indicatorPoint2;
    [SerializeField] private GameObject missionCompletePanel;
    
    [SerializeField] private Notification hintNotification;
    [SerializeField] private Notification phaseNotification;
    
    private enum Goals
    {
        Phase1,
        Phase2,
        Phase3,
        Phase4,
        Phase5
    }
    private Goals _goals;
    private enum Zone
    {
        None,
        Point1,
        Point2
    }
    private Zone _zone;
    void Start()
    {
        SetPhase(Goals.Phase1);
        missionCompletePanel.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_zone == Zone.Point1 && _goals == Goals.Phase2)
            {
                SetPhase(Goals.Phase3);
            }
            if (_zone == Zone.Point2 && _goals == Goals.Phase4)
            {
                SetPhase(Goals.Phase5);
            }
        }
    }

    private void SetPhase(Goals newPhase)
    {
        _goals = newPhase;
        switch (_goals)
        {
            case Goals.Phase1:
                goalsStatusText.text = "目標:マーカーへ向かう";
                break;
            case Goals.Phase2:
                goalsStatusText.text = "目標:破片を回収する";
                break;
            case Goals.Phase3:
                goalsStatusText.text = "目標:受け渡し地点へ向かう";
                break;
            case Goals.Phase4:
                goalsStatusText.text = "目標:破片を降ろす";
                break;
            case Goals.Phase5:
                goalsStatusText.text = "ミッション完了";
                missionCompletePanel.SetActive(true);
                break;
        }
        phaseNotification.Title = "Goal Update";
        phaseNotification.Open();
        indicatorPoint1.enabled = (newPhase == Goals.Phase1 || newPhase == Goals.Phase2);
        indicatorPoint2.enabled = (newPhase == Goals.Phase3 || newPhase == Goals.Phase4);
    }

    public void ChangeToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void ChangeToSimMode()
    {
        SceneManager.LoadScene("SimMode");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            _zone = Zone.Point1;
            hintNotification.Description = "Press [F] to pick up";
            hintNotification.Open();
            if (_goals == Goals.Phase1)
            {
                SetPhase(Goals.Phase2);
            }
        }
        else if (other.CompareTag("Target2"))
        {
            _zone = Zone.Point2;
            hintNotification.Description = "Press [F] to drop off";
            hintNotification.Open();
            if (_goals == Goals.Phase3)
            {
                SetPhase(Goals.Phase4);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Target") || other.CompareTag("Target2"))
        {
            _zone = Zone.None;
            hintNotification.Close();
        }
    }
}
