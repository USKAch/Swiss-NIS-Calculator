import json

with open('src/NIS.Core/Resources/antennas.json', 'r', encoding='utf-8') as f:
    data = json.load(f)

# Collect unique antennas with non-zero patterns by type (yagi, quad, log-periodic)
directional_types = ['log-periodic', 'quad', 'yagi']
antennas_seen = set()
antennas_with_patterns = []

for antenna in data['antennas']:
    atype = antenna.get('antennaType', 'other')
    if atype in directional_types:
        key = (antenna['manufacturer'], antenna['model'])
        if key not in antennas_seen:
            # Check if any band has non-zero pattern
            has_pattern = any(any(p != 0 for p in band['pattern']) for band in antenna['bands'])
            if has_pattern:
                antennas_seen.add(key)
                # Get gain range
                gains = [band['gainDbi'] for band in antenna['bands']]
                gain_str = f"{min(gains):.1f}" if min(gains) == max(gains) else f"{min(gains):.1f}-{max(gains):.1f}"
                antennas_with_patterns.append({
                    'manufacturer': antenna['manufacturer'],
                    'model': antenna['model'],
                    'type': atype,
                    'gain': gain_str,
                    'bands': len(antenna['bands'])
                })

# Sort by type, manufacturer, model
antennas_with_patterns.sort(key=lambda x: (x['type'], x['manufacturer'], x['model']))

# Print markdown
print("""
## Appendix A: Antennas with Generated Vertical Radiation Patterns

The following directional antennas (Yagi, Quad, Log-Periodic) have vertical radiation patterns generated using the gain-based formula from Section 11.4. Patterns are calculated for each frequency band based on antenna gain.

### A.1 Log-Periodic Antennas

| Manufacturer | Model | Gain (dBi) | Bands |
|--------------|-------|------------|-------|""")

for a in antennas_with_patterns:
    if a['type'] == 'log-periodic':
        print(f"| {a['manufacturer']} | {a['model']} | {a['gain']} | {a['bands']} |")

print("""
### A.2 Quad Antennas

| Manufacturer | Model | Gain (dBi) | Bands |
|--------------|-------|------------|-------|""")

for a in antennas_with_patterns:
    if a['type'] == 'quad':
        print(f"| {a['manufacturer']} | {a['model']} | {a['gain']} | {a['bands']} |")

print("""
### A.3 Yagi Antennas

| Manufacturer | Model | Gain (dBi) | Bands |
|--------------|-------|------------|-------|""")

for a in antennas_with_patterns:
    if a['type'] == 'yagi':
        print(f"| {a['manufacturer']} | {a['model']} | {a['gain']} | {a['bands']} |")

# Count totals
lp_count = sum(1 for a in antennas_with_patterns if a['type'] == 'log-periodic')
quad_count = sum(1 for a in antennas_with_patterns if a['type'] == 'quad')
yagi_count = sum(1 for a in antennas_with_patterns if a['type'] == 'yagi')

print(f"""
*Summary: {lp_count} Log-Periodic, {quad_count} Quad, {yagi_count} Yagi antennas with generated patterns.*
""")
