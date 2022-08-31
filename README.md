# Task Manager
## Introduction
The Task Manager is a simple, and easy-to-use collaboration tool that enables you to organize projects and everything related to them into boards and tasks. 
With Task Manager you can find all kinds of information, such as: 
- What’s being worked on?
- Who’s working on what? 
- What progress the project is making<br />

## Prerequisits
The application is compatible with the latest versions of Windows 8.1 and Windows 10.

## Technologies
The Task Manager is a **Client-Server application** developed in Visual Studio 2022.<br />
Based on .NET 6.0.<br />

### Backend
The application is developed using **MVVM architectural pattern** that is represented by three distinct components, the Model, View and ViewModel.
MVVM architecture puts a heavy emphasis on the Separation of Concerns for each of these layers. Separating the layers benefits us as:<br />
- Modularity: Each layer's internal implementation can be changed or swapped without affecting the others.<br />
- Increased testability: Each layer can be Unit Tested with fake data, which is not possible if the ViewModel's code is written in the Code-Behind of the View.<br />

Also is used **pattern REST** for describing an uniform interface between physically separate components in a Client-Server architecture.
Used **RESTful web API** is based on HTTP methods such as GET and POST. HTTP requests are used o access data or resources in the application via URL-encoded parameters are generally formatted as either JSON to transmit the data.<br />
For authentication and authorization is used **JWT (Json WEb Tokens)** Beared library.<br />
For serialization/deserialization of Json format is used **Newtonsoft.Json library**.<br />

### Frontend
For user interface was used **WPF (Windows Presentation Foundation)** with Material Design libraries for better style.<br />

## Screenshots
Page XXX:
add screenshot

Page XXX:
add screenshot

## Video Tutorial
Visit link XXX

## License
This application is licensed under the terms of the MIT License. Please see the LICENSE file for full details.
