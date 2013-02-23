using System;
using System.Net;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace SilverlightApplication3
{

    public class Catalog
    {
        private static List<Product> products;
        private static Random random = new Random();
        
        public Catalog()
        {
            XDocument productList = XDocument.Load("products.xml");
            
            products = (from objProduct in productList.Element("products").Elements("product")
                                      select new Product
                                      {
                                          Id = int.Parse(objProduct.Element("tradegood").Attribute("id").Value),
                                          ProductName = objProduct.Element("tradegood").Value,
                                          BasePrice = double.Parse(objProduct.Element("baseprice").Value),
                                          MaxQuantity = int.Parse(objProduct.Element("quantity").Value),
                                          PurchaseDMs = objProduct.Element("purchaseDMs").Elements("mod")
                                                        .ToDictionary(e => e.Attribute("type").Value, e => Convert.ToInt32(e.Value)),
                                          ResaleDMs = objProduct.Element("purchaseDMs").Elements("mod")
                                                        .ToDictionary(e => e.Attribute("type").Value, e => Convert.ToInt32(e.Value))
                                      }).ToList();
         }

        public int Quantity(int maxQuantity)
        {
            return random.Next(1, (maxQuantity + 1));
        }

        public double ActualValueModifier(Dictionary<string, int> modifier, string planetType)
        {
            int dieRoll = random.Next(2, 13) - modifier.FirstOrDefault(item => item.Key == planetType).Value;
            dieRoll = (dieRoll < 2) ? 2 : dieRoll;
            switch (dieRoll)
            {
                case 2:
                    return .40;
                case 3:
                    return .50;
                case 4:
                    return .70;
                case 5:
                    return .80;
                case 6:
                    return .90;
                case 7:
                    return 1.00;
                case 8:
                    return 1.10;
                case 9:
                    return 1.20;
                case 10:
                    return 1.30;
                case 11:
                    return 1.50;
                case 12:
                    return 1.70;
                case 13:
                    return 2.00;
                case 14:
                    return 3.00;
                default:
                    return 4.00;
            }

        }

        public Product GetProductById(int id)
        {
            Product p = products.FirstOrDefault(item => item.Id == id);
            return p;
        }

        public Product GetRandomMarketProduct(string planetType)
        {
            int id = int.Parse(random.Next(1, 7).ToString() + random.Next(1, 7).ToString());
            Product p = products.FirstOrDefault(item => item.Id == id);
            p.QuantityAvailable = Quantity(p.MaxQuantity);
            double modifier = ActualValueModifier(p.PurchaseDMs, planetType);
            p.ActualValue = p.BasePrice * modifier * p.QuantityAvailable;
            return p;
        }
    }
}
