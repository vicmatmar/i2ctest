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
        int _total_sensors = 3;

        Task _read_task;
        CancellationTokenSource _cancel_read_task;

        UInt16[] _sensor_calvals;

        public Form1()
        {
            InitializeComponent();

            label_err.Text = "";
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

            // Disable all panels
            int n = 0;
            while(true)
            {
                string ctrlname = string.Format("tableLayoutPanel{0}", n++);
                Control[] controls = Controls.Find(ctrlname, true);
                if (controls.Length > 0)
                    controls[0].Enabled = false;
                else
                    break;
            }

            for (int i = 0; i < _total_sensors; i++)
            {
                string ctrlname = string.Format("label_addr{0}", i);
                Label control = (Label)Controls.Find(ctrlname, true)[0];
                control.Text = string.Format("0x{0:X}", _sensors[i].INA219_ADDRESS);

                // Enable panel
                ctrlname = string.Format("tableLayoutPanel{0}", i);
                Control[] controls = Controls.Find(ctrlname, true);
                controls[0].Enabled = true;
            }

            start_read_task();
        }

        void initCina219()
        {
            _sensors = new Cina219[_total_sensors];

            _sensors[0] = new Cina219(_sensor_addresses[0], _i2c_controller_index);
            _sensors[0].UnitCurrent = Cina219.CurrentUnit.mA;
            _sensors[0].Init();

            _sensor_calvals = new UInt16[_total_sensors];
            _sensor_calvals[0] = _sensors[0].ReadCalibration();
            numericUpDown_cal0.Value = _sensor_calvals[0];

            for(int i = 1; i < _total_sensors; i++)
            {
                _sensors[i] = new Cina219(_sensors[0].I2CController, _sensor_addresses[i]);
                _sensors[i].UnitCurrent = Cina219.CurrentUnit.mA;
                _sensors[i].Init();

                _sensor_calvals[i] = _sensors[i].ReadCalibration();

                string ctrlname = string.Format("numericUpDown_cal{0}", i);
                NumericUpDown control = (NumericUpDown)Controls.Find(ctrlname, true)[0];
                control.Value = _sensor_calvals[i];

            }

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
                delegate () { button_start.Text = "&Start"; });
            synchronizedInvoke(button_single,
                delegate () { button_single.Enabled = true; });

        }
        void stop_read_task()
        {
            if (_cancel_read_task != null)
                _cancel_read_task.Cancel();
        }

        void read_data_continous(CancellationToken cancel)
        {
            while (true)
            {
                if (cancel.IsCancellationRequested)
                    break;

                try
                {
                    read_data();
                    syncLabelSetTextAndColor(label_err, "", Color.Black);
                }
                catch(Exception ex)
                {
                    syncLabelSetTextAndColor(label_err, ex.Message, Color.Black);
                }
            }
        }

        void read_data()
        {
            for(int i=0; i < _sensors.Length; i++)
            {
                UInt16 cal_val = _sensors[i].ReadCalibration();
                string text = string.Format("0x{0:X}", cal_val);

                string ctrlname = string.Format("label_calibration{0}", i);
                syncLabelSetTextAndColor(ctrlname, text, Color.Black);
                if (cal_val != _sensor_calvals[i])
                {
                    ctrlname = string.Format("numericUpDown_cal{0}", i);
                    NumericUpDown control = (NumericUpDown)Controls.Find(ctrlname, true)[0];
                    UInt16 newval = (UInt16)control.Value;
                    _sensors[i].WriteCalibration(newval);
                }

                float volts_shunt = _sensors[i].GetShuntVoltage();
                text = string.Format("{0,5:F3}", volts_shunt);
                ctrlname = string.Format("label_voltage_shunt{0}", i);
                syncLabelSetTextAndColor(ctrlname, text, Color.Black);

                bool ovf = false;
                float volts = _sensors[i].GetVoltage(ref ovf);
                text = string.Format("{0,5:F3}", volts);
                ctrlname = string.Format("label_voltage_bus{0}", i);
                syncLabelSetTextAndColor(ctrlname, text, Color.Black);

                float current = _sensors[i].GetCurrent();
                text = string.Format("{0,5:F2}", current);
                if (ovf)
                    text += " ovf";

                if (current > 10.0)
                {
                    // Trun LED ON, C0 low
                    byte led_bit = _sensors[i].GetACBusPins();
                    byte mask = (byte)~(0x01 << i);
                    led_bit = (byte)(led_bit & mask);
                    _sensors[i].SetACBusPins(led_bit, 0xFF);
                    text += "*";
                }
                else
                {
                    // Trun LED OFF, C0 high
                    byte led_bit = _sensors[i].GetACBusPins();
                    byte mask = (byte)(0x01 << i);
                    led_bit = (byte)(led_bit | mask);
                    _sensors[i].SetACBusPins(led_bit, 0xFF);
                }

                ctrlname = string.Format("label_current{0}", i);
                syncLabelSetTextAndColor(ctrlname, text, Color.Black);

                float power = _sensors[i].GetPower();
                text = string.Format("{0,5:F2}", power);
                ctrlname = string.Format("label_power{0}", i);
                syncLabelSetTextAndColor(ctrlname, text, Color.Black);

            }
        }

        void syncLabelSetTextAndColor(string ctrlname, string text, Color forcolor)
        {
            Label label = (Label)Controls.Find(ctrlname, true)[0];
            syncLabelSetTextAndColor(label, text, Color.Black);
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
            NumericUpDown control = (NumericUpDown)sender;
            int index = Convert.ToInt32(control.Tag);
            lock (control)
            {
                _sensor_calvals[index] = (UInt16)control.Value;
            }
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            if (_read_task.Status == TaskStatus.Running)
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
