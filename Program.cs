using PRG2_Grp_project;



Dictionary<string,Flight> flightDict = new Dictionary<string,Flight>();

//airlineDict is a PLACEHOLDER
Dictionary<string,Airline> airlineDict = new Dictionary<string,Airline>();

//task 1
//open Airlines.csv file
string[] airlinesStrings = File.ReadAllLines("airlines.csv");
foreach (string data in airlinesStrings)
{
    string[] splitData = data.Split(",");
    //Flight is currently null since there is no flight
    Airline airline = new Airline(splitData[0], splitData[1], new Dictionary<string, Flight>());
    airlineDict.Add(splitData[0], airline);
}
Console.WriteLine($"{airlineDict.Count()} Boarding Gates Loaded!");

//Load BoardingGates
Console.WriteLine("Loading Boarding Gates...");
Dictionary<string,BoardingGate> boardingGateDict = new Dictionary<string,BoardingGate>();
string[] boardingGateStrings = File.ReadAllLines("boardinggates.csv");
foreach (string data in boardingGateStrings)
{
    string[] splitData = data.Split(",");
    //Flight is currently null since there is no flight
    BoardingGate boardingGate = new BoardingGate(splitData[0], Convert.ToBoolean(splitData[1]), Convert.ToBoolean(splitData[2]), Convert.ToBoolean(splitData[3]), null);
    boardingGateDict.Add(splitData[0], boardingGate);
}
Console.WriteLine($"{boardingGateDict.Count()} Boarding Gates Loaded!");


//TASK 2

//Load Flights
Console.WriteLine("Loading Airlines...");
string[] flightStrings = File.ReadAllLines("flights.csv");
foreach (string data in flightStrings)
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


