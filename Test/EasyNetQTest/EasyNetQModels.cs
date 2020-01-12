using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;

namespace EasyNetQTest
{
    [Queue("Qka.Order", ExchangeName = "Qka.Order")]
    public class Order
    {
        public int OrderId { get; set; }
    }

    public class OrderConsumer : IConsume<Order>
    {
        //IService _service;
        public OrderConsumer()
        {
            //_service = service;
        }
        //[ForTopic("Topic.Foo")]
        //[AutoSubscriberConsumer(SubscriptionId = "OrderService")]
        public void Consume(Order message)
        {
            //_service.Write();
            Console.WriteLine($"revc : {message.OrderId}" );
        }
    }
}
