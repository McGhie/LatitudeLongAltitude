using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using GoogleApi.Entities.Search.Common.Response;
using Newtonsoft.Json;

namespace LatitudeLongAltitude
{
    internal class Program
    {
        private static IEnumerable<string[]> LoadCsvData(string path, params char[] separator)
        {
            return from line in File.ReadLines(path)
                   let parts = (from p in line.Split(separator, StringSplitOptions.RemoveEmptyEntries) select p)
                   select parts.ToArray();
        }

        /*https://maps.googleapis.com/maps/api/elevation/json?locations=40.714728,-73.998672 */

        public static string code(string Url)
        {
            var myRequest = (HttpWebRequest)WebRequest.Create(Url);

            myRequest.Method = "GET";
            var myResponse = myRequest.GetResponse();
            var sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            var result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();

            return result;
        }


        public static string Altitude(string latlon500)
        {
            var key = "&key=AIzaSyB_k1TpqKs0r2XPl3IqwC0pd73yxDaXOmM";
            var Alt = "";
            var googleElavation = "https://maps.googleapis.com/maps/api/elevation/json?locations=";
            var elevation = "";
            var filenameNew2 = @"C:\Users\blue\Desktop\New folder\entries2.json";


            googleElavation = googleElavation + latlon500 + key;

            Console.WriteLine(googleElavation);
            File.WriteAllText(filenameNew2, googleElavation);

            try
            {
                elevation = code(googleElavation);
                var ele = elevation.Split('[');

                var elevations = ele[1].Split(']');

                elevation = string.Join("", elevations[0]);
                elevation = elevation + ',';
                Console.WriteLine(elevation);


                return elevation;
            }
            catch (Exception e)
            {
                return latlon500;
            }

            /*

            string[] ele = elevation.Split('[');

            string[] elevations = ele[2].Split(']');
            Console.Write(ele[0]);
            Console.WriteLine("ALt" + elevations[0]);


            return elevations[0];
            */
        }


        private static void getthedata()
        {
            var filename = @"C:\Users\blue\Desktop\New folder\postcodes.csv";
            var filenameNew = @"C:\Users\blue\Desktop\New folder\entriesAllmost.json";
            var split = ',';


            var postcode = "";
            var suburb = "";
            var lat = "1";
            var lon = "1";
            var latlon = "1,1";
            var latlon500 = "";
            var counter = 0;
            var totalElevations = "";
            var simpleCounter = 1;


            var lines = LoadCsvData(filename, ',');
            var Allentry = "";

            if (lines != null)
            {
                var howmanyline = lines.Count();

                foreach (var line in lines)
                {
                    try
                    {
                        postcode = line[0].Replace('"', ' ');
                        suburb = line[1].Replace('"', ' ');
                        lat = line[5].Replace('"', ' ');
                        lon = line[6].Replace('"', ' ');
                        latlon = lat + ',' + lon;
                    }
                    catch (Exception e)
                    {
                    }


                    /*   string entry = ('"' + "Location" + '"' + " : {" +
                                        "PostCode" + '"' + ": " + '"' + postcode + '"' + ',' +
                                        " Suburb" + '"' + ": " + '"' + suburb + '"' + ',' +
                                        " Latitude" + '"' + ": " + '"' + lat + '"' + ',' +
                                        " Longitude" + '"' + ": " + '"' + lon + '"' + ',' +
                                        " Altitude" + '"' + ": " + '"' + elv + '"' + "}\n");

                        Console.Write(entry); 

                        Allentry = Allentry + entry;*/


                    counter++;
                    simpleCounter++;
                    if (simpleCounter == howmanyline)
                    {
                        latlon500 = latlon500 + latlon;

                        var elv = Altitude(latlon500);
                        totalElevations = totalElevations + elv;
                        File.WriteAllText(filenameNew, totalElevations);
                    }

                    else if ((counter <= 100) && (simpleCounter != howmanyline))
                    {
                        latlon500 = latlon500 + latlon + '|';
                    }
                    else if (counter == 101)
                    {
                        latlon500 = latlon500 + latlon;

                        var elv = Altitude(latlon500);
                        totalElevations = totalElevations + elv;
                        File.WriteAllText(filenameNew, totalElevations);
                        latlon500 = "";
                        counter = 0;
                    }
                }
            }
        }


        private static void createfile()
        {
            var filename = @"C:\Users\blue\Desktop\New folder\postcodes.csv";
            var googelelvationF = @"C:\Users\blue\Desktop\New folder\entriesAllmost.json";
            var filenameNew = @"C:\Users\blue\Desktop\New folder\data.json";
            var filenameSmall = @"C:\Users\blue\Desktop\New folder\datasmall.js";

            var split = ',';


            var postcode = "";
            var suburb = "";
            var lat = "1";
            var lon = "1";
            var latlon = "1,1";
            var count = 0;
            var ele = "1";
            var elevationList = readJsonfile(googelelvationF);

            var listsize = elevationList.Count;


            var lines = LoadCsvData(filename, ',');
            var Allentry = "";
            var SmallEntry = "var locations = [";


            if (lines != null)
            {
                var howmanyline = lines.Count();

                foreach (var line in lines)
                {
                    try
                    {
                        postcode = line[0].Replace('"', ' ');
                        suburb = line[1].Replace('"', ' ');
                        lat = line[5].Replace('"', ' ');
                        lon = line[6].Replace('"', ' ');
                        ele = elevationList[count++];
                    }
                    catch (Exception e)
                    {
                    }

                    try
                    {
                        var small = '[' + postcode + ',' + '"' + suburb + '"' + ',' + lat + ',' + lon + ',' + ele + "],";
                        SmallEntry = SmallEntry + small;

                        File.WriteAllText(filenameSmall, SmallEntry);
                        Console.Write(small);
                    }
                    catch (Exception e)
                    {
                    }


                    var entry = '"' + "Location" + '"' + " : {" +
                                "PostCode" + '"' + ": " + '"' + postcode + '"' + ',' +
                                " Suburb" + '"' + ": " + '"' + '"' + suburb + '"' + '"' + ',' +
                                " Latitude" + '"' + ": " + '"' + lat + '"' + ',' +
                                " Longitude" + '"' + ": " + '"' + lon + '"' + ',' +
                                " Altitude" + '"' + ": " + '"' + ele + '"' + "},\n{";

                    //   Console.Write(entry);

                    //       Allentry = Allentry + entry;
                }
                //     File.WriteAllText(filenameNew, Allentry);
            }
        }


        private static List<string> readJsonfile(string filename)
        {
            var elevationList = new List<string>();

            using (var r = new StreamReader(filename))
            {
                var json = r.ReadToEnd();
                var items = JsonConvert.DeserializeObject<List<Item>>(json);

                dynamic array = JsonConvert.DeserializeObject(json);
                var i = 0;
                foreach (var item in array)
                {
                    var newele = item.elevation.ToString();
                    //Console.WriteLine();
                    try
                    {
                        elevationList.Add(newele);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("error");
                    }
                }
            }


            //  elevationList.ForEach(Console.WriteLine);
            //Console.WriteLine(elevationList[7]);
            //   Console.ReadKey();

            return elevationList;
        }


        private static void minimise(string file)
        {
        }


        private static void Main(string[] args)
        {
            //            var filename = @"C:\Users\blue\Desktop\New folder\entriesAllmost.json";
            //   getthedata();

            createfile();


            /*
            try
            {
                restringTheData(filename);
            }
            catch (Exception e)
            {
                Console.WriteLine("oh no");
            }
            */
        } //end of main
    }
}