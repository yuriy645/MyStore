using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyStore.Infrastructure
{
    public class LogReaderService
    {
        public async Task<string> ReadLog()
        {
            string message = null;

            string path = "log.txt";
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                int maxLineCount = 1000;
                int maxLineCallCount = 40;

                var timeNow = DateTime.Now.AddHours(7).ToString("d");
                var regexDate = new Regex(@$"^CALL TIME {timeNow} .*"); //Call time {DateTime.Now.AddHours(-5)}, 
                var regexEnd = new Regex(@$"^[*][*][*].*");

                for (int i = 0; i < maxLineCount; i++)
                {
                    line = await reader.ReadLineAsync();


                    if ( (line == null)  )
                    {
                        return message;
                    }

                    if ( regexDate.IsMatch(line) ) //если было совпадение даты вызова метода, то считать все строки с текущей до строки ***
                    {
                        message = message +  line  + ",  LOG LINE NUMBER: " + i + "  <br>";  // <font color="red" >П</font>

                        for (int j = 0; j < maxLineCallCount; i++)
                        {
                            line = await reader.ReadLineAsync();
                            message = message + line  + "  <br>";
                            

                            if ( (line == null) || (regexEnd.IsMatch(line)) )
                            {
                                message = message + "  <br> <br>" ;
                                break;
                            }
                        }
                    }
                }
            }


            return message;
        }
    }
}
