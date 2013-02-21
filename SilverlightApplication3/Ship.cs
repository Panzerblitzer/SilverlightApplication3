using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


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
        public ObservableCollection<Product> Cargo;
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

        public void loadCargo(int cargoTons, Product lineItem)
        {
            CargoHold += cargoTons;
            Cargo.Add(lineItem);
        }

        public void unloadCargo(int cargoTons, Product lineItem)
        {
            CargoHold -= cargoTons;
            Cargo.Remove(lineItem);
        }
        #endregion
    }
}
