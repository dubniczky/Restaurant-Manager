# Dotnet Core Restaurant Manager

A web-based restaurant manager with WPF desktop application admin interface.

<div align="center">
  <img src="https://media.githubusercontent.com/media/dubniczky/Restaurant-Manager/main/assets/logo-transparent.png" alt="Avatar" width="120" height="120"/>
</div>

## Support ❤️

If you find the project useful, please consider supporting, or contributing.

[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/dubniczky)

## System

The system consists of 4 main parts:

1. **Database:** containing food, order and user data
1. **Web Interface:** user interface for ordering, using ASP.NET MVC and Entity Framework
1. **Service:** web service for administration, using ASP.NET Core MVC and Entity Framework
1. **Admin Client:** for managing admin service, WPF graphic desktop interface using MVVM architecture.

## Components

1. `Restaurant.Core`: Core functionality shared between all other components.
1. `Restaurant.Admin`: Admin interface WPF desktop app.
1. `Restaurant.Server`: Serving client interface, managing requests and admin app connections.
1. `Restaurant.Test`: System tests.

## Diagrams

### Database Diagram

![Database Digram](/diagrams/database-diagram.jpg)

### WPF App Class Diagram

![App Class Digram](/diagrams/class-diagram.png)