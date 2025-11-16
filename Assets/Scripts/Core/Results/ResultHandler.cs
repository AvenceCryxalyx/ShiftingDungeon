using System.Collections.Generic;
using UnityEngine;

public class ResultHandler : MonoBehaviour
{
    public struct SalesReport
    {
        public int amount;
        public Sprite Icon;
    }

    public struct LoanReductionReport
    {
        public int LoanRemainingBefore;
        public int LoanRemainingAfter;
        public int DaysRemaining;
    }


    [SerializeField]
    private ResultSalesPanel salesPanel;
    [SerializeField]
    private ResultLoanDetailsPanel loanPanel;

    private LoanReductionReport LoanReport;
    private Inventory playerInventory;
    private List<SalesReport> sales = new List<SalesReport>();

    private void Awake()
    {
        playerInventory = Player.instance.GetComponentInChildren<Inventory>();

        if (playerInventory == null)
        {
            return;
        }

        LoanReport = new LoanReductionReport();
        LoanReport.LoanRemainingBefore = LoanManager.instance.AmountRemaining;
        LoanReport.DaysRemaining = LoanManager.instance.DaysRemaining;
        LoanReport.LoanRemainingAfter = LoanManager.instance.AmountRemaining;

        foreach(Item item in playerInventory.Items)
        {
            SalesReport sale = new SalesReport();
            sale.amount = Mathf.CeilToInt(item.Value.Current);
            sale.Icon = item.Icon;
            sales.Add(sale);
        }
        FinalizeResults();
    }

    public void AddSalesReport(SalesReport report)
    {
        sales.Add(report);
    }

    public void FinalizeResults()
    {
        PayoffLoan();
        LoanReport.LoanRemainingAfter = LoanManager.instance.AmountRemaining;
        playerInventory.ClearInventory();
    }

    public void PayoffLoan()
    {
        foreach (SalesReport sale in sales)
        {
            LoanManager.instance.PayOffLoan(sale.amount);
            salesPanel.AddItemSoldImage(sale.Icon);
        }
        sales.Clear();
    }

    public void ProceedToNext()
    {
        Debug.Log("Trigger");
        AppInstance.LoadGameMode("DungeonScene");
    }

    public void ReturnToMain()
    {
        Debug.Log("Trigger");
        AppInstance.LoadFirstMode();
    }
}
