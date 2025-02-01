using PRG2_Grp_project;
using System.Globalization;
using System.Xml.Serialization;
using System.IO;

//==========================================================
// Student Number : S10266029
// Student Name : Aloysius Luke Tay Shi Yuan
// Partner Name : Samuel Ng En Yi
//==========================================================

Dictionary<string, Flight> flightDict = new Dictionary<string, Flight>();
Dictionary<string, Airline> airlineDict = new Dictionary<string, Airline>();
Dictionary<string, BoardingGate> boardingGateDict = new Dictionary<string, BoardingGate>();
Dictionary<string, double> feeDict = new Dictionary<string, double>();

// Create Terminal 5
Terminal terminalFive = new Terminal("Terminal 5", airlineDict, flightDict, boardingGateDict, feeDict);

//Task 1
// Load Airlines
try
{
    Console.WriteLine("Loading Airlines...");
    string[] airlinesStrings = File.ReadAllLines("airlines.csv");

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

//Task 2
// Load Flights
try
{
    Console.WriteLine("Loading Flights...");
    string[] flightStrings = File.ReadAllLines("flights.csv");

    foreach (string data in flightStrings.Skip(1))
    {
        string[] splitData = data.Split(",");
        Flight flight = null;

        if (splitData.Length < 5) continue;

        if (splitData[4] == "LWTT")
        {
            flight = new LWTTFlight(splitData[0], splitData[1], splitData[2], Convert.ToDateTime(splitData[3]), "Scheduled", 500);
        }
        else if (splitData[4] == "DDJB")
        {
            flight = new DDJBFlight(splitData[0], splitData[1], splitData[2], Convert.ToDateTime(splitData[3]), "Scheduled", 300);
        }
        else if (splitData[4] == "CFFT")
        {
            flight = new CFFTFlight(splitData[0], splitData[1], splitData[2], Convert.ToDateTime(splitData[3]), "Scheduled", 150);
        }
        else
        {
            flight = new NORMFlight(splitData[0], splitData[1], splitData[2], Convert.ToDateTime(splitData[3]), "Scheduled");
        }

        terminalFive.Flights.Add(splitData[0], flight);
    }
    Console.WriteLine($"{terminalFive.Flights.Count()} Flights Loaded!");
}
catch (Exception ex)
{
    Console.WriteLine($"Error loading flights.csv: {ex.Message}");
    return;
}

// Task 3: List All Flights
void TaskThree()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Flight Number",-15}{"Airline Name",-25}{"Origin",-25}{"Destination",-25}{"Expected Departure/Arrival Time",-30}");

    foreach (Flight flight in terminalFive.Flights.Values)
    {
        // Extract the airline code from the flight number (first two characters)
        string airlineCode = flight.FlightNumber.Substring(0, 2);

        // Find the airline name using the airline dictionary
        string airlineName = "Unknown Airline";
        foreach (var airline in terminalFive.Airlines.Values)
        {
            if (airline.Code == airlineCode) // Ensure we compare with Airline.Code, not the dictionary key
            {
                airlineName = airline.Name;
                break;
            }
        }

        // Display flight details with the correct airline name
        Console.WriteLine($"{flight.FlightNumber,-15}{airlineName,-25}{flight.Origin,-25}{flight.Destination,-25}{flight.ExpectedTime:dd/MM/yyyy hh:mm tt}");
    }
}



// Task 4: List All Boarding Gates
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

// Task 5: Assign a Boarding Gate to a Flight
void TaskFive()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Assign a Boarding Gate to a Flight");
    Console.WriteLine("=============================================");

    string flightNumber;
    Flight selectedFlight = null;

    // Prompt for Flight Number
    while (true)
    {
        Console.Write("Enter Flight Number:\n");
        flightNumber = Console.ReadLine();

        if (terminalFive.Flights.TryGetValue(flightNumber, out selectedFlight))
        {
            break;
        }
        Console.WriteLine("Invalid Flight Number. Please try again.");
    }

    string boardingGateNumber;
    BoardingGate selectedBoardingGate = null;

    // Prompt for Boarding Gate Name
    while (true)
    {
        Console.Write("Enter Boarding Gate Name:\n");
        boardingGateNumber = Console.ReadLine();

        if (terminalFive.BoardingGates.TryGetValue(boardingGateNumber, out selectedBoardingGate))
        {
            if (selectedBoardingGate.Flight == null)
            {
                break;
            }
            else
            {
                Console.WriteLine("Error: Boarding Gate is already assigned to another flight. Please select another gate.");
            }
        }
        else
        {
            Console.WriteLine("Invalid Boarding Gate Name. Please try again.");
        }
    }

    // Display Flight and Boarding Gate Details
    Console.WriteLine($"\nFlight Number: {selectedFlight.FlightNumber}");
    Console.WriteLine($"Origin: {selectedFlight.Origin}");
    Console.WriteLine($"Destination: {selectedFlight.Destination}");
    Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime:dd/M/yyyy h:mm:ss tt}");

    // Determine Special Request Code
    string specialRequestCode = "None";
    if (selectedFlight is CFFTFlight) specialRequestCode = "CFFT";
    else if (selectedFlight is DDJBFlight) specialRequestCode = "DDJB";
    else if (selectedFlight is LWTTFlight) specialRequestCode = "LWTT";

    Console.WriteLine($"Special Request Code: {specialRequestCode}");

    // Display Boarding Gate Information
    Console.WriteLine($"Boarding Gate Name: {selectedBoardingGate.GateName}");
    Console.WriteLine($"Supports DDJB: {selectedBoardingGate.SupportsDDJB}");
    Console.WriteLine($"Supports CFFT: {selectedBoardingGate.SupportsCFFT}");
    Console.WriteLine($"Supports LWTT: {selectedBoardingGate.SupportsLWTT}");

    // Ask user if they want to update flight status
    while (true)
    {
        Console.Write("Would you like to update the status of the flight? (Y/N)\n");
        string updateStatusChoice = Console.ReadLine()?.Trim().ToUpper();

        if (updateStatusChoice == "Y")
        {
            Console.WriteLine("1. Delayed");
            Console.WriteLine("2. Boarding");
            Console.WriteLine("3. On Time");
            Console.Write("Please select the new status of the flight:\n");

            while (true)
            {
                string statusOption = Console.ReadLine()?.Trim();
                if (statusOption == "1")
                {
                    selectedFlight.Status = "Delayed";
                    break;
                }
                else if (statusOption == "2")
                {
                    selectedFlight.Status = "Boarding";
                    break;
                }
                else if (statusOption == "3")
                {
                    selectedFlight.Status = "On Time";
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please select a valid status (1, 2, or 3).");
                }
            }
            break;
        }
        else if (updateStatusChoice == "N")
        {
            selectedFlight.Status = "On Time"; // Default status
            break;
        }
        else
        {
            Console.WriteLine("Invalid option. Please enter Y or N.");
        }
    }

    // Assign Flight to Boarding Gate
    selectedBoardingGate.Flight = selectedFlight;
    Console.WriteLine($"\nFlight {selectedFlight.FlightNumber} has been assigned to Boarding Gate {selectedBoardingGate.GateName}!");
}


// Task 6: Create a New Flight
void TaskSix()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Create a New Flight");
    Console.WriteLine("=============================================");

    Console.Write("Enter Flight Number: ");
    string flightNumber = Console.ReadLine().Trim().ToUpper();

    // Prevent duplicate flight numbers
    if (terminalFive.Flights.ContainsKey(flightNumber))
    {
        Console.WriteLine("Error: Flight number already exists.");
        return;
    }

    Console.Write("Enter Origin: ");
    string origin = Console.ReadLine().Trim();

    Console.Write("Enter Destination: ");
    string destination = Console.ReadLine().Trim();

    DateTime expectedDepartureOrArrivalTime;
    while (true)
    {
        Console.Write("Enter expected departure/arrival time (dd/mm/yyyy HH:mm): ");
        string inputDateTime = Console.ReadLine().Trim();

        string[] formats = { "d/M/yyyy H:mm", "dd/MM/yyyy HH:mm" }; // Accept both formats

        if (DateTime.TryParseExact(inputDateTime, formats, null, DateTimeStyles.None, out expectedDepartureOrArrivalTime))
        {
            break;
        }
        Console.WriteLine("Invalid date format. Please enter the date in 'd/M/yyyy H:mm' or 'dd/MM/yyyy HH:mm' format.");
    }

    string specialRequestCode;
    while (true)
    {
        Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
        specialRequestCode = Console.ReadLine().Trim().ToUpper();
        if (specialRequestCode == "CFFT" || specialRequestCode == "DDJB" || specialRequestCode == "LWTT" || specialRequestCode == "NONE")
        {
            break;
        }
        Console.WriteLine("Invalid input! Please enter a valid special request code.");
    }

    // Create Flight Object
    Flight newFlight;
    switch (specialRequestCode)
    {
        case "CFFT":
            newFlight = new CFFTFlight(flightNumber, origin, destination, expectedDepartureOrArrivalTime, "Scheduled", 150);
            break;
        case "DDJB":
            newFlight = new DDJBFlight(flightNumber, origin, destination, expectedDepartureOrArrivalTime, "Scheduled", 300);
            break;
        case "LWTT":
            newFlight = new LWTTFlight(flightNumber, origin, destination, expectedDepartureOrArrivalTime, "Scheduled", 500);
            break;
        default:
            newFlight = new NORMFlight(flightNumber, origin, destination, expectedDepartureOrArrivalTime, "Scheduled");
            specialRequestCode = "None"; // Standardize CSV format
            break;
    }

    // Add new flight to the system
    terminalFive.Flights.Add(newFlight.FlightNumber, newFlight);

    // Append to flights.csv
    try
    {
        using (StreamWriter writer = new StreamWriter("flights.csv", append: true))
        {
            writer.WriteLine($"{newFlight.FlightNumber},{newFlight.Origin},{newFlight.Destination},{newFlight.ExpectedTime:dd/MM/yyyy HH:mm},{specialRequestCode}");
        }
        Console.WriteLine($"Flight {newFlight.FlightNumber} has been successfully added and saved to flights.csv!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error saving flight to CSV: {ex.Message}");
    }
}



// Task 7: Display all flights of a selected airline
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


// Task 8: Modify or delete a flight
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


// Task 9: Display Flight Schedule in Chronological Order
void TaskNine()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Flight Number",-15}{"Airline Name",-25}{"Origin",-25}{"Destination",-25}{"Expected Time",-25}{"Status",-15}{"Boarding Gate",-15}");

    // Make a list of flights and sort by expected time (Selection sort)
    List<Flight> flightList = new List<Flight>(terminalFive.Flights.Values);

    for (int i = 0; i < flightList.Count - 1; i++)
    {
        for (int j = i + 1; j < flightList.Count; j++)
        {
            if (flightList[i].ExpectedTime > flightList[j].ExpectedTime)
            {
                Flight temp = flightList[i];
                flightList[i] = flightList[j];
                flightList[j] = temp;
            }
        }
    }

    foreach (Flight flight in flightList)
    {
        // Extract the airline code from the flight number (first two characters)
        string airlineCode = flight.FlightNumber.Substring(0, 2);

        // Find the airline name using the airline dictionary
        string airlineName = terminalFive.Airlines.Values.FirstOrDefault(a => a.Code == airlineCode)?.Name ?? "Unknown Airline";

        // Find the assigned boarding gate (if any)
        string boardingGateName = "Unassigned";
        foreach (var gate in terminalFive.BoardingGates)
        {
            if (gate.Value.Flight == flight)
            {
                boardingGateName = gate.Key;
                break;
            }
        }

        // If flight status is null, default it to "Scheduled"
        string flightStatus = string.IsNullOrEmpty(flight.Status) ? "Scheduled" : flight.Status;

        // Display flight details with AM/PM formatting and proper spacing
        Console.WriteLine($"{flight.FlightNumber,-15}{airlineName,-25}{flight.Origin,-25}{flight.Destination,-25}{$"{flight.ExpectedTime:dd/MM/yyyy h:mm:ss tt}",-25}{flightStatus,-15}{boardingGateName,-15}");
    }
}



// Advanced Task A: Assign all unassigned flights to available boarding gates
void AdvancedTaskA()
{
    foreach (var flight in terminalFive.Flights.Values)
    {
        if (!boardingGateDict.Values.Any(bg => bg.Flight == flight))
        {
            var availableGate = boardingGateDict.Values.FirstOrDefault(bg => bg.Flight == null);
            if (availableGate != null)
            {
                availableGate.Flight = flight;
                Console.WriteLine($"Assigned {flight.FlightNumber} to {availableGate.GateName}");
            }
        }
    }
}

// Advanced Task B: Calculate total fees per airline
void AdvancedTaskB()
{
    terminalFive.PrintAirlineFees();
}


// Main Menu Loop
while (true)
{
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
