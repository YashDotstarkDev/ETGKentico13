using System;
using System.Diagnostics;
using Castle.Core.Internal;
using CMS.Ecommerce;
using ETG.ABCPdf;
using ETG.Data.Extensions;
using ETG.Data.Repositories.Pages;
using ETG.Data.Settings;
using ETG.Data.Settings.Models;
using ETG.Data.Tour.Models;
using ETG.Data.Tour.Services;
using ETG.Module.Booking.Booking;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Classes.Providers;
using ETG.Module.Booking.ECommerce;
using ETG.Module.Booking.Extensions;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using CMS.Base;
using CMS.EventLog;
using CMS.SiteProvider;
using ETG.Booking.Pricing.Enums;
using ETG.Data.Tour;
using Newtonsoft.Json;
using WebSupergoo.ABCpdf12;

namespace ETG.Module.Booking.Services
{
    public class QuotePDFService : IQuotePDFService
    {
        private const int LINE_SPACING = 6;
        private readonly IBookingCheckoutPageContentsRepository _bookingCheckoutPageContentsRepository;
        private readonly ITourService _tourService;
        private readonly ETGSettings _settings;
        private readonly IBookingDataProvider _bookingDataProvider;
        private readonly IPaymentTermsProvider _paymentTermsProvider;
        private readonly IQuoteAddedServiceProvider _quoteAddedServiceProvider;
        private readonly IAgentLogoRepository _agentLogoRepository;

        public QuotePDFService(ITourService tourService, IETGSettingsService etgSettingsService,
            IBookingCheckoutPageContentsRepository bookingCheckoutPageContentsRepository,
            IBookingDataProvider bookingDataProvider, IPaymentTermsProvider paymentTermsProvider,
            IQuoteAddedServiceProvider quoteAddedServiceProvider, IAgentLogoRepository agentLogoRepository)
        {
            _bookingDataProvider = bookingDataProvider;
            _tourService = tourService;
            _settings = etgSettingsService.GetSettings();
            _bookingCheckoutPageContentsRepository = bookingCheckoutPageContentsRepository;
            _paymentTermsProvider = paymentTermsProvider;
            _quoteAddedServiceProvider = quoteAddedServiceProvider;
            _agentLogoRepository = agentLogoRepository;
        }

        private string GetPDFClientQuoteSection(BookingQuoteInfo quote)
        {
            var text = new System.Text.StringBuilder();
            text.AppendLine("<font font-family=\"Helvetica\">");
            text.AppendLine($"<stylerun line-spacing=\"4\">Ref: {quote.BookingQuoteID}<br></stylerun>");
            text.AppendLine(
                $"<stylerun line-spacing=\"4\">Date: {quote.BookingQuoteCreated.ToString("d MMMM yyyy")}<br></stylerun>");
            text.AppendLine(
                $"<stylerun line-spacing=\"4\">Quote is valid for {quote.BookingQuoteValidDays} days</stylerun>");
            text.AppendLine("</font>");
            return text.ToString();
        }

        private string GetPDFBasicTourInfoSection(TourModel tour)
        {
            var text = new System.Text.StringBuilder();

            text.AppendLine(
                $"<stylerun fontsize=\"14\"><font font-family=\"Helvetica\">{tour.TourSummaryInfo.Name} </stylerun><stylerun fontsize=\"8\"><font font-family=\"Helvetica\">({tour.TourSummaryInfo.TourCode})</font></stylerun><br>");
            text.AppendLine(
                $"<stylerun fontsize=\"8\"><font font-family=\"Helvetica\">{tour.TourSummaryInfo.PrimaryCountryName}</font></stylerun>");

            return text.ToString();
        }

        private string GetPDFPaymentTerms(string contents)
        {
            var text = new System.Text.StringBuilder();

            text.AppendLine(
                $"<stylerun fontsize=\"10\"><font font-family=\"Helvetica\" font-weight=\"700\">Booking Process</font></stylerun><br><stylerun fontsize=\"8\" linespacing=\"2\"><font font-family=\"Helvetica\">{contents.Replace("<p", "<p spacebefore=\"1\" spaceafter=\"0\"")}</font></stylerun>");
            return text.ToString();
        }

        private string SetParagraph(string contents)
        {
            if (contents.IsNullOrEmpty())
            {
                return string.Empty;
            }

            contents = HttpContext.Current.Server.HtmlEncode(contents).Replace("\n", "<br>");
            var text = new System.Text.StringBuilder();

            text.AppendLine(
                $"<stylerun fontsize=\"8\" linespacing=\"2\"><font font-family=\"Helvetica\"><p spacebefore=\"1\" spaceafter=\"0\">{contents}</p></font></stylerun><br>");
            return text.ToString();
        }

        private string[] SetFonts(string[] items)
        {
            if (items.Length > 0)
            {
                for (var i = 0; i < items.Length; i++)
                {
                    items[i] = $"<stylerun fontsize=\"8\"><font font-family=\"Helvetica\">{items[i]}</font></stylerun>";
                }
            }

            return items;
        }

        private string GetEmailDomain(string email)
        {
            if (email.IsNullOrEmpty())
            {
                return null;
            }

            var index = email.IndexOf("@");

            if (index > -1)
            {
                return email.Substring(index + 1);
            }

            return null;
        }

        public MemoryStream GenerateQuotePDF(int quoteId, bool excludeDepositTerms)
        {
            var quote = BookingQuoteInfoProvider.GetBookingQuoteInfo(quoteId);
            if (quote == null)
            {
                return null;
            }

            var tour = _tourService.GetTourByTourCode(quote.BookingQuoteTourCode);

            if (tour == null)
            {
                return null;
            }

            var quoteData = quote.GetDetails();
            if (quoteData == null)
            {
                return null;
            }

            var theDoc = new Doc();
            string path = excludeDepositTerms
                ? HttpContext.Current.Server.MapPath("~/pdf/ETG-Client-Quote-1Pager.pdf")
                : HttpContext.Current.Server.MapPath("~/pdf/ETG-Client-Quote-Page1.pdf");

            theDoc.Read(path);

            var customer = CustomerInfoProvider.GetCustomerInfo(quote.BookingQuoteCustomerID);

            var abcPdfDoc = new PDFDoc(theDoc);
            abcPdfDoc.SetPDFFieldText("PassengerName", $"{customer.CustomerFirstName} {customer.CustomerLastName}");

            var agencyName = customer.GetStringValue("CustomerAgencyName", string.Empty);
            var agencyAgentName = customer.GetStringValue("CustomerAgentName", string.Empty);
            var agencyEmail = customer.GetStringValue("CustomerAgentEmail", string.Empty);
            var agencyPhone = customer.GetStringValue("CustomerAgentPhone", string.Empty);

            var emailDomain = GetEmailDomain(agencyEmail);
            var mainLogo = "~/images/etglogo_pdf.jpg";

            var agentEmailLogo = _agentLogoRepository.GetAgentEmailLogo(agencyEmail);

            if (agentEmailLogo != null)
            {
                mainLogo = agentEmailLogo.AgentEmailLogo;
            }
            else
            {
                if (!emailDomain.IsNullOrEmpty())
                {
                    var agentLogo = _agentLogoRepository.GetAgentLogo(emailDomain, agencyEmail);

                    if (agentLogo != null)
                    {
                        mainLogo = agentLogo.AgentLogoUrl;
                    }
                }
            }

            var logo = $"{SiteContext.CurrentSite.SitePresentationURL}{mainLogo.RemoveQuery().Trim('~')}";
            if (!excludeDepositTerms)
            {
                abcPdfDoc.StampField("Logo");
                abcPdfDoc.AddImageUrl(1, 35, 800, 50, logo);
            }


            abcPdfDoc.SetPDFFieldHTML("TourInfo", GetPDFBasicTourInfoSection(tour), 0);
            abcPdfDoc.SetPDFFieldHTML("ClientQuote", GetPDFClientQuoteSection(quote), 7);
            abcPdfDoc.SetPDFFieldText("TravelAgencyName", agencyName);
            abcPdfDoc.SetPDFFieldText("TravelAdvisor", agencyAgentName);
            abcPdfDoc.SetPDFFieldText("TravelAgencyPhone", agencyPhone);
            abcPdfDoc.SetPDFFieldText("TravelAgencyEmail", agencyEmail);

            abcPdfDoc.SetPDFFieldText("TravelDeparts", tour.TourSummaryInfo.TourDepartsFromInAustralia);
            abcPdfDoc.SetPDFFieldText("TravelEnds", tour.TourSummaryInfo.TourTravelEnds);
            abcPdfDoc.SetPDFFieldText("StartDate", quoteData.DepartureDate.ToString("d MMM yyyy"));
            abcPdfDoc.SetPDFFieldText("Duration", $"{tour.TourSummaryInfo.NoOfNights + 1} days");
            abcPdfDoc.SetPDFFieldText("Experience",
                tour.TourSummaryInfo.Experiences.IsNullOrEmpty() ? "" : tour.TourSummaryInfo.Experiences[0].Name);
            abcPdfDoc.SetPDFFieldText("TravelStyle",
                tour.TourSummaryInfo.TourTypes.IsNullOrEmpty() ? "" : tour.TourSummaryInfo.TourTypes[0].Name);

            var numberOfLogos = GetNumberOfLogos(tour);

            for (int i = 1; i <= 4 - numberOfLogos; i++)
            {
                abcPdfDoc.StampField($"Logo{i}");
            }

            var logoIndex = 4 - numberOfLogos + 1;
            if (tour.HasPeaceOfMindGuarantee)
            {
                abcPdfDoc.AddImage(1, $"Logo{logoIndex}", HttpContext.Current.Server.MapPath("/images/pdf-pom.png"));
                logoIndex++;
            }

            if (tour.HasFreedomOfChoice)
            {
                abcPdfDoc.AddImage(1, $"Logo{logoIndex}",
                    HttpContext.Current.Server.MapPath("/images/PDF-freedom-of-choice.png"));
                logoIndex++;
            }

            if (tour.TourSummaryInfo.TourIsExclusive)
            {
                abcPdfDoc.AddImage(1, $"Logo{logoIndex}",
                    HttpContext.Current.Server.MapPath("/images/pdf-Exclusive-Packages.png"));
                logoIndex++;
            }

            if (tour.TourSummaryInfo.TourHasSafeTravel)
            {
                abcPdfDoc.AddImage(1, $"Logo{logoIndex}",
                    HttpContext.Current.Server.MapPath("/images/pdf-safe-travels.png"));
                logoIndex++;
            }

            abcPdfDoc.SetDocPosition("TextArea");
            var textAreaRect = new XRect();

            textAreaRect.String = theDoc.Rect.String;
            var additionalServices = _quoteAddedServiceProvider.GetAddedServices(quoteId).ToList();
            var hasStartDateColumn = additionalServices.Any(a => a.ItemDate != DateTime.MinValue);
            var tableColumnCount = hasStartDateColumn ? 5 : 4;
            var unitPriceColumnIndex = 2;
            var amountColumnIndex = 3;
            var breakdownTable = new PDFTable(theDoc, tableColumnCount);
            breakdownTable.CellPadding = 5;
            breakdownTable.HorizontalAlignment = 0;
            if (tableColumnCount == 4)
            {
                breakdownTable.SetColumnWidths(new double[] { 2, 8, 5, 5 });
            }
            else
            {
                unitPriceColumnIndex = 3;
                amountColumnIndex = 4;
                breakdownTable.SetColumnWidths(new double[] { 2, 8, 3, 3, 4 });
            }

            breakdownTable.NextRow();
            var breakdownHeaders = new string[tableColumnCount]; // = {"QTY", "DESCRIPTION", "UNIT PRICE", "AMOUNT"};
            breakdownHeaders[0] = "QTY";
            breakdownHeaders[1] = "DESCRIPTION";
            breakdownHeaders[unitPriceColumnIndex] = "UNIT PRICE";
            breakdownHeaders[amountColumnIndex] = "AMOUNT";
            if (hasStartDateColumn)
            {
                breakdownHeaders[2] = "START DATE";
            }

            breakdownHeaders[unitPriceColumnIndex] = breakdownHeaders[unitPriceColumnIndex].FormatText(0, 1);
            breakdownHeaders[amountColumnIndex] = breakdownHeaders[amountColumnIndex].FormatText(0, 1);
            breakdownTable.AddTextStyled(SetFonts(breakdownHeaders));
            breakdownTable.FillRow("220 220 220", 0);

            var customData = quote.GetQuoteCustomData();
            var summary = _bookingDataProvider.GetSummaryData(customData);

            var currencySymbol = "$";
            var currencyPricing =
                new CurrentCurrencyPricing(quote.BookingQuoteCurrency, quote.BookingQuoteConversionRate);

            if (!quote.BookingQuoteCurrency.IsNullOrEmpty() &&
                quote.BookingQuoteCurrency != CurrencyConstants.CODE_AUD && quote.BookingQuoteConversionRate > 0)
            {
                summary.ConvertPricesToCurrentCurrency(currencyPricing);
                currencySymbol = $"{quote.BookingQuoteCurrency}";
            }

            string[] breakdownItem = new string[tableColumnCount];

            if (summary.Packages != null && summary.Packages.Count > 0)
            {
                breakdownTable.NextRow();
                abcPdfDoc.AddRowLine(breakdownTable.RowPositions[breakdownTable.RowPositions.Length - 1].Bottom.PosY);
                breakdownItem[0] = summary.Packages.Count.ToString();
                breakdownItem[1] = $"{summary.TourName} ({summary.TourCode})";
                if (!summary.SelectedTwinShareRoomsType.IsNullOrEmpty())
                {
                    for (var i = 0; i < summary.SelectedTwinShareRoomsType.Count; i++)
                    {
                        breakdownItem[0] +=
                            "<br>" + "&nbsp;".AddLineSpacing(LINE_SPACING);
                        breakdownItem[1] +=
                            "<br>" +
                            $"Room {i + 1}: {summary.SelectedTwinShareRoomsType[i]}".AddLineSpacing(LINE_SPACING);

                        breakdownItem[unitPriceColumnIndex] += "<br>" +
                                                               "&nbsp;".AddLineSpacing(LINE_SPACING);
                        breakdownItem[amountColumnIndex] += "<br>" + "&nbsp;".AddLineSpacing(LINE_SPACING);
                    }
                }

                if (hasStartDateColumn)
                {
                    breakdownItem[2] = "&nbsp;".AddLineSpacing(LINE_SPACING);
                }

                breakdownItem[unitPriceColumnIndex] =
                    $"{summary.GetPackageAdjustedUnitPrice(summary.AgentPriceDifference).FormatPrice(false, currencySymbol)}"
                        .FormatText(0, 1);
                breakdownItem[amountColumnIndex] = summary.GetPackageAdjustedTotalPrice(summary.AgentPriceDifference)
                    .FormatPrice(false, currencySymbol).FormatText(0, 1);

                AddPrePostNights(summary, breakdownItem, unitPriceColumnIndex, amountColumnIndex);

                breakdownTable.AddTextStyled(SetFonts(breakdownItem));
            }

            if (summary.SinglePackages != null && summary.SinglePackages.Count > 0)
            {
                breakdownTable.NextRow();
                abcPdfDoc.AddRowLine(breakdownTable.RowPositions[breakdownTable.RowPositions.Length - 1].Bottom.PosY);
                breakdownItem = new string[tableColumnCount];
                breakdownItem[0] = summary.SinglePackages.Count.ToString().AddLineSpacing(LINE_SPACING);
                breakdownItem[1] = $"{summary.TourName} ({summary.TourCode})".AddLineSpacing(LINE_SPACING);

                if (hasStartDateColumn)
                {
                    breakdownItem[2] = "&nbsp;".AddLineSpacing(LINE_SPACING);
                }

                if (summary.Packages != null && summary.Packages.Count > 0)
                {
                    breakdownItem[unitPriceColumnIndex] =
                        summary.GetSinglePackageAdjustedUnitPrice(0).FormatPrice(false, currencySymbol)
                            .FormatText(LINE_SPACING, 1);
                    breakdownItem[amountColumnIndex] =
                        summary.GetSinglePackageAdjustedTotalPrice(0).FormatPrice(false, currencySymbol)
                            .FormatText(LINE_SPACING, 1);
                }
                else
                {
                    breakdownItem[unitPriceColumnIndex] =
                        summary.GetSinglePackageAdjustedUnitPrice(summary.AgentPriceDifference)
                            .FormatPrice(false, currencySymbol).FormatText(LINE_SPACING, 1);
                    breakdownItem[amountColumnIndex] =
                        summary.GetSinglePackageAdjustedTotalPrice(summary.AgentPriceDifference)
                            .FormatPrice(false, currencySymbol).FormatText(LINE_SPACING, 1);
                }

                AddPrePostNights(summary, breakdownItem, unitPriceColumnIndex, amountColumnIndex);

                breakdownTable.AddTextStyled(SetFonts(breakdownItem));
            }

            if (!summary.RoomOptions.IsNullOrEmpty() && !summary.RoomOptions[0].ItemLabel.IsNullOrEmpty())
            {
                breakdownTable.NextRow();
                abcPdfDoc.AddRowLine(breakdownTable.RowPositions[breakdownTable.RowPositions.Length - 1].Bottom.PosY);

                breakdownItem = new string[tableColumnCount];
                breakdownItem[1] = "<b>Room upgrades</b>".AddLineSpacing(LINE_SPACING);
                for (var i = 0; i < summary.RoomOptions.Count; i++)
                {
                    breakdownItem[0] += "<br>" + summary.RoomOptions[i].Count.ToString().AddLineSpacing(LINE_SPACING);
                    breakdownItem[1] += "<br>" + $"{summary.RoomOptions[i].ItemLabel}".AddLineSpacing(LINE_SPACING);
                    if (hasStartDateColumn)
                    {
                        breakdownItem[2] = "<br>" + "&nbsp;".AddLineSpacing(LINE_SPACING);
                    }

                    breakdownItem[unitPriceColumnIndex] += "<br>" + summary.RoomOptions[i].UnitPriceWithPrePostNights
                        .FormatPrice(false, currencySymbol).FormatText(LINE_SPACING, 1);
                    breakdownItem[amountColumnIndex] += "<br>" + summary.RoomOptions[i].TotalPriceWithPrePostNights
                        .FormatPrice(false, currencySymbol).FormatText(LINE_SPACING, 1);
                }

                //do not consider prepost nights cost for upgrades when a main hotel is ticked, usually for 2 or more hotels in a tour
                if (!summary.HasMainHotel)
                {
                    AddPrePostNights(summary, breakdownItem, unitPriceColumnIndex, amountColumnIndex);
                }

                breakdownTable.AddTextStyled(SetFonts(breakdownItem));
            }

            if (!summary.ExtraOptions.IsNullOrEmpty())
            {
                breakdownTable.NextRow();
                abcPdfDoc.AddRowLine(breakdownTable.RowPositions[breakdownTable.RowPositions.Length - 1].Bottom.PosY);

                breakdownItem = new string[tableColumnCount];
                breakdownItem[1] = "<b>Optional extras</b>".AddLineSpacing(LINE_SPACING);
                for (var i = 0; i < summary.ExtraOptions.Count; i++)
                {
                    breakdownItem[0] += "<br>" + summary.ExtraOptions[i].Count.ToString().AddLineSpacing(LINE_SPACING);
                    breakdownItem[1] += "<br>" + $"{summary.ExtraOptions[i].ItemLabel}".AddLineSpacing(LINE_SPACING);
                    if (hasStartDateColumn)
                    {
                        breakdownItem[2] = "<br>" + "&nbsp;".AddLineSpacing(LINE_SPACING);
                    }

                    breakdownItem[unitPriceColumnIndex] += "<br>" + summary.ExtraOptions[i].UnitPrice
                        .FormatPrice(false, currencySymbol).FormatText(LINE_SPACING, 1);
                    breakdownItem[amountColumnIndex] += "<br>" + summary.ExtraOptions[i].TotalPrice
                        .FormatPrice(false, currencySymbol).FormatText(LINE_SPACING, 1);
                }

                breakdownTable.AddTextStyled(SetFonts(breakdownItem));
            }

            /*
            if (summary.EntireFlexPricing != null && summary.EntireFlexPricing.Count > 0)
            {
                breakdownTable.NextRow();
                abcPdfDoc.AddRowLine(breakdownTable.RowPositions[breakdownTable.RowPositions.Length - 1].Bottom.PosY);

                breakdownItem = new string[tableColumnCount];
               
                breakdownItem[0] += summary.EntireFlexPricing.Count.ToString();
                breakdownItem[1] += $"{summary.EntireFlexPricing.ItemLabel}";
                breakdownItem[unitPriceColumnIndex] += summary.EntireFlexPricing.UnitPrice.FormatPrice(false, currencySymbol).FormatText(0, 1);
                breakdownItem[amountColumnIndex] += summary.EntireFlexPricing.TotalPrice.FormatPrice(false, currencySymbol).FormatText(0, 1);
           
                breakdownTable.AddTextStyled(SetFonts(breakdownItem));

            }*/

            if (!additionalServices.IsNullOrEmpty())
            {
                breakdownTable.NextRow();
                abcPdfDoc.AddRowLine(breakdownTable.RowPositions[breakdownTable.RowPositions.Length - 1].Bottom.PosY);

                breakdownItem = new string[tableColumnCount];
                breakdownItem[0] = "&nbsp;".AddLineSpacing(LINE_SPACING);
                breakdownItem[1] = "<b>Additional Services</b>".AddLineSpacing(LINE_SPACING);
                breakdownItem[2] = "&nbsp;".AddLineSpacing(LINE_SPACING);
                breakdownItem[3] = "&nbsp;".AddLineSpacing(LINE_SPACING);

                if (tableColumnCount > 4)
                {
                    breakdownItem[4] = "&nbsp;".AddLineSpacing(LINE_SPACING);
                }

                breakdownTable.AddTextStyled(SetFonts(breakdownItem));

                var spaceAboveCount = 0;
                for (var i = 0; i < additionalServices.Count; i++)
                {
                    breakdownTable.NextRow();
                    breakdownItem = new string[tableColumnCount];
                    var subLabel = additionalServices[i].ItemSubLabel;

                    breakdownItem[0] += additionalServices[i].Count.ToString().AddLineSpacing(LINE_SPACING);
                    breakdownItem[unitPriceColumnIndex] += additionalServices[i].UnitPrice
                        .FormatPrice(false, currencySymbol).FormatText(LINE_SPACING, 1);
                    breakdownItem[amountColumnIndex] += additionalServices[i].TotalPrice
                        .FormatPrice(false, currencySymbol).FormatText(LINE_SPACING, 1);

                    if (additionalServices[i].ItemDate != DateTime.MinValue)
                    {
                        breakdownItem[2] += AddSpace(spaceAboveCount) +
                                            additionalServices[i].ItemDate.ToString("dd/MM/yyyy")
                                                .AddLineSpacing(LINE_SPACING);
                    }
                    else
                    {
                        breakdownItem[2] += "&nbsp;".AddLineSpacing(LINE_SPACING);
                    }

                    if (subLabel.IsNullOrEmpty())
                    {
                        breakdownItem[1] += $"{additionalServices[i].ItemLabel}".AddLineSpacing(LINE_SPACING);
                    }
                    else
                    {
                        breakdownItem[1] += $"<stylerun linespacing=\"2\">{additionalServices[i].ItemLabel}" + "<br>" +
                                            subLabel + "</stylerun>";
                    }

                    breakdownTable.AddTextStyled(SetFonts(breakdownItem));
                }
            }

            breakdownTable.NextRow();
            abcPdfDoc.AddRowLine(breakdownTable.RowPositions[breakdownTable.RowPositions.Length - 1].Bottom.PosY);

            var addedServiceTotalCost = additionalServices.Select(a => a.TotalPrice).Sum();
            if (summary.Promotion != null)
            {
                breakdownItem = new string[tableColumnCount];
                breakdownItem[0] = "&nbsp;";
                breakdownItem[1] =
                    $"<stylerun linespacing=\"2\">{summary.Promotion.PromotionName}<br>{summary.Promotion.BookByText}</stylerun>";
                if (hasStartDateColumn)
                {
                    breakdownItem[2] = "&nbsp;";
                }

                breakdownItem[unitPriceColumnIndex] = "&nbsp;";
                breakdownItem[amountColumnIndex] =
                    $"-{(summary.Discount).FormatPrice(true, currencySymbol)}".FormatText(0, 1);
                breakdownTable.AddTextStyled(SetFonts(breakdownItem));
            }

            breakdownItem = new string[tableColumnCount];
            breakdownItem[0] = "&nbsp;";
            breakdownItem[1] = "&nbsp;";
            if (hasStartDateColumn)
            {
                breakdownItem[2] = "&nbsp;";
            }

            breakdownItem[unitPriceColumnIndex] = "<b>Total</b>".FormatText(0, 1);

            breakdownItem[amountColumnIndex] =
                $"<b>{(summary.AgentSellPrice + addedServiceTotalCost).FormatPrice(false, currencySymbol)}</b>"
                    .FormatText(0, 1);

            breakdownTable.AddTextStyled(SetFonts(breakdownItem));
            var lastPosition = breakdownTable.RowPositions;
            abcPdfDoc.StampField("TextArea");

            theDoc.TextStyle.HPos = 0;
            //double paymentTermsAreaHeight = 0;

            if (quoteData.FreedomOfChoices != null && !quoteData.FreedomOfChoices.FreedomOfChoiceItems.IsNullOrEmpty())
            {
                theDoc.Rect.Position(36,
                    breakdownTable.RowPositions[breakdownTable.RowPositions.Length - 1].Bottom.PosY - 40);
                abcPdfDoc.SetPDFHTML(
                    "<b><font font-family=\"Helvetica\" font-weight=\"700\">Freedom of Choice</font></b>", 400, 20, 9);

                theDoc.Rect.String = textAreaRect.String;
                theDoc.Rect.Height = breakdownTable.RowPositions[breakdownTable.RowPositions.Length - 1].Bottom.PosY -
                                     120;

                var focTable = new PDFTable(theDoc, 2);
                focTable.CellPadding = 5;
                focTable.HorizontalAlignment = 0;
                focTable.SetColumnWidths(new double[] { 2, 18 });
                focTable.NextRow();
                string[] focTableHeaders = { "DAY", "DESCRIPTION" };

                focTable.AddTextStyled(SetFonts(focTableHeaders));
                focTable.FillRow("220 220 220", 0);

                focTable.NextRow();
                abcPdfDoc.AddRowLine(focTable.RowPositions[focTable.RowPositions.Length - 1].Bottom.PosY);

                var focItems = new string[2];
                for (var i = 0; i < quoteData.FreedomOfChoices.FreedomOfChoiceItems.Count; i++)
                {
                    if (i > 0)
                    {
                        focItems[0] += "<br>";
                        focItems[1] += "<br>";
                    }

                    focItems[0] += quoteData.FreedomOfChoices.FreedomOfChoiceItems[i].DayCaption
                        .AddLineSpacing(LINE_SPACING);
                    focItems[1] += quoteData.FreedomOfChoices.FreedomOfChoiceItems[i].OptionLabel
                        .AddLineSpacing(LINE_SPACING);
                }

                focTable.AddTextStyled(SetFonts(focItems));
                lastPosition = focTable.RowPositions;
                //PDFTable.PagePos pagePos = new PDFTable.PagePos(focTable);
                //paymentTermsAreaHeight = pagePos.PosY - 130 - ((quoteData.FreedomOfChoices.FreedomOfChoiceItems.Count - 1) * 12);
                //var rowPositions = focTable.RowPositions;
                //paymentTermsAreaHeight =  rowPositions.ElementAt(rowPositions.Length - 1).Bottom.PosY - 150;
            }

            var comment = SetParagraph(quote.BookingQuoteCustomerComments);

            var height = lastPosition.ElementAt(lastPosition.Length - 1).Bottom.PosY - 150;
            if (!comment.IsNullOrEmpty())
            {
                var commentY = 100.0;
                if (height > 110)
                {
                    commentY += (height - 110);
                    height = 110;
                }

                theDoc.Rect.Position(35, commentY);
                theDoc.Rect.Height = height;

                theDoc.Layer = theDoc.LayerCount + 1;
                theDoc.Rect.Width = theDoc.MediaBox.Width - 70;
                theDoc.Color.String = "220 220 220";
                theDoc.FillRect();
                theDoc.Layer = 1;
                theDoc.Rect.Position(45, commentY + 10);
                theDoc.Color.String = "0 0 0";
                abcPdfDoc.SetPDFHTML(
                    $"<stylerun><font font-family=\"Helvetica\" fontsize=\"8\" >Travel Agent Comments</font></stylerun><br>{comment}",
                    theDoc.MediaBox.Width - 90, height - 20, 10);
            }


            if (!excludeDepositTerms)
            {
                theDoc.TextStyle.HPos = 0;
                abcPdfDoc.SetDocPosition("BlankSpace");
                theDoc.Rect.Position(40, 100); //rowPositions.ElementAt(rowPositions.Length-1).Bottom.PosY);
                var paymentTerms =
                    GetPDFPaymentTerms(_paymentTermsProvider.GetBookingPaymentTerms(summary, tour.PaymentTerms));
                abcPdfDoc.SetPDFHTML(paymentTerms, theDoc.MediaBox.Width - 80, theDoc.MediaBox.Height - 200, 10);
                abcPdfDoc.StampField("BlankSpace");
                
                var docPage3 = new Doc();
                docPage3.Read(HttpContext.Current.Server.MapPath("~/pdf/ETG_Page2.pdf"));
                theDoc.Append(docPage3);
            }

            var docItinerary = new Doc();

            var stream =
                GetStreamFromUrl($"{SiteContext.CurrentSite.SitePresentationURL}/pdf/{quote.BookingQuoteTourCode}");
            docItinerary.Read(stream);
            theDoc.Append(docItinerary);

            var ms = new MemoryStream();
            theDoc.Flatten();
            theDoc.Save(ms);

            return ms;
        }

        private void AddPrePostNights(CartBookingSummary summary, string[] breakdownItem, int unitPriceColumnIndex,
            int amountColumnIndex)
        {
            if (summary.PreNightsDetails != null)
            {
                breakdownItem[0] += $"<br>{summary.PreNightsDetails.NumberOfNights}".AddLineSpacing(LINE_SPACING);
                breakdownItem[1] +=
                    "<br>" +
                    $"Pre Nights added {summary.PreNightsDetails.DateRangeDisplay}".AddLineSpacing(LINE_SPACING);
                breakdownItem[unitPriceColumnIndex] += "<br>" +
                                                       "&nbsp;".AddLineSpacing(LINE_SPACING);
                breakdownItem[amountColumnIndex] += "<br>" + "&nbsp;".AddLineSpacing(LINE_SPACING);
            }

            if (summary.PostNightsDetails != null)
            {
                breakdownItem[0] += $"<br>{summary.PostNightsDetails.NumberOfNights}".AddLineSpacing(LINE_SPACING);
                breakdownItem[1] +=
                    "<br>" +
                    $"Post Nights added {summary.PostNightsDetails.DateRangeDisplay}".AddLineSpacing(LINE_SPACING);
                breakdownItem[unitPriceColumnIndex] += "<br>" +
                                                       "&nbsp;".AddLineSpacing(LINE_SPACING);
                breakdownItem[amountColumnIndex] += "<br>" + "&nbsp;".AddLineSpacing(LINE_SPACING);
            }
        }

        private string AddSpace(int spaceAboveCount)
        {
            var str = new StringBuilder();
            for (var i = 0; i < spaceAboveCount; i++)
            {
                str.Append("<br>");
            }

            return str.ToString();
        }

        private Stream GetStreamFromUrl(string url)
        {
            byte[] data = null;

            using (var wc = new System.Net.WebClient())
                data = wc.DownloadData(url);

            return new MemoryStream(data);
        }

        private int GetNumberOfLogos(TourModel tour)
        {
            var count = 0;
            if (tour.HasFreedomOfChoice)
            {
                count++;
            }

            if (tour.HasPeaceOfMindGuarantee)
            {
                count++;
            }

            if (tour.TourSummaryInfo.TourIsExclusive)
            {
                count++;
            }

            if (tour.TourSummaryInfo.TourHasSafeTravel)
            {
                count++;
            }

            return count;
        }
    }
}