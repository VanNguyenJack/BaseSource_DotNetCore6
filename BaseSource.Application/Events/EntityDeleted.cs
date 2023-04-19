using MediatR;

namespace BaseSource.Application.Events
{
    public class EntityDeleted<T> : INotification where T : class

    {
        public EntityDeleted(T entity)
        {
            Entity = entity;
        }
        public T Entity { get; private set; }
    }
}
