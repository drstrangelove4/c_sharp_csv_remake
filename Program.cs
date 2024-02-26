// CWD is the /bin directory path, not the root path of the project

string cwd = Environment.CurrentDirectory;
string directory_path = Directory.GetParent(cwd).Parent.Parent.FullName;

// get the current date to make a unique identifier - putting it here should mean it's calculated once.
string current_date = DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss");


// --------------------------------------------------------------------------------------------------------------------------------------


// Get csv files from the path
List<string> csv_files = find_files(directory_path);

// use stream reader to open the file
StreamReader reader = null;


// get data for each file
foreach (string csv_file in csv_files)
{
    Console.WriteLine("Removing header from csv at: " + csv_file);
    // open the file
    reader = new StreamReader(File.OpenRead(csv_file));

    // Get the data with the header removed.
    string removed_header = csv_data(reader);

    // Close the file so we can move it.
    reader.Close();

    // Move the old csv file to a new directory so it is not overwritten.
    move_old_file(directory_path, csv_file, current_date);

    // Write the new file to the current csv path.
    save_data(removed_header, csv_file);
}


// --------------------------------------------------------------------------------------------------------------------------------------
// Move the old file to a new directory.


void move_old_file(string current_root_path, string current_csv_path, string current_date)
{

    // get the filename from the fullpath
    string file_name = current_csv_path.Split('\\').Last();
    Console.WriteLine(file_name);

    // create a new folder name from the date + old-
    string new_path = Path.Join(current_root_path, "old", current_date);

    //Make the new directory if it doesn't exist.
    Directory.CreateDirectory(new_path);

    // Move the file to the new folder
    File.Move(current_csv_path, Path.Join(new_path, file_name));

    Console.WriteLine(file_name + " has been moved to " + new_path);
}


// --------------------------------------------------------------------------------------------------------------------------------------
// Save data


void save_data(string csv_files, string directory_path)
{
    File.WriteAllText(directory_path, csv_files);
}


// --------------------------------------------------------------------------------------------------------------------------------------
// Deals with the CSV data.


string csv_data(StreamReader reader)
{
    // Return string
    string file_data = "";

    // Current row
    int current_row = 0;

    // Use the reader object to read the CSV file until end of file.
    while (!reader.EndOfStream)
    {
        // read the current line of data
        string line = reader.ReadLine();

        // remake the file without the header
        if (current_row != 0)
        {
            file_data += line += '\n';
        }
        // Increment the row we are dealing with
        current_row++;
    }

    return file_data;
}


// --------------------------------------------------------------------------------------------------------------------------------------
// A function that takes a directory path and finds all the csv files in it.


List<string> find_files(string directory_path)
{
    // This function takes a directory path and finds all the CSV files in that path.
    // It returns a list of csv file paths

    string[] all_files = Directory.GetFiles(directory_path);
    List<string> csv_file_paths = new List<string>();

    foreach (string file in all_files)
    {
        if (file.Contains(".csv"))
        {
            csv_file_paths.Add(file);
        }
    }
    return csv_file_paths;
}
