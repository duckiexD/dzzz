using System;

// Интерфейсы
public interface IPaymentProcessor
{
    bool ProcessPayment(decimal amount);
    bool RefundPayment(decimal amount, string transactionId);
}

public interface IPaymentValidator
{
    bool ValidatePayment(decimal amount, string paymentDetails);
}

// Реализации процессоров оплаты
public class PayPalProcessor : IPaymentProcessor
{
    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Обработка PayPal платежа на сумму {amount}");
        // Логика обработки платежа через PayPal
        return true; // В реальном приложении нужно возвращать результат операции
    }

    public bool RefundPayment(decimal amount, string transactionId)
    {
        Console.WriteLine($"Возврат PayPal платежа {transactionId} на сумму {amount}");
        // Логика возврата платежа через PayPal
        return true;
    }
}

public class CreditCardProcessor : IPaymentProcessor, IPaymentValidator
{
    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Обработка кредитной карты на сумму {amount}");
        // Логика обработки платежа по кредитной карте
        return true;
    }

    public bool RefundPayment(decimal amount, string transactionId)
    {
        Console.WriteLine($"Возврат по кредитной карте {transactionId} на сумму {amount}");
        // Логика возврата платежа по кредитной карте
        return true;
    }

    public bool ValidatePayment(decimal amount, string paymentDetails)
    {
        Console.WriteLine($"Валидация кредитной карты: {paymentDetails}");
        // Проверка данных карты, CVV, срока действия и т.д.
        return !string.IsNullOrEmpty(paymentDetails) && amount > 0;
    }
}

public class CryptoCurrencyProcessor : IPaymentProcessor
{
    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Обработка криптовалютного платежа на сумму {amount} BTC");
        // Логика обработки криптовалютного платежа
        return true;
    }

    public bool RefundPayment(decimal amount, string transactionId)
    {
        Console.WriteLine($"Возврат криптовалютного платежа {transactionId} на сумму {amount} BTC");
        // Логика возврата криптовалютного платежа
        return true;
    }
}

// Сервис обработки платежей
public class PaymentService
{
    private readonly IPaymentProcessor _paymentProcessor;
    private readonly IPaymentValidator _paymentValidator;

    public PaymentService(IPaymentProcessor paymentProcessor, IPaymentValidator paymentValidator = null)
    {
        _paymentProcessor = paymentProcessor;
        _paymentValidator = paymentValidator;
    }

    public bool MakePayment(decimal amount, string paymentDetails)
    {
        if (_paymentValidator != null && !_paymentValidator.ValidatePayment(amount, paymentDetails))
        {
            Console.WriteLine("Платеж не прошел валидацию");
            return false;
        }

        return _paymentProcessor.ProcessPayment(amount);
    }

    public bool RefundPayment(decimal amount, string transactionId)
    {
        return _paymentProcessor.RefundPayment(amount, transactionId);
    }
}

// Пример использования
class Program
{
    static void Main(string[] args)
    {
        // Пример с PayPal (без валидации)
        var payPalProcessor = new PayPalProcessor();
        var payPalService = new PaymentService(payPalProcessor);
        payPalService.MakePayment(100.50m, "user@example.com");
        
        // Пример с кредитной картой (с валидацией)
        var creditCardProcessor = new CreditCardProcessor();
        var creditCardService = new PaymentService(creditCardProcessor, creditCardProcessor);
        creditCardService.MakePayment(200.75m, "4111111111111111|12/25|123");
        
        // Пример с криптовалютой (без валидации)
        var cryptoProcessor = new CryptoCurrencyProcessor();
        var cryptoService = new PaymentService(cryptoProcessor);
        cryptoService.MakePayment(0.005m, "1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa");
    }
}
