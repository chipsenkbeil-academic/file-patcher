using System.Xml;
using System.Collections.Generic;

/// <summary>
/// This class provides wrapper functionality to read an XML file designed to
/// work with the patcher's XML file.
/// </summary>
namespace File_Patcher {
    public class PatcherXMLReader {
    
        private XmlDocument document;
        private XmlElement[] elements;

        private Dictionary<string, string> keywords;

        /// <summary>
        /// Creates a new instance of the XMLReader class.
        /// </summary>
        public PatcherXMLReader() {
            document = new XmlDocument();
            elements = new XmlElement[0]; // Empty array
            initKeywords();
        }

        /// <summary>
        /// Support Method: No Precondition.
        /// Initializes the replaceable keywords list.
        /// </summary>
        private void initKeywords() {
            keywords = new Dictionary<string, string>();

            // Used to insert the direct root to the patcher program directory
            keywords.Add(
                "$PATCH_ROOT", 
                System.IO.Path.GetDirectoryName(
                    System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName
                )
            );
        }

        /// <summary>
        /// Reads the XML from the specified file and stores it
        /// in the element array. The location can be a web address.
        /// </summary>
        /// <param name="filePath">The path to the XML file</param>
        /// <returns>The XmlElement array created from the XML file</returns>
        public XmlElement[] read(string filePath) {
            // Load the document and replace the keywords
            document.Load(filePath);
            foreach (string key in keywords.Keys) {
                document.InnerXml = document.InnerXml.Replace(key, keywords[key].ToString());
            }

            // Get all of the children elements
            List<XmlElement> elementList = new List<XmlElement>();
            foreach (XmlElement element in document.DocumentElement.ChildNodes) {
                elementList.Add(element);
            }

            // Copy the elements to the array
            elements = new XmlElement[elementList.Count];
            for (int i = 0; i < elementList.Count; ++i) {
                elements[i] = (XmlElement) elementList[i];
            }

            return elements;
        }

        /// <summary>
        /// Returns the last collection of elements read in by the reader.
        /// </summary>
        /// <returns>The XmlElement array</returns>
        public XmlElement[] getElements() {
            return elements;
        }

        /// <summary>
        /// Support Method: No Precondition.
        /// Replaces the old text in each attribute's value of the element.
        /// </summary>
        /// <param name="element">(Reference) The Element whose attributes will be altered</param>
        /// <param name="oldText">The old text to replace</param>
        /// <param name="newText">The new text to substitude</param>
        private void replaceAttributesValues(ref XmlElement element, string oldText, string newText) {
            for (int i = 0; i < element.Attributes.Count; ++i) {
                element.Attributes.Item(i).Value = element.Attributes.Item(i).Value.Replace(oldText, newText);
            }
        }
    }
}