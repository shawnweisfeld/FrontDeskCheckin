using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontDeskCheckinClient.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Init();
            CheckinCommand = new RelayCommand(
                async () => await Checkin(), 
                () => CanCheckin());
            CheckoutCommand = new RelayCommand(
                async () => await Checkout(),
                () => CanCheckout());
            RefreshCommand = new RelayCommand(
                async () => this.Visitors = await ApiService.GetVisitors(this.Terminal.Key));
        }

        private async void Init()
        {
            this.Terminal = await ClientIdentiy.Get();
            this.Visitors = await ApiService.GetVisitors(this.Terminal.Key);
        }

        private async Task Checkin()
        {
            //Add this person to the list
            var v = new Visitor()
            {
                FirstName = FirstName,
                LastName = LastName,
                Company = Company,
                Sponsor = Sponsor,
                ArrivedAt = DateTime.Now,
                Terminal = Terminal
            };

            await ApiService.AddVisitor(v);

            //clear the UI
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Company = string.Empty;
            this.Sponsor = string.Empty;

            this.Visitors = await ApiService.GetVisitors(this.Terminal.Key);
        }

        private bool CanCheckin()
        {
            return !string.IsNullOrEmpty(this.FirstName)
                && !string.IsNullOrEmpty(this.LastName);
        }


        private async Task Checkout()
        {
            await ApiService.Checkout(this.SelectedVisitor);

            this.Visitors = await ApiService.GetVisitors(this.Terminal.Key);
        }

        private bool CanCheckout()
        {
            return this.Visitors != null
                && this.Visitors.Any()
                && this.SelectedVisitor != null;
        }

        private Terminal terminal;

        public Terminal Terminal
        {
            get
            {
                
                return terminal;
            }
            set
            {
                terminal = value;
                RaisePropertyChanged();
            }
        }

        private List<Visitor> visitors;

        public List<Visitor> Visitors
        {
            get
            {

                return visitors;
            }
            set
            {
                visitors = value;
                RaisePropertyChanged();
            }
        }

        private Visitor selectedVisitor;

        public Visitor SelectedVisitor
        {
            get
            {

                return selectedVisitor;
            }
            set
            {
                selectedVisitor = value;
                RaisePropertyChanged();
                CheckoutCommand.RaiseCanExecuteChanged();
            }
        }


        private string firstName;
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
                RaisePropertyChanged();
                CheckinCommand.RaiseCanExecuteChanged();
            }
        }

        private string lastName;
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
                RaisePropertyChanged();
                CheckinCommand.RaiseCanExecuteChanged();
            }
        }

        private string company;
        public string Company
        {
            get
            {
                return company;
            }
            set
            {
                company = value;
                RaisePropertyChanged();
                CheckinCommand.RaiseCanExecuteChanged();
            }
        }

        private string sponsor;
        public string Sponsor
        {
            get
            {
                return sponsor;
            }
            set
            {
                sponsor = value;
                RaisePropertyChanged();
                CheckinCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand CheckinCommand
        {
            get;
            private set;
        }

        public RelayCommand CheckoutCommand
        {
            get;
            private set;
        }

        public RelayCommand RefreshCommand
        {
            get;
            private set;
        }

    }
}
