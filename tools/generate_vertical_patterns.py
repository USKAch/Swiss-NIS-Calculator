import json
import math

def generate_vertical_pattern(gain_dbi):
    """
    Generate vertical radiation pattern for omnidirectional vertical antennas.

    Formula:
    - HPBW = 105° / √(10^(GainDbi/10))
    - Loss(θ) = min(ZenithAttenuation, Rolloff × (θ / (HPBW/2))²)
    - Rolloff = 3.0 (ensures 3 dB loss at half beamwidth)
    - ZenithAttenuation = 20 + (GainDbi × 1.5)

    Returns array of 10 attenuation values for angles 0°, 10°, 20°, ..., 90°
    """
    # Calculate linear gain and HPBW
    linear_gain = 10 ** (gain_dbi / 10)
    hpbw = 105.0 / math.sqrt(linear_gain)

    # Constants
    rolloff = 3.0
    zenith_attenuation = 20.0 + (gain_dbi * 1.5)

    pattern = []
    for i in range(10):
        theta = i * 10  # 0, 10, 20, ..., 90 degrees

        # Calculate elevation loss
        half_hpbw = hpbw / 2.0
        if half_hpbw > 0:
            elevation_loss = rolloff * (theta / half_hpbw) ** 2
        else:
            elevation_loss = zenith_attenuation

        # Cap at zenith attenuation
        attenuation = min(zenith_attenuation, elevation_loss)

        # Round to 1 decimal place
        pattern.append(round(attenuation, 1))

    return pattern


def is_zero_pattern(pattern):
    """Check if pattern is all zeros."""
    return all(p == 0 for p in pattern)


# Read the JSON file
with open('src/NIS.Core/Resources/antennas.json', 'r', encoding='utf-8') as f:
    data = json.load(f)

# Find vertical antennas with zero patterns and update them
updated = []

print('Vertical antennas with zero patterns - generating new patterns:\n')
print('| Manufacturer | Model | Freq (MHz) | Gain (dBi) | Pattern (0°,30°,60°,90°) |')
print('|--------------|-------|------------|------------|--------------------------|')

for antenna in data['antennas']:
    if antenna.get('antennaType') == 'vertical':
        manufacturer = antenna['manufacturer']
        model = antenna['model']

        for band in antenna['bands']:
            if is_zero_pattern(band['pattern']):
                gain = band['gainDbi']
                new_pattern = generate_vertical_pattern(gain)
                band['pattern'] = new_pattern
                updated.append({
                    'manufacturer': manufacturer,
                    'model': model,
                    'frequency': band['frequencyMHz'],
                    'gain': gain
                })
                print(f"| {manufacturer} | {model} | {band['frequencyMHz']} | {gain} | [{new_pattern[0]}, {new_pattern[3]}, {new_pattern[6]}, {new_pattern[9]}] |")

# Write updated JSON
with open('src/NIS.Core/Resources/antennas.json', 'w', encoding='utf-8') as f:
    json.dump(data, f, indent=2, ensure_ascii=False)

print(f'\nUpdated {len(updated)} vertical antenna bands with generated patterns.')
