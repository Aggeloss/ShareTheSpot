using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static Android.Gms.Maps.GoogleMap;

namespace ShareTheSpot.Droid
{
    [Activity(Label = "DisplayMap")]
    class DisplayMap : Activity, IOnMapReadyCallback, IInfoWindowAdapter
    {
        private GoogleMap mMap;

        public DisplayMap() { }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            SetUpMap();
        }

        public void SetUpMap()
        {
            if (mMap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;

            LatLng latlng = new LatLng(37.98782705000001, 23.73179732609043);

            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 10);
            mMap.MoveCamera(camera);

            MarkerOptions options = new MarkerOptions()
                .SetPosition(latlng)
                .SetTitle("New York")
                .SetSnippet("AKA: The Big Apple")
                .SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueBlue))
                .Draggable(true);

            mMap.AddMarker(options);

            //mMap.MarkerClick += mMap_MarkerClick;
            mMap.MarkerDragEnd += mMap_MarkerDragEnd;

            mMap.SetInfoWindowAdapter(this);
        }

        private void mMap_MarkerDragEnd(object sender, GoogleMap.MarkerDragEndEventArgs e)
        {
            LatLng pos = e.Marker.Position;
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            //This never executes or hits a break point
            var customPopup = LayoutInflater.Inflate(Resource.Layout.info_window, null);

            var titleTextView = customPopup.FindViewById<TextView>(Resource.Id.title);
            if (titleTextView != null)
            {
                titleTextView.Text = marker.Title;
            }

            var snippetTextView = customPopup.FindViewById<TextView>(Resource.Id.snippet);
            if (snippetTextView != null)
            {
                snippetTextView.Text = marker.Snippet;
            }

            return customPopup;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            //throw new NotImplementedException();
            return null;
        }
    }
}