using Engine.Core.Utils;

namespace Engine.Core.IO
{
    public static class CSVReader
    {
        #region Public Methods

        public static T[] ReadCsv<T>(string[] csvLines) where T : new()
        {
            CleanCSV(csvLines);

            string separator = csvLines[0].GetNewlineSeparator();
            // Get the properties of the specified type
            var typeProperties = typeof(T).GetProperties();

            // Create a list to store the objects
            var objects = new List<T>();

            // Loop through the lines
            for (int i = 0; i <= csvLines.Length - 1; i++)
            {
                if (System.String.IsNullOrWhiteSpace(csvLines[i])) continue;
                // Create a new object of the specified type
                var obj = new T();

                // Split the current line into an array of values
                string[] values = csvLines[i].Split(separator);

                // Set the values of the object's properties
                for (int j = 0; j <= values.Length - 1; j++)
                {
                    var property = typeProperties[j];

                    if (Type.GetTypeCode(property.PropertyType) == TypeCode.Double) values[j] = values[j].Replace(',', '.');

                    property.SetValue(obj, Convert.ChangeType(values[j], property.PropertyType));
                }

                // Add the object to the list
                objects.Add(obj);
            }

            // Return the array of objects
            return objects.ToArray();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Removes special characters and empty lines
        /// </summary>
        /// <param name="csvLines"></param>
        private static void CleanCSV(string[] csvLines)
        {
            for (int i = 0; i < csvLines.Length; i++)
            {
                csvLines[i] = csvLines[i].Trim(new char[] { '\uFEFF', '\u200B', ' ' });

                if (csvLines[i].EndsWith(";"))
                {
                    csvLines[i] = csvLines[i].TrimEnd(';');
                }
            }
        }

        #endregion Private Methods
    }
}