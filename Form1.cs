using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;

namespace WeatherApplication
{
    public partial class mainForm : Form
    {
        public TextBox cityBox;
        public RichTextBox fetchedBox;
        public PictureBox weatherIcon;

        private ImageList weatherIcons;
        private Label temperature;
        private Label wind;
        private Label humidity;

        private Button unitsButton;

        private string city = "London";
        private string api = "http://api.openweathermap.org/data/2.5/weather?q=";
        private string apiKey = "&APPID=742535328578319c60a7f3a5245f164c";
        private string unit = "&units=metric";
        private string url = "";

        private string jsonData = "";
        private string weatherTitle = "";
        private string windSpeed = "";
        private string humidtyInfo = "";
        private string temperatureInfo = "";

        private readonly string celius = "°C";
        private readonly string fahrenhiet = "°F";
        private readonly string metre = " km/h";
        private readonly string miles = " mph";

        private dynamic dataObject;
        private bool isMetric = true;

        public mainForm()
        {
            InitializeComponent();
            if (CheckInternetConnection())
            {
                Setter();
                GetWeatherInfo();
            }
            else
            {
                MessageBox.Show("You are offline.");
                return;
            }

        }

        private void Setter()
        {
            cityBox = textBox1;
            cityBox.Text = "Windsor";

            weatherIcons = imageList1;
            weatherIcon = pictureBox1;

            temperature = label3;
            wind = label4;
            humidity = label5;
            unitsButton = button2;

            unitsButton.Text = isMetric ? fahrenhiet : celius;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetWeatherInfo();
        }

        private void GetWeatherInfo()
        {
            if (CheckInternetConnection())
            {
                GetData();                              //Fetches the Json Data
                SetData();                              //Sets the Data
                if (isMetric)
                    DisplayDataInMetric();
                else
                    DisplayDataInImperial();
                choosePicture(weatherTitle);
            }
            else
                MessageBox.Show("You are offline.");
        }

        private void GetData()
        {
            city = cityBox.Text;
            url = api + city + apiKey + unit;

            //Download the string from the given url.
            using (WebClient wc = new WebClient())
            {
                try
                {
                    var json = wc.DownloadString(url);
                    jsonData = json;
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                    return;
                }

            }
            dataObject = JsonConvert.DeserializeObject(jsonData);
        }

        //Extract those data and set them to appropriate Texts.
        private void SetData()
        {
            weatherTitle = dataObject.weather[0].description;
            windSpeed = dataObject.wind.speed;
            humidtyInfo = dataObject.main.humidity;
            temperatureInfo = dataObject.main.temp;
        }


        private void DisplayDataInMetric()
        {
            wind.Text = "Wind: " + windSpeed + metre;
            temperature.Text =  temperatureInfo + celius;
            humidity.Text = "Humidity: " + humidtyInfo + "%";

        }

        private void DisplayDataInImperial()
        {
            wind.Text = "Wind: " + windSpeed + miles;
            temperature.Text = temperatureInfo + fahrenhiet;
            humidity.Text = "Humidity: " + humidtyInfo + "%";

        }

        private void choosePicture(String weatherTitle)
        {
            switch (weatherTitle)
            {
                case "clear sky":
                    weatherIcon.Image = weatherIcons.Images[7];
                    break;

                case "few clouds":
                    weatherIcon.Image = weatherIcons.Images[0];
                    break;

                case "scattered clouds":
                    weatherIcon.Image = weatherIcons.Images[0];
                    break;

                case "broken clouds":
                    weatherIcon.Image = weatherIcons.Images[0];
                    break;

                case "shower rain":
                    weatherIcon.Image = weatherIcons.Images[4];
                    break;

                case "rain" :
                    weatherIcon.Image = weatherIcons.Images[4];
                    break;

                case "drizzle":
                    weatherIcon.Image = weatherIcons.Images[4];
                    break;

                case "thunderstorm":
                    weatherIcon.Image = weatherIcons.Images[6];
                    break;

                case "snow":
                    weatherIcon.Image = weatherIcons.Images[5];
                    break;

                case "mist":
                    weatherIcon.Image = weatherIcons.Images[1];
                    break;

            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isMetric)
            {
                isMetric = false;
                unitsButton.Text = celius;
                unit = "&units=imperial";
                GetWeatherInfo();
            }
            else
            {
                isMetric = true;
                unitsButton.Text = fahrenhiet;
                unit = "&units=metric";
                GetWeatherInfo();
            }
        }

        private bool CheckInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }

            }
            catch
            {
                return false;
            }
        }
    }
}
