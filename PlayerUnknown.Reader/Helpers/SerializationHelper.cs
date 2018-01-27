namespace PlayerUnknown.Reader.Helpers
{
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    /// <summary>
    /// Static helper class providing tools for serializing/deserializing objects.
    /// </summary>
    public static class SerializationHelper
    {
        /// <summary>
        /// Serializes the specified object and writes the XML document to the specified path.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="Obj">The object to serialize.</param>
        /// <param name="Path">The path where the file is saved.</param>
        /// <param name="Encoding">The encoding to generate.</param>
        public static void ExportToXmlFile<T>(T Obj, string Path, Encoding Encoding)
        {
            // Create the stream to write into the specified file
            using (var file = new StreamWriter(Path, false, Encoding))
            {
                // Write the content by calling the method to serialize the object
                file.Write(SerializationHelper.ExportToXmlString(Obj));
            }
        }

        /// <summary>
        /// Serializes the specified object and writes the XML document to the specified path using <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="Obj">The object to serialize.</param>
        /// <param name="Path">The path where the file is saved.</param>
        public static void ExportToXmlFile<T>(T Obj, string Path)
        {
            SerializationHelper.ExportToXmlFile(Obj, Path, Encoding.UTF8);
        }

        /// <summary>
        /// Serializes the specified object and returns the XML document.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="Obj">The object to serialize.</param>
        /// <returns>XML document of the serialized object.</returns>
        public static string ExportToXmlString<T>(T Obj)
        {
            // Initialize the required objects for serialization
            var serializer = new XmlSerializer(typeof(T));
            using (var StringWriter = new StringWriter())
            {
                // Serialize the object
                serializer.Serialize(StringWriter, Obj);

                // Return the serialized object
                return StringWriter.ToString();
            }
        }

        /// <summary>
        /// Deserializes the specified file into an object.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="Path">The path where the object is read.</param>
        /// <param name="Encoding">The character encoding to use. </param>
        /// <returns>The deserialized object.</returns>
        public static T ImportFromXmlFile<T>(string Path, Encoding Encoding)
        {
            // Create the stream to read the specified file
            using (var file = new StreamReader(Path, Encoding))
            {
                // Read the content of the file and call the method to deserialize the object
                return SerializationHelper.ImportFromXmlString<T>(file.ReadToEnd());
            }
        }

        /// <summary>
        /// Deserializes the specified file into an object using <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="Path">The path where the object is read.</param>
        /// <returns>The deserialized object.</returns>
        public static T ImportFromXmlFile<T>(string Path)
        {
            return SerializationHelper.ImportFromXmlFile<T>(Path, Encoding.UTF8);
        }

        /// <summary>
        /// Deserializes the XML document to the specified object.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="SerializedObj">The string representing the serialized object.</param>
        /// <returns>The deserialized object.</returns>
        public static T ImportFromXmlString<T>(string SerializedObj)
        {
            // Initialize the required objects for deserialization
            var serializer = new XmlSerializer(typeof(T));
            using (var StringWriter = new StringReader(SerializedObj))
            {
                // Return the serialized object
                return (T)serializer.Deserialize(StringWriter);
            }
        }
    }
}