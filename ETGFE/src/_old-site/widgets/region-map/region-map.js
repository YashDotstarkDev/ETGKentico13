import { Loader } from '@googlemaps/js-api-loader'

const googleMapsApi = new Loader({
  apiKey: GOOGLE_MAPS_API_KEY,
  version: '3.exp',
  // libraries: ['places'], // See: https://developers.google.com/maps/documentation/javascript/libraries
})

if ($('.widget.region-map').length) {
  googleMapsApi.load().then(async google => {
    const { default: InfoBox } = await import('../../plugins/infobox/infobox.js')

    Promise.all([
      import('./region-map.scss'),
      import('../../plugins/markerclusterer/markerclusterer.js')
    ]).then(() => {
      $('.widget.region-map').each(function (i, el) {
        $(el).data('widget', new RegionMap(el, google, InfoBox))
        $(el).data('widget').init()
      })
    })
  })
}

function RegionMap (el, google, InfoBox) {
  const self = this;
  self.el = $(el);
  self.regions = [];
  self.markers = [];
  self.bounds = new google.maps.LatLngBounds();

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('RegionMap init', self);
    }
    self.el.css('opacity', 1);

    // maps
    self.icon = {
      url: self.el.find('.map-container').attr('data-pin'),
      size: new google.maps.Size(88, 102),
      origin: new google.maps.Point(0, 0),
      anchor: new google.maps.Point(22, 51),
      scaledSize: new google.maps.Size(44, 51)
    };
    self.markers = [];
    self.ibTemplate = require('./infobox-template.hbs');

    self.infoBoxOptions = {
      disableAutoPan: false
      ,maxWidth: 0
      ,pixelOffset: new google.maps.Size(-245, -50)
      ,zIndex: 1
      ,boxStyle: {
        width: '490px'
      }
      ,closeBoxMargin: "0px 0px 0px 0px"
      ,closeBoxURL: self.el.find('.map-container').attr('data-close-image')
      ,infoBoxClearance: new google.maps.Size(40, 40)
      ,isHidden: false
      ,pane: 'floatPane'
      ,enableEventPropagation: false
      ,alignBottom: true
    };
    self.infoBox = new InfoBox(self.infoBoxOptions);
    // self.infoBox = null;

    self.parseLocations();
  }

  self.parseLocations = function() {
    self.el.find('.region-list .region').each(function(i, el){
      self.regions.push({
        index: i,
        id: $(el).attr('data-id'),
        latitude: parseFloat($(el).attr('data-lat')),
        longitude: parseFloat($(el).attr('data-lng')),
        region: $(el).attr('data-region').trim(),
        summary: $(el).attr('data-summary').trim(),
        url: $(el).attr('data-url')
      });

      self.bounds.extend({lat: parseFloat($(el).attr('data-lat')), lng: parseFloat($(el).attr('data-lng'))});

    });

    self.initMap();
  };

  self.initMap = function () {
    // create the map
    self.map = new google.maps.Map(self.el.find('.map-canvas').get(0), {
      center: {lat: -34.397, lng: 150.644},
      zoom: 8,
      maxZoom: 19,
      zoomControl: true,
      disableDefaultUI: true
    });

    self.map.fitBounds(self.bounds);

    // for each region create a marker
    $.each(self.regions, function(index, region) {

      var marker = new google.maps.Marker({
        map: self.map,
        icon: self.icon,
        position: {lat: region.latitude, lng: region.longitude},
        title: region.title
      });

      // create infobox content
      var ibContent = self.ibTemplate(region);

      // define what to do on marker click
      marker.addListener('click', function() {
        self.map.setCenter(marker.getPosition());
        self.infoBox.open(self.map, this);
        self.infoBox.setContent(ibContent);
      });

      self.markers.push(marker);


      // match the region id and add an index?
      self.el.find('.region[data-id="' + region.id + '"]').attr('data-index', index);

    });

    self.markerCluster = new MarkerClusterer(self.map, self.markers, {
      styles: [
        {
          textColor: 'white',
          url: self.el.find('.map-container').attr('data-cluster-image-small'),
          height: 69,
          width: 60,
          textSize:20
        },
        {
          textColor: 'white',
          url: self.el.find('.map-container').attr('data-cluster-image-medium'),
          height: 69,
          width: 60,
          textSize:20
        },
        {
          textColor: 'white',
          url: self.el.find('.map-container').attr('data-cluster-image-large'),
          height: 69,
          width: 60,
          textSize:20
        }
      ]
    });



  };


}



