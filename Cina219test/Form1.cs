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

        Cina219[] _sensors;
        byte[] _sensor_addresses = new byte[] { 0x40, 0x41, 0x44, 0x45 };
        int _total_sensors = 2;

        Task _read_task;
        CancellationTokenSource _cancel_read_task;

        UInt16[] _sensor_calvals;

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

            label_address1.Text = string.Format("Address: 0x{0:X}", _sensors[0].INA219_ADDRESS);
            label_address2.Text = string.Format("Address: 0x{0:X}", _sensors[1].INA219_ADDRESS);

            start_read_task();
        }

        void initCina219()
        {
            _sensors = new Cina219[_total_sensors];

            _sensors[0] = new Cina219(_sensor_addresses[0], _i2c_controller_index);
            _sensors[0].Init();

            _sensor_calvals = new UInt16[_total_sensors];
            _sensor_calvals[0] = _sensors[0].ReadCalibration();
            numericUpDown_cal1.Value = _sensor_calvals[0];

            _sensors[1] = new Cina219(_sensors[0].I2CController, _sensor_addresses[1]);
            _sensors[1].Init();

            _sensor_calvals[1] = _sensors[1].ReadCalibration();
            numericUpDown_cal2.Value = _sensor_calvals[1];

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
            UInt16 cal_val = _sensors[0].ReadCalibration();
            string text = string.Format("0x{0:X}", cal_val);
            syncLabelSetTextAndColor(label_calibration1, text, Color.Black);
            if (cal_val != _sensor_calvals[0])
            {
                UInt16 newval = (UInt16)numericUpDown_cal1.Value;
                _sensors[0].WriteCalibration(newval);
            }

            float volts_shunt = _sensors[0].GetShuntVoltage();
            text = string.Format("{0,5:F3}", volts_shunt);
            syncLabelSetTextAndColor(label_voltage_shunt1, text, Color.Black);

            bool ovf = false;
            float volts = _sensors[0].GetVoltage(ref ovf);
            text = string.Format("{0,5:F3}", volts);
            syncLabelSetTextAndColor(label_voltage_bus1, text, Color.Black);

            float current = _sensors[0].GetCurrent();
            text = string.Format("{0,5:F2}", current);
            if (ovf)
                text += "*";
            syncLabelSetTextAndColor(label_current1, text, Color.Black);
            if (current > 20.0)
            {
                // Trun LED ON, C0 low
                byte led_bit = _sensors[0].GetACBusPins();
                led_bit = (byte)(led_bit & 0xFE);
                _sensors[0].SetACBusPins(led_bit, 0xFF);
            }
            else
            {
                // Trun LED OFF, C0 high
                byte led_bit = _sensors[0].GetACBusPins();
                led_bit = (byte)(led_bit | 0x01);
                _sensors[0].SetACBusPins(led_bit, 0xFF);
            }

            float power = _sensors[0].GetPower();
            text = string.Format("{0,5:F2}", power);
            syncLabelSetTextAndColor(label_power1, text, Color.Black);



            cal_val = _sensors[1].ReadCalibration();
            text = string.Format("0x{0:X}", cal_val);
            syncLabelSetTextAndColor(label_calibration2, text, Color.Black);
            if (cal_val != _sensor_calvals[1])
            {
                UInt16 newval = (UInt16)numericUpDown_cal2.Value;
                _sensors[1].WriteCalibration(newval);
            }

            volts_shunt = _sensors[1].GetShuntVoltage();
            text = string.Format("{0,5:F3}", volts_shunt);
            syncLabelSetTextAndColor(label_voltage_shunt2, text, Color.Black);

            ovf = false;
            volts = _sensors[1].GetVoltage(ref ovf);
            text = string.Format("{0,5:F3}", volts);
            syncLabelSetTextAndColor(label_voltage_bus2, text, Color.Black);

            current = _sensors[1].GetCurrent();
            text = string.Format("{0,5:F2}", current);
            if (ovf)
                text += "*";
            syncLabelSetTextAndColor(label_current2, text, Color.Black);

            power = _sensors[1].GetPower();
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
                _sensor_calvals[0] = (UInt16)numericUpDown_cal1.Value;
            }
        }

        private void numericUpDown_cal2_ValueChanged(object sender, EventArgs e)
        {
            lock (numericUpDown_cal2)
            {
                _sensor_calvals[1] = (UInt16)numericUpDown_cal2.Value;
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
