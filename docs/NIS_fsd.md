# Swiss NIS Calculator - Functional Specification

## 1. Purpose

Calculate RF field strength for Swiss amateur radio antenna approval (NISV compliance).

## 2. Application Flow

### 2.1 Welcome Screen

First screen shown when app launches:

- **Language Selection**: de | fr | it | en (4 toggle buttons, see Section 6 for details)
- **Theme Toggle**: Light / Dark mode switch
- **New Project** → Navigates to Project Info screen (station details), then Project Overview
- **Open Project** → File picker for .nisproj files, then Project Overview
- **Recent Projects** → List of recently opened projects (optional enhancement)
- **Master Data** → Navigates to Master Data Manager (Section 2.5)

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
- "+ Add Configuration" button → Navigates to Configuration Editor (Section 2.4)
- Edit button → Navigates to Configuration Editor with existing data
- Each configuration has its own OKA (evaluation point) with distance and damping

**Action Buttons**:
- "Calculate All" → Runs calculation for all configs → Navigates to Results (Section 2.6)
- "Export Report" → Navigates to Results view with export options (Section 2.6)

### 2.3 Component Selection

All component dropdowns (Antenna, Radio, Cable) share the same behavior:
- **Sorted alphabetically** by name/manufacturer
- **Text search enabled**: Type characters to jump to matching item (e.g., type "Y" to jump to "Yaesu")
- [Edit] button opens the respective Master Editor
- [+ Add] button opens the Master Editor for a new item

### 2.4 Configuration Editor

Screen for creating or editing one antenna configuration. Header shows "Configuration {number}" (e.g., "Configuration 1").

**Section 1: Antenna** (first, most important)
- Antenna: [dropdown from master data] [Edit] [+ Add]
  - Edit → Navigates to Antenna Master Editor (Section 7.1) with selected antenna
  - Add → Navigates to Antenna Master Editor (Section 7.1) for new antenna
- Height: [number] m
- Polarization: [radio buttons] Horizontal | Vertical (mutually exclusive)
  - If Horizontal: Rotation Angle: [number] degrees (default 360) - *information only, not used in calculation*

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

### 2.5 Master Data Manager

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

### 2.6 Calculation & Results

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

### 3.3 Ground Reflection Factor (kr)

The ground reflection factor (Bodenreflexionsfaktor) accounts for constructive interference between the direct wave from the antenna and its reflection from the ground at evaluation points near ground level.

**Value:** kr = 1.6 (fixed, per NISV Anhang 2)

**Physical Effect:**
- Field strength (E) is multiplied by 1.6
- Power density (S = E²/377) is multiplied by 1.6² = **2.56**
- Equivalent to adding **4.08 dB** to the effective gain

This factor ensures worst-case field strength estimation at locations where a person might stand, where direct and reflected waves can combine constructively.

### 3.4 Formulas

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

### 3.5 NIS Limits (Swiss NISV)

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
      "antennaType": "yagi",
      "bands": [
        {
          "frequencyMHz": 14,
          "gainDbi": 6.33,
          "pattern": [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
        },
        {
          "frequencyMHz": 18,
          "gainDbi": 6.71,
          "pattern": [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
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

  "antennaConfigurations": [
    {
      "name": "HF Station",
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

## 6. Language Handling

### 6.1 Supported Languages

| Code | Language | Display Name |
|------|----------|--------------|
| de   | German   | Deutsch      |
| fr   | French   | Français     |
| it   | Italian  | Italiano     |
| en   | English  | English      |

Default language: **de** (Deutsch)

### 6.2 Language Selection

**On Welcome Screen:**
- Four toggle buttons displayed horizontally: de | fr | it | en
- Language change takes effect immediately across the entire UI
- Selected language persists for the session

**Per Project:**
- Language selected when creating a new project is stored in project file
- Export/report output uses the project's language setting

### 6.3 Localization Architecture

The application uses a centralized localization system:

**Strings Singleton (`NIS.Desktop.Localization.Strings`):**
- Single source of truth for all translatable strings
- Implements `INotifyPropertyChanged` for live UI updates
- Properties for each translatable string (e.g., `Save`, `Cancel`, `Antenna`)
- `Language` property controls active language

**XAML Binding Pattern:**
```xml
<TextBlock Text="{Binding Save, Source={x:Static loc:Strings.Instance}}"/>
<Button Content="{Binding Cancel, Source={x:Static loc:Strings.Instance}}"/>
```

### 6.4 Translation Storage

**Embedded Translations:**
- Default translations are compiled into the application
- Stored in a dictionary structure: `Key → { Language → Value }`
- Covers all UI strings, labels, messages, and tooltips

**Custom Translations (AppData):**
- User modifications saved to: `%APPDATA%/SwissNISCalculator/translations.json`
- Merged with embedded translations at startup
- Custom translations override embedded defaults

### 6.5 In-App Translation Editor

Accessible via: **Welcome Screen → Master Data → Translations Tab**

**Features:**
- DataGrid showing all translatable strings
- Columns: Key | Category | German | French | Italian | English
- Inline editing for all language values
- Search/filter by key or text content
- Category filter dropdown

**Categories:**
- Welcome, ProjectInfo, ProjectOverview, ConfigEditor
- AntennaEditor, CableEditor, RadioEditor
- MasterData, Results, Validation, Common

**Workflow:**
1. Navigate to Master Data → Translations tab
2. Search or filter to find desired string
3. Edit values directly in the grid
4. Click "Save" to persist changes
5. Changes take effect immediately

**Unsaved Changes Protection:**
- Warning dialog when leaving with unsaved changes
- Options: Save / Discard / Cancel

### 6.6 String Categories

| Category | Description | Examples |
|----------|-------------|----------|
| Common | Shared buttons, labels | Save, Cancel, Edit, Delete, Close |
| Welcome | Welcome screen elements | NewProject, OpenProject, MasterData |
| ProjectInfo | Station information | Callsign, Address, CreateProject |
| ProjectOverview | Main project screen | Configurations, CalculateAll |
| ConfigEditor | Configuration editing | Antenna, Transmitter, FeedLine |
| AntennaEditor | Antenna management | FrequencyBands, Gain, Pattern |
| CableEditor | Cable management | CableDetails, AttenuationData |
| RadioEditor | Radio management | RadioDetails, MaxPower |
| MasterData | Master data manager | Antennas, Cables, Radios |
| Results | Calculation results | Pass, Fail, FieldStrength |
| Validation | Error messages | ValidationRequired, ValidationModel |

### 6.7 Adding New Translations

When adding new UI elements:

1. Add property to `Strings.cs`:
   ```csharp
   public string NewLabel => Get("NewLabel");
   ```

2. Add category mapping:
   ```csharp
   ["NewLabel"] = "CategoryName",
   ```

3. Add translations for all 4 languages:
   ```csharp
   ["NewLabel"] = new() {
       ["de"] = "Neue Beschriftung",
       ["fr"] = "Nouvelle étiquette",
       ["it"] = "Nuova etichetta",
       ["en"] = "New Label"
   },
   ```

4. Use in XAML:
   ```xml
   <TextBlock Text="{Binding NewLabel, Source={x:Static loc:Strings.Instance}}"/>
   ```

### 6.8 Output Document Language

- Markdown export uses the project's stored language
- All parameter names, descriptions, and explanations are translated
- Numerical values and units remain unchanged across languages

## 7. Master Data Editors

All master data editors follow a full-width layout and can be accessed from:
1. Master Data Manager (Welcome Screen → Master Data)
2. Configuration Editor (Edit/Add buttons next to each component selection)

When accessed from Configuration Editor, the editor returns to the configuration after save.

**UI Controls**:
- All numeric input fields use NumericUpDown controls without spinner buttons for clean appearance
- Validation errors are displayed in the footer bar with clear, actionable messages
- Required fields show validation message when empty on save attempt

### 7.1 Antenna Master Editor

Full-featured editor for antenna definitions with complete band and pattern data.

**Layout**: Full window width with scrollable content

**Antenna Details Section** (two-line layout):
- Line 1: Manufacturer [text field, wide] + Model [text field, wide]
- Line 2: Polarization [Horizontal | Vertical radio buttons] + Rotatable options

**Polarization and Rotation Logic**:
- Polarization: Radio buttons for Horizontal or Vertical (mutually exclusive)
- Rotatable: Checkbox only visible for horizontally polarized antennas
- Rotation Angle: Numeric field (0-360°) only visible when Rotatable is checked
- **Vertical antennas cannot be rotatable** (checkbox hidden)

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

**Vertical Radiation Pattern** (confirmed from VB6 source: "Vertikale Winkeldämpfung"):
- 0° = Horizon (typically 0 dB, maximum radiation direction)
- 90° = Straight up (or down, pattern is symmetric)
- Values represent attenuation in dB relative to maximum gain at each elevation angle
- Used to calculate field strength reduction when OKA is not at antenna height

**Input Validation**:
- Gain: -20 to 50 dBi
- Pattern values: 0 to 60 dB attenuation
- Rotation angle: 0 to 360 degrees

**Actions**:
- [+ Add Band] → Adds new band with default values (50 MHz, 0 dBi, flat pattern)
- [Save] → Validates all fields, shows error message if invalid
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

**Input Validation**:
- Cable name: Required, cannot be empty
- Attenuation values: 0 to 500 dB/100m (varies by frequency band)

**Actions**:
- [Save] → Validates all fields, shows error message if invalid
- [Cancel] → Returns without saving

### 7.3 Radio Master Editor

Editor for radio/transceiver definitions.

**Layout**: Full window width

**Fields**:
- Manufacturer: [text field, required] (e.g., "Yaesu", "Icom", "Kenwood")
- Model: [text field, required] (e.g., "FT-1000", "IC-7300")
- Max Power: [number] W (maximum output power rating, 1-10000W)

**Input Validation**:
- Manufacturer: Required, cannot be empty
- Model: Required, cannot be empty
- Max Power: 1 to 10000 W

**Note**: The radio is used for documentation purposes in the NIS report. The actual output power is specified in each antenna configuration.

**Actions**:
- [Save] → Validates all fields, shows error message if invalid
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

## 10. Unsaved Changes Protection

When the user attempts to close the application with unsaved project changes:

**Dialog**: "Unsaved Changes"
- Message: "Do you want to save your changes before closing?"
- Buttons: [Yes] [No] [Cancel]

**Behavior**:
- **Yes**: Save project, then close application
- **No**: Discard changes, close application
- **Cancel**: Return to application, do not close

This ensures users don't accidentally lose their work.

## 11. Antenna Type Classification

### 11.1 Overview

Each antenna in the master data includes an `antennaType` field that classifies the antenna type. This classification is used to determine which antennas can have vertical radiation patterns applied for NIS calculations.

### 11.2 The antennaType Field

```json
{
  "manufacturer": "Cushcraft",
  "model": "A-3-S",
  "isRotatable": true,
  "antennaType": "yagi",
  "bands": [...]
}
```

Valid values (alphabetical order):
| Value | Description |
|-------|-------------|
| `log-periodic` | Log-periodic dipole arrays (LPDA) |
| `loop` | Loop antennas (delta loops, magnetic loops) |
| `other` | Antennas not fitting other categories |
| `quad` | Quad antennas (cubical quad, X-Quad) |
| `vertical` | Vertical antennas (ground planes, collinear) |
| `wire` | Wire antennas (dipoles, G5RV, longwires) |
| `yagi` | Yagi-Uda directional beam antennas |

### 11.3 Vertical Pattern Data Format

Each antenna band includes a `pattern` array with 10 values representing attenuation at elevation angles 0° to 90° in 10° steps:
- Index 0: 0° (horizon) - typically 0 dB (maximum radiation)
- Index 1: 10°
- ...
- Index 9: 90° (zenith)

Currently all patterns are set to `[0,0,0,0,0,0,0,0,0,0]`. When specific manufacturer pattern data becomes available, it can be entered here.

### 11.4 Pattern Generation Formulas (Reference)

#### 11.4.1 Directional Antennas (Yagi, Quad, Log-Periodic)

For generating patterns for directional antennas based on gain:

**Vertical Half-Power Beamwidth (HPBW):**
```
HPBW = 105° / √(G_linear)
where: G_linear = 10^(G_dBi / 10)
```

**Attenuation at angle θ:**
```
For θ ≤ θ_hp (θ_hp = HPBW / 2):
  Attenuation(θ) = 3 × (θ / θ_hp)²

For θ > θ_hp:
  Attenuation(θ) = 3 + ((θ - θ_hp) / (90 - θ_hp)) × (A_zenith - 3)
  where: A_zenith = min(35, max(20, 20 + (G_dBi - 6)))
```

#### 11.4.2 Vertical Antennas (Omnidirectional)

For generating patterns for omnidirectional vertical antennas based on gain:

**Vertical Half-Power Beamwidth (HPBW):**
```
HPBW = 105° / √(G_linear)
where: G_linear = 10^(G_dBi / 10)
```

**Attenuation at angle θ:**
```
Attenuation(θ) = min(A_zenith, Rolloff × (θ / θ_hp)²)

where:
  θ_hp = HPBW / 2
  Rolloff = 3.0 (ensures 3 dB loss at half beamwidth)
  A_zenith = 20 + (G_dBi × 1.5)
```

The vertical antenna formula uses a simpler quadratic rolloff model with a gain-dependent zenith attenuation cap, appropriate for the broader vertical patterns of collinear and ground-plane antennas.

### 11.5 Antenna Classification Summary

| antennaType | Count | Examples |
|-------------|-------|----------|
| log-periodic | 14 | Titanex LP series, Titan DLP/LP, Cushcraft ASL201 |
| loop | 9 | DeltaLoop, Magnetic Loop |
| quad | 7 | Quad-2El, Quad-3El, X-Quad, Cubex |
| vertical | 34 | GP, Diamond X series, Cushcraft R series |
| wire | 26 | Dipol, G5RV, W3DZZ, Doublet, Langdraht |
| yagi | 229 | Cushcraft A-3-S, Opti-Beam, Hy-Gain TH series, SteppIR |

*Total: 319 antennas. See antennas.json for complete data.*

## 12. Example: HB9FS Station

| Config | Radio | Cable | Antenna | Height | Bands | OKA |
|--------|-------|-------|---------|--------|-------|-----|
| HF Station | 100W | EcoFlex10 15m | Opti-Beam OB9-5 | 12m | 14,18,21,24,28 MHz | Neighbor balcony @ 5.4m |
| 6m Station | 100W | Aircom-plus 20m | Wimo ZX6-2 | 10m | 50 MHz | Garden fence @ 7.4m |
| VHF/UHF | 100W | Aircom-plus 17m | Diamond X-50 | 14m | 144, 432 MHz | Terrace @ 10.4m |

Each configuration includes antenna height and its own evaluation point (OKA) with distance and optional building damping.

## Appendix A: Antennas with Generated Vertical Radiation Patterns

The following antennas have vertical radiation patterns generated using the gain-based formulas from Section 11.4. Directional antennas (Yagi, Quad, Log-Periodic) use the formula from Section 11.4.1, while omnidirectional vertical antennas use Section 11.4.2. Patterns are calculated for each frequency band based on antenna gain.

### A.1 Log-Periodic Antennas

| Manufacturer | Model | Gain (dBi) | Bands |
|--------------|-------|------------|-------|
| Cushcraft | ASL201 | 6.4 | 5 |
| Titan | DLP-19 | 1.0-7.2 | 7 |
| Titan | LP-8 | 5.2-6.0 | 5 |
| Titanex | DLP-10_6 | 8.2 | 2 |
| Titanex | LP-10 | 9.2 | 5 |
| Titanex | LP-1030 | 9.2 | 6 |
| Titanex | LP-11 | 5.6-6.2 | 5 |
| Titanex | LP-12 | 10.2 | 5 |
| Titanex | LP-15 | 4.0-7.2 | 5 |
| Titanex | LP-1830 | 3.8-5.0 | 4 |
| Titanex | LP-5 | 4.2-5.6 | 5 |
| Titanex | LP-6 | 7.7 | 5 |
| Titanex | LP-7 | 7.8 | 6 |
| Titanex | LPDA-5 | 4.2-5.6 | 5 |

### A.2 Quad Antennas

| Manufacturer | Model | Gain (dBi) | Bands |
|--------------|-------|------------|-------|
| Allgemein | Quad-2El | 7.2 | 5 |
| Allgemein | Quad-3El | 8.9 | 5 |
| Cubex | 2-El_Cubex | 0.0-7.7 | 6 |
| Cubex | 2-El_SpitzCu | 6.6-7.9 | 5 |
| Hy-Gain | 2-El_Quad | 7.7-7.8 | 3 |
| Wimo | X-Quad-2 | 12.7 | 1 |
| Wimo | X-Quad-70 | 14.9 | 1 |

### A.3 Yagi Antennas

| Manufacturer | Model | Gain (dBi) | Bands |
|--------------|-------|------------|-------|
| Allgemei | Yagi-3El | 6.2-6.7 | 11 |
| Allgemein | HB9CV | 2.2-6.7 | 9 |
| Allgemein | Helix | 9.7 | 3 |
| Allgemein | Helix-2 | 10.9-17.1 | 3 |
| Allgemein | Yagi-2El | 5.9-6.2 | 11 |
| Allgemein | Yagi-4El | 6.2-8.4 | 11 |
| Allgemein | Yagi-5El | 6.2-10.2 | 11 |
| Allgemein | Yagi-6El | 6.2-11.3 | 11 |
| Allgemein | Yagi-7El | 6.2-12.6 | 11 |
| Allgemein | Yagi-8El | 13.4 | 10 |
| Barker & Williamson | BWD-90 | 0.6-3.0 | 6 |
| Butterfly | HF-5 | 2.0-6.3 | 5 |
| Comet | CA-52HB4 | 10.4 | 1 |
| Cush-Craft | R8 | 0.2-1.8 | 7 |
| Cush-Craft | XM510 | 9.1 | 1 |
| Cush-Craft | XM515 | 8.9 | 1 |
| Cush-Craft | XM520 | 8.6 | 1 |
| Cush_Craft | X7 | 7.1-8.4 | 3 |
| Cushcraft | 10-3CD | 8.0 | 1 |
| Cushcraft | 124WB | 10.2 | 1 |
| Cushcraft | 13B2N | 15.8 | 1 |
| Cushcraft | 15-4CD | 10.0 | 1 |
| Cushcraft | 20-4CD | 10.0 | 1 |
| Cushcraft | 40-2CD | 5.5 | 1 |
| Cushcraft | 719B | 15.3 | 1 |
| Cushcraft | 729B | 17.6 | 1 |
| Cushcraft | A-3-S | 8.0 | 3 |
| Cushcraft | A-3-WS | 7.0-8.1 | 2 |
| Cushcraft | A-4-S | 7.4-7.5 | 3 |
| Cushcraft | A14410SN | 13.2 | 1 |
| Cushcraft | A27-10S | 12.2 | 2 |
| Cushcraft | A270-6S | 9.9 | 2 |
| Cushcraft | A43011N | 17.8 | 1 |
| Cushcraft | A50-3S | 10.2 | 1 |
| Cushcraft | A50-5S | 12.6 | 1 |
| Cushcraft | A503S | 8.0 | 1 |
| Cushcraft | A505S | 10.5 | 1 |
| Cushcraft | A506S | 11.6 | 1 |
| DJ2UT | P5C | 9.5-11.2 | 3 |
| Diamon | V-2000 | 2.1-8.4 | 3 |
| Diamon | V2000 | 2.1-8.4 | 3 |
| Diamond | G-200 | 11.0 | 1 |
| Eigenbau | Bi-Square | 5.2 | 1 |
| Flexa | 2xFX213hor | 14.8 | 1 |
| Flexa | 2xFX217hor | 16.0 | 1 |
| Flexa | 2xFx224hor | 16.6 | 1 |
| Flexa | 2xFx2304horiz | 16.9 | 1 |
| Flexa | 2xFx2317hor | 24.2 | 1 |
| Flexa | 4xFX213hor | 18.0 | 1 |
| Flexa | 4xFx2317hor | 27.2 | 1 |
| Flexa | FX1308-V | 15.3 | 1 |
| Flexa | FX210 | 11.2 | 1 |
| Flexa | FX213 | 12.3 | 1 |
| Flexa | Fx205v | 9.7 | 1 |
| Flexa | Fx217 | 12.9 | 1 |
| Flexa | Fx224 | 13.5 | 1 |
| Flexa | Fx2304horiz | 14.6 | 1 |
| Flexa | Fx2304v | 16.4 | 1 |
| Flexa | Fx2309 | 18.1 | 1 |
| Flexa | Fx2317 | 20.1 | 1 |
| Flexa | Fx2317hor | 20.1 | 1 |
| Flexa | Fx2317vert | 20.1 | 1 |
| Flexa | Fx7015v | 12.3 | 1 |
| Flexa | Fx7033 | 15.3 | 1 |
| Flexa | Fx7033hor | 15.3 | 1 |
| Flexa | Fx7033vert | 15.3 | 1 |
| Flexa | Fx7044 | 16.6 | 1 |
| Flexa | Fx7044-4 | 21.7 | 1 |
| Flexa | Fx7056 | 17.4 | 1 |
| Flexa | Fx7073 | 17.8 | 1 |
| Flexa | Fx7073hor | 17.8 | 1 |
| Flexa | Fx7073vert | 17.8 | 1 |
| Fritzel | FB-DX460 | 2.1-9.2 | 6 |
| Fritzel | FB-DX660 | 8.2-9.0 | 3 |
| Fritzel | FB-Do450 | 2.1-8.9 | 5 |
| Fritzel | FB-Do505 | 6.3-8.9 | 5 |
| Fritzel | FB-Dx506 | 5.6-8.9 | 6 |
| Fritzel | FB13 | 2.1 | 3 |
| Fritzel | FB23 | 5.9-6.5 | 3 |
| Fritzel | FB33 | 7.4-9.1 | 3 |
| Fritzel | FB34 | 2.1-9.2 | 4 |
| Fritzel | FB53 | 7.6-8.8 | 3 |
| Fritzel | MFB23 | 4.5-6.2 | 3 |
| Fritzel | UFB12 | 2.1 | 2 |
| Fritzel | UFB13 | 2.1 | 3 |
| Fritzel | UFB33 | 6.9-7.5 | 3 |
| Hy-Gain | 103BAS | 7.2 | 1 |
| Hy-Gain | 105BAS | 10.8 | 1 |
| Hy-Gain | 155BAS | 9.8 | 1 |
| Hy-Gain | 204BAS | 8.2 | 1 |
| Hy-Gain | 205BAS | 9.8 | 1 |
| Hy-Gain | AV-640 | 1.0-1.8 | 7 |
| Hy-Gain | CA-2x4max | 8.5-11.9 | 2 |
| Hy-Gain | TH11DX | 7.5-9.2 | 5 |
| Hy-Gain | TH2Mk3 | 6.1-6.2 | 3 |
| Hy-Gain | TH3JRS | 7.0-8.9 | 3 |
| Hy-Gain | TH3Mk3 | 7.1-8.4 | 3 |
| Hy-Gain | TH5Mk2 | 7.8-9.0 | 3 |
| Hy-Gain | TH6DXX | 8.2-9.8 | 3 |
| Hy-Gain | TH7DX | 8.2-9.4 | 3 |
| KLM | KT-34A | 7.5-8.9 | 3 |
| KLM | KT-34XA | 9.0-10.9 | 3 |
| Maspr | WHS-32N | 12.8-14.8 | 2 |
| Maspro | WH-59hor | 9.2 | 2 |
| Maspro | WH-59vert | 9.2 | 2 |
| Mosle | TA-33M | 8.7-10.4 | 3 |
| Mosle | TA-33M warc | 2.1-10.4 | 5 |
| Mosle | TA-33jrn warc | 2.1-8.1 | 5 |
| Mosle | TA-34xl | 11.2-11.7 | 3 |
| Mosle | TA-34xl warc | 2.1-11.7 | 5 |
| Mosle | TW-23M | 8.9-9.3 | 2 |
| Mosle | TW-24xl | 10.6-11.2 | 2 |
| Mosle | TW-31xl | 2.1 | 3 |
| Mosle | TW-32xl | 5.7-7.3 | 3 |
| Mosle | TW-33xl | 8.2-9.3 | 3 |
| Mosley | CL-33-M | 8.1-9.4 | 3 |
| Mosley | CL-33warc | 2.1-10.7 | 5 |
| Mosley | CL-36-M | 7.1-9.4 | 3 |
| Mosley | MP-33N | 8.0-10.2 | 3 |
| Mosley | MP-33N warc | 2.1-10.2 | 5 |
| Mosley | Mini-33 | 5.3-6.7 | 3 |
| Mosley | Pro-57B | 7.6-8.2 | 5 |
| Mosley | Pro-57B-40 | 2.1-11.6 | 6 |
| Mosley | Pro-67A | 7.3-8.6 | 5 |
| Mosley | Pro-67B | 7.6-8.2 | 5 |
| Mosley | Pro-67c-3 | 2.1-11.4 | 7 |
| Mosley | Pro-77A | 2.1-11.6 | 7 |
| Mosley | Pro-95 | 11.1-12.7 | 5 |
| Mosley | Pro-96 | 9.9-12.7 | 6 |
| Mosley | TA-31M | 2.1 | 3 |
| Mosley | TA-31jrn | 2.1 | 3 |
| Mosley | TA-32M | 6.0-7.7 | 3 |
| Mosley | TA-32jrn | 5.2-7.7 | 3 |
| Mosley | TA-33jrn | 6.7-8.1 | 3 |
| Mosley | TA-53M | 5.9-8.1 | 5 |
| Muehla | DX2000 | -4.7--1.1 | 8 |
| Opti-Beam | OB10-3W | 7.4-8.7 | 3 |
| Opti-Beam | OB11-3 | 7.2-8.8 | 3 |
| Opti-Beam | OB11-5 | 7.3-7.6 | 5 |
| Opti-Beam | OB12-4 | 5.8-8.9 | 4 |
| Opti-Beam | OB12-4WARC | 5.8-8.9 | 4 |
| Opti-Beam | OB13-6 | 5.8-8.9 | 6 |
| Opti-Beam | OB15-7 | 5.6-8.0 | 7 |
| Opti-Beam | OB16-3 | 9.2-10.7 | 3 |
| Opti-Beam | OB16-5 | 8.0-9.8 | 5 |
| Opti-Beam | OB17-4 | 7.0-10.2 | 4 |
| Opti-Beam | OB18-6 | 6.9-9.7 | 6 |
| Opti-Beam | OB2-30 | 6.0 | 1 |
| Opti-Beam | OB2-40 | 5.8 | 1 |
| Opti-Beam | OB2-40M | 5.7 | 1 |
| Opti-Beam | OB2-80 | 6.0 | 1 |
| Opti-Beam | OB3-30 | 7.9 | 1 |
| Opti-Beam | OB3-80 | 7.0 | 1 |
| Opti-Beam | OB4-2W | 6.4-6.8 | 2 |
| Opti-Beam | OB4-2WARC | 6.4-6.8 | 2 |
| Opti-Beam | OB4-40 | 7.2 | 1 |
| Opti-Beam | OB4020 | 7.4-10.4 | 2 |
| Opti-Beam | OB4030 | 5.8-6.0 | 2 |
| Opti-Beam | OB5-10 | 10.5 | 1 |
| Opti-Beam | OB5-12 | 10.8 | 1 |
| Opti-Beam | OB5-15 | 10.5 | 1 |
| Opti-Beam | OB5-17 | 10.7 | 1 |
| Opti-Beam | OB5-20 | 10.6 | 1 |
| Opti-Beam | OB5-6 | 10.9 | 1 |
| Opti-Beam | OB6-10 | 11.7 | 1 |
| Opti-Beam | OB6-2WARC | 7.3-8.6 | 2 |
| Opti-Beam | OB6-3M | 6.2-6.8 | 3 |
| Opti-Beam | OB6-6 | 11.7 | 1 |
| Opti-Beam | OB7-2W | 7.2-8.0 | 2 |
| Opti-Beam | OB7-2WARC | 7.2-8.0 | 2 |
| Opti-Beam | OB7-3 | 6.2-7.6 | 3 |
| Opti-Beam | OB7-3M | 6.2-7.6 | 3 |
| Opti-Beam | OB804020 | 6.0-10.5 | 3 |
| Opti-Beam | OB9-2W | 9.2-10.0 | 2 |
| Opti-Beam | OB9-2WARC | 9.2-10.0 | 2 |
| Opti-Beam | OB9-5 | 6.3-6.8 | 5 |
| Opti-Beam | OBW10-5 | 6.7-7.0 | 5 |
| Optibeam | OB12-6 | 5.8-7.0 | 6 |
| Optibeam | OB8-4M | 5.7-7.0 | 4 |
| Optibeam | OB8_4M | 5.7-7.0 | 4 |
| SHF | SHF1340 | 18.8 | 1 |
| SHF | SHF1367 | 22.1 | 1 |
| SHF | SHF2328 | 17.6 | 1 |
| SHF | SHF2344 | 20.2 | 1 |
| SHF | SHF2367 | 22.1 | 1 |
| Somme | XP406 | 2.1-8.2 | 6 |
| Somme | XP507 | 2.1-9.2 | 7 |
| Somme | XP707 | 2.1-11.7 | 7 |
| Somme | XP807 | 2.1-13.2 | 7 |
| SteppIR | SteppIR_2El | 6.4-6.8 | 6 |
| SteppIR | SteppIR_3El | 3.9-7.9 | 6 |
| SteppIR | SteppIR_4El | 9.4-12.0 | 6 |
| SteppIR | Stepp_3El | 3.0-6.3 | 6 |
| Tonna | 20089 | 12.4 | 1 |
| Tonna | 20505 | 10.0 | 1 |
| Tonna | 20623 | 17.2 | 1 |
| Tonna | 20624 | 17.2 | 1 |
| Tonna | 20635 | 19.6 | 1 |
| Tonna | 20636 | 19.6 | 1 |
| Tonna | 20650 | 20.1 | 1 |
| Tonna | 20655 | 20.1 | 1 |
| Tonna | 20725 | 17.6 | 1 |
| Tonna | 20804 | 8.9 | 1 |
| Tonna | 20808 | 8.1 | 1 |
| Tonna | 20809 | 12.4 | 1 |
| Tonna | 20813 | 15.3 | 1 |
| Tonna | 20818 | 12.2 | 1 |
| Tonna | 20919 | 15.3 | 1 |
| Tonna | 20922 | 17.4 | 1 |
| Tonna | F9FT-horiz | 8.9 | 1 |
| Tonna | F9FT-vert | 8.9 | 1 |
| Tonna | T-17El-horiz | 15.0 | 1 |
| Tonna | T-17El-vert | 15.0 | 1 |
| Tonna | T-4x17El-horiz | 21.0 | 1 |
| Tonna | T-4x17El-vert | 21.0 | 1 |
| Wimo | D2T | -9.6-4.8 | 7 |
| Wimo | Wx208 | 9.2 | 1 |
| Wimo | Wx214 | 12.2 | 1 |
| Wimo | Wx220 | 14.9 | 1 |
| Wimo | Wx7020 | 13.7 | 1 |
| Wimo | Wx7036 | 16.1 | 1 |
| Wimo | Wy204 | 9.2 | 1 |
| Wimo | Wy207 | 12.2 | 1 |
| Wimo | Wy210 | 14.4 | 1 |
| Wimo | Wy7010 | 13.7 | 1 |
| Wimo | Wy7018 | 16.1 | 1 |
| Wimo | Wy7023 | 17.1 | 1 |
| Wimo | Wy706 | 10.2 | 1 |
| Wimo | ZX6-2 | 6.2 | 1 |

### A.4 Vertical Antennas

Vertical antennas have patterns generated using the omnidirectional formula from Section 11.4.2.

| Manufacturer | Model | Gain (dBi) | Bands |
|--------------|-------|------------|-------|
| Allgemein | GP Triple | 2.2 | 4 |
| Allgemein | Ringo Range | 8.2 | 2 |
| Comet | CX-725 | 2.2-8.4 | 3 |
| Comet | CX-901 | 3.0-8.4 | 3 |
| Comet | CX-902 | 4.4-6.9 | 3 |
| Comet | GP-15 | 3.0-8.6 | 3 |
| Comet | GP-21 | 14.8 | 1 |
| Comet | GP-3N | 4.5-7.2 | 2 |
| Comet | GP-91 | 5.3-10.6 | 3 |
| Comet | GP-95 | 6.0-12.8 | 3 |
| Comet | GP-9N | 8.5-11.9 | 2 |
| Cushcraft | R6000 | 1.5 | 1 |
| Cushcraft | R6xyz | 1.5 | 1 |
| Diamond | DP-CP6 | 1.0-3.0 | 5 |
| Diamond | F-23 | 7.8 | 1 |
| Fritzel | GPA30 | 1.6 | 3 |
| Fritzel | GPA404 | 1.6 | 4 |
| Fritzel | GPA50 | 1.6 | 5 |

*Summary: 14 Log-Periodic, 7 Quad, 229 Yagi, 18 Vertical antennas with generated patterns (total: 268 antennas).*
