using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using System.Xml;
//using System.Xml.Linq;
//using System.Linq;

namespace SilverlightApplication3
{
    public class Product
    {
        private string p_ProductName;
        private int p_BasePrice;
        private int p_Quantity;
        private double p_ActualValue;
        private int p_id;

        public string ProductName
        {
            get { return p_ProductName; }
            set { p_ProductName = value; }
        }
        public int BasePrice
        {
            get { return p_BasePrice; }
            set { p_BasePrice = value; }
        }
        public int MaxQuantity
        {
            get { return p_Quantity; }
            set { p_Quantity = value; }
        }
        public double ActualValue
        {
            get { return p_ActualValue; }
            set { p_ActualValue = value; }
        }
        public int Id
        {
            get { return p_id; }
            set { p_id = value; }
        }

        public Dictionary<string, int> ResaleDMs;
        public Dictionary<string, int> PurchaseDMs;


    }
}
