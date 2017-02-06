using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

using Centralite.CurrentSensor;

namespace Cina219test
{
    public partial class Form1 : Form
    {
        int _i2c_controller_index = 0;

        Cina219 _cina219_1;
        Cina219 _cina219_2;

        byte _cina219_address1 = 0x40;
        byte _cina219_address2 = 0x41;

        Task _read_task;
        CancellationTokenSource _cancel_read_task;

        UInt16 _ina219_calValue1;
        UInt16 _ina219_calValue2;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Make sure we have a i2c controller attached
            _i2c_controller_index = Cina219.GetFirstDevIndex();
            if (_i2c_controller_index < 0)
                throw new Exception("FT232H not found");

            // Init controller
            initCina219();

            button_single.Enabled = false;
            button_start.Text = "Stop";

            label_address1.Text = string.Format("Address: 0x{0:X}", _cina219_address1);
            label_address2.Text = string.Format("Address: 0x{0:X}", _cina219_address2);

            start_read_task();
        }

        void initCina219()
        {
            _cina219_1 = new Cina219(_cina219_address1, _i2c_controller_index);
            _cina219_1.Init();

            _ina219_calValue1 = _cina219_1.ReadCalibration();
            numericUpDown_cal1.Value = _ina219_calValue1;

            _cina219_2 = new Cina219(_cina219_1.I2CController, _cina219_address2);
            _cina219_2.Init();

            _ina219_calValue2 = _cina219_2.ReadCalibration();
            numericUpDown_cal2.Value = _ina219_calValue2;

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
            UInt16 cal_val = _cina219_1.ReadCalibration();
            string text = string.Format("0x{0:X}", cal_val);
            syncLabelSetTextAndColor(label_calibration1, text, Color.Black);
            if (cal_val != _ina219_calValue1)
            {
                UInt16 newval = (UInt16)numericUpDown_cal1.Value;
                _cina219_1.WriteCalibration(newval);
            }

            float volts_shunt = _cina219_1.GetShuntVoltage();
            text = string.Format("{0,5:F3}", volts_shunt);
            syncLabelSetTextAndColor(label_voltage_shunt1, text, Color.Black);

            bool ovf = false;
            float volts = _cina219_1.GetVoltage(ref ovf);
            text = string.Format("{0,5:F3}", volts);
            syncLabelSetTextAndColor(label_voltage_bus1, text, Color.Black);

            float current = _cina219_1.GetCurrent();
            text = string.Format("{0,5:F2}", current);
            if (ovf)
                text += "*";
            syncLabelSetTextAndColor(label_current1, text, Color.Black);
            if (current > 1.0)
            {
                _cina219_1.SetACBusPins(0xFE, 0xFF);
            }
            else
            {
                _cina219_1.SetACBusPins(0xFF, 0xFF);
            }

            float power = _cina219_1.GetPower();
            text = string.Format("{0,5:F2}", power);
            syncLabelSetTextAndColor(label_power1, text, Color.Black);



            cal_val = _cina219_2.ReadCalibration();
            text = string.Format("0x{0:X}", cal_val);
            syncLabelSetTextAndColor(label_calibration2, text, Color.Black);
            if (cal_val != _ina219_calValue2)
            {
                UInt16 newval = (UInt16)numericUpDown_cal2.Value;
                _cina219_2.WriteCalibration(newval);
            }

            volts_shunt = _cina219_2.GetShuntVoltage();
            text = string.Format("{0,5:F3}", volts_shunt);
            syncLabelSetTextAndColor(label_voltage_shunt2, text, Color.Black);

            ovf = false;
            volts = _cina219_2.GetVoltage(ref ovf);
            text = string.Format("{0,5:F3}", volts);
            syncLabelSetTextAndColor(label_voltage_bus2, text, Color.Black);

            current = _cina219_2.GetCurrent();
            text = string.Format("{0,5:F2}", current);
            if (ovf)
                text += "*";
            syncLabelSetTextAndColor(label_current2, text, Color.Black);

            power = _cina219_2.GetPower();
            text = string.Format("{0,5:F2}", power);
            syncLabelSetTextAndColor(label_power2, text, Color.Black);

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
            lock (numericUpDown_cal1)
            {
                _ina219_calValue1 = (UInt16)numericUpDown_cal1.Value;
            }
        }

        private void numericUpDown_cal2_ValueChanged(object sender, EventArgs e)
        {
            lock (numericUpDown_cal2)
            {
                _ina219_calValue2 = (UInt16)numericUpDown_cal2.Value;
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

        private void button1_Click(object sender, EventArgs e)
        {
            _cina219_1.SetACBusPins(0xFE, 0xFF);
        }
    }
}
