﻿using CloudClassroom.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace CloudClassroom.ViewModels
{
    public class Thumbnail : BindableBase
    {
        public string ThumbnailUri { get; set; }
        public int PageNum { get; set; }
        public StrokeCollection Strokes { get; set; }
    }


    public class WhiteboardViewModel : BindableBase
    {
        public WhiteboardViewModel(bool shareDoc = false)
        {
            InitCurrentStatus();

            InitThumbnails(shareDoc);

            InitCommands();
        }

        private void InitCurrentStatus()
        {
            Thumbnails = new ObservableCollection<Thumbnail>();
            PageNums = new ObservableCollection<int>();

            CurrentDrawingAttributes = new DrawingAttributes();
            CurrentDrawingAttributes.Color = Colors.Red;
            CurrentDrawingAttributes.Height = 3;
            CurrentDrawingAttributes.Width = 3;

            CurrentEditingMode = InkCanvasEditingMode.Ink;

            CurrentThickness = 1;
            CurrentColor = "Red";

            IsPenSelected = true;
        }

        private void InitCommands()
        {
            NoteDetailTriggerCommand = new DelegateCommand(() =>
            {
                NoteDetailVisible = !NoteDetailVisible;
            });

            ThumbnailDetailTriggerCommand = new DelegateCommand(() =>
            {
                ThumbnailDetailVisible = !ThumbnailDetailVisible;
            });

            PreviousPageCommand = new DelegateCommand(() =>
            {
                if (CurrentPageNum - 1 > 0)
                {
                    CurrentPageNum -= 1;
                }
            });

            NextPageCommand = new DelegateCommand(() =>
            {
                if (CurrentPageNum + 1 <= Thumbnails.Count)
                {
                    CurrentPageNum += 1;
                }
            });

            PenSelectedCommand = new DelegateCommand(() =>
            {
                IsPenSelected = true;
                CurrentEditingMode = InkCanvasEditingMode.Ink;
            });

            EraserSelectedCommand = new DelegateCommand(() =>
            {
                IsEraserSelected = true;
                CurrentEditingMode = InkCanvasEditingMode.EraseByStroke;
            });

            ClearStrokesCommand = new DelegateCommand(() =>
            {
                CurrentThumbnail?.Strokes?.Clear();
            });

            ThicknessSelectedCommand = new DelegateCommand<string>((thickness) =>
            {
                int t;
                if (int.TryParse(thickness, out t))
                {
                    CurrentThickness = t;
                    CurrentDrawingAttributes.Height = t + 2;
                    CurrentDrawingAttributes.Width = t + 2;
                }
            });

            ColorSelectedCommand = new DelegateCommand<string>((color) =>
            {
                CurrentColor = color;
                CurrentDrawingAttributes.Color = (Color)ColorConverter.ConvertFromString(color);
            });


            OpenStrokesFromFileCommand = new DelegateCommand(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "json文件|*.json";
                openFileDialog.Multiselect = false;

                bool? result = openFileDialog.ShowDialog();

                if (result.HasValue && result.Value)
                {
                    string jsonFile = File.ReadAllText(openFileDialog.FileName,Encoding.UTF8);

                    IList<StrokeModel> strokes = JsonConvert.DeserializeObject<IList<StrokeModel>>(jsonFile);

                    if (strokes == null)
                    {
                        return;
                    }

                    CurrentThumbnail.Strokes.Clear();

                    foreach (var stroke in strokes)
                    {
                        StylusPointCollection stylusPointCollection = new StylusPointCollection();

                        foreach (var point in stroke.Points)
                        {
                            StylusPoint stylusPoint = new StylusPoint()
                            {
                                PressureFactor = 0.5f,
                                X = point.X,
                                Y = point.Y,
                            };

                            stylusPointCollection.Add(stylusPoint);
                        }

                        DrawingAttributes drawingAttributes = new DrawingAttributes()
                        {
                            Color = (Color)ColorConverter.ConvertFromString(stroke.ColorString),
                            Width = stroke.Width,
                            Height = stroke.Height,
                        };

                        CurrentThumbnail.Strokes.Add(new Stroke(stylusPointCollection, drawingAttributes));
                    }
                }
            });

            SaveStrokesToFileCommand = new DelegateCommand(() =>
            {
                IList<StrokeModel> strokes = new List<StrokeModel>();

                foreach (Stroke s in CurrentThumbnail.Strokes)
                {
                    StrokeModel stroke = new StrokeModel()
                    {
                        ColorString = s.DrawingAttributes.Color.ToString(),
                        Width = s.DrawingAttributes.Width,
                        Height = s.DrawingAttributes.Height,
                    };

                    IList<PointModel> points = new List<PointModel>();

                    foreach (StylusPoint sp in s.StylusPoints)
                    {
                        PointModel p = new PointModel()
                        {
                            X = sp.X,
                            Y = sp.Y
                        };

                        points.Add(p);
                    }

                    stroke.Points = points;
                    strokes.Add(stroke);
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "json文件|*.json";
                bool? result = saveFileDialog.ShowDialog();

                if (result.HasValue && result.Value)
                {

                    string json = JsonConvert.SerializeObject(strokes, Formatting.Indented);

                    File.WriteAllText(saveFileDialog.FileName, json, Encoding.UTF8);
                }
            });
        }

        private void InitThumbnails(bool shareDoc)
        {
            if (shareDoc)
            {
                Thumbnails.Add(new Thumbnail()
                {
                    PageNum = 1,
                    ThumbnailUri = "../../Images/1.png",
                    Strokes = new StrokeCollection(),
                });
                Thumbnails.Add(new Thumbnail()
                {
                    PageNum = 2,
                    ThumbnailUri = "../../Images/2.png",
                    Strokes = new StrokeCollection(),
                });
                Thumbnails.Add(new Thumbnail()
                {
                    PageNum = 3,
                    ThumbnailUri = "../../Images/3.png",
                    Strokes = new StrokeCollection(),
                });
                Thumbnails.Add(new Thumbnail()
                {
                    PageNum = 4,
                    ThumbnailUri = "../../Images/4.png",
                    Strokes = new StrokeCollection(),
                });
                Thumbnails.Add(new Thumbnail()
                {
                    PageNum = 5,
                    ThumbnailUri = "../../Images/5.png",
                    Strokes = new StrokeCollection(),
                });
                Thumbnails.Add(new Thumbnail()
                {
                    PageNum = 6,
                    ThumbnailUri = "../../Images/6.png",
                    Strokes = new StrokeCollection(),
                });
                Thumbnails.Add(new Thumbnail()
                {
                    PageNum = 7,
                    ThumbnailUri = "../../Images/7.png",
                    Strokes = new StrokeCollection(),
                });
                //Thumbnails.Add(new Thumbnail()
                //{
                //    PageNum = 8,
                //    ThumbnailUri = "../../Images/8.png",
                //    Strokes = new StrokeCollection(),
                //});
                //Thumbnails.Add(new Thumbnail()
                //{
                //    PageNum = 9,
                //    ThumbnailUri = "../../Images/9.png",
                //    Strokes = new StrokeCollection(),
                //});
                //Thumbnails.Add(new Thumbnail()
                //{
                //    PageNum = 10,
                //    ThumbnailUri = "../../Images/10.png",
                //    Strokes = new StrokeCollection(),
                //});
                //Thumbnails.Add(new Thumbnail()
                //{
                //    PageNum = 11,
                //    ThumbnailUri = "../../Images/11.png",
                //    Strokes = new StrokeCollection(),
                //});
                //Thumbnails.Add(new Thumbnail()
                //{
                //    PageNum = 12,
                //    ThumbnailUri = "../../Images/12.png",
                //    Strokes = new StrokeCollection(),
                //});
                //Thumbnails.Add(new Thumbnail()
                //{
                //    PageNum = 13,
                //    ThumbnailUri = "../../Images/13.png",
                //    Strokes = new StrokeCollection(),
                //});
                //Thumbnails.Add(new Thumbnail()
                //{
                //    PageNum = 14,
                //    ThumbnailUri = "../../Images/14.png",
                //    Strokes = new StrokeCollection(),
                //});
                //Thumbnails.Add(new Thumbnail()
                //{
                //    PageNum = 15,
                //    ThumbnailUri = "../../Images/15.png",
                //    Strokes = new StrokeCollection(),
                //});
                //Thumbnails.Add(new Thumbnail()
                //{
                //    PageNum = 16,
                //    ThumbnailUri = "../../Images/16.png",
                //    Strokes = new StrokeCollection(),
                //});
                //Thumbnails.Add(new Thumbnail()
                //{
                //    PageNum = 17,
                //    ThumbnailUri = "../../Images/17.png",
                //    Strokes = new StrokeCollection(),
                //});
            }

            if (Thumbnails.Count == 0)
            {
                Thumbnails.Add(new Thumbnail()
                {
                    PageNum=1,
                    Strokes = new StrokeCollection(),
                    ThumbnailUri = "",
                });
            }



            PageNums.Clear();
            for (int i = 1; i <= Thumbnails.Count; i++)
            {
                PageNums.Add(i);
            }


            if (Thumbnails.Count > 0)
            {
                CurrentThumbnail = Thumbnails[0];
                CurrentPageNum = CurrentThumbnail.PageNum;
            }
        }

        public DrawingAttributes CurrentDrawingAttributes { get; set; }

        private InkCanvasEditingMode _currentEditingMode;
        public InkCanvasEditingMode CurrentEditingMode
        {
            get { return _currentEditingMode; }
            set { SetProperty(ref _currentEditingMode, value); }
        }

        public ObservableCollection<Thumbnail> Thumbnails { get; set; }

        public ObservableCollection<int> PageNums { get; set; }

        private Thumbnail _currentThumbnail;
        public Thumbnail CurrentThumbnail
        {
            get { return _currentThumbnail; }
            set
            {
                SetProperty(ref _currentThumbnail, value);
                if (CurrentPageNum != value.PageNum)
                {
                    CurrentPageNum = value.PageNum;
                }
            }
        }

        private int _currentPageNum;
        public int CurrentPageNum
        {
            get { return _currentPageNum; }
            set
            {
                SetProperty(ref _currentPageNum, value);
                if (CurrentThumbnail.PageNum != value)
                {
                    CurrentThumbnail = Thumbnails.FirstOrDefault(t => t.PageNum == value);
                }
            }
        }

        private bool _isPenSelected;
        public bool IsPenSelected
        {
            get { return _isPenSelected; }
            set
            {
                SetProperty(ref _isPenSelected, value);
                if (value)
                {
                    IsEraserSelected = false;
                }
            }
        }

        private bool _isEraserSelected;
        public bool IsEraserSelected
        {
            get { return _isEraserSelected; }
            set {

                SetProperty(ref _isEraserSelected, value);
                if (value)
                {
                    IsPenSelected = false;
                }
            }
        }

        private bool _noteDetailVisible;
        public bool NoteDetailVisible
        {
            get { return _noteDetailVisible; }
            set { SetProperty(ref _noteDetailVisible, value); }
        }

        private bool _thumbnailDetailVisible;
        public bool ThumbnailDetailVisible
        {
            get { return _thumbnailDetailVisible; }
            set { SetProperty(ref _thumbnailDetailVisible, value); }
        }

        private int _currentThickness;
        public int CurrentThickness
        {
            get { return _currentThickness; }
            set { SetProperty(ref _currentThickness, value); }
        }

        private string _currentColor;
        public string CurrentColor
        {
            get { return _currentColor; }
            set { SetProperty(ref _currentColor, value); }
        }


        public ICommand NoteDetailTriggerCommand { get; set; }
        public ICommand ThumbnailDetailTriggerCommand { get; set; }
        public ICommand PreviousPageCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand PenSelectedCommand { get; set; }
        public ICommand EraserSelectedCommand { get; set; }
        public ICommand ClearStrokesCommand { get; set; }
        public ICommand ThicknessSelectedCommand { get; set; }
        public ICommand ColorSelectedCommand { get; set; }

        public ICommand OpenStrokesFromFileCommand { get; set; }
        public ICommand SaveStrokesToFileCommand { get; set; }
    }
}
