using System.Windows;

namespace ExportPDF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Display Text
        public string DisplayText
        {
            get;
            set;
        }

        string OutputDirectory = "../../Output/";

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            PDFExporter pdfExporter = new PDFExporter();

            // Load PDF document
            pdfExporter.LoadPDFDocument("../../Data/Barcode.pdf");

            // Export PDF pages as images with default size.
            pdfExporter.ExportAsImage(ExportImageFormat.Jpeg, OutputDirectory);
            
            // Export PDF pages as thumbnail images with 25% of the original page size.
            pdfExporter.ExportAsThumbnails(ExportImageFormat.Tiff, 25, OutputDirectory);

            DisplayText = "Images have been exported successfully in the location: " + '\u0022' + System.IO.Path.GetFullPath("../../Output/")+ '\u0022';
        }
    }
}
