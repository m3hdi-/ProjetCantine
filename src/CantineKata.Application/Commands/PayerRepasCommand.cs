using MediatR;

namespace CantineKata.Application.Commands
{
    public class PayerRepasCommand : IRequest<string>
    {
        public int ClientId{ get; set; }

        public List<string> Supplements { get; set; } = new List<string>();

        public PayerRepasCommand(int clientId, List<string> supplements)
        {
            ClientId = clientId;
            Supplements = supplements;
        }

    }
}
