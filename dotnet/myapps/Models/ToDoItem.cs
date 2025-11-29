namespace libreria.WebApi.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public bool IsCompleted { get; set; }
        public int UserId { get; set; }
        public Users User { get; set; }
    }
}
