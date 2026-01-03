# Swiss NIS Calculator - Functional Specification

## 1. Purpose and Scope

Calculate RF field strength for Swiss amateur radio antenna approval (NISV compliance).

### 1.1 Terminology

The application uses official Swiss NISV/ORNI terminology for evaluation points:

| Language | Full Term | Abbreviation |
|----------|-----------|--------------|
| German | Ort für kurzfristigen Aufenthalt | **OKA** |
| French | Lieu de séjour momentané | **LSM** |
| Italian | Luogo di soggiorno temporaneo | **LST** |
| English | Place of Short-Term Stay | **PSS** |

In this document, "OKA" is used as the default term. The UI displays the appropriate abbreviation based on the selected language.

## 2. User Workflow (End-to-End)

```
Welcome Screen
    -> Project List -> Project Overview
    -> [New Project] -> Project Info -> Project Overview

Navigation Pane
    -> [Projects] -> Project List
    -> [New Project] -> Project Info -> Project Overview
    -> [Master Data] -> Master Data Manager
       -> [Add/Edit Antenna] -> Antenna Editor -> Master Data Manager
       -> [Add/Edit Cable] -> Cable Editor -> Master Data Manager
       -> [Add/Edit Radio] -> Radio Editor -> Master Data Manager
       -> [Add/Edit Evaluation Point] -> PSS Editor -> Master Data Manager
       -> [Translations] -> Translation Editor
    -> [Calculate All] -> Results
    -> [Export Report] -> Results with export options
    -> [Export PDF] -> PDF generation
    -> [Settings] -> Settings
    -> [Factory] -> Factory Mode

Project Overview
    -> [Edit Station Info] -> Project Info -> Project Overview
    -> [Add/Edit Configuration] -> Configuration Editor
       -> [Edit/Add Antenna] -> Antenna Editor -> Configuration Editor
       -> [Edit/Add Cable] -> Cable Editor -> Configuration Editor
       -> [Edit/Add Radio] -> Radio Editor -> Configuration Editor
       -> [Edit/Add Evaluation Point] -> PSS Editor -> Configuration Editor
    -> [Calculate All] -> Results -> Project Overview
```


## 3. Key Screens and Tasks

### 3.1 Welcome Screen

First screen shown when app launches:

- **All Projects List**: shows all available projects with search and sort (by name, last modified)
  - Columns: Project Name, Last Modified, Config Count
  - Default sort: Last Modified (descending)
  - Search filters by project name (case-insensitive)
  - Selecting a project opens the Project Overview
  - Each project row includes **Edit** and **Delete** actions on the right side (same layout as configuration cards)
  - Delete requires confirmation; projects with configurations can be deleted
  - A **+ New Project** button is placed in the same right-side action area (corporate identity consistency)

### 3.2 Navigation Pane

Global navigation available from most screens:

- **Projects** -> Project list (same as Welcome screen)
- **New Project** -> Create a new project
- **Master Data** -> Master Data Manager (Section 3.6)
- **Calculate All** -> Runs calculation for current project (Section 3.7)
- **Export Report** -> Results view with export options (Section 3.7)
- **Export PDF** -> Generates PDF report from Results
- **Import Project** -> Opens file picker to import a .nisproj file (always enabled)
- **Export Project** -> Opens save dialog to export current project as .nisproj (enabled when project loaded)
- **Settings** -> Language and Theme (Section 10)
- **Factory** -> Factory mode access (Section 9)

### 3.3 Project Overview (Main Screen)

Primary workspace after project is loaded/created. Users can edit and add configurations here.

**Header Bar**:
- Project name (read-only display)
- Dirty indicator (*) shown when unsaved changes exist

**Station Info Panel** (non-collapsible):
- Callsign: HB9FS/HB9BL
- Address: Musterstrasse 1, 8000 Zürich
- Click "Edit" → Navigates to Project Info screen to modify

**Configurations List** (DataGrid):

Window default width: 1500px → Available DataGrid width: 1200px

| Column | Width | Content |
|--------|-------|---------|
| Antenna | 200px | Antenna name (primary identifier) |
| Bands | 280px | Frequency bands (text wrapping enabled) |
| Height | 70px | Antenna height + group separator |
| Cable | 120px | Cable type name |
| Power | 100px | Transmitter power + group separator |
| OKA* | 120px | Evaluation point name |
| Dist. | 70px | Distance to evaluation point |
| Actions | 210px | Edit / Delete buttons |

*OKA column header is localized: OKA (de), LSM (fr), LST (it), PSS (en)

Layout concept:
- No horizontal cell padding (first column aligns with view content)
- Vertical padding: 4px
- Group separators: 2px right border after Height and Power columns

- Configurations are identified by their antenna (no user-defined name)
- "+ Add Configuration" button → Navigates to Configuration Editor (Section 3.5)
- Edit button → Navigates to Configuration Editor with existing data
- Each configuration references an evaluation point (OKA/LSM/LST/PSS) from master data (distance and damping are stored in OKA master data)

**Action Buttons**:
- "Back" → Returns to project list
- "Calculate All" → Runs calculation for all configs → Navigates to Results (Section 3.7)

### 3.4 Project Info

Screen for creating or editing station information for a project.

**Fields**:
- Project Name
- Operator Name
- Callsign
- Address
- Location (free text)

**Actions**:
- Save -> Returns to Project Overview
- Cancel -> Discards changes, returns to previous view

### 3.5 Configuration Editor

**Component Selection (shared dropdown behavior):**
All component dropdowns (Antenna, Radio, Cable) share the same behavior:
- **Sorted alphabetically** by name/manufacturer
- **Text search enabled**: Type characters to jump to matching item (e.g., type "Y" to jump to "Yaesu")
- [Edit] button opens the respective Master Editor
- [+ Add] button opens the Master Editor for a new item

Screen for creating or editing one antenna configuration. Header shows "Configuration {number}" (e.g., "Configuration 1").

**Section 1: Antenna** (first, most important)
- Antenna: [dropdown from master data] [Edit] [+ Add]
  - Edit → Navigates to Antenna Editor with selected antenna
  - Add → Navigates to Antenna Editor for new antenna
- Height: [number] m
  - Hint: "Antenna height above OKA" (translated)
- Polarization: [radio buttons] Horizontal | Vertical (mutually exclusive)
  - Fixed by antenna master data (not overrideable per configuration)
  - If Horizontal: Rotation Angle: [number] degrees (default 360) - *information only, not used in calculation*

**Section 2: Transmitter**
- Radio: [dropdown from master data] [Edit] [+ Add]
  - Edit → Navigates to Radio Editor with selected radio
  - Add → Navigates to Radio Editor for new radio
  - Includes common HAM transceivers (Icom, Yaesu, Kenwood, Elecraft, FlexRadio) for HF/VHF/UHF
- Linear (optional): [text field] + [power in W]
  - Free text entry for linear name
  - Power field becomes enabled when name is entered
  - When set, linear power is used in calculations instead of radio power
- Output Power: [number] W (at radio output, used when no linear is configured)

**Section 3: Feed Line**
- Cable Type: [dropdown from master data] [Edit] [+ Add]
  - Edit → Navigates to Cable Editor with selected cable
  - Add → Navigates to Cable Editor for new cable
- Cable Length: [number] m
- Additional Losses: [number] dB
- Loss Description: [text] (e.g., "Connectors, switch")

**Section 4: Operating Parameters**
- Modulation: [dropdown] SSB (0.2) | CW (0.4) | FM/Digital (1.0)
- Activity Factor: [number] (default 0.5)

These apply to all bands of the selected antenna. Bands (frequency, gain, pattern) are defined by the antenna master data.

**Section 5: Evaluation Point (OKA)**
- Evaluation Point: [dropdown from master data] [Edit] [+ Add]
  - Edit → Navigates to PSS Editor with selected evaluation point
  - Add → Navigates to PSS Editor for new evaluation point
- Distance: [read-only display] m (from OKA master data)
- Building Damping: [read-only display] dB (from OKA master data)

Note: Each configuration references exactly one evaluation point (OKA/LSM/LST/PSS) from master data via foreign key. Distance and damping values are stored in OKA master data (single source of truth). To use different values, create a new OKA entry. See Section 1.1 for terminology. The real 3D distance used in calculations is computed as √(horizontal² + height²).

**Actions**:
- Save → Returns to Project Overview
- Cancel → Discards changes, returns to Project Overview

### 3.6 Master Data Manager

Central hub for managing master data, evaluation points (OKA/LSM/LST/PSS), and language strings. Accessed via **Navigation Pane -> Master Data**.

**Navigation Structure**:
```
Master Data Manager
|-- Antennas
|   |-- List of all antennas (searchable, filterable)
|   |-- [Add Antenna] -> Antenna Master Editor
|   |-- [Edit] -> Antenna Master Editor (with existing data)
|-- Cables
|   |-- List of all cables (searchable)
|   |-- [Add Cable] -> Cable Master Editor
|   |-- [Edit] -> Cable Master Editor (with existing data)
|-- Radios
|   |-- List of all radios (searchable)
|   |-- [Add Radio] -> Radio Master Editor
|   |-- [Edit] -> Radio Master Editor (with existing data)
|-- Evaluation Points (OKA/LSM/LST/PSS)
|   |-- List of evaluation points shared across projects (factory table is empty; all entries are user data)
|   |-- [Add Evaluation Point] -> PSS Editor
|   |-- [Edit] -> PSS Editor (with existing data)
|-- Modulations (factory mode editable)
|   |-- SSB=0.2, CW=0.4, FM=1.0
|   |-- [Add], [Edit], and [Save] enabled only in factory mode
|-- Constants (factory mode editable)
|   |-- Ground Reflection Factor kr=1.6
|   |-- Default Activity Factor=0.5
|   |-- [Edit] and [Save] enabled only in factory mode
|   |-- Constants list is fixed; no add/remove actions
|   |-- Persisted in `masterdata.json`
|-- Bands (factory mode editable)
|   |-- List of standard bands (name + center frequency)
|   |-- [Add], [Edit], and [Save] enabled only in factory mode
|   |-- Used for antenna band definitions and cable attenuation points
|   |-- Persisted in `masterdata.json`
|-- Translations
|   |-- Data grid with columns (German first, English last): Category, German (master), French, Italian, English
|   |-- Translation ID/key is hidden (not displayed) and cannot be changed
|   |-- Search and category filter available
|   |-- [Save Changes] persists to `translations.json` in the Data folder
|-- Database (admin mode only)
    |-- Export/Import master data (factory mode)
```

**Factory Mode**: See **Section 9** for factory mode activation, features, and workflow.

**Actions**:
- Back -> Returns to Project list or Project Overview (depending on context)

For CRUD permissions and alternative access points, see **Section 6.1**.

### 3.7 Calculation & Results

Results displayed after "Calculate All":

**Per Configuration**:
- Configuration title: Antenna name with Pass/Fail badge
- Configuration summary line (translated):
  ```
  Antenna: [name], [height]m above OKA, Cable: [description],
  [OKA full name]: [OKA name], [distance]m horizontal distance to antenna mast,
  Distance Antenna-OKA: [calculated]m
  ```
  Example (German):
  ```
  Antenne: Fritzel FB-33, 10m über OKA, Kabel: 15m RG-213,
  Ort für kurzfristigen Aufenthalt: Balkon Nachbar, 4m horizontale Distanz zum Antennenmast,
  Distanz Antenne-OKA: 10.8m
  ```
- Detailed table (see Section 5 Output and Reports)

**Safety Distance Visualization** (optional):
- Diagram showing antenna position and calculated safety distances

**Export Options**:
- Markdown Export: Generates formatted tables per FSD Section 5

**Actions**:
- Close → Returns to Project Overview
- Export Markdown

## 4. Core Calculations

### 4.1 Input Parameters

| Parameter | Symbol | Unit | Source |
|-----------|--------|------|--------|
| Frequency | f | MHz | User selection |
| Horizontal distance | d_h | m | Horizontal ground distance from OKA to antenna mast |
| Antenna height | h | m | Antenna height above OKA |
| Distance antenna-OKA | d | m | Real 3D distance = √(d_h² + h²) |
| TX Power | P | W | User input |
| Activity factor | AF | - | Default 0.5 |
| Modulation factor | MF | - | SSB=0.2, CW=0.4, FM=1.0 |
| Cable attenuation | a1 | dB | Calculated from project cable data |
| Additional losses | a2 | dB | User input (connectors, switches) |
| Antenna gain | g1 | dBi | From project antenna data |
| Vertical angle attenuation | g2 | dB | From antenna pattern (interpolated) |
| Building damping | ag | dB | User input |
| Ground reflection factor | kr | - | Fixed 1.6 |

**Distance Calculation**: The field strength formula uses the real 3D distance from antenna to OKA, calculated as `d = √(horizontal² + height²)`. The vertical angle (used for pattern lookup) is calculated from the horizontal distance and height.

**Data Source Policy**: Configurations reference master data via foreign keys in the database. For data protection and export/import, see **Section 6 (Data Operations)**.

### 4.1.1 Cable Attenuation Interpolation

Cable attenuation (dB/100m) is interpolated by frequency using linear interpolation between the nearest known points.
- If the exact frequency exists, use it.
- If the frequency is between two points, linearly interpolate.
- If outside the defined range, use the nearest endpoint (no extrapolation).

`a1 = AttenuationPer100m(f) * (CableLengthMeters / 100)`

### 4.1.2 Band Selection

Calculations are performed only for antenna bands defined in the antenna master data.
- No interpolation between antenna bands.
- Band/frequency mismatch should not occur; if a desired frequency is missing, the band must be added in antenna master data and calculation is blocked until fixed.

### 4.2 Vertical Angle and Pattern

The vertical angle attenuation (g2) is automatically calculated based on the geometry of the antenna installation:

**Angle Calculation:**
```
Vertical Angle = atan(Antenna Height / Horizontal Distance)
```

Where:
- 0° = Looking horizontally (OKA very far away)
- 90° = Looking straight down (OKA directly below antenna)

**Pattern Lookup:**
The vertical angle is used to extrapolate the vertical attenuation from the antenna's radiation pattern. The pattern array contains 10 values representing attenuation in dB at angles 0°, 10°, 20°, 30°, 40°, 50°, 60°, 70°, 80°, 90°:

| Index | Angle | Description |
|-------|-------|-------------|
| 0 | 0° | Horizon (maximum radiation) |
| 1 | 10° | Slight downward angle |
| ... | ... | ... |
| 9 | 90° | Straight down toward OKA |

Linear interpolation is used for angles between data points.

**Real Distance Calculation:**
The field strength formula uses the real 3D distance:
```
Real Distance (d) = √(Horizontal Distance² + Antenna Height²)
```

**Example:**
- Antenna height: 10m
- Horizontal distance: 4m
- Vertical angle: atan(10/4) = 68.2°
- Pattern lookup: interpolate between index 6 (60°) and index 7 (70°)
- Real distance: √(4² + 10²) = √116 = 10.8m (used in field strength formula)

### 4.3 Ground Reflection Factor (kr)

The ground reflection factor (Bodenreflexionsfaktor) accounts for constructive interference between the direct wave from the antenna and its reflection from the ground at evaluation points near ground level.

**Value:** kr = 1.6 (fixed, per NISV Anhang 2)

**Physical Effect:**
- Field strength (E) is multiplied by 1.6
- Power density (S = E²/377) is multiplied by 1.6² = **2.56**
- Equivalent to adding **4.08 dB** to the effective gain

This factor ensures worst-case field strength estimation at locations where a person might stand, where direct and reflected waves can combine constructively.

### 4.4 Formulas

```
Mean power:           Pmean = P × AF × MF
Total attenuation:    a = a1 + a2
Attenuation factor:   A = 10^(-a/10)
Total antenna gain:   g = g1 - g2
Gain factor:          G = 10^(g/10)
EIRP toward OKA:      Ps = Pmean × A × G
ERP toward OKA:       P's = Ps / 1.64
Building factor:      AG = 10^(-ag/10)
Field strength:       E' = 1.6 × sqrt(30 × Pmean × A × G × AG) / d
Safety distance:      ds = 1.6 × sqrt(30 × Pmean × A × G × AG) / EIGW

where d = √(d_h² + h²) is the real 3D distance from antenna to OKA
```

Note: EIRP (Equivalent Isotropic Radiated Power) and ERP (Effective Radiated Power) are calculated in the direction of the OKA, accounting for the vertical angle attenuation from the antenna's radiation pattern. These are not the maximum values of the antenna, but the effective power radiated toward the evaluation point.

### 4.5 NIS Limits (Swiss NISV)

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

### 4.6 Calculation Precision and Rounding

- Internal calculations use double precision.
- Output values are rounded for display: distances to 1 decimal, power/gain/attenuation to 2 decimals, factors to 2 decimals, field strength and safety distance to 2 decimals.

### 4.7 Edge Cases

- If both horizontal distance and antenna height are 0, show a validation error and block save.
- If only one of them is 0, allow and compute the real distance as √(d_h² + h²).
- If power <= 0, all derived power and field strength values are 0.
- If activity or modulation factor is 0, mean power is 0.

## 5. Output and Reports

### 5.1 Markdown Report Format

One table per antenna configuration. Each report contains a configuration summary and a per-band calculation table.

**Configuration Summary (example fields):**
- Sender power
- Linear (or None)
- Antenna model
- Polarization and rotation angle
- OKA name and distance

**Per-band Calculation Table:**

The table follows the legacy VB6 application structure. Bold rows indicate key input/output values. The table header contains only "Parameter", "Sym", and "Unit" columns—frequency values are not duplicated in the header since they appear in the first data row (f).

| Row | Symbol | Label (German) | Unit | Bold |
|-----|--------|----------------|------|------|
| 1 | f | Frequenz | [MHz] | **Yes** |
| 2 | - | Nr. des OKA auf dem Situationsplan | - | **Yes** |
| 3 | d | Abstand OKA zur Antenne | [m] | **Yes** |
| 4 | P | Leistung am Senderausgang | [W] | **Yes** |
| 5 | AF | Aktivitätsfaktor | [ ] | No |
| 6 | MF | Modulationsfaktor | [ ] | No |
| 7 | Pm | Mittl. Leistung am Senderausgang | [W] | **Yes** |
| 8 | a1 | Kabeldämpfung | [dB] | No |
| 9 | a2 | übrige Dämpfung | [dB] | No |
| 10 | a | Summe der Dämpfung | [dB] | No |
| 11 | A | Dämpfungsfaktor | [ ] | No |
| 12 | g1 | Antennengewinn | [dBi] | No |
| 13 | g2 | Vertikale Winkeldämpfung | [dB] | No |
| 14 | g | Totaler Antennengewinn | [dB] | No |
| 15 | G | Antennengewinnfaktor | [ ] | No |
| 16 | Ps | Massgebende Sendeleistung (EIRP) | [W] | No |
| 17 | P's | Massgebende Sendeleistung (ERP) | [W] | **Yes** |
| 18 | ag | Gebäudedämpfung | [dB] | No |
| 19 | AG | Gebäudedämpfungsfaktor | [ ] | No |
| 20 | kr | Bodenreflexionsfaktor | [ ] | No |
| 21 | E' | Massgebende Feldstärke am OKA | [V/m] | **Yes** |
| 22 | E IGW | Immissions-Grenzwert | [V/m] | No |
| 23 | ds | Sicherheitsabstand | [m] | **Yes** |

**Note**: Rows are displayed as columns in the exported table (one column per frequency band).

### 5.2 Column Explanations

A short explanation section follows the table and describes each column label in plain language. The explanations are derived from the legacy VB6 application:

| Symbol | Explanation (German) |
|--------|---------------------|
| f | Sendefrequenz der Amateurfunkstation |
| Nr. OKA | Im Situationsplan eingezeichneter Ort für den kurzfristigen Aufenthalt |
| d | Antenne - Ort für den kurzfristigen Aufenthalt |
| P | Ausgangsleistung des Senders oder Linears |
| AF | In der Regel AF = 0.5 |
| MF | bei SSB: MF=0.2, bei CW: MF=0.4, bei FM/RTTY/PSK31: MF=1.0 |
| Pm | Ausgangsleistung reduziert um Aktivitäts- und Modulationsfaktor |
| a1 | Kabeldämpfung bezogen auf Kabellänge |
| a2 | Stecker, SWR-Brücke, Antennenschalter |
| a | Kabeldämpfung + übrige Dämpfung |
| A | In absolute Zahl umgerechnete "Summe der Dämpfungen" |
| g1 | Maximaler Gewinn der Antenne gemäss Hersteller |
| g2 | Gewinnverminderung, wegen vertikalem Strahlungsdiagramm der Antenne |
| g | Antennengewinn - vertikale Winkeldämpfung |
| G | In absolute Zahl umgerechneter "Antennengewinn" |
| Ps | Äquivalente abgestrahlte Leistung bezogen auf einen isotropen Strahler |
| P's | Äquivalente abgestrahlte Leistung bezogen auf einen Dipol |
| ag | Dämpfung durch Gebäudemauern und Decken |
| AG | In absolute Zahlen umgerechnete "Gebäudedämpfung" |
| kr | Faktor welcher zu einer Zunahme der Feldstärke führt |
| E' | 6-Minuten-Mittelwert der Feldstärke am Ort für den kurzfristigen Aufenthalt |
| E IGW | Immissions-Grenzwert für die elektrische Feldstärke gemäss NISV |
| ds | Distanz von der Antenne, wo der Immissions-Grenzwert erreicht wird |

### 5.3 PDF Report

PDF export structure (one page per configuration):

**Configuration Pages (1 per config)**:
- Compact project header (operator, callsign, address, location in 2 rows)
- Configuration summary (antenna, radio, cable, OKA, modulation)
- Per-band calculation table (same structure as Section 5.1)
- Compliance status indicator
- Disclaimer

**Per-band Calculation Table** uses the same row structure as the Markdown export (Section 5.1), with bold formatting for key rows. The table header contains only "Parameter", "Sym", and "Unit"—band columns have empty headers since frequencies appear in the first data row:
- **f** (Frequency)
- **Nr. des OKA** (OKA number)
- **d** (Distance to OKA)
- **P** (TX Power)
- **Pm** (Mean Power)
- **P's** (ERP)
- **E'** (Field strength at OKA)
- **ds** (Safety distance)

**Last Page - Column Explanations**:
- Symbol + description table for all calculation parameters (see Section 5.2)
- Disclaimer

**File naming**: `{ProjectName}_YYYYMMDD.pdf`

---

## 6. Data Operations

### 6.1 Master Data Management

**All master data types share identical CRUD behavior** based on the `IsUserData` flag (see Section 7.4):

| Operation | UI Element | Note |
|-----------|------------|------|
| **Create** | [+ Add] button | Always creates user data |
| **Read** | List view, dropdowns | All data visible |
| **Update** | [Edit] button | User data only |
| **Delete** | [Delete] button | User data only |

**Access points:**
- **Navigation Pane → Master Data**: Central management hub with tabs for each type
- **Configuration Editor**: [Edit] and [+ Add] buttons next to each dropdown

**Common UI behavior:**
- All editors use full-width layout with validation errors in footer bar
- [Save] validates and returns; [Cancel] discards and returns
- Navigating away with unsaved changes triggers the warning described in Section 6.4.

**Editor-specific fields:**

| Editor | Fields |
|--------|--------|
| **Antenna** | Manufacturer, Model, Type, Polarization, Bands (Freq/Gain/Pattern) |
| **Cable** | Name, Attenuations at standard frequencies (dB/100m) |
| **Radio** | Manufacturer, Model, MaxPower |
| **Evaluation Point (PSS)** | Name, DefaultDistance, DefaultDamping |

**Antenna bands**: Each band has Frequency (MHz), Gain (dBi), and 10-value vertical pattern. [Auto-calculate] generates pattern from gain (Section 8.4).

**Constants**: Ground Reflection Factor kr=1.6, Default Activity Factor=0.5 (editable in factory mode only, stored in `masterdata.json`)

### 6.2 Export/Import

Two export/import types are supported: **User Data** (backup) and **Factory Data**.

**User Data Export/Import (backup):**
- **Export**: Writes a JSON file containing all projects, evaluation points, and user master data (Antennas/Cables/Radios with `IsUserData = true`).
- **Import**: **Destructive**. Deletes the entire database, then imports from the JSON file.

**Factory Data Export/Import:**
- **Export**: Same JSON structure and content as User Data export (see Appendix B.3).
- **Import**: **Destructive**. Deletes the entire database, then imports from the JSON file.
  - After import, **all master data records** (Antennas, Cables, Radios, Modulations, Evaluation Points) must have `IsUserData = false`.
  - User data is not preserved in factory import (full replacement).

**Confirmation Requirement (all imports):**
- Show a warning that import will delete all existing data and cannot be undone.
- Require explicit confirmation (OK/Cancel).

For JSON file formats, see **Appendix B.3**.

### 6.2.1 Project Import/Export (.nisproj)

**Export Project**:
- Accessed via **Navigation Pane → Export Project** (enabled when a project is loaded)
- Opens a save file dialog for the user to choose the destination
- Writes a .nisproj file containing the project header, configurations, and user-specific master data
- Each configuration includes references to antenna, cable, radio, and evaluation point by name/model (not DB IDs)
- Linear is optional; when absent, the `linear` field is null. When present, it contains `name` (string) and `powerWatts` (number)
- Always includes a `masterData` section (even if empty) to show the structure
- Only user-specific master data (IsUserData=true) is included; factory data is not exported

**Import Project**:
- Accessed via **Navigation Pane → Import Project** (always enabled)
- Opens a file picker dialog for the user to select a .nisproj file
- Reads the file and creates a new project in the database
- If the file contains a `masterData` section, those items are imported first as user data
- For OKAs: if an OKA with the same name exists, its distance and damping values are updated from the import file
- If referenced master data does not exist, placeholder entries are created with default values
- Displays an error dialog if import fails (e.g., invalid JSON, missing required fields)

### 6.3 Validation Rules (UI and Import)

Validation should be enforced both during manual editing and when importing JSON.

**Antenna (master data):**
- Manufacturer and Model are required (non-empty, trimmed).
- At least one band is required.
- Gain per band must be between -20 and 50 dBi.
- Pattern attenuation array must have 10 values, each between 0 and 60 dB.
- If rotatable, HorizontalAngleDegrees must be between 0 and 360.
- If vertically polarized, Rotatable must be false.

**Cable (master data):**
- Name is required (non-empty, trimmed).
- Attenuation values must be non-negative.
- At least one attenuation frequency should be provided.

**Radio (master data):**
- Manufacturer and Model are required (non-empty, trimmed).
- MaxPowerWatts must be > 0.

**Evaluation Point (master data):**
- Name is required (non-empty, trimmed).
- DefaultDistanceMeters must be > 0.
- DefaultDampingDb must be >= 0.

**Project / Configuration (user data):**
- Project Name is required (non-empty, trimmed).
- Configuration references must resolve to existing master data (antenna, cable, radio, modulation, evaluation point).
- PowerWatts, HeightMeters, CableLengthMeters must be > 0.
- AdditionalLossDb must be >= 0.

### 6.3.1 Validation and Error Messages

- Errors are shown inline in the editor footer and prevent Save.
- Import/calculation errors are shown as modal dialogs with a short reason.
- Messages should be concise and action-oriented (e.g., "Power must be greater than 0 W.").

### 6.4 Unsaved Changes and Navigation Warnings

Edits are **not** persisted immediately when fields change. Each editor view maintains a local, unsaved draft until the user explicitly saves.

**Save/Cancel behavior:**
- **Save**: Validate, then write changes to the database.
- **Cancel**: Discard the draft and restore the previously persisted values.

**Leaving an editor view:**
- If the user attempts to navigate away with unsaved changes (via any navigation button), show a warning dialog.
- Dialog text should indicate changes are not saved and ask whether to save before leaving.
- Buttons: [Save] [Discard] [Cancel]
  - **Save**: Save changes, then navigate.
  - **Discard**: Discard changes, then navigate.
  - **Cancel**: Stay on the current view.

**Closing the application:**
- Dialog title: "Unsaved Changes"
- Message: "Do you want to save your changes before closing?"
- Buttons: [Yes] [No] [Cancel]
  - **Yes**: Save project, then close application.
  - **No**: Discard changes, close application.
  - **Cancel**: Return to application, do not close.

This ensures users don't accidentally lose their work.

### 6.5 First-Run Initialization

On first launch:
- Create the application Data folder if it does not exist.
- Seed the writable database from the factory database if `nisdata.db` is missing.
- Create default `settings.json` if missing (language=de, theme=system).
- Load embedded translations, then merge `translations.json` if present.

### 6.6 Database Handling

This section defines the expected database rules for a robust implementation.

**General Rules:**
- SQLite is the single source of truth for application data (see Section 7.1 for locations).
- All master data and project records use integer Id primary keys.
- All references in configurations use Ids, not names (names are display-only).
- Schema changes do not use migrations; incompatible schemas trigger the reset flow in Section 6.7.

**Foreign Key Enforcement:**
- Foreign keys are enforced on connection open (`PRAGMA foreign_keys = ON`).
- `ProjectId` uses `ON DELETE CASCADE` (deleting a project deletes its configurations).
- Master data FKs (RadioId, CableId, AntennaId, ModulationId, OkaId) use `ON DELETE RESTRICT`:
  - Prevents deletion of master data that is referenced by any configuration.
  - UI checks usage before delete; DB provides a safety net.
  - FK columns remain nullable to allow saving incomplete configurations.

**Indexes:**
- Indexes exist for all FK columns in Configurations table (ProjectId, RadioId, CableId, AntennaId, ModulationId, OkaId).
- Unique index on (ProjectId, ConfigNumber) prevents duplicate config numbers per project.

**Constraints:**
- Required fields use NOT NULL.
- Numeric inputs use CHECK constraints:
  - Positive: PowerWatts, HeightMeters, ConfigNumber, MaxPowerWatts, DefaultDistanceMeters
  - Non-negative: LinearPowerWatts, CableLengthMeters, AdditionalLossDb, DefaultDampingDb
  - Range: ActivityFactor (0–1], HorizontalAngleDegrees [0–360], Modulation Factor (0–1]

**Transactions:**
- Multi-step writes (CreateProject, UpdateProject, ImportFactoryData, ImportUserData) run inside a transaction.
- On failure, transaction rolls back to maintain consistency.

**Import Validation:**
- After import, `ValidateConfigurationIntegrity()` checks that all FK references resolve.
- Missing references are reported to the user as warnings.

### 6.7 Database Schema Compatibility

When the application detects an incompatible database schema (e.g., after an update that changes the table structure), it displays a warning dialog before resetting the database:

- **Warning message**: Informs user that all projects and configurations will be deleted
- **Options**:
  - **Yes**: Reset database and continue (data will be lost)
  - **No**: Exit application (user can manually backup or migrate the database)

This ensures users are never surprised by data loss due to schema changes.

### 6.8 Manual Database Migration

For schema changes that can preserve existing data, a Python migration script is provided in `scripts/migrate_db.py`. This script safely updates the database schema while preserving all user data.

**When to use manual migration:**
- After updating to a new version with schema changes
- When the application shows a schema incompatibility warning
- Before the warning dialog forces a database reset

**Migration process:**

1. **Close the application** before running migration
2. **Run the migration script:**
   ```bash
   python scripts/migrate_db.py
   ```
3. The script automatically:
   - Creates a timestamped backup (e.g., `nisdata.db.backup_20250115_143022`)
   - Detects which migrations are needed
   - Applies migrations in order
   - Reports success or restores from backup on failure

**Example output:**
```
NIS Calculator Database Migration v0.5
=============================================
Database: src/NIS.Desktop/Data/nisdata.db

Creating backup: nisdata.db.backup_20250115_143022
Removing OkaDistanceMeters and OkaBuildingDampingDb columns...
(These values now come from OKA master data)
Columns removed successfully!

Migration complete!
```

**Migration script location:**
- Development: `scripts/migrate_db.py` in the repository
- The script targets `src/NIS.Desktop/Data/nisdata.db` by default

**Recovery from failed migration:**
- If migration fails, the script automatically restores from the backup
- Manual recovery: copy the `.backup_*` file back to `nisdata.db`

## 7. Data Model

### 7.1 Overview

All application data is stored in a single SQLite database file (`nisdata.db`).

**Data locations**:
- Windows: `%APPDATA%/SwissNISCalculator/Data/nisdata.db`
- macOS: `~/Library/Application Support/SwissNISCalculator/Data/nisdata.db`
- Linux: `~/.local/share/SwissNISCalculator/Data/nisdata.db`

`settings.json`, `translations.json`, and `masterdata.json` are stored in the same Data folder.

**JSON storage (non-SQLite):**
- `settings.json`: application settings (language, theme).
- `translations.json`: user translation overrides (JSON only, not SQLite).
- `masterdata.json`: bands and constants (factory-mode editable).
- `.nisproj`: project import/export files.
- User/Factory export files: JSON backups (see Appendix B.3).

```
Installation (nisdata.db)
│
├── User Settings (AppSettings)
│   └── Language, Theme, Window position
│
├── Master Data (shared across all projects)
│   │
│   │   Each record has IsUserData flag:
│   │   - false: factory data (read-only, replaceable on import)
│   │   - true: user data (editable, protected on import)
│   │
│   ├── Antennas
│   │   ├── Header: Manufacturer, Model, Type, Polarization
│   │   └── Bands[]
│   │       ├── Frequency (MHz)
│   │       ├── Gain (dBi)
│   │       └── Vertical Pattern[0°..90°] (dB attenuation)
│   │
│   ├── Cables
│   │   ├── Name
│   │   └── Attenuations{frequency → dB/100m}
│   │
│   ├── Radios
│   │   └── Manufacturer, Model, MaxPower
│   │
│   ├── Evaluation Points (OKA/LSM/LST/PSS; master data shared across projects, factory table empty)
│   │   └── Name, DefaultDistance, DefaultDamping
│   │
│   └── Operating Conditions
│       ├── Modulations: SSB (0.2), CW (0.4), FM (1.0)
│       └── Default ActivityFactor: 0.5
│
└── Projects[]
    │
    ├── Header: Name, OperatorName, Address, Location
    │
    └── Configurations[]
        │
        ├── Header: ConfigNumber, Name
        │
        ├── Antenna → (reference to Master Data)
        │   ├── Height (m)
        │   ├── IsRotatable (boolean)
        │   └── HorizontalAngleDegrees (0-360)
        │
        ├── Cable → (reference to Master Data)
        │   ├── Length (m)
        │   ├── AdditionalLoss (dB)
        │   └── AdditionalLossDescription
        │
        ├── Radio → (reference to Master Data)
        │   └── Power (W)
        │
        ├── Linear → (optional)
        │   ├── Name (text)
        │   └── Power (W)
        │
        ├── Evaluation Point → (reference to Master Data by ID)
        │   └── Distance and BuildingDamping from OKA master data
        │
        └── Operating Conditions → (reference to Master Data)
            ├── Modulation (SSB, CW, FM)
            └── ActivityFactor
```

### 7.2 Master Data

Shared reference data used across all projects. Each record has an `IsUserData` flag.

| Type | Factory Data | User Can Add | Key Fields |
|------|--------------|--------------|------------|
| Antennas | 319 antennas | Yes | Manufacturer, Model, Bands[] |
| Cables | ~20 cables | Yes | Name, Attenuations |
| Radios | ~50 radios | Yes | Manufacturer, Model, MaxPower |
| Evaluation Points | None (factory empty) | Yes | Name, DefaultDistance, DefaultDamping |
| Modulations | SSB, CW, FM | No | Name, Factor |
| Bands (JSON) | Standard band list | No | Name, FrequencyMHz |

Modulations are stored in the Modulations table as factory data (editable in factory mode only).
Constants (kr and default activity factor) are stored in `masterdata.json` and editable in factory mode only.

### 7.3 User Data

User-created data, always editable:

| Type | Description |
|------|-------------|
| Projects | Station information (Name, Operator, Address, Location) |
| Configurations | Antenna setups within a project, referencing master data |

### 7.4 Data Protection

| IsUserData | Owner | UI Behavior | On Factory Import (admin) |
|------------|-------|-------------|---------------------------|
| false | Factory | Read-only | Deleted and reimported |
| true | User | Editable | **Deleted** (factory import replaces entire database) |

## 8. Antenna Master Data Management

### 8.1 Overview

Each antenna in the master data includes an `antennaType` field that classifies the antenna type. This classification is used to determine which antennas can have vertical radiation patterns applied for NIS calculations.

### 8.2 The antennaType Field

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

### 8.3 Vertical Pattern Data Format

Each antenna band includes a `pattern` array with 10 values representing attenuation at elevation angles 0° to 90° in 10° steps:
- Index 0: 0° (horizon) - typically 0 dB (maximum radiation)
- Index 1: 10°
- ...
- Index 9: 90° (zenith)

Currently all patterns are set to `[0,0,0,0,0,0,0,0,0,0]`. When specific manufacturer pattern data becomes available, it can be entered here.

### 8.4 Pattern Generation Formulas (Reference)

#### 8.4.1 Directional Antennas (Yagi, Quad, Log-Periodic)

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

#### 8.4.2 Vertical Antennas (Omnidirectional)

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

### 8.4.3 Technical Background and Accuracy

The formulas in 8.4.1 and 8.4.2 represent common mathematical approximations used in RF planning and propagation modeling (often seen in tools like HATA or basic GIS plugins). They are generally "correct" in the sense that they are standard heuristics used to estimate antenna patterns when a specific manufacturer's .msi or .pln file is unavailable. However, there are some technical nuances to keep in mind regarding their accuracy and application.

#### 1. The HPBW Approximation

The formula `HPBW = 105° / √(G_linear)` is a variation of the Kraus approximation.

- **Standard Kraus**: Often expressed as `HPBW ≈ √(41253 / G_linear)` for a pencil beam (considering both Azimuth and Elevation).
- **Our Formula**: Using 105° is a specialized simplification for antennas where we assume a specific relationship between the vertical and horizontal planes. For a Yagi, this is a solid middle-ground estimate. For a Log-Periodic, it may slightly underestimate the beamwidth because LPDAs are wider-band and less efficient per unit of boom length than Yagis.

#### 2. Directional Pattern (Section 8.4.1)

The attenuation model is a dual-segment curve fit:

- **Inner Beam (θ ≤ θ_hp)**: Using `3 × (θ / θ_hp)²` is the standard Gaussian/Quadratic approximation. At the edge of the beam (θ = θ_hp), the formula yields `3 × (1)² = 3 dB`, which correctly defines the Half-Power point.
- **Outer Beam (θ > θ_hp)**: This linear interpolation toward A_zenith is a "safety" model. It prevents the math from suggesting infinite attenuation at 90° and ensures a realistic front-to-back/front-to-side ratio.

#### 3. Vertical/Omni Pattern (Section 8.4.2)

For omnidirectional antennas (like a collinear array), the logic holds up well:

- **Rolloff**: By setting Rolloff = 3.0, we force the curve to hit the 3 dB mark exactly at the HPBW limit.
- **Zenith Cap (A_zenith)**: The formula `20 + (G_dBi × 1.5)` accounts for the fact that higher-gain omnis achieve that gain by "squashing" the vertical lobe, which usually results in deeper nulls toward the zenith (directly above the antenna).

### 8.5 Antenna Classification Summary

| antennaType | Count | Examples |
|-------------|-------|----------|
| log-periodic | 14 | Titanex LP series, Titan DLP/LP, Cushcraft ASL201 |
| loop | 9 | DeltaLoop, Magnetic Loop |
| quad | 7 | Quad-2El, Quad-3El, X-Quad, Cubex |
| vertical | 34 | GP, Diamond X series, Cushcraft R series |
| wire | 26 | Dipol, G5RV, W3DZZ, Doublet, Langdraht |
| yagi | 229 | Cushcraft A-3-S, Opti-Beam, Hy-Gain TH series, SteppIR |

*Total: 319 antennas. Complete data stored in SQLite database (see Section 7).*

## 9. Factory Mode

Factory Mode provides access to administrative features for managing shipped master data and the demo project. This mode is intended for application maintainers and developers.

### 9.1 Activation

Factory Mode is accessed via the **Factory** item in the navigation pane (bottom section). When clicked, a password dialog appears. Enter the password **`HB9BLA`** to access Factory Mode.

### 9.2 Features Enabled in Factory Mode

When Factory Mode is active:

1. **FACTORY MODE indicator**: A red banner appears in the footer.

2. **Editable Factory Data**:
   - Modulations: Add, edit, and delete entries
   - Constants: Edit ground reflection factor (kr) and default activity factor
   - Bands: Add, edit, and delete frequency band definitions
   - Factory master data (antennas, cables, radios with `IsUserData=false`) can be modified

3. **Database Operations**:
   - Export Factory Data: Exports all master data to JSON file
   - Import Factory Data: Replaces entire database with imported data (warning: deletes all user data)

4. **Demo Project**: The shipped database includes a demo project that users can explore immediately

### 9.3 Demo Project

The demo project is a regular project in the shipped `nisdata.db` database. Factory can:
- Create or edit the demo project like any other project
- The demo project should showcase typical antenna configurations
- Changes are committed with the database to GitHub

### 9.4 Factory Mode Workflow

Typical workflow for updating shipped master data and distributing via GitHub:

#### Step 1: Enter Factory Mode
1. Navigate to **Factory** in the navigation pane (bottom section)
2. Enter the password **`HB9BLA`**
3. Verify the red **FACTORY MODE** indicator appears

#### Step 2: Modify Master Data
1. Go to the appropriate tab (Antennas, Cables, Radios, Database)
2. Add, edit, or delete entries as needed
3. For each entry, ensure `IsUserData=false` (factory data)
4. Modulations, Constants, and Bands can be edited in the Database tab

#### Step 3: Update Demo Project (Optional)
1. Create or modify a sample project named "Demo" that showcases typical configurations
2. The demo project is stored directly in the database alongside master data

#### Step 4: Commit to GitHub
Factory Mode edits `src/NIS.Desktop/Data/nisdata.db` directly.
1. Stage and commit the updated database:
   ```bash
   git add src/NIS.Desktop/Data/nisdata.db
   git commit -m "Update master data: [describe changes]"
   git push origin main
   ```

#### Step 5: Create Release
1. Tag the commit with a version number:
   ```bash
   git tag v1.x.x
   git push origin v1.x.x
   ```
2. GitHub Actions builds and packages the release with the updated database

### 9.5 Security Notes

- Factory Mode password is hardcoded - this is intentional as it's for development/maintenance access only
- Factory imports replace all data including user customizations - always warn before proceeding
- The `IsUserData` flag distinguishes factory data (false) from user-created data (true)

## 10. Localization and Theme

Accessible via: **Navigation Pane -> Settings**

### 10.1 Language Handling

#### 10.1.1 Supported Languages

| Code | Language | Display Name |
|------|----------|--------------|
| de   | German   | Deutsch      |
| fr   | French   | Français     |
| it   | Italian  | Italiano     |
| en   | English  | English      |

Default language: **de** (Deutsch)

#### 10.1.2 Language Selection

**In Settings:**
- Four toggle buttons displayed horizontally: de | fr | it | en
- Language change takes effect immediately across the entire UI
- Selected language persists across sessions
- Stored in settings.json and applied at startup
- All exports and reports use the current UI language setting

#### 10.1.3 Localization Architecture

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

#### 10.1.4 Translation Storage

**Embedded Translations:**
- Default translations are compiled into the application
- Stored in a dictionary structure: `Key → { Language → Value }`
- Covers all UI strings, labels, messages, and tooltips

**Custom Translations (AppData):**
- User modifications saved to: `%APPDATA%/SwissNISCalculator/translations.json`
- Merged with embedded translations at startup
- Custom translations override embedded defaults
- Translations are stored in JSON only and are not persisted in SQLite

#### 10.1.5 In-App Translation Editor

Accessible via: **Navigation Pane → Master Data → Translations Tab**

**Features:**
- DataGrid showing all translatable strings
- Columns: Category | German | French | Italian | English (key hidden and immutable)
- Inline editing for all language values
- Search/filter by text content
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

#### 10.1.6 String Categories

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

#### 10.1.7 Adding New Translations

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

#### 10.1.8 Output Document Language

- Markdown and PDF exports use the current UI language setting
- All parameter names, descriptions, and explanations are translated
- Numerical values and units remain unchanged across languages

### 10.2 Theme Support

- **Light Mode**: Default theme
- **Dark Mode**: Toggle available in Settings
- Theme applies globally to the entire application
- Stored in settings.json and applied at startup

### 10.3 Design Guidelines

UI should follow Windows 11 look and feel:
- Fluent-style surfaces, rounded corners, and soft shadows
- Segmented controls and modern toggle styles
- Consistent spacing and typography typical of Windows 11 apps

## 11. Distribution

### 11.1 Platforms

CI/CD must produce packaged releases for:
- Windows (x64)
- macOS (x64, ARM64)
- Linux (x64)

### 11.2 Bundled Data

The application ships with a single pre-populated SQLite database:

| File | Content | Location |
|------|---------|----------|
| `nisdata.db` | Factory master data + demo project | `Data/nisdata.db` |

The database contains:
- All factory master data (antennas, cables, radios, modulations, constants, bands) with `IsUserData=false`
- One demo project ready to explore

**Portable Application**: All files reside in the application folder. No data is stored in %APPDATA% or other system folders. Users can run the app from any location (USB drive, local folder, etc.).

### 11.3 Build and Release Process

#### Local Build
```bash
# Build for development
dotnet build

# Publish as standalone Windows executable
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

#### GitHub Actions Workflow (`.github/workflows/build.yml`)

Triggered by:
- Manual dispatch (Actions → "Run workflow" → enter version tag)

| Job | Platforms | Description |
|-----|-----------|-------------|
| cleanup | - | Deletes existing release and tag with the same version (if any) |
| build | Windows, macOS, Linux | Restore, build, test, publish as single-file self-contained executable |
| release | - | Creates GitHub Release with platform archives |

The workflow can be re-run with the same version tag to rebuild a release—it automatically cleans up the previous release first.

**Artifacts produced:**
- `SwissNISCalculator-Windows.zip`
- `SwissNISCalculator-macOS.zip`
- `SwissNISCalculator-Linux.tar.gz`

Each artifact contains a portable application (Section 11.2) with:
- Single executable file
- `Data/nisdata.db` - pre-populated database with factory master data and demo project

### 11.4 Master Data Update Workflow

Complete workflow for updating shipped master data (see also Section 9.4):

1. **Enter Factory Mode** (Section 9.1): Navigate to Factory in navigation pane, enter password
2. **Modify Data** (Section 9.2): Edit master data (antennas, cables, radios, modulations, constants, bands)
3. **Update Demo Project** (Section 9.3): Create or modify the demo project
4. **Commit**: Push `src/NIS.Desktop/Data/nisdata.db` to GitHub
5. **Run Release**: Go to Actions → "Build and Release" → "Run workflow" → enter version (e.g., v1.0.1)
6. **Distribute**: GitHub Actions builds and publishes the release

**Note**: No upgrade path exists. Each version ships a complete fresh database. Users start over with each new version.

## Appendix A: Antennas with Generated Vertical Radiation Patterns

This appendix is summarized to avoid listing all antenna records in the FSD.
Full antenna inventories and generated patterns are stored in the application database (`nisdata.db`).
Generated patterns follow the formulas in Section 8.4.

## Appendix B: Database Schema and JSON Formats

### B.1 Database Schema

```sql
-- Enable foreign key enforcement (run on every connection)
PRAGMA foreign_keys = ON;

-- MASTER DATA TABLES (IsUserData: false=factory, true=user)

CREATE TABLE Antennas (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Manufacturer TEXT NOT NULL,
    Model TEXT NOT NULL,
    AntennaType TEXT NOT NULL DEFAULT 'other',
    IsHorizontallyPolarized INTEGER NOT NULL DEFAULT 1,
    IsUserData INTEGER NOT NULL DEFAULT 0,
    BandsJson TEXT NOT NULL DEFAULT '[]',
    UNIQUE(Manufacturer, Model)
);

CREATE TABLE Cables (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL UNIQUE,
    IsUserData INTEGER NOT NULL DEFAULT 0,
    AttenuationsJson TEXT NOT NULL DEFAULT '{}'
);

CREATE TABLE Radios (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Manufacturer TEXT NOT NULL,
    Model TEXT NOT NULL,
    MaxPowerWatts REAL NOT NULL DEFAULT 100 CHECK (MaxPowerWatts > 0),
    IsUserData INTEGER NOT NULL DEFAULT 0,
    UNIQUE(Manufacturer, Model)
);

CREATE TABLE Okas (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL UNIQUE,
    DefaultDistanceMeters REAL NOT NULL DEFAULT 10 CHECK (DefaultDistanceMeters > 0),
    DefaultDampingDb REAL NOT NULL DEFAULT 0 CHECK (DefaultDampingDb >= 0),
    IsUserData INTEGER NOT NULL DEFAULT 1
);

CREATE TABLE Modulations (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL UNIQUE,
    Factor REAL NOT NULL CHECK (Factor > 0 AND Factor <= 1),
    IsUserData INTEGER NOT NULL DEFAULT 0
);
-- Factory data: SSB (0.2), CW (0.4), FM (1.0)

-- USER DATA TABLES

CREATE TABLE Projects (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    OperatorName TEXT,
    Callsign TEXT,
    Address TEXT,
    Location TEXT,
    CreatedAt TEXT NOT NULL,
    ModifiedAt TEXT NOT NULL
);

CREATE TABLE Configurations (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ProjectId INTEGER NOT NULL,
    ConfigNumber INTEGER NOT NULL CHECK (ConfigNumber > 0),
    Name TEXT,
    PowerWatts REAL NOT NULL DEFAULT 100 CHECK (PowerWatts > 0),
    RadioId INTEGER,
    LinearName TEXT,
    LinearPowerWatts REAL NOT NULL DEFAULT 0 CHECK (LinearPowerWatts >= 0),
    CableId INTEGER,
    CableLengthMeters REAL NOT NULL DEFAULT 10 CHECK (CableLengthMeters >= 0),
    AdditionalLossDb REAL NOT NULL DEFAULT 0 CHECK (AdditionalLossDb >= 0),
    AdditionalLossDescription TEXT,
    AntennaId INTEGER,
    HeightMeters REAL NOT NULL DEFAULT 10 CHECK (HeightMeters > 0),
    IsRotatable INTEGER NOT NULL DEFAULT 0,
    HorizontalAngleDegrees REAL NOT NULL DEFAULT 360 CHECK (HorizontalAngleDegrees >= 0 AND HorizontalAngleDegrees <= 360),
    ModulationId INTEGER,
    ActivityFactor REAL NOT NULL DEFAULT 0.5 CHECK (ActivityFactor > 0 AND ActivityFactor <= 1),
    OkaId INTEGER,
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
    FOREIGN KEY (RadioId) REFERENCES Radios(Id) ON DELETE RESTRICT,
    FOREIGN KEY (CableId) REFERENCES Cables(Id) ON DELETE RESTRICT,
    FOREIGN KEY (AntennaId) REFERENCES Antennas(Id) ON DELETE RESTRICT,
    FOREIGN KEY (ModulationId) REFERENCES Modulations(Id) ON DELETE RESTRICT,
    FOREIGN KEY (OkaId) REFERENCES Okas(Id) ON DELETE RESTRICT
);

-- Indexes for foreign keys (improves JOIN and DELETE performance)
CREATE INDEX IX_Configurations_ProjectId ON Configurations(ProjectId);
CREATE INDEX IX_Configurations_RadioId ON Configurations(RadioId);
CREATE INDEX IX_Configurations_CableId ON Configurations(CableId);
CREATE INDEX IX_Configurations_AntennaId ON Configurations(AntennaId);
CREATE INDEX IX_Configurations_ModulationId ON Configurations(ModulationId);
CREATE INDEX IX_Configurations_OkaId ON Configurations(OkaId);

-- Unique constraint: each project can only have one config per number
CREATE UNIQUE INDEX IX_Configurations_ProjectId_ConfigNumber ON Configurations(ProjectId, ConfigNumber);
```

### B.2 JSON Column Formats

#### B.2.1 BandsJson (Antennas Table)

```json
[
  {
    "frequencyMHz": 14.0,
    "gainDbi": 8.0,
    "pattern": [0.0, 0.2, 0.5, 1.2, 2.3, 4.0, 6.2, 9.1, 12.8, 17.5]
  }
]
```

**Pattern Array**: 10 values = attenuation (dB) at 0°, 10°, 20°, ... 90° elevation.

#### B.2.2 AttenuationsJson (Cables Table)

```json
{
  "1.8": 0.478,
  "7": 0.994,
  "14": 1.457,
  "28": 2.141,
  "144": 4.800,
  "430": 8.900
}
```

Keys = frequency (MHz), values = attenuation (dB per 100m). Missing frequencies are interpolated.

### B.3 Export/Import JSON (User and Factory)

User and Factory exports share the same JSON structure. Factory import uses the same structure but sets all master data `IsUserData` flags to false after import.

```json
{
  "exportDate": "2024-01-31T12:34:56.789Z",
  "projects": [ /* Project objects, including configurations */ ],
  "okas": [ /* Evaluation point master data (OKA/LSM/LST/PSS) */ ],
  "userAntennas": [ /* Antenna master data */ ],
  "userCables": [ /* Cable master data */ ],
  "userRadios": [ /* Radio master data */ ]
}
```

### B.4 Project File Format (.nisproj)

```json
{
  "project": {
    "name": "Example Station",
    "operator": "HB9XX",
    "callsign": "HB9XX",
    "address": "Street 1, 8000 City",
    "location": "City, CH"
  },
  "configurations": [
    {
      "antenna": { "manufacturer": "Opti-Beam", "model": "OB9-5" },
      "heightMeters": 12,
      "polarization": "horizontal",
      "rotationAngleDegrees": 360,
      "radio": { "manufacturer": "Yaesu", "model": "FT-1000" },
      "linear": null,
      "powerWatts": 100,
      "cable": { "name": "EcoFlex10" },
      "cableLengthMeters": 15,
      "additionalLossDb": 1.0,
      "additionalLossDescription": "Connectors",
      "modulation": "SSB",
      "activityFactor": 0.5,
      "oka": { "id": 1 }
    }
  ],
  "masterData": {
    "antennas": [],
    "cables": [],
    "radios": [],
    "okas": []
  }
}
```

Notes:
- `linear` is optional; use `null` when no linear is present. When present, it contains `name` (string) and `powerWatts` (number), e.g., `{ "name": "SPE Expert 1.5K", "powerWatts": 1500 }`.
- `oka` references OKA master data by ID. Distance and damping values are stored in OKA master data (single source of truth).
- `masterData` is always present (even if arrays are empty) to show the structure for user-specific master data.
- Only user-specific master data (IsUserData=true) is included in `masterData`; factory data is not exported.

### B.5 settings.json

```json
{
  "language": "de",
  "themeMode": 0
}
```

Notes:
- `themeMode`: 0 = System, 1 = Light, 2 = Dark.

### B.6 translations.json

```json
[
  {
    "Key": "Save",
    "Category": "Common",
    "de": "Speichern",
    "fr": "Enregistrer",
    "it": "Salva",
    "en": "Save"
  }
]
```

Notes:
- `Key` and `Category` are required.
- Empty language values are ignored on load to preserve defaults.

### B.7 masterdata.json

```json
{
  "bands": [
    { "name": "160m", "frequencyMHz": 1.8 },
    { "name": "80m", "frequencyMHz": 3.5 }
  ],
  "constants": {
    "groundReflectionFactor": 1.6,
    "defaultActivityFactor": 0.5
  }
}
```

## Appendix C: Example: HB9FS Station

| Config | Radio | Cable | Antenna | Height | Bands | Evaluation Point |
|--------|-------|-------|---------|--------|-------|------------------|
| HF Station | 100W | EcoFlex10 15m | Opti-Beam OB9-5 | 12m | 14,18,21,24,28 MHz | Neighbor balcony @ 5.4m |
| 6m Station | 100W | Aircom-plus 20m | Wimo ZX6-2 | 10m | 50 MHz | Garden fence @ 7.4m |
| VHF/UHF | 100W | Aircom-plus 17m | Diamond X-50 | 14m | 144, 432 MHz | Terrace @ 10.4m |

Each configuration includes antenna height and references an evaluation point (OKA/LSM/LST/PSS) from master data (distance and building damping are stored in OKA master data).
