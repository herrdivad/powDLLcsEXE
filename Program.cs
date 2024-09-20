using PowDLL; // must be downloaded from http://users.uoi.gr/nkourkou/powdll/ and added to the project for building.
              // If you only want to use the binary .exe app, powDLL.dll must be in the same directory! 
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Text;

// Using .NET Framework 2.0. Must be installed to execute and needs an internet connection for Windows Updates at least once. 


/**************************************************************************
 * File:        <powDLLcsEXE_netFramework.cs>
 * Author:      David Herrmann
 * Email:       <david.herrmann@kit.edu>
 * Created:     <20.09.2024>
 * 
 * Project:     <powDLLcsEXE_netFramework>
 * 
 * Description: A short C# script using powDLL.dll to convert powder XRD-RAW-Files to
 *              human-readable file formats. Its intended use is also to be
 *              used as a command line-argument for other (non-C#)-programs
 * 
 * ------------------------------------------------------------------------
 * License:
 * MIT License
 * 
 * Copyright (c) <2024> David Herrmann
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * ------------------------------------------------------------------------
 * 
 * Credits and external License:
 * This project uses the following external libraries:
 * 
 * 1. PowDLL.dll - .NET dynamic link library used for the interconversion
 *                  procedure between variable formats of Powder X-Ray files.
 *    Author: Nikos Kourkoumelis
 *    License: PowDLL is free for academic or commercial use.
 *             PowDLL is distributed in the hope that it will be useful,
 *             but WITHOUT ANY WARRANTY of being free of internal errors.
 *             The author is not responsible for erroneous results obtained with the program
 *    Website: http://users.uoi.gr/nkourkou/powdll/index.html (access 20.09.2024)
 *    
 *    Citation:
 *       Please cite if you find PowDLL useful:
 *       PowDLL, a reusable .NET component for interconverting powder diffraction data:
 *       Recent developments, N. Kourkoumelis, ICDD Annual Spring Meetings (ed. Lisa O'Neill),
 *       Powder Diffraction, 28 (2013) 137-48.
 * 
 **************************************************************************/


namespace powDLLcsEXE_netFramework
{
    internal class Program
    {
        static void Main(string[] args) // at least one args must be given and a second one could be given for other output formats beside .uxd
        {
            if (args.Length < 1)
            {
                throw new ArgumentException("To few arguments. I need at least a path to a (RAW) file I should convert!");
            }

            if (args.Length >= 3)
            {
                throw new ArgumentException("To many arguments. I need only a file and an output extension if you want (default .uxd)");
            }

            string path_to_raw = args[0];
            if (!File.Exists(path_to_raw) || !Path.GetExtension(path_to_raw).Equals(".raw", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine(path_to_raw);
                throw new ArgumentException("There is no raw-File to convert on the given path");
            }

            string output_ext;
            if (args.Length == 2) // could also be written as "string output_ext = args.Length > 1 ? args[1] : ".uxd"";
            {
                output_ext = args[1];
            }
            else
            {
                output_ext = ".uxd";
            }

            // End of input Exceptions. Real program starts here!!!

            PowderFileTypes PowDLLbinFile = new PowderFileTypes();
            PowderFileTypes.suppressMsg = true;
            PowderFileTypes.ProtectOverwrite = false; // should overwrite files or create increments (internal powdll behaviour). If default true, there will be a pop-up text box hinders automated scripting!  
            // PowderFileTypes.alwaysXY = true; // -> seems buggy, testfile gets cut

            Console.WriteLine($"Hello user, I'm ready to convert {path_to_raw} to {output_ext} using PowDLL from Nikos Kourkoumelis!");

            string output_path =  Path.GetDirectoryName(path_to_raw) + "\\" + Path.GetFileNameWithoutExtension(path_to_raw) + output_ext;

            // Console.WriteLine(output_path);

            if (PowDLLbinFile.DoFileConversion(path_to_raw, output_path, PowderFileTypes.ShowErrors.DontShowErr))
            {
                Console.WriteLine($"succ: {output_path}");
            }
            else
            {
                Console.WriteLine("failed");
            }

                
        }
    }
}
