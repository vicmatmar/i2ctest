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

        byte _slave_addr = 0x40;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int index = GetFirstDevIndex();

            if (index >= 0)
            {
                byte[] i2c_start = _fti2c.FormStartBuffer();
                byte[] i2c_idle = _fti2c.FormIdleBuffer();

                _fti2c.Init(index);
                try
                {
                    _fti2c.RegisterPointerSet(_slave_addr, true, 0x00);
                    _fti2c.WriteRegister(_slave_addr, 0x05, 0x1234);
                }
                catch (FTI2CException ex)
                {
                    //Debug.Fail(ex.Message);
                }
                try
                {
                    _fti2c.RegisterPointerSet(_slave_addr, true, 0x00);
                }
                catch (FTI2CException ex)
                {
                    //Debug.Fail(ex.Message);
                }
            }
        }


        byte[] i2c_stop()
        {
            List<byte> start = new List<byte>();
            int i;

            for (i = 0; i < 4; ++i)
            {
                // SDA low, SCL high
                start.Add(SET_DATA_BITS_LOW_BYTES);
                start.Add(0x01);
                start.Add(0x03);
            }

            for (i = 0; i < 4; ++i)
            {
                // SDA high, SCL high
                start.Add(SET_DATA_BITS_LOW_BYTES);
                start.Add(0x03);
                start.Add(0x03);
            }

            // SDA tristate, SCL tristate
            start.Add(SET_DATA_BITS_LOW_BYTES);
            start.Add(0x00);
            start.Add(0x00); //other pins as input with bit 0

            return start.ToArray();

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
