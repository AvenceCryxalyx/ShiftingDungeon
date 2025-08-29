using TMPro;
using UnityEngine;

public class ShftingTimeDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeDisplay;

    private string message;

    private void Start()
    {
        DungeonMode mode = GameMode.Active as DungeonMode;

        mode.ShiftTimer.EvtTicked += UpdateTime;
        mode.ShiftTimer.EvtTimerUp += OnTimeUp;
    }

    private void UpdateTime(int timeLeft)
    {
        if(timeLeft > 0)
        {
            message = string.Format("Time til shift: {0}", timeLeft.ToString());
        }
    }

    private void OnTimeUp()
    {
        message = "Shifting in progress";
    }

    private void Update()
    {
        timeDisplay.text = message;
    }
}
