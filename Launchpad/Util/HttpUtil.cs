using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Launchpad.Util
{
    public static class HttpUtil
    {
        public static async Task<Image> StreamUrlToImage(string imageUrl)
        {
            var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("");
            var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
            return Image.FromStream(new MemoryStream(imageBytes));
        }
        
        public static async Task<Image> StreamUrlToImageAndResize(string imageUrl, int imageWidth, int imageHeight)
        {
            var httpClient = new HttpClient();
            var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
            return Image.FromStream(new MemoryStream(ImageUtil.ResizeImageFromStream(imageBytes, imageWidth, imageHeight)));
        }
    }
}