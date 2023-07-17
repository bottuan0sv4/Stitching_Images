using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Stitching;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Rectangle = System.Drawing.Rectangle;
using Point = System.Drawing.Point;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using Emgu.CV.Features2D;
using System.Collections.Generic;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Image_Stitcher
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // Create change property event and function
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property_name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property_name));
        }

        // Public variables for histogram
        // Public variables for histogram
        public int Min_H;
        public int Max_H;
        public int Max_A;
        public int Min_A;
        public string data;

        // Min histogram range
        public int min_h
        {
            get { return Min_H; }
            set
            {
                Min_H = value;
                OnPropertyChanged("min_h");
            }
        }

        // Max histogram range
        public int max_h
        {
            get { return Max_H; }
            set
            {
                Max_H = value;
                OnPropertyChanged("max_h");
            }
        }

        // Max histogram Y Axis
        public int MaxAxisValue
        {
            get { return Max_A; }
            set
            {
                Max_A = value;
                OnPropertyChanged("MaxAxisValue");
            }
        }

        // Min histogram Y Axis
        public int MinAxisValue
        {
            get { return Min_A; }
            set
            {
                Min_A = value;
                OnPropertyChanged("MinAxisValue");
            }
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        // Variable to change image quality
        public int Image_Quality;

        public int image_quality
        {
            get
            {
                return Image_Quality;
            }
            set
            {
                Image_Quality = value;
                OnPropertyChanged("image_quality");
            }
        }

        // Variable to change image
        public string Img_Path;
        public string img_path
        {
            get
            {
                return Img_Path;
            }
            set
            {
                Img_Path = value;
                OnPropertyChanged("img_path");
}
        }

        public string date_time;



        public MainWindow()
        {

            InitializeComponent();
            SeriesCollection = new SeriesCollection();
            DateTime d = new DateTime();
            d = DateTime.Now;
            date_time = d.ToString("dd-MM-yyyy") + "_" + d.ToString("HH-mm-ss");
            combobox_name.Items.Add(date_time);
            // Initialize the SeriesCollection for the chart
            SeriesCollection = new SeriesCollection();

        }

        private void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "min_h" || e.PropertyName == "max_h")
            {
                // Change histogram based on new min and max
                int[] int_values = ComputeImageHistogram(img_path);
                UpdateHistogram(int_values);
            }
        }


        void Window_Closing(object sender, CancelEventArgs e)
        {

        }

        class input_image
        {
            public string ID { get; set;}
            public string Path { get; set;}

        }

        private void btn_load_Click(object sender, RoutedEventArgs e)
        {
            // Open Explorer to choose folder
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            txt_path.Text = dialog.SelectedPath;
            if (string.IsNullOrEmpty(txt_path.Text))
            {
                // If there is no path in Path Textbox
                MessageBox.Show("No path has been selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Clear everything else
                list_image.Items.Clear();
                txt_info.Text = "";
                main_image.Source = null;
                SeriesCollection.Clear();

                // Get only file paths with image extension
                var files = Directory.EnumerateFiles(txt_path.Text, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".png") || s.EndsWith(".jpg") || s.EndsWith(".tiff") || s.EndsWith(".jpeg"));
                string[] images = (from string c in files select c.ToString()).ToArray();

                if (images.Length == 0)
                {
                    // If there is no image in chosen folder
                    MessageBox.Show("There are no images in the chosen folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (images.Length == 1)
                {
                    // If there is only one image in chosen folder
                    MessageBox.Show("There is only one image in the chosen folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // If there are images in chosen folder
                    // Load the images to ListView
                    int j = 1;
                    foreach (string img in images)
                    {
                        list_image.Items.Add(new input_image() { ID = j.ToString(), Path = img.ToString() });
                        j++;
                    }

                    // Show number of images in Info Textbox
                    txt_info.AppendText("Total images: " + (j - 1).ToString() + ".");
                    txt_info.AppendText(Environment.NewLine);

                    // Show type of image in Info Textbox
                    string imageType = GetImageType(images[0]);
                    txt_info.AppendText("Image type: " + imageType + ".");
                }
            }
        }

        private string GetImageType(string imagePath)
        {
            using (FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    try
                    {
                        byte[] buffer = reader.ReadBytes(4);
                        if (buffer.Length >= 4 && buffer[0] == 0xFF && buffer[1] == 0xD8 && buffer[2] == 0xFF && buffer[3] == 0xE0)
                            return "JPEG";
                        if (buffer.Length >= 4 && buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4E && buffer[3] == 0x47)
                            return "PNG";
                        if (buffer.Length >= 4 && buffer[0] == 0x49 && buffer[1] == 0x49 && buffer[2] == 0x2A && buffer[3] == 0x00)
                            return "TIFF";
                        if (buffer.Length >= 4 && buffer[0] == 0x42 && buffer[1] == 0x4D)
                            return "BMP";
                    }
                    catch (Exception)
                    {
                        return "Unknown";
                    }
                }
            }
            return "Unknown";
        }
    


    private void select_zoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TransformGroup transformGroup = (TransformGroup)main_image.RenderTransform;
            ScaleTransform transform = (ScaleTransform)transformGroup.Children[0];

            int zoom = Convert.ToInt32(select_zoom.Value);
            transform.ScaleX = zoom / 10;
            transform.ScaleY = zoom / 10;

            // Cập nhật vị trí của hình ảnh để nó ở trung tâm
            double imageWidth = main_image.ActualWidth * transform.ScaleX;
            double imageHeight = main_image.ActualHeight * transform.ScaleY;

            double offsetX = (border.ActualWidth - imageWidth) / 2;
            double offsetY = (border.ActualHeight - imageHeight) / 2;

            transformGroup.Children[1].SetValue(TranslateTransform.XProperty, offsetX);
            transformGroup.Children[1].SetValue(TranslateTransform.YProperty, offsetY);
        }


        private void select_quality_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (Convert.ToInt32(select_quality.Value) == 100)
            {
                image_quality = 0;
            }
            if (Convert.ToInt32(select_quality.Value) == 90)
            {
                image_quality = 5;
            }
            else if (Convert.ToInt32(select_quality.Value) == 80)
            {
                image_quality = 10;
            }
            else if (Convert.ToInt32(select_quality.Value) == 70)
            {
                image_quality = 15;
            }
            else if (Convert.ToInt32(select_quality.Value) == 60)
            {
                image_quality = 20;
            }  
        }

        public static string StitchImages(string folderPath, string outputPath, string outputFileName)
        {
            // Load image from path
            string[] imageFiles = Directory.GetFiles(folderPath, "*.jpg");

            // Create array mat
            Mat[] images = new Mat[imageFiles.Length];

            // Input image to array
            Parallel.For(0, imageFiles.Length, i =>
            {
                images[i] = new Mat(imageFiles[i], ImreadModes.Color);
            });

            VectorOfMat vectorOfImages = new VectorOfMat(images);

            // Create stitching
            Stitcher stitcher = new Stitcher();

            // Create panorama
            Mat panorama = new Mat();

            // Stitching
            Stitcher.Status status = stitcher.Stitch(vectorOfImages, panorama);

            if (status == Stitcher.Status.Ok)
            {
                // Create mask for floodFill
                Mat mask = new Mat(panorama.Rows + 2, panorama.Cols + 2, DepthType.Cv8U, 1);
                mask.SetTo(new MCvScalar(0));

                // Fill black areas with white color
                Rectangle rect = new Rectangle();
                CvInvoke.FloodFill(panorama, mask, new Point(0, 0), new MCvScalar(255, 255, 255), out rect, new MCvScalar(1, 1, 1), new MCvScalar(1, 1, 1));
                CvInvoke.FloodFill(panorama, mask, new Point(panorama.Width - 1, 0), new MCvScalar(255, 255, 255), out rect, new MCvScalar(1, 1, 1), new MCvScalar(1, 1, 1));
                CvInvoke.FloodFill(panorama, mask, new Point(0, panorama.Height - 1), new MCvScalar(255, 255, 255), out rect, new MCvScalar(1, 1, 1), new MCvScalar(1, 1, 1));
                CvInvoke.FloodFill(panorama, mask, new Point(panorama.Width - 1, panorama.Height - 1), new MCvScalar(255, 255, 255), out rect, new MCvScalar(1, 1, 1), new MCvScalar(1, 1, 1));

                // Lưu kết quả stitching ra file
                string outputFilePath = System.IO.Path.Combine(outputPath, outputFileName);
                CvInvoke.Imwrite(outputFilePath + ".jpg", panorama);
                return "Stitching completed! Save to: " + outputPath;
            }
            else
            {
                return "Stitching fail!";
            }
        }


        public static int[] ComputeImageHistogram(string imagePath)
        {
            // Convert to gray
            Mat image = new Mat(imagePath, Emgu.CV.CvEnum.ImreadModes.Grayscale);

            // Create arry mat[] histogram
            Mat hist = new Mat();

            // Calculate histogram
            float[] ranges = { 0.0f, 256.0f };
            int[] channel = { 0 };
            int[] histSize = { 256 };
            CvInvoke.CalcHist(new VectorOfMat(new Mat[] { image }), channel, null, hist, histSize, ranges, false);

            // Convert mat
            Image<Gray, float> histImage = hist.ToImage<Gray, float>();
            float[,,] histData = histImage.Data;
            int[] histogram = new int[256];
            for (int i = 0; i < hist.Rows; i++)
            {
                histogram[i] = (int)histData[i, 0, 0];
            }
            return histogram;
        }

        private void btn_execute_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt_path.Text))
            {
                // If there is no path in Path Textbox
                System.Windows.MessageBox.Show("No path has been selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                StitchImages(txt_path.Text, txt_path.Text, combobox_name.Text);

                while (true)
                {
                    if (File.Exists(txt_path.Text + "\\" + combobox_name.Text + ".jpg"))
                    {
                        img_path = txt_path.Text + "\\" + combobox_name.Text + ".jpg";
                        main_image.Source = new BitmapImage(new Uri(img_path, UriKind.Absolute));
                        image_quality = 0;

                        // Get histogram data and pass to 'int_values' variable
                        int[] int_values = ComputeImageHistogram(img_path);

                        // Clear previous chart
                        SeriesCollection.Clear();
                        SeriesCollection.Add(
                            new ColumnSeries
                            {
                                Title = "HISTOGRAM",
                                Values = new ChartValues<int>(int_values),
                                ColumnPadding = 0,
                            }
                        );

                        Labels = Enumerable.Range(min_h, max_h - min_h + 1).Select(i => i.ToString()).ToArray();

                        // Get min, max value for Y Axis
                        MaxAxisValue = int_values.Max();
                        MinAxisValue = 0;

                        Formatter = value => value.ToString("N");
                        DataContext = this;

                        // Apply histogram to the image
                        ApplyHistogramToImage(img_path);

                        break;
                    }
                }
            }
        }

        public static Mat StitchImages(string inputPath)
        {
            SIFT siftDetector = new SIFT();

            // Load images
            string[] imagePaths = Directory.GetFiles(inputPath, "*.*", SearchOption.TopDirectoryOnly);

            Mat modelImage = CvInvoke.Imread(imagePaths[0], Emgu.CV.CvEnum.ImreadModes.Grayscale);
            VectorOfKeyPoint modelKeyPoints = new VectorOfKeyPoint();
            Mat modelDescriptors = new Mat();
            siftDetector.DetectAndCompute(modelImage, null, modelKeyPoints, modelDescriptors, false);

            Mat resultImage = modelImage;

            for (int i = 1; i < imagePaths.Length; i++)
            {
                Mat observedImage = CvInvoke.Imread(imagePaths[i], Emgu.CV.CvEnum.ImreadModes.Grayscale);
                VectorOfKeyPoint observedKeyPoints = new VectorOfKeyPoint();
                Mat observedDescriptors = new Mat();
                siftDetector.DetectAndCompute(observedImage, null, observedKeyPoints, observedDescriptors, false);

                var matcher = new BFMatcher(DistanceType.L2);
                VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch();
                matcher.KnnMatch(modelDescriptors, observedDescriptors, matches, 2, null);

                // Filter matches using ratio test
                var goodMatches = new List<MDMatch>();
                foreach (var match in matches.ToArrayOfArray())
                {
                    if (match[0].Distance < 0.75 * match[1].Distance)
                        goodMatches.Add(match[0]);
                }

                if (goodMatches.Count >= 4)
                {
                    PointF[] pts1 = new PointF[goodMatches.Count];
                    PointF[] pts2 = new PointF[goodMatches.Count];

                    for (int j = 0; j < goodMatches.Count; j++)
                    {
                        pts1[j] = modelKeyPoints[goodMatches[j].QueryIdx].Point;
                        pts2[j] = observedKeyPoints[goodMatches[j].TrainIdx].Point;
                    }

                    Mat homography = CvInvoke.FindHomography(pts1, pts2, RobustEstimationAlgorithm.Ransac, 1.5);
                    Mat result = new Mat();

                    CvInvoke.WarpPerspective(resultImage, result, homography,
                        new System.Drawing.Size(resultImage.Width + observedImage.Width, resultImage.Height));
                    CvInvoke.CvtColor(result, result, Emgu.CV.CvEnum.ColorConversion.Gray2Bgr);

                    resultImage = result;
                }

                observedImage.Dispose();
                observedKeyPoints.Dispose();
                observedDescriptors.Dispose();
                matches.Dispose();
            }

            modelImage.Dispose();
            modelKeyPoints.Dispose();
            modelDescriptors.Dispose();

            return resultImage;
        }

        private Bitmap MatToBitmap(Mat image)
        {
            // Convert the Mat to a Bitmap
            Bitmap bitmap = image.ToBitmap();
            return bitmap;
        }

        private void ApplyHistogramToImage(string imagePath)
        {
            // Read the image
            Mat image = new Mat(imagePath, Emgu.CV.CvEnum.ImreadModes.Color);

            // Split image into 3 channels
            VectorOfMat vm = new VectorOfMat();
            CvInvoke.Split(image, vm);

            for (int i = 0; i < 3; i++)
            {
                Mat temp = vm[i];

                // Apply histogram normalization to each channel
                CvInvoke.Normalize(temp, temp, min_h, max_h, NormType.MinMax);
            }

            // Merge the channels
            CvInvoke.Merge(vm, image);

            // Convert the result back to a BitmapImage and set it to the main image
            Bitmap bitmap = MatToBitmap(image);
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                main_image.Source = bitmapImage;
            }
        }


        private void select_min_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // Update histogram and apply it to the image
            min_h = Convert.ToInt32(select_min.Value);

            // Update histogram based on new min and max
            int[] int_values = ComputeImageHistogram(img_path);
            UpdateHistogram(int_values);

            ApplyHistogramToImage(img_path);
        }

        private void select_max_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // Update histogram and apply it to the image
            max_h = Convert.ToInt32(select_max.Value);

            // Update histogram based on new min and max
            int[] int_values = ComputeImageHistogram(img_path);
            UpdateHistogram(int_values);

            ApplyHistogramToImage(img_path);
        }



        private void UpdateHistogram(int[] int_values)
        {
            // Clear previous chart
            SeriesCollection.Clear();
            SeriesCollection.Add(
                new ColumnSeries
                {
                    Title = "HISTOGRAM",
                    Values = new ChartValues<int>(int_values),
                    ColumnPadding = 0,
                }
            );

            // Update min, max Y
            MaxAxisValue = int_values.Max();
            MinAxisValue = 0;

            // Update X
            Labels = Enumerable.Range(min_h, max_h - min_h + 1).Select(i => i.ToString()).ToArray();

            // Update
            Formatter = value => value.ToString("N");
            DataContext = this;
        }

        private void OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            
            if (string.IsNullOrEmpty(img_path))
            {
                return; 
            }

            // Get the new values for min_h and max_h from the `IntegerUpDown` controls
            min_h = select_min.Value ?? 0;
            max_h = select_max.Value ?? 255;

            // Apply the histogram to the image again
            ApplyHistogramToImage(img_path);

            // Update the histogram chart
            UpdateHistogram(ComputeImageHistogram(img_path)); // histogram in new image
        }



        private void btn_big_Click(object sender, RoutedEventArgs e)
        {
            if (main_image.Source == null)
            {
                System.Windows.MessageBox.Show("There is no wholeboard image.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (File.Exists(txt_path.Text + "\\" + combobox_name.Text + ".jpg"))
            {
                // Set back max data
                max_h = 255;
                select_max.Value = max_h; // Update the control value

                // Update histogram based on new max
                int[] int_values = ComputeImageHistogram(img_path);
                UpdateHistogram(int_values);

                ApplyHistogramToImage(img_path);
            }
        }

        private void btn_small_Click(object sender, RoutedEventArgs e)
        {
            if (main_image.Source == null)
            {
                System.Windows.MessageBox.Show("There is no wholeboard image.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (File.Exists(txt_path.Text + "\\" + combobox_name.Text + ".jpg"))
            {
                // Set back min data
                min_h = 0;
                select_min.Value = min_h; // Update the control value

                // Update histogram based on new min
                int[] int_values = ComputeImageHistogram(img_path);
                UpdateHistogram(int_values);

                ApplyHistogramToImage(img_path);
            }
        }


    }
}


