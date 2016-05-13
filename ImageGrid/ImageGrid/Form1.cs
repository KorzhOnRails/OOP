using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageGrid
{

    public partial class Form1 : Form
    {

        
        public Form1()
        {
            InitializeComponent();
             
        
        }

     

        private void Form1_Load(object sender, EventArgs e)
        {
          
  



           
      }

        private void openFileButton_Click(object sender, EventArgs e)
        {

            // Create an OpenFileDialog object.
            OpenFileDialog openFile1 = new OpenFileDialog();

            // Initialize the OpenFileDialog to look for picture.
            openFile1.InitialDirectory = "c:\\";
            openFile1.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            openFile1.FilterIndex = 2;
            openFile1.RestoreDirectory = true;

            // Check if the user selected a file from the OpenFileDialog.
            if (openFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                int width = 300, height = 300;
                Bitmap histOriginal = new Bitmap(width, height);
                Bitmap histResult = new Bitmap(width, height);
                Bitmap originalImage = new Bitmap(openFile1.FileName);
                label1.Text = openFile1.FileName;
                pictureBox1.Image = originalImage;
                Bitmap resultImage = originalImage;

                Histogram hist = new Histogram(originalImage);

                histOriginal = hist.GetHistogram();
                byte factorIntensityValue = 100;
                Histogram result;
                if (textBoxFactorIntensity1.Text != "" && Byte.TryParse(textBoxFactorIntensity1.Text, out factorIntensityValue))
                {
                     result = new Histogram(hist.GetNormalizedImage(originalImage, (byte)factorIntensityValue));
                     pictureBox3.Image = hist.GetNormalizedImage(originalImage, (byte)factorIntensityValue);
                }
                else
                {
                     result = new Histogram(hist.GetNormalizedImage(originalImage));
                     pictureBox3.Image = hist.GetNormalizedImage(originalImage);
                }
                histResult = result.GetHistogram();
                pictureBox1.Image = originalImage;
                pictureBox2.Image = histOriginal;
                
                pictureBox4.Image = histResult;
            }
        }
    }

    public class Histogram
    {
        public int HistWidth { get; set; }
        public int HistHeight { get; set; }
        public Bitmap Image { get; set; }

        private Bitmap Hist { get; set; }

    
        public Histogram(Bitmap image)
        {
            HistHeight = image.Height;
            HistWidth = image.Width;
            this.Image = image;
            this.Hist = new Bitmap(image, 256, 256);

           
        }

        public Bitmap GetNormalizedImage(Bitmap img , byte intensityFactor = 100)
        {
            Bitmap normImg = new Bitmap(img, img.Height, img.Width);

            byte[] pixelIntensityArray = new byte[256];

            //calculating number of pixels with intensity value of i( from 0 to 255) 
            //and recording this number to i-element of pixelIntensityArray 

            for (int i = 0; i < img.Height - 1; i++)
            {
                for (int j = 0; j < img.Width - 1; j++)
                {
                    Color pixel = img.GetPixel(j, i);
                    pixelIntensityArray[pixel.R]++;
                }
            }

            double[] normalizedPixelIntensityArray = new double[256];
            double sum = 0;
            for (int i = 0; i < pixelIntensityArray.Length; i++)
            {
                if (i == 0)
                {
                    sum = 0;
                }
                else
                {
                    sum = normalizedPixelIntensityArray[i - 1];
                }
                if(intensityFactor > 0 )
                normalizedPixelIntensityArray[i] = intensityFactor*255 * pixelIntensityArray[i] / (double)(img.Width * img.Height);
                normalizedPixelIntensityArray[i] += sum;
                
            }

            for (int i = 0; i < normImg.Height - 1; i++)
            {
                for (int j = 0; j < normImg.Width - 1; j++)
                {
                    Color tempPix = normImg.GetPixel(j, i);
                    Byte pixIntensity = tempPix.R;

                    byte r = (byte)normalizedPixelIntensityArray[pixIntensity];
                    byte g = r;
                    byte b = r;

                    
                    normImg.SetPixel(j, i, Color.FromArgb(255, r, g, b));
                }
            }

            return normImg;
        }
        
        public Bitmap GetHistogram()
        {
                 Byte[] pixelIntensityArray = new Byte[256];

                 //calculating number of pixels with intensity value of i( from 0 to 255) 
                 //and recording this number to i-element of pixelIntensityArray 

                 for (int i = 0; i < Image.Height - 1; i++)
                 {
                     for (int j = 0; j < Image.Width - 1; j++)
                     {
                         Color pixel = Image.GetPixel(j, i);
                         pixelIntensityArray[pixel.R]++;
                     }
                 }
                
            //making histogram picture from  Bitmap image
            for (int i = 0; i < 255; i++ )
            {
                for (int j = 0; j < 255; j++ )
                {
                    if (j < 255-pixelIntensityArray[i] )
                    {
                        Hist.SetPixel(i, j, Color.White);
                    }
                    else
                    {
                        Hist.SetPixel(i, j, Color.Black);
                    }
                }
            }
            return Hist;
        }
    }
}
