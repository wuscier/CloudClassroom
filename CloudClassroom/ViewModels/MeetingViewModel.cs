using CloudClassroom.CustomizedUI;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows.Input;

namespace CloudClassroom.ViewModels
{
    public class MeetingViewModel:BindableBase
    {
        public MeetingViewModel()
        {
            InitData();
        }

        private void InitData()
        {
            WindowSizeChangedCommand = new DelegateCommand(() =>
            {

            });


            WindowLocationChangedCommand = new DelegateCommand(() =>
            {

            });
        }
        
        public ICommand WindowSizeChangedCommand { get; set; }
        public ICommand WindowLocationChangedCommand { get; set; }

        public ICommand MicrophoneTriggerCommand { get; set; }
        public ICommand AudioSettingsOpenedCommand { get; set; }
        public ICommand AudioSelectedCommand { get; set; }
        public ICommand SpeakerSelectedCommand { get; set; }
        public ICommand CameraTriggerCommand { get; set; }
        public ICommand VideoSettingsOpenedCommand { get; set; }
        public ICommand VideoSelectedCommand { get; set; }
        public ICommand OpenShareOptionsCommand { get; set; }
    }
}
