# Release Test Checklist

## Core calculations
- Unit tests for formulas: Pm, a, A, g, G, EIRP, ERP, E, ds with known inputs.
- NIS limit table checks: verify band -> limit mapping matches FSD (including interpolation rules).
- Vertical pattern interpolation: edge angles (0/90), midpoints, and out-of-range handling.
- Cable attenuation interpolation: exact match, between points, below/above range.
- Rounding rules: output values rounded as specified.

## Data integrity / DB
- Schema creation: FK enabled, indexes present, CHECK constraints enforced.
- CRUD per master data type (antenna/cable/radio/OKA/modulation).
- Deleting master data with references: FK behavior (SET NULL/CASCADE) verified.
- Project save/update with configurations: data persists correctly, no orphan rows.

## Import/Export
- User export -> import round-trip: identical data post-import.
- Factory export -> import round-trip: IsUserData set to false.
- Import validation: invalid JSON, missing required fields, negative values.
- .nisproj import with missing master data: placeholder behavior.
- Backward compatibility: legacy .nisproj fields (if still supported).

## UI behavior
- Unsaved changes warnings: editor navigation, app close.
- Master Data Manager: permissions (user vs factory mode).
- Search/sort behavior in project list and master data tabs.
- Read-only behavior for factory data in non-admin mode.

## Localization
- Strings exist for all bindings; no missing keys.
- German is master language in translation grid; English last.
- Language switch updates active views.

## Performance / stress
- Large number of projects/configs: navigation and calculations remain responsive.
- Large antenna pattern datasets: no UI freeze.

## Packaging/Release
- Windows/macOS/Linux build artifacts from GitHub Actions.
- Fresh install: first-run initialization creates data folder and defaults.
- Upgrade path: schema compatibility warning behaves as specified.

## Code review checklist (pre-release)
- Confirm code matches the FSD and document any deviations.
- Review architecture boundaries (ViewModels vs Services vs Models) and flag overly large classes.
- Verify error handling and user feedback for all failure paths.
- Validate data handling rules (IDs vs names, FK behavior, imports are destructive with confirmations).
- Check localization usage and remove hardcoded UI strings.
- Confirm no dead code or legacy fallbacks remain unless explicitly required.
- Review performance hotspots (per-row DB calls, UI thread blocking).
- Validate packaging artifacts and versioning rules.
- Check for unused view models, models, commands, and properties with no bindings.
- Verify master data lookups do not loop per-row (cache or bulk fetch).
- Confirm import/export formats match Appendix B and FSD Section 6.2/6.2.1.

## Code review TODO (current repo findings)
- Replace name-based deletes with ID-based deletes and update callers:
  - `src/NIS.Desktop/Services/DatabaseService.cs:355`
  - `src/NIS.Desktop/Services/DatabaseService.cs:487`
  - `src/NIS.Desktop/Services/DatabaseService.cs:608`
- Save configurations using IDs directly (stop name/model lookups in save path):
  - `src/NIS.Desktop/Services/DatabaseService.cs:987-991`
- Wrap `ImportFactoryData` and `ImportUserData` in a single transaction (ClearAllData + imports):
  - `src/NIS.Desktop/Services/DatabaseService.cs:1147-1290`
- Add post-import integrity validation for missing FK references and report to user:
  - `src/NIS.Desktop/Services/DatabaseService.cs:1147-1290`
- Remove unused ProjectOverview commands/properties or wire them into the view:
  - `src/NIS.Desktop/ViewModels/ProjectOverviewViewModel.cs:169-198`
  - `src/NIS.Desktop/Views/ProjectOverviewView.axaml`
- Remove or justify unused `ShowProjectSelectionAsync`:
  - `src/NIS.Desktop/ViewModels/MainShellViewModel.cs:1358`
- Remove or justify unused constructor parameter in `ProjectInfoViewModel(string language)`:
  - `src/NIS.Desktop/ViewModels/ProjectInfoViewModel.cs:55`
  - `src/NIS.Desktop/ViewModels/MainShellViewModel.cs:212`
- Remove or justify unused `EvaluationPoint` model:
  - `src/NIS.Desktop/Models/Project.cs:172`
- Remove or justify legacy JSON fallback fields if no longer required:
  - `src/NIS.Desktop/Models/Project.cs:212-248`
  - `src/NIS.Desktop/Models/Antenna.cs:74-140`
