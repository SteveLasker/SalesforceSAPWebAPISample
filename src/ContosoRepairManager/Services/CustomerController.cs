using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ContosoRepairManager.Services {
    public class CustomerController : ApiController {
        public async Task<Models.CustomerProduct> Get(string email) {
            var client = await new Salesforce.SalesforceService().GetUsernamePasswordForceClient();

            var contacts = await client.QueryAsync<Models.Salesforce.Contact>(
                    String.Format("SELECT Id, FirstName, LastName, Phone, Email, SAP_Id__c FROM Contact WHERE Email ='{0}'", email));
            var sfContact = contacts.records.FirstOrDefault();

            #region
#error Provide your SAP Connection Information
            string serviceRoot = "the URL for your SAP OData feed";
            string userName = "SAP UserName";
            string password = "SAP Password";
            #endregion

            var sapCtx = new SAPService.ZGWSAMPLE_SRV(new Uri(serviceRoot));
            sapCtx.Credentials = new NetworkCredential(userName, password);

            var bpQuery = from bp in sapCtx.BusinessPartnerCollection
                          where bp.BusinessPartnerID == sfContact.SAP_Id
                          select bp;

            // Extend the query to populate the SalesOrders, LineItems and Product for each LineItem
            var businessPartnerSalesOrders =
                ((System.Data.Services.Client.DataServiceQuery<SAPService.BusinessPartner>)(bpQuery)).Expand("SalesOrders/LineItems/Product");

            // Retrieve the SAP Customer, with the graph of SalesOrders, LineItems and the Product for each LineItem
            var sapCustomerWithOrders = businessPartnerSalesOrders.FirstOrDefault();

            // Map the Salesforce and SAP Objects into the CustomerProduct shape we wish to return in our WebAPI/REST Feed
            Models.CustomerProduct customerProducts = new Models.CustomerProduct() {
                CustomerId = sfContact.Id,
                FirstName = sfContact.FirstName,
                LastName = sfContact.LastName,
                Phone = sfContact.Phone,
                Email = sfContact.Email,
                Products = sapCustomerWithOrders.SalesOrders
                    .SelectMany(so => so.LineItems)
                    .Select(lineItem => new Models.Product() {
                        ProductId = lineItem.Product.ProductId,
                        Name = lineItem.Product.Name,
                        Category = lineItem.Product.Category,
                        SupplierName = lineItem.Product.SupplierName,
                        ProductPicUrl = lineItem.Product.ProductPicUrl,
                        Description = lineItem.Product.Description,
                        PurchaseDate = lineItem.DeliveryDate.Value,
                        WarrantyEndDate = lineItem.DeliveryDate.Value.AddYears(1)
                    }).ToList()
            };
            return customerProducts;
        }
    }
}
