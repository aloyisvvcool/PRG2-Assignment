using PRG2_Grp_project;
using System.Globalization;



//==========================================================
// Student Number : S10266029
// Student Name : Aloysius Luke Tay Shi Yuan
// Partner Name : Samuel Ng En Yi
//==========================================================

Dictionary<string, Flight> flightDict = new Dictionary<string, Flight>();
Dictionary<string, Airline> airlineDict = new Dictionary<string, Airline>();

//task 1
//open Airlines.csv file
string[] airlinesStrings = File.ReadAllLines("airlines.csv");
foreach (string data in airlinesStrings.Skip(1))
{
    string[] splitData = data.Split(",");
    //Flight is currently null since there is no flight
    Airline airline = new Airline(splitData[0], splitData[1], new Dictionary<string, Flight>());
    airlineDict.Add(splitData[0], airline);
}

//Load BoardingGates
Console.WriteLine("Loading Boarding Gates...");
Dictionary<string,BoardingGate> boardingGateDict = new Dictionary<string,BoardingGate>();
string[] boardingGateStrings = File.ReadAllLines("boardinggates.csv");
foreach (string data in boardingGateStrings.Skip(1))
{
    string[] splitData = data.Split(",");
    //Flight is currently null since there is no flight
    BoardingGate boardingGate = new BoardingGate(splitData[0], Convert.ToBoolean(splitData[1]), Convert.ToBoolean(splitData[2]), Convert.ToBoolean(splitData[3]), null);
    boardingGateDict.Add(splitData[0], boardingGate);
}
Console.WriteLine($"{boardingGateDict.Count()} Boarding Gates Loaded!");

/*
//TASK 2
//Load Flights
Console.WriteLine("Loading Airlines...");
string[] flightStrings = File.ReadAllLines("flights.csv");
foreach (string data in flightStrings.Skip(1))
{
    string[] splitData = data.Split(",");
    //Initalise Flight object
    Flight flight = null;

{
    if (splitData[4] == "LWTT")
    {
        flight = new LWTTFlight(splitData[0], splitData[1], splitData[2], Convert.ToDateTime(splitData[3]), null, 500);
    }
    else if (splitData[4] == "DDJB")
    {
        flight = new DDJBFlight(splitData[0], splitData[1], splitData[2], Convert.ToDateTime(splitData[3]), null, 300);
    }
    else if (splitData[4] == "CFFT")
    {
        flight = new CFFTFlight(splitData[0], splitData[1], splitData[2], Convert.ToDateTime(splitData[3]), null, 150);
    }
    else 
    {
        flight = new NORMFlight(splitData[0], splitData[1], splitData[2], Convert.ToDateTime(splitData[3]), null);
    }
}

    flightDict.Add(splitData[0], flight);
}
Console.WriteLine($"{flightDict.Count()} Flights Loaded!");


//DO NOT REMOVE, VARIABLE USED FOR TASK 3,9 AND ADVANCED
Dictionary<string, string> airlineKeyValuePairs = new Dictionary<string, string>();
foreach (string data in airlinesStrings)
{
    string[] splitData = data.Split(",");
    airlineKeyValuePairs.Add(splitData[1], splitData[0]);
}


//TASK 3 (Complete)
void TaskThree()
{
    Console.WriteLine("=============================================\r\nList of Flights for Changi Airport Terminal 5\r\n=============================================");
    Console.WriteLine($"{"Flight Number",-15}{"Airline Name",-25}{"Origin",-25}{"Destination",-25}{"Expected Departure/Arrival Time",-30}");
    //Create Dictionary mapping airline code to airline name
    foreach (Flight flight in flightDict.Values)
    {
        Console.WriteLine($"{flight.FlightNumber,-15}{airlineKeyValuePairs[flight.FlightNumber.Substring(0, 2)],-25}{flight.Origin,-25}{flight.Destination,-25}{flight.ExpectedTime.ToString("d/M/yyyy h:mm:ss tt").ToLower(),-30}");
        //day/month/year hour:minute:seconds am/pm
    }
    Console.WriteLine();
}




//TASK 5  (Input Validated)
void TaskFive()
{
    Console.WriteLine("=============================================\r\nAssign a Boarding Gate to a Flight\r\n=============================================\r\n");
    string flightNumber = "";
    Flight selectedFlight = null;
    while (true)
    {
        try
        {
            Console.Write("Enter Flight Number:\n");
            flightNumber = Console.ReadLine();
            selectedFlight = flightDict[flightNumber];
            break;
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("Invalid Flight Number");
        }
    }


    while (true)
    {
        //Initalise boardingGateNumber
        string boardingGateNumber = "";
        while (true)
        {
            Console.Write("Enter Boarding Gate Name:\n");
            boardingGateNumber = Console.ReadLine();
            if (boardingGateDict.ContainsKey(boardingGateNumber))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid Boarding Gate Name");
            }
        }



        if (boardingGateDict[boardingGateNumber].Flight == null)
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
            else 
            {
                if (selectedFlight is CFFTFlight)
                {
                    Console.WriteLine("Special Request Code: CFFT");
                }
                else if (selectedFlight is DDJBFlight)
                {
                    Console.WriteLine("Special Request Code: DDJB");
                }
                else if (selectedFlight is LWTTFlight)
                {
                    Console.WriteLine("Special Request Code: LWTT");
                }
                else if (selectedFlight is NORMFlight)
                {
                    Console.WriteLine("Special Request Code: None");
                }
            }


            //Search BoardingGate using boardingGateNumber
            BoardingGate selectedBoardingGate = boardingGateDict[boardingGateNumber];
            //Data from Boarding Gate
            Console.WriteLine($"Boarding Gate Name: {selectedBoardingGate.GateName}");
            Console.WriteLine($"Supports DDJB: {selectedBoardingGate.SupportsDDJB}");
            Console.WriteLine($"Supports CFFT: {selectedBoardingGate.SupportsCFFT}");
            Console.WriteLine($"Supports LWTT: {selectedBoardingGate.SupportsLWTT}");

            //Update flight status
            while (true)
            {
                Console.Write("Would you like to update the status of the flight? (Y/N)\n");
                string updateFlightStatus = Console.ReadLine();
                bool exit = false;
                if (updateFlightStatus == "Y")
                {


                    while (true)
                    {
                        //Choose status of the flight
                        Console.WriteLine("1. Delayed\r\n2. Boarding\r\n3. On Time");
                        Console.Write("Please select the new status of the flight:");
                        string statusOption = Console.ReadLine();
                        if (statusOption == "1")
                        {
                            flightDict[flightNumber].Status = "Delayed";
                            exit = true;
                            break;
                        }
                        else if (statusOption == "2")
                        {
                            flightDict[flightNumber].Status = "Boarding";
                            exit = true;
                            break;
                        }
                        else if (statusOption == "3")
                        {
                            flightDict[flightNumber].Status = "On Time";
                            exit = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Option");
                            //Maybe can prompt user for status again
                        }
                    }
                    break;

                }

                //If "N" then flight status is "On Time"
                else if (updateFlightStatus == "N")
                {
                    flightDict[flightNumber].Status = "On Time";
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Option");
                }
            }


            //Assign Flight to Boarding Gate
            boardingGateDict[boardingGateNumber].Flight = selectedFlight;
            Console.WriteLine($"Flight {selectedFlight.FlightNumber} has been assigned to Boarding Gate {selectedBoardingGate.GateName}!");
            break;

        }
        else
        {
            //Asks the user for Boarding Gate again
            Console.WriteLine("Boarding Gate is already assigned");
        }

    }
}





//TASK 6
void TaskSix()
{
    void AddFlight()
    {
        while (true)
        {
            Console.Write("Enter Flight Number: ");
            string flightNumber = Console.ReadLine();
            Console.Write("Enter Origin: ");
            string origin = Console.ReadLine();
            Console.Write("Enter Destination: ");
            string destination = Console.ReadLine();
            Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
            DateTime expectedDepartureOrArrivalTime = DateTime.ParseExact(Console.ReadLine(), "d/M/yyyy H:mm", null);
            Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
            string specialRequestCode = Console.ReadLine();

            //Console.Write("New Flight details(comma separated): ");
            //string newFlightDetails = Console.ReadLine();
            //string[] splitNewFlightDetails = newFlightDetails.Split(",");
            //Console.Write("Additional information: ");
            //string extraNewFlightDetails = Console.ReadLine();

            //Bool for valid info
            Flight newFlight = null;
            bool validInfo = true;
            //Error Handling
            try
            {
                if (specialRequestCode == "None")
                {
                    newFlight = new NORMFlight(flightNumber, origin, destination, expectedDepartureOrArrivalTime, null);
                }
                else if (specialRequestCode == "LWTT")
                {
                    newFlight = new LWTTFlight(flightNumber, origin, destination, expectedDepartureOrArrivalTime, null, 500);
                }
                else if (specialRequestCode == "DDJB")
                {
                    newFlight = new DDJBFlight(flightNumber, origin, destination, expectedDepartureOrArrivalTime, null, 300);
                }
                else if (specialRequestCode == "CFFT")
                    newFlight = new CFFTFlight(flightNumber, origin, destination, expectedDepartureOrArrivalTime, null, 150);
                else { validInfo = false; }
            }

            catch (Exception ex) { validInfo = false; }

            //Add to dictionary and CSV file
            if (validInfo == true)
            {
                flightDict.Add(newFlight.FlightNumber, newFlight);
                using (StreamWriter writer = new StreamWriter("flights.csv", append: true))
                {
                    writer.WriteLine($"{newFlight.FlightNumber},{newFlight.Origin},{newFlight.Destination},{newFlight.ExpectedTime},{specialRequestCode}");
                    break;
                }

            }
            else
            {
                Console.WriteLine("Invalid Information");
            }
        }
    }
    //Call the flight
    AddFlight();
    

    //Prompt for user if they want to add another flight
    while (true)
    {
        Console.Write("Would you like to add another Flight?(Y/N)");
        string addAnotherFlightOption = Console.ReadLine();
        if (addAnotherFlightOption == "Y")
        {
            AddFlight();
        }
        else if (addAnotherFlightOption == "N")
        {
            Console.WriteLine("Flight(s) have been successfully added");
            break;
        }
        else
        {
            Console.WriteLine("Invalid Input");
        }

    }
}





//TASK 9 (Complete)
void TaskNine()
{
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
    foreach (Flight flight in flightList)
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
            Console.WriteLine($"{flight.FlightNumber,-15} {airlineKeyValuePairs[flight.FlightNumber.Substring(0, 2)],-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime,-35} {"Scheduled",-15} {boardingGateName,-15}");
        }
        else
        {
            Console.WriteLine($"{flight.FlightNumber,-15} {airlineKeyValuePairs[flight.FlightNumber.Substring(0, 2)],-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime,-35} {flight.Status,-15} {boardingGateName,-15}");
        }
    }
}





//ADVANCED (a)
void AdvancedTaskA()
{
    Queue<Flight> flightWithoutBoardingGateQueue = new Queue<Flight>();
    List<BoardingGate> freeBoardingGates = new List<BoardingGate>();
    List<string> flightsWithBoardingGateCode = new List<string>();
    //Make a copy of flightDict as a List for sorting
    List<Flight> flightList = new List<Flight>(flightDict.Values);

    //Create list of flightCodes wiht boarding gates
    foreach (BoardingGate boardingGate in boardingGateDict.Values)
    {
        if (boardingGate.Flight != null)
        {
            flightsWithBoardingGateCode.Add(boardingGate.Flight.FlightNumber);
        }
        else
        {
            freeBoardingGates.Add(boardingGate);
        }
    }

    foreach (Flight flight in flightList)
    {
        if (!flightsWithBoardingGateCode.Contains(flight.FlightNumber))
        {
            flightWithoutBoardingGateQueue.Enqueue(flight);
        }
    }

    Console.WriteLine($"Total number of Flights without Boarding Gate: {flightWithoutBoardingGateQueue.Count()}");
    Console.WriteLine($"Total Number of unassigned Boarding Gates: {freeBoardingGates.Count()}");

    Dictionary<string, string> specialCodeDict = new Dictionary<string, string>();
    //Dictionary to map code to special request
    for (int i = 1; i < flightStrings.Count(); i++) //Skip header
    {
        Console.WriteLine(flightStrings[i]);
        string code = "";
        try
        {
            code = flightStrings[i].Split(",")[4];
        }
        catch
        {
        }

        specialCodeDict.Add(flightStrings[i].Split(",")[0], code);
    }


    for (int i = 1; i < flightWithoutBoardingGateQueue.Count + 1; i++)
    {
        Flight flight = flightWithoutBoardingGateQueue.Dequeue();
        if (specialCodeDict[flight.FlightNumber] == "CFFT")
        {
            foreach (BoardingGate boardingGate in freeBoardingGates)
            {
                if (boardingGate.SupportsCFFT == true)
                {
                    boardingGateDict[boardingGate.GateName].Flight = flight;
                    freeBoardingGates.Remove(boardingGate);
                    Console.WriteLine($"{flight.FlightNumber,-15} {airlineKeyValuePairs[flight.FlightNumber.Substring(0, 2)],-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime,-35} {"CFFT",-15} {boardingGate.GateName,-15}");
                    break;
                }
            }
        }

        else if (specialCodeDict[flight.FlightNumber] == "DDJB")
        {
            foreach (BoardingGate boardingGate in freeBoardingGates)
            {
                if (boardingGate.SupportsDDJB == true)
                {
                    boardingGateDict[boardingGate.GateName].Flight = flight;
                    freeBoardingGates.Remove(boardingGate);
                    Console.WriteLine($"{flight.FlightNumber,-15} {airlineKeyValuePairs[flight.FlightNumber.Substring(0, 2)],-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime,-35} {"DDJB",-15} {boardingGate.GateName,-15}");
                    break;
                }
            }
        }

        else if (specialCodeDict[flight.FlightNumber] == "LWTT")
        {
            foreach (BoardingGate boardingGate in freeBoardingGates)
            {
                if (boardingGate.SupportsLWTT == true)
                {
                    boardingGateDict[boardingGate.GateName].Flight = flight;
                    freeBoardingGates.Remove(boardingGate);
                    Console.WriteLine($"{flight.FlightNumber,-15} {airlineKeyValuePairs[flight.FlightNumber.Substring(0, 2)],-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime,-35} {"LWTT",-15} {boardingGate.GateName,-15}");
                    break;
                }
            }
        }

        else
        {
            foreach (BoardingGate boardingGate in freeBoardingGates)
            {
                if (boardingGate.SupportsLWTT == false && boardingGate.SupportsDDJB == false && boardingGate.SupportsCFFT == false)
                {
                    boardingGateDict[boardingGate.GateName].Flight = flight;
                    freeBoardingGates.Remove(boardingGate);
                    Console.WriteLine($"{flight.FlightNumber,-15} {airlineKeyValuePairs[flight.FlightNumber.Substring(0, 2)],-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime,-35} {"No",-15} {boardingGate.GateName,-15}");
                    break;
                }
            }
        }

    }
}





//MENU
while (true)
{
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("=============================================\r\nWelcome to Changi Airport Terminal 5\r\n=============================================\r\n1. List All Flights\r\n2. List Boarding Gates\r\n3. Assign a Boarding Gate to a Flight\r\n4. Create Flight\r\n5. Display Airline Flights\r\n6. Modify Flight Details\r\n7. Display Flight Schedule\r\n0. Exit");
    Console.WriteLine();
    Console.Write("Please select your option:\n");
    string option = Console.ReadLine();
    if (option == "1")
    {
        TaskThree();
    }
    else if (option == "2")
    {

    }
    else if (option == "3")
    {
        TaskFive();
    }
    else if (option == "4")
    {
        TaskSix();
    }
    else if (option == "5")
    {

    }
    else if(option == "6")
    {

    }
    else if (option == "7")
    {
        TaskNine();
    }
    else if (option == "0")
    {
        break;
    }
    else
    {
        Console.WriteLine("Invalid Option");
    }


}
*/