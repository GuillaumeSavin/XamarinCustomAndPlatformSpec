using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace CustomAndPlatformSpec
{
    public partial class RoundedImage : ContentView
    {
        private double startScale;
        private double currentScale;
        private double xOffset;
        private double yOffset;
        private bool fullScreen;
        private double oldSize;
        

        public RoundedImage()
        {
            InitializeComponent();
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
            if (e.Status == GestureStatus.Started)
            {
                startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
            }

            if (e.Status == GestureStatus.Running)
            {
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(1, currentScale);

                // The ScaleOrigin is in relative coordinatesto the wrapped UI element.
                double renderedX = Content.X + xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (Content.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                double renderedY = Content.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates
                double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

                // Apply translation based on the change in origin
                Content.TranslationX = Math.Min(0, Math.Max(targetX, -Content.Width * (currentScale - 1)));
                Content.TranslationY = Math.Min(0, Math.Max(targetY, -Content.Height * (currentScale - 1)));

                // Apply scale factor
                Content.Scale = currentScale;
            }

            if (e.Status == GestureStatus.Completed)
            { // Store the deltas of the wrapped UI element
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
            }

        }

        public void OnDoubleTap(object sender, EventArgs e)
        {
            Image img = (Image)sender;

            if(fullScreen)
            {
                Size = oldSize;
                fullScreen = false;
            } else
            {
                oldSize = Size;
                Size = Application.Current.MainPage.Width;
                fullScreen = true;
            }

        }
    }
}
