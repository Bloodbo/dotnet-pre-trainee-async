static string ProcessData(string dataName)
{
	Console.WriteLine($"Начинаем обработку {dataName} ...");
	Thread.Sleep(3000); // неасинхронный таймер
	return $"Обработка {dataName} завершена";
}
static async Task<string> ProcessDataAsync(string dataName, int delayMilliseconds)
{
	if (dataName == "File2")
	{
		throw new Exception("Невозможно обработать File2!");
	}
	Console.WriteLine($"Начинаем обработку {dataName} ...");
	await Task.Delay(delayMilliseconds); // асинхронный таймер задачи запускаются в одно и то же время
	return $"Обработка {dataName} завершена";
}
private static async Task Main(string[] args)
{
	var watch = new Stopwatch();
	watch.Start();
	Console.WriteLine(ProcessData("File1"));
	Console.WriteLine(ProcessData("File2"));
	Console.WriteLine(ProcessData("File3"));
	watch.Stop();
	Console.WriteLine($"Синхронное время: {watch.Elapsed.TotalSeconds:F2} секунд");

	watch.Start();
	Task<string> t1 = ProcessDataAsync("File1", 3000);
	Task<string> t2 = ProcessDataAsync("File2", 3000);
	Task<string> t3 = ProcessDataAsync("File3", 3000);
	await Task.WhenAll(t1, t2, t3);
	Console.WriteLine(await t1);
	Console.WriteLine(await t2);
	Console.WriteLine(await t3);
	watch.Stop();
	Console.WriteLine($"Асинхронное время: {watch.Elapsed.TotalSeconds:F2} секунд");

	watch.Start();
	Task<string> t1 = ProcessDataAsync("File1", 3000);
	Task<string> t2 = ProcessDataAsync("File2", 1000);	
	Task<string> t3 = ProcessDataAsync("File3", 2000);
	List<Task<string>> taskList = new List<Task<string>>();
	taskList.Add(t1);
	taskList.Add(t2);
	taskList.Add(t3);
	try
	{
	while (taskList.Count > 0)
	{
		Task<string> taskCompleted = await Task.WhenAny(taskList);
		Console.WriteLine(await taskCompleted);
		taskList.Remove(taskCompleted);
	}
	}
	catch(Exception ex)
	{
	Console.WriteLine($"ПРОИЗОШЛА ОШИБКА: {ex.Message}");
	}
	watch.Stop();
	Console.WriteLine($"Асинхронное время (WhenAny): {watch.Elapsed.TotalSeconds:F2} секунд");
}
