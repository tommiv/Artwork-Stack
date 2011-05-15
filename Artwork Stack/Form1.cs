using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using fastJSON;

namespace Artwork_Stack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            const string query  = "deftones - adrenaline";
            WebRequest req = WebRequest.Create("https://ajax.googleapis.com/ajax/services/search/images?v=1.0&q=" + query);
            Stream resp = req.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(resp);
            var response = fastJSON.JSON.Instance.Parse(reader.ReadToEnd());
        }
    }
}
