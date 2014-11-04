using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace File_Patcher
{
    public partial class MainWindow : Form
    {
        private PatchProcessor processor;
        private const string PATCH_FILE = "Patch_Info.txt";
        private string urlToPatch;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            string directory = 
                System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\";

            // Set up the processor to deal with XML patch files
            processor = new PatchProcessor(
                new PatchProcessor.addControlsDelegate(addControlsToList),
                new PatchProcessor.removeControlDelegate(removeControlFromList),
                new PatchProcessor.statusDelegate(setStatus)
            );

            // Create a version file if it does not already exist
            if (!System.IO.File.Exists(directory + PATCH_FILE)) {
                // Create the file
                createPatchFile(
                    directory + PATCH_FILE, 
                    "1.0.0", 
                    "http://www.dualsolace.com/Downloads/patch.xml"
                );

                processor.setVersion(1, 0, 0);
                urlToPatch = "http://www.dualsolace.com/Downloads/patch.xml";
            } else {
                PatcherXMLReader reader = new PatcherXMLReader();
                reader.read(PATCH_FILE);
                foreach (System.Xml.XmlElement element in reader.getElements()) {
                    if (element.Name.ToLower().Equals("version")) {
                        string[] sVersion = element.InnerText.Trim().Split('.');
                        int[] iVersion = new int[sVersion.Length];
                        for (int i = 0; i < sVersion.Length; ++i) {
                            iVersion[i] = Int32.Parse(sVersion[i]);
                        }
                        processor.setVersion(iVersion[0], iVersion[1], iVersion[2]);
                    } else if (element.Name.ToLower().Equals("patch_url")) {
                        urlToPatch = element.InnerText.Trim();
                    }
                }
            }

            // Set version to processor version
            versionLabel.Text = processor.getVersion();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            // Clear the progress list
            progressLayout.Controls.Clear();

            // Parse the XML file
            processor.processXMLFile(urlToPatch);

            // Update the version that is visible
            versionLabel.Text = processor.getVersion();

            // Update the label in the text file
            string directory = 
                System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\";
            createPatchFile(directory + PATCH_FILE, processor.getVersion(), urlToPatch);
        }

        /// <summary>
        /// Used to allow controls to be added from a different thread.
        /// </summary>
        /// <param name="control">The control to add</param>
        private void addControlsToList(params Control[] controls) {
            progressLayout.Controls.AddRange(controls);
        }

        /// <summary>
        /// Used to allow controls to be removed from a different thread.
        /// </summary>
        /// <param name="control">The control to remove</param>
        private void removeControlFromList(Control control) {
            progressLayout.Controls.Remove(control);
        }

        /// <summary>
        /// Update the status on this form.
        /// </summary>
        /// <param name="text"></param>
        public void setStatus(string text) {
            statusLabel.Text = text;
        }

        /// <summary>
        /// Creates (or overwrites) a patch file with the specified information.
        /// </summary>
        /// <param name="file">The patch file</param>
        /// <param name="version">The version information</param>
        /// <param name="url">The URL for the patch</param>
        private void createPatchFile(string file, string version, string url) {
            System.IO.TextWriter writer = 
                new System.IO.StreamWriter(file);
            writer.WriteLine("<patch_info>");
            writer.WriteLine(" <version>" + version + "</version>");
            writer.WriteLine(" <patch_url>" + url + "</patch_url>");
            writer.WriteLine("</patch_info>");
            writer.Close();
        }
    }
}
