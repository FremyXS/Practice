using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ekz2._2
{
    class Program
    {
        static void Main(string[] args)
        {
            var bankAccountInfo = File.ReadAllLines(@"../../../info.txt");
            var bankAccount = new BankAccount(int.Parse(bankAccountInfo[0]));
            bankAccount.AddOperations(bankAccountInfo);

            if (bankAccount.Operations.Last().CountMoney < 0)
                throw new Exception("The Expense Exceeded The Balance On The Card");

            var date = Console.ReadLine();
            var dateTime = Console.ReadLine();

            Console.WriteLine(bankAccount.GetHistory(date, dateTime).CountMoney);
        }
    }
    public class BankAccount
    {
        public int StartCountMoney { get; }
        public List<Operation> Operations { get; private set; } = new List<Operation>();
        public BankAccount(int countMoney)
        {
            StartCountMoney = countMoney;
        }
        public readonly static string OperationIn = "in";
        public readonly static string OperationOut = "out";
        public readonly static string OperationRevert = "revert";
        public void AddOperations(string[] info)
        {
            for(var ind = 1; ind < info.Length; ind++)
                AddOneOperation(info[ind].Split(new char[] { ' ', '|' }, StringSplitOptions.RemoveEmptyEntries));
        }
        private void AddOneOperation(string[] oneOperation)
        {
            var operation = new Operation
                (oneOperation[0],
                 oneOperation[1],
                 oneOperation.Length == 4 ? oneOperation[3] : oneOperation[2],
                 oneOperation.Length == 4 ? oneOperation[2] : null
                 );

            if(Operations.Count == 0)
                operation.GetCountMoney(StartCountMoney, null);
            else
                operation.GetCountMoney(Operations.Last().CountMoney, Operations.Last());

            Operations.Add(operation);
        }
        public Operation GetHistory(string date, string dateTime)
        {
            var operation = Operations.FirstOrDefault(el => el.Date == date && el.DateTime == dateTime);

            if (operation is null)
                return Operations.Last();
            else 
                return operation;
        }
    }
    public class Operation
    {
        public string Date { get; }
        public string DateTime { get; }
        public int CountMoneyCirculation { get; }
        public string TypeOperation { get; }
        public int CountMoney { get; private set; } = 0;
        public Operation(string date, string dateTime, string typeOperation, string countMoney = null)
        {
            Date = date;
            this.DateTime = dateTime;
            CountMoneyCirculation = countMoney != null? int.Parse(countMoney) : 0;
            TypeOperation = typeOperation;
        }
        public void GetCountMoney(int sumMoney, Operation oldOperation)
        {
            if(TypeOperation == BankAccount.OperationIn)
            {
                CountMoney = sumMoney + CountMoneyCirculation;
            }
            else if(TypeOperation == BankAccount.OperationOut)
            {
                CountMoney = sumMoney - CountMoneyCirculation;
            }
            else if(TypeOperation == BankAccount.OperationRevert)
            {
                if(oldOperation.TypeOperation == BankAccount.OperationIn)
                {
                    CountMoney = oldOperation.CountMoney - oldOperation.CountMoneyCirculation;
                }
                else if(oldOperation.TypeOperation == BankAccount.OperationOut)
                {
                    CountMoney = oldOperation.CountMoney + oldOperation.CountMoneyCirculation;
                }
            }
        }
    }
}
