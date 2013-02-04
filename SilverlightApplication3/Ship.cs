using System;
using System.Collections.Generic;


namespace SilverlightApplication3
{
    public class Ship
    {
        #region Public Fields
        public string Name { get; private set; }
        public int CargoCapacity { get; private set; }
        public int CargoHold { get; set; }
        public int FuelCapacity { get; private set; }
        public int Fuel { get; private set; }
        public string Location { get; set; }
        public List<Product> Cargo;
        #endregion

        #region Constructors
        public Ship(string startLocation, int cargoCap, int fuelCap)
        {
            Location = startLocation;
            CargoCapacity = cargoCap;
            FuelCapacity = fuelCap;
        }

        public Ship(int cargoCap, int fuelCap)
        {
            CargoCapacity = cargoCap;
            FuelCapacity = fuelCap;
        }
        #endregion

        #region Public Methods

        public void Travel(string destination, int fuelTons)
        {
            Location = destination;
            Fuel -= fuelTons;
        }

        public void Refuel(int fuelTons)
        {
            Fuel += fuelTons;
        }

        public void loadCargo(int cargoTons)
        {
            CargoHold += cargoTons;
        }

        public void unloadCargo(int cargoTons)
        {
            CargoHold -= cargoTons;
        }
        #endregion
    }
}
