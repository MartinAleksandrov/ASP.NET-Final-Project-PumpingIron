Pumping Iron

Overview

Pumping Iron is a robust ASP.NET-based web application designed for fitness enthusiasts and gym owners. The project facilitates efficient gym management and enhances the user experience by offering various features like class scheduling, membership management, and workout tracking.

Features

Membership Management: Track and manage gym members' details, subscriptions, and payments.

Workout Tracking: Log workouts and monitor progress over time.

Class Scheduling: View, book, and manage fitness classes.

Role-Based Access Control: Separate access for administrators, trainers, and members.

Reporting Tools: Generate insights and reports for gym operations.

Technologies Used

Framework: ASP.NET Core

Programming Language: C#

Frontend: Razor Pages, HTML5, CSS3, JavaScript

Database: Microsoft SQL Server

Testing: xUnit for unit testing

Dependency Injection: Built-in ASP.NET Core DI

Prerequisites

Ensure you have the following installed:

.NET SDK (v6.0 or later)

Microsoft SQL Server

Visual Studio (2022 or later) or any compatible IDE

Git

Getting Started

Cloning the Repository

Clone this repository to your local machine:

git clone https://github.com/your-username/PumpingIron.git

Building the Solution

Open the Pumping _Iron.sln file in Visual Studio.

Restore NuGet packages by building the solution.

Update the database connection string in the appsettings.json file under the Pumping_Iron project.

Run the following command in the Package Manager Console to apply migrations:

Update-Database

Running the Application

Set Pumping_Iron as the startup project in Visual Studio.

Press F5 to build and run the application.

Open your browser and navigate to http://localhost:5000 (or the configured port).

Project Structure

Pumping_Iron
|
├── Pumping_Iron.Common        # Common utilities and shared code
├── Pumping_Iron.Core          # Core business logic
├── Pumping_Iron.Data          # Entity Framework models and migrations
├── Pumping_Iron.Infrastructure # Application infrastructure (e.g., dependency injection)
├── Pumping_Iron.Services      # Service layer for business operations
├── Pumping_Iron.Tests         # Unit tests
├── Pumping_Iron               # Main web application (UI and controllers)
├── LICENSE.txt                # Licensing information
├── Pumping _Iron.sln          # Solution file
└── README.md                  # Project documentation

Testing

Run the unit tests using the following steps:

Open Test Explorer in Visual Studio.

Run all tests to ensure the application is functioning correctly.

Contributing

Contributions are welcome! Please follow these steps:

Fork the repository.

Create a feature branch (git checkout -b feature-name).

Commit your changes (git commit -m 'Add feature').

Push to the branch (git push origin feature-name).

Open a pull request.

License

This project is licensed under the terms of the license specified in the LICENSE.txt file.

Contact

For any inquiries or support, feel free to contact 
martaleksandroff@gmail.com
