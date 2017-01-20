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

namespace i2ctest
{
    public partial class Form1 : Form
    {
        const byte MSB_FALLING_EDGE_CLOCK_BYTE_IN = 0x20;
        const byte MSB_FALLING_EDGE_CLOCK_BYTE_OUT = 0x11;
        const byte MSB_FALLING_EDGE_CLOCK_BIT_OUT = 0x13;
        const byte MSB_RISING_EDGE_CLOCK_BIT_IN = 0x22;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int index = GetFirstDevIndex();
            if(index >= 0)
            {
                FTDI ftdi = new FTDI();
                FTDI.FT_STATUS status = ftdi.OpenByIndex((uint)index);
                Debug.Assert(status == FTDI.FT_STATUS.FT_OK, "Problem opening FTDI device");
                if (status != FTDI.FT_STATUS.FT_OK)
                    return;

                status = ftdi.ResetDevice();
                Debug.Assert(status == FTDI.FT_STATUS.FT_OK, "Problem reseting FTDI device");
                if (status != FTDI.FT_STATUS.FT_OK)
                    return;

                // Purge USB receive buffer first by reading out all old data from FT2232H receive buffer
                status = ftdi.Purge(FTDI.FT_PURGE.FT_PURGE_RX | FTDI.FT_PURGE.FT_PURGE_TX);
                Debug.Assert(status == FTDI.FT_STATUS.FT_OK, "Problem purging FTDI device");
                if (status != FTDI.FT_STATUS.FT_OK)
                    return;
                
                //Set USB request transfer size
                status = ftdi.InTransferSize(65536);
                Debug.Assert(status == FTDI.FT_STATUS.FT_OK, "Problem setting USB transgere size");
                if (status != FTDI.FT_STATUS.FT_OK)
                    return;

                //Disable event and error characters
                status = ftdi.SetCharacters(0, false, 0, false);
                Debug.Assert(status == FTDI.FT_STATUS.FT_OK, "Problem disabling event and error characters");
                if (status != FTDI.FT_STATUS.FT_OK)
                    return;

                //Sets the read and write timeouts in milliseconds for the FT2232H
                status = ftdi.SetTimeouts(5000, 5000);
                Debug.Assert(status == FTDI.FT_STATUS.FT_OK, "Problem setting timeouts");
                if (status != FTDI.FT_STATUS.FT_OK)
                    return;

                //Set the latency timer
                status = ftdi.SetLatency(16);
                Debug.Assert(status == FTDI.FT_STATUS.FT_OK, "Problem setting latency timer");
                if (status != FTDI.FT_STATUS.FT_OK)
                    return;

                //Reset controller
                status = ftdi.SetBitMode(0x0, FTDI.FT_BIT_MODES.FT_BIT_MODE_RESET);
                Debug.Assert(status == FTDI.FT_STATUS.FT_OK, "Problem resetting controller");
                if (status != FTDI.FT_STATUS.FT_OK)
                    return;
                // enable MPSEE mode
                status = ftdi.SetBitMode(0x0, FTDI.FT_BIT_MODES.FT_BIT_MODE_MPSSE);
                Debug.Assert(status == FTDI.FT_STATUS.FT_OK, "Problem enabling MPSEE mode");
                if (status != FTDI.FT_STATUS.FT_OK)
                    return;

                // Disables the clk divide by 5 to allow for a 60MHz master clock.
                byte[] outputBuffer = new byte[3];
                outputBuffer[0] = 0x8A;
                // Disable adaptive clocking
                outputBuffer[1] = 0x97;
                // Enables 3 phase data clocking. Used by I2C interfaces to allow data on both clock edges.
                outputBuffer[2] = 0x8C;
                // sent of commands
                uint outputSent = 0;
                status = ftdi.Write(outputBuffer, outputBuffer.Length, ref outputSent);
                Debug.Assert(status == FTDI.FT_STATUS.FT_OK, "Problem setting i2c");
                if (status != FTDI.FT_STATUS.FT_OK)
                    return;

                /*
                    ADBUS0 TCK/SK ---> SCL
                    ADBUS1 TDI/DO -+-> SDA
                    ADBUS2 TDO/DI -+
                    ADBUS3 TMS/CS
                    ADBUSS GPIOL0
                    ADBUS5 GPIOL1
                    ADBUS6 GPIOl2
                    ADBUS7 GPIOL3
                */

                outputBuffer = new byte[6];
                // Set values and directions of lower 8 pins (ADBUS7-0)
                outputBuffer[0] = 0x80;
                // Set SK,DO high
                outputBuffer[1] = 0x03;
                // Set SK,DO as output, other as input
                outputBuffer[2] = 0x03;

                // Set clock divisor
                outputBuffer[3] = 0x86;
                // 100 kHz clock
                UInt16 dwClockDivisor = 0x012B;
                // low byte
                outputBuffer[4] = (byte)(dwClockDivisor & 0xFF);
                // high byte
                outputBuffer[5] = (byte)((dwClockDivisor >> 8) & 0xFF);

                // sent of commands
                status = ftdi.Write(outputBuffer, outputBuffer.Length, ref outputSent);
                Debug.Assert(status == FTDI.FT_STATUS.FT_OK, "Problem setting pins");
                if (status != FTDI.FT_STATUS.FT_OK)
                    return;

                // Turn of Loopback
                outputBuffer = new byte[1];
                outputBuffer[0] = 0x85;
                status = ftdi.Write(outputBuffer, outputBuffer.Length, ref outputSent);
                Debug.Assert(status == FTDI.FT_STATUS.FT_OK, "Problem Turn of Loopback");
                if (status != FTDI.FT_STATUS.FT_OK)
                    return;

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
                buffer.Add(0x80);
                buffer.Add(0x00);
                buffer.Add(0x01);

                buffer.Add(MSB_RISING_EDGE_CLOCK_BIT_IN);
                // length of 0x00 means scan 1 bit
                buffer.Add(0x00);
                buffer.Add(0x87);

                int n = 0;
                while (true)
                {
                    status = ftdi.Write(buffer.ToArray(), buffer.Count, ref outputSent);
                    Debug.Assert(status == FTDI.FT_STATUS.FT_OK, "Problem writing i2c data");
                    if (status != FTDI.FT_STATUS.FT_OK)
                        return;
                    if (n++ > 2)
                        n = 0;
                }

                byte[] i2cstopt = i2c_stop();



            }
        }

        byte[] i2c_start()
        {
            List<byte> start = new List<byte>();
            int i;

            for (i = 0; i < 4; ++i)
            {
                // SDA high, SCL high
                start.Add(0x80);
                start.Add(0x03);
                start.Add(0x03);
            }

            for (i = 0; i < 4; ++i)
            {
                // SDA low, SCL high
                start.Add(0x80);
                start.Add(0x01);
                start.Add(0x03);
            }

            // SDA low, SCL low
            start.Add(0x80);
            start.Add(0x00);
            start.Add(0x03);

            return start.ToArray();

        }

        byte[] i2c_stop()
        {
            List<byte> start = new List<byte>();
            int i;

            for (i = 0; i < 4; ++i)
            {
                // SDA low, SCL high
                start.Add(0x80);
                start.Add(0x01);
                start.Add(0x03);
            }

            for (i = 0; i < 4; ++i)
            {
                // SDA high, SCL high
                start.Add(0x80);
                start.Add(0x03);
                start.Add(0x03);
            }

            // SDA tristate, SCL tristate
            start.Add(0x80);
            start.Add(0x00);
            start.Add(0x00);

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
