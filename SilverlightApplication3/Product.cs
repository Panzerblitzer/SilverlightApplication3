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
        private double p_BasePrice;
        private int p_MaxQuantity;
        private double p_ActualValue;
        private double p_LotValue;
        private int p_id;
        private int p_Quantity;

        public string ProductName
        {
            get { return p_ProductName; }
            set { p_ProductName = value; }
        }
        public double BasePrice
        {
            get { return p_BasePrice; }
            set { p_BasePrice = value; }
        }
        public int MaxQuantity
        {
            get { return p_MaxQuantity; }
            set { p_MaxQuantity = value; }
        }
        public int QuantityAvailable 
        {
            get { return p_Quantity; }
            set { p_Quantity = value; }
        }
        public double ActualValue
        {
            get { return p_ActualValue; }
            set { p_ActualValue = value; }
        }
        public double LotValue
        {
            get { return p_LotValue; }
            set { p_LotValue = value; }
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
