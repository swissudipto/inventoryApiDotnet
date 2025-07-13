namespace inventoryApiDotnet.Interface
{
    public interface IInvoiceCounterService
    {
        Task<string> GenerateInvoiceNumber();
    }
}