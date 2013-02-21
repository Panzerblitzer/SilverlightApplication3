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

            products = new List<Product>();
            XmlReader reader = XmlReader.Create("products.xml");
            reader.MoveToContent();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "product")
                {
                    Product product = new Product();

                    product.Id = int.Parse(reader.GetAttribute("id"));
                    reader.ReadToDescendant("tradegood");
                    product.ProductName = reader.ReadInnerXml();
                    reader.ReadToNextSibling("baseprice");
                    product.BasePrice = int.Parse(reader.ReadInnerXml());
                    
                    reader.ReadToNextSibling("purchaseDMs");
                    Dictionary<string, int> modifiers = new Dictionary<string, int>();
                    while (reader.ReadToDescendant("mod"))
                    {
                        modifiers.Add(reader.GetAttribute("type"), int.Parse(reader.ReadInnerXml()));
                    }
                    product.PurchaseDMs = modifiers;
                    modifiers.Clear();

                    reader.ReadToNextSibling("resaleDMs");
                    while (reader.ReadToDescendant("mod"))
                    {
                        modifiers.Add(reader.GetAttribute("type"), int.Parse(reader.ReadInnerXml()));
                    }
                    product.ResaleDMs = modifiers;

                    reader.ReadToNextSibling("quantity");
                    product.MaxQuantity = int.Parse(reader.ReadInnerXml());
                    
                    products.Add(product);
                }
                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "products")
                {
                    break;
                }
            }
            reader.Close();
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
            p.ActualValue = p.BasePrice * ActualValueModifier(p.PurchaseDMs, planetType) * p.QuantityAvailable;
            return p;
        }
    }
}
