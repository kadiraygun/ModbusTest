using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyModbus;
using System.IO.Ports;

namespace ModbusTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //1.Adım :Tools>Nuget Package Manager>Package Manager Console
            //2.Adım :Package Sıyrce:nuget.org
            //3.Adım :Default Project>[Kullanılıcak Proje seçilir]
            //4.Adım :PM> Install-Package EasyModbusTCP

            ModbusClient modbus = Ethernet("10.0.0.9", 502);
            //ModbusClient modbus = SerialPortModbus("Com2", 1, 9600, Parity.Even, StopBits.One);
            Test(modbus);

        }


        /// <summary>
        /// Delta PLC için;[Modbus adresleri Projede "Modbus Adresleri.xls" üzerinden görülebilir
        /// </summary>
        /// <param name="ip">ip Adresi</param>
        /// <param name="port">Delta plc Default Port 502</param>
        /// <returns></returns>
        static ModbusClient Ethernet(string ip, int port) => new ModbusClient(ip, port = 502);
        /// <summary>
        /// Delta PLC için;[Modbus adresleri Projede "Modbus Adresleri.xls" üzerinden görülebilir
        /// </summary>
        /// <param name="ComPort">Com adresi</param>
        /// <param name="Adresi">plc adresi 1-255</param>
        /// <param name="budrate">default 9600</param>
        /// <param name="parity">even</param>
        /// <param name="stopbit">One</param>
        /// <returns></returns>
        static ModbusClient SerialPortModbus(string ComPort, byte Adresi, int budrate = 9600, Parity parity = Parity.Even, StopBits stopbit = StopBits.One) => new ModbusClient()
        {
            SerialPort = ComPort,
            Baudrate = budrate,
            Parity = parity,
            UnitIdentifier = Adresi,
            StopBits = stopbit,
        };


        static void Test(ModbusClient modbus)
        {

            //Örnek Delta PLC için;[Modbus adresleri Projede "Modbus Adresleri.xls" üzerinden görülebilir]
            //1 -Bit Okuma M0 için;
            //                  HEX ADRESİ:0x08 0x00 (0800)
            //              Decimal Adresi:2048
            int M0Adres = 2048;
            var miktar = 3;// okunacak register miktarı
            bool[] okunanbitler = modbus.ReadCoils(M0Adres, miktar);//okunacak register miktarı kadar okuma yapar            
            bool M0 = okunanbitler[0];
            bool M1 = okunanbitler[1];
            bool M2 = okunanbitler[2];
            /*.
              .
              .
            */

            //1-Bit Yazma M500 için;
            //              HEX ADRESİ:0x09 0xF4 (09F4)
            //          Decimal Adresi:2548
            int M500Adres = 2548;
            bool M500eYazılacakDeğer = true;
            modbus.WriteSingleCoil(M500Adres, M500eYazılacakDeğer);

            //1 -Word Okuma D0 İçin;
            //                  HEX ADRESİ:0x01 0x00 (1000)
            //              Decimal Adresi:4096
            int D0Adres = 4096;
            miktar = 3;// okunacak register miktarı
            int[] okunanwordlar = modbus.ReadHoldingRegisters(D0Adres, miktar);
            int D0 = okunanwordlar[0];
            int D1 = okunanwordlar[1];
            int D2 = okunanwordlar[2];
            /*.
              .
              .
            */

            //1 -Word Yazma D500 İçin;
            //                  HEX ADRESİ:0x11 0xF4 (11F4)
            //              Decimal Adresi:4596
            int D500Adres = 4596;
            int D500eYazılacakDeğer = 1852;

            modbus.WriteSingleRegister(D500Adres, D500eYazılacakDeğer);
        }
    }
}
