# RAG UI - Avalonia Application

RAG UI is a cross-platform desktop and mobile application built using **Avalonia**, designed to provide a rich and responsive UI for [your domain/use case]. This project leverages Avalonia’s MVVM architecture and reactive programming model for maintainable and scalable UI development.

---

## Table of Contents

- [Features](#features)  
- [Getting Started](#getting-started)  
- [Prerequisites](#prerequisites)  
- [Project Structure](#project-structure)  
- [Building and Running](#building-and-running)  
- [Message Boxes](#message-boxes)  
- [Multi-Targeting](#multi-targeting)  
- [Contributing](#contributing)  
- [License](#license)  

---

## Features

- Cross-platform UI using Avalonia (Windows, Linux, macOS, iOS/Android planned)  
- MVVM architecture for clean separation of concerns  
- ReactiveUI integration for reactive state management  
- Login window with navigation to MainWindow  
- Standard message dialogs for user interaction  
- Modular project structure with shared services and view models  

---

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)  
- [Visual Studio 2022 or 2023](https://visualstudio.microsoft.com/) with **.NET Desktop Development** workload  
- For iOS development (optional): macOS with Xcode installed  
- Optional: Android Studio + Emulator for mobile testing  

---

## Project Structure

```

RAG-UI/
├─ UI/                  # Avalonia UI project
│  ├─ App.xaml           # Application entry point
│  ├─ App.xaml.cs        # App initialization & lifetime
│  ├─ Views/             # XAML views (MainWindow, LoginWindow, etc.)
│  └─ ViewModels/        # ViewModels (MainViewModel, LoginViewModel, etc.)
├─ Services/             # Application services
├─ BusinessObjects/      # Domain/business models
└─ README.md             # Project documentation

````

---

## Building and Running

### Windows (Desktop)

```bash
# Restore packages
dotnet restore

# Build project
dotnet build

# Run project
dotnet run --project UI
````

### iOS (Requires macOS)

```bash
dotnet build -t:Run -f:net8.0-ios18.0 -r:iossimulator-x64
```

> **Note:** iOS build requires a macOS machine with Xcode installed.

---

## Message Boxes

This project uses **`MessageBox.Avalonia`** for displaying modal dialogs.

### Examples

```csharp
// Show info message
await ShowInfoMessage("Operation completed successfully.", "Info");

// Show confirmation message
var result = await ShowConfirmationMessage("Are you sure?", "Confirm");
if (result == ButtonResult.Yes)
{
    // User clicked Yes
}
```

> Make sure to install the package via NuGet:
>
> ```bash
> dotnet add package MessageBox.Avalonia
> ```

---

## Multi-Targeting

This project can be multi-targeted for **desktop + iOS**:

```xml
<TargetFrameworks>net8.0;net8.0-ios18.0</TargetFrameworks>
```

Use `#if IOS` or `#if WINDOWS` in code to separate platform-specific logic.

---

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature-name`)
3. Commit your changes (`git commit -am 'Add feature'`)
4. Push to the branch (`git push origin feature-name`)
5. Open a Pull Request

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
