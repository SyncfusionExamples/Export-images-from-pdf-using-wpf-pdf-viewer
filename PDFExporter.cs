using Syncfusion.Pdf.Parsing;
using Syncfusion.Windows.PdfViewer;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace ExportPDF
{
    internal enum ExportImageFormat
    {
        Jpeg,
        Png,
        Tiff
    }
    internal class PDFExporter
    {
        PdfViewerControl pdfViewerControl = new PdfViewerControl();
        PdfLoadedDocument pdfLoadedDocument = null;
        internal void LoadPDFDocument(string fileName)
        {
            pdfLoadedDocument = new PdfLoadedDocument(fileName);
            pdfViewerControl.Load(pdfLoadedDocument);
        }

        /// <summary>
        /// Export PDF as images with default size with the required image format.
        /// </summary>
        /// <param name="imageFormat">Image format</param>
        /// <param name="directory">Output directory</param>
        internal void ExportAsImage(ExportImageFormat imageFormat, string directory) 
        { 
            BitmapSource[] images = pdfViewerControl.ExportAsImage(0, pdfViewerControl.PageCount-1);
            SaveImages(images,imageFormat,directory,"Images");
        }

        /// <summary>
        /// Export PDF as thumbnail images with custom image sizes.
        /// </summary>
        /// <param name="imageFormat">Image format</param>
        /// <param name="sizePercentage">Percentage of the image size with respect to its default size</param>
        /// <param name="directory">Output directory</param>
        internal void ExportAsThumbnails(ExportImageFormat imageFormat, float sizePercentage, string directory)
        {
            BitmapSource[] images = new BitmapSource[pdfViewerControl.PageCount];

            for (int i=0;i< pdfViewerControl.PageCount;i++)
            {
                SizeF originalSize= pdfLoadedDocument.Pages[i].Size;
                SizeF newSize = new SizeF(originalSize.Width * sizePercentage / 100f, originalSize.Height * sizePercentage / 100f);
                images[i]= pdfViewerControl.ExportAsImage(i, newSize, false);
            }
            SaveImages(images, imageFormat, directory, "Thumbnails");
        }

        /// <summary>
        /// Save the image
        /// </summary>
        /// <param name="images">BitmapSource images</param>
        /// <param name="imageFormat">Image format</param>
        /// <param name="directory">Output directory</param>
        /// <param name="fileName">Output file name</param>
        void SaveImages(BitmapSource[] images, ExportImageFormat imageFormat, string directory, string fileName)
        {
            for (int i = 0; i < images.Length; i++)
            {
                BitmapEncoder encoder = GetEncoder(imageFormat);
                encoder.Frames.Add(BitmapFrame.Create(images[i]));
                FileStream stream = new FileStream(directory + "/" + fileName + "_" + i.ToString() + "." + imageFormat.ToString().ToLower(), FileMode.Create);
                encoder.Save(stream);
            }
        }

        /// <summary>
        /// Gets the BitmapEncoder
        /// </summary>
        /// <param name="imageFormat">Image Format</param>
        /// <returns>BitmapEncoder</returns>
        BitmapEncoder GetEncoder(ExportImageFormat imageFormat)
        {
            if (imageFormat == ExportImageFormat.Png)
                return new PngBitmapEncoder();
            else if (imageFormat == ExportImageFormat.Tiff)
                return new TiffBitmapEncoder();
            return new JpegBitmapEncoder();
        }
    }
}
