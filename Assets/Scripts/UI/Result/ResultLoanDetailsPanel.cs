using TMPro;
using UnityEngine;

public class ResultLoanDetailsPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI daysLeft;
    [SerializeField]
    private TextMeshProUGUI amountRemaining;
    [SerializeField]
    private TextMeshProUGUI amountSold;

    private void Update()
    {
        daysLeft.text = string.Format("Days Left: {0}", LoanManager.instance.DaysRemaining);
        amountRemaining.text = LoanManager.instance.AmountRemaining.ToString();
        amountSold.text = LoanManager.instance.AmountPaidCurrently.ToString();
    }
}
