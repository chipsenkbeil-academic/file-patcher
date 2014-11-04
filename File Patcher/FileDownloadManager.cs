using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

/// <summary>
/// This class represents the class that manages file downloads.
/// </summary>
namespace File_Patcher {
    public class FileDownloadManager {
        private List<CustomWebClient> webClients;

        /// <summary>
        /// Creates a new instance of the FileDownloadManager class.
        /// </summary>
        public FileDownloadManager() {
            webClients = new List<CustomWebClient>();
        }

        /// <summary>
        /// Downloads a file from the Internet.
        /// </summary>
        /// <param name="key">The key to look up the file progress</param>
        /// <param name="source">The URL where the file is located</param>
        /// <param name="destination">The location to store the file</param>
        /// <returns>The CustomWebClient responsible for downloading the specified file</returns>
        public CustomWebClient downloadFile(string source, string destination) {
            CustomWebClient webClient = new CustomWebClient(this, webClients.Count);
            webClient.DownloadFile(source, destination);
            webClients.Add(webClient);
            return webClient;
        }

        /// <summary>
        /// Returns the total number of running web clients.
        /// </summary>
        /// <returns>The integer number</returns>
        public int getWebClientCount() {
            return webClients.Count;
        }

        /// <summary>
        /// Returns whether or not this manager has a web client
        /// that is downloading a file.
        /// </summary>
        /// <returns>A true/false value</returns>
        public bool hasDownloadProgressing() {
            foreach (CustomWebClient client in webClients) {
                if (client.isDownloadingFile()) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a read-only list of the web clients that are being used.
        /// </summary>
        /// <returns>The Read-Only List of CustomWebClient objects</returns>
        public System.Collections.ObjectModel.ReadOnlyCollection<CustomWebClient> getWebClients() {
            return webClients.AsReadOnly();
        }

        /// <summary>
        /// This class represents a custom web client that downloads data and can display the progress.
        /// </summary>
        public class CustomWebClient : System.Net.WebClient {
            private FileDownloadManager parent;
            private int downloadProgress;
            private bool downloadInProgress;
            private int index;

            /// <summary>
            /// Creates a new instance of the CustomWebClient class.
            /// </summary>
            /// <param name="parent">The parent of this client</param>
            /// <param name="index">Used to keep track of this client</param>
            public CustomWebClient(FileDownloadManager parent, int index) {
                this.parent = parent;
                this.index = index;
                downloadProgress = 0;
                downloadInProgress = false;
                this.DownloadProgressChanged += 
                    new System.Net.DownloadProgressChangedEventHandler(Event_DownloadProgressChanged);
                this.DownloadFileCompleted += 
                    new System.ComponentModel.AsyncCompletedEventHandler(Event_DownloadFileCompleted);
            }

            /// <summary>
            /// Downloads a file located at the specified location and saves it to the new location.
            /// This is done without blocking the calling block so it can continue to function.
            /// </summary>
            /// <param name="source">The URL where the file to download is located</param>
            /// <param name="destination">The location to store the file locally</param>
            new public void DownloadFile(string source, string destination) {
                this.DownloadFileAsync(new Uri(source), destination);
                downloadInProgress = true;
            }

            /// <summary>
            /// Executed when new data is received by the web client.
            /// </summary>
            /// <param name="sender">The object sending the data</param>
            /// <param name="args">The information regarding this event</param>
            public void Event_DownloadProgressChanged(
                object sender, 
                System.Net.DownloadProgressChangedEventArgs args
            ) {
                downloadProgress = args.ProgressPercentage;
            }

            /// <summary>
            /// Executed when the file has been successfully downloaded.
            /// </summary>
            /// <param name="sender">The object sending the data</param>
            /// <param name="args">The information regarding this event</param>
            public void Event_DownloadFileCompleted(
                object sender, 
                System.ComponentModel.AsyncCompletedEventArgs args
            ) {
                // Remove this CustomWebClient from the list
                CustomWebClient clientToRemove = null;
                foreach (CustomWebClient client in parent.webClients) {
                    if (client.index == this.index) {
                        clientToRemove = client;
                    }
                }
                if (clientToRemove != null) parent.webClients.Remove(clientToRemove);

                downloadInProgress = false;
            }

            /// <summary>
            /// Returns the current download progress.
            /// </summary>
            /// <returns></returns>
            public int getDownloadProgress() {
                return downloadProgress;
            }

            /// <summary>
            /// Returns whether or not a file is being downloaded.
            /// </summary>
            /// <returns>The true/false value</returns>
            public bool isDownloadingFile() {
                return downloadInProgress;
            }
        }
    }
}