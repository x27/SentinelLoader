using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SentinelLoader
{
    internal static class Program
    {
        static void Main()
        {
            const string US_EXE = "BCDx36HP_Sentinel.exe";
            const string EU_EXE = "UBCD3600XLT_Sentinel.exe";

            Dictionary<uint, uint> d = new Dictionary<uint, uint>
            {
                { 512000000u, 757999999u },
                { 823987500u, 849012499u },
                { 868987500u, 894012499u },
                { 960000000u, 1239999999u },
            };

            try
            {
                string exeFilename = string.Empty;
                if (File.Exists(US_EXE))
                    exeFilename = US_EXE;
                else if (File.Exists(EU_EXE))
                    exeFilename = EU_EXE;
                else
                {
                    MessageBox.Show("No executable Sentinel file found!", "Error");
                    return;
                }

                Assembly asm = Assembly.LoadFrom(exeFilename);
                var freqLibType = asm.GetType("HomePatrol_Sentinel.FrequencyLib");
                var freqTableField = freqLibType.GetField("FrequencyTable", BindingFlags.Static | BindingFlags.NonPublic);
                var freqTable = freqTableField.GetValue(null) as uint[,];
                for (var i = 0; i < freqTable.GetLength(0); i++)
                {
                    if (d.ContainsKey(freqTable[i, 1]))
                    {
                        freqTable[i, 1] = d[freqTable[i, 1]];
                    }
                }
                asm.EntryPoint.Invoke(null, null);
            }
            catch
            {
                MessageBox.Show("Something wrong!","Exception");
            }
        }
    }
}
