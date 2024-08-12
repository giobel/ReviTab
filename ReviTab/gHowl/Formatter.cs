using System;
using System.Collections.Generic;
using System.Text;

namespace gHowl
{
    class Formatter
    {
        //singleton
        static Formatter _instance = new Formatter();

        static Formatter() { }
        private Formatter() { }

        public static Formatter Instance
        {
            get
            {
                return _instance;
            }
        }

        const int tenToThird = 1000;
        const int twoToTenth = 1024;
        string[] FormatBinaryBytesOrders = new string[] { "EiB", "PiB", "TiB", "GiB", "MiB", "KiB", "Bytes" };
        string[] FormatBitsBase10Orders = new string[] { "Eb", "Pb", "Tb", "Gb", "Mb", "kb", "bit" };
        string[] FormatBytesBase10Orders = new string[] { "EB", "PB", "TB", "GB", "MB", "kB", "Bytes" };

        public string FormatBitsBase10(long bits)
        {
            const int scale = tenToThird;
            long max = (long)Math.Pow(scale, FormatBitsBase10Orders.Length - 1);
            foreach (string order in FormatBitsBase10Orders)
            {
                if (bits > max)
                    return string.Format("{0:##.##} {1}", bits / max, order);
                max /= scale;
            }
            return "0 bit";
        }

        public string FormatBytesBase10(long bytes)
        {
            const int scale = tenToThird;
            long max = (long)Math.Pow(scale, FormatBytesBase10Orders.Length - 1);
            foreach (string order in FormatBytesBase10Orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", bytes / max, order);
                max /= scale;
            }
            return "0 Bytes";
        }

        public string FormatBinaryBytes(long bytes)
        {

            long max = (long)Math.Pow(twoToTenth, FormatBinaryBytesOrders.Length - 1);
            foreach (string order in FormatBinaryBytesOrders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", bytes / max, order);
                max /= twoToTenth;
            }
            return "0 Bytes";
        }

        public byte[] AsciiBytes(List<string> txt)
        {
            int letterCount = 0;
            if (txt.Count > 0)
            {
                letterCount += (txt[0] == null ? 0 : txt[0].Length);
            }
            for (int i = 1; i < txt.Count; i++)
            {
                letterCount += 2 + (txt[i] == null ? 0 : txt[i].Length);
            }
            StringBuilder sb = new StringBuilder(letterCount);
            if (txt.Count > 0)
            {
                sb.Append(txt[0]);
            }
            for (int i = 1; i < txt.Count; i++)
            {
                sb.Append("\r\n");
                sb.Append(txt[i]);
            }

            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        double[] bufferArray = new double[0];
        public byte[] DoubleArray(List<double> list, byte[] buffer)
        {
            if (bufferArray.Length != list.Count)
            {
                bufferArray = new double[list.Count];
            }
            list.CopyTo(0, bufferArray, 0, list.Count);
            Buffer.BlockCopy(bufferArray, 0, buffer, 0, Math.Min(list.Count * sizeof(double), buffer.Length));
            return buffer;
        }
    }
}
