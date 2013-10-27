/*
 * The Following Code was developed by Dewald Esterhuizen
 * View Documentation at: http://softwarebydefault.com
 * Licensed under Ms-PL 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace ImageEdgeDetection
{
    public partial class MainForm : Form
    {
        private Bitmap originalBitmap = null;
        private Bitmap previewBitmap = null;
        private Bitmap resultBitmap = null;
        
        public MainForm()
        {
            InitializeComponent();

            cmbEdgeDetection.SelectedIndex = 0;
        }

        private void btnOpenOriginal_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select an image file.";
            ofd.Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg";
            ofd.Filter += "|Bitmap Images(*.bmp)|*.bmp";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader streamReader = new StreamReader(ofd.FileName);
                originalBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
                streamReader.Close();

                previewBitmap = originalBitmap.CopyToSquareCanvas(picPreview.Width);
                picPreview.Image = previewBitmap;

                ApplyFilter(true);
            }
        }

        private void btnSaveNewImage_Click(object sender, EventArgs e)
        {
            ApplyFilter(false);

            if (resultBitmap != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Specify a file name and file path";
                sfd.Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg";
                sfd.Filter += "|Bitmap Images(*.bmp)|*.bmp";

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileExtension = Path.GetExtension(sfd.FileName).ToUpper();
                    ImageFormat imgFormat = ImageFormat.Png;

                    if (fileExtension == "BMP")
                    {
                        imgFormat = ImageFormat.Bmp;
                    }
                    else if (fileExtension == "JPG")
                    {
                        imgFormat = ImageFormat.Jpeg;
                    }

                    StreamWriter streamWriter = new StreamWriter(sfd.FileName, false);
                    resultBitmap.Save(streamWriter.BaseStream, imgFormat);
                    streamWriter.Flush();
                    streamWriter.Close();

                    resultBitmap = null;
                }
            }
        }

        private void ApplyFilter(bool preview)
        {
            if (previewBitmap == null || cmbEdgeDetection.SelectedIndex == -1)
            {
                return;
            }

            Bitmap selectedSource = null;
            Bitmap bitmapResult = null;

            if (preview == true)
            {
                selectedSource = previewBitmap;
            }
            else
            {
                selectedSource = originalBitmap;
            }

            if (selectedSource != null)
            {
                if (cmbEdgeDetection.SelectedItem.ToString() == "None")
                {
                    bitmapResult = selectedSource;
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Sobel Operator")
                {
                    bitmapResult = selectedSource.Sobel3x3Filter();
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Prewitt Operator")
                {
                    bitmapResult = selectedSource.PrewittFilter();
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Robert Operator")
                {
                    bitmapResult = selectedSource.RobertFilter();
                }
                else if (cmbEdgeDetection.SelectedItem.ToString() == "Canny Edge Ditection")
                {
                    try
                    {
                        float TH = (float)Convert.ToDouble(textBox1.Text);
                        float TL = (float)Convert.ToDouble(textBox2.Text);
                        int MaskSize = Convert.ToInt32(textBox3.Text);
                        float Sigma = (float)Convert.ToDouble(textBox4.Text);
                        bitmapResult = selectedSource.CannyEdgeDitection(TH, TL, MaskSize, Sigma);
                    }
                    catch (Exception)
                    {
                        float TH = 52;
                        float TL = 25;
                        int MaskSize = 5;
                        float Sigma = 1;
                        bitmapResult = selectedSource.CannyEdgeDitection(TH, TL, MaskSize, Sigma);
                    }
                }
            }

            if (bitmapResult != null)
            {
                if (preview == true)
                {
                    picPreview.Image = bitmapResult;
                }
                else
                {
                    resultBitmap = bitmapResult;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            TimeSpan dt3 = new TimeSpan();
            dt1 = DateTime.Now;
            toolStripProgressBar1.Value = 0;
            ApplyFilter(true);
            dt2 = DateTime.Now;
            dt3 = dt2 - dt1;
            toolStripStatusLabel2.Text = dt3.ToString();
            toolStripProgressBar1.Value = 100;
        }

        private void indexChange(bool preview) {
            if (cmbEdgeDetection.SelectedIndex == -1)
            {
                return;
            }

            Bitmap selectedSource = null;
            Bitmap bitmapResult = null;

            if (preview == true)
            {
                selectedSource = previewBitmap;
            }
            else
            {
                selectedSource = originalBitmap;
            }
            if (cmbEdgeDetection.SelectedItem.ToString() == "None")
            {
                bitmapResult = selectedSource;
                ManageCannyOptions(false);
            }
            else if (cmbEdgeDetection.SelectedItem.ToString() == "Canny Edge Ditection")
            {
                ManageCannyOptions(true);
            }
            else {
                ManageCannyOptions(false);
            }
            if (bitmapResult != null)
            {
                if (preview == true)
                {
                    picPreview.Image = bitmapResult;
                }
                else
                {
                    resultBitmap = bitmapResult;
                }
            }
        }
        private void cmbEdgeDetection_SelectedIndexChanged(object sender, EventArgs e)
        {
            indexChange(true);
        }

        private void ManageCannyOptions(bool visibility) {
            label2.Visible = visibility;
            label3.Visible = visibility;
            label4.Visible = visibility;
            label5.Visible = visibility;
            label6.Visible = visibility;
            label7.Visible = visibility;
            textBox1.Visible = visibility;
            textBox2.Visible = visibility;
            textBox3.Visible = visibility;
            textBox4.Visible = visibility;
        }
    }
}
