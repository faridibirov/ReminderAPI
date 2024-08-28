# Reminder API

## Overview
The Reminder API is a web application built with ASP.NET Core that allows users to schedule reminders via email or Telegram. This project demonstrates how to create a RESTful API with CRUD operations, integrate email and Telegram notifications, and implement a background service for scheduled tasks.

## Features
- **CRUD Operations**: Create, Read, Update, and Delete reminders in the database.
- **Notification Options**: Send reminders via email (using SendGrid) or Telegram.
- **Background Service**: Automatically send reminders when the scheduled time is reached.
- **Validation**: Prevent scheduling of reminders for past dates or times..
- **Scalable Architecture**: Utilizes design patterns to ensure maintainability and scalability.

## Technologies Used
- **ASP.NET Core**: Web framework used to build the API.
- **Entity Framework Core**: ORM used to interact with the database.
- **SQL Server**: Database used to store reminder data (configurable to use PostgreSQL, MySQL, etc.).
- **SendGrid**: Service for sending email notifications.
- **Telegram Bot API**: Service for sending Telegram notifications.
- **Hosted Service**: Background service to handle the timing and dispatching of reminders.

## Prerequisites
- .NET 8 SDK
- SQL Server or any other supported database
- Telegram Bot API Token for sending Telegram reminders
- SMTP credentials for sending emails via SendGrid

## Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/your-username/reminder-api.git
cd ReminderAPI
```

### 2. Configure the Database and API Keys

Update the connection string and API keys in `appsettings.json`.

## Contribution
If you have any suggestions, improvements, or feedback, please open an issue or submit a pull request. Your contributions are highly appreciated!

Feel free to explore each feature in detail by navigating to the respective directory. Each project folder contains its source code, documentation, and any additional resources.

Happy coding!