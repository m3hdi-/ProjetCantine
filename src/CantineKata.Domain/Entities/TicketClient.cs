namespace CantineKata.Domain.Entities;

public class TicketClient
{
    public Client Client { get; set; }
    public string Nom { get; set; }
    public List<string> Produits { get; set; } = new();
    public decimal MontantTotal { get; set; }
    public decimal PriseEnCharge { get; set; }
    public decimal MontantPaye { get; set; }
    public DateTime Date { get; set; }

    public TicketClient(Client client, List<string> produits, decimal montantTotal, decimal priseEnCharge, decimal montantPaye, DateTime date)
    {
        Client = client;
        Produits = produits;
        MontantTotal = montantTotal;
        PriseEnCharge = priseEnCharge;
        MontantPaye = montantPaye;
        Date = date;
    }
}
