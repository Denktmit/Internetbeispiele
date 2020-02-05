using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtpDataProvider
{
    /// <summary>
    /// Einfache Repräsentation von Verzeichnissen und Dateien
    /// auf einem FTP-Server
    /// </summary>
    public class FtpContent
    {
        /// <summary>
        /// Erstellt eine neue <see cref="FtpContent"/> Instanz
        /// </summary>
        /// <param name="name">Der Name</param>
        /// <param name="path">Der absolute Pfad</param>
        /// <param name="isDirectory">Gibt an, ob es sich um ein Verzeichnis handelt</param>
        public FtpContent(string name, string path, bool isDirectory)
        {
            Name = name;
            Path = path + "/" + name;
            IsDirectory = isDirectory;
        }

        /// <summary>
        /// Der Name
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Der absolute Pfad
        /// </summary>
        public string Path
        {
            get;
            set;
        }

        /// <summary>
        /// Gibt an, ob es sich um ein Verzeichnis handelt
        /// </summary>
        public bool IsDirectory
        {
            get;
            set;
        }
    }
}
