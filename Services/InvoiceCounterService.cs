using inventoryApiDotnet.Constants;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;

namespace inventoryApiDotnet.Services
{
    public class InvoiceCounterService : IInvoiceCounterService
    {
        public readonly IInvoiceCounterRepository _invoiceCounterRepository;

        public InvoiceCounterService(IInvoiceCounterRepository invoiceCounterRepository)
        {
            _invoiceCounterRepository = invoiceCounterRepository;
        }

        public async Task<string> GenerateInvoiceNumber()
        {
            long currentCounter = 0;
            var response = await _invoiceCounterRepository.GetAll();
            if (response.FirstOrDefault() == null)
            {
                var firstInsert = new InvoiceCounter()
                {
                    Counter = 1
                };
                await _invoiceCounterRepository.Add(firstInsert);
                currentCounter = firstInsert.Counter;
            }
            else
            {
                var currentCounterObject = response.FirstOrDefault();
                currentCounter = currentCounterObject.Counter + 1;
                currentCounterObject.Counter = currentCounter;
                await _invoiceCounterRepository.Update(currentCounterObject);
            }

            string InvoiceNumber = InventoryConstants.InvoiceNumberInitials + "-"
                                 + DateTime.Now.Year.ToString() + "-"
                                 + currentCounter.ToString().PadLeft(6,'0');
            return InvoiceNumber;
        }
    }
}