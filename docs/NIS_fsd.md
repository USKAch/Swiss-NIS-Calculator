# Swiss NIS Calculator - Functional Specification

## 1. Purpose

Calculate RF field strength for Swiss amateur radio antenna approval (NISV compliance).

## 2. Application Flow

### 2.1 Welcome Screen

First screen shown when app launches:

- **Language Selection**: de | fr | it | en (4 toggle buttons)
- Default selection: de (Deutsch)
- **Theme Toggle**: Light / Dark mode switch
- Theme applies immediately and persists for the session

### 2.1a Project Selection

Below language/theme selection:

- **New Project** → Navigates to Project Info screen (station details), then Project Overview
- **Open Project** → File picker for .nisproj files, then Project Overview
- **Recent Projects** → List of recently opened projects (optional enhancement)
- **Master Data** → Navigates to Master Data Manager (Section 2.6)

### 2.2 Project Overview (Main Screen)

Primary workspace after project is loaded/created. Users can edit and add configurations here.

**Header Bar**:
- Project name (editable text field with white background)
- Station info summary (callsign)
- Save / Export buttons

**Station Info Panel** (non-collapsible):
- Callsign: HB9FS/HB9BL
- Address: Musterstrasse 1, 8000 Zürich
- Click "Edit" → Navigates to Project Info screen to modify

**Configurations List**:
Each configuration card shows:
- **Primary line**: Antenna name (bold, as primary identifier)
- **Secondary line**: Radio Power | Cable type

Example:
| Antenna (bold) | Details | OKA | Actions |
|----------------|---------|-----|---------|
| Opti-Beam OB9-5 | Yaesu FT-1000 100W \| EcoFlex10 | 5.4m | Edit / Delete |
| Wimo ZX6-2 | Yaesu FT-991 100W \| Aircom-plus | 7.4m | Edit / Delete |

- Configurations are identified by their antenna (no user-defined name)
- "+ Add Configuration" button → Navigates to Configuration Editor (2.4)
- Edit button → Navigates to Configuration Editor with existing data
- Each configuration has its own OKA (evaluation point) with distance and damping

**Action Buttons**:
- "Calculate All" → Runs calculation for all configs → Navigates to Results (2.5)
- "Export Report" → Navigates to Results view with export options (2.5)

### 2.3 Antenna Selection

Antennas are selected via dropdown in the Configuration Editor (same as radios and cables).
- Dropdown shows all antennas from master data
- [Edit] button opens Antenna Master Editor (Section 7.1)
- [+ Add] button opens Antenna Master Editor for new antenna

### 2.4 Configuration Editor

Screen for creating or editing one antenna configuration. Configurations are numbered automatically.

**Section 1: Antenna** (first, most important)
- Antenna: [dropdown from master data] [Edit] [+ Add]
  - Edit → Navigates to Antenna Master Editor (Section 7.1) with selected antenna
  - Add → Navigates to Antenna Master Editor (Section 7.1) for new antenna
- Height: [number] m
- Polarization: [radio buttons] Horizontal | Vertical (mutually exclusive)
  - If Horizontal: Rotation Angle: [number] degrees (default 360)

**Section 2: Transmitter**
- Radio: [dropdown from master data] [Edit] [+ Add]
  - Edit → Navigates to Radio Master Editor (Section 7.3) with selected radio
  - Add → Navigates to Radio Master Editor (Section 7.3) for new radio
  - Includes common HAM transceivers (Icom, Yaesu, Kenwood, Elecraft, FlexRadio) for HF/VHF/UHF
- Linear Amplifier: [manufacturer/model text fields] or "None" (checkbox to enable)
- Output Power: [number] W (at radio or linear output)

**Section 3: Feed Line**
- Cable Type: [dropdown from master data] [Edit] [+ Add]
  - Edit → Navigates to Cable Master Editor (Section 7.2) with selected cable
  - Add → Navigates to Cable Master Editor (Section 7.2) for new cable
- Cable Length: [number] m
- Additional Losses: [number] dB
- Loss Description: [text] (e.g., "Connectors, switch")

**Section 4: Operating Parameters**
- Modulation: [dropdown] SSB (0.2) | CW (0.4) | FM/Digital (1.0)
- Activity Factor: [number] (default 0.5)

These apply to all bands of the selected antenna. Bands (frequency, gain, pattern) are defined by the antenna master data.

**Section 6: Evaluation Point (OKA)**
- Name: [text] (e.g., "Neighbor's balcony")
- Distance: [number] m
- Building Damping: [number] dB

Note: Each configuration has exactly one OKA. OKA = Ort des kurzfristigen Aufenthalts (place of short-term stay).

**Actions**:
- Save → Returns to Project Overview
- Cancel → Discards changes, returns to Project Overview

### 2.6 Master Data Manager

Central hub for managing all master data (antennas, cables, radios). Accessed via "Master Data" button on Welcome screen.

**Navigation Structure**:
```
Master Data Manager
├── Antennas Tab
│   ├── List of all antennas (searchable, filterable)
│   ├── [+ Add Antenna] → Antenna Master Editor
│   └── [Edit] → Antenna Master Editor (with existing data)
│
├── Cables Tab
│   ├── List of all cables (searchable)
│   ├── [+ Add Cable] → Cable Master Editor
│   └── [Edit] → Cable Master Editor (with existing data)
│
└── Radios Tab
    ├── List of all radios (searchable)
    ├── [+ Add Radio] → Radio Master Editor
    └── [Edit] → Radio Master Editor (with existing data)
```

**Actions**:
- Back → Returns to Welcome screen
- Changes to master data are saved to the app's data files (antennas.json, cables.json, radios.json)

**Integration with Configuration Editor**:
When editing a configuration, "Edit" buttons next to antenna/cable/radio selections navigate to the respective Master Data Editor. After saving, user returns to Configuration Editor with updated selection.

### 2.5 Calculation & Results

Results displayed after "Calculate All":

**Per Configuration**:
- Summary: Pass or Fail with highest field strength
- Detailed table (see Section 4 Output Format)

**Safety Distance Visualization** (optional):
- Diagram showing antenna position and calculated safety distances

**Export Options**:
- Markdown Export: Generates formatted tables per FSD Section 4

**Actions**:
- Close → Returns to Project Overview
- Export Markdown

## 3. Calculation

### 3.1 Input Parameters

| Parameter | Symbol | Unit | Source |
|-----------|--------|------|--------|
| Frequency | f | MHz | User selection |
| Distance to antenna | d | m | User input per OKA |
| TX Power | P | W | User input |
| Activity factor | AF | - | Default 0.5 |
| Modulation factor | MF | - | SSB=0.2, CW=0.4, FM=1.0 |
| Cable attenuation | a1 | dB | Calculated from project cable data |
| Additional losses | a2 | dB | User input (connectors, switches) |
| Antenna gain | g1 | dBi | From project antenna data |
| Vertical angle attenuation | g2 | dB | From antenna pattern |
| Building damping | ag | dB | User input |
| Ground reflection factor | kr | - | Fixed 1.6 |

**Data Source Policy**: Calculations use **only project data** (customAntennas, customCables). When a configuration is saved, any selected antenna or cable from master data is automatically copied to the project file. Radio is stored as a reference (manufacturer/model) with power specified in the configuration. This ensures:
- Project files are self-contained and portable
- Calculations are reproducible regardless of master data changes
- Projects can be shared without requiring identical master data

### 3.2 Vertical Angle and Pattern

The vertical angle attenuation (g2) is automatically calculated based on the geometry of the antenna installation:

**Angle Calculation:**
```
Vertical Angle = atan(Antenna Height / Horizontal Distance to OKA)
```

Where:
- 0° = Looking horizontally (OKA very far away)
- 90° = Looking straight down (OKA directly below antenna)

**Pattern Lookup:**
The pattern array contains 10 values representing attenuation in dB at angles 0°, 10°, 20°, 30°, 40°, 50°, 60°, 70°, 80°, 90°:

| Index | Angle | Description |
|-------|-------|-------------|
| 0 | 0° | Horizon (maximum radiation) |
| 1 | 10° | Slight downward angle |
| ... | ... | ... |
| 9 | 90° | Straight down toward OKA |

**Example:**
- Antenna height: 12m
- OKA distance: 5.4m horizontally
- Vertical angle: atan(12/5.4) = 65.8° ≈ 66°
- Pattern lookup: interpolate between index 6 (60°) and index 7 (70°)

### 3.3 Formulas

```
Mean power:           Pm = P × AF × MF
Total attenuation:    a = a1 + a2
Attenuation factor:   A = 10^(-a/10)
Total antenna gain:   g = g1 - g2
Gain factor:          G = 10^(g/10)
EIRP:                 Ps = Pm × A × G
ERP:                  P's = Ps / 1.64
Building factor:      AG = 10^(-ag/10)
Field strength:       E' = 1.6 × sqrt(30 × Pm × A × G × AG) / d
Safety distance:      ds = 1.6 × sqrt(30 × Pm × A × G × AG) / EIGW
```

### 3.3 NIS Limits (Swiss NISV)

| Frequency | Limit (V/m) |
|-----------|-------------|
| 1.8 MHz | 64.7 |
| 3.5 MHz | 46.5 |
| 7 MHz | 32.4 |
| 10-28 MHz | 28 |
| 50 MHz | 28 |
| 144 MHz | 28 |
| 432 MHz | 28.6 |
| 1240 MHz | 48.5 |
| 2300+ MHz | 61 |

## 4. Output Format (Markdown)

One table per antenna configuration:

---

## Immissionsberechnung für HB9FS/HB9BL

| | |
|:------------------------|:-------------------------------------------------------|
| **Sender:**             | 100W                                                   |
| **Linear:**             | —                                                      |
| **Antenne:**            | Opti-Beam OB9-5                                        |
| **Horizontal drehbar:** | Ja, Winkel: 360 Grad                                   |
| **Vertikal drehbar:**   | Nein                                                   |

| Parameter                          | Sym  | Unit   |    14 |    18 |    21 |    24 |    28 |
|:-----------------------------------|:----:|:------:|------:|------:|------:|------:|------:|
| Frequenz                           | f    | MHz    |    14 |    18 |    21 |    24 |    28 |
| Nr. des OKA auf Situationsplan     |      |        |     1 |     1 |     1 |     1 |     1 |
| Abstand OKA zur Antenne            | d    | m      |   5.4 |   5.4 |   5.4 |   5.4 |   5.4 |
| Leistung am Senderausgang          | P    | W      |   100 |   100 |   100 |   100 |   100 |
| Aktivitätsfaktor                   | AF   |        |  0.50 |  0.50 |  0.50 |  0.50 |  0.50 |
| Modulationsfaktor                  | MF   |        |  0.40 |  0.40 |  0.40 |  0.40 |  0.40 |
| Mittl. Leistung am Senderausgang   | Pm   | W      | 20.00 | 20.00 | 20.00 | 20.00 | 20.00 |
| Kabeldämpfung                      | a1   | dB     |  0.22 |  0.25 |  0.27 |  0.30 |  0.32 |
| Übrige Dämpfung                    | a2   | dB     |  1.00 |  1.00 |  1.00 |  1.00 |  1.00 |
| Summe der Dämpfung                 | a    | dB     |  1.22 |  1.25 |  1.27 |  1.30 |  1.32 |
| Dämpfungsfaktor                    | A    |        |  0.76 |  0.75 |  0.75 |  0.74 |  0.74 |
| Antennengewinn                     | g1   | dBi    |  6.33 |  6.71 |  6.76 |  6.64 |  6.59 |
| Vertikale Winkeldämpfung           | g2   | dB     |  3.30 |  0.00 |  0.00 |  0.00 |  0.00 |
| Totaler Antennengewinn             | g    | dB     |  3.03 |  6.71 |  6.76 |  6.64 |  6.59 |
| Antennengewinnfaktor               | G    |        |  2.01 |  4.69 |  4.74 |  4.61 |  4.56 |
| Massgebende Sendeleistung (EIRP)   | Ps   | W      | 30.34 | 70.31 | 70.80 | 68.40 | 67.30 |
| Massgebende Sendeleistung (ERP)    | P's  | W      | 18.50 | 42.87 | 43.17 | 41.70 | 41.04 |
| Gebäudedämpfung                    | ag   | dB     |  0.00 |  0.00 |  0.00 |  0.00 |  0.00 |
| Gebäudedämpfungsfaktor             | AG   |        |  1.00 |  1.00 |  1.00 |  1.00 |  1.00 |
| Bodenreflexionsfaktor              | kr   |        |  1.60 |  1.60 |  1.60 |  1.60 |  1.60 |
| **Massgebende Feldstärke am OKA**  | E'   | V/m    |  8.94 | 13.61 | 13.66 | 13.42 | 13.31 |
| **Immissions-Grenzwert**           | EIGW | V/m    | 28.00 | 28.00 | 28.00 | 28.00 | 28.00 |
| **Sicherheitsabstand**             | ds   | m      |  1.72 |  2.62 |  2.63 |  2.59 |  2.57 |

### Erläuterungen zu den verschiedenen Tabellenspalten

| Parameter                        | Beschreibung                                                                 |
|:---------------------------------|:-----------------------------------------------------------------------------|
| Frequenz                         | Sendefrequenz der Amateurfunkstation                                         |
| Nr. des OKA auf Situationsplan   | Im Situationsplan eingezeichneter Ort für den kurzfristigen Aufenthalt       |
| Abstand OKA zur Antenne          | Antenne - Ort für den kurzfristigen Aufenthalt                               |
|                                  | Horizontalprojektion (Ja/Nein): Nein                                         |
|                                  | Effektive Distanz (Ja/Nein): Ja                                              |
| Leistung am Senderausgang        | Ausgangsleistung des Senders oder Linears                                    |
| Aktivitätsfaktor                 | In der Regel AF = 0.5                                                        |
| Modulationsfaktor                | bei SSB: MF=0.2, bei CW: MF=0.4, bei FM/RTTY/PSK31: MF=1.0                   |
| Mittl. Leistung am Senderausgang | Ausgangsleistung reduziert um Aktivitäts- und Modulationsfaktor              |
| Kabeldämpfung                    | 15.00 m EcoFlex10                                                            |
| Übrige Dämpfung                  | Stecker 1.00 dB                                                              |
| Summe der Dämpfung               | Kabeldämpfung + übrige Dämpfung                                              |
| Dämpfungsfaktor                  | In absolute Zahl umgerechnete "Summe der Dämpfungen"                         |
| Antennengewinn                   | Maximaler Gewinn der Antenne gemäss Hersteller                               |
| Vertikale Winkeldämpfung         | Gewinnverminderung, wegen vertikalem Strahlungsdiagramm der Antenne          |
| Totaler Antennengewinn           | Antennengewinn - vertikale Winkeldämpfung                                    |
| Antennengewinnfaktor             | In absolute Zahl umgerechneter "Antennengewinn"                              |
| Massgebende Sendeleistung (EIRP) | Äquivalente abgestrahlte Leistung bezogen auf einen isotropen Strahler       |
| Massgebende Sendeleistung (ERP)  | Äquivalente abgestrahlte Leistung bezogen auf einen Dipol                    |
| Gebäudedämpfung                  | Dämpfung durch Gebäudemauern und Decken                                      |
| Gebäudedämpfungsfaktor           | In absolute Zahlen umgerechnete "Gebäudedämpfung"                            |
| Bodenreflexionsfaktor            | Faktor welcher zu einer Zunahme der Feldstärke führt                         |
| Massgebende Feldstärke am OKA    | 6-Minuten-Mittelwert der Feldstärke am Ort für den kurzfristigen Aufenthalt  |
| Immissions-Grenzwert             | Immissions-Grenzwert für die elektrische Feldstärke gemäss NISV              |
| Sicherheitsabstand               | Distanz von der Antenne, wo der Immissions-Grenzwert erreicht wird           |

---

**Datum:** 19/10/2023
**Unterschrift:**

---

## 5. Data Architecture

### 5.1 Master Data (shipped with app, read-only)

**antennas.json**
```json
{
  "antennas": [
    {
      "manufacturer": "Opti-Beam",
      "model": "OB9-5",
      "isRotatable": true,
      "bands": [
        {
          "frequencyMHz": 14,
          "gainDbi": 6.33,
          "pattern": [0, 0.04, 0.21, 0.48, 0.93, 1.54, 2.32, 3.30, 4.48, 5.90]
        },
        {
          "frequencyMHz": 18,
          "gainDbi": 6.71,
          "pattern": [0, 0.07, 0.28, 0.63, 1.14, 1.83, 2.71, 3.80, 5.12, 6.72]
        }
      ]
    }
  ]
}
```

**cables.json**
```json
{
  "cables": [
    {
      "name": "EcoFlex10",
      "attenuationPer100m": {
        "1.8": 0.478, "3.5": 0.683, "7": 0.994, "14": 1.457,
        "21": 1.790, "28": 2.141, "50": 2.850, "144": 4.800, "432": 8.900
      }
    }
  ]
}
```

**radios.json**
```json
{
  "radios": [
    {
      "manufacturer": "Yaesu",
      "model": "FT-1000",
      "maxPowerWatts": 200
    }
  ]
}
```

### 5.2 Project File (user data, one per station)

**HB9FS_Station.nisproj**
```json
{
  "name": "HB9FS Station",
  "language": "de",
  "station": {
    "callsign": "HB9FS/HB9BL",
    "address": "Musterstrasse 1, 8000 Zürich"
  },

  "configurations": [
    {
      "radio": {
        "manufacturer": "Yaesu",
        "model": "FT-1000"
      },
      "linear": null,
      "powerWatts": 100,
      "cable": {
        "type": "EcoFlex10",
        "lengthMeters": 15,
        "additionalLossDb": 1.0,
        "additionalLossDescription": "Stecker 1.00 dB"
      },
      "antenna": {
        "manufacturer": "Opti-Beam",
        "model": "OB9-5",
        "heightMeters": 12,
        "isHorizontallyRotatable": true,
        "horizontalAngleDegrees": 360,
        "isVerticallyRotatable": false
      },
      "modulationFactor": 0.4,
      "activityFactor": 0.5,
      "okaName": "Neighbor balcony",
      "okaDistanceMeters": 5.4,
      "okaBuildingDampingDb": 0
    }
  ],

  "customAntennas": [],
  "customCables": [],
  "customRadios": []
}
```

## 6. Languages

Application and output must support 4 languages:

| Code | Language |
|------|----------|
| de   | Deutsch (German) |
| fr   | Français (French) |
| it   | Italiano (Italian) |
| en   | English |

- All UI labels, parameter names, explanations, and output documents available in all 4 languages
- Language is selected per project (selected when a new project is started and stored in project file)
- Output document uses project language

## 7. Master Data Editors

All master data editors follow a full-width layout and can be accessed from:
1. Master Data Manager (Welcome Screen → Master Data)
2. Configuration Editor (Edit/Add buttons next to each component selection)

When accessed from Configuration Editor, the editor returns to the configuration after save.

### 7.1 Antenna Master Editor

Full-featured editor for antenna definitions with complete band and pattern data.

**Layout**: Full window width with scrollable content

**Header Section**:
- Manufacturer: [text field, required]
- Model: [text field, required]
- Is Rotatable: [checkbox] "Horizontally rotatable"

**Frequency Bands Section**:

Each band in an expandable card showing:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│ Band: 14 MHz                                                    [Expand] [X]│
├─────────────────────────────────────────────────────────────────────────────┤
│ Frequency: [14.0    ] MHz    Gain: [6.33   ] dBi                            │
│                                                                             │
│ Vertical Radiation Pattern (attenuation in dB):                             │
│ ┌─────┬─────┬─────┬─────┬─────┬─────┬─────┬─────┬─────┬─────┐              │
│ │ 0°  │ 10° │ 20° │ 30° │ 40° │ 50° │ 60° │ 70° │ 80° │ 90° │              │
│ ├─────┼─────┼─────┼─────┼─────┼─────┼─────┼─────┼─────┼─────┤              │
│ │ 0.0 │ 0.0 │ 0.2 │ 0.5 │ 0.9 │ 1.5 │ 2.3 │ 3.3 │ 4.5 │ 5.9 │              │
│ └─────┴─────┴─────┴─────┴─────┴─────┴─────┴─────┴─────┴─────┘              │
│                                                                             │
│ Pattern preview: 0°=0.0, 30°=0.5, 60°=2.3, 90°=5.9 dB                       │
└─────────────────────────────────────────────────────────────────────────────┘
```

**Pattern Values**:
- 0° = Horizon (typically 0 dB, maximum radiation)
- 90° = Straight down toward OKA (higher attenuation)
- Values represent attenuation in dB relative to maximum gain

**Actions**:
- [+ Add Band] → Adds new band with default values (50 MHz, 0 dBi, flat pattern)
- [Save] → Saves to master data (antennas.json)
- [Cancel] → Returns without saving

### 7.2 Cable Master Editor

Editor for cable definitions with frequency-dependent attenuation data.

**Layout**: Full window width

**Header Section**:
- Cable Name: [text field, required] (e.g., "EcoFlex10", "RG-213")

**Attenuation Data Section**:

Table showing attenuation per 100m at standard frequencies:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│ Attenuation Data (dB per 100m)                                              │
├──────────────┬──────────────┬──────────────┬──────────────┬────────────────┤
│ Frequency    │ Attenuation  │ Frequency    │ Attenuation  │                │
├──────────────┼──────────────┼──────────────┼──────────────┤                │
│ 1.8 MHz      │ [0.478    ]  │ 50 MHz       │ [2.850    ]  │                │
│ 3.5 MHz      │ [0.683    ]  │ 144 MHz      │ [4.800    ]  │                │
│ 7 MHz        │ [0.994    ]  │ 430 MHz      │ [8.900    ]  │                │
│ 10 MHz       │ [1.208    ]  │ 1240 MHz     │ [16.372   ]  │                │
│ 14 MHz       │ [1.457    ]  │ 2300 MHz     │ [23.921   ]  │                │
│ 18 MHz       │ [1.652    ]  │ 5650 MHz     │ [         ]  │                │
│ 21 MHz       │ [1.790    ]  │ 10000 MHz    │ [         ]  │                │
│ 24 MHz       │ [1.997    ]  │              │              │                │
│ 28 MHz       │ [2.141    ]  │              │              │                │
└──────────────┴──────────────┴──────────────┴──────────────┴────────────────┘
```

**Notes**:
- Empty values are allowed (cable may not have data for all frequencies)
- Calculation will interpolate between available frequencies
- Standard amateur radio frequencies are pre-populated

**Actions**:
- [Save] → Saves to master data (cables.json)
- [Cancel] → Returns without saving

### 7.3 Radio Master Editor

Editor for radio/transceiver definitions.

**Layout**: Full window width

**Fields**:
- Manufacturer: [text field, required] (e.g., "Yaesu", "Icom", "Kenwood")
- Model: [text field, required] (e.g., "FT-1000", "IC-7300")
- Max Power: [number] W (maximum output power rating)

**Optional Fields**:
- Frequency Coverage: [text] (e.g., "HF+6m", "VHF/UHF", "All-band")
- Notes: [text area] (additional information)

**Actions**:
- [Save] → Saves to master data (radios.json)
- [Cancel] → Returns without saving

### 7.4 Master Data Storage

Master data is stored in JSON files in the application's data directory:

| File | Content | Location |
|------|---------|----------|
| antennas.json | Antenna definitions with bands and patterns | data/antennas/ |
| cables.json | Cable definitions with attenuation tables | data/cables/ |
| radios.json | Radio/transceiver definitions | data/radios/ |

**Data Flow**:
1. Master data is loaded at application startup
2. Editors modify master data in memory
3. Save writes to JSON files
4. When used in a configuration, master data is copied to project file (customAntennas, customCables)
5. Calculations use only project data (not master data references)

### 7.5 Constants (Read-Only)

Fixed calculation constants per Swiss NISV regulations:

| Constant | Value | Description |
|----------|-------|-------------|
| Ground Reflection Factor (kr) | 1.6 | Fixed for all calculations |
| Default Activity Factor | 0.5 | Default value for configurations |

Note: These constants are defined by regulation and cannot be modified.

## 8. Navigation Flow

```
Welcome Screen
    ├── [New Project] → Project Info → Project Overview
    ├── [Open Project] → Project Overview
    └── [Master Data] → Master Data Manager
            ├── [Add/Edit Antenna] → Antenna Editor → Master Data Manager
            ├── [Add/Edit Cable] → Cable Editor → Master Data Manager
            ├── [Add/Edit Radio] → Radio Editor → Master Data Manager
            └── [Back] → Welcome Screen

Project Overview
    ├── [Edit Station Info] → Project Info → Project Overview
    ├── [Add/Edit Configuration] → Configuration Editor
    │       ├── [Select Antenna] → Antenna Selector → Configuration Editor
    │       ├── [Edit/Add Antenna] → Antenna Editor → Configuration Editor
    │       ├── [Edit/Add Cable] → Cable Editor → Configuration Editor
    │       ├── [Edit/Add Radio] → Radio Editor → Configuration Editor
    │       └── [Save/Cancel] → Project Overview
    └── [Calculate All] → Results → Project Overview
```

## 9. Theme Support

- **Light Mode**: Default theme
- **Dark Mode**: Toggle available on Welcome screen
- Theme applies globally to the entire application

## 10. Example: HB9FS Station

| Config | Radio | Cable | Antenna | Height | Bands | OKA |
|--------|-------|-------|---------|--------|-------|-----|
| HF Station | 100W | EcoFlex10 15m | Opti-Beam OB9-5 | 12m | 14,18,21,24,28 MHz | Neighbor balcony @ 5.4m |
| 6m Station | 100W | Aircom-plus 20m | Wimo ZX6-2 | 10m | 50 MHz | Garden fence @ 7.4m |
| VHF/UHF | 100W | Aircom-plus 17m | Diamond X-50 | 14m | 144, 432 MHz | Terrace @ 10.4m |

Each configuration includes antenna height and its own evaluation point (OKA) with distance and optional building damping.
