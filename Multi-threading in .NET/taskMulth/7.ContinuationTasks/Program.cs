using System;
using System.Threading;
using System.Threading.Tasks;

namespace _7.ContinuationTasks
{
	/// <summary>
	/// 7.	Create a Task and attach continuations to it according to the following criteria:
	///		a.Continuation task should be executed regardless of the result of the parent task.
	///		b.Continuation task should be executed when the parent task finished without success.
	///		c.Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
	///		d.Continuation task should be executed outside of the thread pool when the parent task would be cancelled
	/// </summary>
	class Program
	{
		static CancellationTokenSource cancellationTokenSource;

		static void SuccessAction(CancellationToken cancellationToken)
		{
			Console.WriteLine($"SuccessAction started in: {Thread.CurrentThread.ManagedThreadId} thread");
			if (cancellationToken.IsCancellationRequested)
			{
				Console.WriteLine("SuccessAction cancelled");
				return;
			}

			Console.WriteLine("SuccessAction finished");
		}

		static void FailedAction()
		{
			Console.WriteLine($"FailedAction started in: {Thread.CurrentThread.ManagedThreadId} thread");
			throw new Exception();
		}

		static void ContinuationAction(Task prevTask)
		{
			Console.WriteLine($"ContinuationAction started in: {Thread.CurrentThread.ManagedThreadId} thread");
			Console.WriteLine(prevTask.Status.ToString());
		}

		static void Main(string[] args)
		{
			A();
			Console.WriteLine();
			B();
			Console.WriteLine();
			C();
			Console.WriteLine();
			D();

			Console.Read();
		}

		static void A()
		{
			Console.WriteLine("a.Continuation task should be executed regardless of the result of the parent task.");
			cancellationTokenSource = new CancellationTokenSource();

			Task.Run(() => SuccessAction(cancellationTokenSource.Token), cancellationTokenSource.Token)
				.ContinueWith(ContinuationAction)
				.Wait();

			Task.Run(() => FailedAction())
				.ContinueWith(ContinuationAction)
				.Wait();
		}

		static void B()
		{
			Console.WriteLine("b.Continuation task should be executed when the parent task finished without success.");
			cancellationTokenSource = new CancellationTokenSource();

			try
			{
				Task.Run(() => SuccessAction(cancellationTokenSource.Token), cancellationTokenSource.Token)
					.ContinueWith(ContinuationAction, TaskContinuationOptions.NotOnRanToCompletion)
					.Wait();
			}
			catch (AggregateException) { }

			Task.Run(() => FailedAction())
				.ContinueWith(ContinuationAction, TaskContinuationOptions.NotOnRanToCompletion)
				.Wait();
		}

		static void C()
		{
			Console.WriteLine("c.Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation");
			cancellationTokenSource = new CancellationTokenSource();

			try
			{
				Task.Run(() => SuccessAction(cancellationTokenSource.Token), cancellationTokenSource.Token)
					.ContinueWith(ContinuationAction, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously)
					.Wait();
			}
			catch (AggregateException) { }

			Task.Run(() => FailedAction())
				.ContinueWith(ContinuationAction, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously)
				.Wait();
		}

		static void D()
		{
			Console.WriteLine("d.Continuation task should be executed outside of the thread pool when the parent task would be cancelled");
			cancellationTokenSource = new CancellationTokenSource();

			try
			{
				Task.Run(() => SuccessAction(cancellationTokenSource.Token))
					.ContinueWith(ContinuationAction, TaskContinuationOptions.OnlyOnCanceled)
					.Wait();
			}
			catch (AggregateException) { }

			try
			{
				Task task = Task.Run(() => SuccessAction(cancellationTokenSource.Token), cancellationTokenSource.Token)
					.ContinueWith(ContinuationAction, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);
					cancellationTokenSource.Cancel();
				task.Wait();
			}
			catch (AggregateException) { }
		}
	}
}
