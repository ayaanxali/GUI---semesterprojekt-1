using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RPI;
using RPI.Display;
using RPI.Controller;
using RPI.Heart_Rate_Monitor;

namespace GUI___semesterprojekt_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static RPi rpi = new RPi();
        private static Key startKnap = new Key(rpi, Key.ID.P2);
        private static PWM _pwm = new PWM(rpi);
        private static PulseReader PR = new PulseReader(rpi, _pwm);
        private static SevenSeg sevenseg = new SevenSeg(rpi);
        private static PulsTilBCD pulsConverterBCD; // = new PulsTilBCD(0);
        private static Led _hundredeDisplay = new Led(rpi, Led.ID.LD1);
        private static Style cStyle = new Style(typeof(Border));

        double standartFontSize = 18;
        bool resetBool = true;
        bool knapTændt = false;
        //bool breakOut = false;
        SolidColorBrush enabled = new SolidColorBrush(Color.FromArgb(255, 211, 47, 47));
        SolidColorBrush disabled = new SolidColorBrush(Color.FromArgb(255, 227, 95, 95));
       
        public MainWindow()
        {
            InitializeComponent();

            annuller_BT.Click += annullerBT_Click;
            annuller_BT.Width = 185;
            annuller_BT.Height = 55;
            annuller_BT.Content = "ANNULLER";
            annuller_BT.FontFamily = new FontFamily("Yu Gothic");
            annuller_BT.FontSize = standartFontSize;
            annuller_BT.Name = "Annuller";
            annuller_BT.HorizontalAlignment = HorizontalAlignment.Left;
            annuller_BT.VerticalAlignment = VerticalAlignment.Top;
            annuller_BT.Margin = new Thickness(40, 170, 0, 0);

            cStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(50.0)));
            annuller_BT.Resources.Add(typeof(Border),cStyle);
                       
        }
        System.Windows.Controls.Button annuller_BT = new Button();


        //Eventhandleren til annuller_BT oprettes
        //protected void annullerBT_Click(object sender, EventArgs e)
        private void annullerBT_Click(object sender, EventArgs e)
        {
            resetLayout();
        }

        private void resetLayout()
        {
            enable(KlargørMåling_BT);
            Reset_BT.Margin = new Thickness(40, 170, 0, 0);
            mitGitter.Children.Remove(annuller_BT);
            resetBool = true;
        }

        //Fire metoder vi selv har lavet oprettes
        private void showPulse()
        {
            pulsConverterBCD = new PulsTilBCD(PR.getCalculatedPulse());
            sevenseg.Init_SevenSeg();
            sevenseg.Disp_SevenSeg(pulsConverterBCD.getPulsTilBCD());
        }

        private void erOverHundrede()
        {
            if (PR.getCalculatedPulse() >= 100)
            {
                _hundredeDisplay.on();
            }
        }

        private void disable(Button name)
        {
            
            name.IsEnabled = false;
            name.Background = disabled;
            name.Foreground = Brushes.LightGray;
        }

        private void enable(Button name)
        {
            name.Background = enabled;
            name.Foreground = Brushes.White;
            name.IsEnabled = true;
        }

        private void KlargørMåling_BT_Click(object sender, RoutedEventArgs e)
        {
            disable(KlargørMåling_BT);
            disable(Reset_BT);

            // skriver 00 på sevenSeg displaysne
            sevenseg.Disp_SevenSeg(0);
            _hundredeDisplay.off();
            rpi.wait(1000);

            while (knapTændt == false)
            {
                
                if (startKnap.isPressed())
                {

                    knapTændt = true;
                    PR.StartReading();
                    _pwm.SetPWM(100);


                    // test af om metoden virker og får vist en messagebox
                    //MessageBox.Show("Der er nu trykket på knappen 1. gang og en måling er i gang");
                }
            }
            //MessageBox.Show(knapTændt.ToString());
            while (knapTændt == true)
            {
                if (startKnap.isPressed())
                {
                    knapTændt = false;
                    PR.StartReading();
                    showPulse();
                    erOverHundrede();
                    //MessageBox.Show("Der er nu trykket på knappen 2. gang og en måling er afsluttet");
                    Historik_LB.Items.Add(PR.ReadCalculatedPulse().ToString());
                    enable(KlargørMåling_BT);
                    enable(Reset_BT);
                }
            }
            sevenseg.Disp_SevenSeg(0x60);
            rpi.wait(2000);
            sevenseg.Disp_SevenSeg(0088);
        }

        private void Reset_BT_Click(object sender, RoutedEventArgs e)
        {
            if (resetBool)
            {
                Reset_BT.Margin = new Thickness(40, 250, 0, 0);
                
                enable(annuller_BT);
                disable(KlargørMåling_BT);
                mitGitter.Children.Add(annuller_BT);

                resetBool = false;

            }

            else if (resetBool == false)
            {
                resetLayout();
                Historik_LB.Items.Clear();
                Historik_LB.Items.Add("Dato\t\t Puls");
            }
        }

        private void Historik_LB_Loaded(object sender, RoutedEventArgs e)
        {
            _pwm.InitPWM();
            _pwm.SetPWM(20);
            Historik_LB.Items.Add("Dato\t\t\t Puls");

            cStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(50.0)));
            enable(KlargørMåling_BT);
            enable(Reset_BT);

            KlargørMåling_BT.Resources.Add(typeof(Border), cStyle);
            Reset_BT.Resources.Add(typeof(Border), cStyle);
        }

    }
}
