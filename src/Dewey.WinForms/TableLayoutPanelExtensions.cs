using System.Windows.Forms;

namespace Dewey.WinForms
{
    public static class TableLayoutPanelExtensions
    {
        public static void FixPadding(this TableLayoutPanel value, int paddingLeft, int paddingRight, bool leaveSpaceForScrollBar)
        {
            if (value.VerticalScroll.Visible) {
                value.Padding = new Padding(paddingLeft, 0, paddingRight + SystemInformation.VerticalScrollBarWidth, 0);
                value.Padding = new Padding(paddingLeft, 0, paddingRight, 0);
            } else {
                if (leaveSpaceForScrollBar) {
                    value.Padding = new Padding(paddingLeft, 0, paddingRight + SystemInformation.VerticalScrollBarWidth, 0);
                } else {
                    value.Padding = new Padding(paddingLeft, 0, paddingRight, 0);
                }
            }
        }
    }
}
