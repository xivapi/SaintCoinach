using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Thaliak.Modules.Core.Data {
    public class AttachedImage : FrameworkElement {
        public static readonly DependencyProperty ImageProperty = DependencyProperty.RegisterAttached("Image", typeof(SaintCoinach.Imaging.ImageFile), typeof(AttachedImage), new PropertyMetadata(AttachedImageChanged));
        public static readonly DependencyProperty ImageTypeProperty = DependencyProperty.RegisterAttached("ImageType", typeof(string), typeof(AttachedImage), new PropertyMetadata(AttachedImageChanged));

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

        private static Dictionary<Tuple<uint, uint>, WeakReference<ImageSource>> _SourceCache = new Dictionary<Tuple<uint,uint>,WeakReference<ImageSource>>();
        private static void AttachedImageChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
            var img = (Image)o;

            var srcImage = GetImage(o);
            if (srcImage != null) {
                var srcImageType = GetImageType(o);
                var useImage = GetImageOfType(srcImage, srcImageType);

                var key = Tuple.Create(useImage.Index.DirectoryKey, useImage.Index.FileKey);
                WeakReference<ImageSource> targetRef;
                ImageSource target;
                if (!_SourceCache.TryGetValue(key, out targetRef) || !targetRef.TryGetTarget(out target)) {
                    target = CreateSource(useImage);
                    if (targetRef == null)
                        _SourceCache.Add(key, new WeakReference<ImageSource>(target));
                    else
                        targetRef.SetTarget(target);
                }

                img.Source = target;
            } else
                img.Source = null;
            /*
            var img = (Image)o;
            var newImg = e.NewValue as SaintCoinach.Imaging.ImageFile;
            if (newImg != null) {
                var key = Tuple.Create(newImg.Index.DirectoryKey, newImg.Index.FileKey);
                WeakReference<ImageSource> srcRef;
                ImageSource src;
                if (!_SourceCache.TryGetValue(key, out srcRef) || !srcRef.TryGetTarget(out src)) {
                    src = CreateSource(newImg);
                    if (srcRef == null)
                        _SourceCache.Add(key, new WeakReference<ImageSource>(src));
                    else
                        srcRef.SetTarget(src);
                }
                img.Source = src;
            } else
                img.Source = null;
             * */
        }
        private static SaintCoinach.Imaging.ImageFile GetImageOfType(SaintCoinach.Imaging.ImageFile original, string type) {
            if (string.IsNullOrWhiteSpace(type))
                return original;
            if (!original.Path.StartsWith("ui/icon/"))
                return original;    // XXX: Exception instead?

            var dirPath = original.Path.Substring(0, "ui/icon/000000/".Length);
            var targetPath = string.Format("{0}{1}/{2}", dirPath, type, original.Name);

            SaintCoinach.IO.File typeFile;
            if (original.Directory.Pack.TryGetFile(targetPath, out typeFile))
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
