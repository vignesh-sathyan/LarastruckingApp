namespace LarastruckingApp.Repository.IRepository
{
    public interface ILoggerRepository
    {
        int AddException(string errorMessage, string trace,string innerException="");
    }
}
