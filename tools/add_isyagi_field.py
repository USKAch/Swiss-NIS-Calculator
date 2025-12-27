import json
import re

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

# Track counts
yagi_count = 0
non_yagi_count = 0

# Process each antenna - add isYagi field
for antenna in data['antennas']:
    manufacturer = antenna['manufacturer']
    model = antenna['model']

    antenna['isYagi'] = is_yagi(manufacturer, model)

    if antenna['isYagi']:
        yagi_count += 1
    else:
        non_yagi_count += 1

# Write updated JSON
with open('src/NIS.Core/Resources/antennas.json', 'w', encoding='utf-8') as f:
    json.dump(data, f, indent=2, ensure_ascii=False)

# Print summary
print(f"Added isYagi field to all antennas:")
print(f"  Yagi antennas: {yagi_count}")
print(f"  Non-Yagi antennas: {non_yagi_count}")
print(f"  Total: {yagi_count + non_yagi_count}")
