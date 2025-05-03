namespace CyberGreenhouse.MessageBus.Messages
{
    public class BusMessage
    {
        public Guid Id { get; set; }

        public BusMessage()
        {
            Id = Guid.NewGuid();
        }
    }
}
