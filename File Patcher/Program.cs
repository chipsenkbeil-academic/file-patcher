using ICSharpCode.SharpZipLib.Zip;
using System.IO;

/// <summary>
/// This is the main executing class of the file patcher program.
/// </summary>
public class Program {
    
    private static string programPath =
        System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

    /// <summary>
    /// This is the main executing method of the file patcher program.
    /// </summary>
    /// <param name="args">The string array of command line arguments</param>
    /// <returns>The exit code for the program</returns>
    public static int Main(string[] args) {
        
        //FileDownloadManager m = new FileDownloadManager();
        //m.downloadFile("http://www.dualsolace.com/Downloads/POPS Installer.zip", "POPS Installer.zip");
        File_Patcher.MainWindow window = new File_Patcher.MainWindow();
        window.ShowDialog();
        
        return 0;
    }
}