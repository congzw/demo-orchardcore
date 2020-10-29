namespace NbSites.Core.AutoTasks
{
    public interface IAutoTask
    {
        void Run();
        string Category { get;}
        int Order { get; set; }
    }
}
