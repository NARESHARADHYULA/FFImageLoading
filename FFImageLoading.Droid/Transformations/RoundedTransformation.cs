﻿using System;
using FFImageLoading.Work;
using Android.Graphics;

namespace FFImageLoading.Transformations
{
	public class RoundedTransformation : TransformationBase
	{
		private double _radius;

		public RoundedTransformation(double radius)
		{
			_radius = radius;
		}

		public override void SetParameters(object[] parameters)
		{
			_radius = (double)parameters[0];
		}

		public override string Key
		{
			get { return string.Format("RoundedTransformation, radius = {0}", _radius); }
		}
			
		protected override Bitmap Transform(Bitmap source)
		{
			try
			{
				var transformed = ToRounded(source, (float)_radius);
				return transformed;
			}
			finally
			{
				source.Recycle();
			}
		}

		public static Bitmap ToRounded(Bitmap source, float rad)
		{
			int size = Math.Min(source.Width, source.Height);

			int width = (source.Width - size) / 2;
			int height = (source.Height - size) / 2;

			Bitmap bitmap = Bitmap.CreateBitmap(size, size, Bitmap.Config.Argb8888);

			using (Canvas canvas = new Canvas(bitmap))
			using (Paint paint = new Paint())
			using (BitmapShader shader = new BitmapShader(source, Shader.TileMode.Clamp, Shader.TileMode.Clamp))
			using (Matrix matrix = new Matrix())
			{
				if (width != 0 || height != 0)
				{
					// source isn't square, move viewport to centre
					matrix.SetTranslate(-width, -height);
					shader.SetLocalMatrix(matrix);
				}
				paint.SetShader(shader);
				paint.AntiAlias = true;

				canvas.DrawCircle(rad, rad, rad, paint);

				return bitmap;				
			}
		}
	}
}

