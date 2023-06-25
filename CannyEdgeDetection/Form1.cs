using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace CannyEdgeDetection
{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> imgInput;
        Image<Gray, byte> imgGray;
        Image<Gray, byte> imgGaus;
        Image<Gray, byte> imgGrad;
        Image<Gray, byte> imgDirect;
        Image<Gray, byte> imgSuppression;
        Image<Gray, byte> imgThreshold;
        Image<Gray, byte> imgCanny;
        Image<Gray, byte> imgCanny_Eg;
        public Form1()
        {
            InitializeComponent();
        }

        private void ButInput_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    imgInput = new Image<Bgr, byte>(ofd.FileName);
                    BoxInput.Image = imgInput;
                }
            }
            catch(Exception er)
            {
                MessageBox.Show(er.ToString());
            }
        }

        // Calculation of the Gaussian kernel matrix
        public static double[,] GaussianKernel(int length, double sigma)
        {
            double[,] kernel = new double[length, length];
            double kernelSum = 0;
            // Constant part of the Gaussian equation
            double constant = 1 / (2 * Math.PI * sigma * sigma);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    double distance = 0;
                    int x = i - (length / 2);
                    int y = j - (length / 2);
                    distance = ((y * y) + (x * x)) / (2 * sigma * sigma);
                    kernel[i, j] = constant * Math.Exp(-distance);
                    kernelSum += kernel[i, j];
                }
            }
            // Adjust the kernel matrix so that sum of all entries equal to 1
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    kernel[i, j] = kernel[i, j] * 1 / kernelSum;
                }
            }
            return kernel;
        }

        // Convolution of the Gaussian kernel matrix over the input image
        public static Image<Gray, byte> GaussianBlur(Image<Gray, byte> imgConvolute, 
            Image<Gray, byte> imgGray, double[,] kernelmatrix)
        {
            int nrow = kernelmatrix.GetLength(0);
            int range = (int)Math.Floor(nrow / (double)2);      
            for (int i = 0; i < imgGray.Height; ++i)
            {
                for (int j = 0; j < imgGray.Width; ++j)
                {
                    double sum = 0;
                    for (int m = -1; m < range + 1; ++m)
                    {
                        for (int n = -1; n < range + 1; ++n)
                        {
                            // Case the current pixel coordination is out of bound
                            if ((i + m < 0) || (i + m >= imgGray.Height) || 
                                (j + n < 0) || (j + n >= imgGray.Width))
                                sum += 0;                          
                            else                            
                                sum += (double)kernelmatrix[m + 1, n + 1] * 
                                    imgGray.Data[i + m, j + n, 0];                            
                        }
                    }
                    // Adjust the intensity of the pixel between 0 and 255
                    if (sum < 0)
                        sum = 0;
                    else if (sum > 255)
                        sum = 255;
                    imgConvolute.Data[i, j, 0] = (byte)sum;
                }
            }
            return imgConvolute;
        }

        private void ButGaus_Click(object sender, EventArgs e)
        {
            if(imgInput == null)
            {
                MessageBox.Show("Image not found!");
                return;
            }
            // Convert Bgr image into gray scale image
            imgGray = imgInput.Convert<Gray, byte>();
            imgGaus = imgGray.CopyBlank();
            if(TxtSize.Text == "" || TxtSigma.Text == "")
            {
                MessageBox.Show("Invalid input data!");
                return;
            }

            int length = int.Parse(TxtSize.Text);
            if (length % 2 == 0)
            {
                MessageBox.Show("Kernel size must be odd number!");
                return;
            }
            double sigma = double.Parse(TxtSigma.Text);
            if (sigma < 0)
            {
                MessageBox.Show("Sigma value must be at least 0!");
                return;
            }
            double[,] kernel = GaussianKernel(length, sigma);
            imgGaus = GaussianBlur(imgGaus, imgGray, kernel);
            //CvInvoke.GaussianBlur(imgGray, imgGaus, new Size(length, length), sigma);
            BoxProcess.Image = imgGaus;
        }

        private void ButSobel_Click(object sender, EventArgs e)
        {
            if(imgGaus == null)
            {
                MessageBox.Show("Image not found!");
                return;
            }
            imgGrad = imgGaus.CopyBlank();
            imgDirect = imgGaus.CopyBlank();

            double[,] kernel_x = GradientCalculationX(imgGaus);
            double[,] kernel_y = GradientCalculationY(imgGaus);

            imgGrad = ModuleOfGradient(kernel_x, kernel_y, imgGaus);
            imgDirect = DirectionOfGradient(kernel_x, kernel_y, imgGaus);

            BoxInput.Image = imgGrad;
            BoxProcess.Image = imgDirect;

        }

        // Convolution of kernel matrix over an input image
        public static double[,] Convolution(double[,] kernel, Image<Gray, byte> InputMatrix)
        {
            double[,] Matrix = new double[InputMatrix.Height, InputMatrix.Width];
            int range = (int)Math.Floor(kernel.GetLength(0) / (double)2);
            for (int i = 0; i < InputMatrix.Height; i++)
            {
                for (int j = 0; j < InputMatrix.Width; j++)
                {
                    double sum = 0;
                    for (int m = -1; m < range + 1; m++)
                    {
                        for (int n = -1; n < range + 1; n++)
                        {
                            // Check if current pixel coordination is out of bound
                            if ((i + m < 0) || (i + m >= InputMatrix.Height) || 
                                (j + n < 0) || (j + n >= InputMatrix.Width))
                                sum += 0;
                            else
                                sum += (double)kernel[m + 1, n + 1] * InputMatrix.Data[i + m, j + n, 0];
                        }
                    }
                    Matrix[i, j] = sum;
                }
            }
            return Matrix;
        }
        //Functions to calculate gradient to Gx and Gy
        public static double[,] GradientCalculationX(Image<Gray, byte> InputMatrix)
        {
            // Kernel matrix on x ( horizontal)
            double[,] kernel_x = new double[3, 3] { { -1, 0, 1 }, 
                                                    { -2, 0, 2 }, 
                                                    { -1, 0, 1 } };
            // Call convolution to calculate Gx and then return that matrix
            double[,] G_xMatrix = Convolution(kernel_x, InputMatrix);
            return G_xMatrix;
        }
        public static double[,] GradientCalculationY(Image<Gray, byte> InputMatrix)
        {
            // Kernel matrix on y ( horizontal)
            double[,] kernel_y = new double[3, 3] { { -1, -2, -1 }, 
                                                    { 0, 0, 0 }, 
                                                    { 1, 2, 1 } };
            // Call convolution to calculate Gy and then return that matrix
            double[,] G_yMatrix = Convolution(kernel_y, InputMatrix);
            return G_yMatrix;
        }

        // Functions to calculate magnitude of Gradient from Gx and Gy
        public static Image<Gray, byte> ModuleOfGradient(double[,] G_x, double[,] G_y, 
                                                        Image<Gray, byte> Input)
        {
            double[,] module_mat = new double[Input.Height, Input.Width];
            double max = 0;
            Image<Gray, byte> Module = Input.CopyBlank();
            for (int i = 0; i < Module.Height; i++)
            {
                for (int j = 0; j < Module.Width; j++)
                {
                    // Applying formula to calculate magnitude at each pixel in Gx and Gy
                    double a = G_x[i, j];
                    double b = G_y[i, j];
                    double pixel = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
                    module_mat[i, j] = pixel;
                    // Find max value in the matrix
                    if (pixel > max)
                        max = pixel;
                }
            }
            for (int i = 0; i < Module.Height; i++)
            {
                for (int j = 0; j < Module.Width; j++)
                {
                    // Scale the intensity of the pixel base on maximum intensity
                    // and put it into the image
                    double a = module_mat[i, j];
                    Module.Data[i, j, 0] = (byte)(a / max * 255);
                }
            }
            return Module;
        }

        // Function to calculate the angle to specify the direction
        public static Image<Gray, byte> DirectionOfGradient(double[,] G_x, double[,] G_y, 
            Image<Gray, byte> Input)
        {
            Image<Gray, byte> Direction = Input.CopyBlank();
            double[,] Angle = new double[Direction.Height, Direction.Width];
            // Use arctan to find the angle (in degree unit)
            for (int i = 0; i < Direction.Height; i++)
            {
                for (int j = 0; j < Direction.Width; j++)
                {
                    if (G_x[i, j] == 0)
                    {
                        if (G_y[i, j] >= 0)
                            Angle[i, j] = 90;
                        else
                            Angle[i, j] = -90;
                    }
                    else if (G_x[i, j] < 0)
                    {
                        if (G_y[i, j] >= 0)
                            Angle[i, j] = Math.Atan(G_y[i, j] / G_x[i, j]) * 180 / Math.PI + 180;
                        else
                            Angle[i, j] = Math.Atan(G_y[i, j] / G_x[i, j]) * 180 / Math.PI - 180;
                    }
                    else
                        Angle[i, j] = Math.Atan(G_y[i, j] / G_x[i, j]) * 180 / Math.PI;
                    // All angle need to be more than 0 to find the direction easily
                    if (Angle[i, j] < 0)
                        Angle[i, j] += 180;
                }
            }
            // Summarize the ranges of angles to specify the exact direction for the next step
            for (int i = 0; i < Direction.Height; i++)
            {
                for (int j = 0; j < Direction.Width; j++)
                {
                    if (Angle[i, j] > 0 && Angle[i, j] <= 22.5)
                        Direction.Data[i, j, 0] = 0;
                    else if (Angle[i, j] > 22.5 && Angle[i, j] <= 67.5)
                        Direction.Data[i, j, 0] = 45;
                    else if (Angle[i, j] > 67.5 && Angle[i, j] <= 115.5)
                        Direction.Data[i, j, 0] = 90;
                    else if (Angle[i, j] > 115.5 && Angle[i, j] <= 157.5)
                        Direction.Data[i, j, 0] = 135;
                    else if (Angle[i, j] > 157.5 && Angle[i, j] <= 180)
                        Direction.Data[i, j, 0] = 180;
                }
            }
            return Direction;
        }

        private void ButSuppression_Click(object sender, EventArgs e)
        {
            if(imgGrad == null || imgDirect == null)
            {
                MessageBox.Show("Image not found!");
                return;
            }
            imgSuppression = imgGrad.CopyBlank();
            int[,] paddingMat = ZeroPadding(imgGrad);
            imgSuppression = Suppression(imgSuppression, paddingMat, imgDirect);
            BoxProcess.Image = imgSuppression;
        }
        // Function to zero padding the Gradient magnitude image 
        // before applying non-suppression
        public static int[,] ZeroPadding(Image<Gray, byte> imgGradient)
        {
            int width = imgGradient.Width + 2;
            int height = imgGradient.Height + 2;
            int[,] zeropad_matrix = new int[height, width];

            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    if (i == 0 || i == height - 1 || j == 0 || j == width - 1)
                        zeropad_matrix[i, j] = 0;
                    else
                    {
                        zeropad_matrix[i, j] = imgGradient.Data[i - 1, j - 1, 0];
                    }
                }
            }
            return zeropad_matrix;
        }
        // Function of non-maximum supprssion to thin out the edges
        public static Image<Gray, byte> Suppression(Image<Gray, byte> nonmax, int[,] Module, 
                                                    Image<Gray, byte> Direction)
        {
            int nrow = Module.GetLength(0);
            int ncol = Module.GetLength(1);
            // Copy the real pixel intensity
            // from the zero-padding gradient matrix
            for (int i = 1; i < nrow - 1; i++)
            {
                for (int j = 1; j < ncol - 1; j++)
                {
                    nonmax.Data[i - 1, j - 1, 0] = (byte)Module[i, j];
                }
            }

            for (int i = 1; i < nrow - 1; i++)
            {
                for (int j = 1; j < ncol - 1; j++)
                {
                // Compare the intensity of current pixel with other 2 pixel
                // when gradient directions are 0 and 180
                    if ((Direction.Data[i - 1, j - 1, 0] == 0) || (Direction.Data[i - 1, j - 1, 0] == 180))
                    {
                        if ((Module[i, j] < Module[i, j + 1]) || (Module[i, j] < Module[i, j - 1]))
                            nonmax.Data[i - 1, j - 1, 0] = 0;                    
                    }
                // Compare the intensity of current pixel with other 2 pixel when gradient directions is 45
                    else if (Direction.Data[i - 1, j - 1, 0] == 45)
                    {
                        if ((Module[i, j] < Module[i + 1, j + 1]) || (Module[i, j] < Module[i - 1, j - 1]))
                            nonmax.Data[i - 1, j - 1, 0] = 0;
                    }
                // Compare the intensity of current pixel with other 2 pixel when gradient directions is 90
                    else if (Direction.Data[i - 1, j - 1, 0] == 90)
                    {
                        if ((Module[i, j] < Module[i - 1, j]) || (Module[i, j] < Module[i + 1, j]))
                            nonmax.Data[i - 1, j - 1, 0] = 0;
                    }
                // Compare the intensity of current pixel with other 2 pixel when gradient directions is 135
                    else if (Direction.Data[i - 1, j - 1, 0] == 135)
                    {
                        if ((Module[i, j] < Module[i - 1, j + 1]) || (Module[i, j] < Module[i + 1, j - 1]))
                            nonmax.Data[i - 1, j - 1, 0] = 0;
                    }
                }
            }
            return nonmax;
        }

        private void ButThreshold_Click(object sender, EventArgs e)
        {
            if(imgSuppression == null)
            {
                MessageBox.Show("Image not found!");
                return;
            }
            imgThreshold = imgSuppression.CopyBlank();
            imgThreshold = DoubleThreshold(imgSuppression);
            BoxInput.Image = imgThreshold;
        }

        // Double thresholding for classification of egdes 
        public static Image<Gray, byte> DoubleThreshold(Image<Gray, byte> ImageSuppression)
        {
            Image<Gray, byte> imgThreshold = ImageSuppression.CopyBlank();
            // Set the value of high and low threshold
            double highThreshold, lowThreshold;
            // 255 is the maximum intensity of pixel, 0.09 is high threshold ratio
            highThreshold = 255 * 0.09;
            // 0.05 is the low threshold ratio
            lowThreshold = highThreshold * 0.05;    

            for (int i = 0; i < imgThreshold.Height; i++)
            {
                for (int j = 0; j < imgThreshold.Width; j++)
                {
                    // Determine the strong edges
                    if (ImageSuppression.Data[i, j, 0] > highThreshold)                   
                        imgThreshold.Data[i, j, 0] = 255;                  
                    // Determine the non-relevenat edges
                    else if (ImageSuppression.Data[i, j, 0] < lowThreshold)
                        imgThreshold.Data[i, j, 0] = 0;
                    // Determine the weak edges
                    else if (ImageSuppression.Data[i, j, 0] >= lowThreshold && 
                        ImageSuppression.Data[i, j, 0] <= highThreshold)
                        imgThreshold.Data[i, j, 0] = 25;
                }
            }
            return imgThreshold;
        }
        private void ButHysteresys_Click(object sender, EventArgs e)
        {
            if (imgThreshold == null)
            {
                MessageBox.Show("Image not found!");
                return;
            }
            imgCanny = Hyteresis_Tracking(imgThreshold,25);
            BoxProcess.Image = imgCanny;

            imgCanny_Eg = imgGray.CopyBlank();
            //Canny edge detection using EmguCV built in function
            CvInvoke.Canny(imgGaus, imgCanny_Eg, 11.475, 22.95);
            BoxInput.Image = imgCanny_Eg;
        }

        // Edge tracking by hyteresis algorithm 
        public static Image<Gray, byte> Hyteresis_Tracking(Image<Gray, byte> imgCanny, int weak)
        {
            for(int i = 0; i < imgCanny.Height; ++i)
            {
                for (int j = 0; j < imgCanny.Width; ++j)
                {
                    // Check wether current pixel is weak or not
                    if (imgCanny.Data[i, j, 0] != weak)
                        continue;
                    bool strong = false; 
                    for (int n = -1; n < 2; ++n)
                    {
                        // Check if current pixel coordination is out of bound
                        if (i + n < 0 || i + n >= imgCanny.Height)
                            continue;
                        for (int m = -1; m < 2; ++m)
                        {
                            /*Check if current pixel coordination is out of bound
                            and avoid looping the processing pixel*/
                            if (j + m < 0 || j + m >= imgCanny.Width || (n == 0 && m == 0))
                                continue;
                            /* If any of the surrounding pixel is strong
                            then considered processing pixel as strong and extend the edge*/
                            if (imgCanny.Data[i + n, j + m, 0] == 255)
                            {
                                imgCanny.Data[i, j, 0] = 255;
                                strong = true;
                            }
                        }
                    }
                    // If no surrounding strong pixel then remove it
                    if (strong == false)
                        imgCanny.Data[i, j, 0] = 0;
                }
            }
            return imgCanny;
        }
    }
}
