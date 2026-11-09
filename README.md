# Async vs Sync Processing (dotnet-pre-trainee-async)

## Project Description
This project demonstrates the difference between synchronous blocking code and asynchronous non-blocking code in C#. It simulates processing multiple files with different delays and measures execution time.

## Execution Results
*   **Synchronous processing:** ~9.00 seconds
*   **Asynchronous processing (WhenAll):** ~3.00 seconds
*   **Asynchronous processing (WhenAny):** ~3.00 seconds (but results are printed in completion order)

## Comparison Questions

**1. Why did the synchronous version take longer?**
The synchronous version uses `Thread.Sleep`, which blocks the main thread. It processes files strictly one after another, so the total time is the sum of all delays (3+3+3 = 9 seconds).

**2. Why can the asynchronous waiting periods overlap?**
Because `Task.Delay` does not block the thread. When we start the tasks without awaiting them immediately, they run concurrently in the background. The thread is free to start the next task while waiting for the previous one to finish.

**3. Does async automatically create a new thread?**
No. `async/await` does not create new threads. It uses the existing thread and releases it while waiting for I/O operations (like timers, network, or database calls) to complete.

**4. What is the difference between Thread.Sleep and Task.Delay?**
`Thread.Sleep` blocks the current thread, preventing it from doing any other work. `Task.Delay` creates a task that completes after a specified time, but it does not block the thread during the wait.

**5. Why would this difference matter in an ASP.NET Core application?**
In ASP.NET Core, threads are a limited resource. If you block a thread with `Thread.Sleep`, it cannot process other incoming HTTP requests. This severely limits the application's scalability and throughput. Non-blocking async code allows the server to handle thousands of concurrent requests efficiently.

**6. What is the difference between Task.WhenAll and Task.WhenAny?**
*   `Task.WhenAll` waits for *all* tasks in the collection to complete before continuing.
*   `Task.WhenAny` waits for *at least one* task to complete and returns the first completed task. It is useful for processing results as they become available.

**7. Does asynchronous programming always make code faster?**
No. Async does not make a single operation faster (a 3-second delay is still 3 seconds). It improves *throughput* and *responsiveness* by freeing up threads to do other work while waiting. It only reduces total execution time if independent operations can be run concurrently.
