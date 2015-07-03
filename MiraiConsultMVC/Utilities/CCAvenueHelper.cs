using CCA.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

public static class CCAvenueHelper
{
    private static CCACrypto ccaCrypto = new CCACrypto();

    public static CCAFormDetails getFormDetails(string orderId, string successUrl,
        string cancelUrl, decimal amount)
    {
        string merchantId = ConfigurationManager.AppSettings["MerchantId"].ToString();
        if (amount <= 0 || String.IsNullOrEmpty(orderId) || String.IsNullOrEmpty(successUrl) ||
            String.IsNullOrEmpty(cancelUrl) || String.IsNullOrEmpty(merchantId))
            throw new Exception("Invalid Payment Request!");

        CCAFormDetails details = new CCAFormDetails();

        string workingKey = ConfigurationManager.AppSettings["WorkingKey"].ToString();
        details.cca_accessCode = ConfigurationManager.AppSettings["AccessCode"].ToString();
        details.cca_url = ConfigurationManager.AppSettings["CcavenueUrl"].ToString();
        string redirectSiteUrl = ConfigurationManager.AppSettings["redirectSiteUrl"];       
        string ccaRequest = "merchant_id=" + merchantId + "&order_id=" + orderId + "&amount=" + amount +
                        "&currency=INR&redirect_url=" + redirectSiteUrl + successUrl + "&cancel_url=" + redirectSiteUrl + cancelUrl;
        details.cca_request = ccaCrypto.Encrypt(ccaRequest, workingKey);
        return details;
    }
}

public class CCAFormDetails
{
    public string cca_url;
    public string cca_request;
    public string cca_accessCode;
}