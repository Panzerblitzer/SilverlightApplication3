using System;
using System.Net;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml;
using System.Linq;

namespace SilverlightApplication3
{
    public enum TradeClass {Ag, NonAg, Ind, NonInd, Rich, Poor}

    public class Planet
    {
        private String p_Name;
        private String p_UPC;
        private String p_Location;
        private static Collection<string> namesList;
        private static Random random = new Random();

        #region Private UPC Integers
        private int i_Starport;
        private int i_Size;
        private int i_Atmosphere;
        private int i_Hydrographics;
        private int i_Population;
        private int i_LawLevel;
        private int i_Government;
        private int i_TechLevel;
        #endregion

        #region Private UPC Chars
        private char Starport;
        private char NavalBase;
        private char ScoutBase;
        private char GasGiant;
        private char Size;
        private char Atmosphere;
        private char Hydrographics;
        private char Population;
        private char LawLevel;
        private char Government;
        private char TechLevel;
        private char TradeClass;
        private char TravelZone;
        #endregion

        #region Public fields
        public String Name
        {
            get { return p_Name; }
            set { p_Name = value; }
        }

        public String UPC
        {
            get { return p_UPC; }
            set { p_UPC = value; }
        }

        public String Location
        {
            get { return p_Location; }
            set { p_Location = value; }
        }

        public String HexLocation
        {
            get { return Location.Insert(1, ", "); }
        }

        #endregion

        #region Constructors
        public Planet(string hexNumber)
        {
            UPC = buildUPC();
            Name = getName();
            Location = hexNumber.ToString();
            //System.Diagnostics.Debug.WriteLine(Location + " " + Name + " " + UPC);
        }

        static Planet()
        {
            namesList = new Collection<string>();
            XmlReader reader = XmlReader.Create("planetNames.xml");
            reader.MoveToContent();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "name")
                {
                    namesList.Add(reader.ReadInnerXml());
                }
                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "names")
                {
                    break;
                }
            }
            reader.Close();
        }
        #endregion

        #region Initialization methods
        //methods to generate public fields

        private String buildUPC()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(p_Starport());
            sb.Append('-');
            sb.Append(p_Size());
            sb.Append(p_Atmosphere());
            sb.Append(p_Hydrographics());
            sb.Append(p_Population());
            sb.Append(p_Government());
            sb.Append(p_LawLevel());
            sb.Append(' ');
            sb.Append(p_TechLevel());
            sb.Append(' ');
            sb.Append(p_NavalBase(Starport));
            sb.Append(p_ScoutBase(Starport));
            sb.Append(p_GasGiant());
            sb.Append(' ');
            TravelZone = p_TravelZone();
            sb.Append(TravelZone);
            sb.Append(' ');
            TradeClass = p_TradeClass();
            sb.Append(TradeClass);

            return sb.ToString();
        }

        #endregion

        #region UPC & Stat generation
        //methods to generate the UPC

        private char p_Starport(){
            i_Starport = random.Next(2, 13);

            switch (i_Starport)
            {
                case 2:
                case 3:
                case 4:
                    return Starport = 'A';
                case 5:
                case 6:
                    return Starport = 'B';
                case 7:
                case 8:
                    return Starport = 'C';
                case 9:
                    return Starport = 'D';
                case 10:
                case 11:
                    return Starport = 'E';
                default:
                    return Starport = 'X';
            }
        }

        private char p_ScoutBase(char starportType)
        {
            int roll = random.Next(2, 13);


            switch (starportType){
                case 'A':
                    return ScoutBase = ((roll-3) > 7) ? 'S' : '-';
                case 'B':
                    return ScoutBase = ((roll - 2) > 7) ? 'S' : '-';
                case 'C':
                    return ScoutBase = ((roll - 1) > 7) ? 'S' : '-';
                case 'D':
                    return ScoutBase = (roll > 7) ? 'S' : '-';
                default:
                    return ScoutBase = '-';
            }
        }

        private char p_NavalBase(char starPortType)
        {
            if (i_Starport < 7)
            {
                int roll = random.Next(2, 13);
                return NavalBase = (roll > 7) ? 'N' : '-';
            }
            else
            {
                return NavalBase = '-';
            }
        }

        private char p_GasGiant()
        {
            int roll = random.Next(2, 13);
            return GasGiant = (roll < 10) ? 'G' : '-';
        }

        private char p_Size()
        {
            i_Size = random.Next(0, 11);
            return Size = intToChar(i_Size);
        }

        private char p_Atmosphere()
        {
            i_Atmosphere = ((random.Next(2, 13) - 7) + i_Size);
            i_Atmosphere = (i_Atmosphere < 0) ? 0 : i_Atmosphere;
            i_Atmosphere = (i_Atmosphere > 12) ? 12 : i_Atmosphere;
            //return Atmosphere = (char)i_Atmosphere.ToString();
            return Atmosphere = intToChar(i_Atmosphere);
        }

        private char p_Hydrographics()
        {
            i_Hydrographics = ((random.Next(2, 13) - 7) + i_Atmosphere);
            //exotic atmosphere - 4
            i_Hydrographics = ((i_Atmosphere <= 1) | (i_Atmosphere > 9)) ? i_Hydrographics - 4 : i_Hydrographics;
            //less than 0 = 0 or size <= 0 means no water
            i_Hydrographics = ((i_Hydrographics < 0) | (i_Size <= 0)) ? 0 : i_Hydrographics;
            //anything greater than 10 = A
            return Hydrographics = (i_Hydrographics < 10) ? intToChar(i_Hydrographics) : 'A';
        }

        private char p_Population()
        {
            i_Population = ((random.Next(2, 13)) - 2);
            return Population = (i_Population < 10) ? intToChar(i_Population) : 'A';
        }

        private char p_Government()
        {
            i_Government = ((random.Next(2, 13) - 7) + i_Population);
            i_Government = (i_Government < 0) ? 0 : i_Government;

            return Government = intToChar(i_Government);
        }

        private char p_LawLevel()
        {
            i_LawLevel = ((random.Next(2, 13) - 7) + i_Government);
            i_LawLevel = (i_LawLevel < 0) ? 0 : i_LawLevel;
            i_LawLevel = (i_LawLevel > 9) ? 9 : i_LawLevel;
            return LawLevel = intToChar(i_LawLevel);
        }

        private char p_TechLevel()
        {
            i_TechLevel = random.Next(1, 7);
            //Starport dm
            switch (Starport)
            {
                case 'A':
                    i_TechLevel = i_TechLevel + 6;
                    break;
                case 'B':
                    i_TechLevel = i_TechLevel + 4;
                    break;
                case 'C':
                    i_TechLevel = i_TechLevel + 2;
                    break;
                case 'X':
                    i_TechLevel = i_TechLevel - 4;
                    break;
                default:
                    break;
            }
            //Size dm
            i_TechLevel = (i_Size < 5) ? i_TechLevel + 1 : i_TechLevel;
            i_TechLevel = (i_Size < 2) ? i_TechLevel + 1 : i_TechLevel;
            //Atm dm
            i_TechLevel = ((i_Atmosphere < 4) | (i_Atmosphere > 9)) ? i_TechLevel + 1 : i_TechLevel;
            //Hydro dm
            i_TechLevel = (i_Hydrographics > 8) ? i_TechLevel + 1 : i_TechLevel;
            i_TechLevel = (i_Hydrographics > 9) ? i_TechLevel + 1 : i_TechLevel;
            //Pop dm
            i_TechLevel = ((i_Population < 6) & (i_Population > 0)) ? i_TechLevel + 1 : i_TechLevel;
            i_TechLevel = (i_Population == 9) ? i_TechLevel + 2 : i_TechLevel;
            i_TechLevel = (i_Population > 9) ? i_TechLevel + 4 : i_TechLevel;
            //Government dm
            switch (Government)
            {
                case '0':
                    i_TechLevel = i_TechLevel + 1;
                    break;
                case '5':
                    i_TechLevel = i_TechLevel + 1;
                    break;
                case 'D':
                    i_TechLevel = i_TechLevel - 2;
                    break;
                default:
                    break;
            }

            return TechLevel = intToChar(i_TechLevel);
        }

        private char p_TravelZone()
        {
            if ((i_Starport > 11) || (i_LawLevel == 9))
            {
                if (i_Starport <= 9)
                {
                    return 'A';
                }
                else
                {
                    return 'R';
                }
            }
            else
            {
                //High Government Type and High Law Level
                if ((i_Government > 9) && (i_LawLevel >= 8) && (i_Starport >= 9))
                {
                    switch(i_Government){
                        case(10):
                        case(11):
                        case(12):
                            return 'A';
                        default:
                            return 'R';
                    }
                }
                else
                {
                    return 'G';
                }
            }
        }

        private char p_TradeClass()
        {
            if ((i_Atmosphere > 3) && (i_Atmosphere < 10) && (i_Hydrographics > 3) && (i_Hydrographics < 9) && (i_Population > 4) && (i_Population < 8))
            {
                return 'A';
            }
            else if ((i_Atmosphere < 4) && (i_Hydrographics < 4) && (i_Population > 5))
            {
                return 'a';
            }
            else if (((i_Atmosphere < 3) || (i_Atmosphere == 4) || (i_Atmosphere == 7) || (i_Atmosphere == 9)) && (i_Population > 8))
            {
                return 'I';
            }
            else if (i_Population < 6)
            {
                return 'i';
            }
            else if ((i_Government > 3) && (i_Government < 10) && ((i_Atmosphere == 8) || (i_Atmosphere == 6)) && (i_Population < 9) && (i_Population > 5))
            {
                return 'R';
            }
            else if ((i_Atmosphere > 1) && (i_Atmosphere < 6) && (i_Hydrographics < 4))
            {
                return 'P';
            }
            else
            {
                return '-';
            }
        }

        private string getName()
        {
            int index = random.Next(0, namesList.Count - 1);
            string planetName = namesList.ElementAt(index);
            namesList.RemoveAt(index);
            return planetName;
        }

        #endregion

        //utility method to convert int to UPC friendly char
        private char intToChar(int intToConvert)
        {
            
            //double check if between 0 and 9 inclusive return char
            if ((intToConvert < 10) && (intToConvert >= 0))
            {
                char returnable;
                Char.TryParse(intToConvert.ToString(), out returnable);
                return returnable;
            }
            else if (intToConvert < 0) //convert less than 0 to 0
            {
                return '0';
            }
    
            //convert number greater than 10 to char
            switch (intToConvert)
            {
                case (10):
                    return 'A';
                case (11):
                    return 'B';
                case (12):
                    return 'C';
                case (13):
                    return 'D';
                case (14):
                    return 'F';
                case (15):
                    return 'G';
                case (16):
                    return 'H';
                case (17):
                    return 'I';
                case (18):
                    return 'J';
                case (19):
                    return 'K';
                case (20):
                    return 'L';
                case (21):
                    return 'M';
                case (22):
                    return 'N';
                case (23):
                    return 'O';
                default:
                    return 'Q';
            }
        }
        
    }
}
