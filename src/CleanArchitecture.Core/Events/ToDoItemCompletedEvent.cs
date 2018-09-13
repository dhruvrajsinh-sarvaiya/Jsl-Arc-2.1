using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Events
{
    public class ToDoItemCompletedEvent : BaseDomainEvent
    {
         ToDoItem CompletedItem { get; set; }

        public ToDoItemCompletedEvent(ToDoItem completedItem)
        {
            CompletedItem = completedItem;
        }
    }
}