namespace PlayerUnknown.Leaker
{
    using System;
    using System.Windows.Forms;

    public partial class HLeakerGui : Form
    {
        private IntPtr HProcess = IntPtr.Zero;

        public HLeakerGui(IntPtr HProcess)
        {
            this.InitializeComponent();
            this.HProcess = HProcess;
        }

        private void HLeakerGuiLoad(object Sender, EventArgs E)
        {
        }
    }
}