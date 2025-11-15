namespace Debt_Collector
{
    public class ActivityLog
    {
        public required string LogType { get; set; } = string.Empty; // Initialize with an empty string
        public required string Name { get; set; } = string.Empty; // Initialize with an empty string
        public required string DebtType { get; set; } = string.Empty; // Initialize with an empty string
        public required string PhoneNumber { get; set; } = string.Empty; // Initialize with an empty string
        public decimal DebtAmount { get; set; }
        public decimal AmountBorrowed { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal RemainingBalance { get; set; }
        public DateTime ActionDate { get; set; }
        public string DebtStatus => RemainingBalance == 0 ? "Completed" : "Pending";
        public double DebtProgress => DebtAmount == 0 ? 0 : (double)(DebtAmount - RemainingBalance) / (double)DebtAmount;

        // Custom display method to center LogType
        public string DisplayLogType()
        {
            // Return LogType centered if it is within the UI context
            return string.IsNullOrWhiteSpace(LogType) ? string.Empty : LogType.PadLeft((LogType.Length + 2) / 2).PadRight(LogType.Length);
        }
    }
}