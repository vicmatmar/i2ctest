using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using FTD2XX_NET;

/// <summary>
///  http://www.ftdichip.com/Support/Documents/AppNotes/AN_108_Command_Processor_for_MPSSE_and_MCU_Host_Bus_Emulation_Modes.pdf
///  http://www.ftdichip.com/Support/Documents/AppNotes/AN_113_FTDI_Hi_Speed_USB_To_I2C_Example.pdf
/// </summary>

namespace i2ctest
{
    public partial class Form1 : Form
    {
        const byte MSB_FALLING_EDGE_CLOCK_BYTE_IN = 0x20;
        const byte MSB_FALLING_EDGE_CLOCK_BYTE_OUT = 0x11;
        const byte MSB_FALLING_EDGE_CLOCK_BIT_OUT = 0x13;
        const byte MSB_RISING_EDGE_CLOCK_BIT_IN = 0x22;

        const byte SET_DATA_BITS_LOW_BYTES = 0x80;
        const byte SET_TCK_SK_CLOCK_DIVISOR = 0x86;

        FTI2C _fti2c = new FTI2C();

        const byte INA219_ADDRESS = (0x40);    // 1000000 (A0+A1=GND)
        const byte INA219_READ = (0x01);

        const byte INA219_REG_CONFIG = (0x00);

        const Int16 INA219_CONFIG_BVOLTAGERANGE_MASK = (0x2000);  // Bus Voltage Range Mask
        const Int16 INA219_CONFIG_BVOLTAGERANGE_16V = (0x0000);  // 0-16V Range
        const Int16 INA219_CONFIG_BVOLTAGERANGE_32V = (0x2000);  // 0-32V Range

        const Int16 INA219_CONFIG_GAIN_MASK = (0x1800);  // Gain Mask
        const Int16 INA219_CONFIG_GAIN_1_40MV = (0x0000);  // Gain 1, 40mV Range
        const Int16 INA219_CONFIG_GAIN_2_80MV = (0x0800);  // Gain 2, 80mV Range
        const Int16 INA219_CONFIG_GAIN_4_160MV = (0x1000);  // Gain 4, 160mV Range
        const Int16 INA219_CONFIG_GAIN_8_320MV = (0x1800);  // Gain 8, 320mV Range

        const Int16 INA219_CONFIG_BADCRES_MASK = (0x0780);  // Bus ADC Resolution Mask
        const Int16 INA219_CONFIG_BADCRES_9BIT = (0x0080);  // 9-bit bus res = 0..511
        const Int16 INA219_CONFIG_BADCRES_10BIT = (0x0100);  // 10-bit bus res = 0..1023
        const Int16 INA219_CONFIG_BADCRES_11BIT = (0x0200);  // 11-bit bus res = 0..2047
        const Int16 INA219_CONFIG_BADCRES_12BIT = (0x0400);  // 12-bit bus res = 0..4097

        const Int16 INA219_CONFIG_SADCRES_MASK = (0x0078);  // Shunt ADC Resolution and Averaging Mask
        const Int16 INA219_CONFIG_SADCRES_9BIT_1S_84US = (0x0000);  // 1 x 9-bit shunt sample
        const Int16 INA219_CONFIG_SADCRES_10BIT_1S_148US = (0x0008);  // 1 x 10-bit shunt sample
        const Int16 INA219_CONFIG_SADCRES_11BIT_1S_276US = (0x0010);  // 1 x 11-bit shunt sample
        const Int16 INA219_CONFIG_SADCRES_12BIT_1S_532US = (0x0018);  // 1 x 12-bit shunt sample
        const Int16 INA219_CONFIG_SADCRES_12BIT_2S_1060US = (0x0048);     // 2 x 12-bit shunt samples averaged together
        const Int16 INA219_CONFIG_SADCRES_12BIT_4S_2130US = (0x0050);  // 4 x 12-bit shunt samples averaged together
        const Int16 INA219_CONFIG_SADCRES_12BIT_8S_4260US = (0x0058);  // 8 x 12-bit shunt samples averaged together
        const Int16 INA219_CONFIG_SADCRES_12BIT_16S_8510US = (0x0060);  // 16 x 12-bit shunt samples averaged together
        const Int16 INA219_CONFIG_SADCRES_12BIT_32S_17MS = (0x0068);  // 32 x 12-bit shunt samples averaged together
        const Int16 INA219_CONFIG_SADCRES_12BIT_64S_34MS = (0x0070);  // 64 x 12-bit shunt samples averaged together
        const Int16 INA219_CONFIG_SADCRES_12BIT_128S_69MS = (0x0078);  // 128 x 12-bit shunt samples averaged together

        const Int16 INA219_CONFIG_MODE_MASK = (0x0007);  // Operating Mode Mask
        const Int16 INA219_CONFIG_MODE_POWERDOWN = (0x0000);
        const Int16 INA219_CONFIG_MODE_SVOLT_TRIGGERED = (0x0001);
        const Int16 INA219_CONFIG_MODE_BVOLT_TRIGGERED = (0x0002);
        const Int16 INA219_CONFIG_MODE_SANDBVOLT_TRIGGERED = (0x0003);
        const Int16 INA219_CONFIG_MODE_ADCOFF = (0x0004);
        const Int16 INA219_CONFIG_MODE_SVOLT_CONTINUOUS = (0x0005);
        const Int16 INA219_CONFIG_MODE_BVOLT_CONTINUOUS = (0x0006);
        const Int16 INA219_CONFIG_MODE_SANDBVOLT_CONTINUOUS = (0x0007);

        const byte INA219_REG_SHUNTVOLTAGE = (0x01);
        const byte INA219_REG_BUSVOLTAGE = (0x02);
        const byte INA219_REG_POWER = (0x03);
        const byte INA219_REG_CURRENT = (0x04);
        const byte INA219_REG_CALIBRATION = (0x05);

        UInt16 _ina219_calValue = 8192;
        double _ina219_currentDivider_mA = 20;  // Current LSB = 50uA per bit (1000/50 = 20)
        double _ina219_powerDivider_mW = 1;     // Power LSB = 1mW per bit

        Task _read_task;
        CancellationTokenSource _cancel_read_task;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int index = GetFirstDevIndex();

            if (index >= 0)
            {

                _fti2c.Init(index);

                // VBUS_MAX = 16V
                // VSHUNT_MAX = 0.04          (Assumes Gain 1, 40mV)
                // RSHUNT = 0.1               (Resistor value in ohms)

                // Calibration which uses the highest precision for 
                // current measurement (0.1mA), at the expense of 
                // only supporting 16V at 400mA max.

                // 1. Determine max possible current
                // MaxPossible_I = VSHUNT_MAX / RSHUNT
                // MaxPossible_I = 0.4A

                // 2. Determine max expected current
                // MaxExpected_I = 0.4A

                // 3. Calculate possible range of LSBs (Min = 15-bit, Max = 12-bit)
                // MinimumLSB = MaxExpected_I/32767
                // MinimumLSB = 0.0000122              (12uA per bit)
                // MaximumLSB = MaxExpected_I/4096
                // MaximumLSB = 0.0000977              (98uA per bit)

                // 4. Choose an LSB between the min and max values
                //    (Preferrably a roundish number close to MinLSB)
                // CurrentLSB = 0.00005 (50uA per bit)

                // 5. Compute the calibration register
                // Cal = trunc (0.04096 / (Current_LSB * RSHUNT))
                // Cal = 8192 (0x2000)
                //_ina219_calValue = 0x2000;
                _ina219_calValue = 0xFFFE;
                writeCalibration(_ina219_calValue);
                //_fti2c.WriteRegister(INA219_ADDRESS, INA219_REG_CALIBRATION, _ina219_calValue);

                // 6. Calculate the power LSB
                // PowerLSB = 20 * CurrentLSB
                // PowerLSB = 0.001 (1mW per bit)

                // 7. Compute the maximum current and shunt voltage values before overflow
                // Max_Current = Current_LSB * 32767
                // Max_Current = 1.63835A before overflow
                //
                // If Max_Current > Max_Possible_I then
                //    Max_Current_Before_Overflow = MaxPossible_I
                // Else
                //    Max_Current_Before_Overflow = Max_Current
                // End If
                //
                // Max_Current_Before_Overflow = MaxPossible_I
                // Max_Current_Before_Overflow = 0.4
                //
                // Max_ShuntVoltage = Max_Current_Before_Overflow * RSHUNT
                // Max_ShuntVoltage = 0.04V
                //
                // If Max_ShuntVoltage >= VSHUNT_MAX
                //    Max_ShuntVoltage_Before_Overflow = VSHUNT_MAX
                // Else
                //    Max_ShuntVoltage_Before_Overflow = Max_ShuntVoltage
                // End If
                //
                // Max_ShuntVoltage_Before_Overflow = VSHUNT_MAX
                // Max_ShuntVoltage_Before_Overflow = 0.04V

                // 8. Compute the Maximum Power
                // MaximumPower = Max_Current_Before_Overflow * VBUS_MAX
                // MaximumPower = 0.4 * 16V
                // MaximumPower = 6.4W

                // Set multipliers to convert raw current/power values
                _ina219_currentDivider_mA = 160;  // Current LSB = 50uA per bit (1000/50 = 20)
                _ina219_powerDivider_mW = 1;     // Power LSB = 1mW per bit


                UInt16 config = INA219_CONFIG_BVOLTAGERANGE_16V |
                                    INA219_CONFIG_GAIN_8_320MV |
                                    INA219_CONFIG_BADCRES_12BIT |
                                    INA219_CONFIG_SADCRES_12BIT_32S_17MS |
                                    INA219_CONFIG_MODE_SANDBVOLT_CONTINUOUS;

                _fti2c.WriteRegister(INA219_ADDRESS, INA219_REG_CONFIG, config);

                numericUpDown_cal.Value = _ina219_calValue;
                start_read_task();
            }
        }

        void start_read_task()
        {
            _cancel_read_task = new CancellationTokenSource();
            _read_task = new Task(() => read_data(_cancel_read_task.Token), _cancel_read_task.Token);
            _read_task.Start();
        }

        void writeCalibration(UInt16 value)
        {
            _fti2c.WriteRegister(INA219_ADDRESS, INA219_REG_CALIBRATION, value);
        }

        UInt16 readCalibration()
        {
            _fti2c.RegisterPointerSet(INA219_ADDRESS, false, INA219_REG_CALIBRATION);
            UInt16 cal_val = (UInt16)_fti2c.ReadRegister(INA219_ADDRESS);

            return cal_val;
        }

        void read_data(CancellationToken cancel)
        {
            while (true)
            {
                if (cancel.IsCancellationRequested)
                    return;

                UInt16 cal_val = readCalibration();
                string text = string.Format("0x{0:X}", cal_val);
                syncLabelSetTextAndColor(label_calibration, text, Color.Black);
                if (cal_val != _ina219_calValue)
                {
                    UInt16 newval = (UInt16)numericUpDown_cal.Value;
                    writeCalibration(newval);
                }


                _fti2c.RegisterPointerSet(INA219_ADDRESS, false, INA219_REG_SHUNTVOLTAGE);
                Int16 volts_shunt_reg = _fti2c.ReadRegister(INA219_ADDRESS);
                // Shift to the right 3 to drop CNVR and OVF and multiply by LSB
                double volts_shunt = volts_shunt_reg * 0.01;
                text = string.Format("{0:F4}", volts_shunt);

                syncLabelSetTextAndColor(label_voltage_shunt, text, Color.Black);

                _fti2c.RegisterPointerSet(INA219_ADDRESS, false, INA219_REG_BUSVOLTAGE);
                Int16 volts_reg = _fti2c.ReadRegister(INA219_ADDRESS);
                text = "";
                if ((Int16)(volts_reg & 0x01) == 0x01)
                {
                    text = "Over Flow";
                }
                else
                {
                    // Shift to the right 3 to drop CNVR and OVF and multiply by LSB
                    volts_reg = (Int16)(volts_reg >> 3);
                    double volts = volts_reg * 4e-3;
                    text = string.Format("{0:F4}", volts);
                }
                syncLabelSetTextAndColor(label_voltage_bus, text, Color.Black);


                _fti2c.RegisterPointerSet(INA219_ADDRESS, false, INA219_REG_CURRENT);
                Int16 current_reg = _fti2c.ReadRegister(INA219_ADDRESS);
                double current = current_reg / _ina219_currentDivider_mA;
                text = string.Format(" {0:F4}", current);
                syncLabelSetTextAndColor(label_current, text, Color.Black);
            }
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

        /// <summary>
        /// Returns the index of the first available FTDI 232H device found in the system
        /// </summary>
        /// <returns></returns>
        static public int GetFirstDevIndex()
        {

            FTDI ftdi = new FTDI();

            int count = 10;

            FTDI.FT_DEVICE_INFO_NODE[] devlist = new FTDI.FT_DEVICE_INFO_NODE[count];
            FTDI.FT_STATUS status = ftdi.GetDeviceList(devlist);
            Debug.Assert(status == FTDI.FT_STATUS.FT_OK, "Problem getting FTDI device list");

            int index = -1;
            for (int i = 0; i < count; i++)
            {
                FTDI.FT_DEVICE_INFO_NODE devinfo = devlist[i];
                if (devinfo != null)
                {
                    if (devinfo.Type == FTD2XX_NET.FTDI.FT_DEVICE.FT_DEVICE_232H)
                    {
                        index = i;
                        FTDI.FT_DEVICE device_type = devinfo.Type;
                        break;
                    }
                }
            }

            return index;
        }

        private void numericUpDown_cal_ValueChanged(object sender, EventArgs e)
        {
            lock (numericUpDown_cal)
            {
                _ina219_calValue = (UInt16)numericUpDown_cal.Value;
            }
        }
    }
}
