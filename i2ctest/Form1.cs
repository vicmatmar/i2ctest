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

                Int16 ina219_calValue = 8192;
                _fti2c.WriteRegister(INA219_ADDRESS, INA219_REG_CALIBRATION, ina219_calValue);

                Int16 config = INA219_CONFIG_BVOLTAGERANGE_16V |
                                    INA219_CONFIG_GAIN_1_40MV |
                                    INA219_CONFIG_BADCRES_12BIT |
                                    INA219_CONFIG_SADCRES_12BIT_1S_532US |
                                    INA219_CONFIG_MODE_SANDBVOLT_CONTINUOUS;

                _fti2c.WriteRegister(INA219_ADDRESS, INA219_REG_CONFIG, config);

                Task read_task = new Task(()=>read_data());
                read_task.Start();

            }
        }

        void read_data()
        {
            while(true)
            {
                _fti2c.RegisterPointerSet(INA219_ADDRESS, false, INA219_REG_CALIBRATION);
                Int16 cal_val = _fti2c.ReadRegister(INA219_ADDRESS);

                _fti2c.RegisterPointerSet(INA219_ADDRESS, false, INA219_REG_CURRENT);
                Int16 current_reg = _fti2c.ReadRegister(INA219_ADDRESS);


                _fti2c.RegisterPointerSet(INA219_ADDRESS, false, INA219_REG_BUSVOLTAGE);
                Int16 volts_reg = _fti2c.ReadRegister(INA219_ADDRESS);
                // Shift to the right 3 to drop CNVR and OVF and multiply by LSB
                volts_reg = (Int16)((volts_reg >> 3) * 4);
                double volts = volts_reg * 0.001;


                syncLabelSetTextAndColor(label_voltage, volts.ToString(), Color.Black);
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

    }
}
