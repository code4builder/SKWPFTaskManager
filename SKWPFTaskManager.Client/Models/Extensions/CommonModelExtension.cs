using SKWPFTaskManager.Common.Models;
using System.IO;
using System.Windows.Media.Imaging;

namespace SKWPFTaskManager.Client.Models.Extensions
{
    public static class CommonModelExtension
    {
        public static BitmapImage LoadImage(this CommonModel model)
        {
            if (model?.Photo == null || model.Photo.Length == 0)
                return null;

            var image = new BitmapImage();
            using (var memStream = new MemoryStream(model.Photo))
            {
                memStream.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = memStream;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
