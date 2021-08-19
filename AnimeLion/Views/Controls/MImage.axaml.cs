using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Threading;
using CodeProject.Downloader;
using ReactiveUI;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using SysImage = System.Drawing.Image;

namespace AnimeLion.Views.Controls
{
    [PseudoClasses("isloading")]
    public class MImage : ContentControl
    {
        public static readonly StyledProperty<string> SourceProperty =
            AvaloniaProperty.Register<MImage, string>(nameof(Source));
        
        public static readonly StyledProperty<Stretch> StretchProperty =
            AvaloniaProperty.Register<MImage, Stretch>(nameof(Stretch));
        
        public static readonly StyledProperty<bool> IsLoadedProperty =
            AvaloniaProperty.Register<MImage, bool>(nameof(IsLoaded), true);
        
        private Image partImage;
        private Panel partGrid;
        private Border controlPresenter;
        private bool isInitialize = false;
        private CancellationToken Token;
        private CancellationTokenSource Cts;
        private FileDownloader fileDownloader = new();

        private static List<BitmapCached> cacheBitmaps = new List<BitmapCached>();

        public string Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        
        public Stretch Stretch
        {
            get => GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }
        
        public bool IsLoaded
        {
            get => GetValue(IsLoadedProperty);
            set => SetValue(IsLoadedProperty, value);
        }

        public MImage()
        {
            PseudoClasses.Set(":isloading", true);
            this.WhenAnyValue(x => x.Source).Subscribe(SourceChanged);
            
            Cts = new CancellationTokenSource();
            Token = Cts.Token;

            fileDownloader.Token = Token;
        }

        private void SourceChanged(string changedSource)
        {
            LoadImage();
        }
        
        private void LoadImage(bool isCachedNot = true)
        {
            if(Source == null) return;
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\cache\"))
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\cache\");
            var imgPath = Directory.GetCurrentDirectory() + @"\cache\" + Path.GetFileName(new Uri(Source).AbsolutePath);
            if(isCachedNot)
            {
                foreach (var bitmapCached in cacheBitmaps)
                    if (bitmapCached.NameBitmap == imgPath)
                    {
                        ImageInitialize(bitmapCached.Bitmap);
                        return;
                    }
            }

            fileDownloader.DownloadComplete += (sender, args) =>
            {
                Dispatcher.UIThread.InvokeAsync(() => ImageInitialize(imgPath, args.TotalFileSize));
            };
            fileDownloader.AsyncDownload(Source, imgPath);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            partImage = e.NameScope.Find<Image>("Image");
            controlPresenter = e.NameScope.Find<Border>("ControlVisibleWhileLoaded");
            controlPresenter.Width = Width;
            controlPresenter.Height = Height;
            partGrid = e.NameScope.Find<Panel>("PART");
            LoadImage();
            isInitialize = false;
        }
        
        private void ImageInitialize(Bitmap imgPath)
        {
            partImage.Source = imgPath;
            partImage.Opacity = 1;
            controlPresenter.Opacity = 0;
            // if (Height > imgPath.Size.Height)
            //     Height = imgPath.Size.Height;
            // if (Width > imgPath.Size.Width)
            //     Width = imgPath.Size.Width;
            IsLoaded = false;
            PseudoClasses.Set(":isloading", false);
        }

        private void ImageInitialize(string imgPath, long size)
        {
            var imageBitmap = new Bitmap(imgPath);

            partImage.Source = imageBitmap;
            cacheBitmaps.Add(new BitmapCached(imageBitmap, imgPath, size));
            partImage.Opacity = 1;
            controlPresenter.Opacity = 0;
            // if (Height > imageBitmap.Size.Height)
            //     Height = imageBitmap.Size.Height;
            // if (Width > imageBitmap.Size.Width)
            //     Width = imageBitmap.Size.Width;
            IsLoaded = false;
            PseudoClasses.Set(":isloading", false);
        }

        public long GetFileSize(string url)
        {
            WebResponse response = null;
            long size = -1;
            try
            {
                response = WebRequest.Create(url).GetResponse();
                size = response.ContentLength;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }

            return size;
        }
        
        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            Cts.Cancel();
        }

        protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromLogicalTree(e);
            Cts.Cancel();
        }
    }

    public class BitmapCached
    {
        public Bitmap Bitmap;
        public string NameBitmap;
        public long TotalSize;

        public BitmapCached(Bitmap bt, string name, long totalSize)
        {
            Bitmap = bt;
            NameBitmap = name;
            TotalSize = totalSize;
        }
    }
}