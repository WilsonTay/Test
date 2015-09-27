using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PdfGenerator
{
    public class WkHtmlToPdf
    {

        //public const string FullPath = @"D:\Program Files\wkhtmltopdf\bin\";
        private const string HtmlToPdfExePath = @"wkhtmltopdf.exe";

        public static bool GeneratePdf(string localDir, string html, Stream pdf, Size pageSize)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(html);
            var stream = new MemoryStream(bytes);

            using (var reader = new StreamReader(stream))
            {
                return GeneratePdf(localDir, reader, pdf, pageSize);
            }
        }

        public static bool GeneratePdf(string localDir, StreamReader html, Stream pdf, Size pageSize)
        {
            Process p;
            StreamWriter stdin;
            ProcessStartInfo psi = new ProcessStartInfo();

            //psi.FileName = HtmlToPdfExePath;
            psi.FileName = Path.Combine(localDir, HtmlToPdfExePath);
            psi.WorkingDirectory = Path.GetDirectoryName(psi.FileName);

            // run the conversion utility
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            // note: that we tell wkhtmltopdf to be quiet and not run scripts
            psi.Arguments = "-q --encoding ascii -n --disable-smart-shrinking " + (pageSize.IsEmpty ? "" : "--page-width " + pageSize.Width + "mm --page-height " + pageSize.Height + "mm") + " - -";

            p = Process.Start(psi);

            try
            {
                stdin = p.StandardInput;
                stdin.AutoFlush = true;
                stdin.Write(html.ReadToEnd());
                stdin.Dispose();

                CopyStream(p.StandardOutput.BaseStream, pdf);
                p.StandardOutput.Close();
                pdf.Position = 0;

                p.WaitForExit(10000);

                return true;
            }
            catch
            {
                return false;

            }
            finally
            {
                p.Dispose();
            }
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }
    }
}
