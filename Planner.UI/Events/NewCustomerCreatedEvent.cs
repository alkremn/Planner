using Planner.DataAccess;
using Prism.Events;

namespace Planner.UI.Events
{
    public class NewCustomerCreatedEvent : PubSubEvent<CustomerWrapper>
    {
    }
}
