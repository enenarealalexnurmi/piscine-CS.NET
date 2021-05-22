using System;

//double overpaymentNormal = 0.0;
double overpaymentDecreaseAmount = 0.0;
double overpaymentReducedTerm = 0.0;
string amountString = "платежа";
string termString = "срока";
double differenceBetweenOverpayment = 0.0;

try
{
    double sum = double.Parse(args[0]);
    double rate = double.Parse(args[1]);
    int term = int.Parse(args[2]);
    int selectedMonth = int.Parse(args[3]);
    double payment = double.Parse(args[4]);
    if (sum <= 0.0 || rate <= 0.0 || term < 1 || selectedMonth < 1 || payment <= 0.0 || selectedMonth > term || payment > sum)
        throw new Exception();
    double rateMonth = rate / 12 / 100;
    var currentDate = new DateTime(2021, 5, 1);
    double[] annuityPayments = new double[term];
    Console.WriteLine("Дата		Платеж               ОД		      Проценты	          Остаток долга");
    for (int i = 0; i < term; i++)
    {
        annuityPayments[i] = sum * rateMonth * Math.Pow((1 + rateMonth), term) / (Math.Pow((1 + rateMonth), term) - 1);
    }
    Console.WriteLine($"Переплата при уменьшении платежа: {overpaymentDecreaseAmount}р.");
    Console.WriteLine($"Переплата при уменьшении срока: {overpaymentReducedTerm}р.");
    Console.WriteLine($"Уменьшение {amountString} выгоднее уменьшения {termString} на {Math.Abs(differenceBetweenOverpayment)}р.");
}
catch
{
    Console.WriteLine("Ошибка ввода. Проверьте входные данные и повторите запрос.");
}