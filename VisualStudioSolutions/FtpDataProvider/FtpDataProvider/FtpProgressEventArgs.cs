using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtpDataProvider
{
    public delegate void FtpProgressEventHandler(object sender, FtpProgressEventArgs e);

    /// <summary>
    /// Klasse zur Darstellung der Event-Argumente eines FTP Kopiervorgangs
    /// </summary>
    public class FtpProgressEventArgs
    {
        public FtpProgressEventArgs(long bytesReceived, double percent, double kbPSec, TimeSpan remaning, object uploadingPackage)
        {
            this.BytesReceived = bytesReceived;
            this.Percent = percent;
            this.KbPSec = kbPSec;
            this.Remaning = remaning;
            this.UploadingPackage = uploadingPackage;
        }

        /// <summary>
        /// Fortschrittsangabe des Kopiervorgangs in Prozent
        /// </summary>
        public double Percent { private set; get; }

        /// <summary>
        /// Datendurchsatz des Kopiervorgangs in KB pro Sekunde
        /// </summary>
        public double KbPSec { private set; get; }

        /// <summary>
        /// Verbleibende Zeitspanne des aktuellen Kopiervorgangs
        /// </summary>
        public TimeSpan Remaning { private set; get; }

        /// <summary>
        /// Gets or sets the bytes received.
        /// </summary>
        /// <value>The bytes received.</value>
        public long BytesReceived { private set; get; }

        /// <summary>
        /// Das Paket, welches gerade hochgeladen wird
        /// </summary>
        public object UploadingPackage { private set; get; }
    }
}
