# File Processing Application

## Overview

This application consists of a console application and a GUI application designed to periodically check specified folders for JSON files, process their contents, and generate reports based on the processing results. The console application processes files in parallel to enhance efficiency and minimize downtime.

## Features

### Console Application

- **File Monitoring**: Periodically checks selected folders for JSON files.
- **File Structure**: Each JSON file is expected to contain a list of components with:
  - **Component Name** (string)
  - **Component Status** (integer)
- **Processing Results**:
  - Counts the number of components in each file.
  - Stores the full file name (full path) along with the results.
  - Captures any errors encountered during file parsing (error status and full file name).
- **Parallel Processing**: Processes files concurrently to optimize performance.
- **Inter-Application Communication**: Communicates with the GUI application using named pipes.
- **Data Storage**: Stores processing results in an in-memory database using Entity Framework Core.

### GUI Application

- **Configuration Management**:
  - Reads folder paths, check intervals, and maximum concurrent processing jobs from `appsettings.json`.
- **Report Generation**:
  - Displays processing results for tracked folders, showing one result per processed file.
  - Detects if the console application is running.
  - Provides options to start and stop the console application.

### Testing

- Includes test cases demonstrating the functionality with sample data.
- Implements appropriate unit tests to ensure the reliability of both applications.

### Additional Features

- Supports live configuration updates, allowing the GUI application to pick up changes without needing to restart.

## Purpose

The primary goal of this application is to streamline the processing of JSON files located in specified directories, enabling users to easily monitor and report on component statuses. This setup is ideal for environments where data integrity and timely reporting are crucial.

## Technologies Used

- .NET Framework (or specify if using .NET Core)
- GUI Framework (WPF)

## Installation

1. Clone the repository.
2. Open the solution file in Visual Studio (or your preferred IDE).
3. Build the project and run the applications.

## Usage

- Configure the scanner settings in appsettings.json.
- Run the console application to start processing files based on the configured settings.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

