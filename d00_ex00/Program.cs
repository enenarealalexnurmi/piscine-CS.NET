using System;
using static System.Math;

static double GetAnnuityPayment(double sum, double rateMonth, int term)
{
    double tempPower = Pow((1 + rateMonth), term);
    return sum * rateMonth * tempPower / (tempPower - 1);
}

static void TableWriteLine(string arg1, string arg2, string arg3, string arg4, string arg5)
{
    const int dateAlligment = -10;
    const int paymentAlligment = -18;
    const int mainPartAlligment = -18;
    const int percentPartAlligment = -22;
    const int remaindAlligment = -20;
    static string HorizontalLine(int count)
    {
        string temp = "";

        while (count > 0)
        {
            temp += "-";
            count--;
        }
        return temp;
    }
    arg1 = arg1.Length == 0 ? HorizontalLine(Abs(dateAlligment)) : arg1;
    arg2 = arg2.Length == 0 ? HorizontalLine(Abs(paymentAlligment)) : arg2;
    arg3 = arg3.Length == 0 ? HorizontalLine(Abs(mainPartAlligment)) : arg3;
    arg4 = arg4.Length == 0 ? HorizontalLine(Abs(percentPartAlligment)) : arg4;
    arg5 = arg5.Length == 0 ? HorizontalLine(Abs(remaindAlligment)) : arg5;
    Console.WriteLine($"|{arg1, dateAlligment}|{arg2,paymentAlligment}|{arg3,mainPartAlligment}|{arg4,percentPartAlligment}|{arg5,remaindAlligment}|");
}

static double GetPercents(double sum, double rate, DateTime Date)
{
    return sum * rate * DateTime.DaysInMonth(Date.Year, Date.Month) / (DateTime.IsLeapYear(Date.Year) ? 36600 : 36500);
}

static double GetOverPayment(double sum, double rate, int term, int selectedMounth, double payment, DateTime currentDate, string typeString, int mode)
{
    string[] tableColumnNames = { "Дата", "Платеж", "Погашение долга", "Погашение процентов", "Остаток долга" };
    double overPayment = sum;
    double rateMonth = rate / 12 / 100;
    double annuityPayment = GetAnnuityPayment(sum, rateMonth, term);
    double percents = GetPercents(sum, rate, currentDate);
    double payRemaind = annuityPayment - percents;
    Console.WriteLine("График платежей " + typeString);
    TableWriteLine(tableColumnNames[0], tableColumnNames[1], tableColumnNames[2], tableColumnNames[3], tableColumnNames[4]);
    TableWriteLine("", "", "", "", "");
    int i = 1;
    while (term - i >= 0)
    {
        currentDate = currentDate.AddMonths(1);
        sum -= payRemaind;
        overPayment -= annuityPayment;
        TableWriteLine(currentDate.ToString("dd.MM.yyyy"), annuityPayment.ToString("F2"), payRemaind.ToString("F2"), percents.ToString("F2"), sum.ToString("F2"));
        percents = GetPercents(sum, rate, currentDate);
        payRemaind = annuityPayment - percents;
        if (i == selectedMounth && mode != 0)
        {
            sum -= payment;
            overPayment -= payment;
            switch (mode)
            {
                case 1:
                    annuityPayment = GetAnnuityPayment(sum, rateMonth, term - i);
                    break;
                case 2:
                    term = (int)Log(annuityPayment / (annuityPayment - rateMonth * sum), 1 + rateMonth);
                    i = 0;
                    break;
            }
            percents = GetPercents(sum, rate, currentDate);
            payRemaind = annuityPayment - percents;
        }
        i++;
    }
    if (sum > 0)
        overPayment -= sum;
    return -1 * overPayment;
}

try
{
    double sum = double.Parse(args[0]);
    double rate = double.Parse(args[1]);
    int term = int.Parse(args[2]);
    int selectedMonth = int.Parse(args[3]);
    double payment = double.Parse(args[4]);
    if (sum <= 0.0 || rate <= 0.0 || term < 1 || selectedMonth < 1 || payment <= 0.0 || selectedMonth > term || payment > sum)
        throw new Exception();
    var currentDate = new DateTime(2021, 5, 1);
    double overpaymentNormal = GetOverPayment(sum, rate, term, selectedMonth, payment, currentDate, "", 0);
    Console.WriteLine($"Переплата: {overpaymentNormal:F2}р.\n");
    double overpaymentDecreaseAmount = GetOverPayment(sum, rate, term, selectedMonth, payment, currentDate, "при уменьшении платежа", 1);
    Console.WriteLine();
    double overpaymentReducedTerm = GetOverPayment(sum, rate, term, selectedMonth, payment, currentDate, "при уменьшении срока", 2);
    Console.WriteLine();
    Console.WriteLine($"Переплата при уменьшении платежа: {overpaymentDecreaseAmount:F2}р.");
    Console.WriteLine($"Переплата при уменьшении срока: {overpaymentReducedTerm:F2}р.");
    double differenceBetweenOverpayment = overpaymentDecreaseAmount - overpaymentReducedTerm;
    if ((int)differenceBetweenOverpayment != 0)
    {
        if (differenceBetweenOverpayment > 0)
            Console.WriteLine($"Уменьшение при уменьшении платежа выгоднее уменьшения при уменьшении срока на {Math.Abs(differenceBetweenOverpayment):F2}р.");
        else
            Console.WriteLine($"Уменьшение при уменьшении срока выгоднее уменьшения при уменьшении платежа на {Math.Abs(differenceBetweenOverpayment):F2}р.");
    }
    else
        Console.WriteLine("Переплата одинакова в обоих вариантах.");
}
catch
{
    Console.WriteLine("Ошибка ввода. Проверьте входные данные и повторите запрос.");
}