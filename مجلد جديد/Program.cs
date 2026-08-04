using AlRowad_ERP;
using AlRowad_ERP.Forms;
using System;
using System.Windows.Forms;

namespace AlRowad_ERP.Core
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ًںڑ€ ظ…ظ†ط¹ ط§ط±طھط¯ط§ط¯ ط§ظ„ظ†ظˆط§ظپط° ظˆط§ظ‡طھط²ط§ط² ط§ظ„ط­ط¬ظ… ظپظٹ ط´ط§ط´ط§طھ ظˆظٹظ†ط¯ظˆط² ط§ظ„ط­ط¯ظٹط«ط©
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }

            // ًں› ï¸ڈ ط§ظ„طھط¹ط¯ظٹظ„ ط§ظ„ظ…ط¹ظ…ط§ط±ظٹ: ط¥ظ†ط´ط§ط، ط§ظ„ط´ط§ط´ط© ط§ظ„ط±ط¦ظٹط³ظٹط© ط£ظˆظ„ط§ظ‹ ط¨ط´ظƒظ„ ظ…ظ†ظپطµظ„
            MainForm frmMain = new MainForm();

            // ًں› ï¸ڈ طھظپط¹ظٹظ„ ط§ظ„ط±ط§ط¯ط§ط± ط§ظ„ط¹ط§ط¦ظ… ط¹ظ„ظ‰ ط§ظ„ط´ط§ط´ط© ط§ظ„ط±ط¦ظٹط³ظٹط© ظ„طھط¯ط®ظ„ ظپظٹ ظ†ط¸ط§ظ… ط§ظ„ط­طµط§ظ†ط© ط§ظ„ط±ظ‚ظ…ظٹط©
            FormMonitor.ط±ط§ظ‚ط¨_ط§ظ„ط´ط§ط´ط©(frmMain);

            // ًں› ï¸ڈ ط¹ط±ط¶ ط§ظ„ط´ط§ط´ط© ظˆطھط´ط؛ظٹظ„ ط§ظ„طھط·ط¨ظٹظ‚ ظپظٹ ظˆط¶ط¹ ط§ظ„ط°ط§ظƒط±ط© ط§ظ„ط­ط±ط© (ط¯ظˆظ† ظ‚ظٹظˆط¯ ط§ظ„ط£ط¨ ظˆط§ظ„ط§ط¨ظ†)
            frmMain.Show();
            Application.Run();
        }

        // ط§ط³طھط¯ط¹ط§ط، ط§ظ„ظ…ظ†ظ‡ط¬ ط§ظ„ط³ظٹط§ط¯ظٹ ظ…ظ† ط§ظ„ظˆظٹظ†ط¯ظˆط² ظ„ظ…ظ†ط¹ طھطµط§ط¯ظ… ط§ظ„ظ€ Scaling
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}