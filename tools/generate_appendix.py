import json

with open('src/NIS.Core/Resources/antennas.json', 'r', encoding='utf-8') as f:
    data = json.load(f)

# Collect antennas with non-zero patterns by type (yagi, quad, log-periodic)
directional_types = ['yagi', 'quad', 'log-periodic']
antennas_with_patterns = []

for antenna in data['antennas']:
    atype = antenna.get('antennaType', 'other')
    if atype in directional_types:
        for band in antenna['bands']:
            if any(p != 0 for p in band['pattern']):
                antennas_with_patterns.append({
                    'manufacturer': antenna['manufacturer'],
                    'model': antenna['model'],
                    'type': atype,
                    'freq': band['frequencyMHz'],
                    'gain': band['gainDbi'],
                    'pattern': band['pattern']
                })

# Sort by type, manufacturer, model, freq
antennas_with_patterns.sort(key=lambda x: (x['type'], x['manufacturer'], x['model'], x['freq']))

# Print markdown table
print('## Appendix A: Generated Vertical Radiation Patterns')
print()
print('The following antenna bands have vertical radiation patterns generated using the gain-based formula from Section 11.4.')
print()
print('| Type | Manufacturer | Model | Freq (MHz) | Gain (dBi) | Pattern (0°-90° in 10° steps) |')
print('|------|--------------|-------|------------|------------|-------------------------------|')

for a in antennas_with_patterns:
    pattern_str = ', '.join(str(p) for p in a['pattern'])
    print(f"| {a['type']} | {a['manufacturer']} | {a['model']} | {a['freq']} | {a['gain']} | [{pattern_str}] |")

print()
print(f"*Total: {len(antennas_with_patterns)} antenna bands with generated patterns.*")
