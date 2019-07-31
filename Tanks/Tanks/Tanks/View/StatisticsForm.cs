using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tanks.Model;

namespace Tanks.View
{
    public partial class StatisticsForm : Form
    {

        public static bool isClosed = true;
        public StatisticsForm(BindingList<Log> logs)
        {
            InitializeComponent();
            dgvLog.DataSource = logs;
            isClosed = false;
        }

        public void RefreshDgv(BindingList<Log> logs)
        {
            dgvLog.DataSource = null;
            dgvLog.DataSource = logs;
        }

        private void StatisticsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            isClosed = true;
        }
    }
}
