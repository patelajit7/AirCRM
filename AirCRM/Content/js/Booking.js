objBookings = {
    validateSearch: function () {
        var isValid = true;
        var message = '';
        try {
            var searchType = $("#ddlSearchParam").val();
            var searchValue = $("#txtSearchValue").val();
            if (searchType == "0") {
                isValid = false;
                message = "Please select searching criteria.";
            }
            else if (searchValue == "") {
                isValid = false;
                message = "Please enter searching value.";
            }
            else if (searchType == "2" && searchValue != "") {
                if (isNaN(searchValue)) {
                    isValid = false;
                    message = "You must enter numeric value for selected criteria.";
                }
                else if (searchValue < 1 || searchValue > 2147483647) {
                    isValid = false;
                    message = "You must enter correct Booking Number.";
                }
            }

            if (!isValid) {
                shared.createConfirmDialog("Invalid Param", message, 500, 150, shared.buttonType.OK, "", "")
            }
            else {
            }
        }
        catch (ex) {
            console.log(ex.stack);
        }
        return isValid;
    },
    searchBookingDetailsBySearchType: function () {
        common.progressBarStart();
        try {
            if (objBookings.validateSearch()) {
                var searchType = $("#ddlSearchParam").val();
                var searchValue = $("#txtSearchValue").val();
                try {
                    jQuery.ajax({
                        type: 'POST',
                        url: AppURL + '/bookings-search-by-type',
                        cache: false,
                        dataType: "json",
                        data: { searchParam: searchType, searchValue: searchValue },
                        success: function (response) {
                            //shared.sessionTimeOutThroughAjax(response);
                            if (response.isSuccess) {
                                $("#FlightBookingSearch").html(response.hTMLString);
                            }
                            else {
                                $("#FlightBookingSearch").html('<div class="text-center text-danger">' + response.message + '</div>');
                                Toast.fire({
                                    icon: 'error',
                                    title: response.message
                                })
                            }
                        },
                        error: function () {
                            Toast.fire({
                                icon: 'error',
                                title: response.message
                            })
                        }
                    });
                }
                catch (ex) {
                    console.log(ex.stack);
                    Toast.fire({
                        icon: 'error',
                        title: 'Bookings not found!'
                    })
                }
            }
        }
        catch (ex) {
            console.log(ex.stack);
            Toast.fire({
                icon: 'error',
                title: 'Bookings not found!'
            })
        }
        common.progressBarStop();
    },
    searchBookingDetailsByDate: function () {
        common.progressBarStart();
        try {
            var _startDate = $("#txtSearchFromDate").data().date;
            var _endDate = $("#txtSearchToDate").data().date;
            var _disposition = -1;
            var isOnlineBookings = true;
            var dateType = 1;
            isChecked = true;
            if (!isChecked) {
                dateType = 2;
            }
            try {
                jQuery.ajax(
                    {
                        type: 'POST',
                        url: AppURL + '/bookings-search-search-dates',
                        cache: false,
                        dataType: "json",
                        data: { _startDate: _startDate, _endDate: _endDate, _bookingSubStatus: _disposition, _dateSearchType: dateType, _isOnline: isOnlineBookings },
                        success: function (response) {
                            if (response.isSuccess) {
                                $("#FlightBookingSearch").html(response.hTMLString);
                            }
                            else {
                                if (response.isValidation) {
                                }
                                else {
                                    $("#FlightBookingSearch").html('<div class="text-center text-danger">' + response.message + '</div>');
                                    Toast.fire({
                                        icon: 'error',
                                        title: response.message
                                    })
                                }
                            }
                        }
                    });
            }
            catch (ex) {
                console.log(ex.stack);
            }
        }
        catch (ex) {
            console.log(ex.stack);
            Toast.fire({
                icon: 'error',
                title: 'Bookings not found!'
            })
        }
        common.progressBarStop();
    },
    editBookingDetail: function () {
        common.progressBarStart();
        try {
            $.ajax({
                url: AppURL + "/booking/edit-booking-detail",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ bookingId: TransId }),
                success: function (response) {
                    if (response.isSuccess) {
                        $("#modalPopup").html(response.html);
                        $('#modalPopup').modal({
                            backdrop: 'static',
                            keyboard: false
                        })
                        $.validator.unobtrusive.parse($('#frmBookingDetailsEdit'));
                    }
                    else {
                        console.log(response.message);
                    }
                }
            });
        }
        catch (ex) {
            console.log(ex.stack);
        }
        common.progressBarStop();
    },
    updateBooking: function () {
        try {
            if (!$('#frmBookingDetailsEdit').valid()) {
                return;
            }
            common.progressBarStart();
            var frm = $("#frmBookingDetailsEdit");
            var booking = {};
            booking.Id = TransId;
            booking.PNR = frm.find("#txtPNR").val()
            booking.AirlinePNR = frm.find("#txtAirlinePNR").val()
            booking.PortalId = frm.find("#ddlPortal").val();
            booking.ProviderId = frm.find("#ddlProvider").val();
            booking.BookingSourceType = frm.find("#ddlBookingSource").val();
            try {

                $.ajax({
                    url: AppURL + "/booking/updatebooking",
                    type: "POST",
                    data: '{model: ' + JSON.stringify(booking) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        frm.find("#divBookingDetailsEditMsg").show();
                        frm.find("#divBookingDetailsErrorMsg").hide();
                        frm.find("#divBookingDetailsSuccessMsg").hide();
                        if (response.isSuccess) {
                            frm.find("#divBookingDetailsSuccessMsg").show();
                            frm.find("#divBookingDetailsSuccessMsg").html(response.message);
                            Toast.fire({
                                icon: 'success',
                                title: response.message
                            })
                        }
                        else {
                            frm.find("#divBookingDetailsErrorMsg").show();
                            frm.find("#divBookingDetailsErrorMsg").html(response.message);
                            Toast.fire({
                                icon: 'error',
                                title: response.message
                            })
                        }
                    }
                });
            }
            catch (ex) {
                console.log(ex.stack);
            }
            common.progressBarStop();
        }
        catch (ex) {
            console.log(ex.stack);
        }
    },
    editBillingDetail: function (id) {
        common.progressBarStart();
        try {
            $.ajax({
                url: AppURL + "/booking/edit-billing-detail",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ id: id }),
                success: function (response) {
                    if (response.isSuccess) {
                        $("#modalPopup").html(response.html);
                        $('#modalPopup').modal({
                            backdrop: 'static',
                            keyboard: false
                        })
                        $.validator.unobtrusive.parse($('#frmBillingDetailsEdit'));
                        common.progressBarStop();
                    }
                    else {
                        Toast.fire({
                            icon: 'error',
                            title: response.message
                        })                        
                        common.progressBarStop();
                    }
                }
            });
        }
        catch (ex) {
            console.log(ex.stack);
            common.progressBarStop();
            Toast.fire({
                icon: 'error',
                title: 'Failed to complete the request!'
            })
        }
        
    },
    updateBillingDetail: function () {        
        try {
            if (!$('#frmBillingDetailsEdit').valid()) {
                return;
            }
            common.progressBarStart();
            var frm = $("#frmBillingDetailsEdit");

            var billingDetail = {};
            billingDetail.Id = frm.find("#Id").val();
            billingDetail.BookingId = TransId;
            billingDetail.CardNumber = frm.find("#CardNumber").val();
            billingDetail.CCHolderName = frm.find("#CCHolderName").val();
            billingDetail.CardType = frm.find("#CardType").val();
            billingDetail.ExpiryYear = frm.find("#ExpiryYear").val();
            billingDetail.ExpiryMonth = frm.find("#ExpiryMonth").val();
            billingDetail.CVVNumber = frm.find("#CVVNumber").val();
            billingDetail.AreaCode = frm.find("#AreaCode").val();
            billingDetail.BillingPhone = frm.find("#BillingPhone").val();
            billingDetail.Email = frm.find("#Email").val();
            billingDetail.AddressLine1 = frm.find("#AddressLine1").val();
            billingDetail.AddressLine2 = frm.find("#AddressLine2").val();
            billingDetail.AddressLine3 = frm.find("#AddressLine3").val();
            billingDetail.City = frm.find("#City").val();
            billingDetail.State = frm.find('#State').val();
            billingDetail.Country = frm.find("#CountryCode").val();
            billingDetail.ZipCode = frm.find("#ZipCode").val();

            try {
                $.ajax({
                    url: AppURL + "/booking/updatebillingdetail",
                    type: "POST",
                    data: '{model: ' + JSON.stringify(billingDetail) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        frm.find("#divBillingDetailsEditMsg").show();
                        frm.find("#divBillingDetailsErrorMsg").hide();
                        frm.find("#divBillingDetailsSuccessMsg").hide();
                        if (response.isSuccess) {
                            frm.find("#divBillingDetailsSuccessMsg").show();
                            frm.find("#divBillingDetailsSuccessMsg").html(response.message);
                            Toast.fire({
                                icon: 'success',
                                title: response.message
                            });
                            common.progressBarStop();
                            $('#modalPopup').show().scrollTop(0);
                        }
                        else {
                            frm.find("#divBillingDetailsErrorMsg").show();
                            frm.find("#divBillingDetailsErrorMsg").html(response.message);
                            Toast.fire({
                                icon: 'error',
                                title: response.message
                            });
                            common.progressBarStop();
                            $('#modalPopup').show().scrollTop(0);
                        }
                    }
                });

            }
            catch (ex) {
                console.log(ex.stack);
                common.progressBarStop();
            }
        }
        catch (ex) {
            console.log(ex.stack);
            common.progressBarStop();
        }
    },

    editPriceDetail: function (id) {
        common.progressBarStart();
        try {
            $.ajax({
                url: AppURL + "/Booking/EditPriceDetail",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ id: id }),
                success: function (response) {
                    if (response.isSuccess) {
                        $("#modalPopup").html(response.html);
                        $('#modalPopup').modal({
                            backdrop: 'static',
                            keyboard: false
                        })
                        $.validator.unobtrusive.parse($('#frmPriceDetailsEdit'));
                    }
                    else {
                        console.log(response.message);
                    }
                }
            });
        }
        catch (ex) {
            console.log(ex.stack);
        }
        common.progressBarStop();
    },
    updatePriceDetail: function (id) {
        try {
            if (!$('#frmPriceDetailEdit').valid()) {
                return;
            }
            common.progressBarStart();
            var frm = $("#frmPriceDetailEdit");
            var priceDetails = {};
            priceDetails.Id = id;
            priceDetails.BookingId = TransId;
            priceDetails.PaxType = frm.find("#PaxType").val();
            priceDetails.PaxCount = frm.find("#PaxCount").val();
            priceDetails.FareBaseCode = frm.find("#FareBaseCode").val();
            priceDetails.Currency = frm.find("#Currency").val();
            priceDetails.IsExtendedCancellation = frm.find("#IsExtendedCancellation").is(':checked');
            priceDetails.ExtendedCancellationAmount = frm.find("#ExtendedCancellationAmount").val();
            priceDetails.BaseFare = frm.find("#BaseFare").val();
            priceDetails.Tax = frm.find("#Tax").val();
            priceDetails.Markup = frm.find("#Markup").val();
            priceDetails.TotalAmount = frm.find("#TotalAmount").val();

            try {
                $.ajax({
                    url: AppURL + "/booking/updateflightpricedetail",
                    type: "POST",
                    data: '{model: ' + JSON.stringify(priceDetails) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        frm.find("#divPriceDetailEditMsg").show();
                        frm.find("#divPriceDetailEditErrorMsg").hide();
                        frm.find("#divPriceDetailEditSuccessMsg").hide();
                        if (response.isSuccess) {

                            frm.find("#divPriceDetailEditSuccessMsg").show();
                            frm.find("#divPriceDetailEditSuccessMsg").html(response.message);
                            Toast.fire({
                                icon: 'success',
                                title: response.message
                            })
                            common.progressBarStop();
                        }
                        else {
                            frm.find("#divPriceDetailEditErrorMsg").show();
                            frm.find("#divPriceDetailEditErrorMsg").html(response.message);
                            Toast.fire({
                                icon: 'error',
                                title: response.message
                            })
                            common.progressBarStop();
                        }
                    }
                });
            }
            catch (ex) {
                console.log(ex.stack);
                common.progressBarStop();
            }
        }
        catch (ex) {
            console.log(ex.stack);
            common.progressBarStop();
        }

    },

    editFlightTraveller: function (id) {
        common.progressBarStart();
        try {
            $.ajax({
                url: AppURL + "/booking/editflighttraveller",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ id: id }),
                success: function (response) {
                    if (response.isSuccess) {
                        $("#modalPopup").html(response.html);
                        $('#modalPopup').modal({
                            backdrop: 'static',
                            keyboard: false
                        })
                        $.validator.unobtrusive.parse($('#frmPriceDetailsEdit'));
                    }
                    else {
                        console.log(response.message);
                    }
                }
            });
        }
        catch (ex) {
            console.log(ex.stack);
        }
        common.progressBarStop();
    },
    updateFlightTraveller: function (id) {
        try {
            var frm = $("#frmFlightTravellerEdit");
            if (!$('#frmFlightTravellerEdit').valid()) {
                return;
            }
            common.progressBarStart();
            var traveller = {};
            traveller.Id = id;
            traveller.BookingId = TransId;
            traveller.Title = frm.find("#Title").val();
            traveller.FirstName = frm.find("#FirstName").val();
            traveller.MiddleName = frm.find("#MiddleName").val();
            traveller.LastName = frm.find("#LastName").val();
            traveller.DOB = frm.find("#DOB").val();
            traveller.Gender = frm.find("#Gender").val();
            traveller.PaxType = frm.find("#PaxType").val();

            $.ajax({
                url: AppURL + "/booking/updateflighttraveller",
                type: "POST",
                data: '{model: ' + JSON.stringify(traveller) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    frm.find("#divFlightTravellerEditMsg").show();
                    frm.find("#divFlightTravellerEditErrorMsg").hide();
                    frm.find("#divFlightTravellerEditSuccessMsg").hide();
                    if (response.isSuccess) {
                        frm.find("#divFlightTravellerEditSuccessMsg").show();
                        frm.find("#divFlightTravellerEditSuccessMsg").html(response.message);
                        Toast.fire({
                            icon: 'success',
                            title: response.message
                        })
                        common.progressBarStop();
                    }
                    else {
                        frm.find("#divFlightTravellerEditErrorMsg").show();
                        frm.find("#divFlightTravellerEditErrorMsg").html(response.message);
                        Toast.fire({
                            icon: 'error',
                            title: response.message
                        })
                        common.progressBarStop();
                    }
                }
            });

        }
        catch (ex) {
            console.log(ex.stack);
            common.progressBarStop();
        }
    },

    deleteFlightTraveller: function (id) {
        try {
            var frm = $("#frmFlightTravellerDelete");

            var travellerName = frm.find("#lblTravellerNameToDelete").text()
            var remarks = "Flight traveler details deleted by user.";               // frm.find("#txtRemarks").val();
            remarks = travellerName + " - deleted from record. | " + remarks;

            var isValid = validator.requiredValidation(frm);

            try {
                if (isValid) {
                    $.ajax({
                        url: APP_URL + "Manage/Bookings/DeleteFlightTraveller",
                        type: "POST",
                        data: JSON.stringify({ id: id, bookingId: T_ID }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            frm.find("#divFlightTravellerDeleteMsg").show();
                            frm.find("#divFlightTravellerDeleteErrorMsg").hide();
                            frm.find("#divFlightTravellerDeleteSuccessMsg").hide();
                            shared.sessionTimeOutThroughAjax(response);
                            if (response.isSuccess) {
                                openFlightBooking.saveFlightRemarks(T_ID, '', "Traveller Details: " + remarks);
                                //IS_REMARKS_ADDED = true;
                                frm.find("#divFlightTravellerDeleteSuccessMsg").show();
                                frm.find("#divFlightTravellerDeleteSuccessMsg").html(response.message);
                            }
                            else {
                                frm.find("#divFlightTravellerDeleteErrorMsg").show();
                                frm.find("#divFlightTravellerDeleteErrorMsg").html(response.message);
                                console.log(response.message);
                            }
                        }
                    });
                }
            }
            catch (ex) {
                console.log(ex.stack);
            }
        }
        catch (ex) {
            console.log(ex.stack);
        }
    },

    addNewFlightTraveller: function () {
        try {
            $.ajax({
                url: APP_URL + "Manage/Bookings/AddNewFlightTraveller",
                type: "POST",
                contentType: "application/json",
                //data: JSON.stringify({ bookingId: bookingId }),
                success: function (response) {
                    shared.sessionTimeOutThroughAjax(response);
                    if (response.isSuccess) {
                        $(".reqDisableLink").css({ "pointer-events": "none" });
                        shared.createConfirmDialog("Add New Flight Traveller", "", 860, 500, "", "", "");
                        $("#containterModelDialog").html(response.html);
                    }
                    else {
                        console.log(response.message);
                    }
                }
            });
        }
        catch (ex) {
            console.log(ex.stack);
        }
    },

    saveNewFlightTraveller: function () {
        try {
            var frm = $("#frmAddNewFlightTraveller");
            var traveller = {};
            traveller.BookingId = T_ID;
            traveller.Title = frm.find("#ddlTitle").val();
            traveller.FirstName = frm.find("#txtFirstName").val();
            traveller.MiddleName = frm.find("#txtMiddleName").val();
            traveller.LastName = frm.find("#txtLastName").val();
            traveller.DOB = frm.find("#txtDOB").val();
            traveller.Gender = frm.find("#ddlGender").val();
            traveller.Email = frm.find("#txtEmail").val();
            traveller.PaxType = frm.find("#ddlPaxType").val();
            traveller.TicketNo = frm.find("#txtTicketNo").val();
            traveller.PassportNumber = frm.find("#txtPassportNumber").val();
            traveller.PassportIssuedBy = frm.find("#ddlPassportIssuedBy").val();
            traveller.PassportExpireDate = frm.find("#txtPassportExpireDate").val();

            var remarks = "New flight traveler details added by user.";   //frm.find("#txtRemarks").val();
            var title = frm.find("#ddlTitle").find('option:selected').text()
            var travellerName = "|Add New Traveller|Passenger Name: " + title + " " + traveller.FirstName + " " + (traveller.MiddleName != "" ? traveller.MiddleName : "") + traveller.LastName;
            remarks = travellerName + " - added to record.|" + remarks;

            var isValid = validator.requiredValidation(frm);

            try {
                if (isValid) {
                    $.ajax({
                        url: APP_URL + "Manage/Bookings/SaveNewFlightTraveller",
                        type: "POST",
                        data: '{model: ' + JSON.stringify(traveller) + ', dob: ' + JSON.stringify(frm.find("#txtDOB").val()) + ', passExpDate: ' + JSON.stringify(frm.find("#txtPassportExpireDate").val()) + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            frm.find("#divAddNewFlightTravellerMsg").show();
                            frm.find("#divAddNewFlightTravellerErrorMsg").hide();
                            frm.find("#divAddNewFlightTravellerSuccessMsg").hide();
                            shared.sessionTimeOutThroughAjax(response);
                            if (response.isSuccess) {
                                openFlightBooking.saveFlightRemarks(T_ID, '', "Traveller Details" + remarks);
                                //IS_REMARKS_ADDED = true;
                                frm.find("#divAddNewFlightTravellerSuccessMsg").show();
                                frm.find("#divAddNewFlightTravellerSuccessMsg").html(response.message);
                            }
                            else {
                                frm.find("#divAddNewFlightTravellerErrorMsg").show();
                                frm.find("#divAddNewFlightTravellerErrorMsg").html(response.message);
                                console.log(response.message);
                            }
                        }
                    });
                }
            }
            catch (ex) {
                console.log(ex.stack);
            }
        }
        catch (ex) {
            console.log(ex.stack);
        }
    },

    editFlightBookingSegments: function () {

        try {

            $.ajax({
                url: APP_URL + "Manage/Bookings/EditFlightBookingSegments",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ bookingId: T_ID }),
                success: function (response) {
                    shared.sessionTimeOutThroughAjax(response);
                    if (response.isSuccess) {
                        $(".reqDisableLink").css({ "pointer-events": "none" });
                        shared.createConfirmDialog("Edit Flight Segments", "", 1200, 570, "", "", "");
                        $("#containterModelDialog").html(response.html);
                    }
                    else {
                        console.log(response.message);
                    }
                }
            });
        }
        catch (ex) {
            console.log(ex.stack);
        }
    },

    updateFlightBoogingSegments: function (args) {

        try {

            var frm = $("#frmFlightBookingSegmentsEdit");
            var flightSegs = [];

            var cntOutBound = args.NoOfOutBoundItin;
            var cntInBound = args.NoOfInBoundItin;

            var remark = "Flight segment details updated successfully";

            if (cntOutBound > 0) {

                for (var i = 0; i < cntOutBound; i++) {

                    var flightSeg = {};

                    flightSeg.Id = frm.find("#lblOutSegId" + i).text();
                    flightSeg.BookingId = T_ID;
                    flightSeg.IsReturn = false;
                    flightSeg.AirlineLocator = frm.find("#txtOutSegAirLoc" + i).val();

                    flightSegs.push(flightSeg);
                }
            }

            if (cntInBound > 0) {

                for (var i = 0; i < cntInBound; i++) {

                    var flightSeg = {};

                    flightSeg.Id = frm.find("#lblInSegId" + i).text();
                    flightSeg.BookingId = T_ID;
                    flightSeg.IsReturn = true;
                    flightSeg.AirlineLocator = frm.find("#txtInSegAirLoc" + i).val();

                    flightSegs.push(flightSeg);
                }
            }

            try {

                $.ajax({
                    url: APP_URL + "Manage/Bookings/UpdateFlightBoogingSegments",
                    type: "POST",
                    data: '{model: ' + JSON.stringify(flightSegs) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        shared.sessionTimeOutThroughAjax(response);
                        frm.find("#divFlightBookingSegmentsEditMsg").show();
                        frm.find("#divFlightBookingSegmentsEditErrorMsg").hide();
                        frm.find("#divFlightBookingSegmentsEditSuccessMsg").hide();
                        if (response.isSuccess) {
                            openFlightBooking.saveFlightRemarks(T_ID, '', "Flight Segments | Edit Segments: " + remark);
                            IS_USA_FLT_BK_SEG_TO_EDT_UPDATED = true;
                            frm.find("#divFlightBookingSegmentsEditSuccessMsg").show();
                            frm.find("#divFlightBookingSegmentsEditSuccessMsg").text(response.message);
                        }
                        else {
                            frm.find("#divFlightBookingSegmentsEditErrorMsg").show();
                            frm.find("#divFlightBookingSegmentsEditErrorMsg").text(response.message);
                            console.log(response.message);
                        }
                    }
                });
            }
            catch (ex) {
                console.log(ex.stack);
            }
        }
        catch (ex) {
            console.log(ex.stack);
        }
    },

    getBookingStatusUpdateView: function (id,reference) {
        common.progressBarStart();
        try {
            $.ajax({
                url: AppURL + "/booking/booking-status-view",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ id: id, referenceNumber: reference}),
                success: function (response) {
                    if (response.IsSuccess) {
                        $("#modalPopup").html(response.html);
                        $('#modalPopup').modal({
                            backdrop: 'static',
                            keyboard: false
                        })
                        $.validator.unobtrusive.parse($('#frmBookingStatusUpdate'));
                    }
                    else {
                        console.log(response.message);
                    }
                }
            });
        }
        catch (ex) {
            console.log(ex.stack);
        }
        common.progressBarStop();
    },
    updateBookingStatus: function () {
        try {
            var frm = $("#frmBookingStatusUpdate");
            if (!$('#frmBookingStatusUpdate').valid()) {
                return;
            }
            common.progressBarStart();
            var bookingStatus = {};
            bookingStatus.BookingId = frm.find("#BookingId").val();
            bookingStatus.BookingStatus = frm.find("#BookingStatus").val();
            bookingStatus.Remarks = frm.find("#Remarks").val();
            bookingStatus.ReferenceId = frm.find("#ReferenceId").val();
            $.ajax({
                url: AppURL + "/booking/updatebookingstatus",
                type: "POST",
                data: '{model: ' + JSON.stringify(bookingStatus) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    frm.find("#divBookingStatusMsg").show();
                    frm.find("#divBookingStatusErrorMsg").hide();
                    frm.find("#divBookingStatusSuccessMsg").hide();
                    if (response.isSuccess) {
                        frm.find("#divBookingStatusSuccessMsg").show();
                        frm.find("#divBookingStatusSuccessMsg").html(response.message);
                        Toast.fire({
                            icon: 'success',
                            title: response.message
                        })
                        common.progressBarStop();
                    }
                    else {
                        frm.find("#divBookingStatusErrorMsg").show();
                        frm.find("#divBookingStatusErrorMsg").html(response.message);
                        Toast.fire({
                            icon: 'error',
                            title: response.message
                        })
                        common.progressBarStop();
                    }
                }
            });

        }
        catch (ex) {
            console.log(ex.stack);
            common.progressBarStop();
        }
    },

    getAddRemarksView: function (id) {
        common.progressBarStart();
        try {
            $.ajax({
                url: AppURL + "/booking/booking-remarks-view",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ id: id }),
                success: function (response) {
                    if (response.IsSuccess) {
                        $("#modalPopup").html(response.html);
                        $('#modalPopup').modal({
                            backdrop: 'static',
                            keyboard: false
                        })
                        $.validator.unobtrusive.parse($('#frmBookingRemarksAdd'));
                    }
                    else {
                        console.log(response.message);
                    }
                }
            });
        }
        catch (ex) {
            console.log(ex.stack);
        }
        common.progressBarStop();
    },
    AddRemarks: function () {
        try {
            var frm = $("#frmBookingRemarksAdd");
            if (!$('#frmBookingRemarksAdd').valid()) {
                return;
            }
            common.progressBarStart();
            var bookingRemarks = {};
            bookingRemarks.BookingId = frm.find("#BookingId").val();
            bookingRemarks.Remarks = frm.find("#Remarks").val();
            $.ajax({
                url: AppURL + "/booking/addbookingremarks",
                type: "POST",
                data: '{model: ' + JSON.stringify(bookingRemarks) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    frm.find("#divBookingRemarksMsg").show();
                    frm.find("#divBookingRemarksErrorMsg").hide();
                    frm.find("#divBookingRemarksSuccessMsg").hide();
                    if (response.IsSuccess) {
                        frm.find("#divBookingRemarksSuccessMsg").show();
                        frm.find("#divBookingRemarksSuccessMsg").html("Remarks addedd successfully.");
                        Toast.fire({
                            icon: 'success',
                            title: "Remarks addedd successfully."
                        });
                        objBookings.getBookingRemarks(TransId);
                        common.progressBarStop();
                    }
                    else {
                        frm.find("#divBookingRemarksErrorMsg").show();
                        frm.find("#divBookingRemarksErrorMsg").html("Remarks add failed!");
                        Toast.fire({
                            icon: 'error',
                            title: "Remarks add failed!"
                        })
                        common.progressBarStop();
                    }
                }
            });

        }
        catch (ex) {
            console.log(ex.stack);
            common.progressBarStop();
        }
    },
    getBookingRemarks: function (id) {        
        try {
            $.ajax({
                url: AppURL + "/booking/bookingremarks",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ id: id }),
                success: function (response) {
                    if (response.IsSuccess) {
                        $("#divBookingRemarks").html(response.html);
                        
                    }
                    else {
                        console.log(response.message);
                        
                    }
                }
            });
        }
        catch (ex) {
            console.log(ex.stack);
        }
        
    },

    getBookingAssignView: function (id) {
        common.progressBarStart();
        try {
            $.ajax({
                url: AppURL + "/booking/booking-assign-view",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ id: id }),
                success: function (response) {
                    if (response.isSuccess) {
                        $("#bookingAssignPopup").html(response.html);
                        $('#bookingAssignPopup').modal({
                            backdrop: 'static',
                            keyboard: false
                        })
                        $.validator.unobtrusive.parse($('#frmBookingAssign'));
                    }
                    else {
                        console.log(response.message);
                    }
                }
            });
        }
        catch (ex) {
            console.log(ex.stack);
        }
        common.progressBarStop();
    },
    AssignBooking: function () {
        try {
            var frm = $("#frmBookingAssign");
            if (!$('#frmBookingAssign').valid()) {
                return;
            }
            common.progressBarStart();
            var bookingRemarks = {};
            bookingRemarks.BookingId = frm.find("#BookingId").val();
            bookingRemarks.UserId = frm.find("#UserId").val();
            $.ajax({
                url: AppURL + "/booking/bookingassigntoagent",
                type: "POST",
                data: '{model: ' + JSON.stringify(bookingRemarks) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    frm.find("#divBookingAssignMsg").show();
                    frm.find("#divBookingAssignErrorMsg").hide();
                    frm.find("#divBookingRemarksSuccessMsg").hide();
                    if (response.isSuccess) {
                        frm.find("#divBookingAssignSuccessMsg").show();
                        frm.find("#divBookingAssignSuccessMsg").html(response.message);
                        Toast.fire({
                            icon: 'success',
                            title: response.message
                        });
                        common.progressBarStop();
                    }
                    else {
                        frm.find("#divBookingAssignErrorMsg").show();
                        frm.find("#divBookingAssignErrorMsg").html(response.message);
                        Toast.fire({
                            icon: 'error',
                            title: response.message
                        })
                        common.progressBarStop();
                    }
                }
            });

        }
        catch (ex) {
            console.log(ex.stack);
            common.progressBarStop();
        }
    },
    RetriveBookingDetails: function () {
        try {
            var frm = $("#frmRetrievPNR");
            if (!$('#frmRetrievPNR').valid()) {
                return;
            }
            common.progressBarStart();
            
            var id = frm.find("#ReferenceNumber").val();
            
            $.ajax({
                url: AppURL + "/booking/retrievepnr",
                type: "POST",
                data: JSON.stringify({ id: id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    frm.find("#divRetrievePNRMsg").show();
                    frm.find("#divRetrievePNRErrorMsg").hide();
                    frm.find("#divRetrievePNRSuccessMsg").hide();
                    if (response.isSuccess) {
                        frm.find("#divRetrievePNRSuccessMsg").show();
                        frm.find("#divRetrievePNRSuccessMsg").html(response.message);
                        Toast.fire({
                            icon: 'success',
                            title: response.message
                        });
                        openBooking(response.BookingId);
                        common.progressBarStop();
                    }
                    else {
                        frm.find("#divRetrievePNRErrorMsg").show();
                        frm.find("#divRetrievePNRErrorMsg").html(response.message);
                        Toast.fire({
                            icon: 'error',
                            title: response.message
                        })
                        common.progressBarStop();
                    }
                }
            });

        }
        catch (ex) {
            console.log(ex.stack);
            common.progressBarStop();
        }
    },
    SendEmailToCustomer: function (id) {
        common.progressBarStart();
        try {
            $.ajax({
                url: AppURL + "/booking/sendemailtocustomer",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ id: id }),
                success: function (response) {
                    if (response.isSuccess) {
                        Toast.fire({
                            icon: 'success',
                            title: response.message
                        });
                    }
                    else {
                        Toast.fire({
                            icon: 'error',
                            title: response.message
                        });
                    }
                }
            });
        }
        catch (ex) {
            console.log(ex.stack);
        }
        common.progressBarStop();
    },
}

userObj = {
    id: 0,
    isExist: false,
    getUser: function (commandType) {
        try {
            common.progressBarStart();
            var skipFlag = $('#skipFlag').val();
            var name = $('#txtSearchName').val();
            jQuery.ajax(
                {
                    type: 'GET',
                    url: AppURL + '/users-data',
                    cache: false,
                    data: { skipflag: skipFlag, commandType: commandType, name: name },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.TransactionStatus == true && result.ResultList != null) {
                            $("#divUser").html(result.ResultList);
                            $("#preloader").fadeOut("slow")
                            common.progressBarStop();

                        }
                        else {
                            $("#divUser").html(result.ResultList);
                            common.progressBarStop();
                            return false;
                        }
                    }
                });

        }
        catch (ex) {
            common.progressBarStop();
            console.log(ex.stack);
        }
    },
    getNextPrevUser: function (cType) {
        $('#skipFlag').val("true");
        this.getUser(cType);
        $('#skipFlag').val("false");
    },
    deleteUserPop: function (ID) {
        this.id = ID;
        $('#delete-user').modal('show');
    },
    delUser: function () {
        try {
            common.progressBarStart();
            jQuery.ajax(
                {
                    type: 'GET',
                    url: AppURL + "/users-delete",
                    cache: false,
                    data: { id: this.id },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.isSuccess == true) {
                            userObj.getUser('Next');
                            common.progressBarStop();
                        }
                        else {
                            common.progressBarStop();
                        }
                    }
                });

        }
        catch (ex) {
            common.progressBarStop();
            console.log(ex.stack);

        }
    }

}