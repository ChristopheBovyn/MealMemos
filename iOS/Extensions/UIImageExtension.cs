using System;
using CoreGraphics;
using UIKit;

namespace UIImageExtension
{
    public static class UIImageExtension
    {
        public static UIImage ResizeImage(this UIImage image, float scale)
        {
            var imageSize = image.Size;
            var scaledSize = new CGSize(width: imageSize.Width * scale, height: imageSize.Height * scale);


            return Resize(image, scaledSize);
        }

        public static UIImage Resize(this UIImage image, CGSize targetSize)
        {
            var size = image.Size;
            var widthRatio = targetSize.Width / image.Size.Width;
            var heightRatio = targetSize.Height / image.Size.Height;
            CGSize newSize = new CGSize();
            if (widthRatio > heightRatio)
            {
                newSize = new CGSize(width: size.Width * heightRatio, height: size.Height * heightRatio);
            }
            else
            {
                newSize = new CGSize(width: size.Width * widthRatio, height: size.Height * widthRatio);
            }

            return ResizeByWidthHeight(image, newSize);
        }

        public static UIImage ResizeByWidthHeight(this UIImage image, CGSize targetSize)
        {
            var rect = new CGRect(x: 0, y: 0, width: targetSize.Width, height: targetSize.Height);
            UIGraphics.BeginImageContextWithOptions(targetSize, false, 0);
            image.Draw(rect);
            var newImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return newImage;
        }
    }
}
