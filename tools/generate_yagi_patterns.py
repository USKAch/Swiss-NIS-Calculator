import json
import math

def generate_pattern_from_gain(gain_dbi):
    """
    Generate vertical radiation pattern for Yagi antenna based on gain.

    Formula from FSD Section 11.4:
    - HPBW = 105° / √(G_linear), where G_linear = 10^(G_dBi / 10)
    - θ_hp = HPBW / 2 (half-power angle)

    Attenuation at angle θ:
    - For θ ≤ θ_hp: Attenuation(θ) = 3 × (θ / θ_hp)²
    - For θ > θ_hp: Attenuation(θ) = 3 + ((θ - θ_hp) / (90 - θ_hp)) × (A_zenith - 3)
      where A_zenith = min(35, max(20, 20 + (G_dBi - 6)))

    Returns array of 10 attenuation values for angles 0°, 10°, 20°, ..., 90°
    """
    # Calculate linear gain and HPBW
    g_linear = 10 ** (gain_dbi / 10)
    hpbw = 105.0 / math.sqrt(g_linear)
    theta_hp = hpbw / 2  # Half-power angle

    # Calculate zenith attenuation (clamped between 20 and 35 dB)
    a_zenith = min(35, max(20, 20 + (gain_dbi - 6)))

    pattern = []
    for i in range(10):
        theta = i * 10  # 0, 10, 20, ..., 90 degrees

        if theta <= theta_hp:
            # Within half-power beamwidth
            if theta_hp > 0:
                attenuation = 3 * (theta / theta_hp) ** 2
            else:
                attenuation = 0
        else:
            # Beyond half-power beamwidth
            if (90 - theta_hp) > 0:
                attenuation = 3 + ((theta - theta_hp) / (90 - theta_hp)) * (a_zenith - 3)
            else:
                attenuation = a_zenith

        # Round to 1 decimal place
        pattern.append(round(attenuation, 1))

    return pattern


def is_zero_pattern(pattern):
    """Check if pattern is all zeros."""
    return all(p == 0 for p in pattern)


# Read the JSON file
with open('src/NIS.Core/Resources/antennas.json', 'r', encoding='utf-8') as f:
    data = json.load(f)

# Find Yagi antennas with zero patterns
yagis_with_zero_pattern = []

for antenna in data['antennas']:
    if antenna.get('antennaType') == 'yagi':
        manufacturer = antenna['manufacturer']
        model = antenna['model']

        for band in antenna['bands']:
            if is_zero_pattern(band['pattern']):
                yagis_with_zero_pattern.append({
                    'manufacturer': manufacturer,
                    'model': model,
                    'frequency': band['frequencyMHz'],
                    'gain': band['gainDbi'],
                    'band': band
                })

# Print summary
print(f"Found {len(yagis_with_zero_pattern)} Yagi antenna bands with zero patterns:\n")
print("| Manufacturer | Model | Freq (MHz) | Gain (dBi) | Generated Pattern |")
print("|--------------|-------|------------|------------|-------------------|")

# Generate and apply patterns
for item in yagis_with_zero_pattern:
    gain = item['gain']
    new_pattern = generate_pattern_from_gain(gain)
    item['band']['pattern'] = new_pattern

    # Format pattern for display (abbreviated)
    pattern_str = f"[{new_pattern[0]}, {new_pattern[3]}, {new_pattern[6]}, {new_pattern[9]}]"
    print(f"| {item['manufacturer']} | {item['model']} | {item['frequency']} | {gain} | {pattern_str} |")

# Write updated JSON
with open('src/NIS.Core/Resources/antennas.json', 'w', encoding='utf-8') as f:
    json.dump(data, f, indent=2, ensure_ascii=False)

print(f"\nUpdated {len(yagis_with_zero_pattern)} antenna bands with generated patterns.")
