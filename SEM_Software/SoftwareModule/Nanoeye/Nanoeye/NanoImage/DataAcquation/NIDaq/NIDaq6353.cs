using System;
using System.Collections.Generic;
using System.Diagnostics;
using NationalInstruments.DAQmx;


namespace SEC.Nanoeye.NanoImage.DataAcquation.NIDaq
{
    internal class NIDaq6353 : NIDaq6351
    {
        public NIDaq6353(string device)
        {
            daqDevice = device;
            Trace.WriteLine(DaqSystem.Local.DriverMajorVersion.ToString() + "." + DaqSystem.Local.DriverMinorVersion + "." + DaqSystem.Local.DriverUpdateVersion, this.ToString());
        }

        ~NIDaq6353()
        {
            Dispose();
        }

        public static string[] SearchNIDaq6353()
        {
            List<string> deviceList = new List<string>();

            foreach (string deviceName in DaqSystem.Local.Devices)
            {
                Device device = DaqSystem.Local.LoadDevice(deviceName);

                if (device.ProductType.Contains("6353"))
                {
                    deviceList.Add(deviceName);
                }
                device.Dispose();
            }
            return deviceList.ToArray();
        }

        public override void ValidateSetting(SettingScanner setting)
        {
            System.Text.StringBuilder msg = new System.Text.StringBuilder();
            System.Text.StringBuilder arg = new System.Text.StringBuilder();

            if (setting.AiChannel >= 8)
            {
                setting.AiChannel = 7;
                msg.AppendLine("AiChannel is large then 7.");
                arg.AppendLine("AiChannel");
            }
            if (setting.AiClock > 2000000)
            {
                setting.AiClock = 2000000;
                msg.AppendLine("AiClock is large then 2000000.");
                arg.AppendLine("AiClock");
            }
            if ((setting.AiMaximum != 10) && (setting.AiMaximum != 5) && (setting.AiMaximum != 2) && (setting.AiMaximum != 1) && (setting.AiMaximum != 0.5f) && (setting.AiMaximum != 0.2f) && (setting.AiMaximum != 0.1f))
            {
                setting.AiMaximum = 10;
                msg.AppendLine("AiMaximum is invalid number.");
                arg.AppendLine("AiMaximum");
            }

            if (setting.AiMaximum != (setting.AiMinimum * -1))
            {
                setting.AiMinimum = setting.AiMaximum * -1;
                msg.AppendLine("AiMinimum is invalid number.");
                arg.AppendLine("AiMinimum");
            }

            if (setting.AoClock > 2000000)
            {
                setting.AoClock = 2000000;
                msg.AppendLine("AoClock is large then 2000000.");
                arg.AppendLine("AoClock");
            }

            if ((setting.AoMaximum != 10) && (setting.AiMaximum != 5))
            {
                setting.AoMaximum = 10;
                msg.AppendLine("AoMaximum is invalid number.");
                arg.AppendLine("AoMaximum");
            }

            if (setting.AoMaximum != (setting.AoMinimum * -1))
            {
                setting.AoMinimum = setting.AoMaximum * -1;
                msg.AppendLine("AoMinimum is invalid number.");
                arg.AppendLine("AoMinimum");
            }

            if ((setting.AreaShiftX > 1) || (setting.AreaShiftX < -1))
            {
                setting.AreaShiftX = 0;
                msg.AppendLine("AreaShiftX must be bettwen from -1 to 1.");
                arg.AppendLine("AreaShiftX");
            }

            if ((setting.AreaShiftY > 1) || (setting.AreaShiftY < -1))
            {
                setting.AreaShiftY = 0;
                msg.AppendLine("AreaShiftY must be bettwen from -1 to 1.");
                arg.AppendLine("AreaShiftY");
            }

            if ((setting.RatioX < 0.1) || (setting.RatioX > 1))
            {
                setting.RatioX = 1;
                msg.AppendLine("RatioX must be bettwen from 0.1 to 1.");
                arg.AppendLine("RatioX");
            }

            if ((setting.RatioY < 0.1) || (setting.RatioY > 1))
            {
                setting.RatioY = 1;
                msg.AppendLine("RatioY must be bettwen from 0.1 to 1.");
                arg.AppendLine("RatioY");
            }

            if ((Math.Abs(setting.ShiftX) + Math.Abs(setting.RatioX)) > 1)
            {
                setting.ShiftX = 0;
                msg.AppendLine("ShiftX is invalid number.");
                arg.AppendLine("ShiftX");
            }

            if ((Math.Abs(setting.ShiftY) + Math.Abs(setting.RatioY)) > 1)
            {
                setting.ShiftY = 0;
                msg.AppendLine("ShiftY is invalid number.");
                arg.AppendLine("ShiftY");
            }

            if ((Math.Abs(setting.AreaShiftX) + Math.Abs(setting.ShiftX) + Math.Abs(setting.RatioX)) > 1)
            {
                setting.ShiftX = 0;
                msg.AppendLine("ShiftX is invalid number.");
                arg.AppendLine("ShiftX");
            }

            if ((Math.Abs(setting.AreaShiftY) + Math.Abs(setting.ShiftY) + Math.Abs(setting.RatioY)) > 1)
            {
                setting.ShiftY = 0;
                msg.AppendLine("ShiftY is invalid number.");
                arg.AppendLine("ShiftY");
            }

            if (msg.Length > 0)
            {
                throw new ArgumentException(msg.ToString(), arg.ToString());
            }
        }

        public override string ToString()
        {
            return "NiDaq6353-" + daqDevice;
        }

    }
}
