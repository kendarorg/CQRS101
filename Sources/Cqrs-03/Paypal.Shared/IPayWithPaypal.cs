using System;

namespace PayPal.Shared
{
    public interface IPayWithPayPal
    {
        Guid PayAndNotify(string payPalUser, double amount);
        void CancelPayment(Guid paymentId);
    }
}