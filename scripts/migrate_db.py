#!/usr/bin/env python3
"""
Database migration script for NIS Calculator.

Migration v0.5:
- Removes OkaDistanceMeters and OkaBuildingDampingDb columns from Configurations table
  (these values now come from OKA master data - single source of truth)

Run this once to update existing databases before using the new version.
"""

import sqlite3
import shutil
from pathlib import Path
from datetime import datetime


def get_column_names(cursor, table_name):
    """Get all column names for a table."""
    cursor.execute(f"PRAGMA table_info({table_name})")
    return {row[1] for row in cursor.fetchall()}


def migrate_database(db_path: Path):
    """Apply all migrations to the database."""

    if not db_path.exists():
        print(f"Database not found: {db_path}")
        return False

    # Create backup
    backup_path = db_path.with_suffix(f".db.backup_{datetime.now().strftime('%Y%m%d_%H%M%S')}")
    print(f"Creating backup: {backup_path}")
    shutil.copy2(db_path, backup_path)

    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()

    try:
        changes_made = False

        # Migration: Remove OkaDistanceMeters and OkaBuildingDampingDb from Configurations
        # (values now come from OKA master data - single source of truth)
        cursor.execute("SELECT name FROM sqlite_master WHERE type='table' AND name='Configurations'")
        if cursor.fetchone():
            columns = get_column_names(cursor, "Configurations")

            if "OkaDistanceMeters" in columns or "OkaBuildingDampingDb" in columns:
                print("Removing OkaDistanceMeters and OkaBuildingDampingDb columns...")
                print("(These values now come from OKA master data)")

                # SQLite doesn't support DROP COLUMN in older versions, so we recreate the table
                cursor.execute("""
                    CREATE TABLE Configurations_new (
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
                    )
                """)

                # Copy data (excluding the removed columns)
                cursor.execute("""
                    INSERT INTO Configurations_new (
                        Id, ProjectId, ConfigNumber, Name, PowerWatts,
                        RadioId, LinearName, LinearPowerWatts,
                        CableId, CableLengthMeters, AdditionalLossDb, AdditionalLossDescription,
                        AntennaId, HeightMeters, IsRotatable, HorizontalAngleDegrees,
                        ModulationId, ActivityFactor, OkaId
                    )
                    SELECT
                        Id, ProjectId, ConfigNumber, Name, PowerWatts,
                        RadioId, LinearName, LinearPowerWatts,
                        CableId, CableLengthMeters, AdditionalLossDb, AdditionalLossDescription,
                        AntennaId, HeightMeters, IsRotatable, HorizontalAngleDegrees,
                        ModulationId, ActivityFactor, OkaId
                    FROM Configurations
                """)

                # Drop old table and rename new one
                cursor.execute("DROP TABLE Configurations")
                cursor.execute("ALTER TABLE Configurations_new RENAME TO Configurations")

                # Recreate indexes
                cursor.execute("CREATE INDEX IF NOT EXISTS IX_Configurations_ProjectId ON Configurations(ProjectId)")
                cursor.execute("CREATE INDEX IF NOT EXISTS IX_Configurations_RadioId ON Configurations(RadioId)")
                cursor.execute("CREATE INDEX IF NOT EXISTS IX_Configurations_CableId ON Configurations(CableId)")
                cursor.execute("CREATE INDEX IF NOT EXISTS IX_Configurations_AntennaId ON Configurations(AntennaId)")
                cursor.execute("CREATE INDEX IF NOT EXISTS IX_Configurations_ModulationId ON Configurations(ModulationId)")
                cursor.execute("CREATE INDEX IF NOT EXISTS IX_Configurations_OkaId ON Configurations(OkaId)")
                cursor.execute("""
                    CREATE UNIQUE INDEX IF NOT EXISTS IX_Configurations_ProjectId_ConfigNumber
                    ON Configurations(ProjectId, ConfigNumber)
                """)

                changes_made = True
                print("Columns removed successfully!")

        conn.commit()

        if changes_made:
            print("\nMigration complete!")
        else:
            print("\nNo changes needed - database is up to date")

        return True

    except Exception as e:
        print(f"\nMigration failed: {e}")
        conn.rollback()
        print(f"Restoring from backup: {backup_path}")
        shutil.copy2(backup_path, db_path)
        return False

    finally:
        conn.close()


def main():
    script_dir = Path(__file__).parent
    project_root = script_dir.parent

    db_path = project_root / "src" / "NIS.Desktop" / "Data" / "nisdata.db"

    print("NIS Calculator Database Migration v0.5")
    print("=" * 45)
    print(f"Database: {db_path}")
    print()
    print("This migration removes OkaDistanceMeters and")
    print("OkaBuildingDampingDb from Configurations table.")
    print("(Values now come from OKA master data)")
    print()

    if db_path.exists():
        migrate_database(db_path)
    else:
        print("Database file not found!")

    print("\nDone!")


if __name__ == "__main__":
    main()
