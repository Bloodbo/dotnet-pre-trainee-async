static string ProcessData(string dataName)
{
	Console.WriteLine($"Начинаем обработку {dataName} ...");
	Thread.Sleep(3000); // неасинхронный таймер
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
}
