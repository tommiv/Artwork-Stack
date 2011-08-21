using System.Windows.Forms;

namespace Artwork_Stack
{
    public partial class formJobs : Form
    {
        public formJobs()
        {
            InitializeComponent();
        }

        private void gridJobs_DoubleClick(object sender, System.EventArgs e)
        {
            var dw = (formDoWork)Application.OpenForms["formDoWork"];
            dw.Navigate((int)gridJobs.SelectedRows[0].Cells["id"].Value);
        }
    }
}
