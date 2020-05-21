using System;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Oddity;

namespace Launchpad.Forms
{
    public partial class InitialForm : Form
    {
        private static async Task GetAllMissions()
        {
            await Task.Run(() =>
            {
                var appData = new HttpClient().GetAsync("https://launchpadx.herokuapp.com/api/update").Result;
                if (appData.StatusCode == HttpStatusCode.OK)
                {
                    var json = JObject.Parse(appData.Content.ReadAsStringAsync().Result);
                    var newAppVer = (string)json.SelectToken("currentVersion");
                    var currentAppVer = Application.ProductVersion.Remove(3);
                    if (newAppVer != currentAppVer)
                    {
                        MessageBox.Show($"New version is available and ready to use!\nFor more information please visit https://github.com/skyffx/Launchpad" +
                                        $"\n\nYour current version: {currentAppVer}\nNew version: {newAppVer}",
                            "—Launchpad—", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                //
                var response = new HttpClient().GetAsync("https://api.spacexdata.com/v3/").Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    try
                    {
                        var missionsData = new OddityCore().Launches.GetAll().Execute();
                        var appFormThread = new Thread(() => new MainForm(missionsData).ShowDialog());
                        appFormThread.SetApartmentState(ApartmentState.STA);
                        appFormThread.Start();
                        Application.Exit();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show($"SpaceX, we have a problem!\n\nhttps://api.spacexdata.com/v3/launches\n" +
                                        $"=> Request is not completed!\n\nPlease to run app again.",
                            "—Launchpad—", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }
                else
                {
                    MessageBox.Show($"SpaceX, we have a problem!\nHttpStatusCode: {response.StatusCode.ToString()}",
                        "—Launchpad—", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            });
        }
        
        //
        
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x00080000; // Required: set WS_EX_LAYERED extended style
                return cp;
            }
        }

        //Updates the Form's display using API calls
        private void UpdateFormDisplay(Image backgroundImage)
        {
            var screenDc = API.GetDC(IntPtr.Zero);
            var memDc = API.CreateCompatibleDC(screenDc);
            var hBitmap = IntPtr.Zero;
            var oldBitmap = IntPtr.Zero;

            try
            {
                //Display-image
                var bmp = new Bitmap(backgroundImage);
                hBitmap = bmp.GetHbitmap(Color.FromArgb(0));  //Set the fact that background is transparent
                oldBitmap = API.SelectObject(memDc, hBitmap);

                //Display-rectangle
                var size = bmp.Size;
                var pointSource = new Point(0, 0);
                var topPos = new Point(this.Left, this.Top);

                //Set up blending options
                var blend = new API.BLENDFUNCTION
                {
                    BlendOp = API.AC_SRC_OVER,
                    BlendFlags = 0,
                    SourceConstantAlpha = 255,
                    AlphaFormat = API.AC_SRC_ALPHA
                };

                API.UpdateLayeredWindow(this.Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, API.ULW_ALPHA);

                //Clean-up
                bmp.Dispose();
                API.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    API.SelectObject(memDc, oldBitmap);
                    API.DeleteObject(hBitmap);
                }
                API.DeleteDC(memDc);
            }
            catch (Exception)
            {
                // ignored
            }
        }
        
        private void AppLoad(object sender, EventArgs e)
        {
            UpdateFormDisplay(BackgroundImage);
        }

        //
        
        public InitialForm()
        {
            InitializeComponent();
            Task.Run(GetAllMissions);
        }
    }
    
    internal class API
    {
        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;
        public const Int32 ULW_ALPHA = 0x00000002;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize,
            IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);
        
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        
        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);
        
        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}