using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
                async () => this.Visitors = await RefreshList());
        }

        private async void Init()
        {
            this.Terminal = await ClientIdentiy.Get();
            this.Visitors = await RefreshList();
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

            using (HttpClient client = new HttpClient())
            {
                var url = "http://localhost:9958/Api/AddVisitor";
                var content = new StringContent(JsonConvert.SerializeObject(v));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = await client.PostAsync(url, content);
            }

            //clear the UI
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Company = string.Empty;
            this.Sponsor = string.Empty;

            this.Visitors = await RefreshList();
        }

        private bool CanCheckin()
        {
            return !string.IsNullOrEmpty(this.FirstName)
                && !string.IsNullOrEmpty(this.LastName);
        }


        private async Task Checkout()
        {
            using (HttpClient client = new HttpClient())
            {
                var tmp = this.SelectedVisitor;
                tmp.DepartedAt = DateTime.Now;
                var url = "http://localhost:9958/Api/CheckoutVisitor";
                var content = new StringContent(JsonConvert.SerializeObject(tmp));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = await client.PostAsync(url, content);
            }

            this.Visitors = await RefreshList();
        }

        private bool CanCheckout()
        {
            return this.Visitors != null
                && this.Visitors.Any()
                && this.SelectedVisitor != null;
        }

        private async Task<List<Visitor>> RefreshList()
        {
            //refresh the list of people
            using (HttpClient client = new HttpClient())
            {
                var url = string.Format("http://localhost:9958/Api/GetVisitors/{0}", terminal.Key);
                var result = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<List<Visitor>>(result).OrderBy(x => x.ToString()).ToList();
            }
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
