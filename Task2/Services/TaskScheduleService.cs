namespace Task2.Services
{
    public class TaskScheduleService
    {

        public TaskScheduleService()
        {

        }
        public void Execute(Action action, int hours, CancellationToken token = default)
        {
            _ = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    action();
                    await Task.Delay(TimeSpan.FromHours(hours), token);
                }
            }, token);
        }

    }
}
