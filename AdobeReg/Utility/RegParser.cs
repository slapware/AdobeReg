using System;
using System.Collections.Generic;
using AdobeReg.Models;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Extensions.Configuration; // for app settings
using AdobeReg.Utility;

namespace AdobeReg.Utility
{
    public class RegParser
    {
        public string xmldoc { get; set; }
        public AOrder aorder { get; set; }
        public Source source { get; set; }
        public AUser auser { get; set; }
        public List<AOrder> itemorders;
        public IConfiguration Configuration { get; }
        private ISlapCrypto sCrypt { get; set; }
        private IAdobeGuid aGuid { get; set; }

        public RegParser(string xmlin, IConfiguration configuration, ISlapCrypto crpto, IAdobeGuid adobeGuid)
        {
            this.xmldoc = xmlin;
            this.itemorders = new List<AOrder>();
            Configuration = configuration; // added to inject configuration
            sCrypt = crpto;
            aGuid = adobeGuid;
        }
        /// <summary>
        /// Parse this Digital River order confirmation.
        /// </summary>
        /// <returns>The parse.</returns>
        public bool Parse()
        {
            #region
            // Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(this.xmldoc);
            string xPathString = "//orderID";
            XmlNode xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _orderid = (xmlNode.InnerText);
            xPathString = "//title";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _title = (xmlNode.InnerText);
            xPathString = "//submissionDate";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _subdate = (xmlNode.InnerText);
            xPathString = "//site/siteAddress/addressID";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _addressid = (xmlNode.InnerText);
            xPathString = "//site/siteAddress/city";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _city = (xmlNode.InnerText);
            xPathString = "//site/siteAddress/country";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _country = (xmlNode.InnerText);
            xPathString = "//site/siteAddress/line1";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _line1 = (xmlNode.InnerText);
            xPathString = "//site/siteAddress/name1";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _name1 = (xmlNode.InnerText);
            xPathString = "//xmlNode/siteAddress/name2";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            if (xmlNode != null)
            {
                var _name2 = (xmlNode.InnerText);
            }
            xPathString = "//site/siteAddress/phoneNumber";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _phone = (xmlNode.InnerText);
            xPathString = "//pricing/total/currencyCode";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _currencycode = (xmlNode.InnerText);
            xPathString = "//site/siteAddress/postalCode";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _postalcode = (xmlNode.InnerText);
            xPathString = "//site/siteAddress/state";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _state = (xmlNode.InnerText);
            xPathString = "//loginID";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _loginid = (xmlNode.InnerText);
            xPathString = "//password";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _password = (xmlNode.InnerText);
            // NOTE: total order amount as doube **********************
            xPathString = "//pricing/total/amount";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _total = (xmlNode.InnerText);
            double tamount;
            if (xmlNode != null)
                tamount = Convert.ToDouble(_total);
            else
                tamount = 0.0;
            // NOTE: sub total order amount as dounle *****************
            xPathString = "//pricing/subtotal/amount";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _subtotal = (xmlNode.InnerText);
            double samount;
            if (xmlNode != null)
                samount = Convert.ToDouble(_subtotal);
            else
                samount = 0.0;
            // NOTE: tax amount of order as double *******************
            xPathString = "//pricing/tax/amount";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _tax = (xmlNode.InnerText);
            double xamount;
            if (xmlNode != null)
                xamount = Convert.ToDouble(_tax);
            else
                xamount = 0.0;
            
            xPathString = "//paymentInfos/paymentInfo/customerEmail";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _custEmail = (xmlNode.InnerText);
            xPathString = "//paymentInfos/paymentInfo/customerLastName";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _custLastName = (xmlNode.InnerText);
            xPathString = "//paymentInfos/paymentInfo/customerFirstName";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _custFirstName = (xmlNode.InnerText);
            xPathString = "//paymentInfos/paymentInfo/paymentMethodName";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _payMethod = (xmlNode.InnerText);
            xPathString = "//testOrder";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _testorder = (xmlNode.InnerText);
            xPathString = "//paymentInfos/paymentInfo/billingAddress/postalCode";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _billPostal = (xmlNode.InnerText);
            xPathString = "//paymentInfos/paymentInfo/billingAddress/city";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            var _billCity = (xmlNode.InnerText);
            #endregion
            // NOTE: add user and source information here.
            sCrypt.setConfig(this.Configuration);
            sCrypt.SetKey("PSW");
            string encpass = sCrypt.Encrypt(_password);
            aGuid.setVendor(Configuration.GetSection("VendorID").Value);
            AUser myuser = new AUser()
            {
                user_id = _loginid,
                password = encpass,
                uuid = aGuid.makeGuid()
            };
            this.auser = myuser;
            Source _source = new Source()
            {
                orderid = _orderid,
                type = _payMethod,
                city = _city,
                address1 = _addressid,
                site_name1 = _name1,
                site_phone = _phone,
                postalCode = _postalcode,
                state = _state,
                totalamount = tamount,
                subtotal = samount,
                taxtotal = xamount
            };
            this.source = _source;

            // NOTE: The lineItems loop for order items
            XmlNodeList itemNodes = doc.SelectNodes("//lineItems/item");
            foreach(XmlNode lineNode in itemNodes)
            {
                xmlNode = lineNode.SelectSingleNode(".//quantity");
                var _quantity = (xmlNode.InnerText);
                xmlNode = lineNode.SelectSingleNode(".//lineItemID");
                var _lineitem = (xmlNode.InnerText);
                // NOTE: unit price as double **********************
                xmlNode = lineNode.SelectSingleNode(".//pricing/unitPrice/amount");
                var _unitAmount = (xmlNode.InnerText);
                double uamount;
                if (xmlNode != null)
                    uamount = Convert.ToDouble(_unitAmount);
                else
                    uamount = 0.0;
                
                xmlNode = lineNode.SelectSingleNode(".//product/productID");
                var _productid = (xmlNode.InnerText);
                xmlNode = lineNode.SelectSingleNode(".//product/externalReferenceID");
                var _externalRef = (xmlNode.InnerText);
                xmlNode = lineNode.SelectSingleNode(".//productInfo/mfrPartNumber");
                var _mfrPart = (xmlNode.InnerText);

                string  _hcstoreid = "";
                string _downUrl = "";
                XmlNodeList eitemNodes = lineNode.SelectNodes(".//extendedAttributes/item");

                // NOTE: lineItems item list for oders
                foreach (XmlNode itemNode in eitemNodes)
                {
                    string name = itemNode["name"].InnerText;
                    string value = itemNode["value"].InnerText;
                    Console.WriteLine("Name: {0} {1}", name, value);

                    if (itemNode["name"].InnerText.CompareTo("hcstoreID") == 0)
                        _hcstoreid = itemNode["value"].InnerText;
                    if (itemNode["name"].InnerText.CompareTo("DGPDownloadURL") == 0)
                        _downUrl = itemNode["value"].InnerText;
                     
                }
                // NOTE: add each order info here.
                AOrder _order = new AOrder()
                {
                    orderid = _orderid,
                    orderdate = _subdate,
                    title = _title,
                    currency = _currencycode,
                    submissionDate = _subdate,
                    custemail = _custEmail,
                    firstname = _custFirstName,
                    lastname = _custLastName,
                    paymethod = _payMethod,
                    lineitem = _lineitem,
                    productid = _productid,
                    isbn = _externalRef,
                    unitprice = uamount,
                    quantity = _quantity,
                    testorder = _testorder,
                    postalcode = _postalcode,
                    store = _hcstoreid,
                    billpostalCode = _billPostal,
                    source = this.source,
                    auser = myuser,
                    url = _downUrl
                };
                this.source.auser = myuser;
                if (!String.IsNullOrEmpty(_order.isbn))
                {
                    this.itemorders.Add(_order);
                }
            } // lineItems loop
            return true;
        } // Parse

    } // public class RegParser
} // namespace AdobeReg.Utility
