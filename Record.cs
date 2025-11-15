using System.ComponentModel;

namespace Debt_Collector;

public class Record : INotifyPropertyChanged
{
    private decimal _debtAmount;
    private decimal _balance;

    public string Name { get; set; } = string.Empty;
    public string DebtType { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public decimal DebtAmount
    {
        get => _debtAmount;
        set
        {
            if (_debtAmount != value)
            {
                _debtAmount = value;
                OnPropertyChanged(nameof(DebtAmount));
                OnPropertyChanged(nameof(DebtProgress));
                OnPropertyChanged(nameof(TotalPaid));
                OnPropertyChanged(nameof(DebtStatus));
            }
        }
    }
    public decimal Balance
    {
        get => _balance;
        set
        {
            if (_balance != value)
            {
                _balance = value;
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(DebtProgress));
                OnPropertyChanged(nameof(TotalPaid));
                OnPropertyChanged(nameof(DebtStatus));
            }
        }
    }
    public DateTime CreationDate { get; set; }

    public string DebtStatus => Balance == 0 ? "Completed" : "Pending";
    public double DebtProgress => DebtAmount == 0 ? 0 : (double)(DebtAmount - Balance) / (double)DebtAmount;
    public decimal TotalPaid => DebtAmount - Balance;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}