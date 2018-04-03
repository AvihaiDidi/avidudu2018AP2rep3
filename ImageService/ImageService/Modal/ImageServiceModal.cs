﻿using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;

namespace ImageService.Modal
{
    /// <summary>
    /// A class that implemetns the ImageServiceModal interface.
    /// </summary>
    public class ImageServiceModal : IImageServiceModal
    {
        /// <summary>
        /// The Function Adds A file in the appropriate place in the file system
        /// also creates a thumbnail in the appropriate place
        /// </summary>
        /// <param name="path">The Path of the Image from the file</param>
        /// <returns>Indication if the Addition Was Successful</returns>
        public string AddFile(string path, out bool result)
        {
            result = false;
            //first check if file exists
            try
            {
                if (!System.IO.File.Exists(path))
                {
                    result = false;
                    return "AddFile error: File does not exist.";
                }
                // top-level folder name.
                string OutputPath = ConfigurationManager.AppSettings["OutputDir"];
                //get file name to update path for copying
                string FileName = System.IO.Path.GetFileName(path);
                DateTime Date = System.IO.File.GetCreationTime(path);
                //parse date into path for a new folder
                //string Time = Date.Year.ToString() + @"\" + Date.Month.ToString();
                string Time = System.IO.Path.Combine(Date.Year.ToString(), Date.Month.ToString());
                //thumbnail creation//
                int ThumbSize = Int32.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
                Thread.Sleep(1000);
                System.IO.FileStream STREAM = new FileStream(path, FileMode.Open);
                Bitmap MyBitmap;
                try
                {
                    MyBitmap = (Bitmap)Image.FromStream(STREAM);
                }
                catch (Exception e)
                {
                    return "Error: " + e.ToString() + "\n" + e.StackTrace.ToString() + "\n" + e.HelpLink;
                }
                Image MyThumbnail = MyBitmap.GetThumbnailImage(ThumbSize, ThumbSize, () => false, IntPtr.Zero);
                string TargetThumb = System.IO.Path.Combine(OutputPath, "Thumbnails");
                TargetThumb = System.IO.Path.Combine(TargetThumb, Time);

                if (!System.IO.Directory.Exists(TargetThumb))
                {
                    System.IO.Directory.CreateDirectory(TargetThumb);
                }
                MyThumbnail.Save(Path.Combine(TargetThumb, FileName));

                //pic creation//
                string TargetFile = System.IO.Path.Combine(OutputPath, Time);
                if (!System.IO.Directory.Exists(TargetFile))
                {
                    System.IO.Directory.CreateDirectory(TargetFile);
                }
                TargetFile = System.IO.Path.Combine(TargetFile, FileName);
                System.IO.File.Copy(path, TargetFile);
                //return "test1"; //todo remove this
                MyBitmap.Dispose();
                MyThumbnail.Dispose();
                STREAM.Flush();
                result = true;
                return OutputPath;
            }
            catch (Exception e)
            {
                return "Error: " + e.ToString() + "\n" + e.StackTrace.ToString() + "\n" + e.HelpLink;
            }
        }

        /*
        /// <summary>
        /// A function that retrieves the datetime WITHOUT loading the whole image.
        /// </summary>
        /// <param name="path">The path to the image.</param>
        /// <returns></returns>
        public static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream ImageFileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image MyImage = Image.FromStream(ImageFileStream, false, false))
            {
                PropertyItem PropItem = MyImage.GetPropertyItem(36867);
                string DateTaken = RegularExpression.Replace(Encoding.UTF8.GetString(PropItem.Value), "-", 2);
                return DateTime.Parse(DateTaken);
            }
        }
        */
    }
}
