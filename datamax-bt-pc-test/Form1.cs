﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test_BT
{
    public partial class Form1 : Form
    {
        int iRx = 0;
        int iTx = 0;

        delegate void SetTxtLogCallback(string text);

        public Form1()
        {
            InitializeComponent();
        }

        private void btnAlive_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort.Write((char)1 + "F\r");
                SetTxtLog("Sent status " + iTx + "\r\n");
            }
            catch (Exception)
            {
                SetTxtLog("Error Tx" + iTx + "\r\n");
            }
            finally
            {
                iTx++;
            }
        }

        private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            while (serialPort.BytesToRead > 0)
            {
                if (serialPort.ReadChar() != 13)
                {
                    SetTxtLogCallback t = new SetTxtLogCallback(SetTxtLog);
                    this.Invoke(t, new object[] { "Ack " + (iRx++) + "\r\n" });
                }
            }
        }

        private void SetTxtLog(string text)
        {
            txtLog.Text += text;
            txtLog.Select(txtLog.TextLength, 0);
            txtLog.ScrollToCaret();
        }

        private void btnOpenCOM_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort.PortName = "COM" + numericUpDown1.Value.ToString();
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                    SetTxtLog("Connected\r\n");
                    btnAlive.Enabled = true;
                    btnCloseCOM.Enabled = true;
                    btnOpenCOM.Enabled = false;
                }
            }
            catch (Exception expt)
            {
                MessageBox.Show("Connection failure", "Connection failed  : \"" + expt.Message + "\"");
            }
        }

        private void btnCloseCOM_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                btnAlive.Enabled = false;
                btnCloseCOM.Enabled = false;
                btnOpenCOM.Enabled = true;
                SetTxtLog("Disconnected\r\n");
            }
        }
    }
}
