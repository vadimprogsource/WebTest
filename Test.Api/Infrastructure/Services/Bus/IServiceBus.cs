using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Api.Infrastructure.Services.Bus;

public interface IServiceBus
{
    void Send<TMessage>(TMessage message);
    void SendAsync<TMessage>(TMessage message);
    IEnumerable<TMessage> Retrieve<TMessage>(string topicKey);
    Task<IEnumerable<TMessage>> RetrieveAsync<TMessage>(string topicKey);
    void Receive<TMessage>(IReceiver<TMessage> receiver);
    void Receive<TMessage>(IReceiverAsync<TMessage> receiver);

}
