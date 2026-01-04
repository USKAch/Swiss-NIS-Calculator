# Swiss NIS Calculator

[![Build](https://github.com/USKAch/Swiss-NIS-Calculator/actions/workflows/build.yml/badge.svg)](https://github.com/USKAch/Swiss-NIS-Calculator/actions/workflows/build.yml)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Avalonia UI](https://img.shields.io/badge/Avalonia-11.x-8B44AC)](https://avaloniaui.net/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

[![Windows](https://img.shields.io/badge/Windows-x64-0078D4?logo=windows&logoColor=white)](../../releases)
[![macOS](https://img.shields.io/badge/macOS-x64-000000?logo=apple&logoColor=white)](../../releases)
[![Linux](https://img.shields.io/badge/Linux-x64-FCC624?logo=linux&logoColor=black)](../../releases)

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
- **Project Import/Export** - Share projects via .nisproj files with embedded user master data
- **PDF Export** - One page per configuration with column explanations
- **Multi-Language Support** - German, French, Italian, English

## Screenshots

*Coming soon*

## Installation

### Download Pre-built Binaries

Download the latest release for your platform from [Releases](../../releases):

| Platform | Download |
|----------|----------|
| Windows | `SwissNISCalculator-Windows.zip` |
| macOS | `SwissNISCalculator-macOS.dmg` |
| Linux | `SwissNISCalculator-Linux.tar.gz` |

### Windows
1. Extract the ZIP file
2. Run `SwissNISCalculator.exe`

### macOS
1. Open the downloaded DMG
2. Drag `SwissNISCalculator.app` to `Applications` (recommended). If you run directly from the DMG, the app stores its data under `~/Library/Application Support/SwissNISCalculator` because the DMG volume is read-only.
3. Launch the app (you may need to allow it in Security & Privacy settings)

### Linux
1. Extract: `tar -xzvf SwissNISCalculator-Linux.tar.gz`
2. Make executable: `chmod +x SwissNISCalculator`
3. Run: `./SwissNISCalculator`

### Build from Source

```bash
# Clone the repository
git clone https://github.com/USKAch/Swiss-NIS-Calculator.git
cd Swiss-NIS-Calculator

# Build
dotnet build

# Run
dotnet run --project src/NIS.Desktop

# Publish for your platform
dotnet publish src/NIS.Desktop -c Release -r win-x64 --self-contained
dotnet publish src/NIS.Desktop -c Release -r osx-x64 --self-contained
dotnet publish src/NIS.Desktop -c Release -r linux-x64 --self-contained
```

## Usage

### Quick Start

1. **Create a New Project** - Enter station details (callsign, operator, address)
2. **Add Antenna Configuration** - Select radio, cable, and antenna from master data
3. **Set Evaluation Point** - Define distance and building damping (OKA/LSM/LST/PSS depending on language)
4. **Calculate** - View field strength results and compliance status

### Import/Export Projects

Share station configurations with other users via .nisproj files:

- **Export Project** - Save your project to a portable .nisproj file (includes user-specific master data)
- **Import Project** - Load a project from a .nisproj file (creates missing master data automatically)

Access these features directly from the navigation pane.

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
|   +-- NIS.Desktop/           # Avalonia UI application
|       +-- Calculations/      # Field strength calculator, pattern generator
|       +-- Controls/          # Custom controls (PolarDiagram)
|       +-- Localization/      # Multi-language support
|       +-- Models/            # Data models
|       +-- Services/          # Database, settings, PDF export
|       +-- ViewModels/        # MVVM ViewModels
|       +-- Views/             # XAML Views
|       +-- Data/              # SQLite database, JSON backups
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
| Database | SQLite (Dapper) |
| PDF Export | QuestPDF |

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
