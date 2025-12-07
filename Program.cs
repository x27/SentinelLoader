using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace SentinelLoader
{
    internal static class Program
    {
        static void Main()
        {
            Dictionary<uint, uint> d = new Dictionary<uint, uint>
            {
                { 512000000u, 757999999u },
                { 823987500u, 849012499u },
                { 868987500u, 894012499u },
                { 960000000u, 1239999999u },
            };

            try
            {
                Assembly asm = Assembly.LoadFrom("BCDx36HP_Sentinel.exe");
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
