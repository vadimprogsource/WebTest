using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Api.Infrastructure;


public interface IMessageReceiver<TMessage>
{
    void Receive(IServiceBusContext context, TMessage message);
}


public interface IServiceBusContext
{
    void Send<TMessage>(TMessage message);
}

public interface IServiceBus
{
    IServiceBusContext Context { get; }
    void Register<TMessage>(IMessageReceiver<TMessage> receiver);
}
