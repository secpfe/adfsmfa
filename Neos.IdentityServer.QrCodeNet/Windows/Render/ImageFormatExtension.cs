﻿//******************************************************************************************************************************************************************************************//
// Copyright (c) 2011 George Mamaladze                                                                                                                                                      //
//                                                                                                                                                                                          //
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),                                       //
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,   //
// and to permit persons to whom the Software is furnished to do so, subject to the following conditions:                                                                                   //
//                                                                                                                                                                                          //
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.                                                           //
//                                                                                                                                                                                          //
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,                                      //
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,                            //
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                               //
//                                                                                                                                                                                          //
//******************************************************************************************************************************************************************************************//

using System;
using System.Windows.Media.Imaging;

namespace Neos.IdentityServer.MultiFactor.QrEncoding.Windows.Render
{
    public static class ImageFormatExtension
    {
        public static BitmapEncoder ChooseEncoder(this ImageFormatEnum imageFormat)
        {
            switch (imageFormat)
            {
                case ImageFormatEnum.BMP:
                    return new BmpBitmapEncoder();
                case ImageFormatEnum.GIF:
                    return new GifBitmapEncoder();
                case ImageFormatEnum.JPEG:
                    return new JpegBitmapEncoder();
                case ImageFormatEnum.PNG:
                    return new PngBitmapEncoder();
                case ImageFormatEnum.TIFF:
                    return new TiffBitmapEncoder();
                case ImageFormatEnum.WDP:
                    return new WmpBitmapEncoder();
                default:
                    throw new ArgumentOutOfRangeException("imageFormat", imageFormat, "No such encoder support for this imageFormat");
            }
        }
    }
}
