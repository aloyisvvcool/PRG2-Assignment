using PRG2_Grp_project;
using System.Globalization;
using System.Xml.Serialization;



//==========================================================
// Student Number : S10267956
// Student Name : Samuel Ng En Yi
// Partner Name : Aloysius Luke Tay Shi Yuan
//==========================================================



Dictionary<string, Flight> flightDict = new Dictionary<string, Flight>();
Dictionary<string, Airline> airlineDict = new Dictionary<string, Airline>();
Dictionary<string, BoardingGate> boardingGateDict = new Dictionary<string, BoardingGate>();
Dictionary<string, double> feeDict = new Dictionary<string, double>();
//Create terminal 5
Terminal terminalFive = new Terminal("Terminal 5", airlineDict, flightDict, boardingGateDict, feeDict);




//task 1
//open Airlines.csv file
Console.WriteLine("Loading Airlines");
string[] airlinesStrings = File.ReadAllLines("airlines.csv");
foreach (string data in airlinesStrings.Skip(1))
{
    string[] splitData = data.Split(",");
    //Flight is currently null since there is no flight
    Airline airline = new Airline(splitData[0], splitData[1], new Dictionary<string, Flight>());
    terminalFive.Airlines.Add(splitData[0], airline);
}
Console.WriteLine($"{terminalFive.Airlines.Count()} Airlines Loaded!");

//Load BoardingGates
Console.WriteLine("Loading Boarding Gates...");

string[] boardingGateStrings = File.ReadAllLines("boardinggates.csv");
foreach (string data in boardingGateStrings.Skip(1))
{
    string[] splitData = data.Split(",");
    //Flight is currently null since there is no flight
    BoardingGate boardingGate = new BoardingGate(splitData[0], Convert.ToBoolean(splitData[2]), Convert.ToBoolean(splitData[1]), Convert.ToBoolean(splitData[3]), null);
    terminalFive.BoardingGates.Add(splitData[0], boardingGate);
}
Console.WriteLine($"{terminalFive.BoardingGates.Count()} Boarding Gates Loaded!");


//TASK 2
//Load Flights
Console.WriteLine("Loading Flights...");
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

    terminalFive.Flights.Add(splitData[0], flight);
}
Console.WriteLine($"{terminalFive.Flights.Count()} Flights Loaded!");


//DO NOT REMOVE, GLOBAL VARIABLE USED FOR TASK 3,9 AND ADVANCED
Dictionary<string, string> airlineKeyValuePairs = new Dictionary<string, string>();
foreach (string data in airlinesStrings)
{
    string[] splitData = data.Split(",");
    airlineKeyValuePairs.Add(splitData[1], splitData[0]);
}


//GLOBAL VARIABLE FOR TASK 5,9, ADVANCED
Dictionary<string, string> specialCodeDict = new Dictionary<string, string>();
//Dictionary to map code to special request, e.g. EK 870 to DDJB
for (int i = 1; i < flightStrings.Count(); i++) //Skip header
{
    string code = "";
    try
    {
        code = flightStrings[i].Split(",")[4];
    }
    catch
    {
        //Keeps code as "" for Normal Flights
    }

    specialCodeDict.Add(flightStrings[i].Split(",")[0], code);
}


//TASK 3 (Complete)
void TaskThree()
{
    Console.WriteLine("=============================================\r\nList of Flights for Changi Airport Terminal 5\r\n=============================================");
    Console.WriteLine($"{"Flight Number",-15}{"Airline Name",-25}{"Origin",-25}{"Destination",-25}{"Expected Departure/Arrival Time",-30}");
    //Create Dictionary mapping airline code to airline name
    foreach (Flight flight in terminalFive.Flights.Values)
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
            selectedFlight = terminalFive.Flights[flightNumber];
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
        bool matchingSpecialCode = false;
        while (true)
        {
            Console.Write("Enter Boarding Gate Name:\n");
            boardingGateNumber = Console.ReadLine();
            if (terminalFive.BoardingGates.ContainsKey(boardingGateNumber))
            {
                Console.WriteLine($"{selectedFlight.FlightNumber}, {specialCodeDict[selectedFlight.FlightNumber]}");
                if (specialCodeDict[selectedFlight.FlightNumber] == "DDJB" && terminalFive.BoardingGates[boardingGateNumber].SupportsDDJB == true)
                {
                    break;
                }
                else if (specialCodeDict[selectedFlight.FlightNumber] == "CFFT" && terminalFive.BoardingGates[boardingGateNumber].SupportsCFFT == true)
                {
                    break;
                }
                else if (specialCodeDict[selectedFlight.FlightNumber] == "LWTT" && terminalFive.BoardingGates[boardingGateNumber].SupportsLWTT == true)
                {
                    break;
                }
                else if (specialCodeDict[selectedFlight.FlightNumber] == "" || specialCodeDict[selectedFlight.FlightNumber] == "None")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Boarding Gate does not support Flight Special Request Code");
                }

            }
            else
            {
                Console.WriteLine("Invalid Boarding Gate Name");
            }
        }



        if (terminalFive.BoardingGates[boardingGateNumber].Flight == null)
        {
            //Data from Flight
            Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
            Console.WriteLine($"Origin: {selectedFlight.Origin}");
            Console.WriteLine($"Destination: {selectedFlight.Destination}");
            Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime}");
            if (selectedFlight is NORMFlight)
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
            BoardingGate selectedBoardingGate = terminalFive.BoardingGates[boardingGateNumber];
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
                            terminalFive.Flights[flightNumber].Status = "Delayed";
                            exit = true;
                            break;
                        }
                        else if (statusOption == "2")
                        {
                            terminalFive.Flights[flightNumber].Status = "Boarding";
                            exit = true;
                            break;
                        }
                        else if (statusOption == "3")
                        {
                            terminalFive.Flights[flightNumber].Status = "On Time";
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
                    terminalFive.Flights[flightNumber].Status = "On Time";
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Option");
                }
            }


            //Assign Flight to Boarding Gate
            terminalFive.BoardingGates[boardingGateNumber].Flight = selectedFlight;
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

            //Initialise DateTime Object, DateTime.Now will not be used as actualy date
            DateTime expectedDepartureOrArrivalTime = DateTime.Now;
            while (true)
            {
                try
                {
                    Console.Write("Enter expected departure/arrival time (d/M/yyyy H:mm): ");
                    string inputDateTime = Console.ReadLine();
                    expectedDepartureOrArrivalTime = DateTime.ParseExact(inputDateTime, "d/M/yyyy H:mm", null);
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid date format. Please enter the date in 'd/M/yyyy H:mm' format.");
                }
            }

            string specialRequestCode = "";
            while (true)
            {
                Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                specialRequestCode = Console.ReadLine();
                if (specialRequestCode == "CFFT" || specialRequestCode == "DDJB" || specialRequestCode == "LWTT" || specialRequestCode == "None")
                {
                    break;
                }
                Console.WriteLine("Invalid input! Please enter a valid special request code.");
            }


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
                terminalFive.Flights.Add(newFlight.FlightNumber, newFlight);
                using (StreamWriter writer = new StreamWriter("flights.csv", append: true))
                {
                    writer.WriteLine($"{newFlight.FlightNumber},{newFlight.Origin},{newFlight.Destination},{newFlight.ExpectedTime},{specialRequestCode}");
                }
                Console.WriteLine($"Flight {flightNumber} has been added!");

                //Add new flight to special code dict
                specialCodeDict[newFlight.FlightNumber] = specialRequestCode;
                break;

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

    //Make a copy of terminalFive.Flights as a List for sorting, new list because we do not want to sort original copy of terminalFive.Flights
    //Contains only the flight objects
    List<Flight> flightList = new List<Flight>(terminalFive.Flights.Values);

    //Selection Sort
    for (int i = 0; i < flightList.Count; i++)
    {
        for (int j = 0; j < flightList.Count; j++)
        {
            if (flightList[i].CompareTo(flightList[j]) == -1)
            {
                Flight placeholder = flightList[j];
                flightList[j] = flightList[i];
                flightList[i] = placeholder;


            }

        }
    }

    //
    foreach (Flight flight in flightList)
    {
        string boardingGateName = "Unassigned";
        //Loop through each boarding gate to check if the flight is assigned to it
        foreach (BoardingGate boardingGate in terminalFive.BoardingGates.Values)
        {

            if (boardingGate.Flight == flight)
            {
                //Change boardingGateName to name of Boarding Gate matched
                boardingGateName = boardingGate.GateName;
            }
        }

        //If flight status is blank
        if (flight.Status == null)
        {
            Console.WriteLine($"{flight.FlightNumber,-15} {airlineKeyValuePairs[flight.FlightNumber.Substring(0, 2)],-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime,-35} {"Scheduled",-15} {boardingGateName,-15}");
        }
        //If flight status is not blank and has a value
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
    //Make a copy of terminalFive.Flights as a List for sorting
    List<Flight> flightList = new List<Flight>(terminalFive.Flights.Values);

    //Create list of flightCodes with boarding gates
    //Initialise number of Flights already assigned Boarding Gates
    int numOfFlightsWithBoardingGate = 0;
    foreach (BoardingGate boardingGate in terminalFive.BoardingGates.Values)
    {
        if (boardingGate.Flight != null)
        {
            flightsWithBoardingGateCode.Add(boardingGate.Flight.FlightNumber);
            numOfFlightsWithBoardingGate += 1;
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
    Console.WriteLine($"Total number of unassigned Boarding Gates: {freeBoardingGates.Count()}");



    //Checking if flight has special request code, match and assign to gate, then print Flight details.
    //Initialise Number of processed Flights and Boarding Gate
    int processedNumber = 0;
    while (flightWithoutBoardingGateQueue.Count > 0)
    {
        Flight flight = flightWithoutBoardingGateQueue.Dequeue();
        if (specialCodeDict[flight.FlightNumber] == "CFFT")
        {
            foreach (BoardingGate boardingGate in freeBoardingGates)
            {
                if (boardingGate.SupportsCFFT == true)
                {
                    terminalFive.BoardingGates[boardingGate.GateName].Flight = flight;
                    freeBoardingGates.Remove(boardingGate);
                    Console.WriteLine($"{flight.FlightNumber,-15} {airlineKeyValuePairs[flight.FlightNumber.Substring(0, 2)],-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime,-35} {"CFFT",-15} {boardingGate.GateName,-15}");
                    processedNumber++;
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
                    terminalFive.BoardingGates[boardingGate.GateName].Flight = flight;
                    freeBoardingGates.Remove(boardingGate);
                    Console.WriteLine($"{flight.FlightNumber,-15} {airlineKeyValuePairs[flight.FlightNumber.Substring(0, 2)],-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime,-35} {"DDJB",-15} {boardingGate.GateName,-15}");
                    processedNumber++;
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
                    terminalFive.BoardingGates[boardingGate.GateName].Flight = flight;
                    freeBoardingGates.Remove(boardingGate);
                    Console.WriteLine($"{flight.FlightNumber,-15} {airlineKeyValuePairs[flight.FlightNumber.Substring(0, 2)],-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime,-35} {"LWTT",-15} {boardingGate.GateName,-15}");
                    processedNumber++;
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
                    terminalFive.BoardingGates[boardingGate.GateName].Flight = flight;
                    freeBoardingGates.Remove(boardingGate);
                    Console.WriteLine($"{flight.FlightNumber,-15} {airlineKeyValuePairs[flight.FlightNumber.Substring(0, 2)],-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime,-35} {"No",-15} {boardingGate.GateName,-15}");
                    processedNumber++;
                    break;
                }
            }
        }

    }


    Console.WriteLine($"Total number of Flights and Boarding Gates processed and assigned: {processedNumber}");
    try
    {
        Console.WriteLine($"Total number of Flights and Boarding Gates that were processed automatically over those that were already assigned: {(processedNumber / numOfFlightsWithBoardingGate * 100.00):F2}% ");
        double automaticallyAssignedOverAlreadyAssigned = (processedNumber / numOfFlightsWithBoardingGate * 100.00);
    }
    catch (DivideByZeroException)
    {
        Console.WriteLine("Unable to calculate total number of Flights and Boarding Gates that were processed automatically over those that were already assigned because 0 Flights and Boarding Gates were assigned before");
    }


}





//task 4
void TaskFour()
{
    Console.WriteLine("=============================================\nList of Boarding Gates for Changi Airport Terminal 5\n=============================================");
    Console.WriteLine("Gate Name\tDDJB\t\tCFFT\t\tLWTT");
    foreach (BoardingGate bg in terminalFive.BoardingGates.Values)
    {
        Console.WriteLine($"{bg.GateName}\t\t{bg.SupportsDDJB}\t\t{bg.SupportsCFFT}\t\t{bg.SupportsLWTT}");
    }
}




//task 7
void TaskSeven()
{
    Console.WriteLine("=============================================\nList of Airlines for Changi Airport Terminal 5\n=============================================");
    Console.WriteLine("Airline Code\tAirline Name");
    foreach (Airline al in terminalFive.Airlines.Values) //list all the Airlines available
    {
        Console.WriteLine($"{al.Code}\t\t{al.Name}");
    }

    Console.Write("Enter Airline Code: "); //prompt the user to enter the 2-Letter Airline Code (e.g. SQ or MH, etc.)
    string searchCode = Console.ReadLine();
    Console.Write($"=============================================\nList of Flights for "); //prompt the user to enter the 2-Letter Airline Code (e.g. SQ or MH, etc.)
    Airline searchAirline = null;
    foreach (Airline al in terminalFive.Airlines.Values)
        if (al.Code == searchCode)
        {
            searchAirline = al; //retrieve the Airline object selected
            Console.Write(searchAirline.Name);
        }
    Console.Write("\n=============================================");
    Console.WriteLine("\nFlight Number\tAirline Name\t\tOrigin\t\t\tDestination\t\tExpectedDeparture/Arrival Time");
    foreach (Flight f in terminalFive.Flights.Values) //for each Flight from that Airline, show their Airline Number, Origin and Destination
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
    foreach (Flight f in terminalFive.Flights.Values)
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

}






void TaskEight()
{
    //task 8

    Console.WriteLine("=============================================\nList of Airlines for Changi Airport Terminal 5\n=============================================");
    Console.WriteLine("Airline Code\tAirline Name");
    foreach (Airline al in terminalFive.Airlines.Values) //list all the Airlines available
    {
        Console.WriteLine($"{al.Code}\t\t{al.Name}");
    }

    Console.Write("Enter Airline Code: "); //prompt the user to enter the 2-Letter Airline Code (e.g. SQ or MH, etc.)
    string searchCode1 = Console.ReadLine();
    Console.Write($"=============================================\nList of Flights for "); //prompt the user to enter the 2-Letter Airline Code (e.g. SQ or MH, etc.)
    Airline searchAirline1 = null;
    foreach (Airline al in terminalFive.Airlines.Values)
        if (al.Code == searchCode1)
        {
            searchAirline1 = al; //retrieve the Airline object selected
            Console.Write(searchAirline1.Name);
        }
    Console.Write("\n=============================================");
    Console.WriteLine("\nFlight Number\tAirline Name\t\tOrigin\t\t\tDestination\t\tExpectedDeparture/Arrival Time");
    foreach (Flight f in terminalFive.Flights.Values) //for each Flight from that Airline, show their Airline Number, Origin and Destination
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
    foreach (Flight f in terminalFive.Flights.Values)
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

            terminalFive.Flights[searchFlight1.FlightNumber] = searchFlight1;
            Console.WriteLine("Flight Updated!");

        }
        else if (choice == "2")
        {
            Console.Write("\nEnter new Status: ");
            string newStatus = Console.ReadLine();
            searchFlight1.Status = newStatus;

            terminalFive.Flights[searchFlight1.FlightNumber] = searchFlight1;
            Console.WriteLine("Flight Updated!");
        }
        else if (choice == "3")
        {
            // add if have
        }
        else if (choice == "4")
        {
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
            foreach (var kvp in terminalFive.Flights)
            {
                if (kvp.Value == searchFlight1)
                {
                    terminalFive.Flights.Remove(kvp.Key);
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
    Console.WriteLine("=============================================");
    Console.WriteLine("Welcome to Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("3. Assign a Boarding Gate to a Flight");
    Console.WriteLine("4. Create Flight");
    Console.WriteLine("5. Display Airline Flights");
    Console.WriteLine("6. Modify Flight Details");
    Console.WriteLine("7. Display Flight Schedule");
    Console.WriteLine("8. Process all unassigned flights to boarding gates in bulk");
    Console.WriteLine("0. Exit");
    Console.Write("Please select your option:\n");
    string option = Console.ReadLine();
    if (option == "1")
    {
        TaskThree();
    }
    else if (option == "2")
    {
        TaskFour();
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
        TaskSeven();
    }
    else if (option == "6")
    {
        TaskEight();
    }
    else if (option == "7")
    {
        TaskNine();
    }
    else if (option == "8")
    {
        AdvancedTaskA();
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
