using MediatR;

namespace BaseSource.Application.Events
{
    public class EntityUpdated<T> : INotification where T : class
    {
        public EntityUpdated(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
