using System;
using System.Windows.Forms;

namespace AlRowad_ERP.Core // ًں› ï¸ڈ ط§ظ„ظ†ط·ط§ظ‚ ط§ظ„طµط­ظٹط­ ظˆط§ظ„ظ…ط¹طھظ…ط¯ ط¯ط§ط®ظ„ ظ…ط¬ظ„ط¯ ط§ظ„ط£ط³ط§ط³ظٹط§طھ
{
    public static class FormMonitor
    {
        public static void ط±ط§ظ‚ط¨_ط§ظ„ط´ط§ط´ط©(Form frm)
        {
            if (frm == null) return;

            frm.FormClosed += (sender, e) =>
            {
                // ط¥ط°ط§ ط£ط؛ظ„ظ‚طھ ط¢ط®ط± ط´ط§ط´ط© ظپظٹ ط§ظ„ط°ط§ظƒط±ط© ظٹظ…ظˆطھ ط§ظ„ط¨ط±ظ†ط§ظ…ط¬ ظپظˆط±ط§ظ‹ ظˆظٹظ†طھظ‡ظٹ طھط¹ظ„ظٹظ‚ ط§ظ„ظپظٹط¬ظˆط§ظ„ ط³طھظˆط¯ظٹظˆ
                if (Application.OpenForms.Count == 0)
                {
                    Application.Exit();
                }
            };
        }
    }
}