using Castle.Core.Internal;
using CMS.CustomTables;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Search;
using CMS.UIControls;
using CMSApp.Custom.Models;
using CommonServiceLocator;
using CsvHelper;
using ETG.Algolia.Tasks;
using ETG.Booking.Pricing.Classes.Info;
using ETG.Booking.Pricing.Classes.Providers;
using ETG.Core.CustomTables;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using ETG.Core.Search;
using ETG.Module.Booking.CustomTables;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using CMS.EventLog;
using CMSApp.Custom.ETGBooking.Uploader;
using CsvHelper.Configuration;
using ETG.Data.Destination.Models;
using ETG.Data.Destination.Services;
using ETG.Data.Tour.Repositories;

namespace CMSApp.CMSModules.ETGBooking.PageTemplates
{
    public partial class ImportBookingPrices : CMSPage
    {
        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (!upload.HasFile)
                {
                    litMessage.Text = "Please select file";
                    return;
                }

                var filePath = Server.MapPath($"~/Custom/Temp/{Guid.NewGuid()}.csv");
                upload.SaveAs(filePath);
                ProcessCSV(filePath);
                RebuildSearchIndex();
            }
            catch (Exception ex)
            {
                litMessage.Text = "Something went wrong.";
                EventLogProvider.LogException("IMPPRICE", "Import", ex);
            }
        }

        private void RebuildSearchIndex()
        {
            var searchService = ServiceLocator.Current.GetInstance<IETGSearchService>();
            
            var message = searchService.RebuildTourSearchIndex();

            if (message.IsNullOrEmpty())
            {
                EventLogProvider.LogInformation("IMPORTPRICE", "REBUILDINDEX");
            }

        }


        protected void btnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!upload.HasFile)
                {
                    litMessage.Text = "Please select file";
                    return;
                }

                var filePath = Server.MapPath($"~/Custom/Temp/{Guid.NewGuid()}.csv");
                upload.SaveAs(filePath);
                ValidateCSV(filePath);
            }
            catch (Exception ex)
            {
                litMessage.Text = "Something went wrong.";
                EventLogProvider.LogException("IMPPRICE", "Validate", ex);
            }
        }

        private void ValidateCSV(string filePath)
        {
            var message = new System.Text.StringBuilder();
            var updateDetails = new System.Text.StringBuilder();
            var reader = new System.IO.StreamReader(filePath);
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var records = csv.GetRecords<BookingPriceImportModel>().Where(a => !a.PackageCode.Contains("XXXXX")).ToList();

                var allTourCodes = records.Select(a => a.PackageCode).Distinct().ToList();

                var tourRepository = ServiceLocator.Current.GetInstance<ITourProductRepository>();
                var invalidTourCodes = tourRepository.GetInvalidTourCodes(allTourCodes);
                
                var tourAdditions = tourRepository.GetTourBookingAdditions(allTourCodes);

                var validator = new PricingCSVValidator(tourAdditions);
                var validateResult = validator.Validate(records);

                var messages = new System.Text.StringBuilder();

                if (!invalidTourCodes.IsNullOrEmpty())
                {
                    messages.AppendLine($"Invalid package codes: {string.Join(", ", invalidTourCodes)}");
                }
                if (!validateResult.Success)
                {
                    validateResult.Errors.ForEach(e => messages.AppendLine(e));
                }

                if (!invalidTourCodes.IsNullOrEmpty() || !validateResult.Success)
                {
                    litMessage.Text = messages.ToString().Replace("\n", "<br>");
                    return;
                }

                List<UploaderBookingAdditions> RoomUpgradeForUpdates;
                List<UploaderBookingAdditions> OptioanlExtrasForUpdates;

                (RoomUpgradeForUpdates, OptioanlExtrasForUpdates) = validator.GetBookingAdditionsForUpgrades();

                if (!RoomUpgradeForUpdates.IsNullOrEmpty())
                {
                    updateDetails.AppendLine("Room upgrades to update");
                    foreach (var roomUpgradeForUpdate in RoomUpgradeForUpdates)
                    {
                        var roomUpgrade = RoomUpgradeProvider.GetRoomUpgrades().OnCurrentSite()
                            .WhereEquals("NodeID", roomUpgradeForUpdate.NodeID).FirstOrDefault();

                        if (roomUpgrade != null)
                        {
                            updateDetails.AppendLine($"Tour {roomUpgradeForUpdate.TourCode}: {roomUpgrade.RoomUpgradeTitle} -> {roomUpgradeForUpdate.CSVLabel}");
                            roomUpgrade.RoomUpgradeTitle = roomUpgradeForUpdate.CSVLabel;
                            roomUpgrade.Update();
                        }
                    }
                }
                if (!OptioanlExtrasForUpdates.IsNullOrEmpty())
                {
                    foreach (var optionalExtrasForUpdate in OptioanlExtrasForUpdates)
                    {
                        var optionalExtras = OptionalExtrasProvider.GetOptionalExtras().OnCurrentSite()
                            .WhereEquals("NodeID", optionalExtrasForUpdate.NodeID).FirstOrDefault();

                        if (optionalExtras != null)
                        {
                            updateDetails.AppendLine($"Tour {optionalExtrasForUpdate.TourCode}: {optionalExtras.OptionalExtrasTitle} -> {optionalExtrasForUpdate.CSVLabel}");
                            optionalExtras.OptionalExtrasTitle = optionalExtrasForUpdate.CSVLabel;
                            optionalExtras.Update();
                        }
                    }
                }
            }

            litMessage.Text = $"CSV is valid.<br><br>{updateDetails.ToString().Replace("\n", "<br>")}";
        }

        protected void ProcessCSV(string filePath, bool deleteAll = true)
        {
            var success = false;
            using (var transactionScope = new CMSTransactionScope())
            {
                var message = new System.Text.StringBuilder();
                var reader = new System.IO.StreamReader(filePath, Encoding.GetEncoding("iso-8859-1"));
                var rowSuccessCount = 0;
                var rowFailedCount = 0;

                
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    var records = csv.GetRecords<BookingPriceImportModel>().Where(a => !a.PackageCode.Contains("XXXXX")).ToList();

                    var allTourCodes = records.Select(a => a.PackageCode).Distinct().ToList();
                    
                    var tourRepository = ServiceLocator.Current.GetInstance<ITourProductRepository>();

                    var invalidTourCodes = tourRepository.GetInvalidTourCodes(allTourCodes);
                    var tourAdditions = tourRepository.GetTourBookingAdditions(allTourCodes);

                    var validator = new PricingCSVValidator(tourAdditions);
                    var validateResult = validator.Validate(records);

                    var messages = new System.Text.StringBuilder();

                    if (!invalidTourCodes.IsNullOrEmpty())
                    {
                        messages.AppendLine($"Invalid package codes: {string.Join(", ", invalidTourCodes)}");
                    }
                    if (!validateResult.Success)
                    {
                        validateResult.Errors.ForEach(e => messages.AppendLine(e));
                    }

                    if (!invalidTourCodes.Where(a=>!a.Contains("XXXXX")).IsNullOrEmpty() || !validateResult.Success)
                    {
                        litMessage.Text = messages.ToString().Replace("\n", "<br>");
                        return;
                    }

                    var tourCodes = records.Where(a=>!a.PackageCode.IsNullOrEmpty()).Select(a => a.PackageCode).Distinct();
                    var tourCodesSql = GetTourCodesSqlINValue(tourCodes);
                    /*ConnectionHelper.ExecuteNonQuery(
                        $@"select * from ETG_BookingPriceSummary
                                    where BookingPriceSummaryTourCode IN 
                                    (SELECT TourCode From ETG_Tour
                                    where TourPrimaryCountry = '{destinationGuid}')", null, QueryTypeEnum.SQLQuery);*/
                    ConnectionHelper.ExecuteNonQuery(
                        $@"DELETE from ETG_BookingRoomOption
                                    where BookingRoomOptionBookingPriceID IN 
                                        (SELECT BookingPriceID From ETG_BookingPrice
                                            where BookingPriceTourCode IN 
                                                ({tourCodesSql})
                                        )", null, QueryTypeEnum.SQLQuery);
                    ConnectionHelper.ExecuteNonQuery(
                        $@"DELETE FROM ETG_BookingPrice 
                             where BookingPriceTourCode IN 
                                ({tourCodesSql}
                                )", null, QueryTypeEnum.SQLQuery);
                    

                    var rowNumber = 1;
                    foreach (var record in records)
                    {
                        rowNumber++;

                        if (record.PackageCode.IsNullOrEmpty())
                        {
                            continue;
                        }
                        try
                        {
                            bool recordIsNew = deleteAll;

                            var startDate = UploaderHelpers.GetDateTime(record.StartDate);
                            var endDate = UploaderHelpers.GetDateTime(record.EndDate);

                            BookingPriceInfo bookingPrice = null;

                            if (!deleteAll)
                            {
                                bookingPrice = BookingPriceInfoProvider.GetBookingPrices()
                                    .WhereEquals(nameof(BookingPriceInfo.BookingPriceTourCode), record.PackageCode)
                                    .WhereEquals(nameof(BookingPriceInfo.BookingPriceStartDate), startDate)
                                    .WhereEquals(nameof(BookingPriceInfo.BookingPriceEndDate), endDate)
                                    .FirstOrDefault();
                            }

                            if (bookingPrice == null)
                            {
                                recordIsNew = true;
                                bookingPrice = new BookingPriceInfo();
                                bookingPrice.BookingPriceGuid = Guid.NewGuid();
                            }

                            bookingPrice.BookingPriceLastModified = DateTime.Now;
                            bookingPrice.BookingPriceTourCode = record.PackageCode;
                            bookingPrice.BookingPriceStartDate = startDate;
                            bookingPrice.BookingPriceEndDate = endDate;
                            bookingPrice.BookingPriceTwinSharePrice = ValidationHelper.GetDouble(record.PriceTwinShare, 0);
                            bookingPrice.BookingPriceSingleSupplementalCost =
                                ValidationHelper.GetDouble(record.SingleSupplementCost, 0);
                            bookingPrice.BookingPricePreNightTwinPrice = ValidationHelper.GetDouble(record.PreNightTN, 0);
                            bookingPrice.BookingPricePostNightTwinPrice = ValidationHelper.GetDouble(record.PostNightTN, 0);
                            bookingPrice.BookingPricePreNightSinglePrice = ValidationHelper.GetDouble(record.PreNightSG, 0);
                            bookingPrice.BookingPricePostNightSinglePrice = ValidationHelper.GetDouble(record.PostNightSG, 0);
                            bookingPrice.BalanceDueDays = ValidationHelper.GetInteger(record.BalanceDueDate, 0);
                            bookingPrice.SecondInstalmentDays = ValidationHelper.GetInteger(record.SecondInstalmentDays, 0);
                            bookingPrice.SecondInstalmentPercent = ValidationHelper.GetDouble(record.SecondInstalmentPercent.IsNullOrEmpty() ? "0" : record.SecondInstalmentPercent.Replace("%", "") , 0);

                            if (recordIsNew)
                            {
                                bookingPrice.Insert();
                            }
                            else
                            {
                                bookingPrice.Update();

                                ConnectionHelper.ExecuteNonQuery(
                                    $"DELETE FROM ETG_BookingRoomOption WHERE BookingRoomOptionBookingPriceID={bookingPrice.BookingPriceID}", null, QueryTypeEnum.SQLQuery);
                            }

                            for (var i = 1; i <= 5; i++)
                            {
                                Type recordType = record.GetType();

                                var labelValue =
                                    ValidationHelper.GetString(recordType.GetProperty($"Twin{i}Label")?.GetValue(record),
                                        string.Empty).Trim();
                                double ppValue =
                                    ValidationHelper.GetDouble(recordType.GetProperty($"Twin{i}PP")?.GetValue(record), 0);

                                if (!labelValue.IsNullOrEmpty() && !labelValue.Equals("0") && ppValue >0)
                                {
                                    var optionInfo = new BookingRoomOptionInfo
                                    {
                                        BookingRoomOptionGuid = Guid.NewGuid(),
                                        BookingRoomOptionLastModified = DateTime.Now,
                                        BookingRoomOptionLabel = labelValue,
                                        BookingRoomOptionPricePerPerson = ppValue,
                                        BookingRoomOptionBookingPriceID = bookingPrice.BookingPriceID,
                                        BookingRoomOptionTwinPreNightPrice = ValidationHelper.GetDouble(recordType.GetProperty($"PreNightRU{i}")?.GetValue(record), 0),
                                        BookingRoomOptionTwinPostNightPrice = ValidationHelper.GetDouble(recordType.GetProperty($"PostNightRU{i}")?.GetValue(record), 0),
                                        BookingRoomOptionOrder = i,
                                        BookingRoomOptionType = 1
                                    };

                                    optionInfo.Insert();
                                }

                            }

                            for (var i = 1; i <= 5; i++)
                            {
                                Type recordType = record.GetType();

                                var labelValue =
                                    ValidationHelper.GetString(recordType.GetProperty($"Single{i}Label")?.GetValue(record),
                                        string.Empty).Trim();
                                double ppValue =
                                    ValidationHelper.GetDouble(recordType.GetProperty($"Single{i}PP")?.GetValue(record), 0);

                                if (!labelValue.IsNullOrEmpty() && !labelValue.Equals("0") && ppValue > 0)
                                {
                                    var optionInfo = new BookingRoomOptionInfo
                                    {
                                        BookingRoomOptionGuid = Guid.NewGuid(),
                                        BookingRoomOptionLastModified = DateTime.Now,
                                        BookingRoomOptionLabel = labelValue,
                                        BookingRoomOptionPricePerPerson = ppValue,
                                        BookingRoomOptionBookingPriceID = bookingPrice.BookingPriceID,
                                        BookingRoomOptionOrder = i,
                                        BookingRoomOptionType = 2
                                    };

                                    optionInfo.Insert();
                                }

                            }

                            for (var i = 1; i <= 8; i++)
                            {
                                Type recordType = record.GetType();

                                var labelValue =
                                    ValidationHelper.GetString(recordType.GetProperty($"Extra{i}Label")?.GetValue(record),
                                        string.Empty).Trim();
                                double ppValue =
                                    ValidationHelper.GetDouble(recordType.GetProperty($"Extra{i}PP")?.GetValue(record), 0);

                                var labelSoloValue =
                                    ValidationHelper.GetString(recordType.GetProperty($"Extra{i}LabelSolo")?.GetValue(record),
                                        string.Empty).Trim();
                                double ppSoloValue =
                                    ValidationHelper.GetDouble(recordType.GetProperty($"Extra{i}Solo")?.GetValue(record), 0);
                                
                                if (!labelValue.IsNullOrEmpty() && !labelValue.Equals("0") && ppValue > 0)
                                {
                                    var optionInfo = new BookingRoomOptionInfo
                                    {
                                        BookingRoomOptionGuid = Guid.NewGuid(),
                                        BookingRoomOptionLastModified = DateTime.Now,
                                        BookingRoomOptionLabel = labelValue,
                                        BookingRoomOptionPricePerPerson = ppValue,
                                        BookingRoomOptionBookingPriceID = bookingPrice.BookingPriceID,
                                        BookingRoomOptionOrder = i,
                                        BookingRoomOptionType = 3
                                    };

                                    if (!labelSoloValue.IsNullOrEmpty() && !labelSoloValue.Equals("0") &&
                                        ppSoloValue > 0)
                                    {
                                        optionInfo.BookingRoomOptionLabel2 = labelSoloValue;
                                        optionInfo.BookingRoomOptionPricePerPerson2 = ppSoloValue;
                                    }

                                    optionInfo.Insert();
                                }
                            }

                            rowSuccessCount++;

                        }
                        catch (Exception ex)
                        {
                            rowFailedCount++;
                            message.AppendLine($"<br>Row {rowNumber}: Error {ex.Message}");
                            var logItem = new BookingPricingImportLogItem
                            {
                                ItemCreatedWhen = DateTime.Now,
                                LogData = JsonConvert.SerializeObject(record),
                                LogDetails = ex.Message + ex.StackTrace,
                                LogStatus = "Error"
                            };
                            logItem.Insert();

                        }
                    }

                    if (rowFailedCount == 0)
                    {
                        List<UploaderBookingAdditions> RoomUpgradeForUpdates;
                        List<UploaderBookingAdditions> OptioanlExtrasForUpdates;

                        (RoomUpgradeForUpdates, OptioanlExtrasForUpdates) = validator.GetBookingAdditionsForUpgrades();

                        if (!RoomUpgradeForUpdates.IsNullOrEmpty())
                        {
                            foreach (var roomUpgradeForUpdate in RoomUpgradeForUpdates)
                            {
                                var roomUpgrade = RoomUpgradeProvider.GetRoomUpgrades().OnCurrentSite()
                                    .WhereEquals("NodeID", roomUpgradeForUpdate.NodeID).FirstOrDefault();

                                if (roomUpgrade != null)
                                {
                                    roomUpgrade.RoomUpgradeTitle = roomUpgradeForUpdate.CSVLabel;
                                    roomUpgrade.Update();
                                }
                            }
                        }
                        if (!OptioanlExtrasForUpdates.IsNullOrEmpty())
                        {
                            foreach (var optionalExtrasForUpdate in OptioanlExtrasForUpdates)
                            {
                                var optionalExtras = OptionalExtrasProvider.GetOptionalExtras().OnCurrentSite()
                                    .WhereEquals("NodeID", optionalExtrasForUpdate.NodeID).FirstOrDefault();

                                if (optionalExtras != null)
                                {
                                    optionalExtras.OptionalExtrasTitle = optionalExtrasForUpdate.CSVLabel;
                                    optionalExtras.Update();
                                }
                            }
                        }
                        transactionScope.Commit();

                        var tours = CustomTableItemProvider.GetItems<TourAlgoliaSyncQueueItem>().ToList();

                        if (!tours.IsNullOrEmpty())
                        {
                            var etgSearchTaskCreator = ServiceLocator.Current.GetInstance<IETGSearchTaskCreator>();


                            foreach (var t in tours)
                            {
                                var tour = TourProvider.GetTours().OnCurrentSite().WhereEquals(nameof(Tour.TourCode), t.TourCode).FirstOrDefault();
                                etgSearchTaskCreator.CreateSearchTask(tour, SearchTaskTypeEnum.Update);
                            }
                        }
                        ConnectionHelper.ExecuteNonQuery(
                            $"DELETE FROM ETG_TourAlgoliaSyncQueue", null, QueryTypeEnum.SQLQuery);
                        message.AppendLine("<br>Import was successful.");
                        message.AppendLine("<br><br>Processed Packages<br>");
                        message.AppendLine($"{string.Join("<br>",records.Where(a=>!a.PackageCode.IsNullOrEmpty()).Select(a=> $"{a.PackageCode}").Distinct().ToList())}");
                        success = true;
                    }
                    else
                    {
                        message.AppendLine($"<br>Error detected. No pricing were updated. Please review CSV file.");
                    }
                }

                if (success)
                {
                    pnlMessage.Attributes.Add("style", "clear: both");
                }
                else
                {
                    pnlMessage.Attributes.Add("style", "clear: both;color: red");
                }

                litMessage.Text = message.ToString();
            }
            
        }

        private string GetTourCodesSqlINValue(IEnumerable<string> tourCodes)
        {

            var codes = tourCodes.Select(a => $"'{a}'");

            return string.Join(",", codes);

        }
    }
}