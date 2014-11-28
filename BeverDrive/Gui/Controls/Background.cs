using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using BeverDrive.Core.Extensions;

namespace BeverDrive.Gui.Controls
{
	public class BackgroundImage
	{
		public string Name { get; set; }
		public Image Image { get; set; }
		public BackgroundImage(string name, Image image)
		{
			this.Name = name;
			this.Image = image;
		}
	}

	/// <summary>
	/// Class for the background, with support for superslow fading of images
	/// </summary>
	public class Background
	{
		private const int MAXALPHA = 6;
		private string name;
		private Image image;
		private Rectangle srcRect;

		public int Alpha { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		/// <summary>
		/// Draws the background, with or without background image
		/// </summary>
		/// <param name="graphic"></param>
		/// <param name="backColor"></param>
		public void Draw(Graphics graphic, Color backColor)
		{
			graphic.Clear(backColor);
			// TODO: Remove this to have superslow backgrounds
			return;
			
			if (image == null || this.Alpha == 0)
			{
			}
			else
			{
				float beta = (float)Math.Abs(this.Alpha) / 10f;
				//graphic.DrawImage(this.image, new Rectangle(0, 0, this.Width, this.Height), srcRect, GraphicsUnit.Pixel);
				graphic.DrawImageAlphaFaded(this.image, new Rectangle(0, 0, this.Width, this.Height), srcRect, beta);
			}

			// Fade in/out background
			if (this.Alpha < MAXALPHA)
			{
				this.Alpha += 1;
				if (this.Alpha == 0)
					this.image = null;
			}

			return;
		}

		/// <summary>
		/// Use this to set a background image, if the name is the same as the previous images name
		/// nothing happens to prevent stupid fades of the same image
		/// </summary>
		/// <param name="backgroundName"></param>
		/// <param name="backgroundImage"></param>
		public void SetBackgroundImage(string backgroundName, Image backgroundImage)
		{
			if (backgroundImage == null)
			{
				// Fade out background image if it exists, from -6 to 0
				// and only if we are in fade in
				if (image != null && this.Alpha > 0)
					this.Alpha = -MAXALPHA;
			}
			else
			{
				if (this.image == null || this.name != backgroundName)
				{
					name = backgroundName;
					image = backgroundImage;
					this.Alpha = 0;

					// Calculate a source rectangle for the image just set
					if (image != null)
						this.srcRect = image.CalculateScaling(this.Width, this.Height);

					// Render prefaded images for slow speedup (!!)
					//backgroundFadedImages = this.PreFadeImage(backgroundImage.CoverImage);
				}
			}
		}

		/// <summary>
		/// Calculates prefaded images
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		private List<Image> PreFadeImage(Image image, Color backColor)
		{
			int i = 1;
			var result = new List<Image>();
			var destRect = new Rectangle(0, 0, this.Width, this.Height);
			var srcRect = image.CalculateScaling(this.Width, this.Height);

			// Add completely opaque background as the first image
			var first = new Bitmap(this.Width, this.Height);
			using (var g = Graphics.FromImage(first))
			{
				g.Clear(backColor);
			}

			result.Add(first);

			// Calculate all the different fades and add them to the list
			while (i < MAXALPHA + 1)
			{
				var img = new Bitmap(this.Width, this.Height);

				// Initialize the color matrix. 
				// Note the value 0.8 in row 4, column 4. 
				float[][] matrixItems ={ 
				new float[] {1, 0, 0, 0, 0},
				new float[] {0, 1, 0, 0, 0},
				new float[] {0, 0, 1, 0, 0},
				new float[] {0, 0, 0, (float)((float)i / 10f), 0}, 
				new float[] {0, 0, 0, 0, 1}};
				ColorMatrix colorMatrix = new ColorMatrix(matrixItems);

				// Create an ImageAttributes object and set its color matrix.
				ImageAttributes imageAtt = new ImageAttributes();
				imageAtt.SetColorMatrix(
					colorMatrix,
					ColorMatrixFlag.Default,
					ColorAdjustType.Bitmap);

				// Now draw the semitransparent bitmap image. 
				using (var graphic = Graphics.FromImage(img))
				{
					graphic.DrawImage(
						image,
						destRect,
						srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height,
						GraphicsUnit.Pixel,
						imageAtt);
				}

				result.Add(img);
				i++;
			}

			return result;
		}
	}
}
