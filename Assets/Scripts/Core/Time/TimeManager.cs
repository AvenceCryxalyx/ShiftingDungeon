using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Timer
{
    public Action EvtTimerUp;
    public Action<int> EvtTicked;

    public int TimeLeft
    {
        get
        {
            return Mathf.FloorToInt(triggerTime - _timeElapsed);
        }
    }

    private bool isActive = false;
    private float triggerTime;
    private float _timeElapsed;
    public Timer(float triggerTime)
    {
        this.triggerTime = triggerTime;
        TimeManager.instance.RegisterTimer(this);
    }

    ~Timer()
    {
        TimeManager.instance.RemoveTimer(this);
    }

    public void Start()
    {
        isActive = true;
    }

    private void OnTimeUp()
    {
        if (EvtTimerUp != null)
        {
            EvtTimerUp.Invoke();
        }
        isActive = false;
    }

    public void Stop()
    {
        isActive = false;
        Reset();
    }

    public void Reset()
    {
        _timeElapsed = 0;
    }

    public void Restart()
    {
        Reset();
        Start();
    }
    public void Tick(float timeTicked)
    {
        if (!isActive)
            return;
        _timeElapsed += timeTicked;
        if(EvtTicked != null)
        {
            EvtTicked.Invoke(TimeLeft);
        }
        if (_timeElapsed > triggerTime)
        {
            Debug.Log(_timeElapsed);
            OnTimeUp();
        }
    }
}

public class TimeManager : SimpleSingleton<TimeManager>
{
    public Action<int> OnDayChanged;

    public int Days { get; private set; }
    public float TimeElapsed { get; private set; }
    private List<Timer> registeredTimers = new List<Timer>();

    private void Start()
    {
        TimeElapsed = 0;
        Days = 0;
    }

    public void ProgressDay()
    {
        Days++;
        if(OnDayChanged != null)
        {
            OnDayChanged.Invoke(Days);
        }
    }

    public void RegisterTimer(Timer timer)
    {
        if(!registeredTimers.Contains(timer))
        {
            registeredTimers.Add(timer);
        }
    }

    public void RemoveTimer(Timer timer)
    {
        if (registeredTimers.Contains(timer))
        {
            registeredTimers.Remove(timer);
        }
    }

    private void Update()
    {
        TimeElapsed += Time.deltaTime;
        foreach(Timer timer in registeredTimers)
        {
            timer.Tick(Time.deltaTime);
        }
    }

}
