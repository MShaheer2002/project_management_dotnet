namespace project_management_backend.Domain.Entities.Task
{
    public class Task
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description {get; private set;}
        

    }


    public enum TaskStatus
    {
        Backlog,

    }
}