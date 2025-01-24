using System.Buffers.Text;
using PRG2_Grp_project;



Dictionary<string,Flight> flightDict = new Dictionary<string,Flight>();

//airlineDict is a PLACEHOLDER
Dictionary<string,Airline> airlineDict = new Dictionary<string,Airline>();

//task 1
//open Airlines.csv file
Console.WriteLine("Loading Airlines...");
string[] airlinesStrings = File.ReadAllLines("airlines.csv"); //load the airlines.csv file
foreach (string data in airlinesStrings.Skip(1))
{
    string[] splitData = data.Split(",");
    //Flight is currently null since there is no flight
    Airline airline = new Airline(splitData[0], splitData[1], new Dictionary<string, Flight>()); //create the Airline objects based on the data loaded
    airlineDict.Add(splitData[0], airline); //add the Airlines objects into an Airline Dictionary

}
Console.WriteLine($"{airlineDict.Count()} Airlines Loaded!");

//Load BoardingGates
Console.WriteLine("Loading Boarding Gates...");
Dictionary<string,BoardingGate> boardingGateDict = new Dictionary<string,BoardingGate>(); //load the boardinggates.csv file
string[] boardingGateStrings = File.ReadAllLines("boardinggates.csv");
foreach (string data in boardingGateStrings.Skip(1))
{
    string[] splitData = data.Split(",");
    //Flight is currently null since there is no flight
    BoardingGate boardingGate = new BoardingGate(splitData[0], Convert.ToBoolean(splitData[1]), Convert.ToBoolean(splitData[2]), Convert.ToBoolean(splitData[3]), null); //create the Boarding Gate objects based on the data loaded
    boardingGateDict.Add(splitData[0], boardingGate); //add the Boarding Gate objects into a Boarding Gate dictionary
}
Console.WriteLine($"{boardingGateDict.Count()} Boarding Gates Loaded!");


//TASK 2

//Load Flights
Console.WriteLine("Loading Airlines...");
string[] flightStrings = File.ReadAllLines("flights.csv");
foreach (string data in flightStrings.Skip(1))
{
    string[] splitData = data.Split(",");
    //Initalise Flight object
    Flight flight = null;
    try
    {
        if (splitData[4] == "LWTT")
        {
            flight = new LWTTFlight(splitData[0], splitData[1], splitData[2], Convert.ToDateTime(splitData[3]), null);
        }
        else if (splitData[4] == "DDJB")
        {
            flight = new DDJBFlight(splitData[0], splitData[1], splitData[2], Convert.ToDateTime(splitData[3]), null);
        }
        else if (splitData[4] == "CFFT")
        {
            flight = new CFFTFlight(splitData[0], splitData[1], splitData[2], Convert.ToDateTime(splitData[3]), null);
        }
    }
    catch (IndexOutOfRangeException) //Since index 4 of split data is non-existent in normal flights
    {
        flight = new NORMFlight(splitData[0], splitData[1], splitData[2], Convert.ToDateTime(splitData[3]), null);
    }

    flightDict.Add(splitData[0], flight);
}
Console.WriteLine($"{flightDict.Count()} Airlines Loaded!");




//TASK 3
//Header

Console.WriteLine($"{"Flight Number",-15}{"Airline Name",-25}{"Origin",-25}{"Destination",-25}{"Expected Departure/Arrival Time",-30}");

foreach (Flight flight in flightDict.Values)
{
    string airlineName = "";
    foreach (Airline airline in airlineDict.Values)
    {
        //error bc task 1 not done, Airlines not initalised
        if (flight.FlightNumber.Substring(0, 2) == airline.Code)
        {
            airlineName = airline.Name;
            break;
        }
    }
    Console.WriteLine($"{flight.FlightNumber,-15}{airlineDict[flight.FlightNumber.Substring(0,2)].Name,-25}{flight.Origin,-25}{flight.Destination,-25}{flight.ExpectedTime.ToString("d/M/yyyy h:mm:ss tt"),-30}");
    //day/month/year hour:minute:seconds am/pm
}

//task 4
Console.WriteLine("=============================================\nList of Boarding Gates for Changi Airport Terminal 5\n=============================================");
Console.WriteLine("Gate Name\tDDJB\t\tCFFT\t\tLWTT");
foreach (BoardingGate bg in boardingGateDict.Values)
{
    Console.WriteLine($"{bg.GateName}\t\t{bg.SupportsDDJB}\t\t{bg.SupportsCFFT}\t\t{bg.SupportsLWTT}");
}

//TASK 5
Console.WriteLine("=============================================\r\nAssign a Boarding Gate to a Flight\r\n=============================================\r\n");
Console.Write("Enter Flight Number:\n");
string flightNumber = Console.ReadLine();
Flight selectedFlight = flightDict[flightNumber];

void BoardingGateToFlight(Flight selectedFlight)
{
    Console.Write("Enter Boarding Gate Name:\n");
    string boardingGateNumber = Console.ReadLine();

    if (boardingGateDict[boardingGateNumber] == null)
    {
        //Data from Flight
        Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
        Console.WriteLine($"Origin: {selectedFlight.Origin}");
        Console.WriteLine($"Destination: {selectedFlight.Destination}");
        Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime}");
        //HOW TO DO DIFFERENTIATE THE CODES
        if (selectedFlight.Status == null)
        {
            Console.WriteLine($"Special Request Code: None");
        }
        else {Console.WriteLine($"Special Request Code: {selectedFlight.Status}");}
        //THIS PART ON TOP WRONG


        //Search BoardingGate using boardingGateNumber
        BoardingGate selectedBoardingGate = boardingGateDict[boardingGateNumber]; 
        //Data from Boarding Gate
        Console.WriteLine($"Boarding Gate Name: {selectedBoardingGate.GateName}");
        Console.WriteLine($"Supports DDJB: {selectedBoardingGate.SupportsDDJB}");
        Console.WriteLine($"Supports CFFT: {selectedBoardingGate.SupportsCFFT}");
        Console.WriteLine($"Supports LWTT: {selectedBoardingGate.SupportsLWTT}");

        //Update flight status
        void UpdateFlightStatus()
        {
            Console.Write("Would you like to update the status of the flight? (Y/N)\n");
            string updateFlightStatus = Console.ReadLine();
            if (updateFlightStatus == "Y")
            {
                //Choose status of the flight
                Console.WriteLine("1. Delayed\r\n2. Boarding\r\n3. On Time");
                Console.Write("Please select the new status of the flight:");
                string statusOption = Console.ReadLine();

                if (statusOption == "1")
                {
                    flightDict[flightNumber].Status = "Delayed";
                }
                else if (statusOption == "2")
                {
                    flightDict[flightNumber].Status = "Boarding";
                }
                else if (statusOption == "3")
                {
                    flightDict[flightNumber].Status = "On Time";
                }
                else
                {
                    Console.WriteLine("Invalid Option");
                    //Maybe can prompt user for status again
                }


            }
            else if (updateFlightStatus == "N")
            {

            }
            else
            {
                Console.WriteLine("Invalid Option");
                UpdateFlightStatus();
            }
        }
        UpdateFlightStatus();


        //Assign Flight to Boarding Gate
        boardingGateDict[boardingGateNumber].Flight = selectedFlight;
        Console.WriteLine($"Flight {selectedFlight} has been assigned to Boarding Gate {selectedBoardingGate}!");

    }
    else
    {
        //Asks the user for Boarding Gate again
        Console.WriteLine("Boarding Gate is already assigned");
        BoardingGateToFlight(selectedFlight);
    }

}
BoardingGateToFlight(selectedFlight);




//TASK 6
void CreateNewFlight()
{
    Console.Write("New Flight details(comma separated): ");
    string newFlightDetails = Console.ReadLine();
    string[] splitNewFlightDetails = newFlightDetails.Split(",");
    Console.Write("Additional information: ");
    string extraNewFlightDetails = Console.ReadLine();
    Flight newFlight = null;
    //Bool for valid info
    bool validInfo = true;
    //Error Handling
    try
    {
        if (extraNewFlightDetails == "")
        {
            newFlight = new NORMFlight(splitNewFlightDetails[0], splitNewFlightDetails[1], splitNewFlightDetails[2], Convert.ToDateTime(splitNewFlightDetails[3]), null);
        }
        else if (extraNewFlightDetails == "LWTT")
        {
            newFlight = new LWTTFlight(splitNewFlightDetails[0], splitNewFlightDetails[1], splitNewFlightDetails[2], Convert.ToDateTime(splitNewFlightDetails[3]), extraNewFlightDetails);
        }
        else if (extraNewFlightDetails == "DDJB")
        {
            newFlight = new DDJBFlight(splitNewFlightDetails[0], splitNewFlightDetails[1], splitNewFlightDetails[2], Convert.ToDateTime(splitNewFlightDetails[3]), extraNewFlightDetails);
        }
        else if (extraNewFlightDetails == "CFFT")
            newFlight = new CFFTFlight(splitNewFlightDetails[0], splitNewFlightDetails[1], splitNewFlightDetails[2], Convert.ToDateTime(splitNewFlightDetails[3]), extraNewFlightDetails);
        else { validInfo = false; }
    }

    catch (Exception ex) { validInfo = false; }

    //Add to dictionary and CSV file
    if (validInfo == true)
    {
        flightDict.Add(newFlight.FlightNumber, newFlight);
        using (StreamWriter writer = new StreamWriter("flights.csv", append: true))
        {
            writer.WriteLine($"{newFlight.FlightNumber},{newFlight.Origin},{newFlight.Destination},{newFlight.ExpectedTime},{newFlight.Status}");
        }

    }
    else
    {
        Console.WriteLine("Invalid Information");
    }

    //Prompt for user if they want to add another flight
    while (true)
    {
        Console.Write("Would you like to add another Flight?(Y/N)");
        string addAnotherFlightOption = Console.ReadLine();
        if (addAnotherFlightOption == "Y")
        {
            CreateNewFlight();
            break;
        }
        else if (addAnotherFlightOption == "N")
        {
            Console.WriteLine("Flight(s) have been successfully added");
        }

    }
}

//Call method
CreateNewFlight();

//task 7
Console.WriteLine("=============================================\nList of Airlines for Changi Airport Terminal 5\n=============================================");
Console.WriteLine("Airline Code\tAirline Name");
foreach (Airline al in airlineDict.Values) //list all the Airlines available
{
    Console.WriteLine($"{al.Code}\t\t{al.Name}");
}

Console.Write("Enter Airline Code: "); //prompt the user to enter the 2-Letter Airline Code (e.g. SQ or MH, etc.)
string searchCode = Console.ReadLine();
Console.Write($"=============================================\nList of Flights for "); //prompt the user to enter the 2-Letter Airline Code (e.g. SQ or MH, etc.)
Airline searchAirline = null;
foreach (Airline al in airlineDict.Values)
    if (al.Code == searchCode) { 
        searchAirline = al; //retrieve the Airline object selected
        Console.Write(searchAirline.Name);
}
Console.Write("\n=============================================");
Console.WriteLine("\nFlight Number\tAirline Name\t\tOrigin\t\t\tDestination\t\tExpectedDeparture/Arrival Time");
foreach (Flight f in flightDict.Values) //for each Flight from that Airline, show their Airline Number, Origin and Destination
{
    if (f != null)
    {
        if (new string(f.FlightNumber.Take(2).ToArray()) == searchCode)
        {
            Console.WriteLine($"{f.FlightNumber}\t\t{searchAirline.Name}\t{f.Origin}\t\t{f.Destination}\t\t{f.ExpectedTime}");
        }
    }
}

Console.Write("Select a Flight Number: "); //prompt the user to select a Flight Number
string searchFlightNum = Console.ReadLine();
Flight searchFlight;
foreach (Flight f in flightDict.Values)
{
    if (f != null)
    {
        if (f.FlightNumber == searchFlightNum)
        {
            searchFlight = f; //retrieve the Flight object selected
            Console.WriteLine($"Flight Number:\t\t\t\t{f.FlightNumber}"); //display the following Flight details, which are all the flight specifications
            Console.WriteLine($"Airline Name:\t\t\t\t{searchAirline}");
            Console.WriteLine($"Origin:\t\t\t\t\t{f.Origin}");
            Console.WriteLine($"Destination:\t\t\t\t{f.Destination}");
            Console.WriteLine($"Expected Departure/Arrival Time:\t{f.ExpectedTime}");
        }
    }
}


//task 8

Console.WriteLine("=============================================\nList of Airlines for Changi Airport Terminal 5\n=============================================");
Console.WriteLine("Airline Code\tAirline Name");
foreach (Airline al in airlineDict.Values) //list all the Airlines available
{
    Console.WriteLine($"{al.Code}\t\t{al.Name}");
}

Console.Write("Enter Airline Code: "); //prompt the user to enter the 2-Letter Airline Code (e.g. SQ or MH, etc.)
string searchCode1 = Console.ReadLine();
Console.Write($"=============================================\nList of Flights for "); //prompt the user to enter the 2-Letter Airline Code (e.g. SQ or MH, etc.)
Airline searchAirline1 = null;
foreach (Airline al in airlineDict.Values)
    if (al.Code == searchCode1)
    {
        searchAirline1 = al; //retrieve the Airline object selected
        Console.Write(searchAirline1.Name);
    }
Console.Write("\n=============================================");
Console.WriteLine("\nFlight Number\tAirline Name\t\tOrigin\t\t\tDestination\t\tExpectedDeparture/Arrival Time");
foreach (Flight f in flightDict.Values) //for each Flight from that Airline, show their Airline Number, Origin and Destination
{
    if (f != null)
    {
        if (new string(f.FlightNumber.Take(2).ToArray()) == searchCode1)
        {
            Console.WriteLine($"{f.FlightNumber}\t\t{searchAirline1.Name}\t{f.Origin}\t\t{f.Destination}\t\t{f.ExpectedTime}");
        }
    }
}


Console.WriteLine("Choose an existing Flight to modify or delete: ");//prompt the user to select a Flight Number
string searchFlightNum1 = Console.ReadLine();
Flight searchFlight1 = null;
foreach (Flight f in flightDict.Values)
{
    if (f != null)
    {
        if (f.FlightNumber == searchFlightNum1)
        {
            searchFlight1 = f; //retrieve the Flight object selected
            break;
        }
    }
}
Console.Write("1. Modify Flight\n2. Delete Flight");
string choice = Console.ReadLine();
if (choice == "1")
{
    Console.Write("1. Modify Basic Information\r\n2. Modify Status\r\n3. Modify Special Request Code\r\n4. Modify Boarding Gate");
    string actionChoice = Console.ReadLine();
    if (actionChoice == "1")
    {
        Console.Write("\nEnter new Origin: ");
        string newOrigin = Console.ReadLine();
        searchFlight1.Origin = newOrigin;

        Console.Write("\nEnter new Destination: ");
        string newDestination = Console.ReadLine();
        searchFlight1.Destination = newDestination;

        Console.Write("\nEnter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
        string newETA = Console.ReadLine();
        searchFlight1.ExpectedTime = Convert.ToDateTime(newETA);

        flightDict[searchFlight1.FlightNumber] = searchFlight1;
        Console.WriteLine("Flight Updated!");

    }
    else if (choice == "2")
    {
        Console.Write("\nEnter new Status: ");
        string newStatus = Console.ReadLine();
        searchFlight1.Status = newStatus;

        flightDict[searchFlight1.FlightNumber] = searchFlight1;
        Console.WriteLine("Flight Updated!");
    }
    else if (choice == "3")
    {
        // add if have
    }
    else if (choice == "4") {
        //add if have
    }

    Console.WriteLine($"Flight Number: {searchFlight1.FlightNumber}");
    Console.WriteLine($"Airline Name: {searchAirline1}");
    Console.WriteLine($"Origin: {searchFlight1.Origin}");
    Console.WriteLine($"Destiantion: {searchFlight1.Destination}");
    Console.WriteLine($"Expected Departure/Arrival Time: {searchFlight1.ExpectedTime}");
    Console.WriteLine($"Status: {searchFlight1.Status}");
}
else if (choice == "0")
{
    Console.Write("Confirm Deletion? [Y/N]");
    string deleteChoice = Console.ReadLine();
    if (deleteChoice == "Y")
    {
        foreach (var kvp in flightDict)
        {
            if (kvp.Value == searchFlight1)
            {
                flightDict.Remove(kvp.Key);
            }
        }
    }


}


//TASK 9
Console.WriteLine("=============================================\r\nFlight Schedule for Changi Airport Terminal 5\r\n=============================================");
Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-20} {"Origin",-20} {"Destination",-20} {"Expected Time",-35} {"Status",-15} {"Boarding Gate",-15}");

//Make a copy of flightDict as a List for sorting
List<Flight> flightList = new List<Flight>(flightDict.Values);

//Selection Sort
for (int i = 0; i < flightList.Count; i++)
{
    for (int j = 0; j < flightList.Count; j++)
    {
        if (flightList[i].CompareTo(flightList[j]) == 1)
        {
            Flight placeholder = flightList[j];
            flightList[j] = flightList[i];
            flightList[i] = placeholder;


        }

    }
}

//NOT DONE, BOARDING GATE FEATURE NOT DONE
foreach(Flight flight in flightList)
{  
    string boardingGateName = "Unassigned";
    foreach (BoardingGate boardingGate in boardingGateDict.Values)
    {
        
        if (boardingGate.Flight == flight)
        {
            boardingGateName = boardingGate.GateName;
        }
    }
    if (flight.Status == null)
    {
        //AIRLINE DICT IDK THE NAME YET
        Console.WriteLine($"{flight.FlightNumber,-15} {airlineDict[flight.FlightNumber.Substring(0, 2)].Name,-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime,-35} {"Scheduled",-15} {boardingGateName,-15}");
    }
    else
    {
        Console.WriteLine($"{flight.FlightNumber,-15} {airlineDict[flight.FlightNumber.Substring(0, 2)].Name,-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime,-35} {flight.Status,-15} {boardingGateName,-15}");
    }
}