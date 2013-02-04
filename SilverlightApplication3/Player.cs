using System;
using System.Net;

namespace SilverlightApplication3
{
    public class Player
    {

        #region Public Fields
        public string Name { get; private set; }

        public double Bank { get; private set; }
        #endregion

        //constructor
        public Player(string playerName, double startingBank)
        {
            Name = playerName;
            Bank = startingBank;
        }

        #region Public Methods
        public void Purchase(double price)
        {
            Bank -= price;
        }

        public void Sale(double price)
        {
            Bank += price;
        }
        #endregion
    }
}
