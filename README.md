# Swiss NIS Calculator

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Avalonia UI](https://img.shields.io/badge/Avalonia-11.x-8B44AC?logo=data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAyNCAyNCI+PHBhdGggZmlsbD0id2hpdGUiIGQ9Ik0xMiAyTDEgMjFoMjJMMTIgMnoiLz48L3N2Zz4=)](https://avaloniaui.net/)
[![Platform](https://img.shields.io/badge/Platform-Windows-0078D4?logo=windows&logoColor=white)](https://github.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

> RF field strength calculator for Swiss amateur radio antenna approval (NISV compliance)

## Overview

Swiss NIS Calculator is a modern desktop application for calculating electromagnetic field strength to ensure compliance with Swiss NISV (Verordnung uber den Schutz vor nichtionisierender Strahlung) regulations. It helps amateur radio operators prepare documentation for antenna installation approval.

### Key Features

- **Complete NIS Calculation** - Field strength (V/m), EIRP, ERP, and safety distances
- **Multi-Band Support** - HF through SHF frequency bands (1.8 MHz - 10 GHz)
- **Antenna Pattern Database** - Vertical radiation patterns with 10-degree resolution
- **Cable Loss Calculation** - Frequency-dependent attenuation for common cable types
- **Swiss Limit Compliance** - Automatic verification against NISV limits
- **Master Data Management** - Full editors for antennas, cables, and radios
- **Project-Based Workflow** - Save and load station configurations
- **Multi-Language Support** - German, French, Italian, English

## Screenshots

*Coming soon*

## Installation

### Windows (Portable)

1. Download the latest release from [Releases](../../releases)
2. Extract `SwissNISCalculator-win-x64.zip`
3. Run `NIS.Desktop.exe`

### Build from Source

```bash
# Clone the repository
git clone https://github.com/yourusername/Swiss-NIS-Calculator.git
cd Swiss-NIS-Calculator

# Build
dotnet build

# Run
dotnet run --project src/NIS.Desktop
```

## Usage

### Quick Start

1. **Create a New Project** - Enter station details (callsign, operator, address)
2. **Add Antenna Configuration** - Select radio, cable, and antenna from master data
3. **Set Evaluation Point (OKA)** - Define distance and building damping
4. **Calculate** - View field strength results and compliance status

### Master Data Management

Access the Master Data Manager from the Welcome screen to:

- **Antennas** - Add/edit antennas with frequency bands and vertical radiation patterns
- **Cables** - Add/edit cables with frequency-dependent attenuation tables
- **Radios** - Add/edit transmitter specifications

## Technical Details

### Calculation Formulas

```
Mean Power:        Pm = P x AF x MF
Attenuation:       A = 10^(-a/10)
Gain Factor:       G = 10^(g/10)
Field Strength:    E = 1.6 x sqrt(30 x Pm x A x G x AG) / d
Safety Distance:   ds = 1.6 x sqrt(30 x Pm x A x G x AG) / EIGW
```

### Swiss NISV Limits

| Frequency | Limit (V/m) |
|-----------|-------------|
| 1.8 MHz   | 64.7        |
| 3.5 MHz   | 46.5        |
| 7 MHz     | 32.4        |
| 10-28 MHz | 28.0        |
| 50 MHz    | 28.0        |
| 144 MHz   | 28.0        |
| 432 MHz   | 28.6        |
| 1240 MHz  | 48.5        |
| 2300+ MHz | 61.0        |

## Project Structure

```
Swiss-NIS-Calculator/
+-- src/
|   +-- NIS.Core/              # Calculation library
|   |   +-- Models/            # Data models
|   |   +-- Services/          # Calculation services
|   |   +-- Data/              # Database loaders
|   +-- NIS.Desktop/           # Avalonia UI application
|       +-- ViewModels/        # MVVM ViewModels
|       +-- Views/             # XAML Views
|       +-- Controls/          # Custom controls
+-- data/                      # Master data (JSON)
+-- docs/                      # Documentation
+-- tests/                     # Unit tests
+-- legacy/                    # Original VB6 source (reference)
```

## Technology Stack

| Component | Technology |
|-----------|------------|
| Framework | .NET 8.0 |
| UI | Avalonia UI 11.x |
| Architecture | MVVM (CommunityToolkit.Mvvm) |
| PDF Export | QuestPDF |
| Data Format | JSON |

## Documentation

- [Functional Specification](docs/NIS_fsd.md) - Detailed feature specification
- [API Reference](docs/) - Core library documentation

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Original VB6 application by HB9ZS
- Swiss NISV regulations and calculation methodology
- [Avalonia UI](https://avaloniaui.net/) for the cross-platform UI framework

## Contact

For questions or support, please open an issue on GitHub.

---

Made with :heart: for the amateur radio community
