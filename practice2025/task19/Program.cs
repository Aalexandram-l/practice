using System;
using System.Threading;
using task17;

namespace task17
{
    internal class Program
    {
        private static void Main()
        {
            Reporter.Init();
            Reporter.Write("--- Запуск ServerThread ---");

            using var server = new ServerThread();

            for (int i = 1; i <= 5; i++)
            {
                var stepCmd = new TestStepCommand(i);
                var fullCmd = new RepeatableStepCommand(stepCmd, 3, server);
                server.Enqueue(fullCmd);
            }

            Thread.Sleep(500);

            Reporter.Write("--- Все команды завершены ---");

            Reporter.Write("--- Отправка HardStop ---");
            server.Enqueue(new SoftStop(server));

            server.Join();
            Reporter.Write("--- ServerThread остановлен ---");
            Reporter.Write("Отчёты готовы: Report.txt и report.csv");
        }
    }
}
