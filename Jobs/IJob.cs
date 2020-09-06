namespace Jobs
{
    public interface IJob
    {
        void Run(string identifier);
    }
}