using System;
using System.IO;

namespace HW5
{
    class Program
    {
        static void Main(string[] args)
        {


            int i, h;
            string addressofinput, addressofconvolution, addressofoutput;
            addressofinput = Console.ReadLine();
            addressofconvolution = Console.ReadLine();
            addressofoutput = Console.ReadLine();
            double[,] returnofaddressofinput = new double[,] { };
            returnofaddressofinput = ReadImageDataFromFile(addressofinput);

            double[,] returnofaddressofconvolution = new double[,] { };
            returnofaddressofconvolution = ReadImageDataFromFile(addressofconvolution);

            ////สร้าง array ที่เก็บค่าภาพ repeated texture
            double[,] test2 = new double[returnofaddressofinput.GetLength(0) + 2, returnofaddressofinput.GetLength(1) + 2];
            for (i = 0; i < returnofaddressofinput.GetLength(1); i++)
            {
                for (h = 0; h < returnofaddressofinput.GetLength(0); h++)
                {
                    test2[i + 1, h + 1] = returnofaddressofinput[i, h];
                }
            }

            //repeatออกมาเป็นแถวหน้า 
            for (i = 0; i < returnofaddressofinput.GetLength(1); i++)
            {
                h = 0;
                test2[i + 1, h] = returnofaddressofinput[i, h + returnofaddressofinput.GetLength(0) - 1];
            }

            //repeatออกมาเป็นแถวหลัง
            for (i = 0; i < returnofaddressofinput.GetLength(1); i++)
            {
                h = 0;
                test2[i + 1, test2.GetLength(1)-1] = returnofaddressofinput[i, h];
            }

            //repeatออกมาเป็นแถวบน
            for (h = 0; h < returnofaddressofinput.GetLength(0); h++)
            {
                i = 0;
                test2[i, h + 1] = returnofaddressofinput[returnofaddressofinput.GetLength(1) - 1, h];
            }

            //repeatออกมาเป็นแถวล่าง
            for (h = 0; h < returnofaddressofinput.GetLength(0); h++)
            {
                i = 0;
                test2[i + test2.GetLength(1) - 1, h + 1] = returnofaddressofinput[i, h];
            }

            //repeatออกมาเป็นมุม
            test2[0, 0] = returnofaddressofinput[returnofaddressofinput.GetLength(1) - 1, returnofaddressofinput.GetLength(0) - 1];

            test2[0, test2.GetLength(0) - 1] = returnofaddressofinput[returnofaddressofinput.GetLength(1) - 1, 0];

            test2[test2.GetLength(0) - 1, 0] = returnofaddressofinput[0, returnofaddressofinput.GetLength(1) - 1];

            test2[test2.GetLength(1) - 1, test2.GetLength(0) - 1] = returnofaddressofinput[0, 0];

            //Convolve
            double[] mul = new double[returnofaddressofconvolution.GetLength(0)*returnofaddressofconvolution.GetLength(1)];
            double[,] mulans = new double[returnofaddressofinput.GetLength(1), returnofaddressofinput.GetLength(0)];
            double[,] output2 = new double[,] { };
            int c = 0,a=0,p=0;
            //รันค่าให้ไปคูณไปเรื่อยๆจนกว่าจะพอดีกับช่อง input ที่สร้างเพิ่ม แนวตั้ง
            for (p = 0; p < returnofaddressofinput.GetLength(1); p++)
            {
                //รันค่าให้ไปคูณไปเรื่อยๆจนกว่าจะพอดีกับช่อง input ที่สร้างเพิ่ม แนวนอน
                for (a = 0; a < returnofaddressofinput.GetLength(0); a++)
                {
                    //วนค่่าไปคูณกับfileconvolve
                    for (i = p; i < returnofaddressofconvolution.GetLength(1)+p; i++)
                    {
                        for (h = a; h < returnofaddressofconvolution.GetLength(0)+a; h++)
                        {
                            mul[c] = (test2[i, h] * returnofaddressofconvolution[i-p,h-a]);
                            c++;

                        }
                    }

                    c = 0;
                    //  นำค่ามา + กันและเก็บไว้ที่ตัวแสดงคำตอบ
                    for (int j = 0; j < mul.GetLength(0); j++)
                    {
                        mulans[p, a] = mulans[p, a] + mul[j];
                    }

                }
               
            }
            WriteImageDataToFile(addressofoutput, mulans);
            Console.WriteLine();
        }



            static double[,] ReadImageDataFromFile(string imageDataFilePath)
            {
                string[] lines = System.IO.File.ReadAllLines(imageDataFilePath);
                int imageHeight = lines.Length;
                int imageWidth = lines[0].Split(',').Length;
                double[,] imageDataArray = new double[imageHeight, imageWidth];

                for (int i = 0; i < imageHeight; i++)
                {
                    string[] items = lines[i].Split(',');
                    for (int j = 0; j < imageWidth; j++)
                    {
                        imageDataArray[i, j] = double.Parse(items[j]);
                    }
                }
                return imageDataArray;
            }



            static void WriteImageDataToFile(string imageDataFilePath,
                                         double[,] imageDataArray)
        {
            string imageDataString = "";
            for (int i = 0; i < imageDataArray.GetLength(0); i++)
            {
                for (int j = 0; j < imageDataArray.GetLength(1) - 1; j++)
                {
                    imageDataString += imageDataArray[i, j] + ", ";
                }
                imageDataString += imageDataArray[i,
                                                imageDataArray.GetLength(1) - 1];
                imageDataString += "\n";
            }

            System.IO.File.WriteAllText(imageDataFilePath, imageDataString);
        }

       
    }
}
