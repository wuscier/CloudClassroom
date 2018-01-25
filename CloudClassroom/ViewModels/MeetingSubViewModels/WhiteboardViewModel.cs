using CloudClassroom.Events;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Ink;

namespace Classroom.ViewModels
{
    public class Thumbnail : BindableBase
    {
        public string ThumbnailUri { get; set; }
        public int PageNum { get; set; }
        public StrokeCollection Strokes { get; set; }
    }


    public class WhiteboardViewModel : BindableBase
    {
        private SubscriptionToken _nextToken;
        private SubscriptionToken _previousToken;
        private SubscriptionToken _penToken;
        private SubscriptionToken _eraserToken;
        private SubscriptionToken _clearToken;

        public WhiteboardViewModel()
        {
            SubscribeEvents();

            IsPenSelected = true;

            Thumbnails = new ObservableCollection<Thumbnail>();
            PageNums = new ObservableCollection<int>();

            InitThumbnails();

            int pageCount = Thumbnails.Count;
            InitPageNums(pageCount);
            if (pageCount > 0)
            {
                CurrentThumbnail = Thumbnails[0];
                CurrentPageNum = CurrentThumbnail.PageNum;
            }
        }


        private void SubscribeEvents()
        {
            _previousToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<PreviousPageEvent>().Subscribe((argument) =>
             {
                 if (CurrentPageNum - 1 > 0)
                 {
                     CurrentPageNum -= 1;
                 }
             }, ThreadOption.PublisherThread, true, filter => { return filter.Target == Target.WhiteboardViewModel; });

            _nextToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<NextPageEvent>().Subscribe((argument) =>
             {
                 if (CurrentPageNum + 1 <= Thumbnails.Count)
                 {
                     CurrentPageNum += 1;
                 }
             }, ThreadOption.PublisherThread, true, filter => { return filter.Target == Target.WhiteboardViewModel; });

            _penToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<PenSelectedEvent>().Subscribe((argument) =>
            {
                IsPenSelected = true;
            }, ThreadOption.PublisherThread, true, filter => { return filter.Target == Target.WhiteboardViewModel; });

            _eraserToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<EraserSelectedEvent>().Subscribe((argument) =>
            {
                IsEraserSelected = true;
            }, ThreadOption.PublisherThread, true, filter => { return filter.Target == Target.WhiteboardViewModel; });

            _clearToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<StrokesClearedEvent>().Subscribe((argument) =>
            {
                CurrentThumbnail.Strokes.Clear();
            }, ThreadOption.PublisherThread, true, filter => { return filter.Target == Target.WhiteboardViewModel; });
        }

        public void UnsubscribeEvents()
        {
            EventAggregatorManager.Instance.EventAggregator.GetEvent<NextPageEvent>().Unsubscribe(_nextToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<PreviousPageEvent>().Unsubscribe(_previousToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<PenSelectedEvent>().Unsubscribe(_penToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<EraserSelectedEvent>().Unsubscribe(_eraserToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<StrokesClearedEvent>().Unsubscribe(_clearToken);
        }

        private void InitThumbnails()
        {
            //Thumbnails.Add(new Thumbnail()
            //{
            //    PageNum = 1,
            //    ThumbnailUri = "../Images/1.png",
            //    Strokes = new StrokeCollection(),
            //});
            //Thumbnails.Add(new Thumbnail()
            //{
            //    PageNum = 2,
            //    ThumbnailUri = "../Images/2.png",
            //    Strokes = new StrokeCollection(),
            //});
            //Thumbnails.Add(new Thumbnail()
            //{
            //    PageNum = 3,
            //    ThumbnailUri = "../Images/3.png",
            //    Strokes = new StrokeCollection(),
            //});
            //Thumbnails.Add(new Thumbnail()
            //{
            //    PageNum = 4,
            //    ThumbnailUri = "../Images/4.png",
            //    Strokes = new StrokeCollection(),
            //});
            //Thumbnails.Add(new Thumbnail()
            //{
            //    PageNum = 5,
            //    ThumbnailUri = "../Images/5.png",
            //    Strokes = new StrokeCollection(),
            //});
            //Thumbnails.Add(new Thumbnail()
            //{
            //    PageNum = 6,
            //    ThumbnailUri = "../Images/6.png",
            //    Strokes = new StrokeCollection(),
            //});
            //Thumbnails.Add(new Thumbnail()
            //{
            //    PageNum = 7,
            //    ThumbnailUri = "../Images/7.png",
            //    Strokes = new StrokeCollection(),
            //});
            //Thumbnails.Add(new Thumbnail()
            //{
            //    PageNum = 8,
            //    ThumbnailUri = "../Images/8.png",
            //    Strokes = new StrokeCollection(),
            //});
            //Thumbnails.Add(new Thumbnail()
            //{
            //    PageNum = 9,
            //    ThumbnailUri = "../Images/9.png",
            //    Strokes = new StrokeCollection(),
            //});
            //Thumbnails.Add(new Thumbnail()
            //{
            //    PageNum = 10,
            //    ThumbnailUri = "../Images/10.png",
            //    Strokes = new StrokeCollection(),
            //});
            //Thumbnails.Add(new Thumbnail()
            //{
            //    PageNum = 11,
            //    ThumbnailUri = "../Images/11.png",
            //    Strokes = new StrokeCollection(),
            //});
            //Thumbnails.Add(new Thumbnail()
            //{
            //    PageNum = 12,
            //    ThumbnailUri = "../Images/12.png",
            //    Strokes = new StrokeCollection(),
            //});
            //Thumbnails.Add(new Thumbnail()
            //{
            //    PageNum = 13,
            //    ThumbnailUri = "../Images/13.png",
            //    Strokes = new StrokeCollection(),
            //});
            //Thumbnails.Add(new Thumbnail()
            //{
            //    PageNum = 14,
            //    ThumbnailUri = "../Images/14.png",
            //    Strokes = new StrokeCollection(),
            //});
            //Thumbnails.Add(new Thumbnail()
            //{
            //    PageNum = 15,
            //    ThumbnailUri = "../Images/15.png",
            //    Strokes = new StrokeCollection(),
            //});
            //Thumbnails.Add(new Thumbnail()
            //{
            //    PageNum = 16,
            //    ThumbnailUri = "../Images/16.png",
            //    Strokes = new StrokeCollection(),
            //});
            //Thumbnails.Add(new Thumbnail()
            //{
            //    PageNum = 17,
            //    ThumbnailUri = "../Images/17.png",
            //    Strokes = new StrokeCollection(),
            //});
        }

        private void InitPageNums(int totalPages)
        {
            PageNums.Clear();
            for (int i = 1; i <= totalPages; i++)
            {
                PageNums.Add(i);
            }
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



    }
}
