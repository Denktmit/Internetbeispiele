using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace FtpDataProvider
{
    /// <summary>
    /// Ermöglicht den Daten-Zugriff auf FTP-Server
    /// </summary>
    public class FtpProvider
    {
        #region C'tor

        /// <summary>
        /// Erstellt eine neue <see cref="FtpProvider"/> Instanz
        /// </summary>
        public FtpProvider()
        {
        }

        /// <summary>
        /// Erstellt eine neue <see cref="FtpProvider"/> Instanz
        /// </summary>
        /// <param name="serverAdress">Adresse des Servers</param>
        /// <param name="login">Login</param>
        /// <param name="password">Passwort</param>
        public FtpProvider(string serverAdress, string login, string password)
        {
            this.ServerAdress = serverAdress;
            this.Login = login;
            this.Password = password;
            this.PassiveMode = true;
            this.UseBinary = true;
            this.Proxy = null;
            this.KeepAlive = true;
            this.ConnectionGroupName = "MyFtpGroup";
            this.ConnectionLimit = 9;
        }

        /// <summary>
        /// Erstellt eine neue <see cref="FtpProvider"/> Instanz
        /// </summary>
        /// <param name="serverAdress"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="passiveMode"></param>
        /// <param name="useBinary"></param>
        /// <param name="proxy"></param>
        /// <param name="keepAlive"></param>
        /// <param name="connectionGroupName"></param>
        /// <param name="connectionLimit"></param>
        public FtpProvider(string serverAdress, string login, string password, bool passiveMode, bool useBinary, IWebProxy proxy, bool keepAlive, string connectionGroupName, int connectionLimit)
        {
            this.ServerAdress = serverAdress;
            this.Login = login;
            this.Password = password;
            this.PassiveMode = passiveMode;
            this.UseBinary = useBinary;
            this.Proxy = proxy;
            this.KeepAlive = keepAlive;
            this.ConnectionGroupName = connectionGroupName;
            this.ConnectionLimit = connectionLimit;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Die Adresse des FTP Servers
        /// </summary>
        public string ServerAdress { get; set; }

        /// <summary>
        /// Der Login des FTP Servers
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Das Passwort des FTP Servers
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Das Verhalten der Datenübertragung
        /// </summary>
        public bool PassiveMode { get; set; }

        /// <summary>
        /// Der Datentyp einer Datenübertragung
        /// </summary>
        public bool UseBinary { get; set; }

        /// <summary>
        /// Gibt an, ob die Steuerungsverbindung am Ende einer Datenübertragung geschlossen werden soll
        /// </summary>
        public bool KeepAlive { get; set; }

        /// <summary>
        /// Der Name der Verbindungsgruppe
        /// </summary>
        public string ConnectionGroupName { get; set; }

        /// <summary>
        /// Die maximale Anzahl von Verbindungen
        /// </summary>
        public int ConnectionLimit { get; set; }

        /// <summary>
        /// Der für die Kommunikation mit dem FTP-Server zu verwendene Proxy
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>
        /// Das Paket, welches gerade hoch-/ runtergeladen wird
        /// </summary>
        public object TransferPackage { get; set; }

        /// <summary>
        /// Version des Transfer-Pakets
        /// </summary>
        public int Version { get; set; }

        #endregion

        #region Events

        public event FtpProgressEventHandler FtpProgressChanged;

        #endregion

        #region Public Methods

        /// <summary>
        /// Läd eine Datei auf den FTP-Server
        /// </summary>
        /// <param name="sourceFile">Der absolute Pfad der hochzuladenen Datei auf dem lokalen Sytem</param>
        /// <param name="destinationFolder">Der absolute Ziel-Pfad auf dem FTP-Server</param>
        public void UploadFile(string sourceFile, string destinationFolder)
        {
            UploadFile(sourceFile, destinationFolder, null, string.Empty);
        }

        /// <summary>
        /// Läd eine Datei auf den FTP-Server
        /// </summary>
        /// <param name="sourceFile">Der absolute Pfad der hochzuladenen Datei auf dem lokalen Sytem</param>
        /// <param name="destinationFolder">Der absolute Ziel-Pfad auf dem FTP-Server</param>
        /// <param name="newFileName">Der neue Dateiname auf dem FTP-Server</param>
        public void UploadFile(string sourceFile, string destinationFolder, string newFileName)
        {
            UploadFile(sourceFile, destinationFolder, null, newFileName);
        }

        /// <summary>
        /// Läd eine Datei auf den FTP-Server
        /// </summary>
        /// <param name="sourceFile">Der absolute Pfad der hochzuladenen Datei auf dem lokalen Sytem</param>
        /// <param name="destinationFolder">Der absolute Ziel-Pfad auf dem FTP-Server</param>
        /// <param name="token">Das CancellationToken, welches signalisiert dass der asynchrone Vorgang abgebrochen werden soll</param>
        public void UploadFile(string sourceFile, string destinationFolder, CancellationTokenSource token)
        {
            UploadFile(sourceFile, destinationFolder, token, string.Empty);
        }

        /// <summary>
        /// Läd eine Datei auf den FTP-Server
        /// </summary>
        /// <param name="sourceFile">Der absolute Pfad der hochzuladenen Datei auf dem lokalen Sytem</param>
        /// <param name="destinationFolder">Der absolute Ziel-Pfad auf dem FTP-Server</param>
        /// <param name="token">Das CancellationToken, welches signalisiert dass der asynchrone Vorgang abgebrochen werden soll</param>
        /// <param name="newFileName">Der neue Dateiname auf dem FTP-Server</param>
        public void UploadFile(string sourceFile, string destinationFolder, CancellationTokenSource token, string newFileName)
        {
            FileInfo srcFileInfo = new FileInfo(sourceFile);

            string targetFile = string.IsNullOrEmpty(newFileName) ? srcFileInfo.Name : newFileName;

            FtpWebRequest request = GetFtpRequest(destinationFolder + "/" + targetFile, WebRequestMethods.Ftp.UploadFile);

            Stream destStream = request.GetRequestStream();
            FileStream srcStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);

            int length = 24576;

            if (srcFileInfo.Length < length)
                length = Convert.ToInt32(srcFileInfo.Length);

            byte[] streamBuffer = new byte[length];

            const int offset = 0;
            int remainSec = 0;
            int remain = streamBuffer.Length;
            long iMax = srcFileInfo.Length;
            long bytesReceived = 0;
            double kbPSec = 0;

            DateTime start = DateTime.Now;

            try
            {
                while (srcStream.Read(streamBuffer, offset, remain) > 0)
                {
                    if (token != null && token.IsCancellationRequested)
                        break;

                    // Bufferinhalt in die Zieldatei schreiben
                    destStream.Write(streamBuffer, offset, remain);

                    // Prozentualen Fortschritt berechnen
                    double percent = 1.0 * (bytesReceived * 100) / iMax;

                    // Datendurchsatz berechnen (Empfangene Bytes / verstrichene Zeit)
                    bytesReceived += streamBuffer.Length;
                    int sec = (int)Math.Round(DateTime.Now.Subtract(start).TotalMilliseconds, MidpointRounding.AwayFromZero);

                    if (sec > 0)
                        kbPSec = 1.0 * bytesReceived / sec;

                    // Restzeit des Kopiervorgangs berechnen (verbleibende Bytes / Datendurchsatz)
                    if (kbPSec > 0)
                    {
                        remainSec = (int)Math.Round(((((iMax - bytesReceived) / kbPSec) / 1000)), MidpointRounding.AwayFromZero);
                    }
                    TimeSpan remaning = new TimeSpan(0, 0, remainSec);

                    // Uploadevent feuern
                    OnFtpProgressChanged(new FtpProgressEventArgs(bytesReceived, percent, kbPSec, remaning, this.TransferPackage));

                    // StreamBuffer ggf. anpassen, um keine Nullen in die Datei zu schreiben
                    if ((iMax - bytesReceived) >= 24576) continue;

                    streamBuffer = new byte[iMax - bytesReceived];
                    remain = streamBuffer.Length;
                }
            }
            finally
            {
                destStream.Close();
                srcStream.Close();
            }

            if (token != null && token.IsCancellationRequested)
                DeleteFile(destinationFolder + "/" + targetFile);
        }

        /// <summary>
        /// Läd eine Datei vom FTP herunter
        /// </summary>
        /// <param name="sourceFolder">Der Quellpfad auf dem FTP-Server</param>
        /// <param name="sourceFile">Die Quell-Datei die heruntergeladen werden soll</param>
        /// <param name="destination">Der Zielpfad auf dem lokalen System</param>
        public void DownloadFile(string sourceFolder, string sourceFile, string destination)
        {
            DownloadFile(sourceFolder, sourceFile, destination, string.Empty, null);
        }

        /// <summary>
        /// Läd eine Datei vom FTP herunter
        /// </summary>
        /// <param name="sourceFolder">Der Quellpfad auf dem FTP-Server</param>
        /// <param name="sourceFile">Die Quell-Datei die heruntergeladen werden soll</param>
        /// <param name="destination">Der Zielpfad auf dem lokalen System</param>
        /// <param name="newFileName">Der neue Dateiname auf dem lokalen System</param>
        public void DownloadFile(string sourceFolder, string sourceFile, string destination, string newFileName)
        {
            DownloadFile(sourceFolder, sourceFile, destination, newFileName, null);
        }

        /// <summary>
        /// Läd eine Datei vom FTP herunter
        /// </summary>
        /// <param name="sourceFolder">Der Quellpfad auf dem FTP-Server</param>
        /// <param name="sourceFile">Die Quell-Datei die heruntergeladen werden soll</param>
        /// <param name="destination">Der Zielpfad auf dem lokalen System</param>
        /// <param name="newFileName">Der neue Dateiname auf dem lokalen System</param>
        /// <param name="token">Das CancellationToken, welches signalisiert dass der asynchrone Vorgang abgebrochen werden soll</param>
        public void DownloadFile(string sourceFolder, string sourceFile, string destination, string newFileName, CancellationTokenSource token)
        {
            long fileSize = GetFileSize(sourceFolder + "/" + sourceFile);

            string targetFile = string.IsNullOrEmpty(newFileName) ? sourceFile : newFileName;

            FtpWebRequest request = GetFtpRequest(sourceFolder + "/" + sourceFile, WebRequestMethods.Ftp.DownloadFile);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream srcStream = response.GetResponseStream();
            FileStream destStream = new FileStream(Path.Combine(destination, targetFile), FileMode.Create, FileAccess.Write);

            int length = 2048;

            if (fileSize < length)
                length = Convert.ToInt32(fileSize);

            byte[] streamBuffer = new byte[length];

            const int offset = 0;
            int remainSec = 0;
            long iMax = fileSize;
            long bytesReceived = 0;
            double kbPSec = 0;
            int bytesRead = srcStream.Read(streamBuffer, offset, length);

            DateTime start = DateTime.Now;

            try
            {
                while (bytesRead > 0)
                {
                    if (token != null && token.IsCancellationRequested)
                    {
                        request.Abort();
                        break;
                    }

                    // Bufferinhalt in die Zieldatei schreiben
                    destStream.Write(streamBuffer, offset, bytesRead);

                    // Prozentualen Fortschritt berechnen
                    double percent = 1.0 * (bytesReceived * 100) / iMax;

                    // Datendurchsatz berechnen (Empfangene Bytes / verstrichene Zeit)
                    bytesReceived += bytesRead;
                    int sec = (int)Math.Round(DateTime.Now.Subtract(start).TotalMilliseconds, MidpointRounding.AwayFromZero);

                    if (sec > 0)
                        kbPSec = 1.0 * bytesReceived / sec;

                    // Restzeit des Kopiervorgangs berechnen (verbleibende Bytes / Datendurchsatz)
                    if (kbPSec > 0)
                    {
                        remainSec = (int)Math.Round(((((iMax - bytesReceived) / kbPSec) / 1000)), MidpointRounding.AwayFromZero);
                    }
                    TimeSpan remaning = new TimeSpan(0, 0, remainSec);

                    // Downloadevent feuern
                    OnFtpProgressChanged(new FtpProgressEventArgs(bytesReceived, percent, kbPSec, remaning, this.TransferPackage));

                    bytesRead = srcStream.Read(streamBuffer, offset, length);
                }
            }
            finally
            {
                if (destStream != null)
                    destStream.Close();

                if (srcStream != null)
                    srcStream.Close();

                if (response != null)
                    response.Close();

                if (token != null && token.IsCancellationRequested)
                    File.Delete(Path.Combine(destination, sourceFile));
            }
        }


        /// <summary>
        /// Liefert eine Liste von Ordnern und Dateien zurück, die sich in einem bestimmten Verzeichnis auf dem Server befinden
        /// </summary>
        /// <param name="folder">Absoluter Pfad dessen Inhalt ausgegeben werden soll</param>
        /// <returns>Eine Liste von Ordnern und Dateien</returns>
        public List<FtpContent> GetDirectoryDetailList(string folder)
        {
            // Anmerkung: Diese Methode kann unter Umständen auf einigen FTP-Servern nicht das gewünschte Resultat liefern.
            // Dies liegt daran, dass der "LIST" Befehl auf einigen Servern zu unterschiedliche Antworten führt.
            List<string> folderContent = GetDirectoryList(folder);

            FtpWebRequest request = GetFtpRequest(folder, WebRequestMethods.Ftp.ListDirectoryDetails);

            WebResponse webResponse = request.GetResponse();

            List<FtpContent> content = new List<FtpContent>();

            StreamReader streamReader = new StreamReader(webResponse.GetResponseStream());

            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();

                if (line.StartsWith("d"))
                {
                    foreach (string entry in folderContent)
                    {
                        if (line.EndsWith(entry))
                        {
                            FtpContent c = new FtpContent(entry, folder, true);
                            content.Add(c);
                            break;
                        }
                    }
                }
                else if (!line.EndsWith("."))
                {
                    foreach (string entry in folderContent)
                    {
                        if (line.EndsWith(entry))
                        {
                            FtpContent c = new FtpContent(entry, folder, false);
                            content.Add(c);
                            break;
                        }
                    }
                }
            }

            if (!folderContent.Contains(".."))
                content.Add(new FtpContent("..", folder, true));

            streamReader.Close();

            webResponse.Close();

            return content;
        }

        /// <summary>
        /// Liefert eine Liste des Verzeichnis Inhalts zurück, zu einem
        /// übergebenen Pfad
        /// </summary>
        /// <param name="folder">Absoluter Pfad dessen Inhalt ausgegeben werden soll</param>
        /// <returns>Eine Liste des Verzeichnis Inhalts</returns>
        public List<string> GetDirectoryList(string folder)
        {
            FtpWebRequest request = GetFtpRequest(folder, WebRequestMethods.Ftp.ListDirectory);

            WebResponse webResponse = request.GetResponse();

            List<string> files = new List<string>();

            StreamReader streamReader = new StreamReader(webResponse.GetResponseStream());

            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                files.Add(Path.GetFileName(line));
            }

            streamReader.Close();

            webResponse.Close();

            return files;
        }

        /// <summary>
        /// Emittelt die Größe einer Datei in Bytes
        /// </summary>
        /// <param name="file">Der vollständige Pfad zur Datei</param>
        /// <returns>Die Größe der Datei in Bytes</returns>
        public long GetFileSize(string file)
        {
            FtpWebRequest request = GetFtpRequest(file, WebRequestMethods.Ftp.GetFileSize);

            WebResponse response = request.GetResponse();

            try
            {
                return response.ContentLength;
            }
            finally
            {
                response.Close();
            }
        }

        /// <summary>
        /// Löscht eine Datei
        /// </summary>
        /// <param name="file">Absoluter Pfad der zu löschenden Datei</param>
        public void DeleteFile(string file)
        {
            FtpWebRequest request = GetFtpRequest(file, WebRequestMethods.Ftp.DeleteFile);
            request.GetResponse().Close();
        }

        /// <summary>
        /// Löscht ein Verzeichnis
        /// </summary>
        /// <param name="dir">Absoluter Pfad zum löschenden Verzeichnis</param>
        public void DeleteDirectory(string dir)
        {
            FtpWebRequest request = GetFtpRequest(dir, WebRequestMethods.Ftp.RemoveDirectory);
            request.GetResponse().Close();
        }

        /// <summary>
        /// Verschiebt eine Datei oder einen Ordner.
        /// </summary>
        /// <param name="source">Absoluter Pfad zur Quelle</param>
        /// <param name="destination">Relativer Pfad zum Ziel</param>
        public void Move(string source, string destination)
        {
            /* Info:  Beim umbennen von Ordnern muss im Ziel der zu verschiebene Ordnername
            mit angegeben werden. Beispiel /root/test -> nach /root/move
            Source: /root/test Destination: move/test */

            FtpWebRequest request = GetFtpRequest(source, WebRequestMethods.Ftp.Rename);
            request.RenameTo = destination;
            request.GetResponse().Close();
        }

        /// <summary>
        /// Überprüft, ob auf dem FTP Server ein Verzeichnis oder eine Datei zum
        /// übergebenen absoluten Pfad existiert.
        /// </summary>
        /// <param name="dirOrFile">Der absolute Pfad zum Verzeichnis oder der Datei</param>
        /// <returns>Wahreitswert, ob das Verzeichnis oder die Datei existiert</returns>
        public bool DirectoryOrFileExists(string dirOrFile)
        {
            try
            {
                string parentDir = Path.GetDirectoryName(dirOrFile);
                List<string> folderObjects = GetDirectoryList(parentDir);
                string name = Path.GetFileName(dirOrFile);
                return folderObjects.Contains(name);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Erstellt ein neues Verzeichnis
        /// </summary>
        /// <param name="newFolder">Absoluter Pfad des neuen Verzeichnis</param>
        public void CreateFolder(string newFolder)
        {
            if (newFolder.Contains('/'))
            {
                // Ggf. führenden Slash entfernen
                if (newFolder[0] == '/')
                    newFolder = newFolder.Remove(0, 1);

                CreateFolder(newFolder.Split('/'));
            }
            else
            {
                CreateFolder(new string[] { newFolder });
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Erstellt ein neues Verzeihnis
        /// </summary>
        /// <param name="pathElements">String-Array, welches die zu erstellenden Ordner repräsentiert</param>
        private void CreateFolder(string[] pathElements)
        {
            if (pathElements.Length < 1)
                return;

            string path = string.Empty;

            for (int i = 0; i < pathElements.Length; i++)
            {
                path += "/" + pathElements[i];

                if (!DirectoryOrFileExists(path) && path != "/")
                {
                    FtpWebRequest request = GetFtpRequest(path, WebRequestMethods.Ftp.MakeDirectory);
                    request.GetResponse().Close();
                }
            }
        }

        /// <summary>
        /// Erzeugt einen <see cref="FtpWebRequest"/> aus dem übergebenen Pfad
        /// und der übergebenen <see cref="WebRequestMethods"/>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private FtpWebRequest GetFtpRequest(string path, string method)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ServerAdress + "/" + path));
            request.Method = method;
            request.UseBinary = this.UseBinary;
            request.Credentials = new NetworkCredential(Login, Password);
            request.Proxy = this.Proxy;
            request.KeepAlive = this.KeepAlive;
            request.UsePassive = this.PassiveMode;
            request.ConnectionGroupName = this.ConnectionGroupName;
            request.ServicePoint.ConnectionLimit = this.ConnectionLimit;
            return request;
        }

        #endregion

        #region Prodected Methods

        /// <summary>
        /// Auslösen des FtpProgressChanged-Events bei Änderung des Kopierstatus einer Datei
        /// </summary>
        /// <param name="e">FTP Copy Event Argumente</param>
        protected virtual void OnFtpProgressChanged(FtpProgressEventArgs e)
        {
            FtpProgressEventHandler handler = FtpProgressChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion
    }
}
