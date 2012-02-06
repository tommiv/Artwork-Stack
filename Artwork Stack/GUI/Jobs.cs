using System.Windows.Forms;

namespace Artwork_Stack.GUI
{
    public partial class Jobs : Form
    {
        public Jobs()
        {
            InitializeComponent();
        }

        private void gridJobs_DoubleClick(object sender, System.EventArgs e)
        {
            var dw = (DoWork)Application.OpenForms["DoWork"];
            dw.Navigate((int)gridJobs.SelectedRows[0].Cells[Fields.ID].Value);
        }
    }
}
