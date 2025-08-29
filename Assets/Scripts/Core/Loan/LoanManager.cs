using UnityEngine;
using System.Collections.Generic;
using System;

public class LoanManager : SimpleSingleton<LoanManager>
{
    [Serializable]
    public struct LoanBreakdown
    {
        public int AmountToBePaid;
        public int DaysToPay;
    }

    public List<LoanBreakdown> loanDetails = new List<LoanBreakdown>();

    public LoanBreakdown CurrentLoan { get; private set; }

    public int AmountPaidCurrently { get; private set; }
    public int DaysRemaining { get; private set; }
    public int AmountRemaining { get { return CurrentLoan.AmountToBePaid - AmountPaidCurrently; } }

    private int currentIndex = 0;

    private void Start()
    {
        CurrentLoan = loanDetails[currentIndex];
    }

    public void PayOffLoan(int amount)
    {
        AmountPaidCurrently += amount;
        if(IsCurrentPaid())
        {
            OnCurrentPaidInFull();
        }
    }

    public bool IsCurrentPaid()
    {
        return (AmountRemaining <= 0);
    }

    public bool IsFullyPaid()
    {
        return (currentIndex == loanDetails.Count) && IsCurrentPaid();
    }

    public void OnDayProgressed()
    {
        DaysRemaining--;
    }

    public void OnCurrentPaidInFull()
    {
        currentIndex++;
        AmountPaidCurrently = Mathf.Abs(AmountPaidCurrently);
        if(currentIndex < loanDetails.Count)
        {
            CurrentLoan = loanDetails[currentIndex];
            DaysRemaining += CurrentLoan.DaysToPay;
        }
    }
}
