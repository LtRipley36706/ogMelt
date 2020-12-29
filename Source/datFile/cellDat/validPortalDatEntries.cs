using System.Collections.Generic;
using System.IO;

namespace Melt
{
    public class validPortalDatEntries
    {
        static validPortalDatEntries instance = new validPortalDatEntries();

        List<uint> validStabs;
        List<uint> validEnvironments;
        Dictionary<ushort, ushort> textureIdMigrationTable;
        Dictionary<uint, Dictionary<ushort, ushort>> buildingPortalEnvIdMigrationTable;

        public static ushort translateTextureId(ushort id)
        {
            if(instance.textureIdMigrationTable.ContainsKey(id))
                return instance.textureIdMigrationTable[id];
            else
                return 0x032a; //placeholder texture
        }

        public static ushort translateBuildingPortalEnvId(uint modelId, ushort id)
        {
            Dictionary<ushort, ushort> entry;
            if (instance.buildingPortalEnvIdMigrationTable.TryGetValue(modelId, out entry))
            {
                ushort value;
                if (entry.TryGetValue(id, out value))
                    return value;
            }
            return id; //return untranslated
        }

        public static bool isValidStabEntry(uint id)
        {
            return instance.validStabs.Contains(id);
        }

        public static bool isValidEnvironmentEntry(uint id)
        {
            return instance.validEnvironments.Contains(id);
        }

        public validPortalDatEntries()
        {
            validStabs = new List<uint>();
            validEnvironments = new List<uint>();
            textureIdMigrationTable = new Dictionary<ushort, ushort>();
            buildingPortalEnvIdMigrationTable = new Dictionary<uint, Dictionary<ushort, ushort>>();

            StreamReader inputFile = new StreamReader(new FileStream("./input/validStabs.txt", FileMode.Open, FileAccess.Read));

            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine();
                uint value = uint.Parse(line, System.Globalization.NumberStyles.HexNumber);

                if (value != 0)
                    validStabs.Add(value);
            }

            inputFile = new StreamReader(new FileStream("./input/validEnvironments.txt", FileMode.Open, FileAccess.Read));

            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine();
                uint value = uint.Parse(line, System.Globalization.NumberStyles.HexNumber);

                if (value != 0)
                    validEnvironments.Add(value);
            }

            inputFile = new StreamReader(new FileStream("./input/textureIdMigrationTable.txt", FileMode.Open, FileAccess.Read));

            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine();
                string[] splitLine = line.Split(' ');

                if (splitLine.Length >= 2)
                {
                    ushort a = ushort.Parse(splitLine[0], System.Globalization.NumberStyles.HexNumber);
                    ushort b = ushort.Parse(splitLine[1], System.Globalization.NumberStyles.HexNumber);
                    textureIdMigrationTable.Add(a, b);
                }
            }

            inputFile = new StreamReader(new FileStream("./input/buildingPortalEnvironmentIdMigrationTable.txt", FileMode.Open, FileAccess.Read));

            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine();
                string[] splitLine = line.Split(' ');

                if (splitLine.Length >= 3)
                {
                    uint modelId = uint.Parse(splitLine[0], System.Globalization.NumberStyles.HexNumber);
                    ushort a = ushort.Parse(splitLine[1], System.Globalization.NumberStyles.HexNumber);
                    ushort b = ushort.Parse(splitLine[2], System.Globalization.NumberStyles.HexNumber);
                    Dictionary<ushort, ushort> entry;
                    if (!buildingPortalEnvIdMigrationTable.TryGetValue(modelId, out entry))
                    {
                        entry = new Dictionary<ushort, ushort>();
                        entry.Add(a, b);
                        buildingPortalEnvIdMigrationTable.Add(modelId, entry);
                    }
                    else
                        entry.Add(a, b);
                }
            }
        }
    }
}