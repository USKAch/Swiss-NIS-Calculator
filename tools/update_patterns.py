import json
import math
import re

def generate_pattern_from_gain(gain_dbi):
    """Generate realistic vertical radiation pattern based on antenna gain."""
    # For a horizontal Yagi, vertical half-power beamwidth is approximately:
    # HPBW ≈ 105 / sqrt(linear_gain)
    linear_gain = 10 ** (gain_dbi / 10.0)
    hpbw_degrees = 105.0 / math.sqrt(linear_gain)

    # Half-power angle (where attenuation is 3 dB)
    half_power_angle = hpbw_degrees / 2.0

    pattern = []
    for i in range(10):
        angle = i * 10.0  # 0, 10, 20, ... 90 degrees

        if angle <= half_power_angle:
            # Within half-power angle: gradual Gaussian-like rolloff
            att = 3.0 * (angle / half_power_angle) ** 2
        else:
            # Beyond half-power: steeper rolloff toward zenith attenuation
            normalized_angle = (angle - half_power_angle) / (90.0 - half_power_angle)
            zenith_attenuation = 20.0 + (gain_dbi - 6.0)
            zenith_attenuation = max(20.0, min(35.0, zenith_attenuation))
            att = 3.0 + normalized_angle * (zenith_attenuation - 3.0)

        # Round to 1 decimal place
        pattern.append(round(att, 1))

    return pattern

def is_all_zeros(pattern):
    """Check if pattern is all zeros."""
    return all(p == 0 for p in pattern)

def is_yagi(manufacturer, model):
    """Detect if antenna is a Yagi based on manufacturer/model naming patterns."""
    name = f"{manufacturer} {model}".lower()

    # Explicit Yagi indicators
    if 'yagi' in name:
        return True

    # Known Yagi manufacturers/model patterns
    yagi_patterns = [
        r'\d+-\d+cd',      # Cushcraft pattern like 10-3CD, 15-4CD, 20-4CD, 40-2CD
        r'\d+bas',          # Hy-Gain BAS series
        r'th\d+',           # Hy-Gain TH series
        r'th\d+mk',         # Hy-Gain TH Mk series
        r'^a-?\d+-?s',      # Cushcraft A-3-S, A50-3S, etc.
        r'^a\d+s',          # Cushcraft A503S, A505S
        r'fb-?d',           # Fritzel FB-Do, FB-DX
        r'^fb\d+',          # Fritzel FB13, FB34
        r'pro-\d+',         # Mosley Pro series
        r'ta-\d+',          # Mosley TA series
        r'tw-\d+',          # Mosley TW series
        r'cl-\d+',          # Mosley CL series
        r'mp-\d+',          # Mosley MP series
        r'mini-\d+',        # Mosley Mini series
        r'^lp-?\d+',        # Titanex LP series (log-periodic, similar pattern)
        r'xp\d+',           # Somme XP series
        r'^2\d{4}',         # Tonna 20xxx series
        r'wx\d+',           # Wimo WX series
        r'wy\d+',           # Wimo WY series
        r'x-quad',          # Wimo X-Quad
        r'fx\d+',           # Flexa FX series
        r'\d+wb',           # Cushcraft WB series (124WB)
        r'\d+b2n',          # Cushcraft B2N series (13B2N)
        r'a\d+n',           # Cushcraft N series (A14410SN, A43011N)
        r'a27-\d+s',        # Cushcraft dual-band
        r'a270-\d+s',       # Cushcraft dual-band
        r'719b|729b',       # Cushcraft 70cm beams
        r'ca-52',           # Comet CA-52HB4
        r'shf\d+',          # SHF series
    ]

    for pattern in yagi_patterns:
        if re.search(pattern, model.lower()):
            return True

    return False

# Read the JSON file
with open('src/NIS.Core/Resources/antennas.json', 'r', encoding='utf-8') as f:
    data = json.load(f)

# Track updated antennas
updated_antennas = []

# Process each antenna
for antenna in data['antennas']:
    manufacturer = antenna['manufacturer']
    model = antenna['model']

    for band in antenna['bands']:
        freq = band['frequencyMHz']
        gain = band['gainDbi']
        pattern = band['pattern']

        if is_all_zeros(pattern):
            new_pattern = generate_pattern_from_gain(gain)
            band['pattern'] = new_pattern
            updated_antennas.append({
                'manufacturer': manufacturer,
                'model': model,
                'frequency': freq,
                'gain': gain,
                'pattern': new_pattern
            })

# Write updated JSON
with open('src/NIS.Core/Resources/antennas.json', 'w', encoding='utf-8') as f:
    json.dump(data, f, indent=2, ensure_ascii=False)

# Print summary
print(f"Updated {len(updated_antennas)} antenna bands with realistic patterns\n")
print("| Manufacturer | Model | Freq (MHz) | Gain (dBi) | Type |")
print("|--------------|-------|------------|------------|------|")
for ant in updated_antennas:
    yagi = "Yagi" if is_yagi(ant['manufacturer'], ant['model']) else ""
    print(f"| {ant['manufacturer']} | {ant['model']} | {ant['frequency']} | {ant['gain']} | {yagi} |")
