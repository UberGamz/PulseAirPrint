using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PulseAirPrint
{
    public partial class PulseAirPrintForm : Form
    {

        public PulseAirPrintForm()
        {
            InitializeComponent();
        }

        private void PulseAirPrintFormed_Load(object sender, EventArgs e)
        {

        }

        private void okBTN_Click(object sender, EventArgs e)
        {
            double.Parse(drillSizeBox.Text);
            double.Parse(rotaryPDBox.Text);
            double.Parse(nGearToInsideDim.Text);
            double.Parse(nGearToOutsideDim.Text);
            double.Parse(insideDim.Text);
            double.Parse(gearToInsideDim.Text);
            double.Parse(gearToOutsideDim.Text);

            if (
                paSide.Text != null &&
                drillSizeBox.Text != null &&
                thruBox.Text != null &&
                rotaryPDBox.Text != null &&
                nGearToInsideDim.Text != null &&
                nGearToOutsideDim.Text != null &&
                insideDim.Text != null &&
                gearToInsideDim.Text != null &&
                gearToOutsideDim.Text != null
                ) {
            this.Close();
            }
        }
    }
}
