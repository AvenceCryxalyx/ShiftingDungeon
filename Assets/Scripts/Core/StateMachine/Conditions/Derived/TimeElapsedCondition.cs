using UnityEngine;

public class TimeElapsedCondition : Condition
{
    private Timer timer;
    private bool isTimeUp = false;

    public override void Initialize(ConditionSO so)
    {
        base.Initialize(so);
        TimeElapsedConditionSO con = so as TimeElapsedConditionSO;
        timer = new Timer(con.WaitSeconds);
        timer.EvtTimerUp += OnTimeUp;
    }

    public override void Activate(StateController unit)
    {
        isTimeUp = false;
        timer.Start();
    }

    public override void Deactivate(StateController unit)
    {
        isTimeUp = false;
        timer.Stop();
    }

    protected override bool IsMet(StateController unit)
    {
        return isTimeUp;
    }

    private void OnTimeUp()
    {
        isTimeUp = true;
    }

    ~TimeElapsedCondition()
    {
        timer.EvtTimerUp -= OnTimeUp;
        timer = null;
    }
}
