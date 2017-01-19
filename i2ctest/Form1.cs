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

                // 100 kHz clock
                UInt16 dwClockDivisor = 0x012B;

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
