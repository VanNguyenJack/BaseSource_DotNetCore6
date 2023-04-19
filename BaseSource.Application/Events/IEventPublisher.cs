using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Application.Events
{
    public interface IEventPublisher
    {
        Task PublishMessage<T>(T entity, string key = null) where T : class;

        Task EntityInserted<T>(T entity) where T : class;
        Task EntityUpdated<T>(T entity) where T : class;
        Task EntityDeleted<T>(T entity) where T : class;
    }
}
