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



//Task 1
// Load Airlines
string[] airlinesStrings = File.ReadAllLines("airlines.csv");
try
{
    Console.WriteLine("Loading Airlines...");

    foreach (string data in airlinesStrings.Skip(1))
    {
        string[] splitData = data.Split(",");
        Airline airline = new Airline(splitData[0], splitData[1], new Dictionary<string, Flight>());
        terminalFive.Airlines.Add(splitData[0], airline);
    }
    Console.WriteLine($"{terminalFive.Airlines.Count()} Airlines Loaded!");
}
catch (Exception ex)
{
    Console.WriteLine($"Error loading airlines.csv: {ex.Message}");
    return;
}

// Load Boarding Gates
try
{
    Console.WriteLine("Loading Boarding Gates...");
    string[] boardingGateStrings = File.ReadAllLines("boardinggates.csv");

    foreach (string data in boardingGateStrings.Skip(1))
    {
        string[] splitData = data.Split(",");
        BoardingGate boardingGate = new BoardingGate(splitData[0], Convert.ToBoolean(splitData[2]), Convert.ToBoolean(splitData[1]), Convert.ToBoolean(splitData[3]), null);
        terminalFive.BoardingGates.Add(splitData[0], boardingGate);
    }
    Console.WriteLine($"{terminalFive.BoardingGates.Count()} Boarding Gates Loaded!");
}
catch (Exception ex)
{
    Console.WriteLine($"Error loading boardinggates.csv: {ex.Message}");
    return;
}


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
    //For loop to print each Flight Details
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
            //Prompt for flight number
            Console.Write("Enter Flight Number:\n");
            flightNumber = Console.ReadLine();

            //Will give error when flight number is not inside
            selectedFlight = terminalFive.Flights[flightNumber];
            break;
        }
        //Error handling for if the flight number does not match with any in the dictionary
        catch (KeyNotFoundException)
        {
            Console.WriteLine("Invalid Flight Number");
        }
    }


    while (true)
    {
        //Initalise boardingGateNumber
        string boardingGateNumber = "";

        //Variable to check if the Boarding Gate and the Flight have matching Special Flight Code
        bool matchingSpecialCode = false;
        while (true)
        {
            //Prompt for Boarding Gate Name
            Console.Write("Enter Boarding Gate Name:\n");
            boardingGateNumber = Console.ReadLine();

            //Check if the boarding gate number matches with any boarding gates in the dictionary
            if (terminalFive.BoardingGates.ContainsKey(boardingGateNumber))
            {
                //Check if both Boarding Gate and Flight support the same Special Flight Request Code
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
                    //Output if the boarding gate does not support the Special Flight Request Code
                    Console.WriteLine("Boarding Gate does not support Flight Special Request Code");
                }

            }

            //Executes if there is no boarding gates matching with the provided boarding gate number
            else
            {
                Console.WriteLine("Invalid Boarding Gate Name");
            }
        }


        //Check whether the boarding gate already has flight assigned to it.
        if (terminalFive.BoardingGates[boardingGateNumber].Flight == null)
        {
            //Display data from flight
            Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
            Console.WriteLine($"Origin: {selectedFlight.Origin}");
            Console.WriteLine($"Destination: {selectedFlight.Destination}");
            Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime}");

            //Display Special Request Code
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
                //Prompt the user if they want to change the flight status
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

                        //Input validation
                        else
                        {
                            Console.WriteLine("Invalid Option");
                        }
                    }
                    break;

                }

                //If "N" then flight status is set to "On Time"
                else if (updateFlightStatus == "N")
                {
                    terminalFive.Flights[flightNumber].Status = "On Time";
                    break;
                }

                //Input validation
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
    //Method is called again if user wants to add another flight
    void AddFlight()
    {
        while (true)
        {
            //Initialise flightNumber
            string flightNumber = "";
            while (true)
            {
                //Prompt user for flight details
                Console.Write("Enter Flight Number: ");
                flightNumber = Console.ReadLine();

                //Check if flight already exists in the dictionary, if it is prompt the user for another Flight number
                if (terminalFive.Flights.ContainsKey(flightNumber))
                {
                    Console.WriteLine("Error: Flight number already exists. Please enter a unique flight number.");
                }

                //break out of the loop and continue
                else
                {
                    break;
                }
            }

            Console.Write("Enter Origin: ");
            string origin = Console.ReadLine();
            Console.Write("Enter Destination: ");
            string destination = Console.ReadLine();

            //Initialise DateTime Object, DateTime.Now will not be used as actualy date but just as a placeholder
            DateTime expectedDepartureOrArrivalTime = DateTime.Now;

            //While true loop for error handling of dateTime
            while (true)
            {
                try
                {
                    Console.Write("Enter expected departure/arrival time (dd/mm/yyyy hh:mm): ");
                    string inputDateTime = Console.ReadLine();
                    expectedDepartureOrArrivalTime = DateTime.ParseExact(inputDateTime, "d/M/yyyy H:mm", null);
                    break;
                }
                //If the format of dateTime is not accepted, it will prompt the user to use the correct format
                catch (FormatException)
                {
                    Console.WriteLine("Invalid date format. Please enter the date in 'd/M/yyyy H:mm' format.");
                }
            }

            //Initialise the specialRequestCode Variable
            string specialRequestCode = "";
            while (true)
            {
                //Prompt user for special request code
                Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                specialRequestCode = Console.ReadLine();

                //if statement to accept only valid Special Request Codes
                if (specialRequestCode == "CFFT" || specialRequestCode == "DDJB" || specialRequestCode == "LWTT" || specialRequestCode == "None")
                {
                    break;
                }

                //Prompts the user to input a valid Special Request Code
                Console.WriteLine("Invalid input! Please enter a valid special request code.");
            }


            //Initialise newFlight
            Flight newFlight = null;

            //Create the flight objects
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


            //Add to dictionary and CSV file
            terminalFive.Flights.Add(newFlight.FlightNumber, newFlight);
            using (StreamWriter writer = new StreamWriter("flights.csv", append: true))
            {
                //If statement so the special flight request code will be " "
                if (specialRequestCode == "None")
                {
                    writer.WriteLine($"{newFlight.FlightNumber},{newFlight.Origin},{newFlight.Destination},{newFlight.ExpectedTime},");
                }

                //For flights with special flight request codes
                else
                {
                    writer.WriteLine($"{newFlight.FlightNumber},{newFlight.Origin},{newFlight.Destination},{newFlight.ExpectedTime},{specialRequestCode}");
                }

            }
            Console.WriteLine($"Flight {flightNumber} has been added!");

            //Add new flight to special code dict
            specialCodeDict[newFlight.FlightNumber] = specialRequestCode;
            break;
        }
    }
    //Call the flight
    AddFlight();


    //Prompt for user if they want to add another flight
    while (true)
    {
        Console.Write("Would you like to add another Flight?(Y/N)\n");
        string addAnotherFlightOption = Console.ReadLine();
        //If yes call the AddFlight() method again
        if (addAnotherFlightOption == "Y")
        {
            AddFlight();
        }

        //Breaks the loop
        else if (addAnotherFlightOption == "N")
        {
            break;
        }

        //Input validation
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
        //Used to display boardingGate for flight later, it will remain as "Unassigned" unless changed. 
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
    //Create a Queue for flights without boarding gates assigned to them.
    Queue<Flight> flightWithoutBoardingGateQueue = new Queue<Flight>();

    //Create list for Boarding Gates with no flights assigned to it
    List<BoardingGate> freeBoardingGates = new List<BoardingGate>();

    //Create a list of flight names of flights that have been assigned a boarding gate
    List<string> flightsWithBoardingGateCode = new List<string>();

    //Make a copy of terminalFive.Flights as a List of all flights 
    List<Flight> flightList = new List<Flight>(terminalFive.Flights.Values);

    //Initialise number of Flights already assigned Boarding Gates
    int numOfFlightsWithBoardingGate = 0;

    foreach (BoardingGate boardingGate in terminalFive.BoardingGates.Values)
    {
        //if boarding gate has been assigned a flight
        if (boardingGate.Flight != null)
        {
            flightsWithBoardingGateCode.Add(boardingGate.Flight.FlightNumber);
            //Adds 1 to the number of flights already assigned boarding gate
            numOfFlightsWithBoardingGate += 1;
        }

        //if boarding gate has not ben assigned a flight
        else
        {
            //Add boarding gate into list of boarding gates without assigned flights
            freeBoardingGates.Add(boardingGate);
        }
    }

    foreach (Flight flight in flightList)
    {
        //If list of flights assigned to boarding gates does not contain the flight number
        if (!flightsWithBoardingGateCode.Contains(flight.FlightNumber))
        {
            //Add the flight to the queue of flights not assigned to a boarding gate 
            flightWithoutBoardingGateQueue.Enqueue(flight);
        }
    }


    //Display number of flights not assigned a boarding gate and number of unassigned boarding gates
    Console.WriteLine($"Total number of Flights without Boarding Gate: {flightWithoutBoardingGateQueue.Count()}");
    Console.WriteLine($"Total number of unassigned Boarding Gates: {freeBoardingGates.Count()}");



    //Checking if flight has special request code, match and assign to gate, then print Flight details.
    //Initialise Number of processed Flights and Boarding Gate
    int processedNumber = 0;
    while (flightWithoutBoardingGateQueue.Count > 0)
    {
        //Remove the flight from the queue and initialise the flight as a flight
        var flight = flightWithoutBoardingGateQueue.Dequeue();
        if (specialCodeDict[flight.FlightNumber] == "CFFT")
        {
            //foreach loop to go through all boarding gates
            foreach (BoardingGate boardingGate in freeBoardingGates)
            {
                //Checks if the boarding gates support CFFT
                if (boardingGate.SupportsCFFT == true)
                {
                    //Sets the selected flight to be the flight in the boarding gate
                    terminalFive.BoardingGates[boardingGate.GateName].Flight = flight;
                    //remove the boarding gate from the free boarding gate dict as it has now been assigned
                    freeBoardingGates.Remove(boardingGate);
                    //Display information about the boarding gate and what flight has been assigned to it
                    Console.WriteLine($"{flight.FlightNumber,-15} {airlineKeyValuePairs[flight.FlightNumber.Substring(0, 2)],-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime,-35} {"CFFT",-15} {boardingGate.GateName,-15}");
                    //Increase the processedNumber counter by 1
                    processedNumber++;
                    break;
                }
            }
        }

        else if (specialCodeDict[flight.FlightNumber] == "DDJB")
        {
            //foreach loop to go through all boarding gates
            foreach (BoardingGate boardingGate in freeBoardingGates)
            {
                //Checks if the boarding gates support DDJB
                if (boardingGate.SupportsDDJB == true)
                {
                    //Sets the selected flight to be the flight in the boarding gate
                    terminalFive.BoardingGates[boardingGate.GateName].Flight = flight;
                    //remove the boarding gate from the free boarding gate dict as it has now been assigned
                    freeBoardingGates.Remove(boardingGate);
                    //Display information about the boarding gate and what flight has been assigned to it
                    Console.WriteLine($"{flight.FlightNumber,-15} {airlineKeyValuePairs[flight.FlightNumber.Substring(0, 2)],-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime,-35} {"DDJB",-15} {boardingGate.GateName,-15}");
                    //Increase the processedNumber counter by 1
                    processedNumber++;
                    break;
                }
            }
        }

        else if (specialCodeDict[flight.FlightNumber] == "LWTT")
        {
            //foreach loop to go through all boarding gates
            foreach (BoardingGate boardingGate in freeBoardingGates)
            {
                //Checks if the boarding gates support LWTT
                if (boardingGate.SupportsLWTT == true)
                {
                    //Sets the selected flight to be the flight in the boarding gate
                    terminalFive.BoardingGates[boardingGate.GateName].Flight = flight;
                    //remove the boarding gate from the free boarding gate dict as it has now been assigned
                    freeBoardingGates.Remove(boardingGate);
                    //Display information about the boarding gate and what flight has been assigned to it
                    Console.WriteLine($"{flight.FlightNumber,-15} {airlineKeyValuePairs[flight.FlightNumber.Substring(0, 2)],-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime,-35} {"LWTT",-15} {boardingGate.GateName,-15}");
                    //Increase the processedNumber counter by 1
                    processedNumber++;
                    break;
                }
            }
        }

        else
        {
            //foreach loop to go through all boarding gates
            foreach (BoardingGate boardingGate in freeBoardingGates)
            {
                //Checks if the boarding gates support none of the special flight request codes
                if (boardingGate.SupportsLWTT == false && boardingGate.SupportsDDJB == false && boardingGate.SupportsCFFT == false)
                {
                    //Sets the selected flight to be the flight in the boarding gate
                    terminalFive.BoardingGates[boardingGate.GateName].Flight = flight;
                    //remove the boarding gate from the free boarding gate dict as it has now been assigned
                    freeBoardingGates.Remove(boardingGate);
                    //Display information about the boarding gate and what flight has been assigned to it
                    Console.WriteLine($"{flight.FlightNumber,-15} {airlineKeyValuePairs[flight.FlightNumber.Substring(0, 2)],-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime,-35} {"No",-15} {boardingGate.GateName,-15}");
                    //Increase the processedNumber counter by 1
                    processedNumber++;
                    break;
                }
            }
        }

    }

    //Display total number of flights automatically processed
    Console.WriteLine($"Total number of Flights and Boarding Gates processed and assigned: {processedNumber}");

    //Display total number of Flights and Boarding Gates process automatically over those that were already assigned
    try
    {
        Console.WriteLine($"Total number of Flights and Boarding Gates that were processed automatically over those that were already assigned: {(processedNumber / numOfFlightsWithBoardingGate * 100.00):F2}% ");
        double automaticallyAssignedOverAlreadyAssigned = (processedNumber / numOfFlightsWithBoardingGate * 100.00);
    }

    //Error happens when no flight has been processed beforehand
    catch (DivideByZeroException)
    {
        Console.WriteLine("Unable to calculate total number of Flights and Boarding Gates that were processed automatically over those that were already assigned because 0 Flights and Boarding Gates were assigned before");
    }


}





//task 4
void TaskFour()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("Gate Name\tDDJB\tCFFT\tLWTT");

    foreach (BoardingGate bg in terminalFive.BoardingGates.Values)
    {
        Console.WriteLine($"{bg.GateName}\t\t{bg.SupportsDDJB}\t{bg.SupportsCFFT}\t{bg.SupportsLWTT}");
    }
}




//task 7
void TaskSeven()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("Airline Code\tAirline Name");

    // Display all airlines and ensure airline codes are shown
    foreach (var airline in terminalFive.Airlines.Values)
    {
        Console.WriteLine($"{airline.Code}\t\t{airline.Name}");
    }

    Console.Write("\nEnter Airline Code: ");
    string airlineCode = Console.ReadLine().Trim().ToUpper(); // Ensure case-insensitive input

    // Check if an airline with this code exists
    Airline selectedAirline = terminalFive.Airlines.Values.FirstOrDefault(a => a.Code == airlineCode);

    if (selectedAirline == null)
    {
        Console.WriteLine("Invalid Airline Code. Please try again.");
        return;
    }

    Console.WriteLine($"\n=============================================");
    Console.WriteLine($"List of Flights for {selectedAirline.Name}");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Flight Number",-15}{"Origin",-25}{"Destination",-25}{"Expected Time",-30}");

    // Ensure only flights belonging to this airline are displayed
    foreach (var flight in terminalFive.Flights.Values)
    {
        if (flight.FlightNumber.StartsWith(airlineCode))
        {
            Console.WriteLine($"{flight.FlightNumber,-15}{flight.Origin,-25}{flight.Destination,-25}{flight.ExpectedTime:dd/MM/yyyy hh:mm tt}");
        }
    }
}






void TaskEight()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("Airline Code\tAirline Name");

    // Display all available airlines
    foreach (var airline in terminalFive.Airlines.Values)
    {
        Console.WriteLine($"{airline.Code,-10}{airline.Name}");
    }

    Console.Write("Enter Airline Code:\n");
    string airlineCode = Console.ReadLine().Trim().ToUpper();

    // Find the selected airline
    Airline selectedAirline = terminalFive.Airlines.Values.FirstOrDefault(a => a.Code == airlineCode);
    if (selectedAirline == null)
    {
        Console.WriteLine("Invalid Airline Code. Please try again.");
        return;
    }

    Console.WriteLine($"\nList of Flights for {selectedAirline.Name}");
    Console.WriteLine($"{"Flight Number",-10}{"Airline Name",-20}{"Origin",-25}{"Destination",-25}{"Expected Departure/Arrival Time",-30}");

    // Get all flights for the selected airline
    List<Flight> airlineFlights = terminalFive.Flights.Values
        .Where(f => f.FlightNumber.StartsWith(airlineCode))
        .ToList();

    if (airlineFlights.Count == 0)
    {
        Console.WriteLine("No flights available for this airline.");
        return;
    }

    foreach (var flight in airlineFlights)
    {
        Console.WriteLine($"{flight.FlightNumber,-10}{selectedAirline.Name,-20}{flight.Origin,-25}{flight.Destination,-25}{flight.ExpectedTime:dd/MM/yyyy h:mm:ss tt}");
    }

    Console.Write("\nChoose an existing Flight to modify or delete:\n");
    string selectedFlightNumber = Console.ReadLine().Trim().ToUpper();

    Flight selectedFlight = airlineFlights.FirstOrDefault(f => f.FlightNumber == selectedFlightNumber);
    if (selectedFlight == null)
    {
        Console.WriteLine("Invalid Flight Number. Please try again.");
        return;
    }

    Console.WriteLine("\n1. Modify Flight");
    Console.WriteLine("2. Delete Flight");
    Console.Write("Choose an option:\n");
    string choice = Console.ReadLine().Trim();

    if (choice == "1")
    {
        Console.WriteLine("\n1. Modify Basic Information");
        Console.WriteLine("2. Modify Status");
        Console.WriteLine("3. Modify Special Request Code");
        Console.WriteLine("4. Modify Boarding Gate");
        Console.Write("Choose an option:\n");
        string modifyChoice = Console.ReadLine().Trim();

        if (modifyChoice == "1") // Modify Basic Information
        {
            Console.Write("Enter new Origin: ");
            selectedFlight.Origin = Console.ReadLine().Trim();

            Console.Write("Enter new Destination: ");
            selectedFlight.Destination = Console.ReadLine().Trim();

            DateTime newExpectedTime;
            while (true)
            {
                Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy HH:mm):\n");
                string inputDateTime = Console.ReadLine().Trim();

                string[] formats = { "d/M/yyyy H:mm", "dd/MM/yyyy HH:mm" };
                if (DateTime.TryParseExact(inputDateTime, formats, null, DateTimeStyles.None, out newExpectedTime))
                {
                    selectedFlight.ExpectedTime = newExpectedTime;
                    break;
                }
                Console.WriteLine("Invalid date format. Please enter the date in 'dd/mm/yyyy H:mm' format.");
            }

            Console.WriteLine("Flight updated!");

            // Display Updated Flight Details
            Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
            Console.WriteLine($"Airline Name: {selectedAirline.Name}");
            Console.WriteLine($"Origin: {selectedFlight.Origin}");
            Console.WriteLine($"Destination: {selectedFlight.Destination}");
            Console.WriteLine($"Expected Departure/Arrival Time: {selectedFlight.ExpectedTime:dd/MM/yyyy h:mm:ss tt}");
            Console.WriteLine($"Status: {selectedFlight.Status}");

            string specialRequestCode = "None";
            if (selectedFlight is CFFTFlight) specialRequestCode = "CFFT";
            else if (selectedFlight is DDJBFlight) specialRequestCode = "DDJB";
            else if (selectedFlight is LWTTFlight) specialRequestCode = "LWTT";

            Console.WriteLine($"Special Request Code: {specialRequestCode}");

            string boardingGateName = "Unassigned";
            foreach (var gate in terminalFive.BoardingGates)
            {
                if (gate.Value.Flight == selectedFlight)
                {
                    boardingGateName = gate.Key;
                    break;
                }
            }
            Console.WriteLine($"Boarding Gate: {boardingGateName}");

        }
        else if (modifyChoice == "2") // Modify Flight Status
        {
            Console.WriteLine("\n1. Delayed");
            Console.WriteLine("2. Boarding");
            Console.WriteLine("3. On Time");
            Console.Write("Choose new status:\n");

            while (true)
            {
                string statusOption = Console.ReadLine().Trim();
                if (statusOption == "1") { selectedFlight.Status = "Delayed"; break; }
                else if (statusOption == "2") { selectedFlight.Status = "Boarding"; break; }
                else if (statusOption == "3") { selectedFlight.Status = "On Time"; break; }
                else { Console.WriteLine("Invalid option. Please select a valid status (1, 2, or 3)."); }
            }
            Console.WriteLine("Flight status updated!\n");
        }
        else if (modifyChoice == "3") // Modify Special Request Code
        {
            Console.Write("Enter new Special Request Code (CFFT/DDJB/LWTT/None):\n");
            string newSpecialRequestCode = Console.ReadLine().Trim().ToUpper();
            if (newSpecialRequestCode != "CFFT" && newSpecialRequestCode != "DDJB" && newSpecialRequestCode != "LWTT" && newSpecialRequestCode != "NONE")
            {
                Console.WriteLine("Invalid Special Request Code. Update failed.");
                return;
            }

            // Update Flight Type
            Flight newFlight;
            switch (newSpecialRequestCode)
            {
                case "CFFT":
                    newFlight = new CFFTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status, 150);
                    break;
                case "DDJB":
                    newFlight = new DDJBFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status, 300);
                    break;
                case "LWTT":
                    newFlight = new LWTTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status, 500);
                    break;
                default:
                    newFlight = new NORMFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status);
                    break;
            }

            terminalFive.Flights[selectedFlight.FlightNumber] = newFlight;
            Console.WriteLine("Special Request Code updated!");
        }
    }
    else if (choice == "2") // Delete Flight
    {
        Console.Write("Are you sure you want to delete this flight? (Y/N):\n");
        if (Console.ReadLine()?.ToUpper() == "Y")
        {
            terminalFive.Flights.Remove(selectedFlightNumber);
            Console.WriteLine("Flight deleted successfully.");
        }
    }
    else
    {
        Console.WriteLine("Invalid choice. Returning to main menu.");
    }
}





void AdvancedTaskB()
{
    terminalFive.PrintAirlineFees();
}













// Main Menu Loop
while (true)
{
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("\n=============================================");
    Console.WriteLine("Welcome to Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("3. Assign a Boarding Gate to a Flight");
    Console.WriteLine("4. Create Flight");
    Console.WriteLine("5. Display Airline Flights");
    Console.WriteLine("6. Modify Flight Details");
    Console.WriteLine("7. Display Flight Schedule");
    Console.WriteLine("8. Process Unassigned Flights to Boarding Gates in Bulk");
    Console.WriteLine("9. Calculate Total Fees for Each Airline");
    Console.WriteLine("0. Exit");
    Console.Write("Please select your option: ");

    string option = Console.ReadLine();
    if (option == "1") TaskThree();
    else if (option == "2") TaskFour();
    else if (option == "3") TaskFive();
    else if (option == "4") TaskSix();
    else if (option == "5") TaskSeven();
    else if (option == "6") TaskEight();
    else if (option == "7") TaskNine();
    else if (option == "8") AdvancedTaskA();
    else if (option == "9") AdvancedTaskB();
    else if (option == "0") break;
    else Console.WriteLine("Invalid Option");
}
