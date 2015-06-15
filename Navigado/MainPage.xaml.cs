using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Navigado
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private String BingMapsToken = "BingMapsToken by Bing Maps dev center https://www.bingmapsportal.com/";

        private ObservableCollection<NavLink> _navLinks = new ObservableCollection<NavLink>()
        {
            new NavLink() { Label = "Map", Symbol = Windows.UI.Xaml.Controls.Symbol.Map  },
            new NavLink() { Label = "MapDrive", Symbol = Windows.UI.Xaml.Controls.Symbol.MapDrive },
            new NavLink() { Label = "MapPin", Symbol = Windows.UI.Xaml.Controls.Symbol.MapPin },
            new NavLink() { Label = "Camera", Symbol = Windows.UI.Xaml.Controls.Symbol.Camera },
        };

        public ObservableCollection<NavLink> NavLinks
        {
            get { return _navLinks; }
        }

        public BasicGeoposition seattleLocation = new BasicGeoposition()
        {
            //Geopoint for Seattle 
            Latitude = 47.604,
            Longitude = -122.329
        };
        
        public BasicGeoposition spaceNeedlePosition = new BasicGeoposition()
        {
            //Geopoint for Seattle 
            Latitude = 47.6204,
            Longitude = -122.3491
        };

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Splitter.IsPaneOpen = (Splitter.IsPaneOpen == true) ? false : true;
            //StatusBorder.Visibility = Visibility.Collapsed;
        }

        private async void MapControlLoaded(object sender, RoutedEventArgs e)
        {
            myMapControl.Center = new Geopoint(seattleLocation);
            myMapControl.ZoomLevel = 12;

            if (myMapControl.Is3DSupported)
            {
                this.myMapControl.Style = MapStyle.Aerial3DWithRoads;
                
                Geopoint spaceNeedlePoint = new Geopoint(seattleLocation);

                MapScene spaceNeedleScene = MapScene.CreateFromLocationAndRadius(spaceNeedlePoint,
                                                                                    400, /* show this many meters around */
                                                                                    135, /* looking at it to the south east*/
                                                                                    60 /* degrees pitch */);

                await myMapControl.TrySetSceneAsync(spaceNeedleScene);
            }
            else
            {
                //string status = "3D views are not supported on this device.";
                this.myMapControl.Style = MapStyle.Aerial;
            }

            MapIcon seattleMapIcon = new MapIcon();
            seattleMapIcon.Location = new Geopoint(seattleLocation);
            seattleMapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
            seattleMapIcon.Title = "Parking here";
            seattleMapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/mappin.png"));
            myMapControl.MapElements.Add(seattleMapIcon);
        }

        private void NavLinksList_ItemClick(object sender, ItemClickEventArgs e)
        {
            String label = (e.ClickedItem as NavLink).Label;
            switch (label)
            {
                case "Map":
                    if (myMapControl.Is3DSupported)
                    {
                        if ( this.myMapControl.Style == MapStyle.Aerial3DWithRoads)
                        {
                            this.myMapControl.Style = MapStyle.Road;
                        }
                        else
                        {
                            this.myMapControl.Style = MapStyle.Aerial3DWithRoads;
                        }

                        MapIcon seattleMapIcon = new MapIcon();
                        seattleMapIcon.Location = new Geopoint(seattleLocation);
                        seattleMapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
                        seattleMapIcon.Title = "Parking here";
                        seattleMapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/mappin.png"));
                        myMapControl.MapElements.Add(seattleMapIcon);
                    }
                        break;
                default:
                    break;
            }
        }
    }
}
