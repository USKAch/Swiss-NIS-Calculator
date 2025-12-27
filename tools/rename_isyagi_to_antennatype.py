import json

# Read the JSON file
with open('src/NIS.Core/Resources/antennas.json', 'r', encoding='utf-8') as f:
    data = json.load(f)

# Track counts
yagi_count = 0
other_count = 0

# Process each antenna - rename isYagi to antennaType
for antenna in data['antennas']:
    if 'isYagi' in antenna:
        is_yagi = antenna.pop('isYagi')
        antenna['antennaType'] = 'yagi' if is_yagi else 'other'

        if is_yagi:
            yagi_count += 1
        else:
            other_count += 1

# Write updated JSON
with open('src/NIS.Core/Resources/antennas.json', 'w', encoding='utf-8') as f:
    json.dump(data, f, indent=2, ensure_ascii=False)

# Print summary
print(f"Renamed isYagi to antennaType:")
print(f"  Yagi antennas: {yagi_count}")
print(f"  Other antennas: {other_count}")
print(f"  Total: {yagi_count + other_count}")
