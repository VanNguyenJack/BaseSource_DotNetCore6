using MediatR;

namespace BaseSource.Application.Events
{
    public class EntityInserted<T> : INotification where T : class
    {
        public EntityInserted(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
