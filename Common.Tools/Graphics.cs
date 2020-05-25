using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Ujeby.Common.Tools
{
	public class Graphics
	{
		private static Bitmap BlurImage(Bitmap original)
		{
			var blurSize = 9;
			var rectangle = new Rectangle(0, 0, original.Width, original.Height);
			var blurred = new Bitmap(original.Width, original.Height);

			// make an exact copy of the bitmap provided
			using (var graphics = System.Drawing.Graphics.FromImage(blurred))
				graphics.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
					new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);

			// look at every pixel in the blur rectangle
			for (Int32 xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
			{
				for (Int32 yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
				{
					Int32 avgR = 0, avgG = 0, avgB = 0;
					Int32 blurPixelCount = 0;

					// average the color of the red, green and blue for each pixel in the
					// blur size while making sure you don't go outside the image bounds
					for (Int32 x = xx; (x < xx + blurSize && x < original.Width); x++)
					{
						for (Int32 y = yy; (y < yy + blurSize && y < original.Height); y++)
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

					// now that we know the average for the blur size, set each pixel to that color
					for (Int32 x = xx; x < xx + blurSize && x < original.Width && x < rectangle.Width; x++)
						for (Int32 y = yy; y < yy + blurSize && y < original.Height && y < rectangle.Height; y++)
							blurred.SetPixel(x, y, Color.FromArgb(avgR, avgG, avgB));
				}
			}

			return blurred;
		}

		public static Image MakeGrayscale3(Image original)
		{
			if (original == null)
				return null;

			//create a blank bitmap the same size as original
			var newBitmap = new Bitmap(original.Width, original.Height);

			// get a graphics object from the new image
			var g = System.Drawing.Graphics.FromImage(newBitmap);

			// create the grayscale ColorMatrix
			var colorMatrix = new ColorMatrix(
				new float[][]
				{
					new float[] {.3f, .3f, .3f, 0, 0},
					new float[] {.59f, .59f, .59f, 0, 0},
					new float[] {.11f, .11f, .11f, 0, 0},
					new float[] {0, 0, 0, 1, 0},
					new float[] {0, 0, 0, 0, 1}
				});

			// create some image attributes
			var attributes = new ImageAttributes();

			// set the color matrix attribute
			attributes.SetColorMatrix(colorMatrix);

			// draw the original image on the new image
			// using the grayscale color matrix
			g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

			// dispose the Graphics object
			g.Dispose();

			// dispose old image
			original.Dispose();

			return newBitmap;
		}

		public static ImageFormat GetImageFormat(string extension)
		{
			extension = extension.Replace(".", string.Empty).ToLower();

			if (extension.Contains("?"))
				extension = extension.Substring(0, extension.IndexOf('?'));

			if (extension == "png")
				return ImageFormat.Png;
			else if (extension == "jpg")
				return ImageFormat.Jpeg;
			else if (extension == "jpeg")
				return ImageFormat.Jpeg;
			else if (extension == "gif")
				return ImageFormat.Gif;

			throw new NotImplementedException($"unknown image extension: { extension }");
		}

		public static string GetImageExtension(ImageFormat imageFormat)
		{
			if (imageFormat == ImageFormat.Jpeg)
				return "jpg";
			else if (imageFormat == ImageFormat.Png)
				return "png";
			else if (imageFormat == ImageFormat.Gif)
				return "gif";
			else if (imageFormat == ImageFormat.Bmp)
				return "bmp";

			throw new NotImplementedException($"unknown imageFormat { imageFormat.ToString() }");
		}
	}
}
