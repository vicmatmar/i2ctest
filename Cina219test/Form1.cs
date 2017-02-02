﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

using CINA219;

namespace Cina219test
{
    public partial class Form1 : Form
    {

        Cina219 _cina219;
        byte _cina219_address = 0x40;

        Task _read_task;
        CancellationTokenSource _cancel_read_task;
        UInt16 _ina219_calValue;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Make sure we have a i2c controller attached
            int i2c_controller_index = Cina219.GetFirstDevIndex();
            if (i2c_controller_index < 0)
                throw new Exception("FT232H not found");

            // Init controller
            _cina219 = new Cina219(_cina219_address, i2c_controller_index);
            _cina219.Init();

            _ina219_calValue = _cina219.ReadCalibration();
            numericUpDown_cal.Value = _ina219_calValue;

            button_single.Enabled = false;
            button_start.Text = "Stop";

            start_read_task();
        }

        void start_read_task()
        {
            _cancel_read_task = new CancellationTokenSource();
            _read_task = new Task(() => read_data_continous(_cancel_read_task.Token), _cancel_read_task.Token);
            _read_task.ContinueWith(ct => read_data_done(_read_task), TaskContinuationOptions.OnlyOnRanToCompletion);
            _read_task.Start();
        }

        void read_data_done(Task t)
        {
            synchronizedInvoke(button_start,
                delegate() { button_start.Text = "&Start"; });
            synchronizedInvoke(button_single,
                delegate () { button_single.Enabled = true; });

        }
        void stop_read_task()
        {
            if(_cancel_read_task != null)
                _cancel_read_task.Cancel();
        }

        void read_data_continous(CancellationToken cancel)
        {
            while (true)
            {
                if (cancel.IsCancellationRequested)
                    break;

                read_data();
            }
        }

        void read_data()
        {
            UInt16 cal_val = _cina219.ReadCalibration();
            string text = string.Format("0x{0:X}", cal_val);
            syncLabelSetTextAndColor(label_calibration, text, Color.Black);
            if (cal_val != _ina219_calValue)
            {
                UInt16 newval = (UInt16)numericUpDown_cal.Value;
                _cina219.WriteCalibration(newval);
            }

            float volts_shunt = _cina219.GetShuntVoltage();
            text = string.Format("{0,5:F3}", volts_shunt);
            syncLabelSetTextAndColor(label_voltage_shunt, text, Color.Black);

            bool ovf = false;
            float volts = _cina219.GetVoltage(ref ovf);
            text = string.Format("{0,5:F3}", volts);
            syncLabelSetTextAndColor(label_voltage_bus, text, Color.Black);

            float current = _cina219.GetCurrent();
            text = string.Format("{0,5:F2}", current);
            if (ovf)
                text += "*";
            syncLabelSetTextAndColor(label_current, text, Color.Black);

            float power = _cina219.GetPower();
            text = string.Format("{0,5:F2}", power);
            syncLabelSetTextAndColor(label_power, text, Color.Black);

        }

        void syncLabelSetTextAndColor(Label control, string text, Color forcolor)
        {
            synchronizedInvoke(control,
                delegate ()
                {
                    control.Text = text;
                    control.ForeColor = forcolor;
                });
        }

        void synchronizedInvoke(ISynchronizeInvoke sync, Action action)
        {
            // If the invoke is not required, then invoke here and get out.
            if (!sync.InvokeRequired)
            {
                // Execute action.
                action();

                // Get out.
                return;
            }

            try
            {
                // Marshal to the required context.
                sync.Invoke(action, new object[] { });
                //sync.BeginInvoke(action, new object[] { });
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        private void numericUpDown_cal_ValueChanged(object sender, EventArgs e)
        {
            lock (numericUpDown_cal)
            {
                _ina219_calValue = (UInt16)numericUpDown_cal.Value;
            }
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            if(_read_task.Status == TaskStatus.Running)
            {
                stop_read_task();
            }
            else
            {
                button_single.Enabled = false;
                button_start.Text = "Stop";

                start_read_task();
            }
        }

        private void button_single_Click(object sender, EventArgs e)
        {
            read_data();
        }
    }
}
