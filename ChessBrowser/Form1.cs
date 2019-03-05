using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessBrowser
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void label1_Click(object sender, EventArgs e)
    {

    }

    private void button1_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFD = new OpenFileDialog();

      openFD.InitialDirectory = "c:\\";
      openFD.Filter = "pgn files|*.pgn";
      openFD.RestoreDirectory = true;

      if (openFD.ShowDialog() == DialogResult.OK)
      {
        Console.WriteLine("uploading");
        DisableControls();
        uploadProgress.Value = 0;
        backgroundWorker1.RunWorkerAsync(openFD.FileName);
      }
    }

    private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {
      UploadGamesToDatabase((string)e.Argument);
    }

    private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      uploadProgress.Value = 0;
      EnableControls();
    }

    private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      uploadProgress.PerformStep();
    }

    private void whiteWin_CheckedChanged(object sender, EventArgs e)
    {
      winnerButton = sender as RadioButton;
    }

    private void blackWin_CheckedChanged(object sender, EventArgs e)
    {
      winnerButton = sender as RadioButton;
    }

    private void drawWin_CheckedChanged(object sender, EventArgs e)
    {
      winnerButton = sender as RadioButton;
    }

    private void dateCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      startDate.Enabled = dateCheckBox.Checked;
      endDate.Enabled = dateCheckBox.Checked;
    }

    private void Form1_Load(object sender, EventArgs e)
    {

    }

    private void button1_Click_1(object sender, EventArgs e)
    {
      if (whitePlayerText.Text == "" && blackPlayerText.Text == "" && openingMoveText.Text == ""
        && winnerButton == null && !dateCheckBox.Checked)
      {
        MessageBox.Show("Please select at least one filter");
        return;
      }

      DisableControls();
      string result = PerformQuery(whitePlayerText.Text, blackPlayerText.Text, openingMoveText.Text, 
        winnerButton != null ? winnerButton.Text : "",
        dateCheckBox.Checked, startDate.Value, endDate.Value,
        showMovesCheckBox.Checked);
      resultText.Text = result;
      EnableControls();
    }

    private void radioButton1_CheckedChanged(object sender, EventArgs e)
    {
      winnerButton = null;
    }
  }
}
