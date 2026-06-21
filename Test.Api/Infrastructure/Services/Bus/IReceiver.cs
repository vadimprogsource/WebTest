using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Api.Infrastructure.Services.Bus;

public interface IReceiver<TMessage>
{
    Task Receive(IServiceBus bus, TMessage message);
}

public interface IReceiverAsync<TMessage>
{
    Task ReceiveAsync(IServiceBus bus, TMessage message);
}
