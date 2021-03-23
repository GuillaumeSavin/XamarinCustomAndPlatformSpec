using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CustomAndPlatformSpec
{ 
    public partial class RoundedImage : ContentView
    { 
        public RoundedImage()
        {
            InitializeComponent();

            var pinchGesture = new PinchGestureRecognizer();

            pinchGesture.PinchUpdated += OnPinchUpdated;
            GestureRecognizers.Add(pinchGesture);
        }

        public static readonly BindableProperty ImageProperty = BindableProperty.Create(nameof(Image), typeof(ImageSource), typeof(RoundedImage), null, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (newValue is ImageSource image && bindable is RoundedImage)
            {
                ((RoundedImage)bindable).image.Source = (ImageSource)newValue;
            }
        });

        public static readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(Frame), typeof(double), typeof(RoundedImage), null, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (newValue is Double frame && bindable is RoundedImage)
            {
                ((RoundedImage)bindable).frame.HeightRequest = (double)newValue;
                ((RoundedImage)bindable).frame.WidthRequest = (double)newValue;
                ((RoundedImage)bindable).frame.CornerRadius = Convert.ToSingle((double)newValue);
            }
        });

        public ImageSource Image
        {
            get
            {
                return (ImageSource)GetValue(ImageProperty);
            }

            set
            {
                SetValue(ImageProperty, value);
            }
        }

        public Double Size
        {
            get
            {
                return (Double)GetValue(SizeProperty);
            }

            set
            {
                SetValue(SizeProperty, value);
            }
        }

        public void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {

        }

    }
}
