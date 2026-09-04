$cmsj(document).ready(function(e) {
   
    if ($cmsj('.form-select-status').val() != "Confirmed") {
        $cmsj('#field_BookingReference label').addClass("hide");
        $cmsj('#field_DepartureDate label').addClass("hide");
        $cmsj('#field_ReturnDate label').addClass("hide");
        $cmsj('#field_BookingReference .form-control').addClass("hide");
        $cmsj('#field_DepartureDate .control-group-inline').addClass("hide");
        $cmsj('#field_ReturnDate .control-group-inline').addClass("hide");
    }
    $cmsj('.form-select-status').change(function(e) {

                if ($cmsj(this).val() === "Confirmed") {
                    $cmsj('#field_BookingReference label').removeClass("hide");
                    $cmsj('#field_DepartureDate label').removeClass("hide");
                    $cmsj('#field_ReturnDate label').removeClass("hide");
                    $cmsj('#field_BookingReference .form-control').removeClass("hide");
                    $cmsj('#field_DepartureDate .control-group-inline').removeClass("hide");
                    $cmsj('#field_ReturnDate .control-group-inline').removeClass("hide");
                } else {
                    $cmsj('#field_BookingReference label').addClass("hide");
                    $cmsj('#field_DepartureDate label').addClass("hide");
                    $cmsj('#field_ReturnDate label').addClass("hide");
                    $cmsj('#field_BookingReference .form-control').addClass("hide");
                    $cmsj('#field_DepartureDate .control-group-inline').addClass("hide");
                    $cmsj('#field_ReturnDate .control-group-inline').addClass("hide");
                }


            });
});