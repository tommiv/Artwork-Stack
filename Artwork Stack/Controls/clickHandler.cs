using System.Windows.Forms;

namespace Artwork_Stack.Controls
{
    public class clickHandler : Label
    {
        public clickHandler() { this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, false); }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;  // Turn on WS_EX_TRANSPARENT
                return cp;
            }
        }
        protected override void OnPaintBackground(PaintEventArgs e) { /*Prevent erasing background*/ }
    }
}
