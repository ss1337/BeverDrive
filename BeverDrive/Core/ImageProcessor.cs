using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using BeverDrive.Core.Extensions;

namespace BeverDrive.Core
{
	public class ImageProcessor
	{
		public Image Original { get; set; }
		public List<Image> ProcessedImages { get; private set; }
		public bool ProcessComplete { get; private set; }

		private Thread procThread;

		public ImageProcessor()
		{
		}

		public ImageProcessor(string filename)
		{
			this.Original = Image.FromFile(filename);
			this.ProcessComplete = false;
		}

		public ImageProcessor(Image image)
		{
			this.Original = image;
			this.ProcessComplete = false;
		}

		public void NewOriginal(string filename)
		{
			this.Original = Image.FromFile(filename);
			this.ProcessComplete = false;
		}

		public void NewOriginal(Image image)
		{
			this.Original = image;
			this.ProcessComplete = false;
		}

		/// <summary>
		/// Produces a list of blurred images, fading from startAlpha to endAlpha in steps
		/// </summary>
		/// <param name="blurSize">Size of blur window</param>
		/// <param name="startAlpha">Initial alpha value</param>
		/// <param name="endAlpha">Final alpha value</param>
		/// <param name="steps">Number of images</param>
		public void BlurAndFade(int blurSize, float startAlpha, float endAlpha, float steps, Color backgroundColor)
		{
			this.ProcessedImages = new List<Image>();
			this.ProcessComplete = false;

			procThread = new Thread(new ThreadStart(() =>
			{
				// Blur first...
				Image blurred = this.BlurImage(this.Original, blurSize);

				// Then fade
				float stepAlpha = 0.0f;
				if (steps > 1)
					stepAlpha = (endAlpha - startAlpha) / (float)(steps - 1);

				for (int i = 0; i < steps; i++)
					this.ProcessedImages.Add(this.FadeImage(blurred, startAlpha + stepAlpha * i, backgroundColor));

				this.ProcessComplete = true;
			}));

			procThread.Start();
		}

		/// <summary>
		/// Crops the image in this.Original to fit scaled in the specified size
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void CropToSize(int width, int height)
		{
			Rectangle destRect = new Rectangle(0, 0, width, height);
			Rectangle srcRect = this.Original.CalculateScaling(width, height);

			// Crop image
			Bitmap result = new Bitmap(width, height);
			using (var graphics = Graphics.FromImage(result))
				graphics.DrawImage(this.Original, destRect, srcRect, GraphicsUnit.Pixel);

			this.Original = result;
		}

		/// <summary>
		/// Produces a list of images, fading from startAlpha to endAlpha in steps
		/// </summary>
		/// <param name="startAlpha">Initial alpha value</param>
		/// <param name="endAlpha">Final alpha value</param>
		/// <param name="steps">Number of images</param>
		public void Fade(float startAlpha, float endAlpha, float steps, Color backgroundColor)
		{
			this.ProcessedImages = new List<Image>();
			this.ProcessComplete = false;

			procThread = new Thread(new ThreadStart(() =>
			{
				float stepAlpha = (endAlpha - startAlpha) / (float)(steps - 1);

				for (int i = 0; i < steps; i++)
					this.ProcessedImages.Add(this.FadeImage(this.Original, startAlpha + stepAlpha * i, backgroundColor));

				this.ProcessComplete = true;
			}));

			procThread.Start();
		}

		/// <summary>
		/// Blur filter adapted from http://notes.ericwillis.com/2009/10/blur-an-image-with-csharp/
		/// </summary>
		/// <param name="original">Original image</param>
		/// <param name="blurSize">Size of blur window</param>
		/// <returns></returns>
		public Image BlurImage(Image original, int blurSize)
		{
			Bitmap blurred = new Bitmap(original.Width, original.Height);
 
			// Make an exact copy of the bitmap provided
			using(Graphics graphics = Graphics.FromImage(blurred))
				graphics.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
					new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
 
			// Look at every pixel in the blur rectangle
			for (int xx = 0; xx < blurred.Width; xx++)
			{
				for (int yy = 0; yy < original.Height; yy++)
				{
					int avgR = 0, avgG = 0, avgB = 0;
					int blurPixelCount = 0;
		 
					// Average the color of the red, green and blue for each pixel in the
					// blur size while making sure you don't go outside the image bounds
					for (int x = xx; (x < xx + blurSize && x < blurred.Width); x++)
					{
						for (int y = yy; (y < yy + blurSize && y < blurred.Height); y++)
						{
							Color pixel = blurred.GetPixel(x, y);
		 
							avgR += pixel.R;
							avgG += pixel.G;
							avgB += pixel.B;
		 
							blurPixelCount++;
						}
					}
		 
					avgR = avgR / blurPixelCount;
					avgG = avgG / blurPixelCount;
					avgB = avgB / blurPixelCount;
		 
					// Now that we know the average for the blur size, set each pixel to that color
					for (int x = xx; x < xx + blurSize && x < blurred.Width; x++)
						for (int y = yy; y < yy + blurSize && y < blurred.Height; y++)
							blurred.SetPixel(x, y, Color.FromArgb(avgR, avgG, avgB));
				}
			}
		 
			return blurred;
		}

		/// <summary>
		/// Fades an image
		/// </summary>
		/// <param name="alpha">Alpha value from 0.0 to 1.0</param>
		public Image FadeImage(Image original, float alpha, Color backgroundColor)
		{
			// Initialize the color matrix.
			// Note the value 0.8 in row 4, column 4.
			float[][] matrixItems ={ 
				new float[] {1, 0, 0, 0, 0},
				new float[] {0, 1, 0, 0, 0},
				new float[] {0, 0, 1, 0, 0},
				new float[] {0, 0, 0, alpha, 0}, 
				new float[] {0, 0, 0, 0, 1}}; 

			ColorMatrix colorMatrix = new ColorMatrix(matrixItems);

			// Create an ImageAttributes object and set its color matrix.
			ImageAttributes imageAtt = new ImageAttributes();
			imageAtt.SetColorMatrix(
				colorMatrix,
				ColorMatrixFlag.Default,
				ColorAdjustType.Bitmap);

			// Now draw the semitransparent bitmap image.
			int iWidth = original.Width;
			int iHeight = original.Height;

			Bitmap res = new Bitmap(iWidth, iHeight);
			using(var graphic = Graphics.FromImage(res))
			{
				graphic.Clear(backgroundColor);
				graphic.DrawImage(
					original, 
					new Rectangle(0, 0, iWidth, iHeight),   // destination rectangle
					0.0f,                                   // source rectangle x 
					0.0f,                                   // source rectangle y
					iWidth,                                 // source rectangle width
					iHeight,                                // source rectangle height
					GraphicsUnit.Pixel, 
					imageAtt
				);
			}
			return res;
		}
	}
}
