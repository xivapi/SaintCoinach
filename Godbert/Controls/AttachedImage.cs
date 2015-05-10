using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Godbert.Controls {
    public static class AttachedImage {
        public static readonly DependencyProperty ImageProperty = DependencyProperty.RegisterAttached("Image",
            typeof(SaintCoinach.Imaging.ImageFile), typeof(AttachedImage), new PropertyMetadata(AttachedImageChanged));

        public static readonly DependencyProperty ImageTypeProperty = DependencyProperty.RegisterAttached("ImageType",
            typeof(string), typeof(AttachedImage), new PropertyMetadata(AttachedImageChanged));

        public static readonly DependencyProperty UseImageTypeProperty = DependencyProperty.RegisterAttached("UseImageType",
            typeof(bool), typeof(AttachedImage), new PropertyMetadata(true, AttachedImageChanged));

        public static readonly DependencyProperty IsSizeRestrictedProperty = DependencyProperty.RegisterAttached("IsSizeRestricted",
            typeof(bool?), typeof(AttachedImage), new PropertyMetadata(null, AttachedImageChanged));

        private static readonly Dictionary<string, WeakReference<ImageSource>> _SourceCache =
            new Dictionary<string, WeakReference<ImageSource>>();

        public static bool GetUseImageType(DependencyObject o) {
            return (bool)o.GetValue(UseImageTypeProperty);
        }
        public static void SetUseImageType(DependencyObject o, bool newValue) {
            o.SetValue(UseImageTypeProperty, newValue);
        }

        public static bool? GetIsSizeRestricted(DependencyObject o) {
            return (bool?)o.GetValue(IsSizeRestrictedProperty);
        }

        public static void SetIsSizeRestricted(DependencyObject o, bool? newValue) {
            o.SetValue(IsSizeRestrictedProperty, newValue);
        }

        public static SaintCoinach.Imaging.ImageFile GetImage(DependencyObject o) {
            return (SaintCoinach.Imaging.ImageFile)o.GetValue(ImageProperty);
        }

        public static void SetImage(DependencyObject o, SaintCoinach.Imaging.ImageFile v) {
            o.SetValue(ImageProperty, v);
        }

        public static string GetImageType(DependencyObject o) {
            return (string)o.GetValue(ImageTypeProperty);
        }

        public static void SetImageType(DependencyObject o, string v) {
            o.SetValue(ImageTypeProperty, v);
        }

        private static void AttachedImageChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
            var img = (Image)o;

            var srcImage = GetImage(o);
            var srcImageType = GetImageType(o);
            var useSrcImageType = GetUseImageType(o);
            if (!useSrcImageType)
                srcImageType = null;

            double w = double.PositiveInfinity, h = double.PositiveInfinity;

            if (srcImage != null) {
                var useImage = GetImageOfType(srcImage, srcImageType);

                w = useImage.Width;
                h = useImage.Height;

                img.Source = GetAsImageSource(useImage);
            } else
                img.Source = null;

            var sizeRestrict = GetIsSizeRestricted(o);
            if (sizeRestrict.HasValue) {
                if (sizeRestrict.Value == false)
                    w = h = double.PositiveInfinity;

                img.MaxWidth = w;
                img.MaxHeight = h;
            }
        }

        private static ImageSource GetAsImageSource(SaintCoinach.Imaging.ImageFile file) {
            if (file == null)
                return null;

            var key = file.Path;
            WeakReference<ImageSource> targetRef;
            ImageSource target;
            if (!_SourceCache.TryGetValue(key, out targetRef) || !targetRef.TryGetTarget(out target)) {
                target = CreateSource(file);
                if (targetRef == null)
                    _SourceCache.Add(key, new WeakReference<ImageSource>(target));
                else
                    targetRef.SetTarget(target);
            }

            return target;
        }

        private static SaintCoinach.Imaging.ImageFile GetImageOfType(SaintCoinach.Imaging.ImageFile original,
                                                                     string type) {
            if (string.IsNullOrWhiteSpace(type))
                return original;
            if (!original.Path.StartsWith("ui/icon/"))
                return original; // XXX: Exception instead?

            var dirPath = original.Path.Substring(0, "ui/icon/000000/".Length);
            var targetPath = string.Format("{0}{1}/{2}", dirPath, type, original.Path.Split('/').Last());

            SaintCoinach.IO.File typeFile;
            if (original.Pack.Collection.TryGetFile(targetPath, out typeFile))
                return (SaintCoinach.Imaging.ImageFile)typeFile;
            return original;
        }

        private static ImageSource CreateSource(SaintCoinach.Imaging.ImageFile file) {
            var argb = SaintCoinach.Imaging.ImageConverter.GetA8R8G8B8(file);
            return BitmapSource.Create(
                                       file.Width, file.Height,
                96, 96,
                PixelFormats.Bgra32, null,
                argb, file.Width * 4);
        }
    }
}
