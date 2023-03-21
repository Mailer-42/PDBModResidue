using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PDBModResidue
{
    class Program
    {
        static string _LogFileName = "";
        static string _InputFileName = "";
        static string _OutputFileName = "";
        static int _TotalLinesOfInputFile = 0;
        static string _OutputData = "";
        static int _ATOMLineNumber = 0;
        static int _ResidueNumber = 0;
        static string _AminoAcidPreviousName = "";
        static string _AminoAcidCurrentName = "";
        static string _Session1 = "";
        static string _Session2 = "";
        static string _Session3 = "";
        static string _Session4 = "";
        static string _Session5 = "";
        static string _Session6 = "";
        static int _InputDataLength = 0;
        static string _InputDataLine = "";
        static string[] _ChainLetterList = { "A", "B", "C", "D", "E", "F", "G"};
        static int _ChainLetterIndex = 0;
        static string _ChainLetter = "";
        static string _Elenment = "";

        static void Main(string[] args)
        {
            try
            {
                //Define the log file name and path
                _LogFileName = ".\\log\\" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".log";
                //Show Author
                WriteConsoleAndLogFile("===========================================================");
                WriteConsoleAndLogFile("PDB file modify the Residue sector");
                WriteConsoleAndLogFile("Power by Mailer");
                WriteConsoleAndLogFile("Version 1.0.0 Copry Rigth 19/04/2022");
                WriteConsoleAndLogFile("===========================================================");

                //Determine parameter
                if (args.Length == 1)
                {
                    if (args[0].ToString().Trim() == "?")
                    {
                        //Show help
                        WriteConsoleAndLogFile("To execute this application please follow below commang :");
                        WriteConsoleAndLogFile("PDBModResidue.exe input_file_name output_file_name");
                        WriteConsoleAndLogFile("Example : ");
                        WriteConsoleAndLogFile("PDBModResidue.exe result.pdb output.pdb");
                    }
                    else
                    {
                        //Show help
                        WriteConsoleAndLogFile("To display help please follow below command :");
                        WriteConsoleAndLogFile("PDBModResidue.exe ?");
                    }
                }
                else if (args.Length == 2)
                {
                    //Get parameter information
                    _InputFileName = ".\\input\\" + args[0].ToString().Trim();
                    _OutputFileName = ".\\output\\" + args[1].ToString().Trim();

                    //Log
                    WriteConsoleAndLogFile("Input file name :" + _InputFileName);
                    WriteConsoleAndLogFile("Output file name :" + _OutputFileName);

                    //Check existing file
                    if (System.IO.File.Exists(_InputFileName))
                    {
                        //Log
                        WriteConsoleAndLogFile("Read all lines of input file");

                        //*********************************
                        //Read input file
                        //*********************************
                        List<string> InputData = readFileAllLine(_InputFileName);
                        //Get total line number
                        _TotalLinesOfInputFile = InputData.Count;
                        //Log
                        WriteConsoleAndLogFile("Total lines of input file : " + _TotalLinesOfInputFile.ToString().Trim());
                        //Process on input file
                        if (_TotalLinesOfInputFile > 0)
                        {
                            //Read input fil
                            for (int i = 0; i < _TotalLinesOfInputFile; i++)
                            {
                                //Get Input data line
                                _InputDataLine = InputData[i];
                                _InputDataLength = _InputDataLine.Length;                                
                                //Get Input data line length
                                //Log
                                WriteConsoleAndLogFile("Process on line number : " + (i + 1).ToString() + "/" + _TotalLinesOfInputFile.ToString());
                                WriteConsoleAndLogFile("Input line data : " + _InputDataLine);
                                WriteConsoleAndLogFile("Input line data length : " + _InputDataLength);
                                //Set ChainLetet
                                _ChainLetter = _ChainLetterList[_ChainLetterIndex];

                                if (_InputDataLine.Trim() != "END")
                                {
                                    //Get data session
                                    _Session1 = _InputDataLine.Substring(0, 4);
                                    if (_Session1 == "ATOM")
                                    {
                                        //Get Element
                                        _Elenment = _InputDataLine.Substring(13, 4);

                                        //Get Session
                                        _Session2 = ""; //Atom running number
                                        _Session3 = _InputDataLine.Substring(11, 10);
                                        _Session4 = ""; //Chain Letter
                                        _Session5 = ""; //Residue running number
                                        _Session6 = _InputDataLine.Substring(26, 54);

                                        //Get amino acid name
                                        _AminoAcidCurrentName = _InputDataLine.Substring(17, 4);

                                        //Atom running line
                                        _ATOMLineNumber++;

                                        //Check for amino acid name
                                        if (_AminoAcidCurrentName != _AminoAcidPreviousName)
                                        {
                                            _ResidueNumber++;
                                            _AminoAcidPreviousName = _AminoAcidCurrentName;
                                        }
                                        else
                                        {
                                            if(_Elenment.Trim() == "N")
                                            {
                                                _ResidueNumber++;
                                            }
                                        }
                                        //Define Amino Acid number for output file
                                        _Session2 = PutSpacePrefix(_ATOMLineNumber.ToString().Trim(), 7, " ");
                                        //Define Chain letter
                                        _Session4 = _ChainLetter;
                                        //Define Residue number for output file 
                                        _Session5 = PutSpacePrefix(_ResidueNumber.ToString().Trim(), 4, " ");
                                    }
                                    else if (_Session1.Trim() == "TER")
                                    {
                                        _ChainLetterIndex++;
                                        _ResidueNumber = 0;
                                        _AminoAcidPreviousName = "";
                                        _AminoAcidCurrentName = "";
                                        _Session1 = _InputDataLine;
                                        _Session2 = ""; //Atom running number
                                        _Session3 = "";
                                        _Session4 = ""; //Chain Letter
                                        _Session5 = ""; //Residue running number
                                        _Session6 = "";
                                    }
                                    else
                                    {
                                        _Session1 = _InputDataLine;
                                        _Session2 = ""; //Atom running number
                                        _Session3 = "";
                                        _Session4 = ""; //Chain Letter
                                        _Session5 = ""; //Residue running number
                                        _Session6 = "";
                                    }
                                }
                                else
                                {
                                    _Session1 = _InputDataLine;
                                    _Session2 = ""; //Atom running number
                                    _Session3 = "";
                                    _Session4 = ""; //Chain Letter
                                    _Session5 = ""; //Residue running number
                                    _Session6 = "";
                                }
                                //*********************************
                                //Output data
                                //*********************************
                                _OutputData = _Session1 + _Session2 + _Session3 + _Session4 + _Session5 + _Session6;
                                //Log
                                WriteConsoleAndLogFile("Output line data : " + _OutputData);
                                //Write output file
                                WriteOutputFile(_OutputData);

                                //Delay
                                //System.Threading.Thread.Sleep(100);
                            }
                        }
                        else
                        {
                            //log when file has no any data to process
                            WriteConsoleAndLogFile("No data to process");
                        }
                    }
                    else
                    {
                        //Show warning message
                        WriteConsoleAndLogFile("Error :");
                        WriteConsoleAndLogFile("Input file does not exist");
                    }
                }
                else
                {
                    //Show warning message
                    WriteConsoleAndLogFile("Parameter is invalid");
                    //Show help
                    WriteConsoleAndLogFile("To display help please follow below command :");
                    WriteConsoleAndLogFile("PDBModResidue.exe ?");
                }
            }
            catch(Exception ex)
            {
                //End of process message
                WriteConsoleAndLogFile("Error");
                WriteConsoleAndLogFile(ex.Message.ToString());
            }
            finally
            {
                //End of process message
                WriteConsoleAndLogFile("===========================================================");
                WriteConsoleAndLogFile("Process done.");
                WriteConsoleAndLogFile("===========================================================");
                WriteConsoleAndLogFile("PDB file modify the Residue sector");
                WriteConsoleAndLogFile("Power by Mailer");
                WriteConsoleAndLogFile("Version 1.0.0 Copry Rigth 19/04/2022");
                WriteConsoleAndLogFile("===========================================================");
            }
        }


        /// <summary>
        /// To read all lines
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static List<string> readFileAllLine(string fileName)
        {
            try
            {
                List<string> listOfStringLine = new List<string>();
                string strLine = "***Nothing***";
                //Read file
                string[] lines = System.IO.File.ReadAllLines(fileName);
                if (lines.Length > 0)
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        strLine = lines[i].ToString();
                        listOfStringLine.Add(strLine);
                    }
                }

                return listOfStringLine;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// To Append text into file
        /// need to set output file name property
        /// </summary>
        /// <param name="TextToWrite"></param>
        /// 
        private static void WriteOutputFile(string TextToWrite)
        {
            try
            {
                //Write output file
                using (FileStream fs = new FileStream(_OutputFileName, FileMode.Append, FileAccess.Write))
                {
                    StreamWriter writer = new StreamWriter(fs);
                    writer.WriteLine(TextToWrite);
                    writer.Close();
                    writer.Dispose();
                    fs.Close();
                    fs.Dispose();
                }
            }
            catch (Exception ex)
            {
                using (FileStream fs = new FileStream(_LogFileName + ".error", FileMode.Append, FileAccess.Write))
                {
                    StreamWriter writer = new StreamWriter(fs);
                    writer.WriteLine(TextToWrite);
                    writer.WriteLine("Error");
                    writer.WriteLine(ex.Message);
                    writer.Close();
                    writer.Dispose();
                    fs.Close();
                    fs.Dispose();
                }
                //Throw exception
                throw ex;
            }
        }

        /// <summary>
        /// To Append text into file
        /// need to set LogFileName property
        /// </summary>
        /// <param name="TextToWrite"></param>
        /// 
        private static void WriteConsoleAndLogFile(string TextToWrite)
        {
            try
            {
                //Text to write with date time
                TextToWrite = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " : " + TextToWrite;
                //Write on console
                Console.WriteLine(TextToWrite);
                //Write log file
                using (FileStream fs = new FileStream(_LogFileName, FileMode.Append, FileAccess.Write))
                {
                    StreamWriter writer = new StreamWriter(fs);
                    writer.WriteLine(TextToWrite);
                    writer.Close();
                    writer.Dispose();
                    fs.Close();
                    fs.Dispose();
                }
            }
            catch (Exception ex)
            {
                using (FileStream fs = new FileStream(_LogFileName + ".error", FileMode.Append, FileAccess.Write))
                {
                    StreamWriter writer = new StreamWriter(fs);
                    writer.WriteLine(TextToWrite);
                    writer.WriteLine("Error");
                    writer.WriteLine(ex.Message);
                    writer.Close();
                    writer.Dispose();
                    fs.Close();
                    fs.Dispose();
                }
                //Throw exception
                throw ex;
            }
        }

        /// <summary>
        /// To add prefix to string
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="TotalFieldLength"></param>
        /// <param name="Prefix"></param>
        /// <returns></returns>
        private static string PutSpacePrefix(string Data, int TotalFieldLength, string Prefix)
        {
            try
            {
                string Result = Data.Trim();
                for(int i = 0; i < (TotalFieldLength - Data.Trim().Length); i++)
                {
                    Result = Prefix + Result;
                }
                return Result;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
