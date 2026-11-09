using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

internal class Program
{
    static string ProcessData(string dataName)
    {
        Console.WriteLine($"Начинаем обработку {dataName} ...");
        Thread.Sleep(3000);
        return $"Обработка {dataName} завершена";
    }

    static async Task<string> ProcessDataAsync(string dataName, int delayMilliseconds)
    {
        Console.WriteLine($"Начинаем обработку {dataName} ...");
        await Task.Delay(delayMilliseconds);

        if (dataName == "File2")
        {
            throw new Exception("Невозможно обработать File2!");
        }

        return $"Обработка {dataName} завершена";
    }

    private static async Task Main(string[] args)
    {
        var watch = new Stopwatch();
        
        // 1. Синхронный блок
        watch.Start();
        Console.WriteLine(ProcessData("File1"));
        Console.WriteLine(ProcessData("File2"));
        Console.WriteLine(ProcessData("File3"));
        watch.Stop();
        Console.WriteLine($"Синхронное время: {watch.Elapsed.TotalSeconds:F2} секунд\n");

        // 2. WhenAll блок
        watch.Restart(); // Сброс и старт
        Task<string> t1 = ProcessDataAsync("File1", 3000);
        Task<string> t2 = ProcessDataAsync("File2", 3000);
        Task<string> t3 = ProcessDataAsync("File3", 3000);
        
        try
        {
            await Task.WhenAll(t1, t2, t3);
            Console.WriteLine(await t1);
            Console.WriteLine(await t2);
            Console.WriteLine(await t3);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ОШИБКА в WhenAll: {ex.Message}");
        }
        
        watch.Stop();
        Console.WriteLine($"Асинхронное время (WhenAll): {watch.Elapsed.TotalSeconds:F2} секунд\n");

        // 3. WhenAny блок
        watch.Restart();
        Task<string> a1 = ProcessDataAsync("File1", 3000);
        Task<string> a2 = ProcessDataAsync("File2", 1000); 
        Task<string> a3 = ProcessDataAsync("File3", 2000);
        
        var taskList = new List<Task<string>> { a1, a2, a3 };
        
        try
        {
            while (taskList.Count > 0)
            {
                Task<string> taskCompleted = await Task.WhenAny(taskList);
                Console.WriteLine(await taskCompleted);
                taskList.Remove(taskCompleted);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ПРОИЗОШЛА ОШИБКА в WhenAny: {ex.Message}");
        }
        
        watch.Stop();
        Console.WriteLine($"Асинхронное время (WhenAny): {watch.Elapsed.TotalSeconds:F2} секунд");
    }
}
