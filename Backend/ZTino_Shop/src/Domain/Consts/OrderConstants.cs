namespace Domain.Constants
{
    public static class OrderStatus
    {
        public const string Pending = "Pending";
        public const string Confirmed = "Confirmed";
        public const string Shipping = "Shipping";
        public const string Delivered = "Delivered";
        public const string Cancelled = "Cancelled";
        public const string Returned = "Returned";
    }

    public static class PaymentMethod
    {
        public const string COD = "COD";
        public const string BankTransfer = "BankTransfer";
        public const string CreditCard = "CreditCard";
        public const string EWallet = "EWallet";
    }

    public static class PaymentStatus
    {
        public const string Pending = "Pending";
        public const string Completed = "Completed";
        public const string Failed = "Failed";
        public const string Refunded = "Refunded";
    }

    public static class InvoiceStatus
    {
        public const string Unpaid = "Unpaid";
        public const string Paid = "Paid";
        public const string Void = "Void";
        public const string Overdue = "Overdue";
    }
}