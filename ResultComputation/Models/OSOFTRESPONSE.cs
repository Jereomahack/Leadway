using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LightWay.Models
{
    public class OSOFTRESPONSE
    {
        public string MerchantNumber { get; set; }

        public decimal ProductId { get; set; }

        public decimal TransactionType { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string SiteRedirectURL { get; set; }

        public string TransactionReference { get; set; }

        public string Hash { get; set; }

        public decimal PaymentItemId { get; set; }

        public string SiteName { get; set; }

        public string CustomerId { get; set; }

        public string CustomerIdDescription { get; set; }

        public string CustomerName { get; set; }

        public string CustomerNameDescription { get; set; }

        public string PaymentItemName { get; set; }

        public string retRef { get; set; }

        public string PayRef { get; set; }

        public string ResponseCode { get; set; }

        public string ResponseDescription { get; set; }

        public DateTime TransactionDate { get; set; }

        public DateTime ConfirmationDate { get; set; }

        public string GatewayRef { get; set; }

        public string SwitchRef { get; set; }

        public string CardNumber { get; set; }

        public string ApprAmt { get; set; }
    }
}