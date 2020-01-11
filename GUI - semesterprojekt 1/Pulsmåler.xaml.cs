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
using System.ComponentModel; //CloseEventArgs
using RPI;
using RPI.Display;
using RPI.Controller;
using RPI.Heart_Rate_Monitor;
using System.Windows.Threading;

namespace GUI___semesterprojekt_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Erklærer de 9 attributter 
        private RPi _rpi;
        private Key _startKnap;
        private PWM _pwm;
        private PulseReader _pulseReader;
        private SevenSeg _sevenSeg;
        private Led _hundredeDisplay;
        private PulsTilBCD _pulsTilBCD;
        private bool resetBool = true;
        private bool knapTændt = false;

        // Erklærer attributter der relaterer sig til layout (er ikke skrevet i klassediagram - spørg lene om dette)
        private Style cStyle;
        private double standartFontSize;
        private SolidColorBrush enabledColor;
        // Opretter annuller knappen
        System.Windows.Controls.Button annuller_BT; 

        public MainWindow()
        {
            InitializeComponent();
            _rpi = new RPi();
            _startKnap = new Key(_rpi, Key.ID.P1);
            _pwm = new PWM(_rpi);
            _pulseReader = new PulseReader(_rpi, _pwm);
            _sevenSeg = new SevenSeg(_rpi);
            _hundredeDisplay = new Led(_rpi, Led.ID.LD1);
            standartFontSize = 18;
            enabledColor = new SolidColorBrush(Color.FromArgb(255, 211, 47, 47));
            annuller_BT = new Button();
            cStyle = new Style(typeof(Border));


            // Definerer "Annuller"-knappens udseende
            annuller_BT.Click += annullerBT_Click;
            annuller_BT.Width = 185;
            annuller_BT.Height = 55;
            annuller_BT.Content = "ANNULLER";
            annuller_BT.FontFamily = new FontFamily("Yu Gothic");
            annuller_BT.FontSize = standartFontSize;
            annuller_BT.Name = "ANNULLER";
            annuller_BT.HorizontalAlignment = HorizontalAlignment.Left;
            annuller_BT.VerticalAlignment = VerticalAlignment.Top;
            annuller_BT.Margin = new Thickness(40, 170, 0, 0);

            cStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(50.0)));
            annuller_BT.Resources.Add(typeof(Border),cStyle);

            _pwm.InitPWM();
            _rpi.wait(200);
            _pwm.SetPWM(20);

        }

         // Eventhandleren til annuller_BT oprettes
         private void annullerBT_Click(object sender, EventArgs e)
        {
            resetLayout();
        }
        // Eventhandler for "Klargør måling"-knappen
        private void KlargørMåling_BT_Click(object sender, RoutedEventArgs e)
        {
            // Disabler de to knapper
            disable(KlargørMåling_BT);
            disable(Reset_BT);
            //Reset_BT.IsEnabled = false;
            Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);

            // Skriver 00 på sevenSeg displaysne
            _sevenSeg.Disp_SevenSeg(00);
            _hundredeDisplay.off();

            while (knapTændt == false)
            {
                if (_startKnap.isPressed())
                {
                    knapTændt = true;
                    _pulseReader.StartReading();
                    _pwm.SetPWM(100);

                }
            }
            while (knapTændt == true)
            {
                if (_startKnap.isPressed())
                {
                    knapTændt = false;
                    _pulseReader.StartReading();
                    showPulse();
                    erOverHundrede();
                    Historik_LB.Items.Add(_pulseReader.ReadCalculatedPulse().ToString());
                    enable(KlargørMåling_BT);
                    enable(Reset_BT);
                }
            }
        }

        // Metode til at resette layoutet
        private void resetLayout()
        {
            enable(KlargørMåling_BT);
            Reset_BT.Margin = new Thickness(40, 170, 0, 0);
            mitGitter.Children.Remove(annuller_BT);
            resetBool = true;
        }

        // Fire metoder vi selv har lavet oprettes
        private void showPulse()
        {
            _pulsTilBCD = new PulsTilBCD(_pulseReader.getCalculatedPulse());
            _sevenSeg.Init_SevenSeg();
            _sevenSeg.Disp_SevenSeg(_pulsTilBCD.getPulsTilBCD());
        }

        private void erOverHundrede()
        {
            if (_pulseReader.getCalculatedPulse() >= 100)
            {
                _hundredeDisplay.on();
            }
        }

        private void disable(Button name)
        {
            name.IsEnabled = false;
            name.Foreground = Brushes.LightGray;
        }

        private void enable(Button name)
        {
            name.Background = enabledColor;
            name.Foreground = Brushes.White;
            name.IsEnabled = true;
        }

        
        // Eventhandler for "Reset historik"-knappen
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
            

            Historik_LB.Items.Add("Dato\t\t\t Puls");

            cStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(50.0)));
            enable(KlargørMåling_BT);
            enable(Reset_BT);

            KlargørMåling_BT.Resources.Add(typeof(Border), cStyle);
            Reset_BT.Resources.Add(typeof(Border), cStyle);

        }
        
        
        // Eventhandleren slukker for status LED og alle 7-segments displays
        private void Window_Closed(object sender, EventArgs e)
        {

            _pwm.StopPWM();
            _hundredeDisplay.off();
            _sevenSeg.Close_SevenSeg();
        }



    }
}
