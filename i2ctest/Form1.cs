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

                byte[] i2cstart = i2c_start();
                List<byte> buffer = new List<byte>();
                buffer.AddRange(i2cstart);

                // clock data byte on clock edge MSB first
                buffer.Add(MSB_FALLING_EDGE_CLOCK_BYTE_OUT);
                // data length of 0x0000 means 1 byte
                buffer.Add(0x00);
                buffer.Add(0x00);

                byte data = 0xA5;
                buffer.Add(data);

                // SDA tristate, SCL low
                buffer.Add(SET_DATA_BITS_LOW_BYTES);
                buffer.Add(0x00);
                buffer.Add(0x01);

                buffer.Add(MSB_RISING_EDGE_CLOCK_BIT_IN);
                // length of 0x00 means scan 1 bit
                buffer.Add(0x00);
                buffer.Add(0x87);

                _fti2c.RawWrite(buffer.ToArray());


                byte[] inputBuffer = _fti2c.RawRead(1);
                // Check ACK
                if (((inputBuffer[0] & 0x01) != 0x00))
                {
                    //return;
                }

                // SDA high, SCL low
                buffer = new List<byte>();
                buffer.Add(SET_DATA_BITS_LOW_BYTES);
                buffer.Add(0x03);
                buffer.Add(0x03);
                uint outputSent = 0;
                FTDI.FT_STATUS status = _fti2c.Controller.Write(buffer.ToArray(), buffer.Count, ref outputSent);
                Debug.Assert(status == FTDI.FT_STATUS.FT_OK, "Problem writing i2c data");
                if (status != FTDI.FT_STATUS.FT_OK)
                    return;



            }
        }

        byte[] i2c_start()
        {
            List<byte> start = new List<byte>();
            int i;

            // Repeat commands to ensure the minimum period of the start hold time ie 600ns is achieved
            for (i = 0; i < 4; ++i)
            {
                // SDA high, SCL high
                start.Add(SET_DATA_BITS_LOW_BYTES); //Command to set directions of lower 8 pins and force value on bits set as output
                start.Add(0x03); //Set SDA, SCL high, WP disabled by SK, DO at bit 1, GPIOL0 at bit 0
                start.Add(0x03); //Set SK,DO,GPIOL0 pins as output with bit 1, other pins as input with bit 0
            }

            for (i = 0; i < 4; ++i)
            {
                // SDA low, SCL high
                start.Add(SET_DATA_BITS_LOW_BYTES); //Command to set directions of lower 8 pins and force value on bits set as output
                start.Add(0x01); //Set SDA low, SCL high, WP disabled by SK at bit 1, DO, GPIOL0 at bit 0
                start.Add(0x03); //Set SK,DO,GPIOL0 pins as output with bit 1, other pins as input with bit 0
            }

            // SDA low, SCL low
            start.Add(SET_DATA_BITS_LOW_BYTES);//Command to set directions of lower 8 pins and force value on bits set as output
            start.Add(0x00);//Set SDA, SCL low, WP disabled by SK, DO, GPIOL0 at bit 0
            start.Add(0x03);//Set SK,DO,GPIOL0 pins as output with bit 1, other pins as input with bit 0

            return start.ToArray();

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
