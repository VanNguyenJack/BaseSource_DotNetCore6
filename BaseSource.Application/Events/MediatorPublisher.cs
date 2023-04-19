using MediatR;
using System.Threading.Tasks;

namespace BaseSource.Application.Events
{
    public class MediatorPublisher : IEventPublisher
    {
        IMediator _eventPublisher;
        public MediatorPublisher(IMediator eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }
        public async Task PublishMessage<T>(T entity, string key = null) where T : class
        {
            //Write event to DB with full info to can retry
            await _eventPublisher.Publish(entity);
        }
        public async Task EntityInserted<T>(T entity) where T : class
        {
            await _eventPublisher.Publish(new EntityInserted<T>(entity));
        }
        public async Task EntityUpdated<T>(T entity) where T : class
        {
            await _eventPublisher.Publish(new EntityUpdated<T>(entity));
        }
        public async Task EntityDeleted<T>(T entity) where T : class
        {
            await _eventPublisher.Publish(new EntityDeleted<T>(entity));
        }
    }
}
