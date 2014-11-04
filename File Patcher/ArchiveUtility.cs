using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.BZip2;
using System.IO;

/// <summary>
/// TODO:
/// 1. Add ZIP compression
/// 2. Add Tar compression
/// 3. Add Tar decompression
/// 4. Add GZip compression
/// 5. Add GZip decompression
/// 6. Add BZip2 compression
/// 7. Add BZip2 decompression
/// 
/// This class provides static methods to handle file archives.
/// </summary>
namespace File_Patcher {
    public class ArchiveUtility {

        // Used for the archiving/unarchiving to decide how many
        // bytes to write per loop
        private const int BYTES_PER_WRITE = 2048;

        /// <summary>
        /// Unzips a ZIP file to the specified location (creates location if
        /// it does not exist). If the new location is left blank or null, the
        /// contents of the ZIP file will be placed in the current directory.
        /// </summary>
        /// <param name="archivePath">The path to the archive file</param>
        /// <param name="newLocation">The location where the archive's contents will be stored</param>
        /// <param name="archivePassword">The password to use with the archive</param>
        /// <param name="deleteArchive">Whether or not to delete the archive after it is decompressed</param>
        public static void unzip(string archivePath, string newLocation, string archivePassword, bool deleteArchive) {
            // Check to see if we have valid archive (if not, exit method)
            if (archivePath == null || archivePath == string.Empty 
                || !File.Exists(archivePath)) { return; }

            // Create a new input stream to store the archive
            ZipInputStream ziStream = new ZipInputStream(File.OpenRead(archivePath));

            // Check if we are to use a password with this archive
            if (archivePassword != null && archivePassword != string.Empty) { 
                ziStream.Password = archivePassword;
            }

            // Check to see where the contents should be stored
            string currentDirectory = string.Empty;
            if (newLocation != null && newLocation != string.Empty) {
                currentDirectory = newLocation;
            } else {
                currentDirectory = Directory.GetCurrentDirectory();
            }

            // Add a backslash if necessary (GetCurrentDirectory never adds a backslash)
            if (!(currentDirectory.EndsWith("\\") || currentDirectory.EndsWith("/"))) {
                currentDirectory += "\\";
            }

            // Create the directory if it does not exist
            if (!Directory.Exists(currentDirectory)) {
                Directory.CreateDirectory(currentDirectory);
            }

            // Cycle through each entry in the archive
            ZipEntry currentEntry = null;
            string currentFile = string.Empty;
            while ((currentEntry = ziStream.GetNextEntry()) != null) {

                // Grab the current file name and extension
                currentFile = string.Empty;
                currentFile = Path.GetFileName(currentEntry.Name);
                if (currentFile == null || currentFile == string.Empty) { continue; }

                // Create the directory for this file if it doesn't already exist
                string filePath = currentDirectory + currentEntry.Name;
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath)) {
                    Directory.CreateDirectory(directoryPath);
                }

                // Write the file if it is valid
                if (currentFile != null && currentFile != string.Empty) {
                    // Open a file stream from the created file
                    FileStream fStream = File.Create(filePath);

                    // Loop through and write the individual bytes of the file
                    byte[] byteBuffer = new byte[BYTES_PER_WRITE];
                    int bytesToWrite = ziStream.Read(byteBuffer, 0, byteBuffer.Length);
                    while (bytesToWrite > 0) {
                        fStream.Write(byteBuffer, 0, bytesToWrite);
                        bytesToWrite = ziStream.Read(byteBuffer, 0, byteBuffer.Length);
                    }

                    // Close the file stream
                    fStream.Close();
                }
            }

            // Close the archive stream
            ziStream.Close();

            // If specified, delete the archive
            if (deleteArchive) { File.Delete(archivePath); }
        }
    }
}