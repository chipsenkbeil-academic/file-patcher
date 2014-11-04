namespace File_Patcher {
    public class PatchProcessor {

        // The version structure
        private struct Version {
            public int major;
            public int minor;
            public int revision;
        }

        // Contain the information about the version associated with the patcher
        private Version currentVersion;

        private FileDownloadManager fileManager = new FileDownloadManager();
        private LinearTaskManager taskManager = new LinearTaskManager();

        // The constants used for generic messages
        private const string DOWNLOAD_ERROR = 
            "One or more attributes for the download command are either incorrect or missing!";
        private const string DECOMPRESS_ERROR = 
            "One or more attributes for the decompress command are either incorrect or missing!";
        private const string MOVE_ERROR = 
            "One or more attributes for the move command are either incorrect or missing!";
        private const string DELETE_ERROR = 
            "The attribute for the delete command is either incorrect or missing!";
        private const string EXECUTION_ERROR = 
            "The attribute for the execution command is either incorrect or missing!";

        // The delegates used to interact with the UI
        public delegate void addControlsDelegate(params System.Windows.Forms.Control[] controls);
        public delegate void removeControlDelegate(System.Windows.Forms.Control control);
        public delegate void statusDelegate(string newText);

        // The delegate objects to be used
        private addControlsDelegate addControls;
        private removeControlDelegate removeControl;
        private statusDelegate status;

        // The delegates used for the task manager
        private delegate void downloadFileDelegate(string src, string dest);
        private delegate void unzipFileDelegate(string src, string dest, string pass, bool delete);
        private delegate void moveFileDelegate(string src, string dest);
        private delegate void deleteFileDelegate(string file);
        private delegate void executeFileDelegate(string file, bool wait);

        /// <summary>
        /// Creates a new instance of the PatchProcessor class.
        /// </summary>
        /// <param name="delAddControls">The delegate for adding controls</param>
        /// <param name="delRemoveControl">The delegate for removing a control</param>
        /// <param name="delStatus">The delegate for changing the status</param>
        public PatchProcessor(
            addControlsDelegate delAddControls, 
            removeControlDelegate delRemoveControl, 
            statusDelegate delStatus
        ) {
            addControls = delAddControls;
            removeControl = delRemoveControl;
            status = delStatus;

            // Create a new version and give it the default values
            currentVersion = new Version();
            setVersion(1, 0, 0);
        }

        /// <summary>
        /// Sets the version that this patch processor will use when
        /// reading XML files.
        /// </summary>
        /// <param name="major">The new major version</param>
        /// <param name="minor">The new minor version</param>
        /// <param name="revision">The new revision version</param>
        public void setVersion(int major, int minor, int revision) {
            currentVersion.major = major;
            currentVersion.minor = minor;
            currentVersion.revision = revision;
        }

        /// <summary>
        /// Sets the version that this patch processor will use when
        /// reading XML files.
        /// </summary>
        /// <param name="newVersion">The string version</param>
        /// <example>setVersion("1.2.1");</example>
        public void setVersion(string newVersion) {
            string[] sVersion = newVersion.Split('.');
            int[] iVersion = new int[sVersion.Length];
            for (int i = 0; i < sVersion.Length; ++i) {
                iVersion[i] = int.Parse(sVersion[i]);
            }
            setVersion(iVersion[0], iVersion[1], iVersion[2]);
        }

        /// <summary>
        /// Returns the version associated with this patch processor as a string.
        /// </summary>
        /// <returns>The string version</returns>
        public string getVersion() {
            return (
                currentVersion.major + "." + 
                currentVersion.minor + "." + 
                currentVersion.revision
            );
        }

        /// <summary>
        /// Reads an XML file and processes the information.
        /// </summary>
        /// <param name="urlPath">The path to the XML file (can be a URL)</param>
        public void processXMLFile(string urlPath) {
            PatcherXMLReader reader = new PatcherXMLReader();
            reader.read(urlPath);
            System.Xml.XmlElement element = null;
            string elementName = string.Empty;

            // Clear the task list
            taskManager.clear();

            // Loop through and add all of the tasks
            for (int i = 0; i < reader.getElements().Length; ++i) {
                element = reader.getElements()[i];
                elementName = element.Name.Trim().ToLower();

                // Check the version
                if (elementName.Equals("version")) {
                    if (element.HasAttribute("equal_to") && element.HasAttribute("update_to")) {
                        if (element.GetAttribute("equal_to").Equals(getVersion())) {

                            // Add the tasks
                            foreach (System.Xml.XmlElement taskElement in element.ChildNodes) {
                                addTaskFromElement(ref taskManager, taskElement);
                            }

                            // Execute the tasks
                            taskManager.executeList();

                            // Update the version
                            setVersion(element.GetAttribute("update_to"));

                            // Update the status
                            status(
                                "Update from " + element.GetAttribute("equal_to") +
                                " to " + element.GetAttribute("update_to") +
                                " succeeded!");
                        }
                    } else {
                        System.Windows.Forms.MessageBox.Show("Version command is missing one or more attributes!");
                    }
                }
            }

            // Update the status
            status("Finished update process!");
        }

        private void addTaskFromElement(ref LinearTaskManager manager, System.Xml.XmlElement element) {
            // Supports multiple simultaneous downloads
            if (element.Name.ToLower().Equals("download")) {
                // Check to see if the necessary attributes exist
                if (!element.HasAttribute("src") || !element.HasAttribute("dest")) {
                    // Display a message giving details about the error
                    System.Windows.Forms.MessageBox.Show(DOWNLOAD_ERROR);

                    // Exit the entire method
                    return;
                }

                // Add a delegate for the download process
                taskManager.addTask(
                    new downloadFileDelegate(downloadFile),
                    element.GetAttribute("src"),
                    element.GetAttribute("dest")
                );

            // Only one decompression (nothing else occurs)
            } else if (element.Name.ToLower().Equals("decompress")) {
                // Check to see if the necessary attributes exist
                if (!element.HasAttribute("src") || !element.HasAttribute("dest")) {
                    // Display a message giving details about the error
                    System.Windows.Forms.MessageBox.Show(DECOMPRESS_ERROR);

                    // Exit the entire method
                    return;
                }

                // Check for a password attribute
                string password = string.Empty;
                if (element.HasAttribute("pass")) {
                    password = element.GetAttribute("pass");
                }

                // Check for a deletion attribute
                bool delete = false;
                if (element.HasAttribute("delete")) {
                    System.Boolean.TryParse(element.GetAttribute("delete"), out delete);
                }

                // Add a delegate for the decompression process
                taskManager.addTask(
                    new unzipFileDelegate(unzipFile), 
                    element.GetAttribute("src"),
                    element.GetAttribute("dest"),
                    password, delete
                );

            // Only one move (nothing else occurs)
            } else if (element.Name.ToLower().Equals("move")) {
                // Check to see if the necessary attributes exist
                if (!element.HasAttribute("src") || !element.HasAttribute("dest")) {
                    // Display a message giving details about the error
                    System.Windows.Forms.MessageBox.Show(MOVE_ERROR);

                    // Exit the entire method
                    return;
                }

                // Add a delegate for the move process
                taskManager.addTask(
                    new moveFileDelegate(moveFile),
                    element.GetAttribute("src"),
                    element.GetAttribute("dest")
                );

            // Only one delete (nothing else occurs)
            } else if (element.Name.ToLower().Equals("delete")) {
                // Check to see if the necessary attributes exist
                if (!element.HasAttribute("src")) {
                    // Display a message giving details about the error
                    System.Windows.Forms.MessageBox.Show(DELETE_ERROR);

                    // Exit the entire method
                    return;
                }

                // Add a delegate for the delete process
                taskManager.addTask(
                    new deleteFileDelegate(deleteFile),
                    element.GetAttribute("src")
                );

            // Only one execution (nothing else occurs)
            } else if (element.Name.ToLower().Equals("execute")) {
                // Check to see if the necessary attributes exist
                if (!element.HasAttribute("src")) {
                    // Display a message giving details about the error
                    System.Windows.Forms.MessageBox.Show(EXECUTION_ERROR);

                    // Exit the entire method
                    return;
                }

                // Check if the optional attribute is included
                bool wait = true;
                if (element.HasAttribute("wait")) {
                    System.Boolean.TryParse(element.GetAttribute("wait"), out wait);
                }

                // Add a delegate for the move process
                taskManager.addTask(
                    new executeFileDelegate(executeFile),
                    element.GetAttribute("src"), wait
                );
            }
        }

        /// <summary>
        /// Unzips the file specified.
        /// </summary>
        /// <param name="source">The file to unzip</param>
        /// <param name="destination">The destination for the file's contents</param>
        private void unzipFile(
            string source, string destination, 
            string password, bool delete
        ) {
            // Update status to mention progress
            status(
                "Unzipping " + System.IO.Path.GetFileName(source)
            );

            // Unzip the zipped file
            ArchiveUtility.unzip(
                source, 
                destination, 
                password, delete
            );

            // Update status to mention being finished
            status("File unzipped!");
        }

        /// <summary>
        /// Download a file and update the progress information.
        /// <strike>This is threaded so it will not block the calling thread.</strike>
        /// </summary>
        private void downloadFile(string urlPath, string localPath) {
            // Update status to mention this download
            status("Downloading: " + System.IO.Path.GetFileName(urlPath)); 

            // Create a new progress bar
            System.Windows.Forms.ProgressBar progressBar = 
                new System.Windows.Forms.ProgressBar();
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 0;

            // Create a new label
            System.Windows.Forms.Label progressLabel = 
                new System.Windows.Forms.Label();
            progressLabel.Text = "Loading...";

            // Add the two controls
            addControls(progressBar, progressLabel);

            // Cycle through and update everything
            int oldProgress = 0;
            FileDownloadManager.CustomWebClient client = 
                fileManager.downloadFile(urlPath, localPath);
            while (client.isDownloadingFile()) {
                if (oldProgress < client.getDownloadProgress()) {
                    oldProgress = client.getDownloadProgress();
                    progressBar.Value = oldProgress;
                    progressLabel.Text = "(" + oldProgress + "%) ";
                }

                // Allow the application to process the information
                // NOTE: Was necessary for the download progress event to occur
                System.Windows.Forms.Application.DoEvents();
            }

            // Update with full percent
            progressBar.Value = 100;
            progressLabel.Text = System.IO.Path.GetFileName(urlPath);

            // Update status to mention this download was completed
            status("Download finished!");
        }

        /// <summary>
        /// Moves the specified file to a new location.
        /// </summary>
        /// <param name="source">The old location</param>
        /// <param name="destination">The new location</param>
        private void moveFile(string source, string destination) {
            if (System.IO.File.Exists(source)) {
                // Update the status
                status("Moving " + System.IO.Path.GetFileName(source) + "...");

                // Create the directory for the file if it doesn't exist
                if (!System.IO.Directory.Exists(
                    System.IO.Path.GetDirectoryName(destination)
                )) {
                    System.IO.Directory.CreateDirectory(
                        System.IO.Path.GetDirectoryName(destination)
                    );
                }

                // Move the file
                System.IO.File.Move(source, destination);
            }
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="fileToDelete">The file to delete</param>
        private void deleteFile(string fileToDelete) {
            // Update the status
            status("Deleting " + System.IO.Path.GetFileName(fileToDelete) + "...");

            // Delete the file
            System.IO.File.Delete(fileToDelete);
        }

        /// <summary>
        /// Executes the specified file.
        /// </summary>
        /// <param name="fileToExecute">The file to execute</param>
        /// <param name="waitToFinish">Whether or not to wait for the executing file to terminate</param>
        private void executeFile(string fileToExecute, bool waitToFinish) {
            // Update the status
            status("Executing " + System.IO.Path.GetFileName(fileToExecute) + "...");

            // Create the new process
            System.Diagnostics.Process process = new System.Diagnostics.Process();

            // Set the executable
            process.StartInfo.FileName = fileToExecute;

            // Start the process
            process.Start();

            // Wait for termination if asked
            if (waitToFinish) {
                while (!process.HasExited) {
                    // Prevents this application from consuming too much memory
                    System.Threading.Thread.Sleep(1);

                    // Prevents this application from freezing
                    System.Windows.Forms.Application.DoEvents();
                }
            }

            // Release the process resources
            process.Close();
        }
    }
}