using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FTD2XX_NET;

namespace i2ctest
{
    /// <summary>
    ///  http://www.ftdichip.com/Support/Documents/AppNotes/AN_108_Command_Processor_for_MPSSE_and_MCU_Host_Bus_Emulation_Modes.pdf
    ///  http://www.ftdichip.com/Support/Documents/AppNotes/AN_113_FTDI_Hi_Speed_USB_To_I2C_Example.pdf
    ///  http://www.ftdichip.com/Support/Documents/AppNotes/AN_255_USB%20to%20I2C%20Example%20using%20the%20FT232H%20and%20FT201X%20devices.pdf
    /// </summary>

    class FTI2C
    {
        const byte MSB_FALLING_EDGE_CLOCK_BYTE_IN = 0x20;
        const byte MSB_FALLING_EDGE_CLOCK_BYTE_OUT = 0x11;
        const byte MSB_FALLING_EDGE_CLOCK_BIT_OUT = 0x13;
        const byte MSB_RISING_EDGE_CLOCK_BIT_IN = 0x22;

        const byte SET_DATA_BITS_LOW_BYTES = 0x80;
        const byte SET_TCK_SK_CLOCK_DIVISOR = 0x86;

        FTDI _ftdi;
        public FTDI Controller { get { return _ftdi; } }

        public void Init(int index)
        {
            _ftdi = new FTDI();
            FTDI.FT_STATUS status = _ftdi.OpenByIndex((uint)index);
            if (status != FTDI.FT_STATUS.FT_OK)
                throw new FTI2CException("Problem opening FTDI device");

            status = _ftdi.ResetDevice();
            if (status != FTDI.FT_STATUS.FT_OK)
                throw new FTI2CException("Problem reseting FTDI device");

            // Purge USB receive buffer first by reading out all old data from FT2232H receive buffer
            status = _ftdi.Purge(FTDI.FT_PURGE.FT_PURGE_RX | FTDI.FT_PURGE.FT_PURGE_TX);
            if (status != FTDI.FT_STATUS.FT_OK)
                throw new FTI2CException("Problem purging FTDI device");

            //Set USB request transfer size
            status = _ftdi.InTransferSize(65536);
            if (status != FTDI.FT_STATUS.FT_OK)
                throw new FTI2CException("Problem setting USB transgere size");

            //Disable event and error characters
            status = _ftdi.SetCharacters(0, false, 0, false);
            if (status != FTDI.FT_STATUS.FT_OK)
                throw new FTI2CException("Problem disabling event and error characters");

            //Sets the read and write timeouts in milliseconds for the FT2232H
            status = _ftdi.SetTimeouts(5000, 5000);
            if (status != FTDI.FT_STATUS.FT_OK)
                throw new FTI2CException("Problem setting timeouts");

            //Set the latency timer
            status = _ftdi.SetLatency(16);
            if (status != FTDI.FT_STATUS.FT_OK)
                throw new FTI2CException("Problem setting latency timer");

            //Reset controller
            status = _ftdi.SetBitMode(0x0, FTDI.FT_BIT_MODES.FT_BIT_MODE_RESET);
            if (status != FTDI.FT_STATUS.FT_OK)
                throw new FTI2CException("Problem resetting controller");

            // enable MPSEE mode
            status = _ftdi.SetBitMode(0x0, FTDI.FT_BIT_MODES.FT_BIT_MODE_MPSSE);
            if (status != FTDI.FT_STATUS.FT_OK)
                throw new FTI2CException("Problem enabling MPSEE mode");

            // Disables the clk divide by 5 to allow for a 60MHz master clock.
            byte[] outputBuffer = new byte[3];
            outputBuffer[0] = 0x8A;
            // Disable adaptive clocking
            outputBuffer[1] = 0x97;
            // Enables 3 phase data clocking. Used by I2C interfaces to allow data on both clock edges.
            outputBuffer[2] = 0x8C;
            // sent of commands
            uint outputSent = 0;
            status = _ftdi.Write(outputBuffer, outputBuffer.Length, ref outputSent);
            if (status != FTDI.FT_STATUS.FT_OK)
                throw new FTI2CException("Problem setting ft clock");

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
            outputBuffer[0] = SET_DATA_BITS_LOW_BYTES;
            // Set SK,DO high
            outputBuffer[1] = 0x03;
            // Set SK,DO as output, other as input
            outputBuffer[2] = 0x03;

            // Set clock divisor
            outputBuffer[3] = SET_TCK_SK_CLOCK_DIVISOR;
            //TCK/SK period = 12MHz*5 / ( (1 + (0xValueH << 8) | 0xValueL) ) * 2)
            // 100 kHz clock
            UInt16 dwClockDivisor = 0x00CB; //(0x012B/3) =~ 200
            //double test = (60E6) / ( (1+ ((0x00 << 8) | 0xCB)) * 2 );
            // low byte
            outputBuffer[4] = (byte)(dwClockDivisor & 0xFF);
            // high byte
            outputBuffer[5] = (byte)((dwClockDivisor >> 8) & 0xFF);

            // sent of commands
            status = _ftdi.Write(outputBuffer, outputBuffer.Length, ref outputSent);
            if (status != FTDI.FT_STATUS.FT_OK)
                throw new FTI2CException("Problem setting i2c master clock");

            // Turn off Loopback
            outputBuffer = new byte[1];
            outputBuffer[0] = 0x85;
            status = _ftdi.Write(outputBuffer, outputBuffer.Length, ref outputSent);
            if (status != FTDI.FT_STATUS.FT_OK)
                throw new FTI2CException("Problem Turn off Loopback");
        }

        public void SetI2CLinesIdle()
        {
            List<byte> buffer = new List<byte>();
            // Set the idle states for the AD lines
            buffer.Add(SET_DATA_BITS_LOW_BYTES); // Command to set ADbus direction and data 
            buffer.Add(0xFF);// Set all 8 lines to high level
            buffer.Add(0x01);// Set all pins as i/p except bit 2 (SCL)

            // IDLE line states are ...
            // AD0 (SCL) is output high (open drain, pulled up externally)
            // AD1 (DATA OUT) is output high (open drain, pulled up externally)
            // AD2 (DATA IN) is input (therefore the output value specified is ignored)
            // AD3 to AD7 are inputs (not used in this application)
            // Set the idle states for the AC lines
            buffer.Add(0x82); // Command to set ACbus direction and data
            buffer.Add(0xFF); // Set all 8 lines to high level
            buffer.Add(0x00); // All inputs

            RawWrite(buffer.ToArray());
        }

        public uint RawWrite(byte[] buffer)
        {
            uint outputSent = 0;
            FTDI.FT_STATUS status =_ftdi.Write(buffer, buffer.Length, ref outputSent);
            if (status != FTDI.FT_STATUS.FT_OK)
                throw new FTI2CException("Problem writing data");
            return outputSent;
        }

        public byte[] RawRead(uint count)
        {

            byte[] inputBuffer = new byte[count];
            uint inputRead = 0;
            FTDI.FT_STATUS status = _ftdi.Read(inputBuffer, count, ref inputRead);
            if (status != FTDI.FT_STATUS.FT_OK)
                throw new FTI2CException("Problem reading data");
            if (inputRead != count)
                throw new FTI2CException("Problem reading data");
            return inputBuffer;
        }

    }

    class FTI2CException: Exception
    {
        public FTI2CException(string msg):base(msg)
        {
            
        }
    }
}
