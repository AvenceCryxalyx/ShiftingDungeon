using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

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

    public bool IsActive { get; private set; }
    private float triggerTime;
    private float _timeElapsed;
    private bool isRegistered;

    public Timer(float triggerTime)
    {
        this.triggerTime = triggerTime;
    }

    ~Timer()
    {
        if(isRegistered)
        {
            Stop();
        }
    }

    public void Start()
    {
        if (TimeManager.instance && !isRegistered)
        {
            TimeManager.instance.RegisterTimer(this);
            isRegistered = true;
        }
        Reset();
        IsActive = true;
    }

    private void OnTimeUp()
    {
        if (EvtTimerUp != null)
        {
            EvtTimerUp.Invoke();
        }
        Stop();
    }

    public void Pause()
    {
        IsActive = false;
    }

    public void Resume()
    {
        IsActive = true;
    }

    public void Stop()
    {
        if(!isRegistered || !IsActive)
        {
            return;
        }
        TimeManager.instance.RemoveTimer(this);
        isRegistered = false;
        IsActive = false;
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
        if (!IsActive || !isRegistered)
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
        foreach(Timer timer in registeredTimers.Where(x => x.IsActive))
        {
            timer.Tick(Time.deltaTime);
        }
    }

}
