import json
import re

def classify_antenna(manufacturer, model):
    """Classify antenna into one of the defined types."""
    name = f"{manufacturer} {model}".lower()
    model_lower = model.lower()

    # Log-Periodic antennas
    log_periodic_patterns = [
        r'^lp-?\d+',           # Titanex LP series
        r'^dlp-?\d+',          # Titanex DLP series (also log-periodic)
    ]
    for pattern in log_periodic_patterns:
        if re.search(pattern, model_lower):
            return "log-periodic"

    # Quad antennas
    if 'quad' in name or 'x-quad' in name:
        return "quad"

    # Loop antennas
    if 'loop' in name or 'deltaloop' in model_lower or 'delta loop' in name:
        return "loop"

    # Wire antennas
    wire_patterns = [
        r'dipol',              # Dipol, Dipole
        r'g5rv',               # G5RV
        r'w3dzz',              # W3DZZ
        r'doublet',            # Doublet
        r'^lw\s*\d+',          # Langdraht LW series
        r'windom',             # Windom
        r'zepp',               # Zepp, Zeppelin
        r'inverted.?v',        # Inverted V
        r'fd-?\d+',            # Fritzel FD series (folded dipole)
    ]
    for pattern in wire_patterns:
        if re.search(pattern, name):
            return "wire"
    if manufacturer.lower() == 'langdraht':
        return "wire"

    # Vertical antennas (ground planes, verticals)
    vertical_patterns = [
        r'^gp',                # GP (ground plane)
        r'gpa\d+',             # Fritzel GPA series
        r'^v-?\d+',            # V-2000, etc.
        r'vertical',           # Explicit vertical
        r'^r-?\d+',            # Cushcraft R series (R6000, R7000, etc.)
        r'r\d+xyz',            # R6xyz, R7xyz, R8xyz
        r'^cx-?\d+',           # Comet CX series
        r'hustler',            # Hustler verticals
        r'butternut',          # Butternut verticals
        r'hf-?\d+[a-z]?$',     # Butterfly HF series
        r'^av-?\d+',           # AV series verticals
        r'^atas',              # Yaesu ATAS
        r'^bwd-?\d+',          # Barker & Williamson BWD
        r'^asl\d+',            # Cushcraft ASL (vertical)
        r'd-\d+-w',            # Cushcraft D-3-W, D-4 (trap vertical)
        r'^d-\d+$',            # Cushcraft D-4
        r'triple',             # GP Triple
        r'ringo',              # Ringo Range (vertical)
        r'helix',              # Helix (vertical collinear)
        r'collinear',          # Collinear
        r'x-?\d+0$',           # Diamond X-50, X-200, X-300
        r'^f-?\d+$',           # Diamond F-23
        r'^g-?\d+',            # Diamond G-200
        r'^dp-?cp',            # Diamond DP-CP6
        r'ufb\d+',             # Fritzel UFB series
        r'w3-\d+',             # Fritzel W3 series
    ]
    for pattern in vertical_patterns:
        if re.search(pattern, model_lower):
            return "vertical"

    # Yagi antennas (directional beams)
    yagi_patterns = [
        r'yagi',               # Explicit Yagi
        r'\d+-\d+cd',          # Cushcraft CD series (10-3CD, 15-4CD, 20-4CD, 40-2CD)
        r'\d+bas',             # Hy-Gain BAS series
        r'^th\d+',             # Hy-Gain TH series
        r'^a-?\d+-?s',         # Cushcraft A-3-S, A-4-S
        r'^a\d+s',             # Cushcraft A503S, A505S
        r'^a50-\d+s',          # Cushcraft A50-3S, A50-5S
        r'fb-?d',              # Fritzel FB-Do, FB-DX
        r'^fb\d+',             # Fritzel FB13, FB34
        r'pro-?\d+',           # Mosley Pro series
        r'^ta-?\d+',           # Mosley TA series
        r'^tw-?\d+',           # Mosley TW series
        r'^cl-?\d+',           # Mosley CL series
        r'^mp-?\d+',           # Mosley MP series
        r'^mini-?\d+',         # Mosley Mini series
        r'^xp\d+',             # Somme XP series
        r'^2\d{4}',            # Tonna 20xxx series
        r'^wx\d+',             # Wimo WX series
        r'^wy\d+',             # Wimo WY series
        r'^fx\d+',             # Flexa FX series
        r'\d+wb',              # Cushcraft WB series (124WB)
        r'\d+b2n',             # Cushcraft B2N series (13B2N)
        r'a\d+n',              # Cushcraft N series (A14410SN, A43011N)
        r'^a27-\d+s',          # Cushcraft dual-band
        r'^a270-\d+s',         # Cushcraft dual-band
        r'^719b|^729b',        # Cushcraft 70cm beams
        r'^ca-52',             # Comet CA-52HB4
        r'^shf\d+',            # SHF series
        r'^ob\d+',             # Opti-Beam OB series
    ]
    for pattern in yagi_patterns:
        if re.search(pattern, model_lower):
            return "yagi"

    # Default to other
    return "other"


# Read the JSON file
with open('src/NIS.Core/Resources/antennas.json', 'r', encoding='utf-8') as f:
    data = json.load(f)

# Track counts by type
counts = {
    "log-periodic": 0,
    "loop": 0,
    "other": 0,
    "quad": 0,
    "vertical": 0,
    "wire": 0,
    "yagi": 0
}

# Classification details for reporting
classifications = []

# Process each antenna
for antenna in data['antennas']:
    manufacturer = antenna['manufacturer']
    model = antenna['model']
    antenna_type = classify_antenna(manufacturer, model)

    antenna['antennaType'] = antenna_type
    counts[antenna_type] += 1
    classifications.append({
        'manufacturer': manufacturer,
        'model': model,
        'type': antenna_type
    })

# Write updated JSON
with open('src/NIS.Core/Resources/antennas.json', 'w', encoding='utf-8') as f:
    json.dump(data, f, indent=2, ensure_ascii=False)

# Print summary
print("Antenna Classification Summary:")
print("=" * 40)
for t in sorted(counts.keys()):
    print(f"  {t}: {counts[t]}")
print(f"  {'='*20}")
print(f"  Total: {sum(counts.values())}")
print()

# Print by type
for antenna_type in sorted(counts.keys()):
    if counts[antenna_type] > 0:
        print(f"\n{antenna_type.upper()} ({counts[antenna_type]}):")
        print("-" * 40)
        for c in classifications:
            if c['type'] == antenna_type:
                print(f"  {c['manufacturer']} {c['model']}")
